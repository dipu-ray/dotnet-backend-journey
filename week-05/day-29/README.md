# Fixing StockBalance and CurrentStock Mismatches: Day 2

**Date:** 6 November 2026
**Database:** SQL Server

> **Note:** this is the **second-day** plan for the mismatch problem between the `Stock` table (the ledger) and the `CurrentStock` table. It continues the first-day document (*Fixing StockBalance and CurrentStock Mismatches After Transfer and Stock Adjustment*). I have not seen the data or the result of the first day, so the sections marked `[ ... ]` are for **your findings**. The queries are written to **find the cause with evidence**, and any data change must be done after a **backup**, inside a **transaction**, and with the senior's approval.

## Table of Contents
- [1. Day 1 Recap](#1-day-1-recap)
- [2. Day 2 Goal and Plan](#2-day-2-goal-and-plan)
- [3. Step 1: Rebuild the Mismatch List](#3-step-1-rebuild-the-mismatch-list)
- [4. Step 2: Trace One Case from Start to End](#4-step-2-trace-one-case-from-start-to-end)
- [5. Step 3: Classify All Mismatches by Cause](#5-step-3-classify-all-mismatches-by-cause)
- [6. Step 4: Fix by Category](#6-step-4-fix-by-category)
- [7. Step 5: Verify](#7-step-5-verify)
- [8. Day 2 Progress Log](#8-day-2-progress-log)
- [9. If Still Stuck](#9-if-still-stuck)
- [10. Summary](#10-summary)

---

## 1. Day 1 Recap

**Problem:** after a **Transfer** or a **Stock Adjustment**, some items have a `StockBalance` (In minus Out from `Stock`) that does not equal `CurrentStock`. The task is to find them and make `Difference = StockBalance - CurrentStock` equal to `0`.

**The rule**
```
CurrentStock  =  SUM(IN)  -  SUM(OUT)     per project and item, from non-deleted Stock rows
```

| Day 1 | Result |
|-------|--------|
| Tables studied | `Projects`, `Stock`, `CurrentStock`, `Transfers`, `TransferDetails`, `StockAdjustment`, `StockAdjustmentDetail` |
| Mismatch query written | Stock summary and current stock summary joined with a `FULL JOIN`, with transfer projects (`OUTER APPLY`) and a stock adjustment check |
| What worked | [Fill in] |
| What did not work / where I got stuck | [Fill in] |

---

## 2. Day 2 Goal and Plan

**Goal:** understand **why** the mismatches happen, then fix them **by cause** instead of one by one.

| Step | Action | Result |
|------|--------|--------|
| 1 | Rebuild the list of mismatches (`#Mismatch`) | Every project and item with a difference |
| 2 | **Trace one small case** from start to end | The exact missing, duplicate, or deleted record |
| 3 | **Classify** all mismatches by likely cause | A count per cause |
| 4 | **Fix each category** with the right action | Differences become `0` |
| 5 | **Verify** that no mismatch remains | Empty mismatch list |

The key idea of Day 2: **do not try to fix everything at once.** Understand **one case completely**, then use what it shows to handle the rest.

---

## 3. Step 1: Rebuild the Mismatch List

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
    COALESCE(s.ProjectId, c.ProjectId) AS ProjectId,
    COALESCE(s.ItemId, c.ItemId)       AS ItemId,
    ISNULL(s.StockBalance, 0)          AS StockBalance,
    ISNULL(c.CurrentStock, 0)          AS CurrentStock,
    ISNULL(s.StockBalance, 0) - ISNULL(c.CurrentStock, 0) AS Difference
INTO #Mismatch
FROM s
FULL JOIN c
    ON s.ProjectId = c.ProjectId
   AND s.ItemId    = c.ItemId
WHERE ISNULL(s.StockBalance, 0) <> ISNULL(c.CurrentStock, 0);
```

This version has **no transfer or adjustment filter**, so it lists **every** mismatch. The transfer and adjustment checks are handled in Step 3, where each row is labelled with its likely cause.

**Overview**
```sql
SELECT COUNT(*) AS TotalMismatches,
       SUM(Difference) AS NetDifference,
       SUM(ABS(Difference)) AS TotalAbsoluteDifference
FROM #Mismatch;
```

**Find a small case to trace** (the smallest non-zero difference is the easiest to understand)
```sql
SELECT TOP 20 m.*, p.Name AS ProjectName
FROM #Mismatch m
LEFT JOIN Projects p ON p.Id = m.ProjectId
ORDER BY ABS(m.Difference), m.ItemId;
```

Pick **one** project and item from this list for Step 2: `[ProjectId = ___ , ItemId = ___ ]`

---

## 4. Step 2: Trace One Case from Start to End

Set the project and item chosen above.

```sql
DECLARE @ProjectId INT = 0;      -- replace with the chosen ProjectId
DECLARE @ItemId    INT = 0;      -- replace with the chosen ItemId
```

### 4.1 Every stock row, with a running balance

```sql
SELECT
    st.Id AS StockId,
    st.StockType,
    st.Quantity,
    st.ReferenceTable,
    st.ReferenceId,
    ISNULL(st.IsDelete, 0) AS IsDeleted,
    SUM(CASE
            WHEN ISNULL(st.IsDelete, 0) = 1 THEN 0
            WHEN UPPER(st.StockType) = 'IN'  THEN  ISNULL(st.Quantity, 0)
            WHEN UPPER(st.StockType) = 'OUT' THEN -ISNULL(st.Quantity, 0)
            ELSE 0
        END) OVER (ORDER BY st.Id ROWS UNBOUNDED PRECEDING) AS RunningBalance
FROM Stock st
WHERE st.ProjectId = @ProjectId
  AND st.ItemId    = @ItemId
ORDER BY st.Id;

SELECT SUM(ISNULL(Quantity, 0)) AS CurrentStock
FROM CurrentStock
WHERE ProjectId = @ProjectId
  AND ItemId    = @ItemId;
```

- The **last `RunningBalance`** is the `StockBalance`. Compare it with `CurrentStock`.
- Look at the rows with `ReferenceTable = 'Transfers'` and `'StockAdjustment'`. These are the ones the problem is about.
- If `Stock` has a date column, order by it as well.

### 4.2 What the documents say should exist

For the same project and item, list each **transfer** and **adjustment** with the quantity that **should** be in `Stock` and the quantity that **is** in `Stock`.

> **Assumptions:** a transfer creates an `OUT` row in the **from** project (quantity `TransferQuantity`) and an `IN` row in the **to** project (quantity `ReceivedQuantity`), referenced by `Transfers.Id`. An adjustment creates a row referenced by `StockAdjustment.Id`. If `StockAdjustment` has a project column, filter on it too.

```sql
SELECT
    'Transfer'                         AS DocType,
    t.Id                               AS DocId,
    CAST(t.TransferNo AS NVARCHAR(50)) AS DocNo,
    CAST(t.Status AS NVARCHAR(50))     AS DocStatus,
    CASE WHEN t.FromProjectId = @ProjectId THEN 'OUT' ELSE 'IN' END AS ExpectedDirection,
    CASE WHEN t.FromProjectId = @ProjectId
         THEN td.TransferQuantity
         ELSE ISNULL(td.ReceivedQuantity, 0) END AS ExpectedQty,
    (SELECT SUM(ISNULL(st.Quantity, 0))
     FROM Stock st
     WHERE st.ReferenceTable = 'Transfers'
       AND st.ReferenceId    = t.Id
       AND st.ItemId         = @ItemId
       AND st.ProjectId      = @ProjectId
       AND ISNULL(st.IsDelete, 0) = 0)  AS QtyInStock
FROM Transfers t
INNER JOIN TransferDetails td ON td.TransferId = t.Id
WHERE td.ItemId = @ItemId
  AND @ProjectId IN (t.FromProjectId, t.ToProjectId)

UNION ALL

SELECT
    'Adjustment',
    sa.Id,
    CAST(sa.Id AS NVARCHAR(50)),
    CAST(sa.AdjustmentType AS NVARCHAR(50)),
    CAST(sa.AdjustmentType AS NVARCHAR(50)),
    sad.Quantity,
    (SELECT SUM(ISNULL(st.Quantity, 0))
     FROM Stock st
     WHERE st.ReferenceTable = 'StockAdjustment'
       AND st.ReferenceId    = sa.Id
       AND st.ItemId         = @ItemId
       AND st.ProjectId      = @ProjectId
       AND ISNULL(st.IsDelete, 0) = 0)
FROM StockAdjustment sa
INNER JOIN StockAdjustmentDetail sad ON sad.StockAdjustmentId = sa.Id
WHERE sad.ItemId = @ItemId;
```

**How to read each row**

| What you see | Meaning |
|--------------|---------|
| `ExpectedQty = QtyInStock` | The document is correctly reflected in `Stock` |
| `QtyInStock` is `NULL` or `0` | The document has **no stock row** (never posted, or deleted) |
| `QtyInStock` is **double** the expected quantity | The stock row was **posted twice** |
| `QtyInStock` is different from the expected quantity | The quantity changed after posting, or was partly posted |
| Transfer with `ExpectedQty = 0` and status not received | The transfer is **not received yet**, so no `IN` row is expected |
| Adjustment of another project shows `NULL` | Not a problem if it belongs to a different project |

### 4.3 Deleted rows for this case

```sql
SELECT *
FROM Stock
WHERE ProjectId = @ProjectId
  AND ItemId    = @ItemId
  AND ISNULL(IsDelete, 0) = 1;
```

### 4.4 Write down the finding

| Question | Answer |
|----------|--------|
| Project and item traced | [ ] |
| `StockBalance` / `CurrentStock` / `Difference` | [ ] |
| Document(s) that explain the difference | [ ] |
| What exactly is missing, duplicated, or deleted | [ ] |
| Which table is correct for this case (`Stock` or `CurrentStock`) | [ ] |

If the difference is **fully explained** by the documents, you have found the cause for this case. Repeat with a second case from a **different** group to see whether it is the same cause.

---

## 5. Step 3: Classify All Mismatches by Cause

Label every row of `#Mismatch` with a likely cause.

```sql
IF OBJECT_ID('tempdb..#Classified') IS NOT NULL DROP TABLE #Classified;

SELECT
    m.ProjectId,
    m.ItemId,
    m.StockBalance,
    m.CurrentStock,
    m.Difference,
    CASE
        -- 1. No CurrentStock row at all for this project and item
        WHEN NOT EXISTS (SELECT 1 FROM CurrentStock cs
                         WHERE cs.ProjectId = m.ProjectId AND cs.ItemId = m.ItemId)
            THEN '1. No CurrentStock row'

        -- 2. CurrentStock has a quantity but Stock has no net history
        WHEN m.StockBalance = 0 AND m.CurrentStock <> 0
            THEN '2. CurrentStock without Stock history'

        -- 3. Deleted Stock rows linked to transfers or adjustments
        WHEN EXISTS (SELECT 1 FROM Stock st
                     WHERE st.ProjectId = m.ProjectId AND st.ItemId = m.ItemId
                       AND ISNULL(st.IsDelete, 0) = 1
                       AND st.ReferenceTable IN ('Transfers', 'StockAdjustment'))
            THEN '3. Deleted Stock row (transfer or adjustment)'

        -- 4. A transfer of this item and project has no Stock row
        WHEN EXISTS (SELECT 1
                     FROM TransferDetails td
                     INNER JOIN Transfers t ON t.Id = td.TransferId
                     WHERE td.ItemId = m.ItemId
                       AND m.ProjectId IN (t.FromProjectId, t.ToProjectId)
                       AND NOT EXISTS (SELECT 1 FROM Stock st
                                       WHERE st.ReferenceTable = 'Transfers'
                                         AND st.ReferenceId = t.Id
                                         AND st.ItemId = td.ItemId
                                         AND st.ProjectId = m.ProjectId
                                         AND ISNULL(st.IsDelete, 0) = 0))
            THEN '4. Transfer not posted in Stock'

        -- 5. An adjustment of this item has no Stock row in this project
        WHEN EXISTS (SELECT 1
                     FROM StockAdjustmentDetail sad
                     WHERE sad.ItemId = m.ItemId
                       AND NOT EXISTS (SELECT 1 FROM Stock st
                                       WHERE st.ReferenceTable = 'StockAdjustment'
                                         AND st.ReferenceId = sad.StockAdjustmentId
                                         AND st.ItemId = sad.ItemId
                                         AND st.ProjectId = m.ProjectId
                                         AND ISNULL(st.IsDelete, 0) = 0))
            THEN '5. Adjustment not posted in Stock'

        -- 6. Everything else
        ELSE '6. Other (manual review)'
    END AS LikelyCause
INTO #Classified
FROM #Mismatch m;

-- Summary: how many cases per cause
SELECT
    LikelyCause,
    COUNT(*)             AS TotalCases,
    SUM(Difference)      AS NetDifference,
    SUM(ABS(Difference)) AS TotalAbsoluteDifference
FROM #Classified
GROUP BY LikelyCause
ORDER BY LikelyCause;
```

**Notes**
- The labels are **likely** causes, checked in order. The first matching rule wins.
- Category 5 can be a **false positive** when an adjustment belongs to a **different project** (the adjustment has no stock row in this project). If `StockAdjustment` has a project column, add it to the check.
- Compare the labels with the case traced in Step 2. They should agree.

**Result table to fill in**

| LikelyCause | TotalCases | NetDifference |
|-------------|------------|---------------|
| 1. No CurrentStock row | [ ] | [ ] |
| 2. CurrentStock without Stock history | [ ] | [ ] |
| 3. Deleted Stock row (transfer or adjustment) | [ ] | [ ] |
| 4. Transfer not posted in Stock | [ ] | [ ] |
| 5. Adjustment not posted in Stock | [ ] | [ ] |
| 6. Other (manual review) | [ ] | [ ] |

---

## 6. Step 4: Fix by Category

Decide the **source of truth** with the senior first (see the first-day document). The table assumes **`Stock` is the source of truth** unless the cause shows otherwise.

| Category | Meaning | Recommended fix |
|----------|---------|-----------------|
| **1. No CurrentStock row** | `CurrentStock` was never created | Insert the row with `Quantity = StockBalance` |
| **2. CurrentStock without Stock history** | Stock exists with no ledger | Decide with the senior: set `CurrentStock` to `0`, or create the missing history through a **stock adjustment** if the quantity is real |
| **3. Deleted Stock row** | A transfer or adjustment row was deleted but its effect remains in `CurrentStock` | If the delete was a **mistake**, restore the row (`IsDelete = 0`). If it was **intended**, set `CurrentStock` to the `StockBalance` |
| **4. Transfer not posted** | The transfer exists with no `OUT` / `IN` rows in `Stock` | Post the missing movement through the application's process (or a controlled script), or set `CurrentStock` to the `StockBalance` if the transfer should not count |
| **5. Adjustment not posted** | The adjustment exists with no stock row | Post the missing row through the application's process, or set `CurrentStock` to the `StockBalance` |
| **6. Other** | No clear document cause | Trace each case as in Step 2 and ask the senior |

### Apply the fix: set `CurrentStock` to the `StockBalance` for selected categories

Start with a **dry run** (`ROLLBACK`). Change it to `COMMIT` only after the check query returns no rows. Take a backup before this step.

```sql
BEGIN TRAN;

-- 1. Keep a copy of the rows that will change
SELECT cs.*
INTO CurrentStock_Backup_20261106
FROM CurrentStock cs
INNER JOIN #Classified k
    ON cs.ProjectId = k.ProjectId
   AND cs.ItemId    = k.ItemId
WHERE k.LikelyCause IN ('3. Deleted Stock row (transfer or adjustment)',
                        '4. Transfer not posted in Stock',
                        '5. Adjustment not posted in Stock');   -- choose the categories you agreed on

-- 2. Update existing rows (only where there is exactly one CurrentStock row for the project and item)
UPDATE cs
SET cs.Quantity = k.StockBalance
FROM CurrentStock cs
INNER JOIN #Classified k
    ON cs.ProjectId = k.ProjectId
   AND cs.ItemId    = k.ItemId
WHERE k.LikelyCause IN ('3. Deleted Stock row (transfer or adjustment)',
                        '4. Transfer not posted in Stock',
                        '5. Adjustment not posted in Stock')
  AND (SELECT COUNT(*) FROM CurrentStock x
       WHERE x.ProjectId = cs.ProjectId AND x.ItemId = cs.ItemId) = 1;

PRINT 'Rows updated: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

-- 3. Insert missing rows (category 1). Add every column the table requires.
INSERT INTO CurrentStock (ProjectId, ItemId, Quantity /*, other required columns */)
SELECT k.ProjectId, k.ItemId, k.StockBalance
FROM #Classified k
WHERE k.LikelyCause = '1. No CurrentStock row'
  AND NOT EXISTS (SELECT 1 FROM CurrentStock cs
                  WHERE cs.ProjectId = k.ProjectId AND cs.ItemId = k.ItemId);

-- 4. Check: rows of the chosen categories that still differ (should return no rows)
SELECT k.ProjectId, k.ItemId, k.StockBalance, ISNULL(cs.Quantity, 0) AS CurrentStock
FROM #Classified k
LEFT JOIN (SELECT ProjectId, ItemId, SUM(ISNULL(Quantity, 0)) AS Quantity
           FROM CurrentStock GROUP BY ProjectId, ItemId) cs
       ON cs.ProjectId = k.ProjectId AND cs.ItemId = k.ItemId
WHERE k.LikelyCause IN ('1. No CurrentStock row',
                        '3. Deleted Stock row (transfer or adjustment)',
                        '4. Transfer not posted in Stock',
                        '5. Adjustment not posted in Stock')
  AND ISNULL(cs.Quantity, 0) <> k.StockBalance;

ROLLBACK TRAN;      -- dry run. Change to COMMIT TRAN after the check returns no rows.
```

**Before running**
- Check for **duplicate `CurrentStock` rows** for the same project and item (the update skips them, and they need manual handling):
```sql
SELECT ProjectId, ItemId, COUNT(*) AS TotalRows
FROM CurrentStock
GROUP BY ProjectId, ItemId
HAVING COUNT(*) > 1;
```
- Fix categories **2** and **6** **one case at a time**, because the right fix depends on the cause.

---

## 7. Step 5: Verify

Run the Step 1 query again and check the result:

```sql
-- Re-run the Step 1 query, then:
SELECT COUNT(*) AS RemainingMismatches FROM #Mismatch;
```

`RemainingMismatches` should be `0`, or only the cases that were intentionally left for review.

Also confirm:
- [ ] The stock report in the application shows the same numbers for a few checked items
- [ ] The backup table (`CurrentStock_Backup_20261106`) exists and can be used to undo the change
- [ ] The categories and the fixes were agreed with the senior

---

## 8. Day 2 Progress Log

| Time | What I tried | Result | Next |
|------|--------------|--------|------|
| [ ] | Rebuilt the mismatch list | [ ] | [ ] |
| [ ] | Traced one case (project ___, item ___) | [ ] | [ ] |
| [ ] | Classified all mismatches | [ ] | [ ] |
| [ ] | Fixed category ___ | [ ] | [ ] |
| [ ] | Verified | [ ] | [ ] |

**Cause found:** [ ]
**Cases fixed:** [ ] of [ ]
**Cases left for review:** [ ]

---

## 9. If Still Stuck

Collect this information. It lets the senior (or anyone helping) understand the problem quickly:

| Item | Where from |
|------|------------|
| The **summary table** from Step 3 (cases per cause) | Step 3 |
| **One traced case**: the `Stock` rows, the `CurrentStock` value, and the document list | Step 2 |
| The **source of the procedure** that performs a transfer and the one that performs an adjustment | `EXEC sp_helptext '[dbo].[ProcedureName]'` |
| Which procedures update `CurrentStock` | `SELECT OBJECT_NAME(object_id) FROM sys.sql_modules WHERE definition LIKE '%CurrentStock%';` |
| How `Quantity`, `StockType`, `ReferenceTable`, and `ReferenceId` look in sample rows | `SELECT TOP 20 * FROM Stock WHERE ReferenceTable IN ('Transfers','StockAdjustment');` |

**Questions for the senior**
- [ ] Which table is the source of truth, `Stock` or `CurrentStock`?
- [ ] When and how is `CurrentStock` updated (procedure, trigger, or application code)?
- [ ] Does a transfer update both projects, and when (on send or on receive)?
- [ ] Does an adjustment have a project, and how does it reference `Stock`?
- [ ] May a script correct data, or must corrections go through the application?

---

## 10. Summary

- **Day 2 approach:** rebuild the mismatch list, **trace one small case completely**, **classify** all mismatches by likely cause, then **fix by category**.
- **Tracing** one case (the stock rows with a running balance, plus the transfer and adjustment documents with the expected and actual stock quantities) shows exactly what is missing, duplicated, or deleted.
- **Classification** groups the mismatches into: no `CurrentStock` row, `CurrentStock` without history, deleted stock row, transfer not posted, adjustment not posted, and other.
- **Fixing by category** is safer than fixing row by row. Use a **backup**, a **transaction with a dry run**, and a **check query** before committing.
- **Verify** that the mismatch list is empty, and keep a record of what was changed.
- Find out **why** the mismatches happened (which process updates only one of the two tables), so the problem does not return.