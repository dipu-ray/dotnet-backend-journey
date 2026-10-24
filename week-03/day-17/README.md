# Topic: Demo Project Understanding and SQL Recap

**Date:** 24 October 2026

Today's work:

1. **Demo project understanding:** studying a demo project to understand how it is built and how it works.
2. **SQL recap:** a quick revision of the basic SQL topics, from `SQL Intro` to `SQL Aliases`.

> **Note:** I do not have the details of the demo project, so Part 1 is a **ready-to-fill template** with a method for understanding a project. Replace the `[ ... ]` placeholders with the real information.

## Table of Contents

- [Part 1: Demo Project Understanding](#part-1-demo-project-understanding)
- [Part 2: SQL Recap](#part-2-sql-recap)
- [Cheat Sheet](#cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# Part 1: Demo Project Understanding

## 1. Project Overview

| Field        | Details                                       |
| ------------ | --------------------------------------------- |
| Project name | [Name]                                        |
| Purpose      | [What problem it solves, in one or two lines] |
| Type         | [Web app / Web API / Desktop / Mobile]        |
| Users        | [Admin, staff, customer, ...]                 |
| Technology   | [For example ASP.NET Core, C#, SQL Server]    |
| Database     | [SQL Server / MySQL / PostgreSQL]             |
| Shared by    | [Name / team]                                 |
| Date studied | 24 October 2026                               |

## 2. How I Approached Understanding the Project

| Step | What to do                                                                             | Done |
| ---- | -------------------------------------------------------------------------------------- | ---- |
| 1    | Read the **README / documentation** or ask for a short explanation                     | [ ]  |
| 2    | Understand the **business purpose**: who uses it and why                               | [ ]  |
| 3    | Look at the **database**: tables, columns, relationships                               | [ ]  |
| 4    | **Run** the project and click through the main screens / endpoints                     | [ ]  |
| 5    | **Trace one feature** from start to end (see below)                                    | [ ]  |
| 6    | Write down **questions** and ask the team                                              | [ ]  |
| 7    | Make a small **change** (for example add a field or fix a label) to test understanding | [ ]  |

## 3. Project Structure

Typical layout of a **.NET backend** project (adjust to the real project):

```
ProjectName/
├── Controllers/     -> receive requests (API endpoints)
├── Models/          -> data classes (entities, DTOs)
├── Services/        -> business logic
├── Data/            -> database context, repositories
├── appsettings.json -> configuration (connection string, settings)
└── Program.cs       -> application start-up
```

| Folder / file      | Purpose in this project       |
| ------------------ | ----------------------------- |
| [Folder 1]         | [Purpose]                     |
| [Folder 2]         | [Purpose]                     |
| `appsettings.json` | [Connection string, settings] |
| `Program.cs`       | [Start-up and services]       |

## 4. Modules and Features

| #   | Module / Feature    | What it does  | Main tables        |
| --- | ------------------- | ------------- | ------------------ |
| 1   | [Login / Users]     | [Description] | [Users]            |
| 2   | [Customers]         | [Description] | [Customers]        |
| 3   | [Orders / Invoices] | [Description] | [Orders, Invoices] |
| 4   | [Reports]           | [Description] | [..]               |

## 5. Database Understanding

Explore the database first:

```sql
-- List all tables
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- See the columns of one table
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Customers';

-- Peek at the data
SELECT TOP 10 * FROM Customers;
```

| Table     | Purpose          | Key columns                   |
| --------- | ---------------- | ----------------------------- |
| [Table 1] | [What it stores] | [Id (PK), ...]                |
| [Table 2] | [What it stores] | [Id (PK), Table1Id (FK), ...] |

**Relationships**

```
[Table1] (1) ----< [Table2] (many)
```

## 6. Following One Feature from Start to End

Pick one feature (for example "Create a customer") and trace it:

```
User action  ->  Controller / API  ->  Service (logic)  ->  SQL / Database  ->  Response to user
```

| Layer          | What happens                      | File / object             |
| -------------- | --------------------------------- | ------------------------- |
| User interface | [Form / button / Postman request] | [..]                      |
| Controller     | [Receives the request]            | [..]                      |
| Service        | [Validates and processes]         | [..]                      |
| Database       | [INSERT / SELECT / UPDATE]        | [Table, stored procedure] |
| Response       | [Data returned to the user]       | [..]                      |

## 7. What I Understood

- [Point 1]
- [Point 2]
- [Point 3]

## 8. Questions to Ask

- [ ] [Question 1]
- [ ] [Question 2]

## 9. Next Steps

- [ ] [Run the project locally and test each main feature]
- [ ] [Read the stored procedures and queries used by the project]
- [ ] [Pick a small task to work on]

---

# Part 2: SQL Recap

## Sample Table: `products`

| id  | name     | category    | price | quantity |
| --- | -------- | ----------- | ----- | -------- |
| 1   | Laptop   | Electronics | 55000 | 5        |
| 2   | Mouse    | Electronics | 500   | 50       |
| 3   | Keyboard | Electronics | 1200  | 30       |
| 4   | Notebook | Stationery  | 80    | 200      |
| 5   | Pen      | Stationery  | 10    | 500      |
| 6   | Chair    | Furniture   | 4500  | NULL     |

---

## SQL Home and SQL Intro

**SQL** (Structured Query Language) is the standard language to **store, read, change, and delete data** in a **relational database**.

- Data is stored in **tables** made of **rows** (records) and **columns** (fields).
- SQL is used by databases such as **SQL Server, MySQL, PostgreSQL, and Oracle**.
- The basic operations are called **CRUD**:

| CRUD       | SQL command   |
| ---------- | ------------- |
| **C**reate | `INSERT INTO` |
| **R**ead   | `SELECT`      |
| **U**pdate | `UPDATE`      |
| **D**elete | `DELETE`      |

## SQL Syntax

- A database has **tables**, and SQL commands are written as **statements**.
- Keywords are **not case sensitive** (`select` = `SELECT`), but UPPERCASE is the common style.
- A statement ends with a semicolon `;` (required in some databases when running many statements).
- Text values use **single quotes** (`'Dhaka'`), numbers do not.
- Comments: `-- single line` and `/* multi-line */`.

Order of the clauses in a `SELECT` statement:

```sql
SELECT   columns
FROM     table
WHERE    condition          -- filter rows
GROUP BY columns            -- group rows
HAVING   condition          -- filter groups
ORDER BY columns;           -- sort result
```

## SQL Select

Reads data from a table.

```sql
SELECT * FROM products;               -- all columns
SELECT name, price FROM products;     -- specific columns
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

| Operator          | Meaning    |
| ----------------- | ---------- |
| `=`               | Equal      |
| `<>` or `!=`      | Not equal  |
| `>` `<` `>=` `<=` | Comparison |

## SQL Order By

Sorts the result: `ASC` (small to large, default) or `DESC` (large to small).

```sql
SELECT name, price FROM products ORDER BY price DESC;
SELECT name, price FROM products ORDER BY category ASC, price DESC;
```

## SQL And, Or, Not

```sql
-- AND: both conditions must be true
SELECT * FROM products WHERE category = 'Electronics' AND price < 1000;      -- Mouse

-- OR: at least one condition must be true
SELECT * FROM products WHERE category = 'Furniture' OR price < 50;           -- Pen, Chair

-- NOT: reverses a condition
SELECT * FROM products WHERE NOT category = 'Electronics';                   -- Notebook, Pen, Chair
```

> `AND` runs before `OR`. Use parentheses `()` to make the order clear.

## SQL Insert Into

Adds a new row.

```sql
INSERT INTO products (id, name, category, price, quantity)
VALUES (7, 'Monitor', 'Electronics', 15000, 10);
```

Insert several rows:

```sql
INSERT INTO products (id, name, category, price, quantity) VALUES
(8, 'Desk', 'Furniture', 7000, 4),
(9, 'Marker', 'Stationery', 40, 120);
```

## SQL Null Values

`NULL` means **no value** (it is not `0` and not an empty string). Use `IS NULL` and `IS NOT NULL`. Never use `= NULL`.

```sql
SELECT name FROM products WHERE quantity IS NULL;       -- Chair
SELECT name FROM products WHERE quantity IS NOT NULL;
```

## SQL Update

Changes existing rows.

```sql
UPDATE products
SET price = 600, quantity = 45
WHERE id = 2;
```

## SQL Delete

Removes rows.

```sql
DELETE FROM products WHERE id = 9;
```

> **Always use `WHERE` with `UPDATE` and `DELETE`.** Without it, **all rows** are changed or deleted. Test with a `SELECT ... WHERE` first.

## SQL Select Top

Limits the number of rows returned. Usually used with `ORDER BY`.

```sql
-- SQL Server
SELECT TOP 3 name, price FROM products ORDER BY price DESC;

-- MySQL and PostgreSQL
SELECT name, price FROM products ORDER BY price DESC LIMIT 3;
```

Result: Laptop, Chair, Keyboard

---

## SQL Aggregate Functions

An **aggregate function** works on **many rows** and returns **one value**. They **ignore `NULL`** (except `COUNT(*)`).

| Function  | Returns        | Example                               | Result  |
| --------- | -------------- | ------------------------------------- | ------- |
| `MIN()`   | Smallest value | `SELECT MIN(price) FROM products;`    | `10`    |
| `MAX()`   | Largest value  | `SELECT MAX(price) FROM products;`    | `55000` |
| `COUNT()` | Number of rows | `SELECT COUNT(*) FROM products;`      | `6`     |
| `SUM()`   | Total          | `SELECT SUM(quantity) FROM products;` | `785`   |
| `AVG()`   | Average        | `SELECT AVG(price) FROM products;`    | `10215` |

### SQL Min() and Max()

```sql
SELECT MIN(price) AS cheapest, MAX(price) AS most_expensive
FROM products;
```

| cheapest | most_expensive |
| -------- | -------------- |
| 10       | 55000          |

### SQL Count()

```sql
SELECT COUNT(*) FROM products;                   -- 6 (all rows)
SELECT COUNT(quantity) FROM products;            -- 5 (Chair has NULL)
SELECT COUNT(DISTINCT category) FROM products;   -- 3
```

### SQL Sum()

```sql
SELECT SUM(price * quantity) AS total_stock_value
FROM products;
```

### SQL Avg()

```sql
SELECT AVG(price) FROM products WHERE category = 'Electronics';
```

Result: `18900` (56700 / 3)

---

## SQL Like and Wildcards

`LIKE` searches a **pattern** in text. Wildcards:

| Wildcard | Meaning                          | Database   |
| -------- | -------------------------------- | ---------- |
| `%`      | Any number of characters         | All        |
| `_`      | Exactly one character            | All        |
| `[ ]`    | One character from a set         | SQL Server |
| `[^ ]`   | One character **not** in the set | SQL Server |

```sql
SELECT name FROM products WHERE name LIKE 'P%';      -- starts with P: Pen
SELECT name FROM products WHERE name LIKE '%book';   -- ends with book: Notebook
SELECT name FROM products WHERE name LIKE '%o%';     -- contains o: Laptop, Mouse, Keyboard, Notebook
SELECT name FROM products WHERE name LIKE '_en';     -- one letter + en: Pen
SELECT name FROM products WHERE name LIKE '[LM]%';   -- SQL Server: starts with L or M: Laptop, Mouse
```

## SQL In

Matches **any value in a list** (a short form of many `OR`).

```sql
SELECT * FROM products WHERE category IN ('Electronics', 'Furniture');
SELECT * FROM products WHERE category NOT IN ('Electronics', 'Furniture');   -- Notebook, Pen
```

## SQL Between

Matches a **range**. Both ends are **included**.

```sql
SELECT * FROM products WHERE price BETWEEN 500 AND 5000;       -- Mouse, Keyboard, Chair
SELECT * FROM products WHERE price NOT BETWEEN 500 AND 5000;   -- Laptop, Notebook, Pen
```

> Put the smaller value first. For `DATETIME` columns, use `>= start AND < next day` instead of `BETWEEN` to avoid missing the last day.

## SQL Aliases

A **temporary name** for a column or a table, used only in that query.

```sql
SELECT name AS product_name,
       price * quantity AS stock_value
FROM products AS p;
```

With spaces in the alias:

```sql
SELECT name AS "Product Name" FROM products;     -- standard / PostgreSQL
SELECT name AS [Product Name] FROM products;     -- SQL Server
```

---

## Cheat Sheet

| Topic                   | One-line meaning                           |
| ----------------------- | ------------------------------------------ |
| SQL                     | Language to work with relational databases |
| `SELECT`                | Read data                                  |
| `SELECT DISTINCT`       | Read unique values                         |
| `WHERE`                 | Filter rows                                |
| `ORDER BY`              | Sort (`ASC` / `DESC`)                      |
| `AND / OR / NOT`        | Combine or reverse conditions              |
| `INSERT INTO`           | Add rows                                   |
| `NULL`                  | No value, test with `IS NULL`              |
| `UPDATE`                | Change rows                                |
| `DELETE`                | Remove rows                                |
| `TOP` / `LIMIT`         | Limit the number of rows                   |
| `MIN MAX COUNT SUM AVG` | One value from many rows                   |
| `LIKE` + wildcards      | Search a text pattern (`%`, `_`)           |
| `IN`                    | Match any value in a list                  |
| `BETWEEN`               | Match a range (inclusive)                  |
| Alias (`AS`)            | Temporary name                             |

---

## Practice Tasks

Use the `products` table above.

1. Select all products, then only `name` and `price`.
2. List the unique categories.
3. Find products with a price above 1000, sorted from the highest price to the lowest.
4. Find Electronics products that cost less than 1000, using `AND`.
5. Find products in Furniture or Stationery using `IN`, then again using `OR`.
6. Insert two new products, update one price, and delete one product.
7. Find products whose quantity is `NULL`.
8. Show the 3 most expensive products using `TOP` (or `LIMIT`).
9. Find the cheapest price, the highest price, the number of products, the total quantity, and the average price in one query, using aliases.
10. Find products whose name starts with `K` or contains `en`, using `LIKE`.

---

## Summary

- **Demo project:** understand it by reading the documentation, studying the **database**, running it, and **tracing one feature** from the user action to the database and back. Write down questions.
- **SQL basics:** `SELECT` reads, `INSERT INTO` adds, `UPDATE` changes, `DELETE` removes. Filter with `WHERE`, sort with `ORDER BY`, and limit rows with `TOP` / `LIMIT`.
- **Conditions:** `AND`, `OR`, `NOT`, `IN`, `BETWEEN`, `LIKE`, and `IS NULL`.
- **Aggregates:** `MIN`, `MAX`, `COUNT`, `SUM`, `AVG` return one value from many rows and ignore `NULL`.
- **Aliases** (`AS`) give columns and tables readable temporary names.
- Always use `WHERE` with `UPDATE` and `DELETE`.
