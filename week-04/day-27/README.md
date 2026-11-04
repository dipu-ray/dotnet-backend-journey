# Identifying Stock and CurrentStock Mismatches for Adjusted and Transferred Items

**Date:** 4 November 2026

## Table of Contents

- [1. Problem Statement](#1-problem-statement)
- [2. Database Tables](#2-database-tables)
- [3. Solution](#3-solution)
- [4. Understanding the Solution](#4-understanding-the-solution)
- [5. Summary](#5-summary)

---

## 1. Problem Statement

For items that have gone through **Stock Adjustment** and **Transfer**, identify where the quantity calculated from the **`Stock`** table does not match the quantity in the **`CurrentStock`** table.

For each mismatch, the result should show:

- the **project** and **item**,
- the **projects involved in transfers** (from and to),
- the quantities calculated from `Stock` (in, out, and balance),
- the **current stock**, and
- the **difference** between the two.

---

## 2. Database Tables

| Table          | Purpose                                                                                        |
| -------------- | ---------------------------------------------------------------------------------------------- |
| `Stock`        | History of every stock movement (in and out), with a reference to the document that created it |
| `CurrentStock` | Current quantity of each item in each project                                                  |
| `Projects`     | List of projects                                                                               |
| `Transfers`    | Stock transfers between projects                                                               |

**Columns used**

| Table          | Columns                                                                                                    |
| -------------- | ---------------------------------------------------------------------------------------------------------- |
| `Stock`        | `ProjectId`, `ItemId`, `StockType` (`IN` / `OUT`), `Quantity`, `IsDelete`, `ReferenceTable`, `ReferenceId` |
| `CurrentStock` | `ProjectId`, `ItemId`, `Quantity`                                                                          |
| `Projects`     | `Id`, `Name`                                                                                               |
| `Transfers`    | `Id`, `FromProjectId`, `ToProjectId`                                                                       |

**How the tables are linked**

```
Stock.ReferenceTable = 'Transfers'        and  Stock.ReferenceId = Transfers.Id
Stock.ReferenceTable = 'StockAdjustment'  (the row was created by a stock adjustment)
Stock.ProjectId / CurrentStock.ProjectId  =    Projects.Id
```

**Requirement:** the query uses `STRING_AGG`, which is available in **SQL Server 2017 (14.x) and later**.

---

## 3. Solution

```sql
SELECT
    FinalResult.ProjectId,
    FinalResult.ItemId,
    FinalResult.ProjectName,
    trans.FromProjectId,
    trans.ToProjectId,
    FinalResult.InAmount,
    FinalResult.OutAmount,
    FinalResult.StockBalance,
    FinalResult.CurrentStock,
    FinalResult.StockBalance - FinalResult.CurrentStock AS Difference

FROM
(
    SELECT
        COALESCE(s.ProjectId, c.ProjectId) AS ProjectId,
        COALESCE(s.ItemId, c.ItemId) AS ItemId,
        p.Name AS ProjectName,

        ISNULL(s.InAmount, 0) AS InAmount,
        ISNULL(s.OutAmount, 0) AS OutAmount,
        ISNULL(s.StockBalance, 0) AS StockBalance,
        ISNULL(c.CurrentStock, 0) AS CurrentStock

    FROM
    (
        SELECT
            ProjectId,
            ItemId,

            SUM(CASE
                WHEN StockType = 'IN'
                THEN ISNULL(Quantity, 0)
                ELSE 0
            END) AS InAmount,

            SUM(CASE
                WHEN StockType = 'OUT'
                THEN ISNULL(Quantity, 0)
                ELSE 0
            END) AS OutAmount,

            SUM(CASE
                WHEN StockType = 'IN'
                THEN ISNULL(Quantity, 0)
                WHEN StockType = 'OUT'
                THEN -ISNULL(Quantity, 0)
                ELSE 0
            END) AS StockBalance

        FROM Stock
        WHERE ISNULL(IsDelete, 0) = 0
        GROUP BY ProjectId, ItemId

    ) s

    FULL JOIN
    (
        SELECT
            ProjectId,
            ItemId,
            SUM(ISNULL(Quantity, 0)) AS CurrentStock

        FROM CurrentStock
        GROUP BY ProjectId, ItemId

    ) c
        ON s.ProjectId = c.ProjectId
        AND s.ItemId = c.ItemId

    LEFT JOIN Projects p
        ON p.Id = COALESCE(s.ProjectId, c.ProjectId)

) AS FinalResult

OUTER APPLY
(
    SELECT
        STRING_AGG(x.FromProjectId, ', ') AS FromProjectId,
        STRING_AGG(x.ToProjectId, ', ') AS ToProjectId

    FROM
    (
        SELECT DISTINCT
            CONVERT(NVARCHAR(20), tr.FromProjectId) AS FromProjectId,
            CONVERT(NVARCHAR(20), tr.ToProjectId) AS ToProjectId

        FROM Stock st
        INNER JOIN Transfers tr
            ON tr.Id = st.ReferenceId

        WHERE st.ItemId = FinalResult.ItemId
          AND st.ProjectId = FinalResult.ProjectId
          AND st.ReferenceTable = 'Transfers'
          AND ISNULL(st.IsDelete, 0) = 0

    ) x

) trans

WHERE
    FinalResult.StockBalance <> FinalResult.CurrentStock
    AND FinalResult.CurrentStock <> 0

    AND EXISTS
    (
        SELECT 1
        FROM Stock st
        WHERE st.ItemId = FinalResult.ItemId
          AND st.ProjectId = FinalResult.ProjectId
          AND st.ReferenceTable = 'StockAdjustment'
          AND ISNULL(st.IsDelete, 0) = 0
    )

ORDER BY
    FinalResult.ProjectName,
    FinalResult.ItemId;
```

---

## 4. Understanding the Solution

The query has four parts: a stock summary, a current stock summary, a transfer lookup, and the final filter.

### Step 1: Summarize the `Stock` table (subquery `s`)

```sql
SELECT
    ProjectId,
    ItemId,
    SUM(CASE WHEN StockType = 'IN'  THEN ISNULL(Quantity, 0) ELSE 0 END) AS InAmount,
    SUM(CASE WHEN StockType = 'OUT' THEN ISNULL(Quantity, 0) ELSE 0 END) AS OutAmount,
    SUM(CASE WHEN StockType = 'IN'  THEN  ISNULL(Quantity, 0)
             WHEN StockType = 'OUT' THEN -ISNULL(Quantity, 0)
             ELSE 0 END) AS StockBalance
FROM Stock
WHERE ISNULL(IsDelete, 0) = 0
GROUP BY ProjectId, ItemId
```

| Part                            | Purpose                                     |
| ------------------------------- | ------------------------------------------- |
| `WHERE ISNULL(IsDelete, 0) = 0` | Ignores deleted rows                        |
| `GROUP BY ProjectId, ItemId`    | One row for each item in each project       |
| `SUM(CASE ... 'IN' ...)`        | **InAmount**: total quantity that came in   |
| `SUM(CASE ... 'OUT' ...)`       | **OutAmount**: total quantity that went out |
| `IN` as `+` and `OUT` as `-`    | **StockBalance**: In minus Out              |
| `ISNULL(Quantity, 0)`           | Treats a `NULL` quantity as `0`             |

### Step 2: Summarize the `CurrentStock` table (subquery `c`)

```sql
SELECT
    ProjectId,
    ItemId,
    SUM(ISNULL(Quantity, 0)) AS CurrentStock
FROM CurrentStock
GROUP BY ProjectId, ItemId
```

One row for each item in each project, with its current quantity.

### Step 3: Combine both summaries (`FinalResult`)

```sql
FROM (...) s
FULL JOIN (...) c
    ON s.ProjectId = c.ProjectId
   AND s.ItemId = c.ItemId
LEFT JOIN Projects p
    ON p.Id = COALESCE(s.ProjectId, c.ProjectId)
```

| Part                                                                    | Purpose                                                                                                                                               |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| `FULL JOIN` on **both** `ProjectId` and `ItemId`                        | Matches the stock summary with the current stock summary for the same item in the same project. Keeps combinations that exist in only one of the two. |
| `COALESCE(s.ProjectId, c.ProjectId)` and `COALESCE(s.ItemId, c.ItemId)` | Takes the project and item from whichever side has them                                                                                               |
| `LEFT JOIN Projects`                                                    | Adds the project name                                                                                                                                 |
| `ISNULL(..., 0)`                                                        | Shows `0` where one side has no row                                                                                                                   |

The result of this step is called `FinalResult`. It holds, for each item in each project: `InAmount`, `OutAmount`, `StockBalance`, and `CurrentStock`.

### Step 4: Find the transfer projects (`OUTER APPLY`)

```sql
OUTER APPLY
(
    SELECT
        STRING_AGG(x.FromProjectId, ', ') AS FromProjectId,
        STRING_AGG(x.ToProjectId, ', ')   AS ToProjectId
    FROM
    (
        SELECT DISTINCT
            CONVERT(NVARCHAR(20), tr.FromProjectId) AS FromProjectId,
            CONVERT(NVARCHAR(20), tr.ToProjectId)   AS ToProjectId
        FROM Stock st
        INNER JOIN Transfers tr ON tr.Id = st.ReferenceId
        WHERE st.ItemId = FinalResult.ItemId
          AND st.ProjectId = FinalResult.ProjectId
          AND st.ReferenceTable = 'Transfers'
          AND ISNULL(st.IsDelete, 0) = 0
    ) x
) trans
```

For **each row of `FinalResult`**, this part looks up the transfers connected to that item and project.

| Part                                                                        | Purpose                                                                                                                                                           |
| --------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `FROM Stock st INNER JOIN Transfers tr ON tr.Id = st.ReferenceId`           | Finds the transfer that created each stock row                                                                                                                    |
| `st.ReferenceTable = 'Transfers'`                                           | Only stock rows created by transfers                                                                                                                              |
| `st.ItemId = FinalResult.ItemId` and `st.ProjectId = FinalResult.ProjectId` | Only the current row's item and project (a **correlated** subquery)                                                                                               |
| `SELECT DISTINCT`                                                           | Removes repeated from/to pairs                                                                                                                                    |
| `CONVERT(NVARCHAR(20), ...)`                                                | Converts the project IDs to text so they can be joined into a string                                                                                              |
| `STRING_AGG(..., ', ')`                                                     | Joins the IDs into one comma-separated value, for example `1, 3`                                                                                                  |
| `OUTER APPLY`                                                               | Works like a `LEFT JOIN` for a correlated subquery: the row stays in the result even when it has no transfers (`FromProjectId` and `ToProjectId` are then `NULL`) |

`FromProjectId` and `ToProjectId` are aggregated separately. Each one is a list of project IDs that took part in transfers of that item for that project.

### Step 5: Filter to the mismatches (`WHERE`)

```sql
WHERE
    FinalResult.StockBalance <> FinalResult.CurrentStock
    AND FinalResult.CurrentStock <> 0
    AND EXISTS
    (
        SELECT 1
        FROM Stock st
        WHERE st.ItemId = FinalResult.ItemId
          AND st.ProjectId = FinalResult.ProjectId
          AND st.ReferenceTable = 'StockAdjustment'
          AND ISNULL(st.IsDelete, 0) = 0
    )
```

A row is kept only when **all three** conditions are true:

| Condition                            | Meaning                                                                               |
| ------------------------------------ | ------------------------------------------------------------------------------------- |
| `StockBalance <> CurrentStock`       | The two tables **do not match**                                                       |
| `CurrentStock <> 0`                  | The item has **current stock** (rows where the current stock is `0` are excluded)     |
| `EXISTS (... 'StockAdjustment' ...)` | The item in that project has **at least one stock row created by a stock adjustment** |

### Step 6: Sort

```sql
ORDER BY FinalResult.ProjectName, FinalResult.ItemId;
```

The result is sorted by project name, then by item.

---

## 5. Summary

- **Goal:** find items with a **stock adjustment** where the balance calculated from `Stock` does not match `CurrentStock`.
- **Stock summary:** `InAmount`, `OutAmount`, and `StockBalance` are calculated per **project and item** with `SUM(CASE ...)`, ignoring deleted rows.
- **Current stock summary:** `CurrentStock` is summed per project and item.
- **Combination:** a `FULL JOIN` on `ProjectId` **and** `ItemId`, plus a `LEFT JOIN` to `Projects` for the name.
- **Transfers:** `OUTER APPLY` with `STRING_AGG` lists the from and to projects of the transfers linked to each item and project (`Stock.ReferenceTable = 'Transfers'`, `Stock.ReferenceId = Transfers.Id`).
- **Filter:** keep rows where `StockBalance <> CurrentStock`, `CurrentStock <> 0`, and a `StockAdjustment` stock row exists.
- **Difference:** `StockBalance - CurrentStock` shows the size of each mismatch.
