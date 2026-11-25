# SaaS Product Issue Fixing and C# Dictionary

**Date:** 25 November 2026

Today's work:

1. **SaaS Product:** fixing some issues (checking the module workflows, the **client due** problem, and data checking and entry)
2. **C# Advanced:** **Collections**, the **`Dictionary`**

> **Note:** I do not have the exact details of the SaaS issue (which client, which amounts, what the cause was), so Part 1 documents the **method and the workflow** and has **fill-in spaces** (`[ ... ]`) for the actual findings. The queries in Part 1 use **assumed table and column names** and must be adjusted to the real database.

## Table of Contents
- [Part 1: SaaS Product, Fixing Issues](#part-1-saas-product-fixing-issues)
  - [1. Summary of the Work](#1-summary-of-the-work)
  - [2. Understanding the Workflow Module by Module](#2-understanding-the-workflow-module-by-module)
  - [3. The Problem: Client Due](#3-the-problem-client-due)
  - [4. How Client Due Is Connected to Stock and Invoices](#4-how-client-due-is-connected-to-stock-and-invoices)
  - [5. The Fix in Three Steps](#5-the-fix-in-three-steps)
  - [6. Checking Queries](#6-checking-queries)
  - [7. Data Checking and Entry](#7-data-checking-and-entry)
  - [8. Verification, Issues, and Learnings](#8-verification-issues-and-learnings)
- [Part 2: C# Collections, Dictionary](#part-2-c-collections-dictionary)
  - [1. What is a Dictionary?](#1-what-is-a-dictionary)
  - [2. Basics: Create, Add, and Read](#2-basics-create-add-and-read)
  - [3. Looping Through a Dictionary](#3-looping-through-a-dictionary)
  - [4. Dictionary Operations](#4-dictionary-operations)
  - [5. Good to Know](#5-good-to-know)
  - [6. Quick Reference](#6-quick-reference)
- [Summary](#summary)

---

# Part 1: SaaS Product, Fixing Issues

## 1. Summary of the Work

| # | Task | Status |
|---|------|--------|
| 1 | Visit and understand the **workflow**, one module at a time | Done |
| 2 | **Check the client due** and find the problem | Done |
| 3 | Fix the problem in the order: **1. check stock, 2. create the client invoice, 3. fix the client due** | [Done / In progress] |
| 4 | **Check and enter** some data in the SaaS product | Done |

---

## 2. Understanding the Workflow Module by Module

Before fixing the issue, the modules were visited **one by one** to see how data moves between them.

| Module | Purpose | What I checked | Notes |
|--------|---------|----------------|-------|
| **Items / Products** | List of goods and their details | [ ] | [ ] |
| **Clients (customers)** | Client information and dues | [ ] | [ ] |
| **Purchase** | Buying goods, which **increases stock** | [ ] | [ ] |
| **Stock / Inventory** | Quantity in the store | [ ] | [ ] |
| **Sales / Client Invoice** | Selling to a client, which **reduces stock** and **creates the client due** | [ ] | [ ] |
| **Payments / Collection** | Money received from the client, which **reduces the due** | [ ] | [ ] |
| **Accounts / Reports** | Dues, income, and expense reports | [ ] | [ ] |

**How the modules are connected**

```
Purchase  ->  Stock increases
                 |
Client Invoice  ->  Stock decreases  ->  Client Due increases
                                              |
Payment received  ->  Client Due decreases
                                              |
Reports  <-  show stock, client due, and income
```

---

## 3. The Problem: Client Due

| Item | Details |
|------|---------|
| Problem | The **client due** shown in the software was **not correct** |
| Client | [Client name] |
| Due shown in the software | [Amount] |
| Due that should be shown | [Amount] |
| Difference | [Amount] |
| Where it was noticed | [Client due report / client screen] |

**What was done to find the problem**
1. Open the **client** and look at the **due** shown by the software.
2. List the client's **invoices** and **payments**, and calculate the due by hand: `total invoices - total payments`.
3. Compare the **calculated due** with the **shown due**.
4. For the invoices involved, check whether the **stock** of the items is correct.
5. Find **which record is missing or wrong** and explains the difference between the two numbers.

**Cause found:** [Write the cause, for example a missing invoice, a wrong amount, a payment not linked to the client, or wrong stock.]

---

## 4. How Client Due Is Connected to Stock and Invoices

**Client due** is the money a client **still owes**:

```
Client Due  =  Total of the client's invoices  -  Total of the payments received from the client
```

| Event | Effect on stock | Effect on client due |
|-------|-----------------|----------------------|
| **Client invoice** is created | Stock **decreases** | Due **increases** by the invoice total |
| **Payment** is received | No change | Due **decreases** by the payment |
| **Invoice** is cancelled or deleted | Stock **increases** back | Due **decreases** by the invoice total |

**Why the order of the fix matters**

A client invoice **reduces stock**, and it **creates the due**. So:

1. If the **stock is wrong**, the invoice can fail or can make the stock negative.
2. If the **invoice is missing or wrong**, the due can never be right.
3. Only after the stock and the invoice are right, the **due can be fixed** and will stay correct.

---

## 5. The Fix in Three Steps

### Step 1: Check the stock

Make sure the **stock of the items** is correct **before** creating the invoice.

| Check | Done |
|-------|------|
| Items on the invoice exist in the item list | [ ] |
| **Stock quantity** of each item is enough for the invoice | [ ] |
| Stock history (in and out) matches the current stock | [ ] |
| If the stock is wrong, correct it first (purchase entry or stock adjustment) | [ ] |

Result: [Stock was correct / Stock was corrected for these items: ...]

### Step 2: Create the client invoice

| Check | Done |
|-------|------|
| Select the **correct client** | [ ] |
| Add the **items, quantities, prices**, and any discount | [ ] |
| Check the **total amount** | [ ] |
| Save the invoice **through the software** (not by editing the database directly) | [ ] |
| Confirm that the **stock decreased** and the **client due increased** by the invoice total | [ ] |

Result: Invoice no. [ ], total [ ].

### Step 3: Fix the client due

| Check | Done |
|-------|------|
| Compare the due with `total invoices - total payments` | [ ] |
| If a **payment** is missing or wrong, record or correct it | [ ] |
| Fix any **remaining difference** with a proper correction entry | [ ] |
| Re-check the due on the client screen and in the report | [ ] |

Result: Due before [ ], due after [ ].

> Make corrections with **proper entries** (invoice, payment, adjustment), so the history stays complete. Do not silently change numbers.

---

## 6. Checking Queries

> **Assumption:** the tables are `Clients`, `Invoices`, and `Payments`, with the columns shown. Replace them with the real names. Use `SELECT` only while checking.

**Calculated client due**
```sql
DECLARE @ClientId INT = 0;      -- the client being checked

SELECT
    c.Id,
    c.Name,
    ISNULL(i.TotalInvoiced, 0) AS TotalInvoiced,
    ISNULL(p.TotalPaid, 0)     AS TotalPaid,
    ISNULL(i.TotalInvoiced, 0) - ISNULL(p.TotalPaid, 0) AS CalculatedDue
FROM Clients c
LEFT JOIN (
    SELECT ClientId, SUM(TotalAmount) AS TotalInvoiced
    FROM Invoices
    WHERE ISNULL(IsDelete, 0) = 0
    GROUP BY ClientId
) i ON i.ClientId = c.Id
LEFT JOIN (
    SELECT ClientId, SUM(Amount) AS TotalPaid
    FROM Payments
    WHERE ISNULL(IsDelete, 0) = 0
    GROUP BY ClientId
) p ON p.ClientId = c.Id
WHERE c.Id = @ClientId;
```

Compare `CalculatedDue` with the due **shown in the software**. The difference is the amount to explain.

**Invoices of the client**
```sql
SELECT Id, InvoiceDate, TotalAmount
FROM Invoices
WHERE ClientId = @ClientId
  AND ISNULL(IsDelete, 0) = 0
ORDER BY InvoiceDate;
```

**Payments of the client**
```sql
SELECT Id, PaymentDate, Amount
FROM Payments
WHERE ClientId = @ClientId
  AND ISNULL(IsDelete, 0) = 0
ORDER BY PaymentDate;
```

**Stock check for one item** (In minus Out, compared with the current stock)
```sql
SELECT
    SUM(CASE WHEN UPPER(StockType) = 'IN'  THEN  ISNULL(Quantity, 0)
             WHEN UPPER(StockType) = 'OUT' THEN -ISNULL(Quantity, 0)
             ELSE 0 END) AS StockBalance
FROM Stock
WHERE ItemId = 0                       -- the item being checked
  AND ISNULL(IsDelete, 0) = 0;

SELECT SUM(Quantity) AS CurrentStock
FROM CurrentStock
WHERE ItemId = 0;
```

---

## 7. Data Checking and Entry

Some data was **checked and entered** in the SaaS product.

| # | Module | What was checked or entered | Records | Result |
|---|--------|-----------------------------|---------|--------|
| 1 | [For example Items] | [Checked / Entered] | [Number] | [OK / Problem found] |
| 2 | [For example Clients] | [ ] | [ ] | [ ] |
| 3 | [For example Purchase] | [ ] | [ ] | [ ] |
| 4 | [For example Invoice] | [ ] | [ ] | [ ] |

**Data entry rules followed**
- Enter data **exactly as in the source**.
- Use **consistent names and formats** (dates, units, client names).
- **Search before adding**, to avoid duplicates.
- **Verify** after entering: compare the count and the totals with the source.

---

## 8. Verification, Issues, and Learnings

### Verification checklist

| Check | Result |
|-------|--------|
| Stock of the items is correct | [ ] |
| The client invoice exists and the amount is correct | [ ] |
| The client due in the software equals `total invoices - total payments` | [ ] |
| Reports show the same due as the client screen | [ ] |

### Issues

| Issue | Cause | How it was handled |
|-------|-------|--------------------|
| [ ] | [ ] | [ ] |

### What I learned
- The **modules are connected**: a **client invoice** affects **stock** and the **client due**, and a **payment** affects the due.
- To fix a due problem, **find the exact difference** by calculating `invoices - payments`, then find the record that explains it.
- Fix in a **safe order**: **stock first**, then the **invoice**, then the **due**.
- [Add your own points]

---

# Part 2: C# Collections, Dictionary

## 1. What is a Dictionary?

A **`Dictionary<TKey, TValue>`** stores **key-value pairs**. Each value is found through its **unique key**, like a real dictionary where a **word** (key) leads to its **meaning** (value).

```csharp
using System.Collections.Generic;       // Dictionary is in this namespace
```

| Feature | Detail |
|---------|--------|
| **Key-value pairs** | Each item has a **key** and a **value** |
| **Unique keys** | **No two items can have the same key** |
| **Duplicate values** | **Allowed** |
| **Fast lookup** | A value is found **by key** very quickly (it uses hashing) |
| **Types** | `Dictionary<int, string>`: the key is an `int`, the value is a `string` |
| **Dynamic size** | Grows and shrinks automatically |
| **Order** | The order of the items is **not guaranteed** |

```
Key  ->  Value
101  ->  Rahim
102  ->  Karim
103  ->  Dipu
```

### List vs Dictionary

| | `List<T>` | `Dictionary<TKey, TValue>` |
|---|-----------|----------------------------|
| Stores | Single items | **Key-value pairs** |
| Access by | **Index** (`0`, `1`, `2`) | **Key** (`101`, `"Rahim"`) |
| Search by value | Loops through the items | **By key**, very fast |
| Duplicates | Allowed | **Keys must be unique** |
| Use when | A simple ordered list | You look up values by an **ID or name** |

---

## 2. Basics: Create, Add, and Read

```csharp
Dictionary<int, string> students = new Dictionary<int, string>();

students.Add(101, "Rahim");
students.Add(102, "Karim");
students.Add(103, "Dipu");

Console.WriteLine(students[101]);
```

**Output**
```
Rahim
```

| Line | Meaning |
|------|---------|
| `new Dictionary<int, string>()` | An empty dictionary. Keys are `int`, values are `string` |
| `students.Add(101, "Rahim")` | Adds the key `101` with the value `"Rahim"` |
| `students[101]` | Reads the value for the key `101` |

**Other ways to create and fill a dictionary**
```csharp
// Collection initializer
var a = new Dictionary<int, string>
{
    { 101, "Rahim" },
    { 102, "Karim" }
};

// Index initializer
var b = new Dictionary<int, string>
{
    [101] = "Rahim",
    [102] = "Karim"
};
```

---

## 3. Looping Through a Dictionary

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Dictionary<int, string> students = new Dictionary<int, string>();

        students.Add(101, "Rahim");
        students.Add(102, "Karim");
        students.Add(103, "Abdul");

        foreach (var student in students)
        {
            Console.WriteLine($"Student ID: {student.Key}, Name: {student.Value}");
        }
    }
}
```

**Output**
```
Student ID: 101, Name: Rahim
Student ID: 102, Name: Karim
Student ID: 103, Name: Abdul
```

**How it works**
- Each item in a `foreach` is a **`KeyValuePair<int, string>`** (`var` lets the compiler figure out the type).
- `student.Key` is the **key** and `student.Value` is the **value**.

---

## 4. Dictionary Operations

### Code

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        // 1. Dictionary Create
        Dictionary<int, string> employees = new Dictionary<int, string>();

        // 2. Add() - Insert Employees
        employees.Add(101, "Rahim");
        employees.Add(102, "Karim");
        employees.Add(103, "Hasan");
        employees.Add(104, "Sakib");
        Console.WriteLine("=== All Employees ===");
        foreach (var emp in employees)
        {
            Console.WriteLine(
                $"ID: {emp.Key}, Name: {emp.Value}");
        }

        // 3. ContainsKey() - Check Employee ID
        Console.WriteLine("\n=== ContainsKey ===");
        if (employees.ContainsKey(102))
        {
            Console.WriteLine("Employee ID 102 Exists");
        }
        else
        {
            Console.WriteLine("Employee Not Found");
        }

        // 4. ContainsValue() - Check Employee Name
        Console.WriteLine("\n=== ContainsValue ===");
        if (employees.ContainsValue("Hasan"))
        {
            Console.WriteLine("Hasan Exists");
        }
        else
        {
            Console.WriteLine("Employee Name Not Found");
        }

        // 5. TryGetValue() - Get Employee Name
        Console.WriteLine("\n=== TryGetValue ===");
        if (employees.TryGetValue(103, out string name))
        {
            Console.WriteLine($"Employee Name: {name}");
        }
        else
        {
            Console.WriteLine("Employee Not Found");
        }

        // 6. Remove() - Delete Employee
        Console.WriteLine("\n=== Remove ===");
        if (employees.Remove(104))
        {
            Console.WriteLine("Employee 104 Deleted");
        }
        else
        {
            Console.WriteLine("Employee Not Found");
        }

        // 7. Keys - Display All Employee IDs
        Console.WriteLine("\n=== Employee IDs ===");
        foreach (int id in employees.Keys)
        {
            Console.WriteLine(id);
        }

        // 8. Values - Display All Employee Names
        Console.WriteLine("\n=== Employee Names ===");
        foreach (string empName in employees.Values)
        {
            Console.WriteLine(empName);
        }

        // 9. Count - Total Employees
        Console.WriteLine("\n=== Total Employees ===");
        Console.WriteLine(employees.Count);

        // 10. Clear() - Delete All Employees
        Console.WriteLine("\n=== Clear ===");
        employees.Clear();
        Console.WriteLine("All Employees Deleted");
        Console.WriteLine($"Total Employees After Clear: {employees.Count}");
    }
}
```

### Output

```
=== All Employees ===
ID: 101, Name: Rahim
ID: 102, Name: Karim
ID: 103, Name: Hasan
ID: 104, Name: Sakib

=== ContainsKey ===
Employee ID 102 Exists

=== ContainsValue ===
Hasan Exists

=== TryGetValue ===
Employee Name: Hasan

=== Remove ===
Employee 104 Deleted

=== Employee IDs ===
101
102
103

=== Employee Names ===
Rahim
Karim
Hasan

=== Total Employees ===
3

=== Clear ===
All Employees Deleted
Total Employees After Clear: 0
```

### The dictionary at each step

| Step | Code | Dictionary after the step |
|------|------|---------------------------|
| 1 | Create | *(empty)* |
| 2 | `Add` x 4 | `101: Rahim`, `102: Karim`, `103: Hasan`, `104: Sakib` |
| 3 | `ContainsKey(102)` | Unchanged. Returns `true` |
| 4 | `ContainsValue("Hasan")` | Unchanged. Returns `true` |
| 5 | `TryGetValue(103, out name)` | Unchanged. `name` = `"Hasan"` |
| 6 | `Remove(104)` | `101: Rahim`, `102: Karim`, `103: Hasan` |
| 7 to 9 | `Keys`, `Values`, `Count` | Unchanged |
| 10 | `Clear()` | *(empty, Count = 0)* |

### Members explained

| # | Member | What it does | Returns |
|---|--------|--------------|---------|
| 2 | `Add(key, value)` | Adds a new pair | nothing. Throws an exception if the **key already exists** |
| 3 | `ContainsKey(key)` | Does this **key** exist? | `bool` |
| 4 | `ContainsValue(value)` | Does this **value** exist? | `bool` |
| 5 | `TryGetValue(key, out value)` | Gets the value for a key **safely** | `bool`: `true` if found, and the value comes out through `out` |
| 6 | `Remove(key)` | Removes the pair with this key | `bool`: `true` if it was removed, `false` if the key was not found |
| 7 | `Keys` | All the keys | A collection of keys |
| 8 | `Values` | All the values | A collection of values |
| 9 | `Count` | Number of pairs | `int` |
| 10 | `Clear()` | Removes **all** pairs | nothing |

### Notes

- **`TryGetValue` and `out string name`:** the method returns `true` or `false` **and** puts the value into `name`. This avoids an exception when the key is missing. `name` can be used inside the `if` block.
- **`ContainsKey` vs `ContainsValue`:** `ContainsKey` is **fast** (it uses the hash). `ContainsValue` has to **check every item one by one**, so it is slower on a large dictionary.
- **`Remove` returns `bool`**, which allows `if (employees.Remove(104))` to confirm the deletion.
- After `Clear()`, the dictionary still exists, but `Count` is `0`.

---

## 5. Good to Know

### Add, indexer, and TryGetValue

| Action | Code | If the key exists | If the key does not exist |
|--------|------|-------------------|---------------------------|
| **Add** a pair | `d.Add(101, "Rahim")` | **Throws** `ArgumentException` | Adds it |
| **Add or update** | `d[101] = "Rahim"` | **Replaces** the value | **Adds** it |
| **Read** with the indexer | `d[101]` | Returns the value | **Throws** `KeyNotFoundException` |
| **Read** safely | `d.TryGetValue(101, out v)` | `true`, value in `v` | `false` (no exception) |

```csharp
var d = new Dictionary<int, string>();

d.Add(101, "Rahim");
d[101] = "Rahim Uddin";          // update the value of an existing key
d[102] = "Karim";                // adds a new key

// d.Add(101, "Another");        // ArgumentException: the key already exists
// Console.WriteLine(d[999]);    // KeyNotFoundException: the key does not exist
```

### Rules

- **Keys must be unique**, and a key **cannot be `null`**.
- **Values can repeat** and (for reference types) can be `null`.
- The **order of items is not guaranteed**. Do not depend on it.
- **Do not add or remove items while looping** through the dictionary with `foreach` (this throws an `InvalidOperationException`).
- Use **`TryGetValue`** when the key might not exist.

### Different types

```csharp
Dictionary<string, int> marks = new Dictionary<string, int>
{
    { "Rahim", 85 },
    { "Karim", 72 }
};

Console.WriteLine(marks["Rahim"]);        // 85
```

---

## 6. Quick Reference

| Task | Code |
|------|------|
| Create | `new Dictionary<int, string>()` |
| Add | `d.Add(key, value)` |
| Add or update | `d[key] = value` |
| Read | `d[key]` |
| Read safely | `d.TryGetValue(key, out value)` |
| Key exists? | `d.ContainsKey(key)` |
| Value exists? | `d.ContainsValue(value)` |
| Remove one | `d.Remove(key)` |
| Remove all | `d.Clear()` |
| All keys | `d.Keys` |
| All values | `d.Values` |
| Number of pairs | `d.Count` |
| Loop | `foreach (var item in d) { item.Key, item.Value }` |

---

## Summary

**SaaS product**
- The modules were visited **one by one** to understand the workflow: **purchase raises stock, a client invoice lowers stock and creates the client due, and a payment lowers the due**.
- The **client due problem** was investigated by calculating `total invoices - total payments` and comparing it with the due shown in the software.
- The fix was done in a safe order: **1. check the stock, 2. create the client invoice, 3. fix the client due**. The order matters because the invoice depends on correct stock, and the due depends on a correct invoice.
- Some data was **checked and entered**. Corrections should be made through **proper entries** so the history stays complete.

**C# Dictionary**
- A **`Dictionary<TKey, TValue>`** stores **key-value pairs** with **unique keys** and gives **fast lookup by key**.
- Core members: `Add`, `ContainsKey`, `ContainsValue`, `TryGetValue`, `Remove`, `Keys`, `Values`, `Count`, and `Clear`.
- `foreach` gives `KeyValuePair` items (`.Key` and `.Value`).
- `Add` throws if the key already exists, the indexer `d[key]` throws if the key is missing, and **`TryGetValue`** is the **safe** way to read.
- The order of the items is **not guaranteed**.