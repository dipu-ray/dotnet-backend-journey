# Topics: Practice SQL

**Date:** 13 October 2026


















# SQL: Joins, UNION, GROUP BY, HAVING, EXISTS

**Date:** 6 October 2026

Notes with simple examples on `JOIN` (inner, left, right, full, self), `UNION`, `UNION ALL`, `GROUP BY`, `HAVING`, and `EXISTS`.

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
- [Quick Reference](#quick-reference)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

## Sample Tables

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

| Join | Returns |
|------|---------|
| **INNER JOIN** | Only rows that **match** in both tables |
| **LEFT JOIN** | **All rows from the left table** + matching rows from the right |
| **RIGHT JOIN** | **All rows from the right table** + matching rows from the left |
| **FULL JOIN** | **All rows from both tables** (matched or not) |
| **SELF JOIN** | A table joined **with itself** |

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

| name | dept_name |
|------|-----------|
| Rahim | IT |
| Karim | IT |
| Nusrat | HR |
| Sadia | Finance |
| Mitu | IT |

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

| name | dept_name |
|------|-----------|
| Rahim | IT |
| Karim | IT |
| Nusrat | HR |
| Sadia | Finance |
| Tanvir | **NULL** |
| Mitu | IT |

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

| name | dept_name |
|------|-----------|
| Rahim | IT |
| Karim | IT |
| Mitu | IT |
| Nusrat | HR |
| Sadia | Finance |
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

| name | dept_name |
|------|-----------|
| Rahim | IT |
| Karim | IT |
| Mitu | IT |
| Nusrat | HR |
| Sadia | Finance |
| Tanvir | **NULL** |
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

| employee | manager |
|----------|---------|
| Rahim | **NULL** |
| Karim | Rahim |
| Nusrat | Rahim |
| Sadia | Rahim |
| Tanvir | Karim |
| Mitu | Karim |

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

| customers: name | city |
|-----------------|------|
| Ayesha | Dhaka |
| Bashir | Chattogram |
| Chitra | Dhaka |

| suppliers: name | city |
|-----------------|------|
| Delta Traders | Dhaka |
| Eastern Co | Sylhet |
| Faruk Ltd | Chattogram |

```sql
SELECT city FROM customers
UNION
SELECT city FROM suppliers;
```

| city |
|------|
| Dhaka |
| Chattogram |
| Sylhet |

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

| city |
|------|
| Dhaka |
| Chattogram |
| Dhaka |
| Dhaka |
| Sylhet |
| Chattogram |

| | `UNION` | `UNION ALL` |
|---|---------|-------------|
| Duplicates | Removed | **Kept** |
| Speed | Slower | **Faster** |
| Rows in the example above | 3 | 6 |

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
|-----------|-----------------|------------|
| IT | 3 | 65000 |
| HR | 1 | 50000 |
| Finance | 1 | 55000 |

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
|-----------|--------------|
| IT | 195000 |
| Finance | 55000 |
| HR | 50000 |

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
|-----------|-----------------|
| IT | 3 |

### WHERE vs HAVING

| | `WHERE` | `HAVING` |
|---|---------|----------|
| Filters | **Rows**, before grouping | **Groups**, after grouping |
| Can use aggregate functions? | No | **Yes** |
| Position | Before `GROUP BY` | After `GROUP BY` |

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
|-----------|-----------------|
| IT | 2 |

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
|-----------|
| IT |
| HR |
| Finance |

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

## Quick Reference

| Need | Use |
|------|-----|
| Only matching rows | `INNER JOIN` |
| All from the left table | `LEFT JOIN` |
| All from the right table | `RIGHT JOIN` |
| All from both tables | `FULL JOIN` |
| Table with itself (hierarchy) | `SELF JOIN` (aliases) |
| Combine results, remove duplicates | `UNION` |
| Combine results, keep duplicates | `UNION ALL` |
| Summarize per group | `GROUP BY` |
| Filter groups | `HAVING` |
| Check if related rows exist | `EXISTS` / `NOT EXISTS` |

---

## Practice Tasks

Use the tables above.

1. Show each employee's name with their department name (`INNER JOIN`).
2. Show **all employees**, even those without a department (`LEFT JOIN`).
3. Show **all departments**, even those without employees (`RIGHT JOIN`).
4. Find employees who have **no department** (`LEFT JOIN` + `IS NULL`).
5. Show each employee with their **manager's name** (`SELF JOIN`).
6. List all distinct cities from customers and suppliers using `UNION`, then again with `UNION ALL`. Compare the row counts.
7. Find the **number of employees** and the **highest salary** in each department.
8. Show only departments whose **average salary is above 52000** (`HAVING`).
9. Find the **total salary per department**, only for employees earning **more than 40000**, and show only departments with total **above 100000**.
10. Use `EXISTS` to list the departments that have at least one employee earning **more than 80000**.

---

## Summary

- **JOIN** combines tables using a related column. `INNER` = matches only, `LEFT` = all left rows, `RIGHT` = all right rows, `FULL` = all rows from both. **SELF JOIN** joins a table to itself using aliases.
- **UNION** merges `SELECT` results and removes duplicates. **UNION ALL** keeps duplicates and is faster.
- **GROUP BY** groups rows so aggregate functions work per group.
- **HAVING** filters groups. **WHERE** filters rows before grouping.
- **EXISTS** checks if a subquery returns any row. Prefer `NOT EXISTS` over `NOT IN` when `NULL` values are possible.