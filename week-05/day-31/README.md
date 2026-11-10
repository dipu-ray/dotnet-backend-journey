# C#: Exception Handling and Arrays

**Date:** 10 November 2026

Today's topics:

1. **Exception Handling:** completed.
2. **Arrays:** about half completed. The completed part is documented below, and the remaining topics are listed in the [progress table](#arrays-progress).

## Table of Contents

- [Part 1: Exception Handling](#part-1-exception-handling)
  - [What is an Exception?](#what-is-an-exception)
  - [try and catch](#try-and-catch)
  - [Common Exception Types](#common-exception-types)
  - [Multiple catch Blocks](#multiple-catch-blocks)
  - [finally](#finally)
  - [throw](#throw)
  - [Exception Properties](#exception-properties)
- [Part 2: Arrays](#part-2-arrays)
  - [What is an Array?](#what-is-an-array)
  - [Declare and Initialize](#declare-and-initialize)
  - [Access and Change Elements](#access-and-change-elements)
  - [Array Length](#array-length)
  - [Loop Through an Array](#loop-through-an-array)
  - [Arrays Progress](#arrays-progress)
- [Summary](#summary)

---

# Part 1: Exception Handling

## What is an Exception?

An **exception** is an **error that happens while the program is running**. If it is not handled, the program **stops (crashes)** and shows an error message.

```csharp
int[] numbers = { 1, 2, 3 };
Console.WriteLine(numbers[10]);      // runtime error: the program crashes
```

| Keyword   | Purpose                                                          |
| --------- | ---------------------------------------------------------------- |
| `try`     | The code that **might** cause an error                           |
| `catch`   | The code that **handles** the error                              |
| `finally` | The code that **always runs**, whether or not there was an error |
| `throw`   | **Creates** (raises) an exception on purpose                     |

---

## try and catch

**Syntax**

```csharp
try
{
    // code that might cause an error
}
catch (Exception e)
{
    // code that handles the error
}
```

**Example**

```csharp
try
{
    int[] numbers = { 1, 2, 3 };
    Console.WriteLine(numbers[10]);
}
catch (Exception e)
{
    Console.WriteLine("Something went wrong: " + e.Message);
}

Console.WriteLine("The program continues.");
```

**Output**

```
Something went wrong: Index was outside the bounds of the array.
The program continues.
```

---

## Common Exception Types

| Exception                  | When it happens                                                                     | Example                            |
| -------------------------- | ----------------------------------------------------------------------------------- | ---------------------------------- |
| `Exception`                | **Base class** of all exceptions (catches everything)                               |                                    |
| `DivideByZeroException`    | Dividing an integer by zero                                                         | `10 / 0`                           |
| `IndexOutOfRangeException` | Using an array index that does not exist                                            | `numbers[10]` on a 3-item array    |
| `FormatException`          | A string is not in the right format for the conversion                              | `int.Parse("abc")`                 |
| `NullReferenceException`   | Using an object that is `null`                                                      | `string s = null; s.Length`        |
| `InvalidCastException`     | An invalid type conversion                                                          | Casting a `string` object to `int` |
| `OverflowException`        | A value is too large or small for the type (in a `checked` context or a conversion) | `Convert.ToByte(300)`              |
| `ArgumentException`        | A method received an invalid argument                                               | `throw new ArgumentException(...)` |
| `FileNotFoundException`    | The file does not exist                                                             | Opening a missing file             |

---

## Multiple catch Blocks

Use **several `catch` blocks** to handle **different errors differently**. The first block that matches runs.

```csharp
try
{
    Console.Write("Enter a number: ");
    int number = int.Parse(Console.ReadLine());

    Console.WriteLine(100 / number);
}
catch (FormatException)
{
    Console.WriteLine("Please enter a valid whole number.");
}
catch (DivideByZeroException)
{
    Console.WriteLine("The number cannot be zero.");
}
catch (Exception e)
{
    Console.WriteLine("Unexpected error: " + e.Message);
}
```

---

## finally

The `finally` block **always runs**, whether an error happened or not. It is used for **clean-up** (closing a file or a connection, for example).

```csharp
try
{
    Console.WriteLine("Trying...");
    int[] numbers = { 1 };
    Console.WriteLine(numbers[5]);
}
catch (IndexOutOfRangeException e)
{
    Console.WriteLine("Error: " + e.Message);
}
finally
{
    Console.WriteLine("The 'try catch' is finished.");
}
```

**Output**

```
Trying...
Error: Index was outside the bounds of the array.
The 'try catch' is finished.
```

---

## throw

`throw` **creates an exception on purpose**. It is used to stop the program when a rule is broken, with a **custom message**.

```csharp
static void CheckAge(int age)
{
    if (age < 18)
    {
        throw new ArgumentException("Access denied - you must be at least 18 years old.");
    }
    else
    {
        Console.WriteLine("Access granted - you are old enough!");
    }
}

try
{
    CheckAge(15);
}
catch (ArgumentException e)
{
    Console.WriteLine(e.Message);
}
```

**Output**

```
Access denied - you must be at least 18 years old.
```

---

## Exception Properties

| Property           | Meaning                                 | Example                        |
| ------------------ | --------------------------------------- | ------------------------------ |
| `e.Message`        | A description of the error              | `Attempted to divide by zero.` |
| `e.GetType().Name` | The name of the exception type          | `DivideByZeroException`        |
| `e.StackTrace`     | Where the error happened (method calls) | Used for debugging             |

```csharp
try
{
    int zero = 0;
    Console.WriteLine(5 / zero);
}
catch (Exception e)
{
    Console.WriteLine(e.GetType().Name);     // DivideByZeroException
    Console.WriteLine(e.Message);            // Attempted to divide by zero.
}
```

---

# Part 2: Arrays

## What is an Array?

An **array** stores **multiple values of the same type** in **one variable**, instead of creating a separate variable for each value.

```csharp
// Without an array
string car1 = "Volvo";
string car2 = "BMW";
string car3 = "Ford";

// With an array
string[] cars = { "Volvo", "BMW", "Ford" };
```

---

## Declare and Initialize

**Declare** with the type, square brackets `[]`, and a name.

```csharp
string[] cars;
```

**Initialize** (give it values):

```csharp
// 1. Values in curly braces (most common)
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
int[] myNum = { 10, 20, 30, 40 };

// 2. Create with new and set the values
string[] cars2 = new string[] { "Volvo", "BMW", "Ford", "Mazda" };

// 3. Create with a size and fill in later
string[] cars3 = new string[4];
cars3[0] = "Volvo";
cars3[1] = "BMW";
```

---

## Access and Change Elements

Use the **index number** in square brackets. The first index is `0`.

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

Console.WriteLine(cars[0]);       // Volvo
Console.WriteLine(cars[2]);       // Ford
```

**Change an element**

```csharp
cars[0] = "Opel";
Console.WriteLine(cars[0]);       // Opel
```

---

## Array Length

`Length` gives the **number of elements**.

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

Console.WriteLine(cars.Length);                    // 4
Console.WriteLine(cars[cars.Length - 1]);          // Mazda (the last element)
```

---

## Loop Through an Array

### `for` loop

Use `Length` as the limit.

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

for (int i = 0; i < cars.Length; i++)
{
    Console.WriteLine(cars[i]);
}
```

Use `i < cars.Length`, not `i <= cars.Length`. The last index is `Length - 1`, so `<=` goes past the end and throws `IndexOutOfRangeException`.

### `foreach` loop

Goes through **each element** directly, with no index.

```csharp
foreach (string car in cars)
{
    Console.WriteLine(car);
}
```

**Syntax:** `foreach (type variableName in arrayName)`

|                         | `for`                                | `foreach`                           |
| ----------------------- | ------------------------------------ | ----------------------------------- |
| Uses an index           | Yes                                  | No                                  |
| Can change the elements | **Yes** (`arr[i] = ...`)             | No (the loop variable is read-only) |
| Best for                | Needing the index or changing values | Just reading every element          |

### Example: sum and average

```csharp
int[] marks = { 80, 65, 90, 75 };
int total = 0;

foreach (int mark in marks)
{
    total += mark;
}

double average = (double)total / marks.Length;

Console.WriteLine("Total: " + total);        // Total: 310
Console.WriteLine("Average: " + average);    // Average: 77.5
```

---

## Arrays Progress

| Arrays topic                                                   | Status            |
| -------------------------------------------------------------- | ----------------- |
| Declare and initialize                                         | Done              |
| Access and change elements                                     | Done              |
| Array length                                                   | Done              |
| Loop through an array (`for`, `foreach`)                       | Done              |
| Sort an array (`Array.Sort`, `Array.Reverse`, `Array.IndexOf`) | Not completed yet |
| `System.Linq` (`Min`, `Max`, `Sum`, `Average`)                 | Not completed yet |
| Multidimensional arrays                                        | Not completed yet |
| Remaining array topics                                         | Not completed yet |

> Update this table when the remaining topics are finished.

---

# Summary

**Exception Handling**

- An **exception** is a run-time error. Without handling, the program crashes.
- **`try`** holds the risky code, **`catch`** handles the error, **`finally`** always runs, and **`throw`** raises an exception on purpose.
- Use **several `catch` blocks**, with **specific** exceptions first and `Exception` last.
- Common exceptions: `DivideByZeroException`, `IndexOutOfRangeException`, `FormatException`, `NullReferenceException`, `InvalidCastException`, `ArgumentException`.
- `e.Message` describes the error, and `e.GetType().Name` gives its type.

**Arrays**

- An array stores **many values of the same type** in one variable, with a **fixed size** and a **zero-based index**.
- Declare with `type[] name = { ... }` or `new type[size]`.
- Access and change elements with `arr[index]`. A wrong index throws `IndexOutOfRangeException`.
- `Length` gives the number of elements, and the last index is `Length - 1`.
- Loop with `for` (needs the index, can change values) or `foreach` (reads every element).
- **Remaining:** `Array.Sort`, `Array.Reverse`, and `Array.IndexOf` sort, reverse, and search. `System.Linq` adds `Min`, `Max`, `Sum`, and `Average`. Multidimensional arrays and the rest of the arrays topics.
