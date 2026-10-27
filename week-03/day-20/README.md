# SQL Self Join: Problem Solving

**Date:** 27 October 2026

## Table of Contents

- [What is a Self Join?](#what-is-a-self-join)
- [Database Used](#database-used)
- [Problem 1: Easy](#problem-1-easy)
- [Problem 2: Medium](#problem-2-medium)
- [Problem 3: Hard](#problem-3-hard)
- [Problem 4: Hard](#problem-4-hard)
- [Summary](#summary)

---

## What is a Self Join?

A **self join** joins a table **with itself**. SQL treats the same table as **two separate copies**, so you must give each copy a **different alias**.

It is used when rows in a table are **related to other rows in the same table**, for example:

| Use case           | Example                                                           |
| ------------------ | ----------------------------------------------------------------- |
| **Hierarchy**      | Employee and manager (both are rows in the same `employee` table) |
| **Comparing rows** | Find employees who earn more than their manager                   |
| **Finding pairs**  | Pairs of employees who report to the same manager                 |

**Syntax**

```sql
SELECT ...
FROM table_name a
JOIN table_name b ON a.column = b.column;
```

---

## Database Used

Tables used in the queries (same structure):

| Table           | Used in         | Columns                                          |
| --------------- | --------------- | ------------------------------------------------ |
| `employeeTable` | Problems 1 to 3 | `employeeid` (PK), `name`, `managerid`           |
| `EmployeesTwo`  | Problem 4       | `employeeid` (PK), `name`, `managerid`, `salary` |

`managerid` is a **foreign key that points to `employeeid` of the same table**. The top person (CEO) has no manager, so their `managerid` is `NULL`.

### Sample data (used for all outputs)

| employeeid | name   | managerid | salary |
| ---------- | ------ | --------- | ------ |
| 1          | Rahim  | NULL      | 90000  |
| 2          | Karim  | 1         | 60000  |
| 3          | Nusrat | 1         | 55000  |
| 4          | Sadia  | 1         | 50000  |
| 5          | Tanvir | 2         | 40000  |
| 6          | Mitu   | 2         | 45000  |
| 7          | Arif   | 3         | 35000  |

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
| ------------- | ------------ |
| Rahim         | **NULL**     |
| Karim         | Rahim        |
| Nusrat        | Rahim        |
| Sadia         | Rahim        |
| Tanvir        | Karim        |
| Mitu          | Karim        |
| Arif          | Nusrat       |

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
| ----------- | ------------- | ------------ |
| 2           | Karim         | Rahim        |
| 3           | Nusrat        | Rahim        |
| 4           | Sadia         | Rahim        |
| 5           | Tanvir        | Karim        |
| 6           | Mitu          | Karim        |
| 7           | Arif          | Nusrat       |

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
| ------------ | --------------- |
| Rahim        | 3               |
| Karim        | 2               |

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
| ------------ | ---------- | -------- | ---------- | -------- |
| Rahim        | Karim      | 60000    | Nusrat     | 55000    |
| Rahim        | Karim      | 60000    | Sadia      | 50000    |
| Rahim        | Nusrat     | 55000    | Sadia      | 50000    |
| Karim        | Tanvir     | 40000    | Mitu       | 45000    |

---

## Summary

- A **self join** joins a table with itself. Use **different aliases** for the two copies, for example `e` (employee) and `m` (manager).
- The relationship is a **foreign key to the same table**: `e.managerid = m.employeeid`.
- **`LEFT JOIN`** keeps employees without a manager (the manager shows as `NULL`). **`INNER JOIN`** keeps only employees who have one.
- Combine a self join with **`GROUP BY`** and **`HAVING`** to count how many employees each manager has.
- To find **pairs** of rows in the same table, join the table **twice more** and use **`e1.employeeid < e2.employeeid`** so that each pair appears once.
- Read every self join as: _"this copy is the employee, that copy is the manager."_
