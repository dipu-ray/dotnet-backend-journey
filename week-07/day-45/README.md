# C# Collections: HashSet, and the Animal Fair Visit

**Date:** 26 November 2026

Today's work:

1. **C# Collections:** the **`HashSet<T>`** (basic concepts, basic methods, and basic operations)
2. **After the lunch break:** a visit to the **Animal Fair**

## Table of Contents
- [Part 1: C# Collections, HashSet](#part-1-c-collections-hashset)
  - [Section 1: Basic Concepts](#section-1-basic-concepts)
  - [Section 2: Basic Methods](#section-2-basic-methods)
  - [Section 3: Basic Operations](#section-3-basic-operations)
- [Part 2: Animal Fair Visit](#part-2-animal-fair-visit)
- [Summary](#summary)

---

# Part 1: C# Collections, HashSet

**Topic list covered**

| Section | Topics |
|---------|--------|
| **1. Basic Concepts** | 1. What is HashSet? 2. Why use HashSet? 3. HashSet vs List 4. HashSet vs Dictionary 5. Creating a HashSet |
| **2. Basic Methods** | 6. `Add()` 7. `Remove()` 8. `Contains()` 9. `Count` 10. `Clear()` |
| **3. Basic Operations** | 11. `foreach` loop with HashSet 12. `UnionWith()` 13. `IntersectWith()` 14. `ExceptWith()` 15. `ToList()` and `ToArray()` |

> **About the order of items:** a `HashSet` does **not guarantee any order**. The outputs below show the order normally seen when items are only added. Do not depend on this order in a program.

---

## Section 1: Basic Concepts

### 1. What is HashSet?

HashSet is a collection in C# that stores only unique values.
It does not allow duplicate items.

### 2. Why Use HashSet?

- Stores only unique values.
- Automatically prevents duplicate items.
- Provides fast searching using Contains().
- Useful for storing unique data.

### 3. HashSet vs List

| Feature | HashSet | List |
|---|---|---|
| Duplicate | Not Allowed | Allowed |
| Index | Not Available | Available |
| Order | No guaranteed order | Insertion order |
| Search | Generally Fast | Generally Slower |
| Use | Unique Data | Multiple Data |

### 4. HashSet vs Dictionary

| Feature | HashSet | Dictionary |
|---|---|---|
| Data | Unique Values | Key-Value Pairs |
| Duplicate | No Duplicate Values | No Duplicate Keys |
| Access | Contains() | Using Key |
| Use | Unique Items | Store Data with Keys |

### 5. Creating a HashSet

First, import the required namespace:

```csharp
using System.Collections.Generic;
```

#### Create an Empty HashSet

```csharp
HashSet<int> numbers = new HashSet<int>();
```

#### Create a HashSet with Initial Values

```csharp
HashSet<string> fruits = new HashSet<string>()
{
    "Apple",
    "Banana",
    "Mango"
};
```

#### Add Items

```csharp
fruits.Add("Orange");
```

#### Display Items

```csharp
foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
```

---

### Code: Basic Concepts example

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // 1. Creating a HashSet
        HashSet<string> students = new HashSet<string>();

        // 2. Adding items
        students.Add("Rahim");
        students.Add("Karim");
        students.Add("Sakib");
        students.Add("Rahim"); // Duplicate item
        Console.WriteLine("=== HashSet Example ===");

        // Display HashSet items
        Console.WriteLine("\nStudents List:");
        foreach (string student in students)
        {
            Console.WriteLine(student);
        }

        // 3. Why use HashSet?
        Console.WriteLine("\n=== Duplicate Check ===");
        bool added = students.Add("Rahim");
        if (added)
        {
            Console.WriteLine("Rahim added successfully.");
        }
        else
        {
            Console.WriteLine("Rahim already exists!");
        }

        // 4. Checking an item
        Console.WriteLine("\n=== Contains Check ===");
        if (students.Contains("Karim"))
        {
            Console.WriteLine("Karim exists in the HashSet.");
        }

        // 5. HashSet vs List
        List<string> studentList = new List<string>();
        studentList.Add("Rahim");
        studentList.Add("Karim");
        studentList.Add("Rahim");
        Console.WriteLine("\n=== List Example ===");
        foreach (string student in studentList)
        {
            Console.WriteLine(student);
        }

        // 6. HashSet vs Dictionary
        Dictionary<int, string> studentDictionary = new Dictionary<int, string>();
        studentDictionary.Add(1, "Rahim");
        studentDictionary.Add(2, "Karim");
        studentDictionary.Add(3, "Sakib");
        Console.WriteLine("\n=== Dictionary Example ===");
        foreach (var student in studentDictionary)
        {
            Console.WriteLine(
                "ID: " + student.Key +
                ", Name: " + student.Value
            );
        }

        // 7. Removing an item
        students.Remove("Sakib");
        Console.WriteLine("\n=== After Removing Sakib ===");
        foreach (string student in students)
        {
            Console.WriteLine(student);
        }
        Console.WriteLine("\nTotal Students: " + students.Count);
    }
}
```

### Output

```
=== HashSet Example ===

Students List:
Rahim
Karim
Sakib

=== Duplicate Check ===
Rahim already exists!

=== Contains Check ===
Karim exists in the HashSet.

=== List Example ===
Rahim
Karim
Rahim

=== Dictionary Example ===
ID: 1, Name: Rahim
ID: 2, Name: Karim
ID: 3, Name: Sakib

=== After Removing Sakib ===
Rahim
Karim

Total Students: 2
```

---

## Section 2: Basic Methods

| # | Method / member | What it does | Returns |
|---|-----------------|--------------|---------|
| 6 | `Add(item)` | Adds an item | `true` if added, `false` if it **already exists** |
| 7 | `Remove(item)` | Removes the item | `true` if removed, `false` if it was **not found** |
| 8 | `Contains(item)` | Is the item in the set? | `bool` |
| 9 | `Count` | The **number of items** | `int` |
| 10 | `Clear()` | Removes **all** items | nothing |

### Code

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create HashSet
        HashSet<string> students = new HashSet<string>();

        // 1. Add()
        students.Add("Rahim");
        students.Add("Karim");
        students.Add("Sakib");
        students.Add("Rahim"); // Duplicate
        Console.WriteLine("=== All Students ===");
        foreach (string student in students)
        {
            Console.WriteLine(student);
        }

        // 2. Count
        Console.WriteLine("\nTotal Students: " + students.Count);

        // 3. Contains()
        Console.WriteLine("\n=== Student Search ===");
        if (students.Contains("Karim"))
        {
            Console.WriteLine("Karim is available.");
        }
        else
        {
            Console.WriteLine("Karim is not available.");
        }

        // 4. Remove()
        students.Remove("Sakib");
        Console.WriteLine("\n=== After Removing Sakib ===");
        foreach (string student in students)
        {
            Console.WriteLine(student);
        }
        Console.WriteLine("Total Students: " + students.Count);

        // 5. Clear()
        students.Clear();
        Console.WriteLine("\n=== After Clear ===");
        Console.WriteLine("Total Students: " + students.Count);
    }
}
```

### Output

```
=== All Students ===
Rahim
Karim
Sakib

Total Students: 3

=== Student Search ===
Karim is available.

=== After Removing Sakib ===
Rahim
Karim
Total Students: 2

=== After Clear ===
Total Students: 0
```

---

## Section 3: Basic Operations

### Operations explained

| # | Method | Meaning | Result with A and B | Changes the set it is called on? |
|---|--------|---------|---------------------|----------------------------------|
| 11 | `foreach` | Goes through every item | | No |
| 12 | `UnionWith(other)` | **Union**: all items that are in **either** set | `10, 20, 30, 40, 50, 60` | **Yes** |
| 13 | `IntersectWith(other)` | **Intersection**: only items that are in **both** sets | `30, 40` | **Yes** |
| 14 | `ExceptWith(other)` | **Difference**: removes the items that are in `other` | `10, 20` (A without B) | **Yes** |
| 15 | `ToList()` / `ToArray()` | Copies the items into a **List** or an **array** | `10, 20, 30, 40` | No |

### Code

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Create two HashSets
        HashSet<int> setA = new HashSet<int>()
        {
            10, 20, 30, 40
        };
        HashSet<int> setB = new HashSet<int>()
        {
            30, 40, 50, 60
        };

        // 1. foreach Loop
        Console.WriteLine("=== Set A ===");
        foreach (int number in setA)
        {
            Console.WriteLine(number);
        }

        // 2. UnionWith()
        HashSet<int> union = new HashSet<int>(setA);
        union.UnionWith(setB);
        Console.WriteLine("\n=== Union ===");
        foreach (int number in union)
        {
            Console.WriteLine(number);
        }

        // 3. IntersectWith()
        HashSet<int> intersect = new HashSet<int>(setA);
        intersect.IntersectWith(setB);
        Console.WriteLine("\n=== Intersection ===");
        foreach (int number in intersect)
        {
            Console.WriteLine(number);
        }

        // 4. ExceptWith()
        HashSet<int> except = new HashSet<int>(setA);
        except.ExceptWith(setB);
        Console.WriteLine("\n=== Except ===");
        foreach (int number in except)
        {
            Console.WriteLine(number);
        }

        // 5. ToList()
        List<int> myList = setA.ToList();
        Console.WriteLine("\n=== ToList ===");
        foreach (int number in myList)
        {
            Console.WriteLine(number);
        }

        // 6. ToArray()
        int[] myArray = setA.ToArray();
        Console.WriteLine("\n=== ToArray ===");
        foreach (int number in myArray)
        {
            Console.WriteLine(number);
        }
    }
}
```

### Output

```
=== Set A ===
10
20
30
40

=== Union ===
10
20
30
40
50
60

=== Intersection ===
30
40

=== Except ===
10
20

=== ToList ===
10
20
30
40

=== ToArray ===
10
20
30
40
```

---

# Part 2: Animal Fair Visit

After the lunch break, the work stopped for a **visit to the Animal Fair**.

## 1. Visit Overview

| Field | Details |
|-------|---------|
| Event | **Animal Fair** |
| Date | 26 November 2026 |
| Time | After the lunch break, from about 2PM to 6PM |
| Place | Agargaon, Dhaka, Bangladesh |
| Went with | Team members & with Sir |
| Purpose | See the fair, and learn what was on show, including the **farm software stalls** |

## 2. What Was at the Fair

| Area | What was there |
|------|----------------|
| **Government stalls** | A number of stalls run by government offices |
| **Animal stalls** | Stalls with animals, covering **almost all kinds of animals** |
| **Farm software stalls** | **A few stalls (about 2 to 6)** showing software for **farms** |

---

# Summary

**C# HashSet**
- A **`HashSet<T>`** stores **unique items only**, has **no index**, and does **not guarantee order**. It is **fast** for `Contains` because it uses hashing.
- **HashSet vs List:** a List keeps order and allows duplicates. A HashSet removes duplicates and is faster to search.
- **HashSet vs Dictionary:** a Dictionary stores **key-value pairs**. A HashSet stores **single values** and answers "does it exist?".
- **Methods:** `Add` and `Remove` return **`bool`**. `Contains`, `Count`, and `Clear` complete the basics.
- **Set operations:** **`UnionWith`** (all items), **`IntersectWith`** (common items), and **`ExceptWith`** (items not in the other set). They **change the set** they are called on, so **copy it first** if the original is needed.
- `ToList()` and `ToArray()` (with `using System.Linq;`) copy a HashSet into a List or an array.

**Animal Fair visit**
- After lunch, the fair was visited: **government stalls**, **animal stalls**, and **farm software stalls**, followed by a **discussion and memories**.
- The farm software stalls are useful to compare with the farm management software studied before.