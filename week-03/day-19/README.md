# SQL Joins: Problem Solving

**Date:** 26 October 2026

## Table of Contents

- [SQL Joins Recap](#sql-joins-recap)
- [Database Used](#database-used)
- [Level 1 Problems](#level-1-problems)
- [Level 2 Problems](#level-2-problems)
- [Summary](#summary)

---

## SQL Joins Recap

A **JOIN** combines rows from two or more tables using a **related column**.

| Join                          | Returns                                                   |
| ----------------------------- | --------------------------------------------------------- |
| `INNER JOIN` (or just `JOIN`) | Only rows that **match** in both tables                   |
| `LEFT JOIN`                   | All rows from the **left** table + matches from the right |
| `RIGHT JOIN`                  | All rows from the **right** table + matches from the left |
| `FULL JOIN`                   | All rows from **both** tables                             |

**Syntax**

```sql
SELECT columns
FROM table1 a
JOIN table2 b ON a.common_column = b.common_column;
```

- `JOIN` alone means `INNER JOIN`.
- `a` and `b` are **table aliases** (short names).
- `AS [Name With Spaces]` is a **column alias**. Square brackets are SQL Server syntax.
- All problems below use `JOIN` (inner join), so only matching rows appear.

---

## Database Used

Tables and columns (as used in the queries):

| Table         | Columns                                                      |
| ------------- | ------------------------------------------------------------ |
| `teachers`    | `teachersid` (PK), `name`                                    |
| `courses`     | `courseid` (PK), `coursename`, `teachersid` (FK to teachers) |
| `students`    | `studentid` (PK), `name`, `department`                       |
| `enrollments` | `studentid` (FK), `courseid` (FK)                            |
| `results`     | `studentid` (FK), `courseid` (FK), `marks`, `grade`          |

**Relationships**

```
teachers (1) ----< courses (many)
students (1) ----< enrollments >---- (1) courses     (many-to-many)
students (1) ----< results   >---- (1) courses       (marks and grade per student per course)
```

### Sample data used for the outputs

**teachers**

| teachersid | name              |
| ---------- | ----------------- |
| 1          | Dr. Anwar Hossain |
| 2          | Ms. Farzana Akter |
| 3          | Mr. Shafiq Rahman |

**courses**

| courseid | coursename               | teachersid |
| -------- | ------------------------ | ---------- |
| 1        | Programming Fundamentals | 1          |
| 2        | Database Systems         | 1          |
| 3        | Basic Electronics        | 2          |
| 4        | Principles of Management | 3          |

**students**

| studentid | name         | department |
| --------- | ------------ | ---------- |
| 1         | Rahim Uddin  | CSE        |
| 2         | Nusrat Jahan | CSE        |
| 3         | Karim Hasan  | EEE        |
| 4         | Sadia Islam  | BBA        |

**enrollments**

| studentid | courseid |
| --------- | -------- |
| 1         | 1        |
| 1         | 2        |
| 2         | 1        |
| 2         | 2        |
| 3         | 3        |
| 4         | 4        |

**results**

| studentid | courseid | marks | grade |
| --------- | -------- | ----- | ----- |
| 1         | 1        | 85    | A+    |
| 1         | 2        | 78    | A     |
| 2         | 1        | 92    | A+    |
| 2         | 2        | 88    | A+    |
| 3         | 3        | 70    | A     |
| 4         | 4        | 65    | B     |

> The outputs below are based on this sample data. Row order is not guaranteed unless `ORDER BY` is used.

---

# Level 1 Problems

## Level 1, Problem 1 (Easy)

**Problem:** Show each teacher's name along with the name of the course they teach.

**Tables used:** `teachers`, `courses`
**Link:** `teachers.teachersid = courses.teachersid`

**Solution**

```sql
SELECT
    t.name       AS [Teacher Name],
    c.coursename AS [Course Name]
FROM teachers t
JOIN courses c ON t.teachersid = c.teachersid;
```

**Output**

| Teacher Name      | Course Name              |
| ----------------- | ------------------------ |
| Dr. Anwar Hossain | Programming Fundamentals |
| Dr. Anwar Hossain | Database Systems         |
| Ms. Farzana Akter | Basic Electronics        |
| Mr. Shafiq Rahman | Principles of Management |

**How it works:** each course row stores the `teachersid` of its teacher. The join matches that value with the teacher's own ID. A teacher who teaches two courses appears twice.

---

## Level 1, Problem 2 (Easy)

**Problem:** Show each student's name along with the courses they are enrolled in.

**Tables used:** `students`, `enrollments`, `courses`
**Links:** `students.studentid = enrollments.studentid` and `enrollments.courseid = courses.courseid`

**Solution**

```sql
SELECT
    s.name       AS [Student Name],
    c.coursename AS [Course Name]
FROM students s
JOIN enrollments e ON s.studentid = e.studentid
JOIN courses c     ON e.courseid  = c.courseid;
```

**Output**

| Student Name | Course Name              |
| ------------ | ------------------------ |
| Rahim Uddin  | Programming Fundamentals |
| Rahim Uddin  | Database Systems         |
| Nusrat Jahan | Programming Fundamentals |
| Nusrat Jahan | Database Systems         |
| Karim Hasan  | Basic Electronics        |
| Sadia Islam  | Principles of Management |

**How it works:** `students` and `courses` have a **many-to-many** relationship, so they cannot be joined directly. The `enrollments` table sits between them. This is the **bridge (junction) table** pattern.

---

## Level 1, Problem 3 (Medium)

**Problem:** Show each student's name, the course they are enrolled in, and the name of the teacher who teaches that course.

**Tables used:** `students`, `enrollments`, `courses`, `teachers`
**Chain:** `students` -> `enrollments` -> `courses` -> `teachers`

**Solution**

```sql
SELECT
    s.name       AS [Student Name],
    t.name       AS [Teacher Name],
    c.coursename AS [Course Name]
FROM students s
JOIN enrollments e ON s.studentid = e.studentid
JOIN courses c     ON e.courseid  = c.courseid
JOIN teachers t    ON c.teachersid = t.teachersid;
```

**Output**

| Student Name | Teacher Name      | Course Name              |
| ------------ | ----------------- | ------------------------ |
| Rahim Uddin  | Dr. Anwar Hossain | Programming Fundamentals |
| Rahim Uddin  | Dr. Anwar Hossain | Database Systems         |
| Nusrat Jahan | Dr. Anwar Hossain | Programming Fundamentals |
| Nusrat Jahan | Dr. Anwar Hossain | Database Systems         |
| Karim Hasan  | Ms. Farzana Akter | Basic Electronics        |
| Sadia Islam  | Mr. Shafiq Rahman | Principles of Management |

**How it works:** this extends Problem 2 by one more join. `students` and `teachers` have no direct link, so the query travels through `enrollments` and `courses`. Both `students` and `teachers` have a `name` column, so each one is given a clear alias (`[Student Name]`, `[Teacher Name]`).

---

## Level 1, Problem 4 (Advanced)

**Problem:** Show each student's name, the teacher, the course, and the student's **marks and grade** in that course.

**Tables used:** `students`, `enrollments`, `courses`, `teachers`, `results`
**Special link:** `results` is joined on **two columns**: student **and** course.

**Solution**

```sql
SELECT
    s.name       AS [Student Name],
    t.name       AS [Teacher Name],
    c.coursename AS [Course Name],
    r.marks      AS [Marks],
    r.grade      AS [Grade]
FROM students s
JOIN enrollments e ON s.studentid = e.studentid
JOIN courses c     ON e.courseid  = c.courseid
JOIN teachers t    ON c.teachersid = t.teachersid
JOIN results r     ON s.studentid = r.studentid
                  AND c.courseid  = r.courseid;
```

**Output**

| Student Name | Teacher Name      | Course Name              | Marks | Grade |
| ------------ | ----------------- | ------------------------ | ----- | ----- |
| Rahim Uddin  | Dr. Anwar Hossain | Programming Fundamentals | 85    | A+    |
| Rahim Uddin  | Dr. Anwar Hossain | Database Systems         | 78    | A     |
| Nusrat Jahan | Dr. Anwar Hossain | Programming Fundamentals | 92    | A+    |
| Nusrat Jahan | Dr. Anwar Hossain | Database Systems         | 88    | A+    |
| Karim Hasan  | Ms. Farzana Akter | Basic Electronics        | 70    | A     |
| Sadia Islam  | Mr. Shafiq Rahman | Principles of Management | 65    | B     |

**How it works:** a result belongs to **one student in one course**, so the join uses `AND`:

```sql
JOIN results r ON s.studentid = r.studentid
              AND c.courseid  = r.courseid
```

This matches each enrollment row with its **own** result row. Joining on the student only would be wrong (see the [important note](#important-note-join-results-on-both-student-and-course)).

---

# Level 2 Problems

## Level 2, Problem 1 (Easy)

**Problem:** Show each student's name, their department, and the courses they are enrolled in.

**Tables used:** `students`, `enrollments`, `courses`

**Solution**

```sql
SELECT
    s.name       AS [Student Name],
    s.department AS [Department],
    c.coursename AS [Course Name]
FROM students s
JOIN enrollments e ON e.studentid = s.studentid
JOIN courses c     ON e.courseid  = c.courseid;
```

**Output**

| Student Name | Department | Course Name              |
| ------------ | ---------- | ------------------------ |
| Rahim Uddin  | CSE        | Programming Fundamentals |
| Rahim Uddin  | CSE        | Database Systems         |
| Nusrat Jahan | CSE        | Programming Fundamentals |
| Nusrat Jahan | CSE        | Database Systems         |
| Karim Hasan  | EEE        | Basic Electronics        |
| Sadia Islam  | BBA        | Principles of Management |

**How it works:** the same two-join pattern as Level 1 Problem 2, with one extra column (`department`) taken from `students`.

---

## Level 2, Problem 2 (Easy)

**Problem:** For each teacher, show the courses they teach and the students enrolled in those courses.

**Tables used:** `teachers`, `courses`, `enrollments`, `students`
**Chain:** `teachers` -> `courses` -> `enrollments` -> `students` (the same chain as Level 1 Problem 3, but in the opposite direction)

**Solution**

```sql
SELECT
    t.name       AS [Teacher Name],
    c.coursename AS [Course Name],
    s.name       AS [Student Name]
FROM teachers t
JOIN courses c     ON t.teachersid = c.teachersid
JOIN enrollments e ON c.courseid   = e.courseid
JOIN students s    ON e.studentid  = s.studentid;
```

**Output**

| Teacher Name      | Course Name              | Student Name |
| ----------------- | ------------------------ | ------------ |
| Dr. Anwar Hossain | Programming Fundamentals | Rahim Uddin  |
| Dr. Anwar Hossain | Programming Fundamentals | Nusrat Jahan |
| Dr. Anwar Hossain | Database Systems         | Rahim Uddin  |
| Dr. Anwar Hossain | Database Systems         | Nusrat Jahan |
| Ms. Farzana Akter | Basic Electronics        | Karim Hasan  |
| Mr. Shafiq Rahman | Principles of Management | Sadia Islam  |

**How it works:** start from the teacher and walk toward the students. The same tables can be joined in any direction, as long as every join uses the right linking columns.

---

## Level 2, Problem 3 (Medium)

**Problem:** Show each student's name, the course, and the marks and grade they got.

**Tables used:** `students`, `results`, `courses`
**Links:** `students.studentid = results.studentid` and `results.courseid = courses.courseid`

**Solution**

```sql
SELECT
    s.name       AS [Student Name],
    c.coursename AS [Course Name],
    r.marks      AS [Marks],
    r.grade      AS [Grade]
FROM students s
JOIN results r ON s.studentid = r.studentid
JOIN courses c ON r.courseid  = c.courseid;
```

**Output**

| Student Name | Course Name              | Marks | Grade |
| ------------ | ------------------------ | ----- | ----- |
| Rahim Uddin  | Programming Fundamentals | 85    | A+    |
| Rahim Uddin  | Database Systems         | 78    | A     |
| Nusrat Jahan | Programming Fundamentals | 92    | A+    |
| Nusrat Jahan | Database Systems         | 88    | A+    |
| Karim Hasan  | Basic Electronics        | 70    | A     |
| Sadia Islam  | Principles of Management | 65    | B     |

**How it works:** the `results` table already has both `studentid` and `courseid`, so it connects `students` and `courses` **directly**. The `enrollments` table is not needed here. Choosing the shortest correct path means fewer joins and simpler code.

---

## Level 2, Problem 4 (Medium)

**Problem:** Show each student's name, the course, and the marks, but only where the marks are **greater than 80**.

**Tables used:** `students`, `enrollments`, `courses`, `results`

**Your solution (as written)**

```sql
SELECT
    s.name       AS [Student Name],
    c.coursename AS [Course Name],
    r.marks      AS [Marks]
FROM students s
JOIN enrollments e ON s.studentid = e.studentid
JOIN courses c     ON e.courseid  = c.courseid
JOIN results r     ON s.studentid = r.studentid
WHERE marks > 80;
```

This query has two weak points:

1. **`results` is joined on the student only**, not on the course. This can pair a course with the **wrong marks** (explained in the next section).
2. **`marks` is not qualified.** It works only because `marks` exists in just one table. Write `r.marks` so the query stays correct if another table gets a `marks` column.

**Corrected solution**

```sql
SELECT
    s.name       AS [Student Name],
    c.coursename AS [Course Name],
    r.marks      AS [Marks]
FROM students s
JOIN enrollments e ON s.studentid = e.studentid
JOIN courses c     ON e.courseid  = c.courseid
JOIN results r     ON s.studentid = r.studentid
                  AND c.courseid  = r.courseid
WHERE r.marks > 80;
```

**Correct output**

| Student Name | Course Name              | Marks |
| ------------ | ------------------------ | ----- |
| Rahim Uddin  | Programming Fundamentals | 85    |
| Nusrat Jahan | Programming Fundamentals | 92    |
| Nusrat Jahan | Database Systems         | 88    |

**Simpler alternative:** no `enrollments` table is needed, because `results` links students and courses directly (see Level 2 Problem 3).

```sql
SELECT
    s.name       AS [Student Name],
    c.coursename AS [Course Name],
    r.marks      AS [Marks]
FROM students s
JOIN results r ON s.studentid = r.studentid
JOIN courses c ON r.courseid  = c.courseid
WHERE r.marks > 80;
```

---

## Summary

- A **JOIN** combines tables through a related column. `JOIN` alone is an **inner join** (matching rows only).
- Use **aliases** for tables (`s`, `c`, `t`) and for columns (`[Student Name]`) to keep queries short and results readable.
- **Many-to-many** relationships (students and courses) need a **bridge table** (`enrollments`).
- To connect tables with no direct link, **chain the joins** through the tables in between: `students -> enrollments -> courses -> teachers`.
- Pick the **shortest correct path**. For example, `results` already links students and courses.
- If tables are related by **two keys**, join on **both** with `AND`. Missing one creates extra, wrong rows.
- **Qualify column names** (`r.marks`, not `marks`) to avoid ambiguity.
- Check the **row count** after joining. Unexpectedly many rows usually means a missing join condition.
