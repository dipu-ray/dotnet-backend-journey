# Business Management Software and SQL Recap

**Date:** 29 October 2026

Today's work:

1. **Business Management Software:** visiting and understanding the software, and how it can be adapted to **your own business type** (for example, **farm management**), and how to **maintain** it.
2. **SQL recap:** a revision of the SQL topics from `SQL Home` to `SQL Group By`.

> **Note:** I do not have the details of the software that was shown, so Part 1 mixes a **fill-in template** (`[ ... ]`, for what you observed) with **general knowledge** of how business management software works and how to adapt it to a farm. Replace the placeholders with what you actually saw.

## Table of Contents

- [Part 1: Business Management Software](#part-1-business-management-software)
  - [Visit Details](#1-visit-details)
  - [What is Business Management Software?](#2-what-is-business-management-software)
  - [How I Explored the Software](#4-how-i-explored-the-software)
  - [What I Learned, Questions, and Next Steps](#8-what-i-learned-questions-and-next-steps)
- [Part 2: SQL Recap](#part-2-sql-recap)
- [Cheat Sheet](#cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# Part 1: Business Management Software

## 1. Visit Details

| Field                      | Details                      |
| -------------------------- | ---------------------------- |
| Software name              | Business Management Software |
| Company                    | Technovate BD                |
| Visit date                 | 29 October 2026              |
| Shown by                   | Sr. Software Engineer        |
| Type                       | Web Application              |
| Technology                 | .NET                         |
| Business types it supports | SaaS Service                 |

## 2. What is Business Management Software?

Software that helps a business **record, manage, and report** its daily work in **one place**, instead of using paper, Excel, and separate tools. It is often called **ERP** (Enterprise Resource Planning) when it covers many areas.

| Without the software             | With the software                  |
| -------------------------------- | ---------------------------------- |
| Paper notebooks and Excel sheets | One central **database**           |
| Hard to find old records         | Quick **search** and **filters**   |
| Manual totals, frequent mistakes | **Automatic** calculations         |
| Reports take hours               | **Instant reports** and dashboards |
| Anyone can change anything       | **Users and roles** control access |

**Basic idea of every business software**

```
Master data (items, customers, suppliers)
        +
Transactions (purchase, sale, expense, stock movement)
        =
Reports (stock, profit, due, performance)
```

## 4. How I Explored the Software

| Step | What to do                                                                         | Done |
| ---- | ---------------------------------------------------------------------------------- | ---- |
| 1    | Log in as **admin** and look at the **dashboard** and menus                        | [ ]  |
| 2    | Open **Settings** to see how the business and company are configured               | [ ]  |
| 3    | Check the **master data**: items, customers, suppliers, categories                 | [ ]  |
| 4    | Add a **new item** and a **new customer**                                          | [ ]  |
| 5    | Create a **purchase**, then check the **stock** increases                          | [ ]  |
| 6    | Create a **sale / invoice**, then check the **stock** decreases                    | [ ]  |
| 7    | Record an **expense** and a **payment**                                            | [ ]  |
| 8    | Open the **reports** (stock, sales, profit) and compare them with what was entered | [ ]  |
| 9    | Look at **users and roles** and log in as a normal user to see the difference      | [ ]  |
| 10   | Find out where the **data** is stored and how **backup** is done                   | [ ]  |

## 8. What I Learned, Questions, and Next Steps

**What I learned**

- A business software has the **same core** (items, customers, suppliers, purchase, sale, stock, expense, reports) for most businesses.
- A different business type changes the **names, categories, and a few extra modules** (for a farm: fields, crops, harvest, livestock).
- Everything depends on the **database**, so SQL is the base skill.
- Customizable to fit specific business needs and workflows.
- Supports end-to-end management, production tracking, and maintenance of farm products.
- Provides seamless tracking and management of all **financial transactions**, including income and expenditure.

---

# Part 2: SQL Recap

## Sample Tables

**products**

| id  | name     | category    | price | quantity |
| --- | -------- | ----------- | ----- | -------- |
| 1   | Laptop   | Electronics | 55000 | 5        |
| 2   | Mouse    | Electronics | 500   | 50       |
| 3   | Keyboard | Electronics | 1200  | 30       |
| 4   | Notebook | Stationery  | 80    | 200      |
| 5   | Pen      | Stationery  | 10    | 500      |
| 6   | Chair    | Furniture   | 4500  | NULL     |

**departments**

| dept_id | dept_name |
| ------- | --------- |
| 1       | IT        |
| 2       | HR        |
| 3       | Finance   |
| 4       | Marketing |

**employees**

| emp_id | name   | dept_id | manager_id | salary |
| ------ | ------ | ------- | ---------- | ------ |
| 1      | Rahim  | 1       | NULL       | 90000  |
| 2      | Karim  | 1       | 1          | 60000  |
| 3      | Nusrat | 2       | 1          | 50000  |
| 4      | Sadia  | 3       | 1          | 55000  |
| 5      | Tanvir | NULL    | 2          | 40000  |
| 6      | Mitu   | 1       | 2          | 45000  |

---

## SQL Home and SQL Intro

**SQL** (Structured Query Language) is the standard language to **store, read, change, and delete data** in a **relational database**. Data lives in **tables** made of **rows** and **columns**.

| CRUD       | SQL command   |
| ---------- | ------------- |
| **C**reate | `INSERT INTO` |
| **R**ead   | `SELECT`      |
| **U**pdate | `UPDATE`      |
| **D**elete | `DELETE`      |

## SQL Syntax

- Keywords are not case sensitive. UPPERCASE is the common style.
- A statement ends with `;`.
- Text values use **single quotes**.
- Comments: `-- single line` and `/* multi-line */`.

Order of clauses in a `SELECT`:

```sql
SELECT   columns
FROM     table
WHERE    condition        -- filter rows
GROUP BY columns          -- group rows
HAVING   condition        -- filter groups
ORDER BY columns;         -- sort
```

## SQL Select

```sql
SELECT * FROM products;
SELECT name, price FROM products;
```

## SQL Select Distinct

Returns only unique values.

```sql
SELECT DISTINCT category FROM products;      -- Electronics, Stationery, Furniture
```

## SQL Where

```sql
SELECT * FROM products WHERE category = 'Electronics';
SELECT * FROM products WHERE price > 1000;
```

## SQL Order By

```sql
SELECT name, price FROM products ORDER BY price DESC;    -- large to small
SELECT name, price FROM products ORDER BY price ASC;     -- small to large (default)
```

## SQL And, Or, Not

```sql
SELECT * FROM products WHERE category = 'Electronics' AND price < 1000;   -- Mouse
SELECT * FROM products WHERE category = 'Furniture' OR price < 50;        -- Pen, Chair
SELECT * FROM products WHERE NOT category = 'Electronics';                -- Notebook, Pen, Chair
```

## SQL Insert Into

```sql
INSERT INTO products (id, name, category, price, quantity)
VALUES (7, 'Monitor', 'Electronics', 15000, 10);
```

## SQL Null Values

`NULL` means no value. Use `IS NULL` and `IS NOT NULL`, never `= NULL`.

```sql
SELECT name FROM products WHERE quantity IS NULL;       -- Chair
SELECT name FROM products WHERE quantity IS NOT NULL;
```

## SQL Update

```sql
UPDATE products SET price = 600 WHERE id = 2;
```

## SQL Delete

```sql
DELETE FROM products WHERE id = 7;
```

> **Always use `WHERE` with `UPDATE` and `DELETE`.**

## SQL Select Top

```sql
SELECT TOP 3 name, price FROM products ORDER BY price DESC;               -- SQL Server
SELECT name, price FROM products ORDER BY price DESC LIMIT 3;             -- MySQL / PostgreSQL
```

Result: Laptop, Chair, Keyboard

## SQL Aggregate Functions

Work on many rows and return **one value**. They ignore `NULL` (except `COUNT(*)`).

| Function  | Example                               | Result  |
| --------- | ------------------------------------- | ------- |
| `MIN()`   | `SELECT MIN(price) FROM products;`    | `10`    |
| `MAX()`   | `SELECT MAX(price) FROM products;`    | `55000` |
| `COUNT()` | `SELECT COUNT(*) FROM products;`      | `6`     |
| `SUM()`   | `SELECT SUM(quantity) FROM products;` | `785`   |
| `AVG()`   | `SELECT AVG(price) FROM products;`    | `10215` |

`COUNT(*)` gives `6`, but `COUNT(quantity)` gives `5` because Chair has `NULL`.

## SQL Like and Wildcards

| Wildcard | Meaning                               |
| -------- | ------------------------------------- |
| `%`      | Any number of characters              |
| `_`      | Exactly one character                 |
| `[ ]`    | One character from a set (SQL Server) |

```sql
SELECT name FROM products WHERE name LIKE 'P%';       -- Pen
SELECT name FROM products WHERE name LIKE '%book';    -- Notebook
SELECT name FROM products WHERE name LIKE '_en';      -- Pen
SELECT name FROM products WHERE name LIKE '[LM]%';    -- Laptop, Mouse (SQL Server)
```

## SQL In

```sql
SELECT * FROM products WHERE category IN ('Electronics', 'Furniture');
SELECT * FROM products WHERE category NOT IN ('Electronics', 'Furniture');
```

## SQL Between

Range, both ends included.

```sql
SELECT * FROM products WHERE price BETWEEN 500 AND 5000;      -- Mouse, Keyboard, Chair
```

## SQL Aliases

```sql
SELECT name AS product_name, price * quantity AS stock_value
FROM products AS p;
```

---

## SQL Joins

A **JOIN** combines rows from two tables using a **related column**.

| Join         | Returns                                               |
| ------------ | ----------------------------------------------------- |
| `INNER JOIN` | Only matching rows                                    |
| `LEFT JOIN`  | All rows from the left table + matches from the right |
| `RIGHT JOIN` | All rows from the right table + matches from the left |
| `FULL JOIN`  | All rows from both tables                             |
| `SELF JOIN`  | A table joined with itself                            |

Missing matches show as `NULL`.

### SQL Inner Join

```sql
SELECT e.name, d.dept_name
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id;
```

Result: Rahim-IT, Karim-IT, Nusrat-HR, Sadia-Finance, Mitu-IT. (Tanvir has no department, and Marketing has no employee, so they are left out.)

### SQL Left Join

```sql
SELECT e.name, d.dept_name
FROM employees e
LEFT JOIN departments d ON e.dept_id = d.dept_id;
```

All 6 employees. Tanvir shows `NULL` for the department.

### SQL Right Join

```sql
SELECT e.name, d.dept_name
FROM employees e
RIGHT JOIN departments d ON e.dept_id = d.dept_id;
```

All 4 departments. Marketing shows `NULL` for the employee.

### SQL Full Join

```sql
SELECT e.name, d.dept_name
FROM employees e
FULL JOIN departments d ON e.dept_id = d.dept_id;
```

Everything from both tables. (MySQL has no `FULL JOIN`. Use `LEFT JOIN UNION RIGHT JOIN`.)

### SQL Self Join

A table joined with itself, using **different aliases**.

```sql
SELECT e.name AS employee, m.name AS manager
FROM employees e
LEFT JOIN employees m ON e.manager_id = m.emp_id;
```

| employee | manager |
| -------- | ------- |
| Rahim    | NULL    |
| Karim    | Rahim   |
| Nusrat   | Rahim   |
| Sadia    | Rahim   |
| Tanvir   | Karim   |
| Mitu     | Karim   |

---

## SQL Union and Union All

Combine the results of two `SELECT` statements. They need the **same number of columns** and **compatible data types**.

| Customers: city | Suppliers: city |
| --------------- | --------------- |
| Dhaka           | Dhaka           |
| Chattogram      | Sylhet          |
| Dhaka           | Chattogram      |

```sql
SELECT city FROM customers
UNION
SELECT city FROM suppliers;        -- removes duplicates: Dhaka, Chattogram, Sylhet (3 rows)

SELECT city FROM customers
UNION ALL
SELECT city FROM suppliers;        -- keeps duplicates (6 rows), faster
```

|            | `UNION` | `UNION ALL` |
| ---------- | ------- | ----------- |
| Duplicates | Removed | **Kept**    |
| Speed      | Slower  | **Faster**  |

## SQL Group By

Groups rows with the same value, so aggregate functions work **per group**.

```sql
SELECT d.dept_name, COUNT(*) AS total, AVG(e.salary) AS avg_salary
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name;
```

| dept_name | total | avg_salary |
| --------- | ----- | ---------- |
| IT        | 3     | 65000      |
| HR        | 1     | 50000      |
| Finance   | 1     | 55000      |

- Every column in `SELECT` must be in `GROUP BY` or inside an aggregate function.
- `WHERE` filters rows **before** grouping. `HAVING` filters groups **after** grouping:

```sql
SELECT d.dept_name, COUNT(*) AS total
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name
HAVING COUNT(*) > 1;          -- IT | 3
```

---

## Cheat Sheet

| Topic                               | One-line meaning                          |
| ----------------------------------- | ----------------------------------------- |
| `SELECT` / `DISTINCT`               | Read data / unique values                 |
| `WHERE`                             | Filter rows                               |
| `ORDER BY`                          | Sort (`ASC` / `DESC`)                     |
| `AND / OR / NOT`                    | Combine or reverse conditions             |
| `INSERT INTO` / `UPDATE` / `DELETE` | Add / change / remove rows                |
| `NULL`                              | No value, test with `IS NULL`             |
| `TOP` / `LIMIT`                     | Limit rows                                |
| `MIN MAX COUNT SUM AVG`             | One value from many rows                  |
| `LIKE` + wildcards                  | Search a text pattern                     |
| `IN` / `BETWEEN`                    | Match a list / a range                    |
| Alias (`AS`)                        | Temporary name                            |
| `INNER JOIN`                        | Matching rows only                        |
| `LEFT / RIGHT / FULL JOIN`          | All rows from one side / both sides       |
| `SELF JOIN`                         | Table joined with itself                  |
| `UNION` / `UNION ALL`               | Combine results without / with duplicates |
| `GROUP BY`                          | Summarize per group                       |

---

## Summary

- **Business management software** keeps master data (items, customers, suppliers) and transactions (purchase, sale, expense, stock) in one database, and produces reports.
- Most business types share the **same core**. A different type (for example, a **farm**) changes the **names, categories, and a few extra modules** such as fields, crops, harvest, and livestock.
- To **maintain** it: enter data daily, **back up** regularly (and test a restore), control **users and roles**, check reports, and fix mistakes with proper corrections.
- **SQL** is the base skill: `SELECT`, `WHERE`, `ORDER BY`, aggregates, `JOIN`, `UNION`, and `GROUP BY` produce most business reports.
- Always use `WHERE` with `UPDATE` and `DELETE`, and use `LEFT JOIN` when you must keep rows that have no match.
