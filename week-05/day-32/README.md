# SaaS Product Data Entry and C# 2D Arrays with Nested Loops

**Date:** 9 November 2026

Today's work:

1. **SaaS product:** data entry, and trying and learning about SaaS product websites.
2. **C# arrays:** 2D arrays, and nested loops using arrays.

> **Note:** I do not have the name of the SaaS product or the data that was entered, so the SaaS notes combine **general information** with **fill-in spaces** (`[ ... ]`) for your own details.

## Table of Contents

- [Part 1: SaaS Product](#part-1-saas-product)
  - [What is SaaS?](#1-what-is-saas)
  - [Parts of a SaaS Website](#2-parts-of-a-saas-website)
  - [How I Explored the Product](#3-how-i-explored-the-product)
  - [Data Entry](#4-data-entry)
  - [Product Notes](#5-product-notes)
- [Part 2: C# 2D Arrays and Nested Loops](#part-2-c-2d-arrays-and-nested-loops)
  - [2D Arrays](#1-2d-arrays)
  - [Nested Loops with a 2D Array](#2-nested-loops-with-a-2d-array)
  - [Nested Loops with a 1D Array](#3-nested-loops-with-a-1d-array)
  - [Arrays Progress](#arrays-progress)
- [Summary](#summary)

---

# Part 1: SaaS Product

## 1. What is SaaS?

**SaaS** (Software as a Service) is software that is **hosted online by the provider** and used **through a web browser** (or an app). The customer usually pays a **subscription** and does not install or maintain anything.

|                        | Traditional software                 | SaaS                                       |
| ---------------------- | ------------------------------------ | ------------------------------------------ |
| Installation           | Installed on your computer or server | **No installation.** Use it in the browser |
| Payment                | Often a one-time purchase            | **Subscription** (monthly or yearly)       |
| Updates                | You install them                     | **Automatic**, by the provider             |
| Server and maintenance | You manage them                      | **The provider** manages them              |
| Access                 | On that computer or network          | **From anywhere** with internet            |
| Data                   | Stored on your own system            | Stored on the **provider's cloud**         |

**Key points about SaaS**

- **Multi-tenant:** many customers use the same software, and each customer's data is kept **separate**.
- **Plans:** products usually offer plans (for example Free, Basic, Pro, Enterprise) with different limits and features.
- **Free trial or demo:** most products let you try before you pay.

**Examples of SaaS products:** Google Workspace, Microsoft 365, Slack, Zoho, Shopify, Salesforce, Trello, Notion.

---

## 2. Parts of a SaaS Website

| Part                            | What it is                                                                   |
| ------------------------------- | ---------------------------------------------------------------------------- |
| **Marketing pages**             | Home, features, pricing, blog, and customer stories that explain the product |
| **Sign up / free trial / demo** | Create an account or request a demo                                          |
| **Login**                       | Access to your own account                                                   |
| **Dashboard**                   | The main page with a summary of your data                                    |
| **Modules / features**          | The main working areas (for example customers, products, orders, reports)    |
| **Settings and profile**        | Company information, preferences, and notifications                          |
| **Users and roles**             | Invite team members and control what each can see and do                     |
| **Billing and subscription**    | Plan, payment method, and invoices                                           |
| **Import and export**           | Upload data from a file, or download your data                               |
| **Integrations and API**        | Connect to other tools                                                       |
| **Help center and support**     | Guides, documentation, and chat or email support                             |

---

## 3. How I Explored the Product

| Product detail | Notes                           |
| -------------- | ------------------------------- |
| Product name   | [Name]                          |
| Website        | [URL]                           |
| What it does   | [One or two lines]              |
| Plan used      | [Free trial / Free / Paid plan] |
| Date explored  | 9 November 2026                 |

| Step | What to do                                                                                    | Done |
| ---- | --------------------------------------------------------------------------------------------- | ---- |
| 1    | Read the **home page**, **features**, and **pricing** to understand the product and its plans | [ ]  |
| 2    | **Sign up** for the free trial or free plan                                                   | [ ]  |
| 3    | Look at the **dashboard** and the menu                                                        | [ ]  |
| 4    | Complete the **profile and company settings**                                                 | [ ]  |
| 5    | **Enter data** (see the next section)                                                         | [ ]  |
| 6    | Try the **main features** with that data                                                      | [ ]  |
| 7    | Look at the **reports** and check that they match the entered data                            | [ ]  |
| 8    | Check **users and roles**                                                                     | [ ]  |
| 9    | Check **import and export**                                                                   | [ ]  |
| 10   | Read the **help center** for anything unclear                                                 | [ ]  |

---

## 4. Data Entry

**Data entry** means **typing or uploading information into the system** so that it can be stored, searched, and used in reports. The quality of every report depends on the quality of the entered data.

### The process

```
Prepare the data  ->  Enter it in the system  ->  Check it  ->  Save  ->  Verify  ->  Correct mistakes
```

| Step           | What to do                                                               |
| -------------- | ------------------------------------------------------------------------ |
| 1. **Prepare** | Collect the data (for example in a spreadsheet) and decide the fields    |
| 2. **Enter**   | Use the **form** for one record at a time, or **import** a file for many |
| 3. **Check**   | Look at required fields, formats, and duplicates before saving           |
| 4. **Save**    | Save the record                                                          |
| 5. **Verify**  | Open the list or report and compare it with the source                   |
| 6. **Correct** | Fix mistakes in the system                                               |

### Types of fields

| Field type                            | Examples            | Tip                                             |
| ------------------------------------- | ------------------- | ----------------------------------------------- |
| **Text**                              | Name, address       | Use the same style every time                   |
| **Number**                            | Quantity, price     | Do not add symbols or extra spaces              |
| **Date**                              | Order date          | Use one date format                             |
| **Dropdown / list**                   | Category, status    | Choose from the list, do not type a new variant |
| **Required field** (often marked `*`) | Name, email         | The record cannot be saved without it           |
| **Unique field**                      | Email, product code | Duplicate values are rejected                   |

### Entering data one by one (form)

1. Open the module (for example **Products** or **Customers**).
2. Click **Add / New**.
3. Fill in the fields, starting with the required ones.
4. Click **Save**.
5. Check that the record appears in the list.

### Entering data in bulk (import)

1. Download the product's **import template** (a CSV or Excel file).
2. Fill in the data **under the same column names**.
3. **Upload** the file.
4. Map the columns if the product asks.
5. Review the **error report** and fix the rows with problems.
6. Check the imported records.

### Good practices

| Practice                                                             | Why                                      |
| -------------------------------------------------------------------- | ---------------------------------------- |
| Enter data **exactly as in the source**                              | Avoids mistakes                          |
| Use **consistent formats** (dates, phone numbers, names, units)      | Search and reports work properly         |
| **Avoid duplicates** (search before adding)                          | Keeps counts and totals correct          |
| Fill in **all required fields**                                      | Records stay complete                    |
| **Double-check numbers** (price, quantity)                           | Small typing mistakes cause wrong totals |
| **Verify after entering** (compare count and totals with the source) | Finds missing or wrong records           |
| Keep the **source file**                                             | You can recheck or re-enter later        |
| **Do not share the login** and log out when finished                 | Protects the data                        |

| Common mistake                           | Effect                                  |
| ---------------------------------------- | --------------------------------------- |
| Spelling the same name in different ways | The same item or customer appears twice |
| Wrong date format                        | Wrong dates or rejected records         |
| Extra spaces or symbols in numbers       | Errors or wrong totals                  |
| Skipping optional but useful fields      | Reports are incomplete                  |
| Entering the same record twice           | Duplicate data                          |

### Data entry log

| Item                        | Details                                   |
| --------------------------- | ----------------------------------------- |
| What data was entered       | [For example products, customers, orders] |
| Number of records entered   | [ ]                                       |
| Method                      | [One by one / import]                     |
| Source                      | [Spreadsheet, document, list]             |
| Problems faced              | [ ]                                       |
| How I fixed them            | [ ]                                       |
| Verified against the source | [Yes / No]                                |

---

## 5. Product Notes

| Question                                      | Answer |
| --------------------------------------------- | ------ |
| Who is the product for?                       | [ ]    |
| Main features I used                          | [ ]    |
| What I liked                                  | [ ]    |
| What was difficult or missing                 | [ ]    |
| Free plan limits (users, records, features)   | [ ]    |
| Paid plans and prices                         | [ ]    |
| Import and export available?                  | [ ]    |
| Support and help available?                   | [ ]    |
| What I learned about how a SaaS product works | [ ]    |

---

# Part 2: C# 2D Arrays and Nested Loops

## 1. 2D Arrays

A **2D (two-dimensional) array** stores values in a **table of rows and columns**. Each element is reached with **two indexes**: `[row, column]`.

```
              column 0   column 1   column 2
   row 0   [    1          2          3     ]
   row 1   [    4          5          6     ]
```

### Declare and initialize

```csharp
// With values
int[,] numbers = { { 1, 2, 3 }, { 4, 5, 6 } };

// With new and the values
int[,] numbers2 = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };

// With a size, filled in later (2 rows, 3 columns)
int[,] grid = new int[2, 3];
```

- The comma inside `[,]` shows that the array has **2 dimensions**.
- Each inner `{ ... }` is **one row**.
- A new array starts with default values (`0` for `int`).

### Access and change elements

```csharp
int[,] numbers = { { 1, 2, 3 }, { 4, 5, 6 } };

Console.WriteLine(numbers[0, 2]);     // 3  (row 0, column 2)
Console.WriteLine(numbers[1, 0]);     // 4  (row 1, column 0)

numbers[1, 0] = 40;                    // change an element
Console.WriteLine(numbers[1, 0]);     // 40
```

Both indexes start at `0`. An index that does not exist, such as `numbers[2, 0]` on a 2-row array, throws `IndexOutOfRangeException`.

### Size of a 2D array

| Member                 | Meaning                      | For `int[,] numbers = { {1,2,3}, {4,5,6} }` |
| ---------------------- | ---------------------------- | ------------------------------------------- |
| `numbers.GetLength(0)` | Number of **rows**           | `2`                                         |
| `numbers.GetLength(1)` | Number of **columns**        | `3`                                         |
| `numbers.Length`       | **Total** number of elements | `6`                                         |
| `numbers.Rank`         | Number of dimensions         | `2`                                         |

```csharp
Console.WriteLine(numbers.GetLength(0));    // 2
Console.WriteLine(numbers.GetLength(1));    // 3
Console.WriteLine(numbers.Length);          // 6
```

> For a 2D array, `Length` is **rows x columns**. Use `GetLength(0)` and `GetLength(1)` for the loop limits.

---

## 2. Nested Loops with a 2D Array

A **nested loop** is a loop **inside another loop**. For a 2D array:

- the **outer loop** goes through the **rows**,
- the **inner loop** goes through the **columns** of the current row.

### Print a 2D array

```csharp
int[,] numbers = { { 1, 2, 3 }, { 4, 5, 6 } };

for (int i = 0; i < numbers.GetLength(0); i++)          // rows
{
    for (int j = 0; j < numbers.GetLength(1); j++)      // columns
    {
        Console.Write(numbers[i, j] + " ");
    }
    Console.WriteLine();                                // new line after each row
}
```

**Output**

```
1 2 3
4 5 6
```

**How it runs**

| i (row) | j (column) | Element |
| ------- | ---------- | ------- |
| 0       | 0, 1, 2    | 1, 2, 3 |
| 1       | 0, 1, 2    | 4, 5, 6 |

For each value of `i`, the inner loop runs **all** values of `j`.

### `foreach` with a 2D array

`foreach` visits **every element**, row by row, without indexes.

```csharp
foreach (int n in numbers)
{
    Console.Write(n + " ");
}
// 1 2 3 4 5 6
```

|                                | Nested `for` | `foreach`         |
| ------------------------------ | ------------ | ----------------- |
| Gives row and column positions | **Yes**      | No                |
| Can change elements            | **Yes**      | No                |
| Prints as a table (row by row) | **Yes**      | No (one sequence) |

### Fill an array: multiplication table

```csharp
int[,] table = new int[3, 3];

for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        table[i, j] = (i + 1) * (j + 1);
    }
}

for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.Write(table[i, j] + "\t");
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

### Example: student marks

Rows are students, columns are subjects.

```csharp
int[,] marks =
{
    { 80, 70, 90 },      // student 1
    { 60, 85, 75 },      // student 2
    { 95, 88, 92 }       // student 3
};

int rows = marks.GetLength(0);
int cols = marks.GetLength(1);
```

**Total of each row (each student)**

```csharp
for (int i = 0; i < rows; i++)
{
    int rowTotal = 0;
    for (int j = 0; j < cols; j++)
    {
        rowTotal += marks[i, j];
    }
    Console.WriteLine($"Student {i + 1} total: {rowTotal}");
}
```

```
Student 1 total: 240
Student 2 total: 220
Student 3 total: 275
```

**Total of each column (each subject)**

```csharp
for (int j = 0; j < cols; j++)
{
    int colTotal = 0;
    for (int i = 0; i < rows; i++)
    {
        colTotal += marks[i, j];
    }
    Console.WriteLine($"Subject {j + 1} total: {colTotal}");
}
```

```
Subject 1 total: 235
Subject 2 total: 243
Subject 3 total: 257
```

For the column totals, the **loops are swapped**: the outer loop goes over columns and the inner loop over rows.

**Total and average of all marks**

```csharp
int total = 0;
foreach (int mark in marks)
{
    total += mark;
}

Console.WriteLine("Total: " + total);                                        // 735
Console.WriteLine("Average: " + ((double)total / marks.Length).ToString("F2"));   // 81.67
```

**Highest mark and its position**

```csharp
int max = marks[0, 0];
int maxRow = 0, maxCol = 0;

for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        if (marks[i, j] > max)
        {
            max = marks[i, j];
            maxRow = i;
            maxCol = j;
        }
    }
}

Console.WriteLine($"Highest mark: {max} at [{maxRow}, {maxCol}]");     // Highest mark: 95 at [2, 0]
```

**Search for a value**

```csharp
int target = 85;
bool found = false;

for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        if (marks[i, j] == target)
        {
            Console.WriteLine($"{target} found at [{i}, {j}]");           // 85 found at [1, 1]
            found = true;
        }
    }
}

if (!found)
{
    Console.WriteLine($"{target} not found");
}
```

---

## 3. Nested Loops with a 1D Array

Nested loops are also used with a **1D array**, when each element must be **compared with the other elements**.

### Find duplicate values

```csharp
int[] data = { 4, 2, 7, 2, 9, 4 };

for (int i = 0; i < data.Length; i++)
{
    for (int j = i + 1; j < data.Length; j++)      // j starts after i, so each pair is checked once
    {
        if (data[i] == data[j])
        {
            Console.WriteLine("Duplicate: " + data[i]);
        }
    }
}
```

**Output**

```
Duplicate: 4
Duplicate: 2
```

- The **outer loop** picks one element (`data[i]`).
- The **inner loop** compares it with every element **after** it (`data[j]`).
- `j = i + 1` avoids comparing an element with itself and avoids comparing the same pair twice.

---

## Arrays Progress

| Arrays topic                                                   | Status   |
| -------------------------------------------------------------- | -------- |
| Declare and initialize                                         | Done     |
| Access and change elements                                     | Done     |
| Array length                                                   | Done     |
| Loop through an array (`for`, `foreach`)                       | Done     |
| Sort an array (`Array.Sort`, `Array.Reverse`, `Array.IndexOf`) | Done     |
| `System.Linq` (`Min`, `Max`, `Sum`, `Average`)                 | Done     |
| **2D arrays**                                                  | **Done** |
| **Nested loops with arrays**                                   | **Done** |

---

# Summary

**SaaS product**

- **SaaS** is software hosted online by the provider and used through a browser, usually on a **subscription**, with automatic updates and no installation.
- A SaaS website has marketing pages, sign-up or demo, a dashboard, modules, settings, users and roles, billing, import and export, and a help center.
- **Data entry** means putting information into the system, one record at a time (form) or in bulk (import). Prepare the data, enter it, check it, and **verify** it against the source.
- Good data entry uses **consistent formats**, avoids **duplicates**, fills in **required fields**, and is **verified** afterwards.

**C# 2D arrays and nested loops**

- A **2D array** is a table of rows and columns, declared as `int[,]` and accessed with `[row, column]`.
- `GetLength(0)` gives the number of rows, `GetLength(1)` the number of columns, and `Length` the total number of elements.
- A **nested loop** puts one loop inside another. For a 2D array, the **outer loop** goes through the rows and the **inner loop** through the columns.
- Swap the loops to calculate **column** totals. Use nested loops for totals, the highest value, and searching.
- With a **1D array**, a nested loop compares each element with the others, for example to find duplicates (`j = i + 1`).
