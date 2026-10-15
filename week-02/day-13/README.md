# Recap SQL (Structured Query Language)

**Date:** 15 October 2026

## Table of Contents
- [Sample Tables](#sample-tables)
- [Part 1: Basics](#part-1-basics)
- [Part 2: Aggregate Functions](#part-2-aggregate-functions)
- [Part 3: Filtering and Searching](#part-3-filtering-and-searching)
- [Part 4: Joins](#part-4-joins)
- [Part 5: UNION](#part-5-union)
- [Part 6: Grouping and Subqueries](#part-6-grouping-and-subqueries)
- [Part 7: Copying Data, CASE, and NULL Functions](#part-7-copying-data-case-and-null-functions)
- [Part 8: Stored Procedures, Comments, Operators, Views](#part-8-stored-procedures-comments-operators-views)
- [Cheat Sheet](#cheat-sheet)
- [Database Differences](#database-differences)

---

## Sample Tables

**products**

| id | name | category | price | quantity |
|----|------|----------|-------|----------|
| 1 | Laptop | Electronics | 55000 | 5 |
| 2 | Mouse | Electronics | 500 | 50 |
| 3 | Keyboard | Electronics | 1200 | 30 |
| 4 | Notebook | Stationery | 80 | 200 |
| 5 | Pen | Stationery | 10 | 500 |
| 6 | Chair | Furniture | 4500 | NULL |

**departments**

| dept_id | dept_name |
|---------|-----------|
| 1 | IT |
| 2 | HR |
| 3 | Finance |
| 4 | Marketing |

**employees**

| emp_id | name | dept_id | manager_id | salary |
|--------|------|---------|------------|--------|
| 1 | Rahim | 1 | NULL | 90000 |
| 2 | Karim | 1 | 1 | 60000 |
| 3 | Nusrat | 2 | 1 | 50000 |
| 4 | Sadia | 3 | 1 | 55000 |
| 5 | Tanvir | NULL | 2 | 40000 |
| 6 | Mitu | 1 | 2 | 45000 |

---

# Part 1: Basics

## SQL Select
Reads data from a table.
```sql
SELECT * FROM products;                -- all columns
SELECT name, price FROM products;      -- specific columns
```

## SQL Select Distinct
Returns only **different (unique)** values.
```sql
SELECT DISTINCT category FROM products;
```
Result: `Electronics`, `Stationery`, `Furniture`

## SQL Where
Filters rows by a condition.
```sql
SELECT * FROM products WHERE category = 'Electronics';
SELECT * FROM products WHERE price > 1000;
```

## SQL Order By
Sorts the result. `ASC` (default) = small to large, `DESC` = large to small.
```sql
SELECT name, price FROM products ORDER BY price DESC;
```
Result: Laptop, Chair, Keyboard, Mouse, Notebook, Pen

## SQL And, Or, Not
Combine or reverse conditions.
```sql
SELECT * FROM products WHERE category = 'Electronics' AND price < 1000;   -- both true: Mouse
SELECT * FROM products WHERE category = 'Furniture' OR price < 50;        -- either true: Pen, Chair
SELECT * FROM products WHERE NOT category = 'Electronics';                -- Notebook, Pen, Chair
```
`AND` runs before `OR`. Use `()` to be clear.

## SQL Insert Into
Adds a new row.
```sql
INSERT INTO products (id, name, category, price, quantity)
VALUES (7, 'Monitor', 'Electronics', 15000, 10);
```

## SQL Null Values
`NULL` means **no value** (not 0, not empty). Use `IS NULL` / `IS NOT NULL`, never `= NULL`.
```sql
SELECT name FROM products WHERE quantity IS NULL;       -- Chair
SELECT name FROM products WHERE quantity IS NOT NULL;
```

## SQL Update
Changes existing rows.
```sql
UPDATE products SET price = 600 WHERE id = 2;
```

## SQL Delete
Removes rows.
```sql
DELETE FROM products WHERE id = 7;
```
> **Always use `WHERE` with `UPDATE` and `DELETE`.** Without it, all rows are affected. Test with `SELECT` first.

## SQL Select Top
Limits how many rows are returned.
```sql
-- SQL Server
SELECT TOP 3 name, price FROM products ORDER BY price DESC;

-- MySQL / PostgreSQL
SELECT name, price FROM products ORDER BY price DESC LIMIT 3;
```
Result: Laptop, Chair, Keyboard

---

# Part 2: Aggregate Functions

An **aggregate function** works on many rows and returns **one value**. They ignore `NULL` (except `COUNT(*)`).

| Function | Returns | Example | Result |
|----------|---------|---------|--------|
| `MIN()` | Smallest value | `SELECT MIN(price) FROM products;` | `10` |
| `MAX()` | Largest value | `SELECT MAX(price) FROM products;` | `55000` |
| `COUNT()` | Number of rows | `SELECT COUNT(*) FROM products;` | `6` |
| `SUM()` | Total | `SELECT SUM(quantity) FROM products;` | `785` |
| `AVG()` | Average | `SELECT AVG(price) FROM products;` | `10215` |

`COUNT(*)` counts all rows (`6`). `COUNT(quantity)` skips `NULL` (`5`).

---

# Part 3: Filtering and Searching

## SQL Like and Wildcards
`LIKE` searches a pattern. Wildcards: `%` = any number of characters, `_` = exactly one character, `[ ]` = a set of characters (SQL Server).
```sql
SELECT name FROM products WHERE name LIKE 'P%';    -- starts with P: Pen
SELECT name FROM products WHERE name LIKE '%book'; -- ends with book: Notebook
SELECT name FROM products WHERE name LIKE '%o%';   -- contains o
SELECT name FROM products WHERE name LIKE '_en';   -- 3 letters ending in en: Pen
SELECT name FROM products WHERE name LIKE '[LM]%'; -- SQL Server: starts with L or M
```

## SQL In
Matches any value in a list (a short form of many `OR`).
```sql
SELECT * FROM products WHERE category IN ('Electronics', 'Furniture');
SELECT * FROM products WHERE category NOT IN ('Electronics', 'Furniture');
```

## SQL Between
Matches a range, **both ends included**.
```sql
SELECT * FROM products WHERE price BETWEEN 500 AND 5000;   -- Mouse, Keyboard, Chair
```

## SQL Aliases
A temporary name for a column or table.
```sql
SELECT name AS product_name, price * quantity AS stock_value
FROM products AS p;
```

---

# Part 4: Joins

A **JOIN** combines rows from two tables using a related column.

| Join | Returns |
|------|---------|
| `INNER JOIN` | Only rows that match in both tables |
| `LEFT JOIN` | All rows from the left table + matches from the right |
| `RIGHT JOIN` | All rows from the right table + matches from the left |
| `FULL JOIN` | All rows from both tables |
| `SELF JOIN` | A table joined with itself |

Missing matches show as `NULL`.

## SQL Inner Join
```sql
SELECT e.name, d.dept_name
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id;
```
Result: Rahim-IT, Karim-IT, Nusrat-HR, Sadia-Finance, Mitu-IT (Tanvir and Marketing are left out)

## SQL Left Join
```sql
SELECT e.name, d.dept_name
FROM employees e
LEFT JOIN departments d ON e.dept_id = d.dept_id;
```
All 6 employees. Tanvir shows `NULL` for `dept_name`.

## SQL Right Join
```sql
SELECT e.name, d.dept_name
FROM employees e
RIGHT JOIN departments d ON e.dept_id = d.dept_id;
```
All 4 departments. Marketing shows `NULL` for `name`.

## SQL Full Join
```sql
SELECT e.name, d.dept_name
FROM employees e
FULL JOIN departments d ON e.dept_id = d.dept_id;
```
Everything from both tables: Tanvir with `NULL` department, Marketing with `NULL` employee. (MySQL has no `FULL JOIN`. Use `LEFT JOIN UNION RIGHT JOIN`.)

## SQL Self Join
A table joined to itself, with aliases (for example, employee and manager).
```sql
SELECT e.name AS employee, m.name AS manager
FROM employees e
LEFT JOIN employees m ON e.manager_id = m.emp_id;
```
Karim's manager is Rahim. Rahim has no manager (`NULL`).

---

# Part 5: UNION

Combines the results of two `SELECT` statements. Both need the **same number of columns** and **compatible data types**.

## SQL Union
Combines results and **removes duplicates**.
```sql
SELECT city FROM customers
UNION
SELECT city FROM suppliers;
```

## SQL Union All
Combines results and **keeps duplicates** (faster).
```sql
SELECT city FROM customers
UNION ALL
SELECT city FROM suppliers;
```

---

# Part 6: Grouping and Subqueries

## SQL Group By
Groups rows with the same value, to use aggregate functions **per group**.
```sql
SELECT d.dept_name, COUNT(*) AS total, AVG(e.salary) AS avg_salary
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name;
```

| dept_name | total | avg_salary |
|-----------|-------|------------|
| IT | 3 | 65000 |
| HR | 1 | 50000 |
| Finance | 1 | 55000 |

## SQL Having
Filters **groups** after `GROUP BY`. (`WHERE` filters rows **before** grouping and cannot use aggregates.)
```sql
SELECT d.dept_name, COUNT(*) AS total
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name
HAVING COUNT(*) > 1;
```
Result: `IT | 3`

## SQL Exists
True if the subquery returns **at least one row**.
```sql
SELECT dept_name
FROM departments d
WHERE EXISTS (SELECT 1 FROM employees e WHERE e.dept_id = d.dept_id);
```
Result: IT, HR, Finance. Use `NOT EXISTS` for Marketing.

## SQL Any
True if the condition is true for **at least one** value from the subquery.
```sql
SELECT name FROM products
WHERE price > ANY (SELECT price FROM products WHERE category = 'Stationery');
-- greater than the smallest (10)
```

## SQL All
True only if the condition is true for **all** values from the subquery.
```sql
SELECT name FROM products
WHERE price > ALL (SELECT price FROM products WHERE category = 'Stationery');
-- greater than the largest (80)
```

---

# Part 7: Copying Data, CASE, and NULL Functions

## SQL Select Into
Creates a **new table** from a query.
```sql
SELECT * INTO products_backup FROM products;                    -- SQL Server / PostgreSQL
CREATE TABLE products_backup AS SELECT * FROM products;         -- MySQL
```

## SQL Insert Into Select
Copies rows into an **existing table**.
```sql
INSERT INTO products_archive (id, name, category, price)
SELECT id, name, category, price
FROM products
WHERE category = 'Stationery';
```

## SQL Case
If-then-else inside a query.
```sql
SELECT name, price,
    CASE
        WHEN price >= 10000 THEN 'Expensive'
        WHEN price >= 1000  THEN 'Medium'
        ELSE 'Cheap'
    END AS price_level
FROM products;
```
No `ELSE` and no match gives `NULL`.

## SQL Null Functions
Replace `NULL` with another value.

| Function | Database |
|----------|----------|
| `COALESCE(a, b)` | All |
| `ISNULL(a, b)` | SQL Server |
| `IFNULL(a, b)` | MySQL |
| `NVL(a, b)` | Oracle |

```sql
SELECT name, price * COALESCE(quantity, 0) AS stock_value FROM products;
```
`NULLIF(a, b)` returns `NULL` if `a = b`. For example, `100 / NULLIF(0, 0)` avoids a divide-by-zero error.

---

# Part 8: Stored Procedures, Comments, Operators, Views

## SQL Stored Procedures
A saved block of SQL you can run by name, with parameters.
```sql
-- SQL Server
CREATE PROCEDURE GetProductsByCategory @Category VARCHAR(30)
AS
BEGIN
    SELECT * FROM products WHERE category = @Category;
END;

EXEC GetProductsByCategory @Category = 'Electronics';
```
```sql
-- MySQL
DELIMITER //
CREATE PROCEDURE GetProductsByCategory(IN p_category VARCHAR(30))
BEGIN
    SELECT * FROM products WHERE category = p_category;
END //
DELIMITER ;

CALL GetProductsByCategory('Electronics');
```

## SQL Comments
Ignored by the database.
```sql
-- single-line comment
SELECT * FROM products;   -- inline comment

/* multi-line
   comment */
```
MySQL also supports `#`.

## SQL Operators

| Type | Operators |
|------|-----------|
| Arithmetic | `+`  `-`  `*`  `/`  `%` |
| Comparison | `=`  `<>` (or `!=`)  `>`  `<`  `>=`  `<=` |
| Logical | `AND`  `OR`  `NOT`  `BETWEEN`  `IN`  `LIKE`  `EXISTS`  `ANY`  `ALL`  `IS NULL` |
| Compound (SQL Server) | `+=`  `-=`  `*=`  `/=`  `%=` |
| Bitwise | `&`  `\|`  `^` |

## SQL Views
A **view** is a **saved `SELECT` query** that works like a **virtual table**. It stores no data, and always shows current data.

```sql
-- Create
CREATE VIEW it_employees AS
SELECT emp_id, name, dept_id, salary
FROM employees
WHERE dept_id = 1;

-- Use
SELECT * FROM it_employees;

-- Change (MySQL / PostgreSQL)      -- SQL Server: ALTER VIEW
CREATE OR REPLACE VIEW it_employees AS
SELECT emp_id, name, dept_id, salary
FROM employees
WHERE dept_id = 1 AND salary > 40000;

-- Delete
DROP VIEW it_employees;
```

Why use views: **simplify** complex queries (such as joins), **hide** sensitive columns, and **reuse** logic.
Simple views can be updated with `INSERT / UPDATE / DELETE`. Views with `GROUP BY`, aggregates, `DISTINCT`, or `UNION` cannot. `WITH CHECK OPTION` keeps changes inside the view's `WHERE` condition.

---

## Cheat Sheet

| Topic | One-line meaning |
|-------|------------------|
| `SELECT` | Read data |
| `SELECT DISTINCT` | Read unique values |
| `WHERE` | Filter rows |
| `ORDER BY` | Sort results |
| `AND / OR / NOT` | Combine or reverse conditions |
| `INSERT INTO` | Add rows |
| `NULL` | No value, test with `IS NULL` |
| `UPDATE` | Change rows |
| `DELETE` | Remove rows |
| `TOP` / `LIMIT` | Limit the number of rows |
| `MIN MAX COUNT SUM AVG` | Calculate one value from many rows |
| `LIKE` + wildcards | Search a text pattern (`%`, `_`) |
| `IN` | Match any value in a list |
| `BETWEEN` | Match a range (inclusive) |
| Alias (`AS`) | Temporary name |
| `INNER JOIN` | Matching rows only |
| `LEFT / RIGHT JOIN` | All rows from one side |
| `FULL JOIN` | All rows from both sides |
| `SELF JOIN` | Table joined with itself |
| `UNION` | Combine results, no duplicates |
| `UNION ALL` | Combine results, keep duplicates |
| `GROUP BY` | Summarize per group |
| `HAVING` | Filter groups |
| `EXISTS` | Check if a subquery returns a row |
| `ANY` | True for at least one value |
| `ALL` | True for every value |
| `SELECT INTO` | Copy data into a new table |
| `INSERT INTO SELECT` | Copy data into an existing table |
| `CASE` | If-then-else |
| `COALESCE / ISNULL / IFNULL` | Replace `NULL` |
| Stored procedure | Saved, reusable SQL with parameters |
| Comments | `--` and `/* */` |
| Operators | Arithmetic, comparison, logical |
| View | Saved query used as a virtual table |

---

## Database Differences

| Feature | SQL Server | MySQL | PostgreSQL |
|---------|-----------|-------|------------|
| Limit rows | `TOP n` | `LIMIT n` | `LIMIT n` |
| `FULL JOIN` | Yes | **No** (use `LEFT JOIN UNION RIGHT JOIN`) | Yes |
| `SELECT INTO` (new table) | Yes | **No** (use `CREATE TABLE AS`) | Yes |
| Replace `NULL` | `ISNULL`, `COALESCE` | `IFNULL`, `COALESCE` | `COALESCE` |
| Run a procedure | `EXEC` | `CALL` | `CALL` |
| Change a view | `ALTER VIEW` | `CREATE OR REPLACE VIEW` | `CREATE OR REPLACE VIEW` |
| `[ ]` wildcard in `LIKE` | Yes | No | No |
| `LIKE` case sensitivity | Insensitive | Insensitive | **Sensitive** (use `ILIKE`) |