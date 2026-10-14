# Topics: Client Handling, Recap SQL + New and Install Visual Studio

**Date:** 7 October 2026

## 1. Client Handling

- **Active Listening:** Carefully listening to and understanding exactly what the client wants.
- **Clear & Prompt Communication:** Regularly sharing work updates and responding quickly to messages or emails.
- **Professionalism:** Maintaining a calm, polite, and respectful demeanor in any situation.
- **Setting Expectations:** Defining the scope of work, deadline, and budget clearly right from the start.
- **Punctuality:** Delivering the project within the promised timeframe.
- **Problem-Solving Attitude:** Resolving any of the client's objections or complaints in a logical manner.
- **Honesty & Transparency:** Admitting mistakes or issues directly instead of hiding them.
- **Handling Feedback:** • Accepting the client's revisions or corrections with a positive attitude.

---

## 2. Recap SQL + New

### Sample Table: `students`

| id  | name   | department | age | city       |
| --- | ------ | ---------- | --- | ---------- |
| 1   | Rahim  | CSE        | 20  | Dhaka      |
| 2   | Karim  | EEE        | 21  | Chattogram |
| 3   | Nusrat | CSE        | 19  | Dhaka      |

### SELECT

```sql
SELECT * FROM students;                 -- all columns
SELECT name, city FROM students;        -- specific columns
```

### WHERE

```sql
SELECT * FROM students WHERE department = 'CSE';
SELECT * FROM students WHERE age >= 20 AND city = 'Dhaka';
SELECT * FROM students WHERE city = 'Dhaka' OR city = 'Sylhet';
```

| Operator                | Meaning              |
| ----------------------- | -------------------- |
| `=`, `<>`               | Equal, not equal     |
| `>`, `<`, `>=`, `<=`    | Comparison           |
| `AND`, `OR`, `NOT`      | Combine conditions   |
| `BETWEEN`, `IN`, `LIKE` | Range, list, pattern |

### INSERT INTO

```sql
INSERT INTO students (id, name, department, age, city)
VALUES (4, 'Sadia', 'BBA', 22, 'Sylhet');
```

### UPDATE

```sql
UPDATE students
SET city = 'Dhaka'
WHERE id = 2;
```

### DELETE

```sql
DELETE FROM students
WHERE id = 4;
```

### Sample Tables

**students**

| id  | name   | dept_id | marks |
| --- | ------ | ------- | ----- |
| 1   | Rahim  | 1       | 85    |
| 2   | Karim  | 2       | 72    |
| 3   | Nusrat | 1       | 91    |
| 4   | Sadia  | 3       | 68    |

**departments**

| dept_id | dept_name |
| ------- | --------- |
| 1       | CSE       |
| 2       | EEE       |
| 3       | BBA       |

### Creating a Table Without Commands (GUI)

1. Open the database, then right-click **Tables** and choose **New Table** (or **Create Table**).
2. Add **columns**: name, data type (`INT`, `VARCHAR`, `DECIMAL`), and whether it allows `NULL`.
3. Set the **Primary Key** (for example `id`) and turn on **Auto Increment / Identity** if needed.
4. **Save** the table and give it a name.
5. Open the table and **add rows** by typing directly into the grid.
   **Things to try (playing around)**

### Largest value: `MAX()`

```sql
SELECT MAX(marks) FROM students;
```

Result: `91`

### INNER JOIN

Returns only the rows that **match** in both tables.

```sql
SELECT students.name, departments.dept_name, students.marks
FROM students
INNER JOIN departments
    ON students.dept_id = departments.dept_id;
```

| name   | dept_name | marks |
| ------ | --------- | ----- |
| Rahim  | CSE       | 85    |
| Karim  | EEE       | 72    |
| Nusrat | CSE       | 91    |
| Sadia  | BBA       | 68    |

---

## 3. Install Visual Studio

1. Download **Visual Studio Community** from **https://visualstudio.microsoft.com/downloads/**
2. Run the installer (`VisualStudioSetup.exe`) and click **Continue**.
3. On the **Workloads** tab, select the workloads below.
4. Check the install location (bottom right).
5. Click **Install**, then **Launch** when it finishes.

| Workload                        | Why you need it                                                |
| ------------------------------- | -------------------------------------------------------------- |
| **ASP.NET and web development** | Core workload: Web API, MVC, Razor Pages, Blazor, ASP.NET Core |
| **Data storage and processing** | SQL Server tools and database work                             |

> **.NET desktop development** is **not required** for backend web work. Add it only if you also want to build console or Windows apps.
