# SQL Basics: SELECT, WHERE, INSERT INTO, UPDATE, DELETE

**Date:** 6 October 2026

Notes on the five core SQL commands with runnable examples. The syntax works in MySQL, PostgreSQL, and SQL Server, with small differences noted where needed.

## Table of Contents

- [What is SQL?](#what-is-sql)
- [Sample Table](#sample-table)
- [SELECT](#select)
- [WHERE](#where)
- [INSERT INTO](#insert-into)
- [UPDATE](#update)
- [DELETE](#delete)
- [Safe Practice Tips](#safe-practice-tips)
- [Quick Reference](#quick-reference)
- [Summary](#summary)
- [Problem Solving](#problem-solving)

---

## What is SQL?

**SQL** (Structured Query Language) is the language used to **store, read, change, and remove data** in a relational database. Data is kept in **tables** made of **rows** (records) and **columns** (fields).

The five commands covered here are the core of **CRUD**:

| CRUD       | SQL Command   | Purpose                      |
| ---------- | ------------- | ---------------------------- |
| **R**ead   | `SELECT`      | Get data from a table        |
| (filter)   | `WHERE`       | Choose which rows to work on |
| **C**reate | `INSERT INTO` | Add new rows                 |
| **U**pdate | `UPDATE`      | Change existing rows         |
| **D**elete | `DELETE`      | Remove rows                  |

> SQL keywords are not case sensitive (`select` = `SELECT`), but writing them in UPPERCASE is the common convention. Every statement ends with a semicolon `;`.

---

## Sample Table

All examples use this `students` table.

```sql
CREATE TABLE students (
    id         INT PRIMARY KEY,
    name       VARCHAR(50),
    department VARCHAR(20),
    age        INT,
    cgpa       DECIMAL(3, 2),
    city       VARCHAR(30)
);

INSERT INTO students (id, name, department, age, cgpa, city) VALUES
(1, 'Rahim',  'CSE', 20, 3.50, 'Dhaka'),
(2, 'Karim',  'EEE', 21, 3.20, 'Chattogram'),
(3, 'Nusrat', 'CSE', 19, 3.80, 'Dhaka'),
(4, 'Sadia',  'BBA', 22, 3.00, 'Sylhet'),
(5, 'Tanvir', 'CSE', 21, NULL, 'Khulna');
```

| id  | name   | department | age | cgpa | city       |
| --- | ------ | ---------- | --- | ---- | ---------- |
| 1   | Rahim  | CSE        | 20  | 3.50 | Dhaka      |
| 2   | Karim  | EEE        | 21  | 3.20 | Chattogram |
| 3   | Nusrat | CSE        | 19  | 3.80 | Dhaka      |
| 4   | Sadia  | BBA        | 22  | 3.00 | Sylhet     |
| 5   | Tanvir | CSE        | 21  | NULL | Khulna     |

---

## SELECT

`SELECT` **reads data** from a table. It does not change anything.

**Syntax**

```sql
SELECT column1, column2
FROM table_name;
```

### Select all columns

`*` means "all columns".

```sql
SELECT * FROM students;
```

Returns the whole table.

### Select specific columns

```sql
SELECT name, department
FROM students;
```

| name   | department |
| ------ | ---------- |
| Rahim  | CSE        |
| Karim  | EEE        |
| Nusrat | CSE        |
| Sadia  | BBA        |
| Tanvir | CSE        |

### Column alias (`AS`)

Gives a column a temporary, friendlier name in the result.

```sql
SELECT name AS student_name, cgpa AS result
FROM students;
```

### Remove duplicates (`DISTINCT`)

```sql
SELECT DISTINCT department
FROM students;
```

| department |
| ---------- |
| CSE        |
| EEE        |
| BBA        |

### Calculated columns

```sql
SELECT name, age, age + 1 AS age_next_year
FROM students;
```

> Avoid `SELECT *` in real applications. Select only the columns you need.

---

## WHERE

`WHERE` **filters rows** by a condition. Only rows where the condition is true are returned (or changed, when used with `UPDATE` / `DELETE`).

**Syntax**

```sql
SELECT columns
FROM table_name
WHERE condition;
```

### Comparison operators

| Operator     | Meaning                  |
| ------------ | ------------------------ |
| `=`          | Equal to                 |
| `<>` or `!=` | Not equal to             |
| `>`          | Greater than             |
| `<`          | Less than                |
| `>=`         | Greater than or equal to |
| `<=`         | Less than or equal to    |

```sql
SELECT * FROM students WHERE department = 'CSE';
```

| id  | name   | department | age | cgpa | city   |
| --- | ------ | ---------- | --- | ---- | ------ |
| 1   | Rahim  | CSE        | 20  | 3.50 | Dhaka  |
| 3   | Nusrat | CSE        | 19  | 3.80 | Dhaka  |
| 5   | Tanvir | CSE        | 21  | NULL | Khulna |

```sql
SELECT name, age FROM students WHERE age >= 21;
```

| name   | age |
| ------ | --- |
| Karim  | 21  |
| Sadia  | 22  |
| Tanvir | 21  |

> Text values go in **single quotes** (`'CSE'`). Numbers do not need quotes. SQL uses a single `=` for comparison.

### Logical operators: AND, OR, NOT

```sql
-- Both conditions must be true
SELECT name FROM students
WHERE department = 'CSE' AND city = 'Dhaka';
-- Rahim, Nusrat

-- At least one condition must be true
SELECT name FROM students
WHERE department = 'EEE' OR department = 'BBA';
-- Karim, Sadia

-- Reverses a condition
SELECT name FROM students
WHERE NOT department = 'CSE';
-- Karim, Sadia
```

Use parentheses when mixing `AND` and `OR` (`AND` runs first):

```sql
SELECT name FROM students
WHERE (department = 'CSE' OR department = 'EEE') AND age > 20;
-- Karim, Tanvir
```

### BETWEEN

Inclusive range check.

```sql
SELECT name, age FROM students
WHERE age BETWEEN 20 AND 21;
-- Rahim (20), Karim (21), Tanvir (21)
```

### IN

Matches any value in a list. It is a shorter form of multiple `OR`.

```sql
SELECT name FROM students
WHERE city IN ('Dhaka', 'Sylhet');
-- Rahim, Nusrat, Sadia
```

### LIKE (pattern matching)

| Wildcard | Meaning                  |
| -------- | ------------------------ |
| `%`      | Any number of characters |
| `_`      | Exactly one character    |

```sql
SELECT name FROM students WHERE name LIKE 'S%';   -- starts with S: Sadia
SELECT name FROM students WHERE name LIKE '%m';   -- ends with m: Rahim, Karim
SELECT name FROM students WHERE name LIKE '%a%';  -- contains a: Rahim, Karim, Nusrat, Sadia, Tanvir
```

### IS NULL / IS NOT NULL

`NULL` means "no value". You **cannot** use `= NULL`. Use `IS NULL`.

```sql
SELECT name FROM students WHERE cgpa IS NULL;       -- Tanvir
SELECT name FROM students WHERE cgpa IS NOT NULL;   -- Rahim, Karim, Nusrat, Sadia
```

---

## INSERT INTO

`INSERT INTO` **adds new rows** to a table.

### Insert with column names (recommended)

```sql
INSERT INTO students (id, name, department, age, cgpa, city)
VALUES (6, 'Mitu', 'BBA', 20, 3.40, 'Rajshahi');
```

### Insert into specific columns only

Columns you skip get `NULL` (or their default value).

```sql
INSERT INTO students (id, name, department)
VALUES (7, 'Arif', 'EEE');
```

### Insert without column names

Values must match **all** columns, in the exact table order. This breaks easily if the table changes.

```sql
INSERT INTO students
VALUES (8, 'Lima', 'CSE', 19, 3.90, 'Dhaka');
```

### Insert multiple rows

```sql
INSERT INTO students (id, name, department, age, cgpa, city) VALUES
(9,  'Hasan', 'CSE', 22, 3.10, 'Dhaka'),
(10, 'Jui',   'BBA', 21, 3.60, 'Khulna');
```

### Rules

- The number of values must match the number of columns listed.
- Text and dates go in single quotes, numbers do not.
- A `PRIMARY KEY` value (like `id`) must be unique, or the insert fails.

---

## UPDATE

`UPDATE` **changes existing rows**.

**Syntax**

```sql
UPDATE table_name
SET column1 = value1, column2 = value2
WHERE condition;
```

### Update one row

```sql
UPDATE students
SET cgpa = 3.30
WHERE id = 5;
```

Tanvir's `cgpa` changes from `NULL` to `3.30`.

### Update multiple columns

```sql
UPDATE students
SET city = 'Dhaka', age = 22
WHERE name = 'Karim';
```

### Update multiple rows

```sql
UPDATE students
SET department = 'Computer Science'
WHERE department = 'CSE';
```

### Update using the current value

```sql
UPDATE students
SET age = age + 1;          -- no WHERE: increases age of EVERY student
```

### Warning: always use WHERE

```sql
UPDATE students SET city = 'Dhaka';   -- changes the city of ALL rows
```

Without `WHERE`, **every row** is updated.

---

## DELETE

`DELETE` **removes rows** from a table.

**Syntax**

```sql
DELETE FROM table_name
WHERE condition;
```

### Delete one row

```sql
DELETE FROM students
WHERE id = 4;
```

### Delete multiple rows

```sql
DELETE FROM students
WHERE department = 'BBA';
```

### Delete with a condition

```sql
DELETE FROM students
WHERE age < 20 OR cgpa IS NULL;
```

### Warning: always use WHERE

```sql
DELETE FROM students;   -- deletes ALL rows (the table structure remains)
```

### DELETE vs TRUNCATE vs DROP

| Command                   | What it does                                        |
| ------------------------- | --------------------------------------------------- |
| `DELETE FROM t WHERE ...` | Removes selected rows (or all rows without `WHERE`) |
| `TRUNCATE TABLE t`        | Removes all rows quickly, keeps the table           |
| `DROP TABLE t`            | Removes the **entire table** (rows and structure)   |

---

## Safe Practice Tips

1. **Test with `SELECT` first.** Before an `UPDATE` or `DELETE`, run the same `WHERE` in a `SELECT` to see exactly which rows will be affected.

   ```sql
   SELECT * FROM students WHERE department = 'BBA';   -- check first
   DELETE FROM students WHERE department = 'BBA';     -- then delete
   ```

2. **Filter by the primary key** (`WHERE id = 5`) when you want to change exactly one row.
3. **Use a transaction** so you can undo a mistake (supported in most databases):

   ```sql
   BEGIN;
   DELETE FROM students WHERE id = 4;
   -- check the result, then:
   ROLLBACK;   -- undo
   -- or COMMIT; to save permanently
   ```

4. **Take a backup** before big changes on real data.

---

## Quick Reference

| Task              | Command                                                |
| ----------------- | ------------------------------------------------------ |
| Read all data     | `SELECT * FROM students;`                              |
| Read some columns | `SELECT name, age FROM students;`                      |
| Filter rows       | `SELECT * FROM students WHERE age > 20;`               |
| Add a row         | `INSERT INTO students (id, name) VALUES (11, 'Rina');` |
| Change a row      | `UPDATE students SET age = 23 WHERE id = 1;`           |
| Remove a row      | `DELETE FROM students WHERE id = 1;`                   |

---

## Summary

- **`SELECT`** reads data; use column names instead of `*` when possible.
- **`WHERE`** filters rows using `=`, `>`, `<`, `AND`, `OR`, `NOT`, `BETWEEN`, `IN`, `LIKE`, `IS NULL`.
- **`INSERT INTO`** adds rows; list the column names for safety.
- **`UPDATE`** changes rows using `SET`; **always** add `WHERE`.
- **`DELETE`** removes rows; **always** add `WHERE`, and test with `SELECT` first.
- Text values use single quotes, and `NULL` is checked with `IS NULL`.

---

## Problem Solving

### Problem 1: Easy

Write a query to display only the `FirstName` and `GPA` of students whose age is 22 (`Age = 22`).

### Solution

```sql
SELECT FirstName, GPA
FROM Students
WHERE Age = 22;
```

### Problem 2: Easy

Suppose you have a table named `Products`:

| ProductID | ProductName   | Category    | Price  | Stock |
| --------- | ------------- | ----------- | ------ | ----- |
| 1         | iPhone 15     | Electronics | 120000 | 15    |
| 2         | Samsung S24   | Electronics | 95000  | 8     |
| 3         | Nike Shoes    | Clothing    | 8000   | 25    |
| 4         | HP Laptop     | Electronics | 65000  | 5     |
| 5         | Adidas Jacket | Clothing    | 6000   | 0     |

**Day 1 Challenge:** Write a query to display only the `ProductName` and `Price` of the products whose category is `'Electronics'`.

### Solution

```sql
SELECT ProductName, Price
FROM Products
WHERE Category = 'Electronics';
```

### Problem 3: Medium

Suppose you have a table named `Products`:

| ProductID | ProductName   | Category    | Price  | Stock |
| --------- | ------------- | ----------- | ------ | ----- |
| 1         | iPhone 15     | Electronics | 120000 | 15    |
| 2         | Samsung S24   | Electronics | 95000  | 8     |
| 3         | Nike Shoes    | Clothing    | 8000   | 25    |
| 4         | HP Laptop     | Electronics | 65000  | 5     |
| 5         | Adidas Jacket | Clothing    | 6000   | 0     |

**Medium Level Challenge:** The company manager asks you: "Show me the `ProductName` and `Stock` of the products whose category is `'Electronics'` and whose price is more than 80,000 Taka."

### Solution

```sql
SELECT ProductName, Stock
FROM Products
WHERE Category = 'Electronics' AND Price > 80000;
```
