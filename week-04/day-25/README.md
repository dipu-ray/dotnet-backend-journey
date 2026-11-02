# Stock Investigation: Item 824 (SQL Server)

**Date:** 2 November 2026

Documentation of the work on **ItemId 824**: checking its stock in and out, its current stock, its transfers, and its adjustments, then comparing the **stock transactions** with the **current stock** for each project.

## Table of Contents

- [1. Exploring the Tables](#1-exploring-the-tables)
- [2. Queries Written](#2-queries-written)
- [3. Solution: Stock per Project](#3-solution-stock-per-project)
- [4. Concepts Used](#4-concepts-used)
- [5. Summary](#5-summary)

---

## 1. Exploring the Tables

First look at each table to understand the columns and data.

```sql
select * from Stock;
select * from CurrentStock;
select * from Transfers;
select * from TransferDetails;
select * from StockAdjustment;
select * from StockAdjustmentDetail;

select * from Items where Id = 824;

select * from Projects;
```

---

## 2. Queries Written

### 5.1 Stock in and stock out from `Stock`

```sql
-- How many stock in/out rows exist for item 824 in the Stock table
select count(*) as TotalStockIn  from stock where itemid = 824 and stocktype = 'in';
select count(*) as TotalStockOut from stock where itemid = 824 and stocktype = 'out';
```

**What it does:** counts the **number of rows** (movements) of type `in` and `out`.

```sql
select
    stocktype,
    count(*)      as TotalRows,
    sum(Quantity) as TotalQuantity
from stock
where itemid = 824
group by stocktype;
```

| stocktype | TotalRows | TotalQuantity        |
| --------- | --------- | -------------------- |
| In        | (rows)    | (total quantity in)  |
| Out       | (rows)    | (total quantity out) |

### 5.2 Current stock from `CurrentStock`

```sql
-- Current stock of item 824 in the CurrentStock table
select sum(Quantity) as TotalStock
from CurrentStock
where itemid = 824;
```

**What it does:** adds the current quantity of item 824 over **all projects**. This is the number to compare with the stock-in minus stock-out from 5.1.

### 5.3 Transfer history from `Transfers`

```sql
-- Transfer history of item 824
select
    t.TransferNo,
    t.TransferDate,
    t.TransferType,
    t.Status,
    td.ItemId,
    td.TransferQuantity,
    td.ReceivedQuantity
from Transfers t
inner join TransferDetails td on t.Id = td.TransferId
where td.ItemId = 824;
```

**What it does:** joins the transfer **header** (`Transfers`) with its **items** (`TransferDetails`) and shows every transfer that contains item 824.

**Reading the result:**

| Column                                | Meaning                                                                           |
| ------------------------------------- | --------------------------------------------------------------------------------- |
| `TransferQuantity`                    | Quantity sent                                                                     |
| `ReceivedQuantity`                    | Quantity received at the other side                                               |
| `TransferQuantity - ReceivedQuantity` | Quantity **not yet received** (in transit, or a difference to explain)            |
| `Status`                              | Whether the transfer is pending, approved, or completed (check the actual values) |

### 5.4 Total transferred

```sql
select sum(TransferQuantity) as TotalTransferred
from TransferDetails
where ItemId = 824;
```

Add the received total and the difference:

```sql
select
    sum(TransferQuantity) as TotalTransferred,
    sum(ReceivedQuantity) as TotalReceived,
    sum(TransferQuantity) - sum(ReceivedQuantity) as NotReceived
from TransferDetails
where ItemId = 824;
```

### 5.5 Stock adjustment history

```sql
select
    sa.Id,
    sa.AdjustmentDate,
    sa.Remarks,
    sa.AdjustmentType,
    sad.ItemId,
    sad.Quantity,
    sad.TotalPrice,
    sad.Reason
from StockAdjustment sa
inner join StockAdjustmentDetail sad on sa.Id = sad.StockAdjustmentId
where sad.ItemId = 824;
```

**What it does:** joins the adjustment **header** with its **items** and shows every adjustment of item 824, with the reason.

### 5.6 Total adjusted quantity

```sql
-- Total adjusted quantity of item 824 (from the StockAdjustmentDetail table)
select sum(Quantity) as TotalAdjustedQuantity
from StockAdjustmentDetail
where ItemId = 824;
```

> The comment in the original query said "from 'TotalAdjustedQuantity' table". `TotalAdjustedQuantity` is the **column alias**, and the table is **`StockAdjustmentDetail`**.

**Important:** adjustments can **increase** or **decrease** stock (see `AdjustmentType`). A plain `SUM(Quantity)` can mix both. Group by the type to see them separately:

```sql
select
    sa.AdjustmentType,
    sum(sad.Quantity) as TotalQuantity
from StockAdjustment sa
inner join StockAdjustmentDetail sad on sa.Id = sad.StockAdjustmentId
where sad.ItemId = 824
group by sa.AdjustmentType;
```

---

## 3. Solution: Stock per Project

**Question:** for item 824, show the stock of each project from the `Stock` table and from the `CurrentStock` table.

```sql
SELECT
    p.Name AS ProjectName,
    ISNULL(s.StockBalance, 0)  AS StockBalance,
    ISNULL(cs.CurrentStock, 0) AS CurrentStock
FROM Projects p

LEFT JOIN (
    SELECT
        ProjectId,
        SUM(Quantity) AS StockBalance
    FROM Stock
    WHERE ItemId = 824
    GROUP BY ProjectId
) s ON p.Id = s.ProjectId

LEFT JOIN (
    SELECT
        ProjectId,
        SUM(Quantity) AS CurrentStock
    FROM CurrentStock
    WHERE ItemId = 824
    GROUP BY ProjectId
) cs ON p.Id = cs.ProjectId

WHERE s.ProjectId IS NOT NULL OR cs.ProjectId IS NOT NULL
ORDER BY p.Name;
```

### How it works

| Part                                                        | What it does                                                                                    |
| ----------------------------------------------------------- | ----------------------------------------------------------------------------------------------- |
| `FROM Projects p`                                           | Start from the list of all projects                                                             |
| Subquery `s`                                                | From `Stock`, take only item 824 and **add up the quantity per project** (`GROUP BY ProjectId`) |
| Subquery `cs`                                               | From `CurrentStock`, take only item 824 and **add up the quantity per project**                 |
| `LEFT JOIN ... s` and `LEFT JOIN ... cs`                    | Attach both totals to each project. A project with no rows gets `NULL`                          |
| `ISNULL(..., 0)`                                            | Show `0` instead of `NULL`                                                                      |
| `WHERE s.ProjectId IS NOT NULL OR cs.ProjectId IS NOT NULL` | Keep only projects that have **at least one** stock or current stock row for item 824           |
| `ORDER BY p.Name`                                           | Sort by project name                                                                            |

**Why subqueries?** Each table is first **summarized to one row per project**. If `Stock` and `CurrentStock` were joined directly, the rows would multiply and the sums would be wrong. Summarizing first avoids this.

### Example result (illustrative numbers)

| ProjectName | StockBalance | CurrentStock |
| ----------- | ------------ | ------------ |
| Project A   | 120          | 120          |
| Project B   | 80           | 75           |
| Project C   | 0            | 30           |

**How to read it**

| Case                                                  | Meaning                                                       |
| ----------------------------------------------------- | ------------------------------------------------------------- |
| `StockBalance = CurrentStock` (Project A)             | The two tables **agree**                                      |
| Different values (Project B)                          | **Mismatch.** Check transfers and adjustments                 |
| `StockBalance = 0` but `CurrentStock > 0` (Project C) | Current stock exists with **no matching movement** in `Stock` |

---

## 7. Review and Improvements

The solution is correct in structure. These points make it more accurate.

### 7.1 Add the In / Out direction (most important)

`SUM(Quantity)` adds **every** row. If `Quantity` is stored as a **positive number for both in and out** (this is how the `Inv_StockReport` stored procedure treats it: `IN` adds, `OUT` subtracts), the plain sum **overstates** the balance.

```sql
-- Stock balance = In minus Out
SUM(
    CASE
        WHEN UPPER(StockType) = 'IN'  THEN  ISNULL(Quantity, 0)
        WHEN UPPER(StockType) = 'OUT' THEN -ISNULL(Quantity, 0)
        ELSE 0
    END
) AS StockBalance
```

> **Assumption to confirm:** how out quantities are stored (positive with `StockType = 'Out'`, or already negative). Check with a quick query: `select top 20 StockType, Quantity from Stock where ItemId = 824;`

### 7.2 Ignore deleted rows

The stored procedure uses soft delete (`IsDelete`). Add the same filter if `Stock` has the column:

```sql
WHERE ItemId = 824
  AND ISNULL(IsDelete, 0) = 0
```

### 7.3 Show the difference

Add a **Difference** column and, if needed, show only the mismatches.

### 7.4 Do not hide stock without a project

The query starts from `Projects`, so rows where `ProjectId` is `NULL` (or points to a missing project) are **not shown**. A `FULL JOIN` of the two summaries keeps them. A `NULL` does not match another `NULL` in a join, so use `ISNULL` in the join condition.

### 7.5 Improved version

```sql
;WITH StockBal AS (
    SELECT
        ProjectId,
        SUM(
            CASE
                WHEN UPPER(StockType) = 'IN'  THEN  ISNULL(Quantity, 0)
                WHEN UPPER(StockType) = 'OUT' THEN -ISNULL(Quantity, 0)
                ELSE 0
            END
        ) AS StockBalance
    FROM Stock
    WHERE ItemId = 824
      AND ISNULL(IsDelete, 0) = 0
    GROUP BY ProjectId
),
CurBal AS (
    SELECT
        ProjectId,
        SUM(Quantity) AS CurrentStock
    FROM CurrentStock
    WHERE ItemId = 824
    GROUP BY ProjectId
)
SELECT
    COALESCE(sb.ProjectId, cb.ProjectId)            AS ProjectId,
    p.Name                                          AS ProjectName,
    ISNULL(sb.StockBalance, 0)                      AS StockBalance,
    ISNULL(cb.CurrentStock, 0)                      AS CurrentStock,
    ISNULL(sb.StockBalance, 0) - ISNULL(cb.CurrentStock, 0) AS Difference
FROM StockBal sb
FULL JOIN CurBal cb
    ON ISNULL(sb.ProjectId, 0) = ISNULL(cb.ProjectId, 0)
LEFT JOIN Projects p
    ON p.Id = COALESCE(sb.ProjectId, cb.ProjectId)
ORDER BY p.Name;
```

**Show only the projects where the two do not match:** add this before `ORDER BY`:

```sql
WHERE ISNULL(sb.StockBalance, 0) <> ISNULL(cb.CurrentStock, 0)
```

| Change              | Why                                                                             |
| ------------------- | ------------------------------------------------------------------------------- |
| `CASE` for In / Out | Correct balance                                                                 |
| `IsDelete` filter   | Ignore deleted movements                                                        |
| `FULL JOIN`         | Show projects present in only one of the two tables, and rows without a project |
| `Difference` column | Mismatches are easy to see                                                      |
| `WITH` (CTE)        | Same logic as the subqueries, easier to read                                    |

---

## 8. Investigating a Mismatch

When a project has a **difference**, look for the cause.

### 8.1 One summary of all sources for item 824

```sql
select 'Stock IN' as Source, sum(Quantity) as Quantity
from Stock
where ItemId = 824 and upper(StockType) = 'IN' and isnull(IsDelete, 0) = 0

union all
select 'Stock OUT', sum(Quantity)
from Stock
where ItemId = 824 and upper(StockType) = 'OUT' and isnull(IsDelete, 0) = 0

union all
select 'CurrentStock', sum(Quantity)
from CurrentStock
where ItemId = 824

union all
select 'Transferred', sum(TransferQuantity)
from TransferDetails
where ItemId = 824

union all
select 'Received (transfers)', sum(ReceivedQuantity)
from TransferDetails
where ItemId = 824

union all
select 'Adjusted', sum(Quantity)
from StockAdjustmentDetail
where ItemId = 824;
```

`UNION ALL` stacks the results into one small table that can be compared line by line.

| Source               | Quantity           |
| -------------------- | ------------------ |
| Stock IN             | (total in)         |
| Stock OUT            | (total out)        |
| CurrentStock         | (current quantity) |
| Transferred          | (sent)             |
| Received (transfers) | (received)         |
| Adjusted             | (adjusted)         |

**Reconciliation idea:**

```
Stock IN - Stock OUT   should equal   CurrentStock
```

If it does not, the difference should be explained by **transfers not received** or **adjustments** that were not recorded in `Stock` (or the other way round).

### 8.2 Transfers not fully received

```sql
select
    t.TransferNo, t.TransferDate, t.TransferType, t.Status,
    td.TransferQuantity, td.ReceivedQuantity,
    td.TransferQuantity - td.ReceivedQuantity as NotReceived
from Transfers t
inner join TransferDetails td on t.Id = td.TransferId
where td.ItemId = 824
  and td.TransferQuantity <> isnull(td.ReceivedQuantity, 0);
```

### 8.3 Adjustments by type

(See the query in 5.6.)

### 8.4 Latest movements of the item

```sql
select top 20 *
from Stock
where ItemId = 824
order by Id desc;
```

### Possible causes of a mismatch

| Cause                                                                     | What to check                                                               |
| ------------------------------------------------------------------------- | --------------------------------------------------------------------------- |
| Transfer sent but **not received** yet                                    | `TransferQuantity` vs `ReceivedQuantity`, and `Status`                      |
| Adjustment saved but **not reflected** in `Stock` or `CurrentStock`       | Adjustment rows against stock rows                                          |
| Stock row **deleted** (`IsDelete = 1`) but `CurrentStock` was not updated | `IsDelete` rows                                                             |
| `CurrentStock` not updated after a movement                               | How and when `CurrentStock` is updated (procedure, trigger, or application) |
| Data entered **directly** in the database                                 | Rows with no reference to a document                                        |
| Wrong project on a row                                                    | `ProjectId` of the movement                                                 |
| Duplicate stock rows                                                      | Same reference entered twice                                                |

---

## 4. Concepts Used

| Concept                                | Where                                                        |
| -------------------------------------- | ------------------------------------------------------------ |
| `SELECT`, `WHERE`, `ORDER BY`          | Every query                                                  |
| `COUNT` vs `SUM`                       | Counting rows vs adding quantity                             |
| `GROUP BY`                             | Totals per project, per stock type, per adjustment type      |
| `INNER JOIN`                           | Header and detail tables (`Transfers` and `TransferDetails`) |
| `LEFT JOIN`, `FULL JOIN`               | Keep rows that have no match                                 |
| Subquery / derived table, CTE (`WITH`) | Summarize a table before joining                             |
| `ISNULL`, `COALESCE`                   | Replace `NULL` with a value                                  |
| `CASE`                                 | In minus out calculation                                     |
| `UNION ALL`                            | One summary table from several sources                       |
| Aliases (`AS`)                         | Readable column names                                        |

---

## 5. Summary

- **Problem:** for item **824**, compare the stock from the **`Stock`** history with **`CurrentStock`** for each project, and explain any difference using **transfers** and **adjustments**.
- **`COUNT(*)`** counts rows. Use **`SUM(Quantity)`** to get the quantity.
- The **per-project solution** summarizes `Stock` and `CurrentStock` separately (subqueries with `GROUP BY ProjectId`), then attaches both to `Projects` with `LEFT JOIN`, and replaces `NULL` with `0`.
- To be accurate: calculate the balance as **In minus Out** with `CASE`, ignore **deleted** rows, add a **Difference** column, and use a **`FULL JOIN`** so no stock is hidden.
- To find the cause of a mismatch, check **transfers not received**, **adjustments**, deleted rows, and how `CurrentStock` is updated.
- Investigate with **`SELECT`** only, and correct data only through the normal process, with a backup.
