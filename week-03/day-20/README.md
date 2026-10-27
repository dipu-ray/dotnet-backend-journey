# SQL Self Join: Problem Solving

**Date:** 27 October 2026

Today's topic: **SQL Self Join**, practised with four problems from easy to hard, using an employee and manager table.

## Table of Contents
- [What is a Self Join?](#what-is-a-self-join)
- [Database Used](#database-used)
- [How to Read a Self Join](#how-to-read-a-self-join)
- [Problem 1: Easy](#problem-1-easy)
- [Problem 2: Medium](#problem-2-medium)
- [Problem 3: Hard](#problem-3-hard)
- [Problem 4: Hard](#problem-4-hard)
- [Bonus Problems](#bonus-problems)
- [Common Mistakes](#common-mistakes)
- [Summary](#summary)

---

## What is a Self Join?

A **self join** joins a table **with itself**. SQL treats the same table as **two separate copies**, so you must give each copy a **different alias**.

It is used when rows in a table are **related to other rows in the same table**, for example:

| Use case | Example |
|----------|---------|
| **Hierarchy** | Employee and manager (both are rows in the same `employee` table) |
| **Comparing rows** | Find employees who earn more than their manager |
| **Finding pairs** | Pairs of employees who report to the same manager |

> A self join is **not a special keyword**. It is a normal `JOIN` (`INNER` or `LEFT`) where both sides are the same table.

**Syntax**
```sql
SELECT ...
FROM table_name a
JOIN table_name b ON a.column = b.column;
```

---

## Database Used

Tables used in the queries (same structure):

| Table | Used in | Columns |
|-------|---------|---------|
| `employeeTable` | Problems 1 to 3 | `employeeid` (PK), `name`, `managerid` |
| `EmployeesTwo` | Problem 4 | `employeeid` (PK), `name`, `managerid`, `salary` |

`managerid` is a **foreign key that points to `employeeid` of the same table**. The top person (CEO) has no manager, so their `managerid` is `NULL`.

### Sample data (used for all outputs)

| employeeid | name | managerid | salary |
|------------|------|-----------|--------|
| 1 | Rahim | NULL | 90000 |
| 2 | Karim | 1 | 60000 |
| 3 | Nusrat | 1 | 55000 |
| 4 | Sadia | 1 | 50000 |
| 5 | Tanvir | 2 | 40000 |
| 6 | Mitu | 2 | 45000 |
| 7 | Arif | 3 | 35000 |

**Reporting structure**
```
Rahim
├── Karim
│   ├── Tanvir
│   └── Mitu
├── Nusrat
│   └── Arif
└── Sadia
```

> The salary column exists only in `EmployeesTwo` (Problem 4). The same sample rows are used for every problem so that the outputs are easy to compare. Row order is not guaranteed unless `ORDER BY` is used.

---

## How to Read a Self Join

```sql
FROM employeeTable e
LEFT JOIN employeeTable m ON e.managerid = m.employeeid
```

| Alias | Role | Meaning |
|-------|------|---------|
| `e` | **Employee** copy | The person we start from |
| `m` | **Manager** copy | The person that `e.managerid` points to |

The join condition connects the two copies:

```
e.managerid  =  m.employeeid
(the employee's manager ID)  matches  (the manager's own ID)
```

**Example for Karim**

| Copy | employeeid | name | managerid |
|------|------------|------|-----------|
| `e` (employee) | 2 | Karim | **1** |
| `m` (manager) | **1** | Rahim | NULL |

`e.managerid = 1` matches `m.employeeid = 1`, so Karim's manager is Rahim.

---

## Problem 1: Easy

**Problem:** Show every employee's name with their manager's name. Employees who have no manager must also appear.

**Solution**
```sql
SELECT
    e.name AS [Employee Name],
    m.name AS [Manager Name]
FROM employeeTable e
LEFT JOIN employeeTable m ON e.managerid = m.employeeid;
```

**Output**

| Employee Name | Manager Name |
|---------------|--------------|
| Rahim | **NULL** |
| Karim | Rahim |
| Nusrat | Rahim |
| Sadia | Rahim |
| Tanvir | Karim |
| Mitu | Karim |
| Arif | Nusrat |

**How it works**
- `LEFT JOIN` keeps **all rows from the left copy (`e`)**, so every employee appears.
- Rahim has `managerid = NULL`, which matches nothing, so his manager name is `NULL`.

---

## Problem 2: Medium

**Problem:** Show the employee ID, employee name, and manager name, only for employees who **have** a manager.

**Solution**
```sql
SELECT
    e.employeeid AS [Employee ID],
    e.name       AS [Employee Name],
    m.name       AS [Manager Name]
FROM employeeTable e
INNER JOIN employeeTable m ON e.managerid = m.employeeid;
```

**Output**

| Employee ID | Employee Name | Manager Name |
|-------------|---------------|--------------|
| 2 | Karim | Rahim |
| 3 | Nusrat | Rahim |
| 4 | Sadia | Rahim |
| 5 | Tanvir | Karim |
| 6 | Mitu | Karim |
| 7 | Arif | Nusrat |

**How it works**
- `INNER JOIN` returns **only rows that match**. Rahim (no manager) is **not** in the result.
- Compared with Problem 1: **6 rows** instead of 7.

| | Problem 1 (`LEFT JOIN`) | Problem 2 (`INNER JOIN`) |
|---|------------------------|--------------------------|
| Employees without a manager | Included (manager is `NULL`) | **Excluded** |
| Rows (sample data) | 7 | 6 |

---

## Problem 3: Hard

**Problem:** Find the managers who have **at least 2 employees** reporting to them, and show how many employees each manager has.

**Solution**
```sql
SELECT
    m.name AS [Manager Name],
    COUNT(e.managerid) AS [Total Employees]
FROM employeeTable e
INNER JOIN employeeTable m ON e.managerid = m.employeeid
GROUP BY m.name
HAVING COUNT(e.managerid) >= 2;
```

**Output**

| Manager Name | Total Employees |
|--------------|-----------------|
| Rahim | 3 |
| Karim | 2 |

**How it works**

| Step | Clause | What happens |
|------|--------|--------------|
| 1 | `INNER JOIN` | Pair each employee `e` with their manager `m` (6 rows) |
| 2 | `GROUP BY m.name` | Group the rows by manager |
| 3 | `COUNT(e.managerid)` | Count the employees in each group |
| 4 | `HAVING ... >= 2` | Keep only managers with 2 or more employees |

The groups before `HAVING`:

| Manager | Employees | Count | Kept? |
|---------|-----------|-------|-------|
| Rahim | Karim, Nusrat, Sadia | 3 | Yes |
| Karim | Tanvir, Mitu | 2 | Yes |
| Nusrat | Arif | 1 | **No** (below 2) |

**Notes**
- `HAVING` filters **groups**. `WHERE` cannot be used here because it runs before grouping and cannot use `COUNT`.
- `COUNT(e.managerid)` and `COUNT(*)` give the same result here, because the inner join leaves no `NULL` manager IDs.
- **Better grouping:** two different managers could have the **same name**. Group by the ID as well, so they are not merged:

```sql
SELECT
    m.name AS [Manager Name],
    COUNT(e.employeeid) AS [Total Employees]
FROM employeeTable e
INNER JOIN employeeTable m ON e.managerid = m.employeeid
GROUP BY m.employeeid, m.name
HAVING COUNT(e.employeeid) >= 2;
```

---

## Problem 4: Hard

**Problem:** For each manager, list every **pair of employees who report to the same manager**, with each employee's salary. Each pair should appear **only once**.

**Solution**
```sql
SELECT
    m.name      AS [Manager Name],
    e1.name     AS [Employee 1],
    e1.salary   AS [Salary 1],
    e2.name     AS [Employee 2],
    e2.salary   AS [Salary 2]
FROM EmployeesTwo m
JOIN EmployeesTwo e1 ON m.employeeid = e1.managerid
JOIN EmployeesTwo e2 ON m.employeeid = e2.managerid
                    AND e1.employeeid < e2.employeeid;
```

**Output**

| Manager Name | Employee 1 | Salary 1 | Employee 2 | Salary 2 |
|--------------|------------|----------|------------|----------|
| Rahim | Karim | 60000 | Nusrat | 55000 |
| Rahim | Karim | 60000 | Sadia | 50000 |
| Rahim | Nusrat | 55000 | Sadia | 50000 |
| Karim | Tanvir | 40000 | Mitu | 45000 |

Nusrat is not in the result because she has only one employee (Arif), so there is nobody to pair him with.

**How it works: three copies of the same table**

| Alias | Role |
|-------|------|
| `m` | The **manager** |
| `e1` | The **first employee** of that manager |
| `e2` | The **second employee** of the same manager |

```
m.employeeid = e1.managerid     -> e1 reports to m
m.employeeid = e2.managerid     -> e2 reports to the same m
e1.employeeid < e2.employeeid   -> avoid duplicate pairs and self pairs
```

**Why `e1.employeeid < e2.employeeid`?**

Without this condition, `e1` and `e2` both range over **all** of a manager's employees. For Rahim (3 employees) this gives `3 x 3 = 9` rows:

| Employee 1 | Employee 2 | Problem |
|------------|------------|---------|
| Karim | Karim | Paired with **themselves** |
| Karim | Nusrat | OK |
| Nusrat | Karim | **Mirror duplicate** of the row above |
| Nusrat | Nusrat | Paired with themselves |
| ... | ... | ... |

| Condition | Effect |
|-----------|--------|
| `e1.employeeid <> e2.employeeid` | Removes self pairs, but keeps mirrored duplicates (`A-B` and `B-A`) |
| **`e1.employeeid < e2.employeeid`** | Removes self pairs **and** mirrored duplicates. Each pair appears once |

Number of rows without the condition: `9 + 4 + 1 = 14`. With the condition: `3 + 1 + 0 = 4`.

> For a manager with `n` employees, the number of unique pairs is `n x (n - 1) / 2`. For `n = 3`, that is `3`.

**Tip:** the extra condition is placed in the `ON` clause of the second join. It could also go in `WHERE`. For an `INNER JOIN` the result is the same.

---

## Bonus Problems

**1. Employees without a manager (the top of the hierarchy)**
```sql
SELECT e.name AS [Employee Name]
FROM employeeTable e
WHERE e.managerid IS NULL;
```
Result: `Rahim`

**2. Employees who are not managers (nobody reports to them)**
```sql
SELECT e.name AS [Employee Name]
FROM employeeTable e
LEFT JOIN employeeTable r ON r.managerid = e.employeeid
WHERE r.employeeid IS NULL;
```
Result: `Sadia`, `Tanvir`, `Mitu`, `Arif`

**3. Employees who earn more than their manager**
```sql
SELECT e.name AS [Employee], e.salary AS [Employee Salary],
       m.name AS [Manager],  m.salary AS [Manager Salary]
FROM EmployeesTwo e
JOIN EmployeesTwo m ON e.managerid = m.employeeid
WHERE e.salary > m.salary;
```
Result: no rows for the sample data, because every manager earns more than their employees.

**4. Salary difference between each pair of employees with the same manager**
```sql
SELECT
    m.name  AS [Manager Name],
    e1.name AS [Employee 1],
    e2.name AS [Employee 2],
    ABS(e1.salary - e2.salary) AS [Salary Difference]
FROM EmployeesTwo m
JOIN EmployeesTwo e1 ON m.employeeid = e1.managerid
JOIN EmployeesTwo e2 ON m.employeeid = e2.managerid
                    AND e1.employeeid < e2.employeeid;
```

**5. Each employee with their manager and their manager's manager** (two levels up)
```sql
SELECT
    e.name  AS [Employee],
    m.name  AS [Manager],
    gm.name AS [Manager's Manager]
FROM employeeTable e
JOIN employeeTable m  ON e.managerid = m.employeeid
JOIN employeeTable gm ON m.managerid = gm.employeeid;
```

| Employee | Manager | Manager's Manager |
|----------|---------|-------------------|
| Tanvir | Karim | Rahim |
| Mitu | Karim | Rahim |
| Arif | Nusrat | Rahim |

**6. Employees who share the same manager with a given employee (for example Tanvir)**
```sql
SELECT peer.name AS [Colleague]
FROM employeeTable e
JOIN employeeTable peer ON e.managerid = peer.managerid
                       AND e.employeeid <> peer.employeeid
WHERE e.name = 'Tanvir';
```
Result: `Mitu`

**7. Managers and the number of employees, including managers with only one employee, sorted by count**
```sql
SELECT m.name AS [Manager Name], COUNT(e.employeeid) AS [Total Employees]
FROM employeeTable e
JOIN employeeTable m ON e.managerid = m.employeeid
GROUP BY m.employeeid, m.name
ORDER BY COUNT(e.employeeid) DESC;
```

---

## Common Mistakes

| Mistake | Problem and fix |
|---------|-----------------|
| **No aliases** | `FROM employeeTable JOIN employeeTable` is ambiguous and gives an error. Always alias each copy (`e`, `m`). |
| **Swapping the join columns** | `e.employeeid = m.managerid` reverses the relationship and shows each manager's employees as if they were the manager. Use **`e.managerid = m.employeeid`**. |
| **Using `INNER JOIN` when the top person must appear** | Rahim (no manager) disappears. Use `LEFT JOIN`. |
| **Using `LEFT JOIN` and then being surprised by `NULL`** | `NULL` in the manager column is correct. It means "no manager". |
| **Missing the `<` condition for pairs** | Gives self pairs and mirrored duplicates. Add `e1.employeeid < e2.employeeid`. |
| **`WHERE COUNT(*) >= 2`** | Not allowed. Use `HAVING` for conditions on aggregates. |
| **Grouping by name only** | Two people with the same name are merged. Group by the ID too. |
| **Unclear column names** | Both copies have `name`. Alias the output columns (`[Employee Name]`, `[Manager Name]`). |

---

## Summary

- A **self join** joins a table with itself. Use **different aliases** for the two copies, for example `e` (employee) and `m` (manager).
- The relationship is a **foreign key to the same table**: `e.managerid = m.employeeid`.
- **`LEFT JOIN`** keeps employees without a manager (the manager shows as `NULL`). **`INNER JOIN`** keeps only employees who have one.
- Combine a self join with **`GROUP BY`** and **`HAVING`** to count how many employees each manager has.
- To find **pairs** of rows in the same table, join the table **twice more** and use **`e1.employeeid < e2.employeeid`** so that each pair appears once.
- Read every self join as: *"this copy is the employee, that copy is the manager."*