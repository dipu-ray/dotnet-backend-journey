# C# Collections: List&lt;T&gt;

**Date:** 24 November 2026

## Table of Contents
- [1. Introduction and Basics](#1-introduction-and-basics)
- [2. CRUD Operations](#2-crud-operations)
- [3. Searching and Checking](#3-searching-and-checking)
- [4. Sorting, Reversing, and Conversion](#4-sorting-reversing-and-conversion)
- [Summary](#summary)

---

## 1. Introduction and Basics

### Create a list and add items

```csharp
List<string> names = new List<string>();

names.Add("Rahim");
names.Add("Karim");
names.Add("Jamal");
names.Add("Hasan");
```

```
Index:     0        1        2        3
        ┌────────┬────────┬────────┬────────┐
names = │ Rahim  │ Karim  │ Jamal  │ Hasan  │      names.Count = 4
        └────────┴────────┴────────┴────────┘
```

---

## 2. CRUD Operations

| Operation | `List<T>` members |
|-----------|-------------------|
| **Create** | `new List<T> { ... }`, `Add`, `Insert` |
| **Read** | `list[index]`, `Count`, `for` / `foreach` |
| **Update** | `list[index] = value` |
| **Delete** | `Remove`, `RemoveAt` |

### Code

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        // Declare list
        var names = new List<string>
        {
            "Rahim",
            "Karim",
            "Hasan",
            "Jamal"
        };

        // Insert and Add
        names.Insert(0, "Abrar");
        names.Add("Sakib");
        names.Add("Rakib");

        // Access
        Console.WriteLine(names[0]);

        // Update
        names[4] = "Junaid";

        // Remove
        names.Remove("Jamal");
        names.RemoveAt(1);

        // Total Students
        Console.WriteLine($"Total Students: {names.Count}");

        // Students List
        Console.WriteLine("\nStudents List: ");
        for (int i = 0; i < names.Count; i++)
        {
            Console.WriteLine($"Index: {i}, Name: {names[i]}");
        }
    }
}
```

### Output

```
Abrar
Total Students: 6

Students List: 
Index: 0, Name: Abrar
Index: 1, Name: Karim
Index: 2, Name: Hasan
Index: 3, Name: Junaid
Index: 4, Name: Sakib
Index: 5, Name: Rakib
```

---

## 3. Searching and Checking

### Code

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        var marks = new List<int>
        {
            45, 60, 75, 60, 85, 90, 30
        };

        // 1. Contains()
        bool isExists = marks.Contains(75);
        Console.WriteLine("Contains 75: " + isExists);

        // 2. IndexOf()
        int firstIndex = marks.IndexOf(60);
        Console.WriteLine("First Index of 60: " + firstIndex);

        // 3. LastIndexOf()
        int lastIndex = marks.LastIndexOf(60);
        Console.WriteLine("Last Index of 60: " + lastIndex);

        // 4. Find()
        int firstMatch = marks.Find(x => x > 70);
        Console.WriteLine("First Mark Greater Than 70: " + firstMatch);

        // 5. FindAll()
        List<int> allMatches = marks.FindAll(x => x > 70);
        Console.WriteLine("All Marks Greater Than 70:");
        foreach (int mark in allMatches)
        {
            Console.WriteLine(mark);
        }

        // 6. Exists()
        bool hasAbove80 = marks.Exists(x => x > 80);
        Console.WriteLine("Any Mark Above 80: " + hasAbove80);

        // 7. TrueForAll()
        bool allPassed = marks.TrueForAll(x => x >= 33);
        Console.WriteLine("All Students Passed: " + allPassed);
    }
}
```

### Output

```
Contains 75: True
First Index of 60: 1
Last Index of 60: 3
First Mark Greater Than 70: 75
All Marks Greater Than 70:
75
85
90
Any Mark Above 80: True
All Students Passed: False
```

---

## 4. Sorting, Reversing, and Conversion

### Code

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        // Original List
        var numbers = new List<int>
        {
            50, 20, 40, 10, 30, 60
        };
        Console.WriteLine("Original List:");
        foreach (int n in numbers)
        {
            Console.Write(n + " ");
        }

        // 1. Sort()
        numbers.Sort();
        Console.WriteLine("\n\nAfter Sort() - Ascending:");
        foreach (int n in numbers)
        {
            Console.Write(n + " ");
        }

        // 2. Reverse()
        numbers.Reverse();
        Console.WriteLine("\n\nAfter Reverse() - Descending:");
        foreach (int n in numbers)
        {
            Console.Write(n + " ");
        }

        // 3. ToArray()
        int[] numberArray = numbers.ToArray();
        Console.WriteLine("\n\nAfter ToArray():");
        foreach (int n in numberArray)
        {
            Console.Write(n + " ");
        }
        Console.WriteLine("\nArray Length: " + numberArray.Length);

        // 4. GetRange()
        List<int> selectedNumbers = numbers.GetRange(1, 3);
        Console.WriteLine("\nAfter GetRange(1, 3):");
        foreach (int n in selectedNumbers)
        {
            Console.Write(n + " ");
        }

        // 5. RemoveRange()
        numbers.RemoveRange(1, 2);
        Console.WriteLine("\n\nAfter RemoveRange(1, 2):");
        foreach (int n in numbers)
        {
            Console.Write(n + " ");
        }

        // 6. ConvertAll()
        List<string> stringNumbers = numbers.ConvertAll(x => x.ToString());
        Console.WriteLine("\n\nAfter ConvertAll():");
        foreach (string n in stringNumbers)
        {
            Console.Write(n + " ");
        }

        Console.WriteLine("\nTotal String Elements: " + stringNumbers.Count);
    }
}
```

### Output

```
Original List:
50 20 40 10 30 60 

After Sort() - Ascending:
10 20 30 40 50 60 

After Reverse() - Descending:
60 50 40 30 20 10 

After ToArray():
60 50 40 30 20 10 
Array Length: 6

After GetRange(1, 3):
50 40 30 

After RemoveRange(1, 2):
60 30 20 10 

After ConvertAll():
60 30 20 10 
Total String Elements: 4
```

---

## Summary

- **`List<T>`** is an **ordered, resizable** collection of items of one type. It lives in `System.Collections.Generic`, uses **`Count`** (an array uses `Length`), and starts at **index `0`**.
- **CRUD:** create with `Add` / `Insert` / an initializer, read with `list[i]`, update with `list[i] = value`, delete with `Remove` / `RemoveAt`. Indexes **shift** after an insert or a removal. `Remove(item)` returns `false` when the item is not in the list.
- **Searching and checking:** `Contains`, `IndexOf`, `LastIndexOf` (find by **value**), `Find`, `FindAll`, `Exists`, and `TrueForAll` (find by **condition**, written as a lambda such as `x => x > 70`).
- **Sorting, reversing, and conversion:** `Sort()` and `Reverse()` change the list in place. `ToArray()`, `GetRange()`, and `ConvertAll()` create **new** collections. `RemoveRange()` changes the original.
- `Sort()` followed by `Reverse()` gives **descending** order.