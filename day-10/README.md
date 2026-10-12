# Topics: Practice SQL

**Date:** 12 October 2026

## Table of Contents

- [Sample Table](#sample-table)
- [Aggregate Functions](#aggregate-functions)
- [MIN()](#min)
- [MAX()](#max)
- [COUNT()](#count)
- [SUM()](#sum)
- [AVG()](#avg)
- [LIKE](#like)
- [Wildcards](#wildcards)
- [IN](#in)
- [BETWEEN](#between)
- [Aliases](#aliases)
- [Summary](#summary)

---

## Sample Table

All examples use the `products` table.

```sql
CREATE TABLE products (
    id       INT PRIMARY KEY,
    name     VARCHAR(50),
    category VARCHAR(30),
    price    DECIMAL(10, 2),
    quantity INT
);

INSERT INTO products (id, name, category, price, quantity) VALUES
(1, 'Laptop',   'Electronics', 55000, 5),
(2, 'Mouse',    'Electronics',   500, 50),
(3, 'Keyboard', 'Electronics',  1200, 30),
(4, 'Notebook', 'Stationery',     80, 200),
(5, 'Pen',      'Stationery',     10, 500),
(6, 'Chair',    'Furniture',    4500, NULL);
```

| id  | name     | category    | price | quantity |
| --- | -------- | ----------- | ----- | -------- |
| 1   | Laptop   | Electronics | 55000 | 5        |
| 2   | Mouse    | Electronics | 500   | 50       |
| 3   | Keyboard | Electronics | 1200  | 30       |
| 4   | Notebook | Stationery  | 80    | 200      |
| 5   | Pen      | Stationery  | 10    | 500      |
| 6   | Chair    | Furniture   | 4500  | NULL     |

---

## Aggregate Functions

An **aggregate function** works on **many rows** and returns **one value**.

| Function  | Returns           |
| --------- | ----------------- |
| `MIN()`   | Smallest value    |
| `MAX()`   | Largest value     |
| `COUNT()` | Number of rows    |
| `SUM()`   | Total of values   |
| `AVG()`   | Average of values |

Key points:

- Aggregate functions **ignore `NULL`** values (except `COUNT(*)`).
- They are often used with `WHERE` to filter rows first, and with `GROUP BY` to calculate per group.

---

## MIN()

Returns the **smallest** value in a column.

```sql
SELECT MIN(price) FROM products;
```

Result: `10`

```sql
SELECT MIN(price) FROM products WHERE category = 'Electronics';
```

Result: `500`

## MAX()

Returns the **largest** value in a column.

```sql
SELECT MAX(price) FROM products;
```

Result: `55000`

## COUNT()

Returns the **number of rows**.

```sql
SELECT COUNT(*) FROM products;              -- all rows
SELECT COUNT(quantity) FROM products;       -- rows where quantity is not NULL
SELECT COUNT(DISTINCT category) FROM products;  -- unique values
```

| Query                      | Result                 |
| -------------------------- | ---------------------- |
| `COUNT(*)`                 | `6`                    |
| `COUNT(quantity)`          | `5` (Chair has `NULL`) |
| `COUNT(DISTINCT category)` | `3`                    |

```sql
SELECT COUNT(*) FROM products WHERE price > 1000;
```

Result: `3`

## SUM()

Returns the **total** of a numeric column.

```sql
SELECT SUM(quantity) FROM products;
```

Result: `785`

```sql
SELECT SUM(price) FROM products WHERE category = 'Stationery';
```

Result: `90`

## AVG()

Returns the **average** of a numeric column.

```sql
SELECT AVG(price) FROM products;
```

Result: `10215` (61290 / 6)

```sql
SELECT AVG(quantity) FROM products;
```

Result: `157` (785 / 5, because the `NULL` row is ignored)

### Using several functions together

```sql
SELECT
    COUNT(*)   AS total_products,
    MIN(price) AS cheapest,
    MAX(price) AS most_expensive,
    AVG(price) AS average_price
FROM products;
```

| total_products | cheapest | most_expensive | average_price |
| -------------- | -------- | -------------- | ------------- |
| 6              | 10       | 55000          | 10215         |

### Per group with `GROUP BY`

```sql
SELECT category, COUNT(*) AS total, AVG(price) AS avg_price
FROM products
GROUP BY category;
```

| category    | total | avg_price |
| ----------- | ----- | --------- |
| Electronics | 3     | 18900     |
| Stationery  | 2     | 45        |
| Furniture   | 1     | 4500      |

---

## LIKE

`LIKE` searches for a **pattern** in text. It is used in `WHERE`, together with **wildcards**.

```sql
SELECT * FROM products WHERE name LIKE 'P%';
```

Result: `Pen`

```sql
SELECT * FROM products WHERE name NOT LIKE 'P%';   -- everything that does not start with P
```

> `LIKE` is usually **case-insensitive** in MySQL and SQL Server, and **case-sensitive** in PostgreSQL (use `ILIKE` there for case-insensitive search).

---

## Wildcards

Wildcards are special characters used with `LIKE`.

| Wildcard         | Meaning                               | Database   |
| ---------------- | ------------------------------------- | ---------- |
| `%`              | Zero or more characters               | All        |
| `_`              | Exactly one character                 | All        |
| `[ ]`            | Any one character from a set or range | SQL Server |
| `[^ ]` or `[! ]` | Any character **not** in the set      | SQL Server |

### Examples

| Pattern                 | Meaning                            | Matches                           |
| ----------------------- | ---------------------------------- | --------------------------------- |
| `'P%'`                  | Starts with P                      | Pen                               |
| `'%book'`               | Ends with "book"                   | Notebook                          |
| `'%o%'`                 | Contains "o"                       | Laptop, Mouse, Keyboard, Notebook |
| `'_en'`                 | Any 1 letter, then "en"            | Pen                               |
| `'M____'`               | M followed by exactly 4 characters | Mouse                             |
| `'[LM]%'` (SQL Server)  | Starts with L or M                 | Laptop, Mouse                     |
| `'[^LM]%'` (SQL Server) | Does not start with L or M         | Keyboard, Notebook, Pen, Chair    |

```sql
SELECT name FROM products WHERE name LIKE '%o%';
SELECT name FROM products WHERE name LIKE '_en';
SELECT name FROM products WHERE name LIKE '[LM]%';   -- SQL Server only
```

> To search for a real `%` or `_` character, use `ESCAPE`: `LIKE '10\%' ESCAPE '\'`.

---

## IN

`IN` checks if a value matches **any value in a list**. It is a shorter form of many `OR` conditions.

```sql
SELECT * FROM products
WHERE category IN ('Electronics', 'Furniture');
```

Result: Laptop, Mouse, Keyboard, Chair

Same as:

```sql
WHERE category = 'Electronics' OR category = 'Furniture'
```

### NOT IN

```sql
SELECT * FROM products
WHERE category NOT IN ('Electronics', 'Furniture');
```

Result: Notebook, Pen

### IN with a subquery

```sql
SELECT name FROM products
WHERE price IN (SELECT MAX(price) FROM products);
```

Result: `Laptop`

---

## BETWEEN

`BETWEEN` selects values within a **range**. The range is **inclusive** (both ends are included).

```sql
SELECT * FROM products
WHERE price BETWEEN 500 AND 5000;
```

Result: Mouse (500), Keyboard (1200), Chair (4500)

Same as:

```sql
WHERE price >= 500 AND price <= 5000
```

### NOT BETWEEN

```sql
SELECT name, price FROM products
WHERE price NOT BETWEEN 500 AND 5000;
```

Result: Laptop, Notebook, Pen

### Works with dates too

```sql
SELECT * FROM orders
WHERE order_date BETWEEN '2026-10-01' AND '2026-10-31';
```

> Always write the **smaller value first**: `BETWEEN 10 AND 100`, not `BETWEEN 100 AND 10`.

---

## Aliases

An **alias** gives a column or table a **temporary name**. It makes results easier to read and queries shorter. The alias exists only for that query.

### Column alias

```sql
SELECT name AS product_name, price AS unit_price
FROM products;
```

| product_name | unit_price |
| ------------ | ---------- |
| Laptop       | 55000      |
| Mouse        | 500        |
| ...          | ...        |

### Alias for a calculation or function

```sql
SELECT name, price * quantity AS total_stock_value
FROM products;

SELECT COUNT(*) AS total_products FROM products;
```

### Alias with spaces

Use quotes (or square brackets in SQL Server).

```sql
SELECT name AS "Product Name" FROM products;     -- standard / PostgreSQL
SELECT name AS [Product Name] FROM products;     -- SQL Server
SELECT name AS `Product Name` FROM products;     -- MySQL
```

### Table alias

Useful when a table name is long or when joining tables.

```sql
SELECT p.name, p.price
FROM products AS p
WHERE p.price > 1000;
```

```sql
SELECT s.name, d.dept_name
FROM students s
INNER JOIN departments d ON s.dept_id = d.dept_id;
```

> `AS` is optional (`products p` works too), but writing it makes the query clearer.

---

## Summary

- **Aggregate functions** (`MIN`, `MAX`, `COUNT`, `SUM`, `AVG`) turn many rows into one value and **ignore `NULL`** (except `COUNT(*)`).
- **`LIKE`** searches text patterns, using **wildcards**: `%` (many characters) and `_` (one character). `[ ]` works in SQL Server.
- **`IN`** matches any value in a list, and `NOT IN` excludes them.
- **`BETWEEN`** selects an **inclusive** range, for numbers, text, and dates.
- **Aliases** (`AS`) give columns and tables temporary, readable names.
