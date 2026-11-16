# C# Fundamentals and OOP: Basic Debugging and Static Members

**Date:** 13 November 2026

Today's topics:

1. **C# Fundamentals:** basic debugging, with **breakpoints** and **step over / step into / step out**
2. **C# OOP:** **static members** and **static classes**

## Table of Contents
- [Part 1: Basic Debugging](#part-1-basic-debugging)
  - [1. What is Debugging?](#1-what-is-debugging)
  - [2. Starting and Stopping the Debugger](#2-starting-and-stopping-the-debugger)
  - [3. Breakpoints](#3-breakpoints)
  - [4. Stepping: Over, Into, and Out](#4-stepping-over-into-and-out)
  - [5. Inspecting Values](#5-inspecting-values)
  - [6. Debugging Example: Finding a Bug](#6-debugging-example-finding-a-bug)
  - [7. Debugging Shortcuts](#7-debugging-shortcuts)
- [Part 2: Static Members and Static Classes](#part-2-static-members-and-static-classes)
  - [1. What is static?](#1-what-is-static)
  - [2. Static Fields and Properties](#2-static-fields-and-properties)
  - [3. Static Methods](#3-static-methods)
  - [4. Static Constructor](#4-static-constructor)
  - [5. Static Classes](#5-static-classes)
  - [6. Static vs Instance](#6-static-vs-instance)
  - [7. Putting It Together](#7-putting-it-together)
  - [8. When to Use Static](#8-when-to-use-static)
- [Summary](#summary)

---

# Part 1: Basic Debugging

## 1. What is Debugging?

**Debugging** is the process of **finding and fixing errors (bugs)** in a program. The **debugger** lets you **pause** the program while it runs, run it **one line at a time**, and **look at the values** of variables.

| Type of bug | When it appears | Example |
|-------------|-----------------|---------|
| **Syntax / compile error** | Before running (the compiler reports it) | A missing `;` |
| **Runtime error** | While running (an exception) | `IndexOutOfRangeException`, `DivideByZeroException` |
| **Logic error** | The program runs but gives a **wrong result** | Integer division giving `81` instead of `81.67` |

The debugger is most useful for **runtime** and **logic** errors.

This document uses **Visual Studio**. The same ideas and most shortcuts also work in Visual Studio Code.

---

## 2. Starting and Stopping the Debugger

| Action | Shortcut | Menu |
|--------|----------|------|
| **Start Debugging** (runs with the debugger, stops at breakpoints) | `F5` | Debug > Start Debugging |
| **Start Without Debugging** (runs normally, breakpoints are ignored) | `Ctrl + F5` | Debug > Start Without Debugging |
| **Stop Debugging** | `Shift + F5` | Debug > Stop Debugging |
| **Restart** | `Ctrl + Shift + F5` | Debug > Restart |

While the program is **paused**, a **yellow arrow** points to the line that is **about to run**. That line has **not run yet**.

---

## 3. Breakpoints

A **breakpoint** tells the debugger to **pause the program** at a specific line.

### Set and remove a breakpoint

| Way | How |
|-----|-----|
| Click the **grey margin** to the left of the line number | A **red dot** appears. Click again to remove it |
| Place the cursor on the line and press **`F9`** | Toggles the breakpoint |

```csharp
static void Main()
{
    int x = 5;
    int y = 3;
    int sum = x + y;         // <- breakpoint here (red dot)
    Console.WriteLine(sum);
}
```

Press **`F5`**. The program runs and **pauses before** the line with the red dot.

### Managing breakpoints

| Task | How |
|------|-----|
| See all breakpoints | Debug > Windows > **Breakpoints** (`Ctrl + Alt + B`) |
| **Disable** a breakpoint (keep it, but skip it) | Right-click the red dot > Disable (the dot becomes hollow) |
| **Delete all** breakpoints | Debug > Delete All Breakpoints (`Ctrl + Shift + F9`) |

### Conditional breakpoint

Pause **only when a condition is true**. This is useful inside loops.

1. Right-click the red dot and choose **Conditions...**
2. Enter a condition, for example `i == 50`.

```csharp
for (int i = 0; i < 100; i++)
{
    Process(i);          // breakpoint with the condition  i == 50  pauses only when i is 50
}
```

### Other breakpoint options

| Option | Meaning |
|--------|---------|
| **Hit Count** | Pause only on the 5th time the line runs, for example |
| **Action (tracepoint)** | Write a message to the Output window **without pausing** |

---

## 4. Stepping: Over, Into, and Out

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

## 5. Inspecting Values

While paused, you can look at the **state of the program**.

| Tool | How to open | What it shows |
|------|-------------|---------------|
| **DataTip** | **Hover** the mouse over a variable | Its current value |
| **Locals** | Debug > Windows > Locals | All variables of the **current method** |
| **Autos** | Debug > Windows > Autos | Variables used on the **current and previous lines** |
| **Watch** | Debug > Windows > Watch | Variables or **expressions you choose** (type `total / marks.Length`) |
| **Call Stack** | Debug > Windows > Call Stack (`Ctrl + Alt + C`) | The chain of method calls that led to the current line |
| **Immediate Window** | Debug > Windows > Immediate (`Ctrl + Alt + I`) | Type an expression, for example `x + y`, and see the result |

You can also **change a value** while paused (edit it in Locals or Watch) to test what would happen.

**Reading the Call Stack**
```
Add(int a, int b)        <- currently here
Main()                   <- called from here
```

Red text in Locals or Watch means the **value just changed** on the last step.

---

## 6. Debugging Example: Finding a Bug

**Goal:** calculate the average of the marks `80, 75, 90`. The expected average is `81.67`.

```csharp
using System;

class Program
{
    static void Main()
    {
        int[] marks = { 80, 75, 90 };
        int total = 0;

        for (int i = 0; i < marks.Length; i++)
        {
            total += marks[i];
        }

        double average = total / marks.Length;       // set a breakpoint on this line
        Console.WriteLine($"Average: {average}");
    }
}
```

**Output:** `Average: 81`. The result is wrong, because the expected value is `81.67`.

### Steps to find the bug

1. **Set a breakpoint** on the line `total += marks[i];` and press `F5`.
2. Press `F5` (Continue) three times, and watch `i`, `marks[i]`, and `total` in **Locals**.

| Pass | `i` | `marks[i]` | `total` after the line |
|------|-----|------------|------------------------|
| 1 | 0 | 80 | 80 |
| 2 | 1 | 75 | 155 |
| 3 | 2 | 90 | 245 |

The loop is correct: `total = 245`.

3. Move the breakpoint to `double average = total / marks.Length;` and press `F5`.
4. Add `total / marks.Length` to the **Watch** window. It shows **`81`**, not `81.666...`.
5. **Cause:** `total` and `marks.Length` are both **`int`**, so the division is an **integer division**. The decimal part is cut off **before** the result is stored in the `double`.

### Fix

```csharp
double average = (double)total / marks.Length;       // 81.666666666666671
Console.WriteLine($"Average: {average:F2}");          // Average: 81.67
```

**Debugging method used:** breakpoint, check the values, compare with the expected result, find the exact line where the value goes wrong, fix, and run again.

---

## 7. Debugging Shortcuts

| Action | Shortcut |
|--------|----------|
| Start Debugging / Continue | `F5` |
| Start Without Debugging | `Ctrl + F5` |
| Stop Debugging | `Shift + F5` |
| Restart | `Ctrl + Shift + F5` |
| Toggle Breakpoint | `F9` |
| Step Over | `F10` |
| Step Into | `F11` |
| Step Out | `Shift + F11` |
| Run to Cursor | `Ctrl + F10` |
| Breakpoints window | `Ctrl + Alt + B` |
| Call Stack window | `Ctrl + Alt + C` |
| Immediate window | `Ctrl + Alt + I` |
| Delete All Breakpoints | `Ctrl + Shift + F9` |

---

# Part 2: Static Members and Static Classes

## 1. What is static?

A **`static`** member belongs to the **class itself**, **not to any object**. There is **only one copy**, shared by everything, and it is used through the **class name**.

| | Instance member | Static member |
|---|-----------------|---------------|
| Belongs to | **Each object** | **The class** |
| Copies | One per object | **One** for the whole class |
| Used with | `objectName.Member` | `ClassName.Member` |
| Needs an object? | Yes | **No** |

You already use static members: `Console.WriteLine()`, `Math.Sqrt()`, `int.Parse()`.

---

## 2. Static Fields and Properties

A **static field** holds **one value shared by all objects** of the class.

```csharp
class Student
{
    public static int Count { get; private set; }     // static property: shared by all students

    public int Id { get; }                            // instance property: one per student
    public string Name { get; }

    public Student(string name)
    {
        Count++;                // increases the single shared counter
        Id = Count;
        Name = name;
    }
}
```

```csharp
Student s1 = new Student("Rahim");
Student s2 = new Student("Karim");
Student s3 = new Student("Nusrat");

Console.WriteLine(Student.Count);      // 3  (through the class name)
Console.WriteLine(s1.Id);              // 1
Console.WriteLine(s2.Id);              // 2
Console.WriteLine(s3.Id);              // 3
```

```
Student.Count  ->  3         (one value, shared)

s1.Id = 1      s2.Id = 2      s3.Id = 3      (each object has its own)
```

- `Count` is shared by every `Student`.
- `Id` is separate for each `Student`.
- A static member is reached with the **class name** (`Student.Count`), **not** with an object (`s1.Count` is an error).

### Static readonly and const

```csharp
public const double Pi = 3.14159;                           // constant, implicitly static
public static readonly string AppName = "MyApp";            // static, assigned once
```

---

## 3. Static Methods

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

```csharp
class Demo
{
    int x = 5;                  // instance field
    static int y = 10;          // static field

    static void Show()
    {
        // Console.WriteLine(x);     // Error: a static method cannot use an instance field
        Console.WriteLine(y);        // 10
    }

    void Display()                   // an instance method can use both
    {
        Console.WriteLine(x);        // 5
        Console.WriteLine(y);        // 10
    }
}
```

> `Main` is `static`, because the program starts **before any object exists**. That is why `Main` can call other **static** methods of its class directly, but needs an **object** to call instance methods.

---

## 4. Static Constructor

A **static constructor** **initializes the static members** of a class.

| Rule | Detail |
|------|--------|
| Keyword | `static`, with **no access modifier** and **no parameters** |
| Runs | **Once**, automatically, **before** the class is first used |
| Called by you? | **No** |
| Count | Only **one** per class |

```csharp
class Config
{
    public static readonly string AppName;

    static Config()                                         // static constructor
    {
        Console.WriteLine("Static constructor runs once.");
        AppName = "MyApp";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Program started");
        Console.WriteLine(Config.AppName);
        Console.WriteLine(Config.AppName);
    }
}
```

**Output**
```
Program started
Static constructor runs once.
MyApp
MyApp
```

The static constructor ran **once**, the first time `Config` was used, and **not** again for the second use.

| | Static constructor | Instance constructor |
|---|--------------------|----------------------|
| Runs | **Once** per class | **Every time** an object is created |
| Parameters | None | Allowed |
| Access modifier | None | Allowed |
| Initializes | Static members | Instance members |

---

## 5. Static Classes

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

### Where static classes are used

- **Utility / helper methods:** `Math`, `Console`, `Convert`, `File`
- **Constants and settings** shared by the program
- **Extension methods** (in a static class)

Helper example:
```csharp
static class Validator
{
    public static bool IsValidAmount(decimal amount)
    {
        return amount > 0;
    }

    public static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}

Console.WriteLine(Validator.IsValidAmount(500));      // True
Console.WriteLine(Validator.IsValidName(""));         // False
```

### Static class vs class with static members

| | Static class | Normal class with static members |
|---|--------------|----------------------------------|
| Can create objects | **No** | **Yes** |
| Instance members | **None** | **Allowed** |
| Use when | Only helper functions or shared data | Objects are needed, plus some shared data (such as a counter) |

---

## 6. Static vs Instance

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

## 7. Putting It Together

```csharp
using System;

class BankAccount
{
    // Static members: shared by all accounts
    public static int TotalAccounts { get; private set; }
    public static decimal InterestRate { get; private set; } = 5m;

    // Instance members: one set per account
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal balance)
    {
        Owner = owner;
        Balance = balance;
        TotalAccounts++;
    }

    // Static method: changes the shared interest rate
    public static void ChangeInterestRate(decimal rate)
    {
        InterestRate = rate;
    }

    // Instance method: uses the object's balance and the shared rate
    public decimal CalculateInterest()
    {
        return Balance * InterestRate / 100;
    }
}

// Static class: helper methods
static class Validator
{
    public static bool IsValidAmount(decimal amount)
    {
        return amount > 0;
    }
}

class Program
{
    static void Main()
    {
        BankAccount a1 = new BankAccount("Rahim", 10000m);
        BankAccount a2 = new BankAccount("Karim", 20000m);

        Console.WriteLine($"Total accounts: {BankAccount.TotalAccounts}");
        Console.WriteLine($"Rahim interest: {a1.CalculateInterest()}");
        Console.WriteLine($"Karim interest: {a2.CalculateInterest()}");

        BankAccount.ChangeInterestRate(6m);
        Console.WriteLine("After changing the rate to 6:");
        Console.WriteLine($"Rahim interest: {a1.CalculateInterest()}");
        Console.WriteLine($"Karim interest: {a2.CalculateInterest()}");

        Console.WriteLine($"Is -5 valid? {Validator.IsValidAmount(-5)}");
    }
}
```

**Output**
```
Total accounts: 2
Rahim interest: 500
Karim interest: 1000
After changing the rate to 6:
Rahim interest: 600
Karim interest: 1200
Is -5 valid? False
```

| Concept | Where |
|---------|-------|
| **Static field / property** | `TotalAccounts` (a counter) and `InterestRate` (one rate for all accounts) |
| **Static method** | `ChangeInterestRate`, called as `BankAccount.ChangeInterestRate(6m)` |
| **Instance members** | `Owner`, `Balance`, `CalculateInterest()` |
| **Static class** | `Validator` |
| **Shared state** | Changing the rate once changes the interest of **both** accounts |

---

## 8. When to Use Static

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

---

# Summary

**Debugging**
- **Debugging** finds and fixes bugs. The debugger pauses the program and lets you check values line by line.
- A **breakpoint** (`F9` or a click in the margin) pauses the program at a line. **Conditional breakpoints** pause only when a condition is true.
- **Step Over** (`F10`) runs the current line, including its method calls. **Step Into** (`F11`) goes inside a called method. **Step Out** (`Shift + F11`) finishes the current method and returns to the caller. **Continue** (`F5`) runs to the next breakpoint.
- Inspect values with **DataTips**, **Locals**, **Autos**, **Watch**, the **Call Stack**, and the **Immediate** window.
- Debug by checking values, comparing with the expected result, and finding the exact line where the value goes wrong.

**Static members and static classes**
- A **static** member belongs to the **class**, with **one shared copy**, and is used with the class name (`Student.Count`).
- **Static methods** cannot use instance members or `this`.
- A **static constructor** runs **once**, before the class is first used, to initialize static members.
- A **static class** has only static members, cannot be instantiated or inherited, and is used for helper methods and constants (`Math`, `Console`).
- Use static for **utilities, shared counters, and constants**. Avoid it for data that belongs to a single object.