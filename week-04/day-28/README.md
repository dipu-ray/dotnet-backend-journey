# Fixing StockBalance and CurrentStock Mismatches After Transfer and Stock Adjustment

**Date:** 5 November 2026
**Database:** SQL Server

> **Important:** this guide was written **without access to the actual data**. It gives a safe method to **find the cause** and to **fix** the mismatches, but the queries that depend on how the application posts transfers and adjustments are marked as **assumptions**. Confirm them first (see [Assumptions to Confirm](#9-assumptions-to-confirm)). A data fix must be run **inside a transaction, after a backup, and with the senior's approval**.

## Table of Contents

- [1. Problem Statement](#1-problem-statement)
- [2. Understanding the Rule](#2-understanding-the-rule)
- [3. Approach](#3-approach)
- [4. Step 1: Backup and List the Mismatches](#4-step-1-backup-and-list-the-mismatches)
- [5. Step 2: Find the Cause](#5-step-2-find-the-cause)
- [6. Step 3: Choose the Source of Truth](#6-step-3-choose-the-source-of-truth)
- [7. Step 4: Fix the Mismatches](#7-step-4-fix-the-mismatches)
- [8. Step 5: Verify and Prevent Recurrence](#8-step-5-verify-and-prevent-recurrence)
- [9. Assumptions to Confirm](#9-assumptions-to-confirm)
- [10. Summary](#10-summary)

---

## 1. Problem Statement

The database contains these tables:

```sql
select * from Projects;
select * from Stock;
select * from CurrentStock;
select * from Transfers;
select * from TransferDetails;
select * from StockAdjustment;
select * from StockAdjustmentDetail;
```

After a **Transfer** or a **Stock Adjustment**, some items have a `StockBalance` (calculated from the `Stock` table) that **does not match** `CurrentStock` (or `CurrentStock` was not set to `0` when it should be).

**Task:** find those items and **fix the mismatch so that `Difference = StockBalance - CurrentStock` becomes `0`**.

---

## 2. Understanding the Rule

| Table          | Role                                                    |
| -------------- | ------------------------------------------------------- |
| `Stock`        | The **ledger**: every movement (`IN` / `OUT`) is a row  |
| `CurrentStock` | The **summary**: the quantity now, per project and item |

**The rule that must always be true, for each project and item:**

```
CurrentStock  =  SUM(IN quantity)  -  SUM(OUT quantity)      (from Stock, ignoring deleted rows)
```

**What a Transfer and a Stock Adjustment should do**

| Operation                             | Expected effect on `Stock`                                                 | Expected effect on `CurrentStock`                |
| ------------------------------------- | -------------------------------------------------------------------------- | ------------------------------------------------ |
| **Transfer** (project A to project B) | `OUT` row in project A and `IN` row in project B, referencing the transfer | Project A **decreases**, project B **increases** |
| **Stock Adjustment** (increase)       | `IN` row referencing the adjustment                                        | **Increases**                                    |
| **Stock Adjustment** (decrease)       | `OUT` row referencing the adjustment                                       | **Decreases**                                    |

A mismatch appears when **one side was updated and the other was not**, for example a `Stock` row was created but `CurrentStock` was not updated, or the other way around.

---

## 3. Approach

| Step | Action                                                                                |
| ---- | ------------------------------------------------------------------------------------- |
| 1    | **Back up** the data and **list** every mismatch (with its difference)                |
| 2    | **Find the cause** using the transfers, adjustments, and deleted rows                 |
| 3    | **Decide the source of truth**: `Stock` or `CurrentStock`                             |
| 4    | **Fix** inside a transaction (dry run first)                                          |
| 5    | **Verify** that the difference is `0`, and find why it happened so it does not repeat |

---

## 4. Step 1: Backup and List the Mismatches

### 4.1 Backup

```sql
BACKUP DATABASE YourDatabaseName
TO DISK = 'C:\Backup\YourDatabaseName_before_stock_fix.bak'
WITH INIT, STATS = 10;
```

### 4.2 List the mismatches into a temporary table

This lists every project and item where the stock history and the current stock differ, **limited to items that have transfer or adjustment rows**.

```sql
IF OBJECT_ID('tempdb..#Mismatch') IS NOT NULL DROP TABLE #Mismatch;

;WITH s AS (
    SELECT
        ProjectId,
        ItemId,
        SUM(CASE
                WHEN UPPER(StockType) = 'IN'  THEN  ISNULL(Quantity, 0)
                WHEN UPPER(StockType) = 'OUT' THEN -ISNULL(Quantity, 0)
                ELSE 0
            END) AS StockBalance
    FROM Stock
    WHERE ISNULL(IsDelete, 0) = 0
    GROUP BY ProjectId, ItemId
),
c AS (
    SELECT
        ProjectId,
        ItemId,
        SUM(ISNULL(Quantity, 0)) AS CurrentStock
    FROM CurrentStock
    GROUP BY ProjectId, ItemId
)
SELECT
    COALESCE(s.ProjectId, c.ProjectId)  AS ProjectId,
    COALESCE(s.ItemId, c.ItemId)        AS ItemId,
    ISNULL(s.StockBalance, 0)           AS StockBalance,
    ISNULL(c.CurrentStock, 0)           AS CurrentStock,
    ISNULL(s.StockBalance, 0) - ISNULL(c.CurrentStock, 0) AS Difference
INTO #Mismatch
FROM s
FULL JOIN c
    ON s.ProjectId = c.ProjectId
   AND s.ItemId    = c.ItemId
WHERE ISNULL(s.StockBalance, 0) <> ISNULL(c.CurrentStock, 0)
  AND EXISTS (
        SELECT 1
        FROM Stock st
        WHERE st.ItemId    = COALESCE(s.ItemId, c.ItemId)
          AND st.ProjectId = COALESCE(s.ProjectId, c.ProjectId)
          AND st.ReferenceTable IN ('Transfers', 'StockAdjustment')
          AND ISNULL(st.IsDelete, 0) = 0
  );

SELECT m.*, p.Name AS ProjectName
FROM #Mismatch m
LEFT JOIN Projects p ON p.Id = m.ProjectId
ORDER BY p.Name, m.ItemId;
```

| Column         | Meaning                       |
| -------------- | ----------------------------- |
| `StockBalance` | In minus out, from `Stock`    |
| `CurrentStock` | Quantity in `CurrentStock`    |
| `Difference`   | `StockBalance - CurrentStock` |

### 4.3 Checks before going further

**Several `CurrentStock` rows for the same project and item?** (A simple update would then be wrong.)

```sql
SELECT ProjectId, ItemId, COUNT(*) AS TotalRows
FROM CurrentStock
GROUP BY ProjectId, ItemId
HAVING COUNT(*) > 1;
```

**Negative balances?** (These usually point to wrong data in `Stock`.)

```sql
SELECT * FROM #Mismatch WHERE StockBalance < 0 OR CurrentStock < 0;
```

---

## 5. Step 2: Find the Cause

### 5.1 Read the pattern of the difference

| Pattern                                                             | Likely cause                                                                                                                                             |
| ------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CurrentStock` **higher** than `StockBalance` (Difference negative) | An `OUT` row is missing or was deleted in `Stock`, or `CurrentStock` did not decrease after a transfer out or an adjustment decrease                     |
| `CurrentStock` **lower** than `StockBalance` (Difference positive)  | An `IN` row is missing from `CurrentStock`, for example a transfer was received or an adjustment increase was saved, but `CurrentStock` did not increase |
| `StockBalance = 0` but `CurrentStock > 0`                           | Current stock exists with **no history** in `Stock`                                                                                                      |
| `CurrentStock = 0` (or no row) but `StockBalance <> 0`              | The `CurrentStock` row is missing or was reset                                                                                                           |

### 5.2 Transfers that do not match the stock rows

> **Assumption:** a transfer creates an `OUT` row in the **from** project and an `IN` row in the **to** project, with `Stock.ReferenceTable = 'Transfers'` and `Stock.ReferenceId = Transfers.Id`.

```sql
SELECT
    t.Id            AS TransferId,
    t.TransferNo,
    t.TransferDate,
    t.Status,
    t.FromProjectId,
    t.ToProjectId,
    td.ItemId,
    td.TransferQuantity,
    td.ReceivedQuantity,
    ISNULL(o.OutQty, 0) AS StockOutQty,
    ISNULL(i.InQty, 0)  AS StockInQty
FROM Transfers t
INNER JOIN TransferDetails td ON td.TransferId = t.Id
OUTER APPLY (
    SELECT SUM(st.Quantity) AS OutQty
    FROM Stock st
    WHERE st.ReferenceTable = 'Transfers'
      AND st.ReferenceId    = t.Id
      AND st.ItemId         = td.ItemId
      AND st.ProjectId      = t.FromProjectId
      AND UPPER(st.StockType) = 'OUT'
      AND ISNULL(st.IsDelete, 0) = 0
) o
OUTER APPLY (
    SELECT SUM(st.Quantity) AS InQty
    FROM Stock st
    WHERE st.ReferenceTable = 'Transfers'
      AND st.ReferenceId    = t.Id
      AND st.ItemId         = td.ItemId
      AND st.ProjectId      = t.ToProjectId
      AND UPPER(st.StockType) = 'IN'
      AND ISNULL(st.IsDelete, 0) = 0
) i
WHERE (ISNULL(o.OutQty, 0) <> td.TransferQuantity
       OR ISNULL(i.InQty, 0) <> ISNULL(td.ReceivedQuantity, 0))
  AND EXISTS (
        SELECT 1 FROM #Mismatch m
        WHERE m.ItemId = td.ItemId
          AND m.ProjectId IN (t.FromProjectId, t.ToProjectId)
  )
ORDER BY t.Id, td.ItemId;
```

| What the result means             | Meaning                                                                           |
| --------------------------------- | --------------------------------------------------------------------------------- |
| `StockOutQty <> TransferQuantity` | The transfer out is missing from `Stock`, or is different                         |
| `StockInQty <> ReceivedQuantity`  | The receipt is missing from `Stock`, or is different (it may not be received yet) |

### 5.3 Adjustments that do not match the stock rows

> **Assumption:** an adjustment creates a `Stock` row with `ReferenceTable = 'StockAdjustment'` and `ReferenceId = StockAdjustment.Id`. If the application stores the **detail** ID instead, change the join accordingly.

```sql
SELECT
    sa.Id            AS AdjustmentId,
    sa.AdjustmentDate,
    sa.AdjustmentType,
    sad.ItemId,
    sad.Quantity     AS AdjustedQuantity,
    ISNULL(x.StockQty, 0) AS StockQuantity
FROM StockAdjustment sa
INNER JOIN StockAdjustmentDetail sad ON sad.StockAdjustmentId = sa.Id
OUTER APPLY (
    SELECT SUM(st.Quantity) AS StockQty
    FROM Stock st
    WHERE st.ReferenceTable = 'StockAdjustment'
      AND st.ReferenceId    = sa.Id
      AND st.ItemId         = sad.ItemId
      AND ISNULL(st.IsDelete, 0) = 0
) x
WHERE ISNULL(x.StockQty, 0) <> sad.Quantity
  AND EXISTS (SELECT 1 FROM #Mismatch m WHERE m.ItemId = sad.ItemId)
ORDER BY sa.Id;
```

`StockQuantity = 0` means the adjustment never created a `Stock` row.

### 5.4 Deleted stock rows for the mismatched items

```sql
SELECT st.*
FROM Stock st
INNER JOIN #Mismatch m
    ON m.ItemId = st.ItemId
   AND m.ProjectId = st.ProjectId
WHERE ISNULL(st.IsDelete, 0) = 1
  AND st.ReferenceTable IN ('Transfers', 'StockAdjustment');
```

A deleted `Stock` row whose effect is still present in `CurrentStock` explains a mismatch.

### 5.5 Duplicate stock rows

```sql
SELECT ItemId, ProjectId, ReferenceTable, ReferenceId, StockType, COUNT(*) AS TotalRows
FROM Stock
WHERE ISNULL(IsDelete, 0) = 0
  AND ReferenceTable IN ('Transfers', 'StockAdjustment')
GROUP BY ItemId, ProjectId, ReferenceTable, ReferenceId, StockType
HAVING COUNT(*) > 1;
```

The same document posted twice doubles its effect on `StockBalance`.

---

## 6. Step 3: Choose the Source of Truth

The fix depends on **which side is correct**.

| Option                            | Use when                                                                                                                                                          | Action                                                                                                                  |
| --------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------- |
| **A. `Stock` is correct**         | The `Stock` rows match the real transfers and adjustments (the usual case, because `Stock` is the ledger and reports such as `Inv_StockReport` calculate from it) | Set `CurrentStock` to the `StockBalance`                                                                                |
| **B. `CurrentStock` is correct**  | The physical count or the quantity in the application is right, and `Stock` is missing movements                                                                  | Add the missing movements to `Stock` (for example through a **stock adjustment**) so the history reaches `CurrentStock` |
| **C. A specific record is wrong** | Step 2 found one wrong record (a wrongly deleted row, a duplicate, a missing receipt)                                                                             | Correct that record, then re-check the difference                                                                       |

> **Decide with the senior before changing data.** If the cause is a wrongly deleted or duplicated `Stock` row, fix that row first (Option C), then check what is still different.

---

## 7. Step 4: Fix the Mismatches

### Option A: Set `CurrentStock` to the `StockBalance`

Run in one batch, with the `#Mismatch` table from Step 1. It starts as a **dry run** (`ROLLBACK`). Change it to `COMMIT` only after checking the result.

```sql
BEGIN TRAN;

-- 1. Keep a copy of the rows that will change
SELECT cs.*
INTO CurrentStock_Backup_20261105
FROM CurrentStock cs
INNER JOIN #Mismatch m
    ON cs.ProjectId = m.ProjectId
   AND cs.ItemId    = m.ItemId;

-- 2. Update existing CurrentStock rows (only where there is exactly one row for the project and item)
UPDATE cs
SET cs.Quantity = m.StockBalance
FROM CurrentStock cs
INNER JOIN #Mismatch m
    ON cs.ProjectId = m.ProjectId
   AND cs.ItemId    = m.ItemId
WHERE (SELECT COUNT(*)
       FROM CurrentStock x
       WHERE x.ProjectId = cs.ProjectId
         AND x.ItemId    = cs.ItemId) = 1;

PRINT 'Rows updated: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

-- 3. Create CurrentStock rows that do not exist yet
--    (add every column that the table requires, for example company or item type)
INSERT INTO CurrentStock (ProjectId, ItemId, Quantity /*, other required columns */)
SELECT m.ProjectId, m.ItemId, m.StockBalance
FROM #Mismatch m
WHERE NOT EXISTS (
    SELECT 1
    FROM CurrentStock cs
    WHERE cs.ProjectId = m.ProjectId
      AND cs.ItemId    = m.ItemId
);

-- 4. Check: this should return no rows
SELECT m.ProjectId, m.ItemId, m.StockBalance, cs.Quantity AS CurrentStock
FROM #Mismatch m
LEFT JOIN (
    SELECT ProjectId, ItemId, SUM(ISNULL(Quantity, 0)) AS Quantity
    FROM CurrentStock
    GROUP BY ProjectId, ItemId
) cs ON cs.ProjectId = m.ProjectId AND cs.ItemId = m.ItemId
WHERE ISNULL(cs.Quantity, 0) <> m.StockBalance;

ROLLBACK TRAN;      -- dry run. Change to COMMIT TRAN after the check above returns no rows.
```

**Notes**

- If `CurrentStock` has **several rows** for the same project and item (see the check in 4.3), the update skips them. Handle those manually, because setting each row to the full balance would double-count.
- Rows where `StockBalance = 0` are updated to `0`, which also covers the case where `CurrentStock` should have been `0`.
- If the `INSERT` fails because of a required column, add that column to the column list and the `SELECT`.

### Option B: Add the missing movements to `Stock`

When `CurrentStock` is the correct quantity:

- Create a **stock adjustment through the application** for the `Difference`, so that `Stock`, `StockAdjustment`, and `CurrentStock` are all updated by the normal process.
- Avoid inserting directly into `Stock`, because the required columns (item type, unit price, reference columns, project and sub project) and the update of `CurrentStock` must follow the application's rules.

| Difference                                  | Adjustment to make                                          |
| ------------------------------------------- | ----------------------------------------------------------- |
| `Difference < 0` (`CurrentStock` is higher) | Increase the stock history by the amount (`IN` adjustment)  |
| `Difference > 0` (`CurrentStock` is lower)  | Decrease the stock history by the amount (`OUT` adjustment) |

### Option C: Fix a specific wrong record

| Finding                                  | Fix                                                                             |
| ---------------------------------------- | ------------------------------------------------------------------------------- |
| Stock row deleted by mistake             | Restore it (`IsDelete = 0`), after confirming with the senior                   |
| Duplicate stock row                      | Mark one of the duplicates as deleted (`IsDelete = 1`)                          |
| Transfer or adjustment with no stock row | Create the missing row through the application's process or a controlled script |
| Receipt not posted                       | Complete the receive process for that transfer                                  |

After each correction, **re-run the Step 1 query** to see what difference remains.

---

## 8. Step 5: Verify and Prevent Recurrence

### 8.1 Verify

Run the Step 1 mismatch query again (without `INTO`). It should return **no rows**, meaning `Difference = 0` for every item.

```sql
SELECT
    COALESCE(s.ProjectId, c.ProjectId) AS ProjectId,
    COALESCE(s.ItemId, c.ItemId)       AS ItemId,
    ISNULL(s.StockBalance, 0)          AS StockBalance,
    ISNULL(c.CurrentStock, 0)          AS CurrentStock
FROM (
    SELECT ProjectId, ItemId,
           SUM(CASE WHEN UPPER(StockType) = 'IN'  THEN  ISNULL(Quantity, 0)
                    WHEN UPPER(StockType) = 'OUT' THEN -ISNULL(Quantity, 0)
                    ELSE 0 END) AS StockBalance
    FROM Stock
    WHERE ISNULL(IsDelete, 0) = 0
    GROUP BY ProjectId, ItemId
) s
FULL JOIN (
    SELECT ProjectId, ItemId, SUM(ISNULL(Quantity, 0)) AS CurrentStock
    FROM CurrentStock
    GROUP BY ProjectId, ItemId
) c ON s.ProjectId = c.ProjectId AND s.ItemId = c.ItemId
WHERE ISNULL(s.StockBalance, 0) <> ISNULL(c.CurrentStock, 0);
```

Also check a few items in the application (stock report) to confirm that the numbers look right.

### 8.2 Find why it happened

A fix without finding the cause lets the problem return. Find the code that performs transfers and adjustments and check that it updates **both** `Stock` and `CurrentStock`.

```sql
-- Stored procedures that touch CurrentStock
SELECT OBJECT_NAME(object_id) AS ProcedureName
FROM sys.sql_modules
WHERE definition LIKE '%CurrentStock%';

-- Read the source of a procedure
EXEC sp_helptext '[dbo].[ProcedureName]';
```

**Points to check in the procedures and the application**

| Check           | Question                                                                                                                  |
| --------------- | ------------------------------------------------------------------------------------------------------------------------- |
| Transfer        | Are the `OUT` row (from project) and `IN` row (to project) both created, and is `CurrentStock` updated for both projects? |
| Receive         | Is the `IN` row and the `CurrentStock` increase created when the transfer is received?                                    |
| Adjustment      | Does it create a `Stock` row **and** update `CurrentStock`, for both increases and decreases?                             |
| Delete / cancel | Does deleting or cancelling a document reverse its `CurrentStock` effect?                                                 |
| Transactions    | Are both updates in one transaction, so one cannot succeed without the other?                                             |
| Direct edits    | Was data changed directly in the database?                                                                                |

Report the finding to the senior, so the root cause can be corrected in the code.

---

## 9. Assumptions to Confirm

| #   | Assumption                                                                                                                                            | How to confirm                                                         |
| --- | ----------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| 1   | `Quantity` in `Stock` is positive for both `IN` and `OUT`, and `StockType` gives the direction                                                        | `select top 20 StockType, Quantity from Stock;`                        |
| 2   | `CurrentStock` should equal In minus Out of non-deleted `Stock` rows, per project and item                                                            | Ask the senior                                                         |
| 3   | Transfers create `OUT` (from project) and `IN` (to project) rows with `ReferenceTable = 'Transfers'` and `ReferenceId = Transfers.Id`                 | `select top 20 * from Stock where ReferenceTable = 'Transfers';`       |
| 4   | Adjustments create rows with `ReferenceTable = 'StockAdjustment'`, and `ReferenceId` is the **header** ID                                             | `select top 20 * from Stock where ReferenceTable = 'StockAdjustment';` |
| 5   | `CurrentStock` has one row per project and item                                                                                                       | Check in 4.3                                                           |
| 6   | Only the columns listed are required to insert a `CurrentStock` row                                                                                   | `EXEC sp_help 'CurrentStock';`                                         |
| 7   | `Stock` is the source of truth (Option A)                                                                                                             | Ask the senior                                                         |
| 8   | All item types should be compared (the `Inv_StockReport` procedure counts only `ItemType = 'Good'`; add that condition if the same rule applies here) | Ask the senior                                                         |

---

## 10. Summary

- **Rule:** for every project and item, `CurrentStock` must equal the **In minus Out** of the non-deleted `Stock` rows. `Difference = StockBalance - CurrentStock` must be `0`.
- **Mismatches** after a transfer or an adjustment mean that one table was updated and the other was not, or a row was deleted, duplicated, or never created.
- **Method:** back up, list the mismatches into `#Mismatch`, find the cause (transfers, adjustments, deleted rows, duplicates), decide the source of truth, fix inside a transaction (dry run first), then verify that the mismatch query returns **no rows**.
- **Usual fix (Option A):** set `CurrentStock` to the `StockBalance` for the mismatched items, after checking for duplicate `CurrentStock` rows.
- **If `CurrentStock` is the correct quantity (Option B):** create a stock adjustment through the application.
- **Prevent recurrence:** check that the transfer, receive, adjustment, and delete processes update `Stock` and `CurrentStock` **together in one transaction**.
