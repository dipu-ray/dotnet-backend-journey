# C#: Exception Handling and Arrays

**Date:** 8 November 2026

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
  - [Sort an Array](#sort-an-array)
  - [System.Linq: Min, Max, Sum, Average](#systemlinq-min-max-sum-average)
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

**Exception handling** means **catching** the error so the program can **continue** or **show a clear message** instead of crashing.

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

**How it works**

```
try block runs
   |
   |-- no error   -> catch is skipped
   |
   |-- error      -> the rest of try is skipped -> catch runs -> the program continues after the catch
```

- As soon as an error happens in `try`, the **remaining lines of `try` are skipped**.
- `e` is the **exception object**. `e.Message` describes the error.

**Another example: divide by zero**

```csharp
try
{
    int x = 10;
    int y = 0;
    Console.WriteLine(x / y);
}
catch (DivideByZeroException e)
{
    Console.WriteLine("Error: " + e.Message);     // Error: Attempted to divide by zero.
}
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

```csharp
try
{
    int number = int.Parse("abc");
}
catch (FormatException e)
{
    Console.WriteLine("Invalid number format.");
}

try
{
    string text = null;
    Console.WriteLine(text.Length);
}
catch (NullReferenceException e)
{
    Console.WriteLine("The string is null.");
}
```

> The exact `Message` text depends on the .NET version.

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

| Input | Output                               |
| ----- | ------------------------------------ |
| `5`   | `20`                                 |
| `abc` | `Please enter a valid whole number.` |
| `0`   | `The number cannot be zero.`         |

**Rule:** put the **specific** exceptions first and the general `Exception` **last**. If `catch (Exception)` comes first, the compiler gives an error, because it would catch everything before the specific blocks.

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

**Order of execution**

| Case     | try                  | catch   | finally  |
| -------- | -------------------- | ------- | -------- |
| No error | Runs fully           | Skipped | **Runs** |
| Error    | Runs until the error | Runs    | **Runs** |

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

- If `CheckAge(15)` were called **without** `try / catch`, the program would crash with that message.
- Inside a `catch` block, write **`throw;`** to **pass the same exception on** to the caller (it keeps the original details).

```csharp
catch (Exception e)
{
    Console.WriteLine("Logging the error...");
    throw;
}
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

| Property         | Detail                                                          |
| ---------------- | --------------------------------------------------------------- |
| Same type        | All elements have the **same data type**                        |
| Fixed size       | The size is set when the array is created and **cannot change** |
| Zero-based index | The first element has index **`0`**                             |
| Reference type   | The array variable holds a reference to the array               |

```
Index:     0        1       2       3
         ┌───────┬───────┬───────┬───────┐
cars  =  │ Volvo │  BMW  │ Ford  │ Mazda │
         └───────┴───────┴───────┴───────┘
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

**Default values** of a new array (`new type[size]`):

| Type                               | Default element value |
| ---------------------------------- | --------------------- |
| `int`, `long`, `double`, `decimal` | `0`                   |
| `bool`                             | `false`               |
| `char`                             | `'\0'`                |
| `string` and other reference types | `null`                |

```csharp
int[] scores = new int[3];
Console.WriteLine(scores[0]);     // 0
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

**Using an index that does not exist** causes an exception:

```csharp
Console.WriteLine(cars[4]);       // IndexOutOfRangeException (valid indexes are 0 to 3)
```

```csharp
try
{
    Console.WriteLine(cars[4]);
}
catch (IndexOutOfRangeException e)
{
    Console.WriteLine("Invalid index: " + e.Message);
}
```

---

## Array Length

`Length` gives the **number of elements**.

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

Console.WriteLine(cars.Length);                    // 4
Console.WriteLine(cars[cars.Length - 1]);          // Mazda (the last element)
```

| Array length | First index | Last index |
| ------------ | ----------- | ---------- |
| `n`          | `0`         | `n - 1`    |

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

## Sort an Array

The `Array` class has ready-made methods.

### `Array.Sort`

Sorts in **ascending order** (A to Z for text, small to large for numbers).

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
Array.Sort(cars);

foreach (string car in cars)
{
    Console.WriteLine(car);
}
```

**Output**

```
BMW
Ford
Mazda
Volvo
```

```csharp
int[] numbers = { 5, 1, 8, 9, 3 };
Array.Sort(numbers);

foreach (int n in numbers)
{
    Console.Write(n + " ");      // 1 3 5 8 9
}
```

### `Array.Reverse`

Reverses the order of the elements. Sort first and then reverse to get **descending** order.

```csharp
int[] numbers = { 5, 1, 8, 9, 3 };

Array.Sort(numbers);
Array.Reverse(numbers);

foreach (int n in numbers)
{
    Console.Write(n + " ");      // 9 8 5 3 1
}
```

### `Array.IndexOf`

Finds the **index** of a value. It returns **`-1`** when the value is not found.

```csharp
string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

Console.WriteLine(Array.IndexOf(cars, "Ford"));     // 2
Console.WriteLine(Array.IndexOf(cars, "Audi"));     // -1
```

> These methods change the original array. `Array.Sort` and `Array.Reverse` do not create a new one.

---

## System.Linq: Min, Max, Sum, Average

Add `using System.Linq;` at the top of the file. It gives extra methods for arrays.

```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] myNumbers = { 5, 1, 8, 9 };

        Console.WriteLine(myNumbers.Max());        // 9   (largest)
        Console.WriteLine(myNumbers.Min());        // 1   (smallest)
        Console.WriteLine(myNumbers.Sum());        // 23  (total)
        Console.WriteLine(myNumbers.Average());    // 5.75
    }
}
```

| Method      | Returns                  |
| ----------- | ------------------------ |
| `Max()`     | The largest value        |
| `Min()`     | The smallest value       |
| `Sum()`     | The total of all values  |
| `Average()` | The average (a `double`) |

---

## Arrays Progress

| Arrays topic                                                   | Status            |
| -------------------------------------------------------------- | ----------------- |
| Declare and initialize                                         | Done              |
| Access and change elements                                     | Done              |
| Array length                                                   | Done              |
| Loop through an array (`for`, `foreach`)                       | Done              |
| Sort an array (`Array.Sort`, `Array.Reverse`, `Array.IndexOf`) | Done              |
| `System.Linq` (`Min`, `Max`, `Sum`, `Average`)                 | Done              |
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
- `Array.Sort`, `Array.Reverse`, and `Array.IndexOf` sort, reverse, and search. `System.Linq` adds `Min`, `Max`, `Sum`, and `Average`.
- **Remaining:** multidimensional arrays and the rest of the arrays topics.
