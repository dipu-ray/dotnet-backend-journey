# SQL: ANY, ALL, SELECT INTO, INSERT INTO SELECT, CASE, NULL Functions, Stored Procedures, Comments, Operators

**Date:** 14 October 2026

## Table of Contents

- [Sample Table](#sample-table)
- [SQL ANY](#sql-any)
- [SQL ALL](#sql-all)
- [SQL SELECT INTO](#sql-select-into)
- [SQL INSERT INTO SELECT](#sql-insert-into-select)
- [SQL CASE](#sql-case)
- [SQL NULL Functions](#sql-null-functions)
- [SQL Stored Procedures](#sql-stored-procedures)
- [SQL Comments](#sql-comments)
- [SQL Operators](#sql-operators)
- [Summary](#summary)

---

## Sample Table

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

## SQL ANY

`ANY` compares a value with a **list of values from a subquery**. The condition is true if it is true for **at least one** value in the list.

```sql
SELECT name, price
FROM products
WHERE price > ANY (SELECT price FROM products WHERE category = 'Stationery');
```

The Stationery prices are `80` and `10`. `price > ANY` means "greater than at least one of them", which is the same as "greater than `10`".

| name     | price |
| -------- | ----- |
| Laptop   | 55000 |
| Mouse    | 500   |
| Keyboard | 1200  |
| Notebook | 80    |
| Chair    | 4500  |

- Works with `=`, `<>`, `>`, `>=`, `<`, `<=`.
- `= ANY (...)` is the same as `IN (...)`.
- `SOME` is a synonym for `ANY` in some databases (for example, SQL Server).

---

## SQL ALL

`ALL` is true only if the condition is true for **every** value in the list.

```sql
SELECT name, price
FROM products
WHERE price > ALL (SELECT price FROM products WHERE category = 'Stationery');
```

`price > ALL` means "greater than both `80` and `10`", which is the same as "greater than `80`".

| name     | price |
| -------- | ----- |
| Laptop   | 55000 |
| Mouse    | 500   |
| Keyboard | 1200  |
| Chair    | 4500  |

### ANY vs ALL

|                     | `ANY`                                      | `ALL`                               |
| ------------------- | ------------------------------------------ | ----------------------------------- |
| True when           | Condition holds for **at least one** value | Condition holds for **every** value |
| `> ` means          | Greater than the **smallest** value        | Greater than the **largest** value  |
| `< ` means          | Less than the **largest** value            | Less than the **smallest** value    |
| Same example result | 5 rows                                     | 4 rows                              |

---

## SQL SELECT INTO

`SELECT INTO` **creates a new table** and copies data into it from an existing table.

```sql
-- SQL Server / PostgreSQL
SELECT * INTO products_backup
FROM products;
```

Copy only some columns and rows:

```sql
SELECT id, name, price
INTO electronics_products
FROM products
WHERE category = 'Electronics';
```

Copy only the **structure** (no data):

```sql
SELECT * INTO products_empty
FROM products
WHERE 1 = 0;        -- condition is never true, so no rows are copied
```

> The new table must **not already exist**.
> **MySQL does not support `SELECT INTO` for tables.** Use:

```sql
-- MySQL
CREATE TABLE products_backup AS
SELECT * FROM products;
```

---

## SQL INSERT INTO SELECT

`INSERT INTO SELECT` **copies rows into an existing table** from another table (or query).

```sql
-- Existing table
CREATE TABLE products_archive (
    id       INT,
    name     VARCHAR(50),
    category VARCHAR(30),
    price    DECIMAL(10, 2)
);

INSERT INTO products_archive (id, name, category, price)
SELECT id, name, category, price
FROM products
WHERE category = 'Stationery';
```

Result in `products_archive`:

| id  | name     | category   | price |
| --- | -------- | ---------- | ----- |
| 4   | Notebook | Stationery | 80    |
| 5   | Pen      | Stationery | 10    |

### Rules

- The **number of columns** and their **data types** must match.
- Existing rows in the target table are **not affected**.
- You can use `WHERE`, `JOIN`, and aggregate functions in the `SELECT` part.

### SELECT INTO vs INSERT INTO SELECT

|              | `SELECT INTO`                                    | `INSERT INTO SELECT`             |
| ------------ | ------------------------------------------------ | -------------------------------- |
| Target table | **Created** automatically                        | Must **already exist**           |
| Use for      | Quick backup / copy                              | Adding rows to an existing table |
| MySQL        | Not supported (use `CREATE TABLE ... AS SELECT`) | Supported                        |

---

## SQL CASE

`CASE` adds **if-then-else logic** inside a query. It checks conditions in order and returns the value of the **first true** condition.

```sql
CASE
    WHEN condition1 THEN result1
    WHEN condition2 THEN result2
    ELSE default_result
END
```

### Example: price label

```sql
SELECT name, price,
    CASE
        WHEN price >= 10000 THEN 'Expensive'
        WHEN price >= 1000  THEN 'Medium'
        ELSE 'Cheap'
    END AS price_level
FROM products;
```

| name     | price | price_level |
| -------- | ----- | ----------- |
| Laptop   | 55000 | Expensive   |
| Mouse    | 500   | Cheap       |
| Keyboard | 1200  | Medium      |
| Notebook | 80    | Cheap       |
| Pen      | 10    | Cheap       |
| Chair    | 4500  | Medium      |

### Simple CASE (compare one value)

```sql
SELECT name,
    CASE category
        WHEN 'Electronics' THEN 'E'
        WHEN 'Stationery'  THEN 'S'
        ELSE 'Other'
    END AS category_code
FROM products;
```

### CASE in ORDER BY

```sql
SELECT name, category
FROM products
ORDER BY
    CASE WHEN category = 'Electronics' THEN 0 ELSE 1 END,   -- Electronics first
    name;
```

### CASE in UPDATE

```sql
UPDATE products
SET price = CASE
                WHEN category = 'Stationery' THEN price * 1.10
                ELSE price
            END;
```

### Notes

- If no condition is true and there is **no `ELSE`**, the result is `NULL`.
- Conditions are checked **top to bottom**, so put the most specific one first.
- Always end with `END`.

---

## SQL NULL Functions

`NULL` means **no value / unknown**. It is **not** zero and **not** an empty string. Any calculation with `NULL` gives `NULL`.

```sql
SELECT name, price * quantity AS stock_value
FROM products;
```

| name     | stock_value |
| -------- | ----------- |
| Laptop   | 275000      |
| Mouse    | 25000       |
| Keyboard | 36000       |
| Notebook | 16000       |
| Pen      | 5000        |
| Chair    | **NULL**    |

### Check for NULL

```sql
SELECT name FROM products WHERE quantity IS NULL;       -- Chair
SELECT name FROM products WHERE quantity IS NOT NULL;
```

> `quantity = NULL` never works. Always use `IS NULL`.

### Replace NULL with a value

| Function              | Database           | Example                 |
| --------------------- | ------------------ | ----------------------- |
| `COALESCE(a, b, ...)` | **All** (standard) | `COALESCE(quantity, 0)` |
| `ISNULL(a, b)`        | SQL Server         | `ISNULL(quantity, 0)`   |
| `IFNULL(a, b)`        | MySQL              | `IFNULL(quantity, 0)`   |
| `NVL(a, b)`           | Oracle             | `NVL(quantity, 0)`      |

```sql
SELECT name, price * COALESCE(quantity, 0) AS stock_value
FROM products;
```

Now Chair shows `0` instead of `NULL`.

`COALESCE` returns the **first non-NULL** value and accepts many arguments:

```sql
SELECT COALESCE(NULL, NULL, 'Default');   -- Default
```

### NULLIF

`NULLIF(a, b)` returns `NULL` if `a` equals `b`, otherwise it returns `a`. It is useful to avoid **division by zero**.

```sql
SELECT 100 / NULLIF(0, 0);   -- NULL instead of an error
```

### NULL and aggregate functions

`SUM`, `AVG`, `MIN`, `MAX`, and `COUNT(column)` **ignore `NULL`**. `COUNT(*)` counts all rows.

---

## SQL Stored Procedures

A **stored procedure** is a **saved block of SQL code** that you can run again and again by name. It can take **parameters**.

### Why use them

- **Reuse** the same code
- Better **performance** (the database can reuse the execution plan)
- **Security**: users can run the procedure without direct table access
- Keeps logic in one place, so changes are easy

### SQL Server

```sql
CREATE PROCEDURE GetProductsByCategory
    @Category VARCHAR(30)
AS
BEGIN
    SELECT * FROM products
    WHERE category = @Category;
END;
```

Run it:

```sql
EXEC GetProductsByCategory @Category = 'Electronics';
```

Multiple parameters (with a default value):

```sql
CREATE PROCEDURE GetProductsByPrice
    @Category VARCHAR(30),
    @MinPrice DECIMAL(10, 2) = 0
AS
BEGIN
    SELECT * FROM products
    WHERE category = @Category AND price >= @MinPrice;
END;

EXEC GetProductsByPrice @Category = 'Electronics', @MinPrice = 1000;
```

A procedure that changes data:

```sql
CREATE PROCEDURE AddProduct
    @Id INT, @Name VARCHAR(50), @Category VARCHAR(30),
    @Price DECIMAL(10, 2), @Quantity INT
AS
BEGIN
    INSERT INTO products (id, name, category, price, quantity)
    VALUES (@Id, @Name, @Category, @Price, @Quantity);
END;

EXEC AddProduct 7, 'Monitor', 'Electronics', 15000, 10;
```

Change or remove:

```sql
ALTER PROCEDURE GetProductsByCategory ...   -- modify
DROP PROCEDURE GetProductsByCategory;        -- delete
```

### MySQL

```sql
DELIMITER //

CREATE PROCEDURE GetProductsByCategory(IN p_category VARCHAR(30))
BEGIN
    SELECT * FROM products
    WHERE category = p_category;
END //

DELIMITER ;

CALL GetProductsByCategory('Electronics');
```

> `DELIMITER` changes the statement ending so the `;` inside the procedure does not end the definition early.

### PostgreSQL

```sql
CREATE PROCEDURE add_stock(p_id INT, p_qty INT)
LANGUAGE SQL
AS $$
    UPDATE products SET quantity = COALESCE(quantity, 0) + p_qty WHERE id = p_id;
$$;

CALL add_stock(6, 10);
```

|                 | SQL Server  | MySQL            | PostgreSQL       |
| --------------- | ----------- | ---------------- | ---------------- |
| Run             | `EXEC name` | `CALL name(...)` | `CALL name(...)` |
| Parameter style | `@param`    | `IN p_param`     | `param`          |

---

## SQL Comments

Comments are **ignored by the database**. They explain the code or temporarily disable it.

### Single-line comment: `--`

```sql
-- This selects all products
SELECT * FROM products;

SELECT name, price   -- only two columns
FROM products;
```

### Multi-line comment: `/* ... */`

```sql
/*
   Report: expensive products
   Author: Rahim
   Date: 14 October 2026
*/
SELECT * FROM products WHERE price > 1000;
```

### Comment out part of a statement

```sql
SELECT name, /* price, */ category
FROM products;
```

### MySQL also supports `#`

```sql
# MySQL single-line comment
SELECT * FROM products;
```

> Use comments to explain **why**, not what is obvious. Keep them short and updated.

---

## SQL Operators

### Arithmetic operators

| Operator | Meaning            | Example  | Result                  |
| -------- | ------------------ | -------- | ----------------------- |
| `+`      | Add                | `10 + 3` | `13`                    |
| `-`      | Subtract           | `10 - 3` | `7`                     |
| `*`      | Multiply           | `10 * 3` | `30`                    |
| `/`      | Divide             | `10 / 3` | `3` (integer) or `3.33` |
| `%`      | Modulo (remainder) | `10 % 3` | `1`                     |

### Comparison operators

| Operator     | Meaning                         |
| ------------ | ------------------------------- |
| `=`          | Equal to                        |
| `<>` or `!=` | Not equal to                    |
| `>` / `<`    | Greater / less than             |
| `>=` / `<=`  | Greater / less than or equal to |

### Logical operators

| Operator      | Meaning                      | Example                                    |
| ------------- | ---------------------------- | ------------------------------------------ |
| `AND`         | Both conditions true         | `price > 100 AND quantity > 10`            |
| `OR`          | At least one true            | `category = 'Furniture' OR price < 50`     |
| `NOT`         | Reverses a condition         | `NOT category = 'Electronics'`             |
| `BETWEEN`     | Within an inclusive range    | `price BETWEEN 100 AND 5000`               |
| `IN`          | Matches a list               | `category IN ('Electronics', 'Furniture')` |
| `LIKE`        | Pattern match                | `name LIKE 'P%'`                           |
| `IS NULL`     | Value is NULL                | `quantity IS NULL`                         |
| `EXISTS`      | Subquery returns a row       | `EXISTS (SELECT ...)`                      |
| `ANY` / `ALL` | Compare with a subquery list | `price > ANY (SELECT ...)`                 |

### Compound assignment operators (SQL Server)

| Operator | Same as     |
| -------- | ----------- |
| `+=`     | `x = x + y` |
| `-=`     | `x = x - y` |
| `*=`     | `x = x * y` |
| `/=`     | `x = x / y` |
| `%=`     | `x = x % y` |

```sql
UPDATE products SET price += 100 WHERE id = 2;   -- SQL Server
```

### Bitwise operators

| Operator | Meaning     |
| -------- | ----------- |
| `&`      | Bitwise AND |
| `\|`     | Bitwise OR  |
| `^`      | Bitwise XOR |

### String concatenation

```sql
SELECT 'Hello' + ' ' + 'World';          -- SQL Server
SELECT 'Hello' || ' ' || 'World';        -- PostgreSQL, Oracle
SELECT CONCAT('Hello', ' ', 'World');    -- MySQL, SQL Server, PostgreSQL
```

### Operator precedence (high to low)

1. `()` parentheses
2. `*`, `/`, `%`
3. `+`, `-`
4. Comparison (`=`, `>`, `<`, ...)
5. `NOT`
6. `AND`
7. `OR`

> Use parentheses when mixing `AND` and `OR`.

---

## Summary

- **`ANY`** is true if the condition matches at least one subquery value. **`ALL`** needs a match with every value.
- **`SELECT INTO`** creates a new table from a query. **`INSERT INTO SELECT`** copies rows into an existing table.
- **`CASE`** gives if-then-else logic. Without `ELSE`, unmatched rows return `NULL`.
- **NULL functions**: `IS NULL`, `COALESCE` (all databases), `ISNULL` (SQL Server), `IFNULL` (MySQL), `NULLIF`.
- **Stored procedures** are saved SQL code with parameters. Run with `EXEC` (SQL Server) or `CALL` (MySQL, PostgreSQL).
- **Comments**: `--` for one line, `/* */` for many lines.
- **Operators**: arithmetic, comparison, and logical. Use parentheses to control the order.
