# Topic: Stock Query and Stored Procedure Study

**Date:** 17 October 2026

Two tasks from today:

1. **Problem:** find the most selling item from the `stock` table.
2. **Study:** find the source code of a stored procedure (`Inv_StockReport`) and understand how it works.

> **Note:** Column names and values such as `ItemId`, `Quantity`, `StockType`, `ReferenceTable`, `'Invoices'`, `'Out'` are taken from the query and stored procedure used at work. Where something is an assumption, it is marked.

## Table of Contents

- [Part 1: Problem, Most Selling Item](#part-1-problem-most-selling-item)
- [Part 2: Finding the Source Code of a Stored Procedure](#part-2-finding-the-source-code-of-a-stored-procedure)
- [Part 3: Understanding Inv_StockReport](#part-3-understanding-inv_stockreport)
- [Part 4: Worked Example](#part-4-worked-example)
- [Part 5: Things to Notice and Ask](#part-5-things-to-notice-and-ask)
- [Concepts Used](#concepts-used)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# Part 1: Problem, Most Selling Item

## Problem Statement

The database has a `stock` table that records every stock movement (items coming **in** and going **out**). Find out the **most selling item**: the `ItemId` with the **highest total quantity sold**. Show the item ID and its total quantity, sorted from highest to lowest.

## Tables Involved

| Table       | Purpose                                                                          |
| ----------- | -------------------------------------------------------------------------------- |
| `stock`     | Every stock movement: item, quantity, type (In / Out), and which table caused it |
| `purchases` | Items bought (stock coming in)                                                   |
| `invoices`  | Items sold (stock going out)                                                     |

Columns used from `stock` (as seen in the query):

| Column           | Meaning                                                    |
| ---------------- | ---------------------------------------------------------- |
| `ItemId`         | The item                                                   |
| `Quantity`       | Quantity moved                                             |
| `StockType`      | `In` or `Out`                                              |
| `ReferenceTable` | Which table created the stock row (for example `Invoices`) |

## Exploring the Data First

```sql
-- select * from purchases;
select * from stock;
-- select * from invoices;
```

The `purchases` and `invoices` lines are commented out (`--`). Only `stock` is read to see its columns and values.

A useful check before filtering:

```sql
SELECT DISTINCT ReferenceTable, StockType
FROM stock;
```

This shows which combinations exist, so the `WHERE` condition can be chosen correctly.

## Solution

```sql
-- Find out the most selling itemID
select top 5 ItemId, sum(Quantity) as TotalQuantity
from stock
where ReferenceTable = 'Invoices' or StockType = 'Out'
group by ItemId
order by TotalQuantity desc;
```

## How the Solution Works

| Step | Clause                           | What it does                                                         |
| ---- | -------------------------------- | -------------------------------------------------------------------- |
| 1    | `from stock`                     | Read the stock table                                                 |
| 2    | `where ... or ...`               | Keep only the rows that are sales (invoice rows, or stock going out) |
| 3    | `group by ItemId`                | Put all rows of the same item together                               |
| 4    | `sum(Quantity) as TotalQuantity` | Add up the quantity for each item                                    |
| 5    | `order by TotalQuantity desc`    | Sort from the largest total to the smallest                          |
| 6    | `top 5`                          | Keep only the first 5 rows (the 5 best selling items)                |

**Example**

| ItemId | Quantity | StockType | ReferenceTable |
| ------ | -------- | --------- | -------------- |
| 1      | 10       | Out       | Invoices       |
| 1      | 5        | Out       | Invoices       |
| 2      | 20       | Out       | Invoices       |
| 2      | 100      | In        | Purchases      |

After filtering and grouping:

| ItemId | TotalQuantity |
| ------ | ------------- |
| 2      | 20            |
| 1      | 15            |

Item `2` is the top selling item.

## Improving the Solution

**1. `OR` or `AND`?**

| Condition                                           | Rows included                                                                            |
| --------------------------------------------------- | ---------------------------------------------------------------------------------------- |
| `ReferenceTable = 'Invoices' OR StockType = 'Out'`  | Invoice rows **plus** any other "Out" rows (for example damage, transfer, or adjustment) |
| `ReferenceTable = 'Invoices' AND StockType = 'Out'` | Only rows that are **both** from invoices **and** stock going out                        |

For "**selling**", `AND` is usually more exact. With `OR`, non-sale stock-out rows can be counted as sales. The right choice depends on what values exist in the data (check with the `SELECT DISTINCT` query above, or ask the senior).

**2. Deleted rows.** The stored procedure in Part 3 filters `ISNULL(s.IsDelete, 0) = 0`. If `stock` has an `IsDelete` column, deleted rows should be excluded here too.

**3. Item name.** `ItemId` alone is hard to read. Join the `Items` table to show the name.

**4. Top 1 or Top 5.** The question says "most selling" (one item), while the query returns 5. Use `TOP 1` for one item, or `TOP 1 WITH TIES` if two items can share the top value.

**Improved version**

```sql
SELECT TOP 5
    s.ItemId,
    i.Name AS ItemName,
    SUM(s.Quantity) AS TotalQuantity
FROM stock AS s
INNER JOIN Items AS i ON i.Id = s.ItemId
WHERE s.ReferenceTable = 'Invoices'
  AND s.StockType = 'Out'
  AND ISNULL(s.IsDelete, 0) = 0
GROUP BY s.ItemId, i.Name
ORDER BY TotalQuantity DESC;
```

**Least selling item (same query, sorted ascending)**

```sql
SELECT TOP 5
    s.ItemId,
    i.Name AS ItemName,
    SUM(s.Quantity) AS TotalQuantity
FROM stock AS s
INNER JOIN Items AS i ON i.Id = s.ItemId
WHERE s.ReferenceTable = 'Invoices'
  AND s.StockType = 'Out'
  AND ISNULL(s.IsDelete, 0) = 0
GROUP BY s.ItemId, i.Name
ORDER BY TotalQuantity ASC;
```

**In a date range** (if `stock` has a date column, for example `CreatedDate`)

```sql
WHERE ...
  AND s.CreatedDate >= '2026-01-01'
  AND s.CreatedDate <  '2026-02-01'
```

---

# Part 2: Finding the Source Code of a Stored Procedure

## Command

```sql
exec sp_helptext '[dbo].[Inv_StockReport]'
```

`sp_helptext` is a built-in system stored procedure. It returns the **text (source code)** of a stored procedure, view, function, or trigger. The result comes as rows, one line of code per row.

| Part                        | Meaning                                          |
| --------------------------- | ------------------------------------------------ |
| `exec`                      | Run a stored procedure                           |
| `sp_helptext`               | System procedure that shows object source code   |
| `'[dbo].[Inv_StockReport]'` | The object: schema `dbo`, name `Inv_StockReport` |

> Tip: press **Ctrl + T** (results to text) before running, so the code looks like normal text and is easy to copy.

## Other Ways to Get the Source Code

**1. `OBJECT_DEFINITION`** (returns the whole code in one value)

```sql
SELECT OBJECT_DEFINITION(OBJECT_ID('[dbo].[Inv_StockReport]'));
```

**2. System view `sys.sql_modules`**

```sql
SELECT m.definition
FROM sys.sql_modules AS m
WHERE m.object_id = OBJECT_ID('[dbo].[Inv_StockReport]');
```

**3. SSMS (GUI)**

1. Expand the database.
2. Open **Programmability** then **Stored Procedures**.
3. Right-click the procedure.
4. Choose **Modify** (opens an `ALTER PROCEDURE` script) or **Script Stored Procedure as** then **CREATE To** then **New Query Editor Window**.

**4. List all stored procedures**

```sql
SELECT SCHEMA_NAME(schema_id) AS SchemaName, name
FROM sys.procedures
ORDER BY name;
```

**5. Find procedures that use a certain table or word**

```sql
SELECT OBJECT_NAME(object_id) AS ProcedureName
FROM sys.sql_modules
WHERE definition LIKE '%stock%';
```

> If a procedure was created `WITH ENCRYPTION`, its source code cannot be viewed.

## Running the Procedure

The first line of the procedure is a comment showing how it is called:

```sql
-- [Inv_StockReport] 7
```

This is a shortcut for:

```sql
EXEC [dbo].[Inv_StockReport] @companyInfoId = 7;
```

More examples:

```sql
-- For one project
EXEC [dbo].[Inv_StockReport] @companyInfoId = 7, @ProjectId = 3;

-- For one project and one sub project
EXEC [dbo].[Inv_StockReport] @companyInfoId = 7, @ProjectId = 3, @SubProjectId = 12;
```

---

# Part 3: Understanding Inv_StockReport

## What It Does

`Inv_StockReport` produces a **stock report**: for each **item** of a company it shows how much stock came **in**, how much went **out**, the **current quantity**, the **prices**, and the **total stock value**. It can optionally be limited to one **project** and one **sub project**.

## Parameters

| Parameter        | Type | Required?           | Meaning                                                             |
| ---------------- | ---- | ------------------- | ------------------------------------------------------------------- |
| `@companyInfoId` | INT  | **Yes**             | Which company's items to report (`7` in the example)                |
| `@ProjectId`     | INT  | No (default `NULL`) | Only count stock of this project. `NULL` means all projects         |
| `@SubProjectId`  | INT  | No (default `NULL`) | Only count stock of this sub project. `NULL` means all sub projects |

## Source Code

```sql
-- [Inv_StockReport] 7
CREATE PROCEDURE [dbo].[Inv_StockReport]
    @companyInfoId INT,
    @ProjectId INT = NULL,
    @SubProjectId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ProjectId =
            CASE
                WHEN @ProjectId IS NULL THEN NULL
                ELSE s.ProjectId
            END,

        SubProjectId =
            CASE
                WHEN @SubProjectId IS NULL THEN NULL
                ELSE s.SubProjectId
            END,

        i.Id AS ItemId,
        i.Name AS ItemName,

        ISNULL(i.LowStockLevel, 0) AS LowStockLevel,

        MIN(ISNULL(s.UnitPrice, 0)) AS UnitPrice,

        MAX(
            ISNULL(s.SalesPrice, 0)
        ) AS SalesPrice,

        SUM(
            CASE
                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'IN'
                THEN ISNULL(s.Quantity, 0)
                ELSE 0
            END
        ) AS InQty,

        SUM(
            CASE
                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'OUT'
                THEN ISNULL(s.Quantity, 0)
                ELSE 0
            END
        ) AS OutQty,

        SUM(
            CASE
                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'IN'
                THEN ISNULL(s.Quantity, 0)

                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'OUT'
                THEN -ISNULL(s.Quantity, 0)

                ELSE 0
            END
        ) AS Quantity,

        SUM(
            CASE
                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'IN'
                THEN ISNULL(s.Quantity, 0)

                WHEN s.ItemType = 'Good'
                     AND UPPER(s.StockType) = 'OUT'
                THEN -ISNULL(s.Quantity, 0)

                ELSE 0
            END
        )
        * ISNULL(
            NULLIF(MIN(s.UnitPrice), 0),
            1.00
        ) AS TotalPrice

    FROM dbo.Items AS i

    LEFT JOIN dbo.Stock AS s
        ON s.ItemId = i.Id
        AND ISNULL(s.IsDelete, 0) = 0
        AND (
            @ProjectId IS NULL
            OR s.ProjectId = @ProjectId
        )
        AND (
            @SubProjectId IS NULL
            OR s.SubProjectId = @SubProjectId
        )

    WHERE
        ISNULL(i.IsDelete, 0) = 0
        AND i.CompanyInfoId = @companyInfoId

    GROUP BY
        CASE
            WHEN @ProjectId IS NULL THEN NULL
            ELSE s.ProjectId
        END,

        CASE
            WHEN @SubProjectId IS NULL THEN NULL
            ELSE s.SubProjectId
        END,

        i.Id,
        i.Name,
        i.LowStockLevel

    ORDER BY
        i.Name

    OPTION (RECOMPILE);
END
```

## Code Explained, Piece by Piece

### 1. The header

```sql
CREATE PROCEDURE [dbo].[Inv_StockReport]
    @companyInfoId INT,
    @ProjectId INT = NULL,
    @SubProjectId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
```

- `CREATE PROCEDURE` defines the stored procedure. `[dbo]` is the schema.
- `@ProjectId INT = NULL` is a parameter with a **default value**. If the caller does not pass it, it is `NULL`.
- `SET NOCOUNT ON` stops SQL Server from sending the "(N rows affected)" message. It reduces network traffic and avoids extra messages for the application.

### 2. The tables: `Items` and `Stock`

```sql
FROM dbo.Items AS i
LEFT JOIN dbo.Stock AS s
    ON s.ItemId = i.Id
```

| Table   | Alias | Role                                                      |
| ------- | ----- | --------------------------------------------------------- |
| `Items` | `i`   | **Master list** of items (name, low stock level, company) |
| `Stock` | `s`   | **Transactions**: every stock in/out row                  |

It is a **`LEFT JOIN`** from `Items`, so **every item appears in the report, even an item that has no stock rows**. (With an `INNER JOIN`, items without stock would disappear.)

### 3. The conditions inside `ON`

```sql
ON s.ItemId = i.Id
AND ISNULL(s.IsDelete, 0) = 0
AND (@ProjectId IS NULL OR s.ProjectId = @ProjectId)
AND (@SubProjectId IS NULL OR s.SubProjectId = @SubProjectId)
```

| Condition                                        | Meaning                                                                                  |
| ------------------------------------------------ | ---------------------------------------------------------------------------------------- |
| `s.ItemId = i.Id`                                | Link each stock row to its item                                                          |
| `ISNULL(s.IsDelete, 0) = 0`                      | Ignore **soft-deleted** stock rows (`IsDelete` is `NULL` or `0` means not deleted)       |
| `@ProjectId IS NULL OR s.ProjectId = @ProjectId` | **Optional filter.** If no project is passed, take all. Otherwise take only that project |
| `@SubProjectId IS NULL OR ...`                   | Same idea for the sub project                                                            |

**Why are these filters in `ON` and not in `WHERE`?**
In a `LEFT JOIN`, a condition on the **right table** placed in `WHERE` would remove the items that have no stock, which turns the `LEFT JOIN` into an `INNER JOIN`. Putting the condition in `ON` filters the stock rows **before** joining, so all items still appear.

### 4. The `WHERE` clause (filters the items)

```sql
WHERE ISNULL(i.IsDelete, 0) = 0
  AND i.CompanyInfoId = @companyInfoId
```

Only items that are **not deleted** and that belong to the **requested company**.

### 5. The `SELECT` columns

| Column               | Expression                                                    | Meaning                                                                                                               |
| -------------------- | ------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| `ProjectId`          | `CASE WHEN @ProjectId IS NULL THEN NULL ELSE s.ProjectId END` | Shows the project only when a project was requested. Otherwise `NULL`, so all projects collapse into one row per item |
| `SubProjectId`       | Same idea                                                     | Same, for the sub project                                                                                             |
| `ItemId`, `ItemName` | `i.Id`, `i.Name`                                              | Item identity                                                                                                         |
| `LowStockLevel`      | `ISNULL(i.LowStockLevel, 0)`                                  | Warning level for low stock. `NULL` becomes `0`                                                                       |
| `UnitPrice`          | `MIN(ISNULL(s.UnitPrice, 0))`                                 | The **lowest** unit price found among the item's stock rows (`NULL` treated as `0`)                                   |
| `SalesPrice`         | `MAX(ISNULL(s.SalesPrice, 0))`                                | The **highest** sales price found                                                                                     |
| `InQty`              | `SUM(CASE ... 'IN' ...)`                                      | Total quantity that came **in**                                                                                       |
| `OutQty`             | `SUM(CASE ... 'OUT' ...)`                                     | Total quantity that went **out**                                                                                      |
| `Quantity`           | `SUM(IN as +, OUT as -)`                                      | **Current stock** = In minus Out                                                                                      |
| `TotalPrice`         | `Quantity * (MIN(UnitPrice), or 1.00 if it is 0 or NULL)`     | **Value of current stock**                                                                                            |

### 6. Conditional aggregation: `SUM(CASE ...)`

```sql
SUM(
    CASE
        WHEN s.ItemType = 'Good' AND UPPER(s.StockType) = 'IN'
        THEN ISNULL(s.Quantity, 0)
        ELSE 0
    END
) AS InQty
```

This is the key technique. A `CASE` decides **per row** what to add, and `SUM` adds them for the whole group.

- `s.ItemType = 'Good'`: count only physical **goods**. Services have no stock.
- `UPPER(s.StockType) = 'IN'`: makes the comparison independent of letter case (`In`, `in`, `IN`).
- `ISNULL(s.Quantity, 0)`: a `NULL` quantity counts as `0`, so the sum does not break.
- `ELSE 0`: rows that do not match add nothing.

The `Quantity` column uses both cases in one expression: `IN` adds, `OUT` subtracts (`-ISNULL(s.Quantity, 0)`).

```
Quantity = InQty - OutQty
```

### 7. The `TotalPrice` calculation

```sql
SUM( ... in minus out ... )
* ISNULL(NULLIF(MIN(s.UnitPrice), 0), 1.00) AS TotalPrice
```

Read it from the inside out:

| Step | Expression          | Result                                                |
| ---- | ------------------- | ----------------------------------------------------- |
| 1    | `MIN(s.UnitPrice)`  | The lowest unit price of the item                     |
| 2    | `NULLIF(..., 0)`    | If that price is `0`, turn it into `NULL`             |
| 3    | `ISNULL(..., 1.00)` | If it is `NULL` (no price or `0`), use `1.00` instead |
| 4    | `Quantity * result` | Stock value                                           |

So the multiplier is the lowest unit price, or `1.00` when there is no usable price. The `1.00` also **prevents a zero or NULL total** caused only by a missing price.

### 8. `GROUP BY`

```sql
GROUP BY
    CASE WHEN @ProjectId IS NULL THEN NULL ELSE s.ProjectId END,
    CASE WHEN @SubProjectId IS NULL THEN NULL ELSE s.SubProjectId END,
    i.Id, i.Name, i.LowStockLevel
```

Every non-aggregated column in `SELECT` must appear in `GROUP BY`. Here the two `CASE` expressions are repeated exactly as they appear in `SELECT`, together with `i.Id`, `i.Name`, and `i.LowStockLevel`.

Result: **one row per item** (or one row per item per project / sub project when those parameters are given).

### 9. `ORDER BY` and `OPTION (RECOMPILE)`

```sql
ORDER BY i.Name
OPTION (RECOMPILE);
```

- Items are listed **alphabetically by name**.
- `OPTION (RECOMPILE)` tells SQL Server to **build a fresh execution plan every time** the query runs, using the **actual parameter values**. It helps with "optional parameter" queries (`@ProjectId IS NULL OR ...`), where one saved plan can be slow for some parameter combinations. The cost is a small compile time on each run.

## Concept Map

| Concept                                              | Where it is used                                      |
| ---------------------------------------------------- | ----------------------------------------------------- |
| Stored procedure with parameters and defaults        | Header                                                |
| `SET NOCOUNT ON`                                     | First line of the body                                |
| `LEFT JOIN`                                          | `Items` to `Stock`, so items with no stock still show |
| Optional parameter filter (`@p IS NULL OR col = @p`) | Inside the `ON` clause                                |
| Soft delete (`IsDelete`)                             | `ISNULL(IsDelete, 0) = 0`                             |
| `ISNULL`                                             | Replace `NULL` with a value                           |
| `NULLIF`                                             | Turn a `0` price into `NULL`                          |
| `UPPER`                                              | Case-independent comparison                           |
| `CASE`                                               | Choose values per row, and in `GROUP BY`              |
| Conditional aggregation (`SUM(CASE ...)`)            | `InQty`, `OutQty`, `Quantity`, `TotalPrice`           |
| `MIN`, `MAX`                                         | `UnitPrice`, `SalesPrice`                             |
| `GROUP BY`                                           | One row per item                                      |
| `ORDER BY`                                           | Sort by item name                                     |
| `OPTION (RECOMPILE)`                                 | Fresh plan for each run                               |

---

# Part 4: Worked Example

**Items** (company `7`)

| Id  | Name   | LowStockLevel |
| --- | ------ | ------------- |
| 1   | Cement | 20            |
| 2   | Brick  | NULL          |
| 3   | Paint  | 5             |

**Stock**

| ItemId | ItemType | StockType | Quantity | UnitPrice | SalesPrice |
| ------ | -------- | --------- | -------- | --------- | ---------- |
| 1      | Good     | In        | 100      | 500       | 600        |
| 1      | Good     | Out       | 30       | 500       | 600        |
| 2      | Good     | In        | 200      | 10        | 12         |
| 2      | Good     | Out       | 50       | 10        | 12         |

Paint has **no stock rows**.

**Run**

```sql
EXEC [dbo].[Inv_StockReport] @companyInfoId = 7;
```

**Result**

| ItemName | LowStockLevel | UnitPrice | SalesPrice | InQty | OutQty | Quantity | TotalPrice |
| -------- | ------------- | --------- | ---------- | ----- | ------ | -------- | ---------- |
| Brick    | 0             | 10        | 12         | 200   | 50     | 150      | 1500       |
| Cement   | 20            | 500       | 600        | 100   | 30     | 70       | 35000      |
| Paint    | 5             | 0         | 0          | 0     | 0      | 0        | 0          |

(`ProjectId` and `SubProjectId` are `NULL` because they were not passed.)

**How the numbers come out**

- **Cement:** `InQty = 100`, `OutQty = 30`, so `Quantity = 100 - 30 = 70`. `TotalPrice = 70 x 500 = 35000`.
- **Brick:** `LowStockLevel` was `NULL`, shown as `0`. `Quantity = 200 - 50 = 150`. `TotalPrice = 150 x 10 = 1500`.
- **Paint:** the `LEFT JOIN` keeps it even without stock. All quantities are `0`, and `TotalPrice = 0 x 1.00 = 0`.
- Sorted by name: Brick, Cement, Paint.

---

# Part 5: Things to Notice and Ask

Good points to understand or confirm with the senior:

| #   | Observation                                                                                                                            | Why it matters                                                                    |
| --- | -------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| 1   | `TotalPrice` uses the **minimum** unit price, not an average or the latest cost                                                        | If an item was bought at different prices, the total value is an approximation    |
| 2   | If any stock row has a unit price of `0` or `NULL`, `MIN(ISNULL(UnitPrice, 0))` becomes `0`                                            | Then `UnitPrice` shows `0`, and `TotalPrice` falls back to a multiplier of `1.00` |
| 3   | When `@ProjectId` is passed, items that have **no stock in that project** still appear, with `ProjectId` as `NULL` and zero quantities | This comes from the `LEFT JOIN` with the filter in `ON`                           |
| 4   | Only `ItemType = 'Good'` rows count for quantities, but `UnitPrice` and `SalesPrice` are taken from **all** rows                       | Services do not affect quantity                                                   |
| 5   | `Quantity` can become **negative** if more went out than came in                                                                       | Useful for spotting data problems                                                 |
| 6   | `LowStockLevel` is shown but not compared with `Quantity` inside the procedure                                                         | The application or report can highlight items where `Quantity <= LowStockLevel`   |
| 7   | The `ProjectId` / `SubProjectId` `CASE` expression appears in both `SELECT` and `GROUP BY`                                             | They must match exactly                                                           |

**Test idea:** compare one item's result with a manual query.

```sql
SELECT
    SUM(CASE WHEN UPPER(StockType) = 'IN'  THEN Quantity ELSE 0 END) AS InQty,
    SUM(CASE WHEN UPPER(StockType) = 'OUT' THEN Quantity ELSE 0 END) AS OutQty
FROM Stock
WHERE ItemId = 1
  AND ISNULL(IsDelete, 0) = 0
  AND ItemType = 'Good';
```

---

## Concepts Used

- `TOP`, `GROUP BY`, `SUM`, `ORDER BY ... DESC / ASC`
- `WHERE` with `AND` / `OR`
- `INNER JOIN` and `LEFT JOIN`
- `sp_helptext`, `OBJECT_DEFINITION`, `sys.sql_modules`
- Stored procedures with parameters and default values
- `CASE`, `ISNULL`, `NULLIF`, `UPPER`
- Conditional aggregation: `SUM(CASE ...)`
- Optional parameters: `@p IS NULL OR column = @p`
- Soft delete flag: `IsDelete`

---

## Practice Tasks

1. Run `SELECT DISTINCT ReferenceTable, StockType FROM stock;` and list the values you find.
2. Change the most selling item query to use `AND`, and compare the results with the `OR` version.
3. Add the item **name** to the most selling item query using a join with `Items`.
4. Find the **top selling item per month** (hint: `GROUP BY` item and month).
5. Get the source code of `Inv_StockReport` using `sp_helptext`, `OBJECT_DEFINITION`, and the SSMS GUI.
6. Run `Inv_StockReport` with only `@companyInfoId`, then with `@ProjectId`, and compare the rows.
7. Pick one item and verify its `InQty`, `OutQty`, and `Quantity` with a manual query.
8. Find items where `Quantity` is **at or below** `LowStockLevel` (write a query on top of the procedure result, or modify a copy of the procedure).
9. Explain in your own words why the `Stock` filters are in the `ON` clause and not in `WHERE`.
10. Write a short procedure of your own, for example `Inv_TopSellingItems @Top INT`, that returns the top selling items.

---

## Summary

- **Most selling item:** filter the sales rows from `stock`, `GROUP BY ItemId`, `SUM(Quantity)`, `ORDER BY ... DESC`, and keep the top rows. Consider `AND` instead of `OR`, excluding deleted rows, and joining `Items` for the name.
- **Source code of a stored procedure:** `exec sp_helptext '[dbo].[Name]'` (or `OBJECT_DEFINITION`, `sys.sql_modules`, or the SSMS right-click menu).
- **`Inv_StockReport`** builds one row per item with **In quantity, Out quantity, current quantity, prices, and total value**, optionally for a project and sub project.
- Its main techniques are a **`LEFT JOIN`** (keep all items), **optional parameters** in `ON`, **`SUM(CASE ...)`** (conditional totals), and **`OPTION (RECOMPILE)`**.
