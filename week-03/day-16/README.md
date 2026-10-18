# Topic: Understanding the `Inv_StockReport` Procedure

**Date:** 18 October 2026

**Database:** `GayerBazarDB` (SQL Server Express, SSMS)  
**Goal:** Understand the `Inv_StockReport` stored procedure line by line

---

## 1. The Core Idea (in one line)

> **IN − OUT = Current Stock**

This is what the whole procedure does at its heart. Everything else (`ISNULL`, `CASE`, `@ProjectId`) is just filtering and safety handling.

---

## 2. What the Procedure Does

For every item in a company, it shows:

| Column          | Meaning                             |
| --------------- | ----------------------------------- |
| `InQty`         | Total quantity that came into stock |
| `OutQty`        | Total quantity that went out        |
| `Quantity`      | Current stock (IN − OUT)            |
| `UnitPrice`     | Lowest purchase price (`MIN`)       |
| `SalesPrice`    | Highest selling price (`MAX`)       |
| `TotalPrice`    | Quantity × UnitPrice                |
| `LowStockLevel` | The item's minimum stock threshold  |

**Parameters:**

| Parameter        | Meaning                                                      |
| ---------------- | ------------------------------------------------------------ |
| `@companyInfoId` | Which company the report is for (required)                   |
| `@ProjectId`     | Optional. `NULL` = all projects, a value = only that project |
| `@SubProjectId`  | Optional. Same logic for sub-projects                        |

---

## 3. My Practice Tables

I created two small tables in my own database:

- `shopitems` → the list of products (`Name`)
- `shopstock` → stock entries (`Name`, `Type`, `Quantity`)

Data in `shopstock`:

| Name | Type | Quantity |
| ---- | ---- | -------- |
| Pen  | In   | 100      |
| Pen  | Out  | 30       |
| Book | In   | 20       |

`shopitems` has three products: **Pen, Book, Pencil** (Pencil has no stock entries).

> SQL Server is usually case-insensitive, so writing `'pen'` still matches `Pen`.

---

## 4. Lesson 1: SELECT and WHERE

```sql
SELECT * FROM shopstock;
SELECT * FROM shopstock WHERE name = 'pen';
SELECT * FROM shopstock WHERE name = 'pen' AND type = 'in';
```

- **`WHERE`** = filter data using a condition
- **`AND`** = a row appears in the result only if all conditions are true

**Results:** 3 rows → 2 Pen rows → 1 row (Pen + In).

---

## 5. Lesson 2: SUM

```sql
SELECT SUM(quantity) FROM shopstock WHERE name = 'pen' AND type = 'in';
-- Result: 100

SELECT type, SUM(quantity) FROM shopstock WHERE name = 'pen' GROUP BY type;
-- In = 100, Out = 30

SELECT SUM(quantity) FROM shopstock WHERE name = 'book' AND type = 'in';
-- Result: 20
```

- **`SUM(...)`** = add up the values
- **`GROUP BY`** = put rows with the same value into one group and calculate each group separately

---

## 6. Lesson 3: CASE WHEN (splitting by condition)

In plain words: **"if... then... else..."**

```sql
-- See what happens on each row
SELECT name, type, quantity,
       CASE WHEN type = 'in'  THEN quantity ELSE 0 END AS InPart,
       CASE WHEN type = 'out' THEN quantity ELSE 0 END AS OutPart
FROM shopstock;
```

| name | type | quantity | InPart | OutPart |
| ---- | ---- | -------- | ------ | ------- |
| Pen  | In   | 100      | 100    | 0       |
| Pen  | Out  | 30       | 0      | 30      |
| Book | In   | 20       | 20     | 0       |

**`SUM(CASE WHEN ...)`** = the total of the InPart / OutPart columns above.

```sql
-- IN − OUT in one query (OUT becomes negative)
SELECT
    SUM(CASE WHEN type = 'in'  THEN quantity
             WHEN type = 'out' THEN -quantity
             ELSE 0 END) AS CurrentStock
FROM shopstock
WHERE name = 'pen';
-- Result: 70
```

| Product | CurrentStock |
| ------- | ------------ |
| Pen     | 70           |
| Book    | 20           |

---

## 7. Lesson 4: GROUP BY and LEFT JOIN

### GROUP BY: all products in one report

```sql
SELECT
    name,
    SUM(CASE WHEN type = 'in'  THEN quantity ELSE 0 END) AS InQty,
    SUM(CASE WHEN type = 'out' THEN quantity ELSE 0 END) AS OutQty,
    SUM(CASE WHEN type = 'in'  THEN quantity
             WHEN type = 'out' THEN -quantity
             ELSE 0 END) AS CurrentStock
FROM shopstock
GROUP BY name;
```

**Rule:** Any plain column in the `SELECT` (like `name`) must also appear in the `GROUP BY`.

### LEFT JOIN: show products even without stock entries

```sql
SELECT i.name, s.type, s.quantity
FROM shopitems AS i
LEFT JOIN shopstock AS s
    ON s.name = i.name;
```

- **`LEFT JOIN`** = keep **all** rows from the left table (`shopitems`), attach matching rows from the right table, and use `NULL` where nothing matches
- **`INNER JOIN`** = only rows that match in both tables (Pencil would disappear)
- `i`, `s` = aliases (short names)

### Putting it together: a small version of the report

```sql
SELECT
    i.name,
    SUM(CASE WHEN s.type = 'in'  THEN ISNULL(s.quantity, 0) ELSE 0 END) AS InQty,
    SUM(CASE WHEN s.type = 'out' THEN ISNULL(s.quantity, 0) ELSE 0 END) AS OutQty,
    SUM(CASE WHEN s.type = 'in'  THEN ISNULL(s.quantity, 0)
             WHEN s.type = 'out' THEN -ISNULL(s.quantity, 0)
             ELSE 0 END) AS CurrentStock
FROM shopitems AS i
LEFT JOIN shopstock AS s
    ON s.name = i.name
GROUP BY i.name
ORDER BY i.name;
```

| name   | InQty | OutQty | CurrentStock |
| ------ | ----- | ------ | ------------ |
| Book   | 20    | 0      | 20           |
| Pen    | 100   | 30     | 70           |
| Pencil | 0     | 0      | 0            |

Pencil has no stock entries, but thanks to the LEFT JOIN it still appears in the report (with quantity 0).

---

## 8. Mapping to the Real Procedure

| My practice query          | Real procedure                           |
| -------------------------- | ---------------------------------------- |
| `shopitems AS i`           | `dbo.Items AS i`                         |
| `LEFT JOIN shopstock AS s` | `LEFT JOIN dbo.Stock AS s`               |
| `SUM(CASE WHEN ...)`       | `InQty`, `OutQty`, `Quantity`            |
| `GROUP BY i.name`          | `GROUP BY i.Id, i.Name, i.LowStockLevel` |
| `ORDER BY i.name`          | `ORDER BY i.Name`                        |

**Extra parts in the real procedure:**

| Part                                              | Purpose                                                             |
| ------------------------------------------------- | ------------------------------------------------------------------- |
| `ISNULL(s.IsDelete, 0) = 0`                       | Exclude deleted stock entries                                       |
| `s.ItemType = 'Good'`                             | Count only goods (not services)                                     |
| `UPPER(s.StockType)`                              | Treat `in`, `In`, `IN` the same                                     |
| `ISNULL(Quantity, 0)`                             | Treat `NULL` as 0                                                   |
| `@ProjectId IS NULL OR s.ProjectId = @ProjectId`  | Optional filter: no filtering if `NULL`, filter if a value is given |
| `i.CompanyInfoId = @companyInfoId`                | Only one company's data                                             |
| `NULLIF(MIN(UnitPrice), 0)` + `ISNULL(..., 1.00)` | Treat a UnitPrice of 0 as 1 so TotalPrice doesn't become zero       |
| `OPTION (RECOMPILE)`                              | Build a fresh query plan for each parameter combination             |
| `SET NOCOUNT ON`                                  | Suppress the "X rows affected" message                              |


## 10. Progress

- [x] Lesson 1: `SELECT`, `WHERE`, `AND`
- [x] Lesson 2: `SUM`
- [x] Lesson 3: `CASE WHEN`
- [x] Lesson 4: `GROUP BY`, `LEFT JOIN`
- [ ] Lesson 5: Reading the full procedure (parameters, `ISNULL`/`NULLIF`, `TotalPrice`)

## 11. Learning Tips (to remember)

1. **Before** running a query, work out the expected result by hand, then run it and compare.
2. Change **one thing at a time** (e.g. `LEFT` → `INNER`) and observe what changes.
3. Don't read big code all at once. Run **small pieces separately**.
4. Try `INSERT/UPDATE/DELETE` on test data before touching the real database.
