# Variables & Data Types in C#

Notes on **variables** and the basic data types **`int`**, **`string`**, **`bool`**, and **`decimal`**, with runnable examples.

## Table of Contents

- [What is a Variable?](#what-is-a-variable)
- [Declaring and Assigning](#declaring-and-assigning)
- [Naming Rules](#naming-rules)
- [What is a Data Type?](#what-is-a-data-type)
- [int](#int)
- [string](#string)
- [bool](#bool)
- [decimal](#decimal)
- [Quick Comparison](#quick-comparison)
- [var and const](#var-and-const)
- [Practice Tasks](#practice-tasks)
- [Problem Solving](#problem-solving)

---

## What is a Variable?

A **variable** is a named place in memory that stores a value. You can read the value and change it while the program runs.

Think of it as a labeled box: the label is the **name**, the content is the **value**, and the box type is the **data type**.

```csharp
int age = 25;
//  ^    ^
//  |    value
//  variable name
```

## Declaring and Assigning

```csharp
// Declaration only
int score;

// Assignment
score = 90;

// Declaration + initialization (most common)
int level = 1;

// Changing the value later
level = 2;

// Multiple variables of the same type
int a = 1, b = 2, c = 3;
```

> A local variable must be assigned a value before it is used, or the compiler gives an error.

## Naming Rules

| Rule                            | Valid                         | Invalid                |
| ------------------------------- | ----------------------------- | ---------------------- |
| Start with a letter or `_`      | `age`, `_count`               | `1age`                 |
| No spaces or special characters | `firstName`                   | `first name`, `price$` |
| Cannot be a keyword             | `className`                   | `class`, `int`         |
| Case sensitive                  | `Age` and `age` are different |                        |

**Convention:** use `camelCase` for local variables (`studentName`, `totalPrice`).

## What is a Data Type?

A **data type** tells the compiler what kind of value a variable can hold, how much memory it needs, and what operations are allowed.

C# is **statically typed**: the type is fixed at compile time.

```csharp
int number = 10;
number = "hello";   // Error: cannot assign string to int
```

---

## int

Stores **whole numbers** (no decimal point), positive or negative.

- Size: 32 bits (4 bytes)
- Range: `-2,147,483,648` to `2,147,483,647`
- Default value: `0`

```csharp
int age = 25;
int temperature = -5;
int population = 170000000;

int a = 10;
int b = 3;

Console.WriteLine(a + b);  // 13
Console.WriteLine(a - b);  // 7
Console.WriteLine(a * b);  // 30
Console.WriteLine(a / b);  // 3   (integer division, decimal part is dropped)
Console.WriteLine(a % b);  // 1   (remainder)

Console.WriteLine(int.MaxValue); // 2147483647
Console.WriteLine(int.MinValue); // -2147483648
```

> For larger whole numbers use `long` (64-bit).

---

## string

Stores **text** (a sequence of characters). Always written inside **double quotes**.

- Default value: `null`
- Strings are **immutable**: operations create a new string instead of changing the old one.

```csharp
string name = "Rahim";
string city = "Dhaka";
string empty = "";

// Concatenation
string fullName = "Abdur" + " " + "Rahim";

// String interpolation (recommended)
int age = 22;
string message = $"My name is {name} and I am {age} years old.";
Console.WriteLine(message);
// My name is Rahim and I am 22 years old.

// Common properties and methods
string text = "Hello World";
Console.WriteLine(text.Length);          // 11
Console.WriteLine(text.ToUpper());       // HELLO WORLD
Console.WriteLine(text.ToLower());       // hello world
Console.WriteLine(text.Contains("World")); // True
Console.WriteLine(text.Replace("World", "C#")); // Hello C#
Console.WriteLine(text.Substring(0, 5)); // Hello
Console.WriteLine(text[0]);              // H

// Escape characters
string path = "C:\\Users\\Rahim";   // backslash
string quote = "He said \"Hi\"";    // double quote
string lines = "Line1\nLine2";      // new line
```

> A single character uses `char` with single quotes: `char grade = 'A';`

---

## bool

Stores only **two values**: `true` or `false`. Used for conditions and decisions.

- Size: 1 byte
- Default value: `false`

```csharp
bool isStudent = true;
bool isLoggedIn = false;

// Result of comparison is a bool
int age = 20;
bool isAdult = age >= 18;
Console.WriteLine(isAdult); // True

// Comparison operators: == != > < >= <=
Console.WriteLine(5 == 5);  // True
Console.WriteLine(5 != 5);  // False
Console.WriteLine(7 > 10);  // False

// Logical operators: && (AND), || (OR), ! (NOT)
bool hasId = true;
bool hasTicket = false;
Console.WriteLine(hasId && hasTicket); // False
Console.WriteLine(hasId || hasTicket); // True
Console.WriteLine(!hasId);             // False

// Using bool in if
if (isAdult)
{
    Console.WriteLine("You can vote.");
}
else
{
    Console.WriteLine("You cannot vote yet.");
}
```

---

## decimal

Stores **numbers with a fractional part** with high precision. Best for **money and financial calculations**.

- Size: 128 bits (16 bytes)
- Precision: about 28-29 significant digits
- Default value: `0m`
- Literals must end with **`m`** or **`M`**

```csharp
decimal price = 99.99m;
decimal salary = 50000.50m;
decimal tax = 0.15m;

decimal total = price + (price * tax);
Console.WriteLine(total); // 114.9885

// Forgetting the m suffix causes an error
// decimal x = 10.5;   // Error: 10.5 is a double
decimal y = 10.5m;     // Correct
```

### decimal vs double vs float

```csharp
double d = 0.1 + 0.2;
Console.WriteLine(d);  // 0.30000000000000004  (floating point error)

decimal m = 0.1m + 0.2m;
Console.WriteLine(m);  // 0.3  (exact)
```

| Type      | Suffix         | Precision     | Use for                 |
| --------- | -------------- | ------------- | ----------------------- |
| `float`   | `f`            | ~7 digits     | graphics, games         |
| `double`  | `d` (optional) | ~15-16 digits | scientific calculations |
| `decimal` | `m`            | ~28-29 digits | **money, finance**      |

---

## Quick Comparison

| Type      | Stores                    | Example   | Default | Size     |
| --------- | ------------------------- | --------- | ------- | -------- |
| `int`     | Whole number              | `42`      | `0`     | 4 bytes  |
| `string`  | Text                      | `"Hello"` | `null`  | varies   |
| `bool`    | true / false              | `true`    | `false` | 1 byte   |
| `decimal` | Precise fractional number | `19.99m`  | `0m`    | 16 bytes |

### All together

```csharp
using System;

class Program
{
    static void Main()
    {
        string productName = "Notebook";
        int quantity = 3;
        decimal unitPrice = 45.50m;
        bool inStock = true;

        decimal totalPrice = quantity * unitPrice;

        Console.WriteLine($"Product : {productName}");
        Console.WriteLine($"Quantity: {quantity}");
        Console.WriteLine($"Price   : {unitPrice}");
        Console.WriteLine($"Total   : {totalPrice}");
        Console.WriteLine($"In stock: {inStock}");
    }
}
```

**Output**

```
Product : Notebook
Quantity: 3
Price   : 45.50
Total   : 136.50
In stock: True
```

---

## var and const

```csharp
// var: compiler infers the type from the value (type is still fixed)
var count = 10;        // int
var title = "C#";      // string
var isOk = true;       // bool
var amount = 5.5m;     // decimal

// const: value can never change, must be set at declaration
const decimal Pi = 3.1416m;
const int MaxUsers = 100;
// MaxUsers = 200;  // Error
```

---

## Practice Tasks

1. Create variables for your name (`string`), age (`int`), student status (`bool`), and monthly allowance (`decimal`), then print them with string interpolation.
2. Write a program that takes `price` and `quantity` and prints the total using `decimal`.
3. Check if a number stored in an `int` variable is even and store the result in a `bool`.
4. Show the difference between `0.1 + 0.2` using `double` and `decimal`.

---

## Summary

- A **variable** is a named storage location for a value.
- **`int`**: whole numbers.
- **`string`**: text in double quotes.
- **`bool`**: `true` or `false`.
- **`decimal`**: precise fractional numbers, use the `m` suffix, ideal for money.

## Problem Solving

### Problem 1: Beginner Level

### Description

Write a C# program to store and display the information of a product.

1. Declare variables for the product name, quantity, price, and stock availability.
2. Display the product name.
3. Display the quantity.
4. Display the price (2 decimal places).
5. Display whether the product is in stock.

## Code

```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        // Declare variables
        string productName = "Laptop";
        int quantity = 2;
        decimal price = 75000.50m;
        bool inStock = true;

        // Display product information
        Console.WriteLine($"Product Name: {productName}");
        Console.WriteLine($"Quantity: {quantity}");
        Console.WriteLine($"Price: ${price:F2}");
        Console.WriteLine($"In Stock: {inStock}");
    }
}
```

### Problem 2: Mid-Level

### Description

Write a C# program to create a profile for a freelancer and convert their earnings from USD to BDT.

1. Declare variables for the freelancer's name, number of completed projects, monthly earnings in USD, top rated status, and the USD to BDT conversion rate.
2. Calculate the total earnings in BDT using `earningsUSD * USDToBDT`.
3. Display the freelancer's name, completed projects, monthly earnings (2 decimal places), and top rated status.
4. Display the converted total earnings in BDT (2 decimal places).

## Code

```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        // Declare variables
        string name = "Arif";
        int completedProjects = 45;
        decimal earningsUSD = 1250.50m;
        bool isTopRated = true;
        decimal USDToBDT = 121.75m;
        decimal totalEarningBDT = earningsUSD * USDToBDT;

        // Profile of the freelancer
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Completed Projects: {completedProjects}");
        Console.WriteLine($"Monthly Earnings: ${earningsUSD:F2}");
        Console.WriteLine($"Top Rated: {isTopRated}");

        // Conversion of earnings to BDT
        Console.WriteLine($"Total Earning in BDT: {totalEarningBDT:F2}");
    }
}
```
