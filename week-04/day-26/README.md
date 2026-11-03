# Stock Report for Item 824: InAmount, OutAmount, StockBalance, CurrentStock, and Difference

**Date:** 3 November 2026
**Database:** SQL Server

## Table of Contents

- [1. Problem Statement](#1-problem-statement)
- [2. Database Tables](#2-database-tables)
- [3. Exploring the Tables](#3-exploring-the-tables)
- [4. Solution](#4-solution)
- [5. Understanding the Solution](#5-understanding-the-solution)
- [6. Practice Queries](#6-practice-queries)
- [7. Summary](#7-summary)

---

## 1. Problem Statement

For **ItemId 824**, find the following values for **each project** from the SQL Server database:

| Column         | Meaning                                                   |
| -------------- | --------------------------------------------------------- |
| `InAmount`     | Total quantity that came **in**                           |
| `OutAmount`    | Total quantity that went **out**                          |
| `StockBalance` | `InAmount - OutAmount`, calculated from the stock history |
| `CurrentStock` | The quantity recorded in the current stock table          |
| `Difference`   | `StockBalance - CurrentStock`                             |

---

## 2. Database Tables

| Table                                      | Purpose                                                                               |
| ------------------------------------------ | ------------------------------------------------------------------------------------- |
| `Projects`                                 | List of projects (`Id`, `Name`)                                                       |
| `Stock`                                    | History of every stock movement (item, project, quantity, `StockType` = `IN` / `OUT`) |
| `CurrentStock`                             | Current quantity of each item in each project                                         |
| `Transfers`, `TransferDetails`             | Stock transfers (header and items)                                                    |
| `StockAdjustment`, `StockAdjustmentDetail` | Stock adjustments (header and items)                                                  |
| `Items`                                    | List of items (`Id = 824` is the item studied)                                        |

**Columns used in the solution**

| Table          | Columns                                                    |
| -------------- | ---------------------------------------------------------- |
| `Stock`        | `ProjectId`, `ItemId`, `StockType`, `Quantity`, `IsDelete` |
| `CurrentStock` | `ProjectId`, `ItemId`, `Quantity`                          |
| `Projects`     | `Id`, `Name`                                               |

---

## 3. Exploring the Tables

The tables were reviewed first to understand their columns and data.

```sql
select * from Projects;
select * from Stock;
select * from CurrentStock;
select * from Transfers;
select * from TransferDetails;
select * from StockAdjustment;
select * from StockAdjustmentDetail;
select * from Items where Id = 824;
```

---

## 4. Solution

```sql
SELECT
    ProjectId,
    ProjectName,
    InAmount,
    OutAmount,
    StockBalance,
    CurrentStock,
    StockBalance - CurrentStock AS Difference

FROM
(
    SELECT
        ProjectId,
        ProjectName,
        InAmount,
        OutAmount,
        StockBalance,
        CurrentStock
    FROM
    (
        SELECT
            COALESCE(s.ProjectId, c.ProjectId) AS ProjectId,
            p.Name AS ProjectName,
            ISNULL(s.InAmount, 0) AS InAmount,
            ISNULL(s.OutAmount, 0) AS OutAmount,
            ISNULL(s.StockBalance, 0) AS StockBalance,
            ISNULL(c.CurrentStock, 0) AS CurrentStock
        FROM
        (
            SELECT
                ProjectId,
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
            WHERE ItemId = 824
              AND ISNULL(IsDelete, 0) = 0
            GROUP BY ProjectId
        ) s
        FULL JOIN
        (
            SELECT
                ProjectId,
                SUM(Quantity) AS CurrentStock
            FROM CurrentStock
            WHERE ItemId = 824
            GROUP BY ProjectId
        ) c
        ON s.ProjectId = c.ProjectId
        LEFT JOIN Projects p
            ON p.Id = COALESCE(s.ProjectId, c.ProjectId)
    ) AS FinalResult
) AS Result

ORDER BY ProjectName;
```

---

## 5. Understanding the Solution

The query is built from the inside out in four steps.

### Step 1: Summarize the `Stock` table (subquery `s`)

```sql
SELECT
    ProjectId,
    SUM(CASE WHEN StockType = 'IN'  THEN ISNULL(Quantity, 0) ELSE 0 END) AS InAmount,
    SUM(CASE WHEN StockType = 'OUT' THEN ISNULL(Quantity, 0) ELSE 0 END) AS OutAmount,
    SUM(CASE WHEN StockType = 'IN'  THEN  ISNULL(Quantity, 0)
             WHEN StockType = 'OUT' THEN -ISNULL(Quantity, 0)
             ELSE 0 END) AS StockBalance
FROM Stock
WHERE ItemId = 824
  AND ISNULL(IsDelete, 0) = 0
GROUP BY ProjectId
```

| Part                         | Purpose                                                      |
| ---------------------------- | ------------------------------------------------------------ |
| `WHERE ItemId = 824`         | Only the item being studied                                  |
| `ISNULL(IsDelete, 0) = 0`    | Ignore deleted rows (`NULL` or `0` means not deleted)        |
| `GROUP BY ProjectId`         | One result row per project                                   |
| `SUM(CASE ... 'IN' ...)`     | Adds only the quantities of `IN` rows, giving **InAmount**   |
| `SUM(CASE ... 'OUT' ...)`    | Adds only the quantities of `OUT` rows, giving **OutAmount** |
| `IN` as `+` and `OUT` as `-` | Gives **StockBalance** = In minus Out                        |
| `ISNULL(Quantity, 0)`        | A `NULL` quantity counts as `0`                              |

This technique is called **conditional aggregation**: a `CASE` inside `SUM` decides, row by row, what to add.

### Step 2: Summarize the `CurrentStock` table (subquery `c`)

```sql
SELECT
    ProjectId,
    SUM(Quantity) AS CurrentStock
FROM CurrentStock
WHERE ItemId = 824
GROUP BY ProjectId
```

One row per project, with the current quantity of item 824.

### Step 3: Combine the two summaries

```sql
FROM (...) s
FULL JOIN (...) c
    ON s.ProjectId = c.ProjectId
LEFT JOIN Projects p
    ON p.Id = COALESCE(s.ProjectId, c.ProjectId)
```

| Part                                 | Purpose                                                                                   |
| ------------------------------------ | ----------------------------------------------------------------------------------------- |
| `FULL JOIN`                          | Keeps projects that appear in **either** summary, even if they are missing from the other |
| `COALESCE(s.ProjectId, c.ProjectId)` | Takes the project ID from whichever side has it                                           |
| `LEFT JOIN Projects p`               | Adds the project **name**                                                                 |
| `ISNULL(..., 0)` in the `SELECT`     | Shows `0` where one side has no row                                                       |

### Step 4: Calculate the difference and sort

```sql
SELECT
    ProjectId, ProjectName, InAmount, OutAmount, StockBalance, CurrentStock,
    StockBalance - CurrentStock AS Difference
FROM (...) AS Result
ORDER BY ProjectName;
```

- The outer query adds the **`Difference`** column: `StockBalance - CurrentStock`.
- `ORDER BY ProjectName` sorts the result by project name.
- The two middle layers (`FinalResult` and `Result`) pass the same columns through. They wrap the calculation so the outer `SELECT` can use the finished column names.

### Output columns

| Column         | Source                              |
| -------------- | ----------------------------------- |
| `ProjectId`    | `Stock` or `CurrentStock`           |
| `ProjectName`  | `Projects.Name`                     |
| `InAmount`     | Sum of `IN` quantities in `Stock`   |
| `OutAmount`    | Sum of `OUT` quantities in `Stock`  |
| `StockBalance` | `InAmount - OutAmount`              |
| `CurrentStock` | Sum of `Quantity` in `CurrentStock` |
| `Difference`   | `StockBalance - CurrentStock`       |

### Example result (illustrative numbers)

| ProjectId | ProjectName | InAmount | OutAmount | StockBalance | CurrentStock | Difference |
| --------- | ----------- | -------- | --------- | ------------ | ------------ | ---------- |
| 1         | Project A   | 150      | 30        | 120          | 120          | 0          |
| 2         | Project B   | 80       | 5         | 75           | 70           | 5          |
| 3         | Project C   | 20       | 10        | 10           | 10           | 0          |

- `Difference = 0`: the stock history and the current stock **agree**.
- `Difference <> 0` (Project B): the two tables **do not agree** for that project.

---

## 6. Practice Queries

These queries break the solution into small parts. The examples use this sample data from the stock table for one item:

| ProjectId | StockType | Quantity |
| --------- | --------- | -------- |
| 1         | IN        | 100      |
| 1         | IN        | 50       |
| 1         | OUT       | 30       |
| 2         | IN        | 80       |
| 2         | OUT       | 5        |
| 3         | IN        | 20       |
| 3         | OUT       | 10       |

> The practice queries below are written for a generic stock table. Use `Stock` (or `StockTransactions` where the query says so) as the table name that exists in your database, and add `WHERE ItemId = 824` to limit them to one item.

### 6.1 Only IN quantity

```sql
SELECT
    SUM(
        CASE
            WHEN StockType = 'IN'
            THEN Quantity
            ELSE 0
        END
    ) AS InAmount
FROM Stock;
```

**Explanation:** for every row, the `CASE` returns the quantity if the row is `IN`, otherwise `0`. `SUM` adds them all, so only incoming quantity is counted.

**Result**

| InAmount |
| -------- |
| 250      |

### 6.2 Only OUT quantity

```sql
SELECT
    SUM(
        CASE
            WHEN StockType = 'OUT'
            THEN Quantity
            ELSE 0
        END
    ) AS OutAmount
FROM Stock;
```

**Explanation:** the same idea for `OUT` rows.

**Result**

| OutAmount |
| --------- |
| 45        |

### 6.3 Only StockBalance

```sql
SUM(
    CASE
        WHEN StockType = 'IN'
        THEN Quantity

        WHEN StockType = 'OUT'
        THEN -Quantity

        ELSE 0
    END
) AS StockBalance
```

**Explanation:** `IN` rows are added (`+Quantity`) and `OUT` rows are subtracted (`-Quantity`), so the sum is the balance. This is an **expression**. Place it in a `SELECT ... FROM Stock` to run it.

```sql
SELECT
    SUM(CASE WHEN StockType = 'IN'  THEN  Quantity
             WHEN StockType = 'OUT' THEN -Quantity
             ELSE 0 END) AS StockBalance
FROM Stock;
```

**Result**

| StockBalance |
| ------------ |
| 205          |

`250 (IN) - 45 (OUT) = 205`

### 6.4 Project wise

```sql
SELECT
    ProjectId,

    SUM(CASE
        WHEN StockType = 'IN' THEN Quantity
        ELSE 0
    END) AS InAmount,

    SUM(CASE
        WHEN StockType = 'OUT' THEN Quantity
        ELSE 0
    END) AS OutAmount,

    sum(
        case
            when stocktype = 'in'
            then quantity
            when stocktype = 'out'
            then -quantity
            else 0
        end
    ) as StockBalance

FROM StockTransactions

GROUP BY ProjectId
```

**Explanation:** `GROUP BY ProjectId` repeats the three calculations **separately for each project**. Every column in the `SELECT` is either in `GROUP BY` (`ProjectId`) or inside an aggregate function (`SUM`).

**Result**

| ProjectId | InAmount | OutAmount | StockBalance |
| --------- | -------- | --------- | ------------ |
| 1         | 150      | 30        | 120          |
| 2         | 80       | 5         | 75           |
| 3         | 20       | 10        | 10           |

### 6.5 Filter using HAVING

```sql
SELECT
    ProjectId,

    SUM(CASE
        WHEN StockType = 'IN' THEN Quantity
        ELSE 0
    END) AS InAmount,

    SUM(CASE
        WHEN StockType = 'OUT' THEN Quantity
        ELSE 0
    END) AS OutAmount,

    sum(
        case
            when stocktype = 'in'
            then quantity
            when stocktype = 'out'
            then -quantity
            else 0
        end
    ) as StockBalance

FROM StockTransactions

GROUP BY ProjectId

having
    sum(
        case
            when stocktype = 'in' then quantity
            when stocktype = 'out' then -quantity
            else 0
        end
    ) > 100

    and

    sum(
        case
            when stocktype = 'out'
            then quantity
            else 0
        end
    ) > 10

    and

    sum(
        case
            when stocktype = 'in'
            then quantity
            else 0
        end
    ) > 30;
```

**Explanation:** `HAVING` filters the **groups** after `GROUP BY`. A project is kept only if **all three** conditions are true:

| Condition            | Meaning                       |
| -------------------- | ----------------------------- |
| StockBalance `> 100` | In minus Out is more than 100 |
| OutAmount `> 10`     | More than 10 went out         |
| InAmount `> 30`      | More than 30 came in          |

**Check against the sample data**

| ProjectId | StockBalance > 100 | OutAmount > 10 | InAmount > 30 | Kept?   |
| --------- | ------------------ | -------------- | ------------- | ------- |
| 1         | 120: Yes           | 30: Yes        | 150: Yes      | **Yes** |
| 2         | 75: No             | 5: No          | 80: Yes       | No      |
| 3         | 10: No             | 10: No         | 20: No        | No      |

**Result**

| ProjectId | InAmount | OutAmount | StockBalance |
| --------- | -------- | --------- | ------------ |
| 1         | 150      | 30        | 120          |

**Why the expressions are repeated in `HAVING`:** in SQL Server, a column alias defined in the `SELECT` (such as `StockBalance`) cannot be used in `HAVING`, so the full expression is written again.

**`WHERE` vs `HAVING`**

|                         | `WHERE`                              | `HAVING`                   |
| ----------------------- | ------------------------------------ | -------------------------- |
| Filters                 | Individual **rows**, before grouping | **Groups**, after grouping |
| Can use `SUM`, `COUNT`? | No                                   | Yes                        |

---

## 7. Summary

- The solution produces **InAmount, OutAmount, StockBalance, CurrentStock, and Difference** for item 824, **per project**.
- **InAmount**, **OutAmount**, and **StockBalance** come from the `Stock` table using **`SUM(CASE ...)`**. **CurrentStock** comes from the `CurrentStock` table.
- Both tables are **summarized per project first** (`GROUP BY ProjectId`), then combined with a **`FULL JOIN`**, and the project name is added with a **`LEFT JOIN`** to `Projects`.
- `ISNULL` and `COALESCE` replace `NULL` values, and `Difference = StockBalance - CurrentStock` shows whether the two tables agree.
- The practice queries build the same logic step by step: IN only, OUT only, balance, **project wise** (`GROUP BY`), and **filtered groups** (`HAVING`).
