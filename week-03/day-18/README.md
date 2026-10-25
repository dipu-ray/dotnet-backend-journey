# Topic: New Client Software Setup and Student Management System (SQL)

**Date:** 25 October 2026

## Table of Contents

- [Part 1: Software Setup for a New Client](#part-1-software-setup-for-a-new-client)
- [Part 2: Student Management System](#part-2-student-management-system)
  - [Requirements](#requirements)
  - [Database Design](#database-design)
  - [Create the Database and Tables](#create-the-database-and-tables)
  - [Insert Sample Data](#insert-sample-data)
  - [Basic Operations (CRUD)](#basic-operations-crud)
  - [Search and Filter](#search-and-filter)
  - [Reports](#reports)
  - [View](#view)
- [Summary](#summary)

---

# Part 1: Software Setup for a New Client

## 1. Client and Setup Details

| Field              | Details            |
| ------------------ | ------------------ |
| Abc Enterprise     | John Doe           |
| Setup date         | 25 October 2026    |
| Setup type         | New software setup |
| Server or computer | Own Server         |
| Number of users    | Max 100            |

## 2. Setup Steps

| #   | Step                   | Details                                                               | Done |
| --- | ---------------------- | --------------------------------------------------------------------- | ---- |
| 1   | **Basic Intro**        | Basic introduction of this softwares                                  | [ ]  |
| 2   | **Software Reg**       | Need some information of create account this software                 | [ ]  |
| 3   | **Features Customize** | Add features of his business type or demand                           | [ ]  |
| 4   | **Handover to Client** | Whole features and how to work this software to understand his client | [ ]  |

## 3. Common Setup Problems

- Small issue solve yourself otherwise contact to senior engineer.

## 7. Handover Checklist

- [ ] Software installed and working
- [ ] Database created or restored
- [ ] Application connects to the database
- [ ] Users and roles created
- [ ] Automatic backup configured and tested
- [ ] Client trained on the main features
- [ ] Documentation and login details handed over securely
- [ ] Client sign-off received

---

# Part 2: Student Management System

## Requirements

A system to manage a school or university's student information. It should store:

- **Departments** the institution has
- **Students** and their personal and department details
- **Teachers** and their departments
- **Courses**, their credits, department, and teacher
- **Enrollments**: which student takes which course in which semester, and the **marks** they get

## Database Design

### Tables

| Table         | Purpose                                       | Primary key    |
| ------------- | --------------------------------------------- | -------------- |
| `Departments` | List of departments                           | `DepartmentId` |
| `Teachers`    | Teachers, linked to a department              | `TeacherId`    |
| `Students`    | Student details, linked to a department       | `StudentId`    |
| `Courses`     | Courses, linked to a department and a teacher | `CourseId`     |
| `Enrollments` | Which student takes which course (with marks) | `EnrollmentId` |

### Relationships

```
Departments (1) ----< Students
Departments (1) ----< Teachers
Departments (1) ----< Courses
Teachers    (1) ----< Courses
Students    (1) ----< Enrollments >---- (1) Courses
```

## Create the Database and Tables

```sql
CREATE DATABASE StudentManagementDB;
GO

USE StudentManagementDB;
GO

-- 1. Departments
CREATE TABLE Departments (
    DepartmentId   INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL UNIQUE
);

-- 2. Teachers
CREATE TABLE Teachers (
    TeacherId    INT IDENTITY(1,1) PRIMARY KEY,
    FullName     VARCHAR(100) NOT NULL,
    Email        VARCHAR(100) UNIQUE,
    DepartmentId INT NOT NULL,
    CONSTRAINT FK_Teachers_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- 3. Students
CREATE TABLE Students (
    StudentId     INT IDENTITY(1,1) PRIMARY KEY,
    StudentCode   VARCHAR(20)  NOT NULL UNIQUE,
    FullName      VARCHAR(100) NOT NULL,
    Email         VARCHAR(100) UNIQUE,
    Phone         VARCHAR(20),
    Gender        VARCHAR(10)  CHECK (Gender IN ('Male', 'Female', 'Other')),
    DateOfBirth   DATE,
    City          VARCHAR(50),
    DepartmentId  INT NOT NULL,
    AdmissionDate DATE NOT NULL DEFAULT GETDATE(),
    IsActive      BIT  NOT NULL DEFAULT 1,
    CONSTRAINT FK_Students_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- 4. Courses
CREATE TABLE Courses (
    CourseId     INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode   VARCHAR(20)  NOT NULL UNIQUE,
    CourseName   VARCHAR(100) NOT NULL,
    Credit       INT NOT NULL CHECK (Credit BETWEEN 1 AND 6),
    DepartmentId INT NOT NULL,
    TeacherId    INT NULL,
    CONSTRAINT FK_Courses_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId),
    CONSTRAINT FK_Courses_Teachers
        FOREIGN KEY (TeacherId) REFERENCES Teachers(TeacherId)
);

-- 5. Enrollments
CREATE TABLE Enrollments (
    EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId    INT NOT NULL,
    CourseId     INT NOT NULL,
    Semester     VARCHAR(20) NOT NULL,
    Marks        DECIMAL(5,2) NULL CHECK (Marks BETWEEN 0 AND 100),
    EnrollDate   DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Enrollments_Students
        FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Enrollments_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    CONSTRAINT UQ_Enrollment UNIQUE (StudentId, CourseId, Semester)
);
```

| Feature used    | Meaning                                                       |
| --------------- | ------------------------------------------------------------- |
| `IDENTITY(1,1)` | Auto-increment ID starting at 1                               |
| `PRIMARY KEY`   | Unique identifier of each row                                 |
| `FOREIGN KEY`   | Links to a row in another table, so invalid links are blocked |
| `NOT NULL`      | A value is required                                           |
| `UNIQUE`        | No duplicate values (for example, student code, email)        |
| `CHECK`         | Only allowed values (for example, marks between 0 and 100)    |
| `DEFAULT`       | Value used when none is given (for example, today's date)     |

## Insert Sample Data

```sql
INSERT INTO Departments (DepartmentName) VALUES
('CSE'), ('EEE'), ('BBA');

INSERT INTO Teachers (FullName, Email, DepartmentId) VALUES
('Dr. Anwar Hossain', 'anwar@example.com',  1),
('Ms. Farzana Akter', 'farzana@example.com', 2),
('Mr. Shafiq Rahman', 'shafiq@example.com', 3);

INSERT INTO Students (StudentCode, FullName, Email, Phone, Gender, DateOfBirth, City, DepartmentId) VALUES
('S-001', 'Rahim Uddin',  'rahim@example.com',  '01700000001', 'Male',   '2004-03-15', 'Dhaka',      1),
('S-002', 'Nusrat Jahan', 'nusrat@example.com', '01700000002', 'Female', '2004-07-21', 'Dhaka',      1),
('S-003', 'Karim Hasan',  'karim@example.com',  '01700000003', 'Male',   '2003-11-02', 'Chattogram', 2),
('S-004', 'Sadia Islam',  'sadia@example.com',  '01700000004', 'Female', '2004-01-30', 'Sylhet',     3),
('S-005', 'Tanvir Ahmed', 'tanvir@example.com', '01700000005', 'Male',   '2004-05-09', 'Khulna',     1),
('S-006', 'Mitu Akter',   'mitu@example.com',   '01700000006', 'Female', '2004-09-12', 'Rajshahi',   2);

INSERT INTO Courses (CourseCode, CourseName, Credit, DepartmentId, TeacherId) VALUES
('CSE101', 'Programming Fundamentals', 3, 1, 1),
('CSE201', 'Database Systems',         3, 1, 1),
('EEE101', 'Basic Electronics',        3, 2, 2),
('BBA101', 'Principles of Management', 3, 3, 3);

INSERT INTO Enrollments (StudentId, CourseId, Semester, Marks) VALUES
(1, 1, 'Spring 2026', 85),
(1, 2, 'Spring 2026', 78),
(2, 1, 'Spring 2026', 92),
(2, 2, 'Spring 2026', 88),
(3, 3, 'Spring 2026', 70),
(4, 4, 'Spring 2026', 65),
(5, 1, 'Spring 2026', NULL);      -- marks not given yet
```

**Students**

| StudentId | StudentCode | FullName     | City       | Department |
| --------- | ----------- | ------------ | ---------- | ---------- |
| 1         | S-001       | Rahim Uddin  | Dhaka      | CSE        |
| 2         | S-002       | Nusrat Jahan | Dhaka      | CSE        |
| 3         | S-003       | Karim Hasan  | Chattogram | EEE        |
| 4         | S-004       | Sadia Islam  | Sylhet     | BBA        |
| 5         | S-005       | Tanvir Ahmed | Khulna     | CSE        |
| 6         | S-006       | Mitu Akter   | Rajshahi   | EEE        |

## Basic Operations (CRUD)

### Create: add a student

```sql
INSERT INTO Students (StudentCode, FullName, Email, Phone, Gender, DateOfBirth, City, DepartmentId)
VALUES ('S-007', 'Arif Hossain', 'arif@example.com', '01700000007', 'Male', '2004-02-18', 'Dhaka', 3);
```

### Read: view students

```sql
SELECT * FROM Students;

SELECT s.StudentCode, s.FullName, s.City, d.DepartmentName
FROM Students s
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId
ORDER BY s.StudentCode;
```

### Update: change student data

```sql
-- Change a phone number
UPDATE Students
SET Phone = '01800000001'
WHERE StudentCode = 'S-001';

-- Enter the missing marks
UPDATE Enrollments
SET Marks = 74
WHERE StudentId = 5 AND CourseId = 1 AND Semester = 'Spring 2026';
```

### Delete: remove data

```sql
-- Remove one enrollment
DELETE FROM Enrollments
WHERE StudentId = 5 AND CourseId = 1 AND Semester = 'Spring 2026';
```

> A student who still has enrollments **cannot be deleted** because of the foreign key. Delete the enrollments first, or better, use a **soft delete**:

```sql
-- Soft delete: keep the record, but mark it inactive
UPDATE Students
SET IsActive = 0
WHERE StudentCode = 'S-006';

-- Show only active students
SELECT * FROM Students WHERE IsActive = 1;
```

> **Always use `WHERE` with `UPDATE` and `DELETE`.**

## Search and Filter

```sql
-- Students whose name starts with R
SELECT StudentCode, FullName FROM Students WHERE FullName LIKE 'R%';

-- Students from Dhaka or Sylhet
SELECT StudentCode, FullName, City FROM Students WHERE City IN ('Dhaka', 'Sylhet');

-- Students admitted in a date range
SELECT StudentCode, FullName, AdmissionDate
FROM Students
WHERE AdmissionDate >= '2026-01-01' AND AdmissionDate < '2027-01-01';

-- Female students in the CSE department
SELECT s.StudentCode, s.FullName
FROM Students s
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId
WHERE d.DepartmentName = 'CSE' AND s.Gender = 'Female';
```

Result of the last query:

| StudentCode | FullName     |
| ----------- | ------------ |
| S-002       | Nusrat Jahan |

## Reports

### 1. Number of students per department

```sql
SELECT d.DepartmentName, COUNT(s.StudentId) AS TotalStudents
FROM Departments d
LEFT JOIN Students s ON s.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentName
ORDER BY TotalStudents DESC;
```

| DepartmentName | TotalStudents |
| -------------- | ------------- |
| CSE            | 3             |
| EEE            | 2             |
| BBA            | 1             |

`LEFT JOIN` keeps departments that have no students (they show `0`).

### 2. Students enrolled in a course

```sql
SELECT s.StudentCode, s.FullName, e.Marks
FROM Enrollments e
INNER JOIN Students s ON e.StudentId = s.StudentId
INNER JOIN Courses  c ON e.CourseId  = c.CourseId
WHERE c.CourseCode = 'CSE201';
```

| StudentCode | FullName     | Marks |
| ----------- | ------------ | ----- |
| S-001       | Rahim Uddin  | 78.00 |
| S-002       | Nusrat Jahan | 88.00 |

### 3. Result sheet with grade (`CASE`)

```sql
SELECT
    s.StudentCode,
    s.FullName,
    c.CourseCode,
    e.Marks,
    CASE
        WHEN e.Marks IS NULL THEN 'Pending'
        WHEN e.Marks >= 80 THEN 'A+'
        WHEN e.Marks >= 70 THEN 'A'
        WHEN e.Marks >= 60 THEN 'B'
        WHEN e.Marks >= 50 THEN 'C'
        ELSE 'F'
    END AS Grade
FROM Enrollments e
INNER JOIN Students s ON e.StudentId = s.StudentId
INNER JOIN Courses  c ON e.CourseId  = c.CourseId
ORDER BY s.StudentCode, c.CourseCode;
```

| StudentCode | FullName     | CourseCode | Marks | Grade   |
| ----------- | ------------ | ---------- | ----- | ------- |
| S-001       | Rahim Uddin  | CSE101     | 85.00 | A+      |
| S-001       | Rahim Uddin  | CSE201     | 78.00 | A       |
| S-002       | Nusrat Jahan | CSE101     | 92.00 | A+      |
| S-002       | Nusrat Jahan | CSE201     | 88.00 | A+      |
| S-003       | Karim Hasan  | EEE101     | 70.00 | A       |
| S-004       | Sadia Islam  | BBA101     | 65.00 | B       |
| S-005       | Tanvir Ahmed | CSE101     | NULL  | Pending |

(This is the data before the marks for Tanvir were entered.)

### 4. Average marks per student, highest first

```sql
SELECT s.StudentCode, s.FullName, AVG(e.Marks) AS AvgMarks
FROM Students s
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
WHERE e.Marks IS NOT NULL
GROUP BY s.StudentCode, s.FullName
ORDER BY AvgMarks DESC;
```

| StudentCode | FullName     | AvgMarks |
| ----------- | ------------ | -------- |
| S-002       | Nusrat Jahan | 90.00    |
| S-001       | Rahim Uddin  | 81.50    |
| S-003       | Karim Hasan  | 70.00    |
| S-004       | Sadia Islam  | 65.00    |

### 5. Top student

```sql
SELECT TOP 1 s.StudentCode, s.FullName, AVG(e.Marks) AS AvgMarks
FROM Students s
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
WHERE e.Marks IS NOT NULL
GROUP BY s.StudentCode, s.FullName
ORDER BY AvgMarks DESC;
```

Result: `S-002 | Nusrat Jahan | 90.00`

Use `ORDER BY AvgMarks ASC` to find the **lowest** average.

### 6. Average, highest, and lowest marks per course

```sql
SELECT c.CourseCode, c.CourseName,
       COUNT(e.EnrollmentId) AS TotalStudents,
       AVG(e.Marks) AS AvgMarks,
       MAX(e.Marks) AS HighestMarks,
       MIN(e.Marks) AS LowestMarks
FROM Courses c
LEFT JOIN Enrollments e ON c.CourseId = e.CourseId
GROUP BY c.CourseCode, c.CourseName
ORDER BY c.CourseCode;
```

| CourseCode | CourseName               | TotalStudents | AvgMarks | HighestMarks | LowestMarks |
| ---------- | ------------------------ | ------------- | -------- | ------------ | ----------- |
| BBA101     | Principles of Management | 1             | 65.00    | 65.00        | 65.00       |
| CSE101     | Programming Fundamentals | 3             | 88.50    | 92.00        | 85.00       |
| CSE201     | Database Systems         | 2             | 83.00    | 88.00        | 78.00       |
| EEE101     | Basic Electronics        | 1             | 70.00    | 70.00        | 70.00       |

`AVG`, `MAX`, and `MIN` ignore `NULL`, so Tanvir's missing marks do not affect them.

### 7. Courses with more than one student (`HAVING`)

```sql
SELECT c.CourseCode, COUNT(*) AS TotalStudents
FROM Enrollments e
INNER JOIN Courses c ON e.CourseId = c.CourseId
GROUP BY c.CourseCode
HAVING COUNT(*) > 1;
```

| CourseCode | TotalStudents |
| ---------- | ------------- |
| CSE101     | 3             |
| CSE201     | 2             |

### 8. Total credits per student

```sql
SELECT s.StudentCode, s.FullName, SUM(c.Credit) AS TotalCredits
FROM Students s
INNER JOIN Enrollments e ON s.StudentId = e.StudentId
INNER JOIN Courses c     ON e.CourseId  = c.CourseId
GROUP BY s.StudentCode, s.FullName;
```

### 9. Students with no enrollment (`NOT EXISTS`)

```sql
SELECT s.StudentCode, s.FullName
FROM Students s
WHERE NOT EXISTS (
    SELECT 1 FROM Enrollments e WHERE e.StudentId = s.StudentId
);
```

Result: `S-006 | Mitu Akter`

### 10. Students who failed (marks below 40)

```sql
SELECT s.StudentCode, s.FullName, c.CourseCode, e.Marks
FROM Enrollments e
INNER JOIN Students s ON e.StudentId = s.StudentId
INNER JOIN Courses  c ON e.CourseId  = c.CourseId
WHERE e.Marks < 40;
```

No rows are returned for the sample data, because nobody has marks below 40.

### 11. Courses and their teachers

```sql
SELECT c.CourseCode, c.CourseName, t.FullName AS Teacher
FROM Courses c
LEFT JOIN Teachers t ON c.TeacherId = t.TeacherId;
```

## View

A view saves a long query as a virtual table.

```sql
CREATE VIEW vw_StudentResults AS
SELECT
    s.StudentCode,
    s.FullName,
    d.DepartmentName,
    c.CourseCode,
    c.CourseName,
    c.Credit,
    e.Semester,
    e.Marks,
    CASE
        WHEN e.Marks IS NULL THEN 'Pending'
        WHEN e.Marks >= 80 THEN 'A+'
        WHEN e.Marks >= 70 THEN 'A'
        WHEN e.Marks >= 60 THEN 'B'
        WHEN e.Marks >= 50 THEN 'C'
        ELSE 'F'
    END AS Grade
FROM Enrollments e
INNER JOIN Students    s ON e.StudentId    = s.StudentId
INNER JOIN Courses     c ON e.CourseId     = c.CourseId
INNER JOIN Departments d ON s.DepartmentId = d.DepartmentId;
```

Use it like a table:

```sql
SELECT * FROM vw_StudentResults WHERE StudentCode = 'S-001';
```

| StudentCode | FullName    | DepartmentName | CourseCode | CourseName               | Credit | Semester    | Marks | Grade |
| ----------- | ----------- | -------------- | ---------- | ------------------------ | ------ | ----------- | ----- | ----- |
| S-001       | Rahim Uddin | CSE            | CSE101     | Programming Fundamentals | 3      | Spring 2026 | 85.00 | A+    |
| S-001       | Rahim Uddin | CSE            | CSE201     | Database Systems         | 3      | Spring 2026 | 78.00 | A     |

### Get one student's result

```sql
CREATE PROCEDURE usp_GetStudentResult
    @StudentCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT StudentCode, FullName, CourseCode, CourseName, Semester, Marks, Grade
    FROM vw_StudentResults
    WHERE StudentCode = @StudentCode
    ORDER BY CourseCode;
END;
```

```sql
EXEC usp_GetStudentResult @StudentCode = 'S-002';
```

---

## Summary

- **Client setup:** collect the requirements and access, install **SQL Server** and **SSMS**, configure the server, create or restore the database, create a **limited application login**, set the **connection string**, set up **backups**, test, hand over, and get sign-off.
- **Student Management System:** five tables (`Departments`, `Teachers`, `Students`, `Courses`, `Enrollments`). `Enrollments` links students and courses (**many-to-many**) and stores the marks.
- **Constraints** (`PRIMARY KEY`, `FOREIGN KEY`, `UNIQUE`, `CHECK`, `DEFAULT`, `NOT NULL`) keep the data correct.
- **Queries used:** CRUD, `LIKE` / `IN` search, `JOIN`, `GROUP BY` and `HAVING`, aggregate functions, `CASE` for grades, `NOT EXISTS`, `TOP` with `ORDER BY`.
- A **view** (`vw_StudentResults`) and **stored procedures** (`usp_AddStudent`, `usp_GetStudentResult`) make the system easier to reuse.
