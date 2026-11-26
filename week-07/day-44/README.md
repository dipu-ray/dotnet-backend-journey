# SaaS Product Issue Fixing and C# Dictionary

**Date:** 25 November 2026

Today's work:

1. **SaaS Product:** fixing some issues (checking the module workflows, the **client due** problem, and data checking and entry)
2. **C# Advanced:** **Collections**, the **`Dictionary`**

> **Note:** I do not have the exact details of the SaaS issue (which client, which amounts, what the cause was), so Part 1 documents the **method and the workflow** and has **fill-in spaces** (`[ ... ]`) for the actual findings.

## Table of Contents
- [Part 1: SaaS Product, Fixing Issues](#part-1-saas-product-fixing-issues)
  - [1. Summary of the Work](#1-summary-of-the-work)
- [Part 2: C# Collections, Dictionary](#part-2-c-collections-dictionary)
  - [1. What is a Dictionary?](#1-what-is-a-dictionary)
  - [2. Basics: Create, Add, and Read](#2-basics-create-add-and-read)
  - [3. Looping Through a Dictionary](#3-looping-through-a-dictionary)
  - [4. Dictionary Operations](#4-dictionary-operations)
  - [5. Quick Reference](#5-quick-reference)
- [Summary](#summary)

---

# Part 1: SaaS Product, Fixing Issues

## 1. Summary of the Work

| # | Task | Status |
|---|------|--------|
| 1 | Visit and understand the **workflow**, one module at a time | Done |
| 2 | **Check the client due** and find the problem | Done |
| 3 | Fix the problem in the order: **1. check stock, 2. create the client invoice, 3. fix the client due** | Done |
| 4 | **Check and enter** some data in the SaaS product | Done |

### What I learned
- The **modules are connected**: a **client invoice** affects **stock** and the **client due**, and a **payment** affects the due.
- To fix a due problem, **find the exact difference** by calculating `invoices - payments`, then find the record that explains it.
- Fix in a **safe order**: **stock first**, then the **invoice**, then the **due**.

---

# Part 2: C# Collections, Dictionary

## 1. What is a Dictionary?

A **`Dictionary<TKey, TValue>`** stores **key-value pairs**. Each value is found through its **unique key**, like a real dictionary where a **word** (key) leads to its **meaning** (value).

```csharp
using System.Collections.Generic;       // Dictionary is in this namespace
```

```
Key  ->  Value
101  ->  Rahim
102  ->  Karim
103  ->  Dipu
```

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

---

## 5. Quick Reference

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