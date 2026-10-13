# Topics: Practice SQL

**Date:** 13 October 2026

## Table of Contents

- [Sample Tables](#sample-tables)
- [SQL Joins](#sql-joins)
- [INNER JOIN](#inner-join)
- [LEFT JOIN](#left-join)
- [RIGHT JOIN](#right-join)
- [FULL JOIN](#full-join)
- [SELF JOIN](#self-join)
- [UNION](#union)
- [UNION ALL](#union-all)
- [GROUP BY](#group-by)
- [HAVING](#having)
- [EXISTS](#exists)
- [Summary](#summary)
- [Problem Solving](#problem-solving)

---

## Sample Tables

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

Things to notice:

- **Tanvir** has **no department** (`dept_id` is `NULL`).
- **Marketing** has **no employees**.
- `manager_id` points to another row in the **same table** (used for the self join).

```sql
CREATE TABLE departments (
    dept_id   INT PRIMARY KEY,
    dept_name VARCHAR(30)
);

CREATE TABLE employees (
    emp_id     INT PRIMARY KEY,
    name       VARCHAR(50),
    dept_id    INT,
    manager_id INT,
    salary     DECIMAL(10, 2)
);

INSERT INTO departments VALUES
(1, 'IT'), (2, 'HR'), (3, 'Finance'), (4, 'Marketing');

INSERT INTO employees VALUES
(1, 'Rahim',  1,    NULL, 90000),
(2, 'Karim',  1,    1,    60000),
(3, 'Nusrat', 2,    1,    50000),
(4, 'Sadia',  3,    1,    55000),
(5, 'Tanvir', NULL, 2,    40000),
(6, 'Mitu',   1,    2,    45000);
```

---

## SQL Joins

A **JOIN** combines rows from **two (or more) tables** using a **related column** (here `dept_id`).

```sql
SELECT columns
FROM table1
JOIN table2
    ON table1.column = table2.column;
```

| Join           | Returns                                                         |
| -------------- | --------------------------------------------------------------- |
| **INNER JOIN** | Only rows that **match** in both tables                         |
| **LEFT JOIN**  | **All rows from the left table** + matching rows from the right |
| **RIGHT JOIN** | **All rows from the right table** + matching rows from the left |
| **FULL JOIN**  | **All rows from both tables** (matched or not)                  |
| **SELF JOIN**  | A table joined **with itself**                                  |

> Where there is no match, the missing side shows `NULL`.

---

## INNER JOIN

Returns only the rows where the join condition is **true in both tables**.

```sql
SELECT e.name, d.dept_name
FROM employees e
INNER JOIN departments d
    ON e.dept_id = d.dept_id;
```

| name   | dept_name |
| ------ | --------- |
| Rahim  | IT        |
| Karim  | IT        |
| Nusrat | HR        |
| Sadia  | Finance   |
| Mitu   | IT        |

Tanvir (no department) and Marketing (no employees) are **not** in the result.

> `JOIN` alone means `INNER JOIN`.

---

## LEFT JOIN

Returns **all rows from the left table** (`employees`) and the matching rows from the right table. If there is no match, the right side is `NULL`.

```sql
SELECT e.name, d.dept_name
FROM employees e
LEFT JOIN departments d
    ON e.dept_id = d.dept_id;
```

| name   | dept_name |
| ------ | --------- |
| Rahim  | IT        |
| Karim  | IT        |
| Nusrat | HR        |
| Sadia  | Finance   |
| Tanvir | **NULL**  |
| Mitu   | IT        |

### Find rows with no match

```sql
SELECT e.name
FROM employees e
LEFT JOIN departments d
    ON e.dept_id = d.dept_id
WHERE d.dept_id IS NULL;
```

Result: `Tanvir` (employees without a department)

> `LEFT JOIN` is also written `LEFT OUTER JOIN`.

---

## RIGHT JOIN

Returns **all rows from the right table** (`departments`) and the matching rows from the left table.

```sql
SELECT e.name, d.dept_name
FROM employees e
RIGHT JOIN departments d
    ON e.dept_id = d.dept_id;
```

| name     | dept_name |
| -------- | --------- |
| Rahim    | IT        |
| Karim    | IT        |
| Mitu     | IT        |
| Nusrat   | HR        |
| Sadia    | Finance   |
| **NULL** | Marketing |

Marketing appears even though it has no employees. (Row order may vary.)

> A `RIGHT JOIN` can always be rewritten as a `LEFT JOIN` by swapping the table order. Most people use `LEFT JOIN`.

---

## FULL JOIN

Returns **all rows from both tables**. Matching rows are combined, and unmatched rows from either side get `NULL`.

```sql
SELECT e.name, d.dept_name
FROM employees e
FULL JOIN departments d
    ON e.dept_id = d.dept_id;
```

| name     | dept_name |
| -------- | --------- |
| Rahim    | IT        |
| Karim    | IT        |
| Mitu     | IT        |
| Nusrat   | HR        |
| Sadia    | Finance   |
| Tanvir   | **NULL**  |
| **NULL** | Marketing |

> Written as `FULL OUTER JOIN` too. **MySQL does not support `FULL JOIN`.** Use a `LEFT JOIN` combined with a `RIGHT JOIN` using `UNION`:

```sql
-- FULL JOIN in MySQL
SELECT e.name, d.dept_name
FROM employees e
LEFT JOIN departments d ON e.dept_id = d.dept_id
UNION
SELECT e.name, d.dept_name
FROM employees e
RIGHT JOIN departments d ON e.dept_id = d.dept_id;
```

---

## SELF JOIN

A table joined **with itself**. You must use **table aliases** so SQL can tell the two copies apart. It is used for hierarchies such as employee and manager.

```sql
SELECT e.name AS employee, m.name AS manager
FROM employees e
LEFT JOIN employees m
    ON e.manager_id = m.emp_id;
```

| employee | manager  |
| -------- | -------- |
| Rahim    | **NULL** |
| Karim    | Rahim    |
| Nusrat   | Rahim    |
| Sadia    | Rahim    |
| Tanvir   | Karim    |
| Mitu     | Karim    |

- `e` is the **employee** copy and `m` is the **manager** copy.
- Rahim has no manager (`NULL`). Use `INNER JOIN` to hide such rows.

---

## UNION

`UNION` combines the results of **two or more `SELECT` statements** into one result and **removes duplicates**.

**Rules**

- Each `SELECT` must have the **same number of columns**.
- Columns must have **compatible data types**, in the **same order**.
- Column names come from the **first** `SELECT`.

**Sample tables for UNION**

| customers: name | city       |
| --------------- | ---------- |
| Ayesha          | Dhaka      |
| Bashir          | Chattogram |
| Chitra          | Dhaka      |

| suppliers: name | city       |
| --------------- | ---------- |
| Delta Traders   | Dhaka      |
| Eastern Co      | Sylhet     |
| Faruk Ltd       | Chattogram |

```sql
SELECT city FROM customers
UNION
SELECT city FROM suppliers;
```

| city       |
| ---------- |
| Dhaka      |
| Chattogram |
| Sylhet     |

Duplicates are removed, so each city appears once. (Row order is not guaranteed. Add `ORDER BY` if needed.)

```sql
-- Combine names with a label
SELECT name, 'Customer' AS type FROM customers
UNION
SELECT name, 'Supplier' AS type FROM suppliers;
```

---

## UNION ALL

Same as `UNION`, but **keeps duplicates**. It is **faster** because it does not check for duplicates.

```sql
SELECT city FROM customers
UNION ALL
SELECT city FROM suppliers;
```

| city       |
| ---------- |
| Dhaka      |
| Chattogram |
| Dhaka      |
| Dhaka      |
| Sylhet     |
| Chattogram |

|                           | `UNION` | `UNION ALL` |
| ------------------------- | ------- | ----------- |
| Duplicates                | Removed | **Kept**    |
| Speed                     | Slower  | **Faster**  |
| Rows in the example above | 3       | 6           |

---

## GROUP BY

`GROUP BY` **groups rows with the same value** so you can use aggregate functions (`COUNT`, `SUM`, `AVG`, `MIN`, `MAX`) **per group**.

```sql
SELECT d.dept_name, COUNT(*) AS total_employees, AVG(e.salary) AS avg_salary
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name;
```

| dept_name | total_employees | avg_salary |
| --------- | --------------- | ---------- |
| IT        | 3               | 65000      |
| HR        | 1               | 50000      |
| Finance   | 1               | 55000      |

**Rules**

- Every column in `SELECT` must be **either in `GROUP BY` or inside an aggregate function**.
- `WHERE` comes **before** `GROUP BY`. `ORDER BY` comes **after**.

```sql
SELECT d.dept_name, SUM(e.salary) AS total_salary
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name
ORDER BY total_salary DESC;
```

| dept_name | total_salary |
| --------- | ------------ |
| IT        | 195000       |
| Finance   | 55000        |
| HR        | 50000        |

---

## HAVING

`HAVING` **filters groups** after `GROUP BY`. It is like `WHERE`, but for **aggregated results**.

```sql
SELECT d.dept_name, COUNT(*) AS total_employees
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name
HAVING COUNT(*) > 1;
```

| dept_name | total_employees |
| --------- | --------------- |
| IT        | 3               |

### WHERE vs HAVING

|                              | `WHERE`                   | `HAVING`                   |
| ---------------------------- | ------------------------- | -------------------------- |
| Filters                      | **Rows**, before grouping | **Groups**, after grouping |
| Can use aggregate functions? | No                        | **Yes**                    |
| Position                     | Before `GROUP BY`         | After `GROUP BY`           |

Using both:

```sql
SELECT d.dept_name, COUNT(*) AS total_employees
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
WHERE e.salary >= 50000          -- 1. filter rows first
GROUP BY d.dept_name             -- 2. then group
HAVING COUNT(*) >= 2;            -- 3. then filter groups
```

| dept_name | total_employees |
| --------- | --------------- |
| IT        | 2               |

Order of execution: `FROM / JOIN` -> `WHERE` -> `GROUP BY` -> `HAVING` -> `SELECT` -> `ORDER BY`.

---

## EXISTS

`EXISTS` checks whether a **subquery returns at least one row**. It returns `TRUE` or `FALSE`, and it stops as soon as it finds a match.

```sql
-- Departments that have at least one employee
SELECT d.dept_name
FROM departments d
WHERE EXISTS (
    SELECT 1
    FROM employees e
    WHERE e.dept_id = d.dept_id
);
```

| dept_name |
| --------- |
| IT        |
| HR        |
| Finance   |

### NOT EXISTS

```sql
-- Departments with no employees
SELECT d.dept_name
FROM departments d
WHERE NOT EXISTS (
    SELECT 1
    FROM employees e
    WHERE e.dept_id = d.dept_id
);
```

Result: `Marketing`

### Another example: employees who manage someone

```sql
SELECT m.name
FROM employees m
WHERE EXISTS (
    SELECT 1
    FROM employees e
    WHERE e.manager_id = m.emp_id
);
```

Result: `Rahim`, `Karim`

### Notes

- The subquery is **correlated**: it uses a column from the outer query (`d.dept_id`).
- `SELECT 1` is common because only the **existence** of a row matters, not its data.
- **`NOT IN` with `NULL` is dangerous.** `WHERE dept_id NOT IN (SELECT dept_id FROM employees)` returns **no rows** because the subquery contains `NULL` (Tanvir). `NOT EXISTS` works correctly.

---

## Summary

- **JOIN** combines tables using a related column. `INNER` = matches only, `LEFT` = all left rows, `RIGHT` = all right rows, `FULL` = all rows from both. **SELF JOIN** joins a table to itself using aliases.
- **UNION** merges `SELECT` results and removes duplicates. **UNION ALL** keeps duplicates and is faster.
- **GROUP BY** groups rows so aggregate functions work per group.
- **HAVING** filters groups. **WHERE** filters rows before grouping.
- **EXISTS** checks if a subquery returns any row. Prefer `NOT EXISTS` over `NOT IN` when `NULL` values are possible.

---

## Problem Solving

This repository contains practice problems on key SQL concepts (Self Join, Parent-Child Relationship, BETWEEN, EXISTS) along with their solutions. Each problem includes a description, table and data setup, the solution code, the expected output, and an explanation of how the code works.

## Table of Contents

1. [Self Join (Manager & Employee Relationship)](#1-self-join-manager--employee-relationship)
2. [Parent & Child Relationship](#2-parent--child-relationship)
3. [Filter with Date Range (BETWEEN)](#3-filter-with-date-range-between)
4. [EXISTS Operator with Dummy Select (`SELECT 5`)](#8-exists-operator-with-dummy-select-select-5)

---

## 1. Self Join (Manager & Employee Relationship)

### Problem

An `Employees` table stores each employee's `EmployeeID`, `EmployeeName`, and the ID of their manager in `ManagerID`. A manager is also an employee, so the manager's details live in the same table.

**Task:** Show each employee's name alongside their manager's name. Employees with no manager (`ManagerID` is `NULL`) must still appear in the result, with `NULL` in the manager column.

**Concept:** A self join joins a table to itself. Here the same table is used twice under two aliases: `e` (employee) and `m` (manager).

### Setup

```sql
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    EmployeeName VARCHAR(50),
    ManagerID INT
);

INSERT INTO Employees (EmployeeID, EmployeeName, ManagerID) VALUES
(1, 'Anik', 3),
(2, 'Banik', 3),
(3, 'Chris', NULL),
(4, 'Devin', 1);
```

### Solution

```sql
SELECT e.employeename AS employee, m.employeename AS manager
FROM employees AS e
LEFT JOIN employees AS m ON e.managerid = m.employeeid;
```

### Expected Output

| employee | manager |
| -------- | ------- |
| Anik     | Chris   |
| Banik    | Chris   |
| Chris    | NULL    |
| Devin    | Anik    |

### Explanation

- `employees AS e` represents the employees, and `employees AS m` represents the managers (same table, different alias).
- The condition `e.managerid = m.employeeid` matches each employee's `ManagerID` to the manager's `EmployeeID`.
- `LEFT JOIN` keeps Chris in the result even though he has no manager. An `INNER JOIN` would have dropped him.

---

## 2. Parent & Child Relationship

### Problem

A `Family` table stores each person's `PersonID`, `PersonName`, and their parent's ID in `ParentID`. A person whose `ParentID` is `NULL` is the root of the family tree.

**Task:** List every parent-child pair in the table, showing the parent's name and the child's name in each row.

**Concept:** This is also a self join, where the same table plays the roles of both parent and child.

### Setup

```sql
CREATE TABLE Family (
    PersonID INT PRIMARY KEY,
    PersonName VARCHAR(50),
    ParentID INT
);

INSERT INTO Family (PersonID, PersonName, ParentID) VALUES
(1, 'Kalam', NULL),
(2, 'Abir', 1),
(3, 'Sultana', 1),
(4, 'Nila', 2),
(5, 'Rony', 2),
(6, 'Sajid', 3);
```

### Solution

```sql
SELECT a.personname AS parent, b.personname AS child
FROM family a
JOIN family b
ON a.personid = b.parentid;
```

### Expected Output

| parent  | child   |
| ------- | ------- |
| Kalam   | Abir    |
| Kalam   | Sultana |
| Abir    | Nila    |
| Abir    | Rony    |
| Sultana | Sajid   |

---

## 3. Filter with Date Range (BETWEEN)

### Problem

A `ProductSales` table records the sale date (`SaleDate`) and amount (`Amount`) for various products.

**Task:** Show only the sales made between **2026-10-05** and **2026-10-12**.

**Concept:** The `BETWEEN` operator filters values within a range. It is **inclusive**, meaning both the start and end values are included in the result.

### Setup

```sql
CREATE TABLE ProductSales (
    SaleID INT PRIMARY KEY,
    ProductName VARCHAR(50),
    SaleDate DATE,
    Amount INT
);

INSERT INTO ProductSales (SaleID, ProductName, SaleDate, Amount) VALUES
(1, 'Laptop', '2026-10-01', 50000),
(2, 'Mouse', '2026-10-03', 1500),
(3, 'Keyboard', '2026-10-05', 2500),
(4, 'Monitor', '2026-10-08', 12000),
(5, 'Phone', '2026-10-10', 25000),
(6, 'Headphones', '2026-10-12', 3500),
(7, 'Tablet', '2026-10-15', 18000),
(8, 'RAM', '2026-10-20', 4500);
```

### Solution

```sql
SELECT * FROM productsales
WHERE saledate BETWEEN '2026-10-05' AND '2026-10-12';
```

### Expected Output

| SaleID | ProductName | SaleDate   | Amount |
| ------ | ----------- | ---------- | ------ |
| 3      | Keyboard    | 2026-10-05 | 2500   |
| 4      | Monitor     | 2026-10-08 | 12000  |
| 5      | Phone       | 2026-10-10 | 25000  |
| 6      | Headphones  | 2026-10-12 | 3500   |

---

## 8. EXISTS Operator with Dummy Select (`SELECT 5`)

### Problem

There are two tables: `Students` (a list of students) and `LibraryBorrows` (which student borrowed which book).

**Task:** Show the names of students who have borrowed at least one book from the library. Use `SELECT 5` inside the `EXISTS` subquery.

**Concept:** `EXISTS` only checks whether the subquery returns any row (true/false). What the subquery selects does not matter, so `SELECT 1`, `SELECT 5`, `SELECT *`, or `SELECT 'abc'` all give the same result. Here, `5` is just a dummy value.

### Setup

```sql
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    StudentName VARCHAR(50)
);

CREATE TABLE LibraryBorrows (
    BorrowID INT PRIMARY KEY,
    StudentID INT,
    BookName VARCHAR(50)
);

INSERT INTO Students (StudentID, StudentName) VALUES
(1, 'Anik'), (2, 'Banik'), (3, 'Evan'), (4, 'Chris');

INSERT INTO LibraryBorrows (BorrowID, StudentID, BookName) VALUES
(101, 1, 'SQL Basics'),
(102, 3, 'Data Science'),
(103, 1, 'Java Programming');
```

### Solution

```sql
SELECT studentname
FROM students
WHERE EXISTS (
    SELECT 5 FROM libraryborrows
    WHERE libraryborrows.studentid = students.studentid
);
```

### Expected Output

| studentname |
| ----------- |
| Anik        |
| Evan        |
