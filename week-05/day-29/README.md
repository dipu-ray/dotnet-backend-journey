# Fixing StockBalance and CurrentStock Mismatches: Day 2

**Date:** 6 November 2026

> **Note:** this is the **second-day** plan for the mismatch problem between the `Stock` table (the ledger) and the `CurrentStock` table. It continues the first-day document (_Fixing StockBalance and CurrentStock Mismatches After Transfer and Stock Adjustment_). I have not seen the data or the result of the first day, so the sections marked `[ ... ]` are for **your findings**. The queries are written to **find the cause with evidence**, and any data change must be done after a **backup**, inside a **transaction**, and with the senior's approval.

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

| Day 1                                 | Result                                                                                                                                 |
| ------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| Tables studied                        | `Projects`, `Stock`, `CurrentStock`, `Transfers`, `TransferDetails`, `StockAdjustment`, `StockAdjustmentDetail`                        |
| Mismatch query written                | Stock summary and current stock summary joined with a `FULL JOIN`, with transfer projects (`OUTER APPLY`) and a stock adjustment check |
| What worked                           | [Fill in]                                                                                                                              |
| What did not work / where I got stuck | [Fill in]                                                                                                                              |

---

## 2. Day 2 Goal and Plan

**Goal:** understand **why** the mismatches happen, then fix them **by cause** instead of one by one.

| Step | Action                                       | Result                                          |
| ---- | -------------------------------------------- | ----------------------------------------------- |
| 1    | Rebuild the list of mismatches (`#Mismatch`) | Every project and item with a difference        |
| 2    | **Trace one small case** from start to end   | The exact missing, duplicate, or deleted record |
| 3    | **Classify** all mismatches by likely cause  | A count per cause                               |
| 4    | **Fix each category** with the right action  | Differences become `0`                          |
| 5    | **Verify** that no mismatch remains          | Empty mismatch list                             |

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

| LikelyCause                                   | TotalCases | NetDifference |
| --------------------------------------------- | ---------- | ------------- |
| 1. No CurrentStock row                        | [ ]        | [ ]           |
| 2. CurrentStock without Stock history         | [ ]        | [ ]           |
| 3. Deleted Stock row (transfer or adjustment) | [ ]        | [ ]           |
| 4. Transfer not posted in Stock               | [ ]        | [ ]           |
| 5. Adjustment not posted in Stock             | [ ]        | [ ]           |
| 6. Other (manual review)                      | [ ]        | [ ]           |

---

## 6. Step 4: Fix by Category

Decide the **source of truth** with the senior first (see the first-day document). The table assumes **`Stock` is the source of truth** unless the cause shows otherwise.

| Category                                  | Meaning                                                                           | Recommended fix                                                                                                                                                    |
| ----------------------------------------- | --------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **1. No CurrentStock row**                | `CurrentStock` was never created                                                  | Insert the row with `Quantity = StockBalance`                                                                                                                      |
| **2. CurrentStock without Stock history** | Stock exists with no ledger                                                       | Decide with the senior: set `CurrentStock` to `0`, or create the missing history through a **stock adjustment** if the quantity is real                            |
| **3. Deleted Stock row**                  | A transfer or adjustment row was deleted but its effect remains in `CurrentStock` | If the delete was a **mistake**, restore the row (`IsDelete = 0`). If it was **intended**, set `CurrentStock` to the `StockBalance`                                |
| **4. Transfer not posted**                | The transfer exists with no `OUT` / `IN` rows in `Stock`                          | Post the missing movement through the application's process (or a controlled script), or set `CurrentStock` to the `StockBalance` if the transfer should not count |
| **5. Adjustment not posted**              | The adjustment exists with no stock row                                           | Post the missing row through the application's process, or set `CurrentStock` to the `StockBalance`                                                                |
| **6. Other**                              | No clear document cause                                                           | Trace each case as in Step 2 and ask the senior                                                                                                                    |

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

| Time | What I tried                            | Result | Next |
| ---- | --------------------------------------- | ------ | ---- |
| [ ]  | Rebuilt the mismatch list               | [ ]    | [ ]  |
| [ ]  | Traced one case (project **_, item _**) | [ ]    | [ ]  |
| [ ]  | Classified all mismatches               | [ ]    | [ ]  |
| [ ]  | Fixed category \_\_\_                   | [ ]    | [ ]  |
| [ ]  | Verified                                | [ ]    | [ ]  |

**Cause found:** [ ]
**Cases fixed:** [ ] of [ ]
**Cases left for review:** [ ]

---

## 9. If Still Stuck

Collect this information. It lets the senior (or anyone helping) understand the problem quickly:

| Item                                                                                             | Where from                                                                                   |
| ------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------- |
| The **summary table** from Step 3 (cases per cause)                                              | Step 3                                                                                       |
| **One traced case**: the `Stock` rows, the `CurrentStock` value, and the document list           | Step 2                                                                                       |
| The **source of the procedure** that performs a transfer and the one that performs an adjustment | `EXEC sp_helptext '[dbo].[ProcedureName]'`                                                   |
| Which procedures update `CurrentStock`                                                           | `SELECT OBJECT_NAME(object_id) FROM sys.sql_modules WHERE definition LIKE '%CurrentStock%';` |
| How `Quantity`, `StockType`, `ReferenceTable`, and `ReferenceId` look in sample rows             | `SELECT TOP 20 * FROM Stock WHERE ReferenceTable IN ('Transfers','StockAdjustment');`        |

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
