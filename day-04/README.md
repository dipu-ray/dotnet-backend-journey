# SQL Basics: SELECT, WHERE, INSERT INTO, UPDATE, DELETE

**Date:** 6 October 2026

Notes on the five core SQL commands with runnable examples. The syntax works in MySQL, PostgreSQL, and SQL Server, with small differences noted where needed.

## Table of Contents

- [Go to C# Section](#loops-in-c-sharp)
- [What is SQL?](#what-is-sql)
- [Sample Table](#sample-table)
- [SELECT](#select)
- [WHERE](#where)
- [INSERT INTO](#insert-into)
- [UPDATE](#update)
- [DELETE](#delete)
- [Safe Practice Tips](#safe-practice-tips)
- [Quick Reference](#quick-reference)
- [SQL Summary](#sql-summary)
- [SQL Problem Solving](#sql-problem-solving)

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

## SQL Summary

- **`SELECT`** reads data; use column names instead of `*` when possible.
- **`WHERE`** filters rows using `=`, `>`, `<`, `AND`, `OR`, `NOT`, `BETWEEN`, `IN`, `LIKE`, `IS NULL`.
- **`INSERT INTO`** adds rows; list the column names for safety.
- **`UPDATE`** changes rows using `SET`; **always** add `WHERE`.
- **`DELETE`** removes rows; **always** add `WHERE`, and test with `SELECT` first.
- Text values use single quotes, and `NULL` is checked with `IS NULL`.

---

## SQL Problem Solving

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

---

# Loops in C Sharp

# Loops in C# (for, while, do-while)

Notes on repeating code with the **`for`**, **`while`**, and **`do-while`** loops, with runnable examples.

## Table of Contents

- [What is a Loop?](#what-is-a-loop)
- [for Loop](#for-loop)
- [while Loop](#while-loop)
- [do-while Loop](#do-while-loop)
- [break and continue](#break-and-continue)
- [Nested Loops](#nested-loops)
- [Infinite Loops](#infinite-loops)
- [Comparison: for vs while vs do-while](#comparison-for-vs-while-vs-do-while)
- [Common Mistakes](#common-mistakes)
- [Summary](#summary)
- [Problem Solving](#problem-solving)

---

## What is a Loop?

A **loop** repeats a block of code as long as a condition is `true`. Without loops, you would have to write the same line again and again.

```csharp
// Without a loop
Console.WriteLine("Hello");
Console.WriteLine("Hello");
Console.WriteLine("Hello");

// With a loop
for (int i = 0; i < 3; i++)
{
    Console.WriteLine("Hello");
}
```

Every loop has three parts:

| Part               | Purpose                                       |
| ------------------ | --------------------------------------------- |
| **Initialization** | Starting value of the counter (`int i = 0`)   |
| **Condition**      | Loop continues while this is `true` (`i < 3`) |
| **Update**         | Changes the counter each time (`i++`)         |

---

## for Loop

Best when you **know how many times** to repeat.

**Syntax**

```csharp
for (initialization; condition; update)
{
    // code to repeat
}
```

**Example: print 1 to 5**

```csharp
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}
```

**Output**

```
1
2
3
4
5
```

### How it runs

1. `int i = 1` runs **once** at the start.
2. `i <= 5` is checked. If `false`, the loop ends.
3. The body runs.
4. `i++` runs.
5. Go back to step 2.

### Counting down

```csharp
for (int i = 5; i >= 1; i--)
{
    Console.WriteLine(i);
}
// 5 4 3 2 1
```

### Changing the step

```csharp
// Even numbers from 2 to 10
for (int i = 2; i <= 10; i += 2)
{
    Console.WriteLine(i);
}
// 2 4 6 8 10
```

### Sum of numbers

```csharp
int sum = 0;

for (int i = 1; i <= 10; i++)
{
    sum += i;
}

Console.WriteLine("Sum: " + sum); // Sum: 55
```

### Multiplication table

```csharp
int n = 5;

for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{n} x {i} = {n * i}");
}
// 5 x 1 = 5
// 5 x 2 = 10
// ...
// 5 x 10 = 50
```

### Looping through a string or array

```csharp
string word = "Hello";

for (int i = 0; i < word.Length; i++)
{
    Console.WriteLine(word[i]);
}
// H e l l o (each on a new line)
```

> Index starts from `0`, so the last index is `Length - 1`. That is why the condition is `i < word.Length`, not `<=`.

---

## while Loop

Best when you **do not know** how many times to repeat. It keeps going until a condition becomes `false`. The condition is checked **before** each run, so the body may run **zero times**.

**Syntax**

```csharp
while (condition)
{
    // code to repeat
}
```

**Example: print 1 to 5**

```csharp
int i = 1;                // initialization (outside)

while (i <= 5)            // condition
{
    Console.WriteLine(i);
    i++;                  // update (inside, do not forget)
}
```

### Runs zero times

```csharp
int x = 10;

while (x < 5)
{
    Console.WriteLine("This never prints");
}
```

### Sum of digits

```csharp
int number = 1234;
int sum = 0;

while (number > 0)
{
    int digit = number % 10;   // last digit
    sum += digit;
    number /= 10;              // remove last digit
}

Console.WriteLine("Sum of digits: " + sum); // 10
```

### Reverse a number

```csharp
int number = 1234;
int reversed = 0;

while (number > 0)
{
    reversed = reversed * 10 + number % 10;
    number /= 10;
}

Console.WriteLine(reversed); // 4321
```

### Repeat until valid input

```csharp
int age = -1;

while (age < 0 || age > 120)
{
    Console.Write("Enter a valid age (0-120): ");
    age = int.Parse(Console.ReadLine());
}

Console.WriteLine("Age: " + age);
```

---

## do-while Loop

Like `while`, but the condition is checked **after** the body. So the body runs **at least once**, even if the condition is `false` from the start.

**Syntax**

```csharp
do
{
    // code to repeat
}
while (condition);     // note the semicolon
```

**Example: print 1 to 5**

```csharp
int i = 1;

do
{
    Console.WriteLine(i);
    i++;
}
while (i <= 5);
```

### Runs at least once

```csharp
int x = 10;

do
{
    Console.WriteLine("This prints once");
}
while (x < 5);
// Output: This prints once
```

### Menu that repeats until the user exits

```csharp
int choice;

do
{
    Console.WriteLine("1. Say Hello");
    Console.WriteLine("2. Say Bye");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");
    choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            Console.WriteLine("Hello!");
            break;
        case 2:
            Console.WriteLine("Bye!");
            break;
        case 0:
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}
while (choice != 0);
```

A menu must show **at least once**, which makes `do-while` a natural fit.

---

## break and continue

| Keyword    | Effect                                                |
| ---------- | ----------------------------------------------------- |
| `break`    | Stops the loop immediately                            |
| `continue` | Skips the rest of this round and goes to the next one |

```csharp
// break: stop when i is 4
for (int i = 1; i <= 10; i++)
{
    if (i == 4)
    {
        break;
    }
    Console.WriteLine(i);
}
// 1 2 3

// continue: skip i = 3
for (int i = 1; i <= 5; i++)
{
    if (i == 3)
    {
        continue;
    }
    Console.WriteLine(i);
}
// 1 2 4 5
```

---

## Nested Loops

A loop inside another loop. The inner loop runs fully for **each** round of the outer loop.

```csharp
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= 3; j++)
    {
        Console.Write(i * j + "\t");
    }
    Console.WriteLine();
}
```

**Output**

```
1	2	3
2	4	6
3	6	9
```

**Pattern: right triangle**

```csharp
for (int i = 1; i <= 4; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write("* ");
    }
    Console.WriteLine();
}
```

```
*
* *
* * *
* * * *
```

---

## Infinite Loops

A loop whose condition never becomes `false` runs forever.

```csharp
// Forgot to update i
int i = 1;
while (i <= 5)
{
    Console.WriteLine(i);
    // i++ is missing -> infinite loop
}

// Intentional infinite loop (exit with break)
while (true)
{
    Console.Write("Type 'exit' to stop: ");
    string input = Console.ReadLine();

    if (input == "exit")
    {
        break;
    }
}
```

> Press `Ctrl + C` in the console to stop an unwanted infinite loop.

---

## Comparison: for vs while vs do-while

|                   | `for`                   | `while`                   | `do-while`                            |
| ----------------- | ----------------------- | ------------------------- | ------------------------------------- |
| Condition checked | Before each run         | Before each run           | **After** each run                    |
| Minimum runs      | 0                       | 0                         | **1**                                 |
| Best for          | Known number of repeats | Unknown number of repeats | Must run at least once (menus, input) |
| Counter setup     | In the loop header      | Outside the loop          | Outside the loop                      |
| Typical use       | Counting, arrays        | Reading until a condition | Menu, validation                      |

The same task (print 1 to 3) in all three:

```csharp
// for
for (int i = 1; i <= 3; i++)
{
    Console.WriteLine(i);
}

// while
int a = 1;
while (a <= 3)
{
    Console.WriteLine(a);
    a++;
}

// do-while
int b = 1;
do
{
    Console.WriteLine(b);
    b++;
}
while (b <= 3);
```

---

## Common Mistakes

```csharp
// 1. Off-by-one error
for (int i = 1; i < 5; i++) { }    // prints 1-4, not 1-5. Use i <= 5.

// 2. Forgetting the update (infinite loop)
// while (i < 5) { Console.WriteLine(i); }

// 3. Semicolon after for/while header
for (int i = 0; i < 3; i++);       // empty loop body!
{
    Console.WriteLine("Runs only once");
}

// 4. Missing semicolon after do-while
// do { } while (x < 5)            // Error: needs ;

// 5. Using the counter outside a for loop
for (int i = 0; i < 3; i++) { }
// Console.WriteLine(i);           // Error: i exists only inside the loop
```

---

## Summary

- A **loop** repeats code while a condition is `true`.
- **`for`**: use when you know the number of repeats. Setup, condition, and update are in one line.
- **`while`**: use when the number of repeats is unknown. Condition is checked first, so it can run **0 times**.
- **`do-while`**: condition is checked last, so it runs **at least once**. Remember the `;` at the end.
- **`break`** exits the loop, **`continue`** skips to the next round.
- Always make sure the condition can become `false`, or you get an infinite loop.

---

## Problem Solving

### Problem 1: Easy

### The Countdown Challenge

### The Problem

Write a program that takes a positive integer `N` as input from the user. Then, using a single `for` loop, print the numbers in descending order from `N` down to `1`, and finally display the text `"Liftoff!"`.

### Example

**Input:**

```
5
```

**Expected Output:**

```
5
4
3
2
1
Liftoff!
```

### Conditions

- A `for` loop must be used to print the numbers.
- The loop must count backwards, from the larger number to the smaller number.

### Solution

### Using `for` loop

```csharp
using System;

class Test
{
    public static void Main(string[] args)
    {
        Console.Write("Enter input: ");
        int n = Convert.ToInt32(Console.ReadLine());

        for (int i = n; i > 0; i--)
        {
            Console.WriteLine($"{i}");
        }
        Console.WriteLine("Liftoff!");
    }
}
```

### Using `while` loop

```csharp
using System;

class Test
{
    public static void Main(string[] args)
    {
        Console.Write("Enter input: ");
        int n = Convert.ToInt32(Console.ReadLine());

        int i = n;
        while (i > 0)
        {
            Console.WriteLine($"{i}");
            i--;
        }
        Console.WriteLine("Liftoff!");
    }
}
```

### Using `do-while` loop

```csharp
using System;

class Test
{
    public static void Main(string[] args)
    {
        Console.Write("Enter input: ");
        int n = Convert.ToInt32(Console.ReadLine());

        int i = n;
        do
        {
            Console.WriteLine($"{i}");
            i--;
        } while (i > 0);
        Console.WriteLine("Liftoff!");
    }
}
```

> **Note:** The `do-while` version runs the loop body at least once, so it would print `0` if the input were `0` or negative. Since the problem guarantees a positive integer, this is fine here.

### Problem 2: Medium

### The Odd-Even Battle

### The Problem

Take a positive integer `N` as input from the user. Then, using a `for` loop, check all the numbers from `1` to `N`. Here is the twist:

- If the number is **odd**, multiply it by `2` and print the result.
- If the number is **even**, add `5` to it and print the result.

### Example

**Input:**

```
4
```

**Expected Output:**

```
2    (because 1 is odd, so 1 * 2 = 2)
7    (because 2 is even, so 2 + 5 = 7)
6    (because 3 is odd, so 3 * 2 = 6)
9    (because 4 is even, so 4 + 5 = 9)
```

### Conditions

- Use only a single `for` loop to go from `1` to `N`.
- Use a condition inside the loop to determine whether the number is even or odd.

### Solution

```csharp
using System;

class Test
{
    public static void Main(string[] args)
    {
        Console.Write("Enter input: ");
        int n = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= n; i++)
        {
            if (i % 2 == 0)
            {
                int sum = i + 5;
                Console.WriteLine($"{sum}");
            }
            else
            {
                int product = i * 2;
                Console.WriteLine($"{product}");
            }
        }
    }
}
```
