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

**Without generics**, the same logic has to be written again for each type:

```csharp
static void Display(int value)      { Console.WriteLine(value); }
static void Display(string value)   { Console.WriteLine(value); }
static void Display(double value)   { Console.WriteLine(value); }
static void Display(bool value)     { Console.WriteLine(value); }
```

Or a method with an `object` parameter is used, which **loses the type** and **boxes** value types.

**With generics**, one method handles all of them:

| Benefit | Explanation |
|---------|-------------|
| **Code reuse** | One method for every type, with no copy and paste |
| **Type safety** | The compiler checks the types. Wrong use is a **compile error**, not a run-time error |
| **No casting** | The result already has the correct type |
| **Performance** | No boxing and unboxing for value types (unlike `object`) |

### Naming of type parameters

| Name | Meaning |
|------|---------|
| `T` | The **T**ype (the usual name for one parameter) |
| `U`, `V` | The second and third type parameters |
| `TKey`, `TValue` | Descriptive names (for example in `Dictionary<TKey, TValue>`) |

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

### How it works

| Call | `T` becomes | Output |
|------|-------------|--------|
| `Display<int>(100)` | `int` | `100` |
| `Display<string>("Rahim")` | `string` | `Rahim` |
| `Display<double>(250.50)` | `double` | `250.5` |
| `Display<bool>(true)` | `bool` | `True` |

- The method is **written once**, and it works for four different types.
- The type is written inside `< >` when calling: `Display<int>(...)`.
- `250.50` prints as `250.5` because a `double` does not keep the trailing zero. `true` prints as `True`.

### Steps to make a generic method

1. Write the method with a **type parameter** after the name: `Display<T>`.
2. Use **`T`** wherever a type is needed: in the parameters, the return type, or inside the body.
3. **Call** the method, giving the real type: `Display<int>(100)`, or let the compiler find it (see [Type Inference](#7-type-inference)).

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

### How `T` is used

| Place | Code | Meaning |
|-------|------|---------|
| **Parameter type** | `T value` | The parameter has the type that is chosen for `T` |
| **Local variable type** | `T data = value;` | A variable of the same type `T` |

When the method is called as `Display<int>(100)`, every `T` inside the method becomes `int`. For `Display<string>("Rahim")`, every `T` becomes `string`.

### Seeing the real type

`typeof(T)` gives the type that `T` was replaced with:

```csharp
static void ShowType<T>(T value)
{
    Console.WriteLine($"{value} is of type {typeof(T).Name}");
}

ShowType<int>(100);          // 100 is of type Int32
ShowType<string>("Rahim");   // Rahim is of type String
ShowType<double>(250.50);    // 250.5 is of type Double
```

### What can be done with `T`

Inside the method, the compiler **does not know what `T` is**. It can be used as a **value that is stored, passed, returned, and printed**, and it has the members that **every object** has (`ToString()`, `Equals()`, `GetType()`). Operations such as `+` or `>` are **not allowed** on a plain `T`.

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

### How it works

| Call | `T` | `first` | `second` |
|------|-----|---------|----------|
| `Display<int>(10, 20)` | `int` | `10` | `20` |
| `Display<string>("Rahim", "Karim")` | `string` | `"Rahim"` | `"Karim"` |
| `Display<double>(10.5, 20.5)` | `double` | `10.5` | `20.5` |

### Both parameters share one type

```csharp
Display<int>(10, "Karim");     // Error: "Karim" is not an int
Display(10, "Karim");          // Error: the compiler cannot decide whether T is int or string
```

If the two values need **different types**, use **two type parameters** (next section).

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

### How it works

| Call | `T` | `U` | `first` | `second` |
|------|-----|-----|---------|----------|
| `Display<int, string>(101, "Rahim")` | `int` | `string` | `101` | `"Rahim"` |
| `Display<string, double>("Price", 250.50)` | `string` | `double` | `"Price"` | `250.5` |
| `Display<bool, int>(true, 100)` | `bool` | `int` | `true` | `100` |

- The type arguments are given **in the same order** as the type parameters: the first type goes to `T`, the second to `U`.
- `T` and `U` are **independent**. They can be the same type or different types.

| | One type parameter (`<T>`) | Two type parameters (`<T, U>`) |
|---|----------------------------|--------------------------------|
| Parameters | Must all be the **same** type | Can be **different** types |
| Example | `Display<int>(10, 20)` | `Display<int, string>(101, "Rahim")` |

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

### How it works

| Call | `T` | Returns | Stored in |
|------|-----|---------|-----------|
| `GetValue<int>(100)` | `int` | an `int` | `int number` |
| `GetValue<string>("Rahim")` | `string` | a `string` | `string name` |
| `GetValue<double>(250.50)` | `double` | a `double` | `double price` |

- The **return type** is written as `T` (`static T GetValue<T>`), in the place where `void`, `int`, or `string` would normally be.
- The returned value is **already the right type**, so `int number = GetValue<int>(100);` needs **no cast**. With an `object` return type, a cast such as `(int)` would be needed.

| Return type | Meaning |
|-------------|---------|
| `void` | Returns nothing (for example `Display<T>`) |
| `T` | Returns a value of the **same type as `T`** (for example `GetValue<T>`) |

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

### How it works

| Call | The compiler sees | `T` is inferred as |
|------|-------------------|--------------------|
| `Display(100)` | An integer argument | `int` |
| `Display("Rahim")` | A text argument | `string` |
| `Display(250.50)` | A decimal argument | `double` |
| `Display(true)` | A `true` or `false` argument | `bool` |

`Display(100)` means exactly the same as `Display<int>(100)`. The output is the same as the output of the first example, with shorter code.

### When inference works and when it does not

| Situation | Inference |
|-----------|-----------|
| `T` appears in a **parameter** (`Display<T>(T value)`) | **Works**, `T` comes from the argument |
| Two parameters of the **same `T`** with **different types** (`Display(10, "Karim")`) | **Fails** (a compile error) |
| `T` is **only** in the **return type** or the method has **no parameters** | **Fails**, the type must be given |

```csharp
static T CreateDefault<T>()
{
    return default(T);
}

// int a = CreateDefault();          // Error: T cannot be inferred
int a = CreateDefault<int>();        // OK: the type is given explicitly, a = 0
string b = CreateDefault<string>();  // OK: b = null
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

### When to write the type explicitly

| Reason | Example |
|--------|---------|
| Inference **cannot** work (no parameter holds `T`) | `CreateDefault<int>()` |
| You want to **force a specific type** | `GetValue<double>(5)` |
| It makes the code **clearer** to read | `Display<int>(100)` |

**Forcing a type**

```csharp
Console.WriteLine(GetValue(5) / 2);            // 2    (T is inferred as int, so integer division)
Console.WriteLine(GetValue<double>(5) / 2);    // 2.5  (T is double, so decimal division)
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