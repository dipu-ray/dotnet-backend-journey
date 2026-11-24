# C# Collections: List&lt;T&gt;

**Date:** 20 November 2026

Documentation of today's work on the **`List<T>`** collection in C#:

1. Introduction and basics
2. CRUD operations (create, read, update, delete)
3. Searching and checking
4. Sorting, reversing, and conversion

## Table of Contents
- [1. Introduction and Basics](#1-introduction-and-basics)
- [2. CRUD Operations](#2-crud-operations)
- [3. Searching and Checking](#3-searching-and-checking)
- [4. Sorting, Reversing, and Conversion](#4-sorting-reversing-and-conversion)
- [5. Quick Reference](#5-quick-reference)
- [Summary](#summary)

---

## 1. Introduction and Basics

### What is a collection?

A **collection** is an object that **stores a group of related items**. **`List<T>`** is the most commonly used collection. It is an **ordered, resizable list** of items of **one type** (`T`).

```csharp
using System.Collections.Generic;       // List<T> is in this namespace
```

| Feature | Detail |
|---------|--------|
| **Generic** | `List<T>`: `T` is the type of the items (`List<string>`, `List<int>`) |
| **Dynamic size** | It **grows and shrinks** automatically |
| **Ordered** | Items keep the order in which they were added |
| **Indexed** | Items are reached by index, starting at **`0`** |
| **Duplicates** | Allowed |

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

### Other ways to create a list

```csharp
// 1. Empty list, items added later
List<string> a = new List<string>();

// 2. Collection initializer: values given at creation
List<string> b = new List<string> { "Rahim", "Karim" };

// 3. With var
var c = new List<int> { 10, 20, 30 };
```

### Basic members

| Member | Meaning | Example |
|--------|---------|---------|
| `Add(item)` | Adds an item at the **end** | `names.Add("Sakib")` |
| `Count` | The **number of items** | `names.Count` |
| `list[i]` | Reads or changes the item at index `i` | `names[0]` |
| `foreach` / `for` | Goes through the items | see below |

```csharp
foreach (string name in names)
{
    Console.WriteLine(name);
}
```

> Changing a list (adding or removing items) **inside a `foreach` over the same list** causes an `InvalidOperationException`. Use a `for` loop, or change a copy.

### Array vs List

| | Array | `List<T>` |
|---|-------|-----------|
| Size | **Fixed** when created | **Dynamic** (grows and shrinks) |
| Number of items | `Length` | `Count` |
| Add an item | Not possible after creation | `Add`, `Insert` |
| Remove an item | Not possible | `Remove`, `RemoveAt`, `RemoveRange` |
| Search, sort, convert | Through `Array` methods | **Built-in methods** (`Contains`, `Find`, `Sort`, ...) |
| Namespace | `System` | `System.Collections.Generic` |

---

## 2. CRUD Operations

**CRUD** means **C**reate, **R**ead, **U**pdate, and **D**elete.

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

### Step-by-step: how the list changes

| Step | Code | List after the step | Count |
|------|------|---------------------|-------|
| 1 | Declare | `Rahim, Karim, Hasan, Jamal` | 4 |
| 2 | `Insert(0, "Abrar")` | `Abrar, Rahim, Karim, Hasan, Jamal` | 5 |
| 3 | `Add("Sakib")` | `Abrar, Rahim, Karim, Hasan, Jamal, Sakib` | 6 |
| 4 | `Add("Rakib")` | `Abrar, Rahim, Karim, Hasan, Jamal, Sakib, Rakib` | 7 |
| 5 | `names[0]` | (read only) prints `Abrar` | 7 |
| 6 | `names[4] = "Junaid"` | `Abrar, Rahim, Karim, Hasan, Junaid, Sakib, Rakib` | 7 |
| 7 | `Remove("Jamal")` | **No change** (`Jamal` is no longer in the list) | 7 |
| 8 | `RemoveAt(1)` | `Abrar, Karim, Hasan, Junaid, Sakib, Rakib` | 6 |

### Methods explained

| Method | What it does | Detail |
|--------|--------------|--------|
| `Add(item)` | Adds at the **end** | |
| `Insert(index, item)` | Adds at a **given index** | The items from that index move one place to the **right**. The index can be `0` to `Count` |
| `names[i]` | **Reads** the item at index `i` | An invalid index throws `ArgumentOutOfRangeException` |
| `names[i] = value` | **Replaces** the item at index `i` | The old item is lost |
| `Remove(item)` | Removes the **first matching item** | Returns **`true`** if removed, **`false`** if the item was not found |
| `RemoveAt(index)` | Removes the item at an **index** | The items after it move one place to the **left** |
| `Count` | Number of items | |

**Notes from this example**
- `Insert(0, "Abrar")` put `Abrar` at the **front** and moved everyone else one place to the right, so `Jamal` moved from index `3` to index `4`.
- At step 6, `names[4] = "Junaid"` **replaced `Jamal`**. So at step 7, `Remove("Jamal")` finds **nothing** to remove and returns `false`. The list is not changed.
- `RemoveAt(1)` removed the item at index `1`, which was `Rahim`.
- After a removal, the **indexes change**. In the final list, `Karim` is at index `1`.

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

### The data

```
Index:    0    1    2    3    4    5    6
        ┌────┬────┬────┬────┬────┬────┬────┐
marks = │ 45 │ 60 │ 75 │ 60 │ 85 │ 90 │ 30 │
        └────┴────┴────┴────┴────┴────┴────┘
```

### Methods explained

| # | Method | What it does | Result here | If nothing matches |
|---|--------|--------------|-------------|--------------------|
| 1 | `Contains(value)` | Is the value in the list? | `True` (`75` exists) | `False` |
| 2 | `IndexOf(value)` | Index of the **first** match | `1` | `-1` |
| 3 | `LastIndexOf(value)` | Index of the **last** match | `3` | `-1` |
| 4 | `Find(condition)` | The **first item** that matches the condition | `75` | The **default value** (`0` for `int`, `null` for `string`) |
| 5 | `FindAll(condition)` | A **new list** of **all** matching items | `75, 85, 90` | An **empty list** |
| 6 | `Exists(condition)` | Does **at least one** item match? | `True` (`85` and `90`) | `False` |
| 7 | `TrueForAll(condition)` | Do **all** items match? | `False` (`30` is below `33`) | |

### The condition: a lambda expression

`x => x > 70` is a **lambda expression**: a short, unnamed method.

| Part | Meaning |
|------|---------|
| `x` | One item of the list, checked one by one |
| `=>` | "goes to" |
| `x > 70` | The condition. The result must be `true` or `false` |

```csharp
marks.Find(x => x > 70);          // first item greater than 70
marks.FindAll(x => x >= 60);      // all items 60 or more
marks.Exists(x => x == 90);       // is there a 90?
```

**Choosing a method**

| Question | Method |
|----------|--------|
| "Is this exact value in the list?" | `Contains` |
| "Where is this value?" | `IndexOf`, `LastIndexOf` |
| "Give me the first item that matches a rule" | `Find` |
| "Give me every item that matches a rule" | `FindAll` |
| "Does any item match a rule?" | `Exists` |
| "Do all items match a rule?" | `TrueForAll` |

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

(Each number is followed by a space, because the code uses `Console.Write(n + " ")`. The `\n\n` in the headings creates the blank lines.)

### Step-by-step: how the list changes

| Step | Code | `numbers` after the step | Notes |
|------|------|--------------------------|-------|
| 0 | Original | `50, 20, 40, 10, 30, 60` | |
| 1 | `Sort()` | `10, 20, 30, 40, 50, 60` | Sorted **in place**, ascending |
| 2 | `Reverse()` | `60, 50, 40, 30, 20, 10` | Order reversed **in place** |
| 3 | `ToArray()` | `numbers` is unchanged | A **new array** with a copy of the items |
| 4 | `GetRange(1, 3)` | `numbers` is unchanged | A **new list** `50, 40, 30` (3 items from index `1`) |
| 5 | `RemoveRange(1, 2)` | `60, 30, 20, 10` | Removed `50` and `40` (2 items from index `1`) |
| 6 | `ConvertAll(...)` | `numbers` is unchanged | A **new `List<string>`**: `"60", "30", "20", "10"` |

### Methods explained

| Method | What it does | Changes the original list? |
|--------|--------------|----------------------------|
| `Sort()` | Sorts the items in **ascending** order | **Yes** |
| `Reverse()` | **Reverses** the current order of the items | **Yes** |
| `ToArray()` | Copies the items into a **new array** | No |
| `GetRange(index, count)` | Returns a **new list** with `count` items starting at `index` | No |
| `RemoveRange(index, count)` | **Removes** `count` items starting at `index` | **Yes** |
| `ConvertAll(converter)` | Returns a **new list** with every item converted (here `int` to `string`) | No |

**Notes**
- **Descending order:** `Reverse()` does not sort. It only **reverses the current order**. It gave descending order here because the list was **sorted ascending first**. So `Sort()` followed by `Reverse()` = descending.
- **`GetRange(1, 3)`** takes **index** `1` and **count** `3` (not an end index): from `60, 50, 40, 30, 20, 10` it returns `50, 40, 30`.
- **`RemoveRange(1, 2)`** removes `2` items from index `1`: `50` and `40`. Because `GetRange` made a copy, `numbers` still had all six items before this step.
- **`ConvertAll(x => x.ToString())`** converts each `int` to a `string`. The new list has the **same number of items** (`4`) but a **different type**.

---

## 5. Quick Reference

| Method / member | Purpose | Returns | Changes the list? |
|-----------------|---------|---------|-------------------|
| `Add(item)` | Add at the end | nothing | Yes |
| `Insert(index, item)` | Add at an index | nothing | Yes |
| `list[i]` | Read an item | the item | No |
| `list[i] = value` | Replace an item | nothing | Yes |
| `Remove(item)` | Remove the first match | `bool` | Yes |
| `RemoveAt(index)` | Remove at an index | nothing | Yes |
| `RemoveRange(index, count)` | Remove several items | nothing | Yes |
| `Count` | Number of items | `int` | No |
| `Contains(value)` | Is the value present? | `bool` | No |
| `IndexOf(value)` | First index (or `-1`) | `int` | No |
| `LastIndexOf(value)` | Last index (or `-1`) | `int` | No |
| `Find(condition)` | First match | item (or default) | No |
| `FindAll(condition)` | All matches | new `List<T>` | No |
| `Exists(condition)` | Any match? | `bool` | No |
| `TrueForAll(condition)` | All match? | `bool` | No |
| `Sort()` | Sort ascending | nothing | Yes |
| `Reverse()` | Reverse the order | nothing | Yes |
| `ToArray()` | Copy to an array | `T[]` | No |
| `GetRange(index, count)` | Copy part of the list | new `List<T>` | No |
| `ConvertAll(converter)` | Convert every item | new `List<TOutput>` | No |

---

## Summary

- **`List<T>`** is an **ordered, resizable** collection of items of one type. It lives in `System.Collections.Generic`, uses **`Count`** (an array uses `Length`), and starts at **index `0`**.
- **CRUD:** create with `Add` / `Insert` / an initializer, read with `list[i]`, update with `list[i] = value`, delete with `Remove` / `RemoveAt`. Indexes **shift** after an insert or a removal. `Remove(item)` returns `false` when the item is not in the list.
- **Searching and checking:** `Contains`, `IndexOf`, `LastIndexOf` (find by **value**), `Find`, `FindAll`, `Exists`, and `TrueForAll` (find by **condition**, written as a lambda such as `x => x > 70`).
- **Sorting, reversing, and conversion:** `Sort()` and `Reverse()` change the list in place. `ToArray()`, `GetRange()`, and `ConvertAll()` create **new** collections. `RemoveRange()` changes the original.
- `Sort()` followed by `Reverse()` gives **descending** order.