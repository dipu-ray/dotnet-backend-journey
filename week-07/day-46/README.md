# C# Generics: Generic Methods

**Date:** 28 November 2026

## Table of Contents
- [1. Introduction](#1-introduction)
- [2. Generic Method Making](#2-generic-method-making)
- [3. T Uses in a Generic Method](#3-t-uses-in-a-generic-method)
- [4. Generic Method Parameter Uses](#4-generic-method-parameter-uses)
- [5. Multiple Type Parameters (T, U)](#5-multiple-type-parameters-t-u)
- [6. Generic Method Return Type](#6-generic-method-return-type)
- [7. Type Inference](#7-type-inference)
- [8. Explicit Type Argument Uses](#8-explicit-type-argument-uses)
- [9. All Topics Together](#9-all-topics-together)
- [10. Quick Reference](#10-quick-reference)
- [Summary](#summary)

---

## 1. Introduction

### What are generics?

**Generics** let you write code **once** that works with **many data types**. Instead of fixing a type like `int` or `string`, you use a **placeholder** (a **type parameter**, usually named `T`) that is replaced by a real type when the code is used.

### What is a generic method?

A **generic method** is a method that has **one or more type parameters** written in angle brackets `< >` after its name.

```csharp
static void Display<T>(T value)
{
    Console.WriteLine(value);
}
```

| Part | Meaning |
|------|---------|
| `Display` | The method name |
| `<T>` | The **type parameter**: a placeholder for a type |
| `T value` | A parameter whose type is `T` |

### Why use generic methods?

| Benefit | Explanation |
|---------|-------------|
| **Code reuse** | One method for every type, with no copy and paste |
| **Type safety** | The compiler checks the types. Wrong use is a **compile error**, not a run-time error |
| **No casting** | The result already has the correct type |
| **Performance** | No boxing and unboxing for value types (unlike `object`) |

---

## 2. Generic Method Making

### Code

```csharp
using System;

class Program
{
    static void Display<T>(T value)
    {
        Console.WriteLine(value);
    }

    static void Main()
    {
        Display<int>(100);
        Display<string>("Rahim");
        Display<double>(250.50);
        Display<bool>(true);
    }
}
```

### Output

```
100
Rahim
250.5
True
```

---

## 3. T Uses in a Generic Method

`T` can be used **anywhere a type can be used**: as a **parameter type**, as the **type of a local variable**, and as a **return type**.

### Code

```csharp
using System;

class Program
{
    static void Display<T>(T value)
    {
        T data = value;
        Console.WriteLine(data);
    }

    static void Main()
    {
        Display<int>(100);
        Display<string>("Rahim");
        Display<double>(250.50);
    }
}
```

### Output

```
100
Rahim
250.5
```

---

## 4. Generic Method Parameter Uses

The **same `T`** can be used for **several parameters**. All of them must then have the **same type**.

### Code

```csharp
using System;

class Program
{
    static void Display<T>(T first, T second)
    {
        Console.WriteLine($"First: {first}");
        Console.WriteLine($"Second: {second}");
    }

    static void Main()
    {
        Display<int>(10, 20);
        Display<string>("Rahim", "Karim");
        Display<double>(10.5, 20.5);
    }
}
```

### Output

```
First: 10
Second: 20
First: Rahim
Second: Karim
First: 10.5
Second: 20.5
```

---

## 5. Multiple Type Parameters (T, U)

A generic method can have **more than one type parameter**. Each one can be a **different type**.

### Code

```csharp
using System;

class Program
{
    static void Display<T, U>(T first, U second)
    {
        Console.WriteLine($"First: {first}");
        Console.WriteLine($"Second: {second}");
    }

    static void Main()
    {
        Display<int, string>(101, "Rahim");
        Display<string, double>("Price", 250.50);
        Display<bool, int>(true, 100);
    }
}
```

### Output

```
First: 101
Second: Rahim
First: Price
Second: 250.5
First: True
Second: 100
```

---

## 6. Generic Method Return Type

A generic method can **return `T`**. The result then has **exactly the type that was chosen**, with **no casting**.

### Code

```csharp
using System;

class Program
{
    static T GetValue<T>(T value)
    {
        return value;
    }

    static void Main()
    {
        int number = GetValue<int>(100);
        string name = GetValue<string>("Rahim");
        double price = GetValue<double>(250.50);

        Console.WriteLine(number);
        Console.WriteLine(name);
        Console.WriteLine(price);
    }
}
```

### Output

```
100
Rahim
250.5
```

---

## 7. Type Inference

**Type inference** means the **compiler works out `T` from the arguments**, so the type argument does **not need to be written**.

### Code

```csharp
using System;

class Program
{
    static void Display<T>(T value)
    {
        Console.WriteLine(value);
    }

    static void Main()
    {
        Display(100);
        Display("Rahim");
        Display(250.50);
        Display(true);
    }
}
```

### Output

```
100
Rahim
250.5
True
```

---

## 8. Explicit Type Argument Uses

**Explicit type arguments** mean **writing the type yourself** inside `< >` when calling the method.

### Code

```csharp
using System;

class Program
{
    static void Display<T>(T value)
    {
        Console.WriteLine(value);
    }

    static void Main()
    {
        Display<int>(100);
        Display<string>("Rahim");
        Display<double>(250.50);
        Display<bool>(true);
    }
}
```

### Output

```
100
Rahim
250.5
True
```

### Inference vs explicit

| | Type inference | Explicit type argument |
|---|----------------|------------------------|
| Call | `Display(100)` | `Display<int>(100)` |
| Who decides `T` | The **compiler** | **You** |
| Code | Shorter | Longer and clearer |
| Works when `T` is only in the return type | **No** | **Yes** |
| Can force a different type | No | **Yes** |
| Result when both are possible | Same | Same |

---

## 9. All Topics Together

### Code

```csharp
using System;

class Program
{
    // 1. Generic Method Making
    // 2. T Uses in Generic Method
    public static void Display<T>(T value)
    {
        T data = value;
        Console.WriteLine($"Value: {data}");
    }

    // 3. Generic Method Parameter Uses
    // 4. Multiple Type Parameters (T, U)
    public static void ShowData<T, U>(T first, U second)
    {
        Console.WriteLine($"First: {first}");
        Console.WriteLine($"Second: {second}");
    }

    // 5. Generic Method Return Type
    public static T GetValue<T>(T value)
    {
        return value;
    }

    static void Main()
    {
        // 6. Type Inference
        Display(100);
        Display("Rahim");

        // 7. Explicit Type Argument Uses
        Display<double>(250.50);

        ShowData<int, string>(101, "Karim");

        int number = GetValue<int>(500);
        string name = GetValue("Hasan");

        Console.WriteLine($"Number: {number}");
        Console.WriteLine($"Name: {name}");
    }
}
```

### Output

```
Value: 100
Value: Rahim
Value: 250.5
First: 101
Second: Karim
Number: 500
Name: Hasan
```

### Where each topic appears

| # | Topic | Where in the code |
|---|-------|-------------------|
| 1 | **Generic method making** | `Display<T>`, `ShowData<T, U>`, `GetValue<T>` |
| 2 | **`T` uses** | `T data = value;` inside `Display<T>` |
| 3 | **Parameter uses** | `T value`, `T first`, `U second` |
| 4 | **Multiple type parameters** | `ShowData<T, U>(T first, U second)` |
| 5 | **Return type** | `static T GetValue<T>(T value)` |
| 6 | **Type inference** | `Display(100)`, `Display("Rahim")`, `GetValue("Hasan")` |
| 7 | **Explicit type argument** | `Display<double>(250.50)`, `ShowData<int, string>(...)`, `GetValue<int>(500)` |

### Line by line (the calls in `Main`)

| Call | Type | How `T` was chosen | Output |
|------|------|--------------------|--------|
| `Display(100)` | `int` | Inferred | `Value: 100` |
| `Display("Rahim")` | `string` | Inferred | `Value: Rahim` |
| `Display<double>(250.50)` | `double` | Explicit | `Value: 250.5` |
| `ShowData<int, string>(101, "Karim")` | `T = int`, `U = string` | Explicit | `First: 101` and `Second: Karim` |
| `GetValue<int>(500)` | `int` | Explicit | `Number: 500` |
| `GetValue("Hasan")` | `string` | Inferred | `Name: Hasan` |

The code mixes **inferred** and **explicit** type arguments, and the result is the same either way.

---

## 10. Quick Reference

| Topic | Syntax | Example |
|-------|--------|---------|
| Generic method | `static void Name<T>(T value)` | `static void Display<T>(T value)` |
| `T` as a local variable | `T data = value;` | |
| Several parameters, one type | `Name<T>(T a, T b)` | `Display<int>(10, 20)` |
| Multiple type parameters | `Name<T, U>(T a, U b)` | `Display<int, string>(101, "Rahim")` |
| Generic return type | `static T Name<T>(T value)` | `int n = GetValue<int>(100);` |
| Type inference | Call without `< >` | `Display(100)` |
| Explicit type argument | Call with `< >` | `Display<int>(100)` |
| Default value of `T` | `default(T)` | `0` for `int`, `null` for `string` |
| The real type of `T` | `typeof(T)` | `typeof(T).Name` |

---

## Summary

- **Generics** let one piece of code work with **many types**, with **type safety**, **no casting**, and **less repeated code**.
- A **generic method** has a **type parameter** in angle brackets after its name (`Display<T>`). `T` is a placeholder that becomes a real type when the method is called.
- **`T`** can be used as a **parameter type**, a **local variable type**, and a **return type**.
- The **same `T`** for several parameters means they must all have the **same type**. Use **`<T, U>`** when the types can **differ**.
- A generic method can **return `T`**, so the result has the **correct type without a cast** (`int n = GetValue<int>(100);`).
- **Type inference:** the compiler works out `T` from the arguments (`Display(100)`), so the type does not have to be written. It **fails** when `T` appears only in the return type, or when the arguments conflict.
- **Explicit type arguments** (`Display<int>(100)`) are needed when inference cannot work, or to **force a type** (`GetValue<double>(5)`).