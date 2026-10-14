# Conditional Statements in C# (if/else, switch)

**Date:** 5 October 2026

Notes on decision making with **`if`**, **`else if`**, **`else`**, and **`switch`**, with runnable examples.

## Table of Contents

- [What are Conditional Statements?](#what-are-conditional-statements)
- [if Statement](#if-statement)
- [if-else Statement](#if-else-statement)
- [else if Ladder](#else-if-ladder)
- [Nested if](#nested-if)
- [switch Statement](#switch-statement)
- [Grouping Cases](#grouping-cases)
- [switch Expression](#switch-expression)
- [if/else vs switch](#ifelse-vs-switch)
- [Common Mistakes](#common-mistakes)
- [Summary](#summary)
- [Problem Solving](#problem-solving)

---

## What are Conditional Statements?

Conditional statements let a program **choose which code to run** based on a condition. The condition must evaluate to a `bool` (`true` or `false`).

```
condition true  -> run this block
condition false -> skip it (or run another block)
```

---

## if Statement

Runs a block **only if** the condition is `true`.

**Syntax**

```csharp
if (condition)
{
    // runs when condition is true
}
```

**Example**

```csharp
int age = 20;

if (age >= 18)
{
    Console.WriteLine("You are an adult.");
}
// Output: You are an adult.
```

---

## if-else Statement

Runs one block if the condition is `true`, otherwise runs the `else` block.

**Syntax**

```csharp
if (condition)
{
    // true block
}
else
{
    // false block
}
```

**Example**

```csharp
int marks = 35;

if (marks >= 40)
{
    Console.WriteLine("Pass");
}
else
{
    Console.WriteLine("Fail");
}
// Output: Fail
```

---

## else if Ladder

Use when there are **more than two** possible outcomes. Conditions are checked **top to bottom**, and the **first true** one runs. The rest are skipped.

**Syntax**

```csharp
if (condition1)
{
}
else if (condition2)
{
}
else if (condition3)
{
}
else
{
    // runs if none of the above is true
}
```

**Example: grade calculator**

```csharp
int marks = 85;

if (marks >= 90)
{
    Console.WriteLine("Grade: A+");
}
else if (marks >= 80)
{
    Console.WriteLine("Grade: A");
}
else if (marks >= 70)
{
    Console.WriteLine("Grade: B");
}
else if (marks >= 40)
{
    Console.WriteLine("Grade: C");
}
else
{
    Console.WriteLine("Grade: F");
}
// Output: Grade: A
```

> Order matters. If you check `marks >= 40` first, an 85 would match there and never reach the higher grades.

### Using logical operators in conditions

```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18 && hasLicense)
{
    Console.WriteLine("You can drive.");
}

string day = "Sunday";

if (day == "Saturday" || day == "Sunday")
{
    Console.WriteLine("Weekend");
}
```

---

## Nested if

An `if` inside another `if`. The inner one runs only if the outer condition is `true`.

```csharp
decimal balance = 1500m;
decimal price = 1200m;
bool isPremium = false;

if (balance >= price)
{
    if (price > 1000m || isPremium)
    {
        Console.WriteLine("Free Delivery");
    }
    else
    {
        Console.WriteLine("Paid Delivery");
    }
}
else
{
    Console.WriteLine("Insufficient balance");
}
// Output: Free Delivery
```

> Too much nesting is hard to read. Often `&&` or an `else if` can replace it.

---

## switch Statement

Compares **one value** against several fixed **cases**. It is cleaner than a long `else if` ladder when you check the same variable for exact matches.

**Syntax**

```csharp
switch (expression)
{
    case value1:
        // code
        break;

    case value2:
        // code
        break;

    default:
        // runs if no case matches
        break;
}
```

**Example: day of the week**

```csharp
int day = 3;

switch (day)
{
    case 1:
        Console.WriteLine("Monday");
        break;
    case 2:
        Console.WriteLine("Tuesday");
        break;
    case 3:
        Console.WriteLine("Wednesday");
        break;
    default:
        Console.WriteLine("Invalid day");
        break;
}
// Output: Wednesday
```

### Key points

- Each case must end with **`break`** (or `return` / `throw` / `goto`). C# does **not** allow accidental fall-through.
- `default` is optional and runs when nothing matches. It can be placed anywhere, but conventionally goes last.
- Works with `int`, `char`, `string`, `enum`, and other types. It does **not** work with ranges like `>= 50` in the classic form.

**Example with string**

```csharp
string command = "stop";

switch (command)
{
    case "start":
        Console.WriteLine("Starting...");
        break;
    case "stop":
        Console.WriteLine("Stopping...");
        break;
    default:
        Console.WriteLine("Unknown command");
        break;
}
// Output: Stopping...
```

---

## Grouping Cases

Stack empty cases to run the **same code** for multiple values.

```csharp
int month = 4;

switch (month)
{
    case 12:
    case 1:
    case 2:
        Console.WriteLine("Winter");
        break;
    case 3:
    case 4:
    case 5:
        Console.WriteLine("Spring");
        break;
    default:
        Console.WriteLine("Other season");
        break;
}
// Output: Spring
```

**Example: vowel or consonant**

```csharp
char ch = 'e';

switch (ch)
{
    case 'a':
    case 'e':
    case 'i':
    case 'o':
    case 'u':
        Console.WriteLine("Vowel");
        break;
    default:
        Console.WriteLine("Consonant");
        break;
}
// Output: Vowel
```

---

## switch Expression

Modern C# (version 8+) offers a shorter form that **returns a value**.

```csharp
int day = 3;

string dayName = day switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    4 => "Thursday",
    5 => "Friday",
    6 => "Saturday",
    7 => "Sunday",
    _ => "Invalid day"      // _ is the default case
};

Console.WriteLine(dayName); // Wednesday
```

It can also handle ranges with relational patterns:

```csharp
int marks = 85;

string grade = marks switch
{
    >= 90 => "A+",
    >= 80 => "A",
    >= 70 => "B",
    >= 40 => "C",
    _ => "F"
};

Console.WriteLine(grade); // A
```

---

## if/else vs switch

|                               | `if / else`                               | `switch`                             |
| ----------------------------- | ----------------------------------------- | ------------------------------------ |
| Best for                      | Ranges, complex conditions (`&&`, `\|\|`) | Exact matches of one value           |
| Conditions                    | Any `bool` expression                     | One expression compared to constants |
| Readability with many options | Gets long                                 | Cleaner                              |
| Example                       | `marks >= 80 && marks < 90`               | `day == 3`                           |

**Rule of thumb:** use `if/else` for ranges and combined conditions, and `switch` when comparing one variable against many fixed values.

---

## Common Mistakes

```csharp
// 1. Using = instead of ==
int x = 5;
// if (x = 5) { }      // Error: assignment, not comparison
if (x == 5) { }        // Correct

// 2. Semicolon right after if
if (x > 3);            // Bug: empty statement, block below always runs
{
    Console.WriteLine("Always prints!");
}

// 3. Forgetting break in switch
// case 1:
//     Console.WriteLine("One");
// case 2:             // Error: control cannot fall through

// 4. Wrong order in else if ladder
// Check the most specific/highest condition first.
```

---

## Summary

- **`if`** runs code when a condition is `true`; **`else`** handles the other case.
- **`else if`** chains multiple conditions; the first true one wins.
- **Nested `if`** puts one decision inside another.
- **`switch`** matches one value against many cases; each case needs `break`, and `default` handles no match.
- **Switch expressions** (`=>`) are a short form that returns a value.
- Use `if/else` for ranges and complex conditions, `switch` for exact matches.

---

## Problem Solving

### Problem 1: Easy

### Description

Write a C# program that takes a student's grade as input and displays the letter grade.

1. Take the grade (integer) as input from the user.
2. Display the letter grade based on the following ranges:
   - 80 to 100: `A`
   - 60 to 79: `B`
   - 40 to 59: `C`
   - 0 to 39: `F`
3. If the grade is outside the range 0 to 100, display "Invalid Grade".

### Code

```csharp
using System;

class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Enter your Grade: ");
        int grade = Convert.ToInt32(Console.ReadLine());

        if (grade >= 80 && grade <= 100)
        {
            Console.WriteLine("A");
        }
        else if (grade >= 60 && grade <= 79)
        {
            Console.WriteLine("B");
        }
        else if (grade >= 40 && grade <= 59)
        {
            Console.WriteLine("C");
        }
        else if (grade < 40 && grade >= 0)
        {
            Console.WriteLine("F");
        }
        else
        {
            Console.WriteLine("Invalid Grade");
        }
    }
}
```

### Problem 2: Medium

### Description

Write a C# program that calculates the final bill of a customer based on their membership type.

1. Take the bill amount and the membership type (1, 2, or 3) as input from the user.
2. Apply the discount according to the membership type using a `switch` statement:
   - **1 (Regular):** 5% discount only if the bill is more than 1000. Otherwise, no discount.
   - **2 (Premium):** 10% discount if the bill is more than 1000. Otherwise, 5% discount.
   - **3 (VIP):** 20% discount on any bill amount.
3. Display the final bill as an integer (decimal part removed).
4. For any other membership type, display "Invalid Membership Type".

### Code

```csharp
using System;

class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Your Bill: ");
        int bill = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Membership Type (1, 2, 3): ");
        int membershipType = Convert.ToInt32(Console.ReadLine());

        decimal discountBill;
        int finalBillInt;

        switch (membershipType)
        {
            case 1: // Regular
                if (bill > 1000)
                {
                    discountBill = bill * 0.95m;
                    finalBillInt = (int)discountBill;
                    Console.WriteLine($"Final Bill: {finalBillInt}");
                }
                else
                {
                    Console.WriteLine($"Final Bill: {bill}");
                }
                break;

            case 2: // Premium
                if (bill > 1000)
                {
                    discountBill = bill * 0.90m;
                }
                else
                {
                    discountBill = bill * 0.95m;
                }
                finalBillInt = (int)discountBill;
                Console.WriteLine($"Final Bill: {finalBillInt}");
                break;

            case 3: // VIP
                discountBill = bill * 0.80m;
                finalBillInt = (int)discountBill;
                Console.WriteLine($"Final Bill: {finalBillInt}");
                break;

            default:
                Console.WriteLine("Invalid Membership Type");
                break;
        }
    }
}
```
