# Topic: Stock Query and Stored Procedure Study

**Date:** 17 October 2026

## Table of Contents

- [Part 1: Problem, Most Selling Item](#part-1-problem-most-selling-item)
- [Part 2: Finding the Source Code of a Stored Procedure](#part-2-finding-the-source-code-of-a-stored-procedure)
- [Part 3: Understanding Inv_StockReport](#part-3-understanding-inv_stockreport)
- [Concepts Used](#concepts-used)
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

---

# Part 3: Understanding Inv_StockReport

## What It Does

`Inv_StockReport` produces a **stock report**: for each **item** of a company it shows how much stock came **in**, how much went **out**, the **current quantity**, the **prices**, and the **total stock value**. It can optionally be limited to one **project** and one **sub project**.

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

## Summary

- **Most selling item:** filter the sales rows from `stock`, `GROUP BY ItemId`, `SUM(Quantity)`, `ORDER BY ... DESC`, and keep the top rows. Consider `AND` instead of `OR`, excluding deleted rows, and joining `Items` for the name.
- **Source code of a stored procedure:** `exec sp_helptext '[dbo].[Name]'` (or `OBJECT_DEFINITION`, `sys.sql_modules`, or the SSMS right-click menu).
- **`Inv_StockReport`** builds one row per item with **In quantity, Out quantity, current quantity, prices, and total value**, optionally for a project and sub project.
- Its main techniques are a **`LEFT JOIN`** (keep all items), **optional parameters** in `ON`, **`SUM(CASE ...)`** (conditional totals), and **`OPTION (RECOMPILE)`**.
