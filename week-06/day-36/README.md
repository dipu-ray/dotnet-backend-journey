# C# Fundamentals and OOP: Basic Debugging and Static Members

**Date:** 16 November 2026

Today's topics:

1. **C# Fundamentals:** basic debugging, with **breakpoints** and **step over / step into / step out**
2. **C# OOP:** **static members** and **static classes**

## Table of Contents
- [Part 1: Basic Debugging](#part-1-basic-debugging)
  - [1. Stepping: Over, Into, and Out](#1-stepping-over-into-and-out)
- [Part 2: Static Members and Static Classes](#part-2-static-members-and-static-classes)
  - [1. Static Methods](#1-static-methods)
  - [2. Static Classes](#2-static-classes)
  - [3. Static vs Instance](#3-static-vs-instance)
  - [4. When to Use Static](#4-when-to-use-static)

---

# Part 1: Basic Debugging

## 1. Stepping: Over, Into, and Out

Once the program is paused, you control it **one step at a time**.

| Command | Shortcut | What it does |
|---------|----------|--------------|
| **Continue** | `F5` | Run until the **next breakpoint** (or the end) |
| **Step Over** | `F10` | Run the **current line completely**, **including any method it calls**, then stop at the **next line** |
| **Step Into** | `F11` | If the line calls a method, **go inside that method** and stop at its first line |
| **Step Out** | `Shift + F11` | Run the **rest of the current method** and return to the **caller** |
| **Run to Cursor** | `Ctrl + F10` | Run until the line where the cursor is |

### Example program

```csharp
using System;

class Program
{
    static int Add(int a, int b)
    {
        int result = a + b;          // line A
        return result;               // line B
    }

    static int Multiply(int a, int b)
    {
        int result = a * b;
        return result;
    }

    static void Main()
    {
        int x = 5;
        int y = 3;

        int sum = Add(x, y);                 // line 1  <- breakpoint here
        int product = Multiply(sum, 2);      // line 2

        Console.WriteLine($"Sum: {sum}");    // line 3
        Console.WriteLine($"Product: {product}");
    }
}
```

Set a breakpoint on `int sum = Add(x, y);` and press `F5`. The program pauses on that line.

### Step Over (`F10`)

```
Paused at:   int sum = Add(x, y);
Press F10 -> the whole Add method runs in one step
Now at:      int product = Multiply(sum, 2);      (sum is now 8)
```

Use **Step Over** when you **trust** the method and only want to move on.

### Step Into (`F11`)

```
Paused at:   int sum = Add(x, y);
Press F11 -> the debugger enters Add
Now at:      int result = a + b;                  (a = 5, b = 3)
Press F10 -> result = 8
Press F10 -> return result;
```

Use **Step Into** when you want to **look inside** a method to see what it does.

### Step Out (`Shift + F11`)

```
Inside Add, at:   int result = a + b;
Press Shift+F11 -> the rest of Add runs
Back in Main, at the line that called Add
```

Use **Step Out** when you stepped into a method and **do not need to see the rest of it**.

### Comparison

| | Step Over (`F10`) | Step Into (`F11`) | Step Out (`Shift + F11`) |
|---|-------------------|-------------------|--------------------------|
| Method call on the line | Runs it **fully** | **Enters** it | (Not applicable) |
| Moves to | The **next line** in the same method | The **first line** of the called method | The **caller**, after the current method finishes |
| Use when | You do not need the details | You want to investigate the method | You want to leave the current method |

### Good to know

- **Step Into** on a line **without** a method call behaves like Step Over.
- Visual Studio normally **skips code that is not yours** (for example `Console.WriteLine`) when you press `F11`. This is the **Just My Code** setting.
- If you pressed `F11` into a method by mistake, press `Shift + F11` to get back out.
- **Continue** (`F5`) is used to jump to the next breakpoint.

---

# Part 2: Static Members and Static Classes

## 1. Static Methods

A **static method** is called with the **class name**, with **no object** needed.

```csharp
class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public int Multiply(int a, int b)       // instance method
    {
        return a * b;
    }
}

int sum = Calculator.Add(5, 3);             // static: called on the class
Console.WriteLine(sum);                     // 8

Calculator calc = new Calculator();
int product = calc.Multiply(5, 3);          // instance: needs an object
Console.WriteLine(product);                 // 15
```

### Rules for static methods

| Rule | Detail |
|------|--------|
| Can use | **Static** members of the class, and its own parameters and local variables |
| Cannot use directly | **Instance** members (they belong to an object that may not exist) |
| Cannot use `this` | There is no current object |
| Called with | `ClassName.Method()` |

---

## 2. Static Classes

A **static class** contains **only static members**.

```csharp
static class MathHelper
{
    public const double Pi = 3.14159;

    public static int Square(int n)
    {
        return n * n;
    }

    public static int Max(int a, int b)
    {
        return a > b ? a : b;
    }
}

Console.WriteLine(MathHelper.Square(5));       // 25
Console.WriteLine(MathHelper.Max(3, 9));       // 9
Console.WriteLine(MathHelper.Pi);              // 3.14159

// MathHelper m = new MathHelper();             // Error: a static class cannot be instantiated
```

### Rules for static classes

| Rule | Detail |
|------|--------|
| Declared with | `static class Name` |
| Instantiation | **Not allowed** (no `new`) |
| Members | **All** must be static |
| Instance constructors | Not allowed (a static constructor is allowed) |
| Inheritance | Cannot be inherited, and cannot inherit from another class (only from `object`) |

---

## 3. Static vs Instance

| | Static | Instance |
|---|--------|----------|
| Belongs to | The **class** | An **object** |
| Created | Once, when the class is first used | Each time `new` is called |
| Access | `ClassName.Member` | `objectName.Member` |
| Shared between objects | **Yes** | **No** (each object has its own) |
| Can use `this` | **No** | **Yes** |
| Can use instance members | **No** (directly) | Yes, and static members too |
| Lifetime | The whole run of the program | While the object exists |

---

## 4. When to Use Static

| Use static for | Example |
|----------------|---------|
| **Utility methods** that do not need object data | `Math.Sqrt`, `Validator.IsValidAmount` |
| **Counters and shared values** for all objects of a class | `Student.Count`, `BankAccount.TotalAccounts` |
| **Constants** | `Pi`, `MaxUsers` |
| **Helpers and extension methods** | A static helper class |

**Be careful with static**
- **Shared static data can be changed from anywhere**, which makes bugs harder to find.
- Static state is **shared across the whole program**, so it needs care when several threads use it.
- Code that depends heavily on static members is **harder to test** and to change.
- If a method needs the **data of an object**, make it an **instance** method.
