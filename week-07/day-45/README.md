# C# Collections: HashSet, and the Animal Fair Visit

**Date:** 26 November 2026

Today's work:

1. **C# Collections:** the **`HashSet<T>`** (basic concepts, basic methods, and basic operations)
2. **After the lunch break:** a visit to the **Animal Fair**

> **Note:** I do not have the details of what was seen at the fair (stall names, products, people), so Part 2 records the structure of the visit from your notes and has **fill-in spaces** (`[ ... ]`) for the details.

## Table of Contents
- [Part 1: C# Collections, HashSet](#part-1-c-collections-hashset)
  - [Section 1: Basic Concepts](#section-1-basic-concepts)
  - [Section 2: Basic Methods](#section-2-basic-methods)
  - [Section 3: Basic Operations](#section-3-basic-operations)
  - [Quick Reference](#quick-reference)
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

A **`HashSet<T>`** is a collection that stores **unique items only**. If the same item is added twice, it is stored **once**.

```csharp
using System.Collections.Generic;       // HashSet<T> is in this namespace
```

| Feature | Detail |
|---------|--------|
| **Unique items** | **No duplicates.** Adding an existing item does nothing |
| **No index** | Items cannot be read by position (`set[0]` is **not** allowed) |
| **No guaranteed order** | The order of the items is not fixed |
| **Fast lookup** | It uses **hashing**, so `Contains` is very fast |
| **Generic** | `HashSet<string>`, `HashSet<int>` |
| **Dynamic size** | Grows and shrinks automatically |

```
Added:   Rahim, Karim, Sakib, Rahim
Stored:  Rahim, Karim, Sakib        (the second "Rahim" is ignored)
```

### 2. Why use HashSet?

| Use | Example |
|-----|---------|
| **Keep only unique values** | Unique student names, unique email addresses |
| **Remove duplicates** from another collection | `new HashSet<int>(listWithDuplicates)` |
| **Check quickly if an item exists** | Is this user already registered? |
| **Set operations** | Union, intersection, and difference of two groups |

### 3. HashSet vs List

| | `List<T>` | `HashSet<T>` |
|---|-----------|--------------|
| Duplicates | **Allowed** | **Not allowed** |
| Order | **Kept** (the order of adding) | **Not guaranteed** |
| Access by index (`list[0]`) | **Yes** | **No** |
| `Contains` speed | Slower (checks one by one) | **Very fast** (hashing) |
| Add method | `Add` returns nothing | `Add` returns **`bool`** (`true` if added, `false` if it already existed) |
| Set operations | Not built in | **Built in** (`UnionWith`, `IntersectWith`, `ExceptWith`) |
| Use when | You need order, an index, or duplicates | You need **uniqueness** and **fast checks** |

### 4. HashSet vs Dictionary

| | `Dictionary<TKey, TValue>` | `HashSet<T>` |
|---|----------------------------|--------------|
| Stores | **Key-value pairs** | **Single values** |
| Uniqueness | Keys are unique | **Values** are unique |
| Lookup | By **key**, returns the **value** | `Contains(item)`, returns **true or false** |
| Example | `1 -> "Rahim"` | `"Rahim"` |
| Use when | You need to **find a value** from a key (ID to name) | You only need to know **whether an item exists**, and to keep items unique |

Both use **hashing**, so both are fast.

### 5. Creating a HashSet

```csharp
// 1. Empty HashSet
HashSet<string> students = new HashSet<string>();

// 2. With values (collection initializer)
HashSet<int> numbers = new HashSet<int>() { 10, 20, 30 };

// 3. From another collection (duplicates are removed)
List<int> list = new List<int> { 1, 2, 2, 3, 3, 3 };
HashSet<int> unique = new HashSet<int>(list);      // 1, 2, 3
```

> By default, text comparison is **case sensitive**: `"rahim"` and `"Rahim"` are **different** items. To ignore case, use `new HashSet<string>(StringComparer.OrdinalIgnoreCase)`.

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

### What the example shows

| Part | Result | Lesson |
|------|--------|--------|
| `Add("Rahim")` a second time | `Rahim` appears **once** in the HashSet | A HashSet **ignores duplicates** |
| `bool added = students.Add("Rahim")` | `false`, so `Rahim already exists!` | `Add` **returns `bool`**, which is a simple duplicate check |
| `Contains("Karim")` | `true` | A quick existence check |
| The `List` with `Rahim` twice | `Rahim` is printed **twice** | A **List allows duplicates** |
| The `Dictionary` | Prints `ID` and `Name` | A Dictionary stores **key-value pairs** |
| `Remove("Sakib")` | `Rahim` and `Karim` remain, `Count` is `2` | `Remove` deletes an item |

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

### The set at each step

| Step | Code | Set after the step | Count |
|------|------|--------------------|-------|
| 1 | `Add` four times (`Rahim` twice) | `Rahim, Karim, Sakib` | **3** (not 4) |
| 2 | `Count` | Unchanged | 3 |
| 3 | `Contains("Karim")` | Unchanged | 3 |
| 4 | `Remove("Sakib")` | `Rahim, Karim` | 2 |
| 5 | `Clear()` | *(empty)* | 0 |

**Notes**
- `Count` is **3**, although `Add` was called four times, because the duplicate `Rahim` was ignored.
- `Add` and `Remove` return a `bool`, so the result can be checked: `if (students.Remove("Sakib")) { ... }`.
- `Clear()` empties the set, but the set object still exists and can be used again.

---

## Section 3: Basic Operations

Sets support **mathematical set operations**. The examples use two sets:

```
Set A = { 10, 20, 30, 40 }
Set B = { 30, 40, 50, 60 }
```

```
        A                 B
   ┌─────────┐       ┌─────────┐
   │ 10  20  │ 30 40 │  50  60 │
   └─────────┘       └─────────┘
                 common
```

### Operations explained

| # | Method | Meaning | Result with A and B | Changes the set it is called on? |
|---|--------|---------|---------------------|----------------------------------|
| 11 | `foreach` | Goes through every item | | No |
| 12 | `UnionWith(other)` | **Union**: all items that are in **either** set | `10, 20, 30, 40, 50, 60` | **Yes** |
| 13 | `IntersectWith(other)` | **Intersection**: only items that are in **both** sets | `30, 40` | **Yes** |
| 14 | `ExceptWith(other)` | **Difference**: removes the items that are in `other` | `10, 20` (A without B) | **Yes** |
| 15 | `ToList()` / `ToArray()` | Copies the items into a **List** or an **array** | `10, 20, 30, 40` | No |

### Important: these methods change the set

`UnionWith`, `IntersectWith`, and `ExceptWith` **modify the set they are called on**. To keep the original, **make a copy first** with the copy constructor:

```csharp
HashSet<int> union = new HashSet<int>(setA);     // a copy of setA
union.UnionWith(setB);                           // changes the copy; setA stays the same
```

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

### How each result is built

| Operation | Starting copy of A | What happens | Result |
|-----------|--------------------|--------------|--------|
| **Union** | `10, 20, 30, 40` | Adds the items of B that are missing (`50`, `60`). `30` and `40` already exist | `10, 20, 30, 40, 50, 60` |
| **Intersection** | `10, 20, 30, 40` | Removes the items that are **not** in B (`10`, `20`) | `30, 40` |
| **Except** | `10, 20, 30, 40` | Removes the items that **are** in B (`30`, `40`) | `10, 20` |

**Notes**
- `ToList()` and `ToArray()` need **`using System.Linq;`**. They make a **copy**, so `setA` is not changed. The output shows that `setA` still has all four items after the other operations, because each operation worked on a **copy**.
- `foreach` is the way to read a HashSet, because there is **no index**.
- Converting to a **List** or an **array** is useful when an **index**, sorting, or a fixed order is needed.

---

## Quick Reference

| Task | Code |
|------|------|
| Create | `new HashSet<string>()` |
| Create from a collection (removes duplicates) | `new HashSet<int>(list)` |
| Add (returns `bool`) | `set.Add(item)` |
| Remove (returns `bool`) | `set.Remove(item)` |
| Exists? | `set.Contains(item)` |
| Number of items | `set.Count` |
| Remove all | `set.Clear()` |
| Loop | `foreach (var item in set)` |
| Union (modifies) | `set.UnionWith(other)` |
| Intersection (modifies) | `set.IntersectWith(other)` |
| Difference (modifies) | `set.ExceptWith(other)` |
| To a List | `set.ToList()` (needs `System.Linq`) |
| To an array | `set.ToArray()` (needs `System.Linq`) |

---

# Part 2: Animal Fair Visit

After the lunch break, the work stopped for a **visit to the Animal Fair**.

## 1. Visit Overview

| Field | Details |
|-------|---------|
| Event | **Animal Fair** |
| Date | 26 November 2026 |
| Time | After the lunch break, [from about ___ to ___] |
| Place | [Venue and area] |
| Went with | [Team members] |
| Purpose | See the fair, and learn what was on show, including the **farm software stalls** |

## 2. What Was at the Fair

| Area | What was there |
|------|----------------|
| **Government stalls** | A number of stalls run by government offices |
| **Animal stalls** | Stalls with animals, covering **almost all kinds of animals** |
| **Farm software stalls** | **A few stalls (about 2 to 6)** showing software for **farms** |

## 3. Government Stalls

| # | Stall / department | What it offered or showed | Notes |
|---|--------------------|---------------------------|-------|
| 1 | [Name] | [Services, information, schemes] | [ ] |
| 2 | [Name] | [ ] | [ ] |
| 3 | [Name] | [ ] | [ ] |

**Useful information collected:** [Services, support for farmers, contacts, leaflets]

## 4. Animal Stalls

Almost all kinds of animals were on show.

| Animal / group | What I saw | Notes |
|----------------|------------|-------|
| [Cattle, goats, sheep] | [ ] | [ ] |
| [Poultry, ducks, birds] | [ ] | [ ] |
| [Fish] | [ ] | [ ] |
| [Other animals] | [ ] | [ ] |

**Things that stood out:** [Breeds, sizes, care methods, anything new]

## 5. Farm Software Stalls

A **small number of stalls (2 to 6)** showed **farm software**. This is closely related to the **business and farm management software** studied earlier, so the stalls are worth recording carefully.

| # | Company / product | What it does | Main features | How it was demonstrated | Notes |
|---|-------------------|--------------|---------------|-------------------------|-------|
| 1 | [Name] | [ ] | [ ] | [ ] | [ ] |
| 2 | [Name] | [ ] | [ ] | [ ] | [ ] |
| 3 | [Name] | [ ] | [ ] | [ ] | [ ] |
| 4 | [Name] | [ ] | [ ] | [ ] | [ ] |
| 5 | [Name] | [ ] | [ ] | [ ] | [ ] |
| 6 | [Name] | [ ] | [ ] | [ ] | [ ] |

**What to compare between the products**

| Point | What to note |
|-------|--------------|
| **Features** | Fields and crops, livestock, production, stock, expenses, sales, reports |
| **Users** | Farmers, farm managers, workers |
| **Platform** | Web, mobile app, or desktop |
| **Language and ease of use** | Is it easy for farm workers to use? |
| **Pricing** | Subscription, one-time, or free plan |
| **Support** | Training, help, and contact |
| **Good ideas to learn from** | [ ] |
| **What is missing** | [ ] |

## 6. Memories and Discussion

At the end of the visit, the group talked about the fair and shared memories.

| Topic | Notes |
|-------|-------|
| **Best moments** | [ ] |
| **What I learned** | [ ] |
| **Discussion about the fair** | [ ] |
| **Ideas for our own work** (for example, features for farm software) | [ ] |
| **Photos / memories** | [ ] |

## 7. Summary of the Visit

- The visit to the **Animal Fair** took place **after the lunch break**.
- The fair had **government stalls**, **animal stalls** with almost all animals, and **2 to 6 farm software stalls**.
- The **farm software stalls** were useful to compare with the business and farm management software studied earlier.
- The day ended with **memories and a discussion** about the fair.

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