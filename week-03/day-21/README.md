# C# Recap: Basics

**Date:** 28 October 2026

A quick recap of the C# fundamentals:

1. Backslash characters (escape sequences)
2. Variables and data types
3. Data type casting
4. Operators
5. Conditions
6. Loops

## Table of Contents
- [1. Backslash Characters (Escape Sequences)](#1-backslash-characters-escape-sequences)
- [2. Variables and Data Types](#2-variables-and-data-types)
- [3. Data Type Casting](#3-data-type-casting)
- [4. Operators](#4-operators)
- [5. Conditions](#5-conditions)
- [6. Loops](#6-loops)
- [All Together: Small Program](#all-together-small-program)
- [Cheat Sheet](#cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# 1. Backslash Characters (Escape Sequences)

A **backslash** `\` inside a string starts an **escape sequence**: a special character that is hard to type directly (new line, tab, a quote, and so on).

| Escape | Meaning |
|--------|---------|
| `\n` | New line |
| `\t` | Tab |
| `\\` | A backslash |
| `\"` | A double quote |
| `\'` | A single quote |
| `\r` | Carriage return (move to the start of the line) |
| `\b` | Backspace |
| `\0` | Null character |
| `\a` | Alert (beep) |
| `\uXXXX` | Unicode character (for example `\u0041` is `A`) |

```csharp
Console.WriteLine("Hello\nWorld");              // new line
Console.WriteLine("Name:\tRahim");              // tab
Console.WriteLine("C:\\Users\\Rahim");          // backslashes
Console.WriteLine("He said \"Hi\"");            // double quotes
Console.WriteLine("It\'s fine");                // single quote
Console.WriteLine("\u0041");                    // A
```

**Output**
```
Hello
World
Name:	Rahim
C:\Users\Rahim
He said "Hi"
It's fine
A
```

### Verbatim string `@`

Prefix the string with `@` to **ignore escape sequences**. This is useful for file paths. Inside `@"..."`, write a quote as `""`.

```csharp
string path = @"C:\Users\Rahim\Documents";
string quote = @"He said ""Hi""";

Console.WriteLine(path);    // C:\Users\Rahim\Documents
Console.WriteLine(quote);   // He said "Hi"
```

### String interpolation `$`

Put variables inside `{ }`.

```csharp
string name = "Rahim";
int age = 22;
Console.WriteLine($"Name: {name}\nAge: {age}");
```

> `$@"..."` combines both (interpolation and verbatim).

---

# 2. Variables and Data Types

## Variables

A **variable** is a **named place in memory** that stores a value.

```csharp
int age = 25;          // data type + name + value
string name = "Rahim";
age = 26;              // the value can change
```

**Naming rules**
- Start with a letter or `_`. No spaces or special characters.
- Cannot be a keyword (`class`, `int`).
- C# is **case sensitive** (`Age` and `age` are different).
- Use `camelCase` for local variables (`studentName`).

## Common Data Types

| Type | Stores | Size | Example | Default |
|------|--------|------|---------|---------|
| `int` | Whole number | 4 bytes | `42` | `0` |
| `long` | Large whole number | 8 bytes | `9000000000L` | `0` |
| `short` | Small whole number | 2 bytes | `100` | `0` |
| `byte` | 0 to 255 | 1 byte | `200` | `0` |
| `float` | Decimal (about 7 digits) | 4 bytes | `3.14f` | `0` |
| `double` | Decimal (about 15 digits) | 8 bytes | `3.14159` | `0` |
| `decimal` | Precise decimal (money) | 16 bytes | `99.99m` | `0` |
| `bool` | `true` or `false` | 1 byte | `true` | `false` |
| `char` | One character (single quotes) | 2 bytes | `'A'` | `'\0'` |
| `string` | Text (double quotes) | varies | `"Hello"` | `null` |

```csharp
int age = 25;
long population = 170000000L;
float height = 5.9f;
double pi = 3.14159265;
decimal price = 99.99m;
bool isStudent = true;
char grade = 'A';
string name = "Rahim";
```

> Suffixes: `f` for `float`, `m` for `decimal`, `L` for `long`. Without `f` or `m`, a decimal number is a `double`.

**Range of `int`:** `-2,147,483,648` to `2,147,483,647`
```csharp
Console.WriteLine(int.MaxValue);   // 2147483647
Console.WriteLine(int.MinValue);   // -2147483648
```

### `decimal` vs `double`

```csharp
Console.WriteLine(0.1 + 0.2);      // 0.30000000000000004  (double, small error)
Console.WriteLine(0.1m + 0.2m);    // 0.3                  (decimal, exact)
```
Use **`decimal` for money**.

## `var`, `const`, and nullable

```csharp
var count = 10;               // the compiler picks the type (int). The type stays fixed.
const double Pi = 3.1416;     // a constant, can never change

int? score = null;            // nullable: an int that can also be null
Console.WriteLine(score ?? 0);   // 0 (use 0 if null)
```

## Strings, quickly

```csharp
string text = "Hello World";
Console.WriteLine(text.Length);              // 11
Console.WriteLine(text.ToUpper());           // HELLO WORLD
Console.WriteLine(text.Contains("World"));   // True
Console.WriteLine(text.Substring(0, 5));     // Hello
Console.WriteLine("Hi " + "there");          // Hi there
```

## Value types and reference types

| | Value types | Reference types |
|---|-------------|-----------------|
| Examples | `int`, `double`, `decimal`, `bool`, `char` | `string`, arrays, classes |
| Stores | The value itself | A reference (address) to the data |
| Can be `null`? | No (unless `int?`) | Yes |

---

# 3. Data Type Casting

**Casting** means converting a value from one data type to another.

## Implicit casting (automatic)

A **smaller type to a larger type**. Nothing is lost, so C# does it automatically.

```
byte -> short -> int -> long -> float -> double
```

```csharp
int a = 10;
double b = a;        // int to double, automatic
long c = a;          // int to long, automatic

Console.WriteLine(b);   // 10
```

`char` converts to `int` (its character code):
```csharp
int code = 'A';
Console.WriteLine(code);   // 65
```

## Explicit casting (manual)

A **larger type to a smaller type**. Data can be lost, so you must write the type in `( )`.

```csharp
double d = 9.78;
int i = (int)d;          // the decimal part is cut off (truncated)

Console.WriteLine(i);    // 9   (not 10)
```

```csharp
int number = 65;
char letter = (char)number;
Console.WriteLine(letter);   // A
```

## Casting in calculations: integer division

```csharp
int x = 10, y = 3;

Console.WriteLine(x / y);            // 3      (int / int = int)
Console.WriteLine((double)x / y);    // 3.3333333333333335
Console.WriteLine(x / 3.0);          // 3.3333333333333335
```

## The `Convert` class

Converts between types, including from strings.

```csharp
int a = Convert.ToInt32("123");        // 123
double b = Convert.ToDouble("3.14");   // 3.14
string c = Convert.ToString(45);       // "45"
bool d = Convert.ToBoolean("true");    // true

Console.WriteLine(Convert.ToInt32(9.78));   // 10  (rounds)
Console.WriteLine((int)9.78);               // 9   (cuts off)
```

| | `(int)9.78` | `Convert.ToInt32(9.78)` |
|---|-------------|--------------------------|
| Result | `9` | `10` |
| Behavior | Truncates (cuts the decimal part) | Rounds to the nearest whole number |

> `Convert.ToInt32` rounds halves to the nearest **even** number: `2.5` becomes `2`, and `3.5` becomes `4`.

## `Parse` and `TryParse` (string to number)

```csharp
int n1 = int.Parse("123");        // 123
// int.Parse("abc");              // FormatException: crashes the program

// TryParse does not crash: it returns true or false
if (int.TryParse("abc", out int n2))
{
    Console.WriteLine(n2);
}
else
{
    Console.WriteLine("Invalid number");   // this runs
}
```

Reading a number from the user:
```csharp
Console.Write("Enter your age: ");
int age = Convert.ToInt32(Console.ReadLine());
```

## Converting to a string

```csharp
int n = 45;
string s1 = n.ToString();    // "45"
string s2 = "" + n;          // "45"
string s3 = $"{n}";          // "45"
```

## Overflow

```csharp
int big = int.MaxValue;
Console.WriteLine(big + 1);            // -2147483648 (wraps around, no error by default)

// checked: throws OverflowException instead of wrapping
// checked { int r = big + 1; }
```

| Cast type | Direction | Needs `( )`? | Data loss? |
|-----------|-----------|--------------|------------|
| Implicit | Small to large (`int` to `double`) | No | No |
| Explicit | Large to small (`double` to `int`) | **Yes** | Possible |
| `Convert` / `Parse` | Any, including strings | Method call | Rounds, or throws on bad input |

---

# 4. Operators

## Arithmetic

| Operator | Name | Example | Result |
|----------|------|---------|--------|
| `+` | Add | `10 + 3` | `13` |
| `-` | Subtract | `10 - 3` | `7` |
| `*` | Multiply | `10 * 3` | `30` |
| `/` | Divide | `10 / 3` | `3` (int) |
| `%` | Modulus (remainder) | `10 % 3` | `1` |
| `++` | Increment | `x++` | `x + 1` |
| `--` | Decrement | `x--` | `x - 1` |

```csharp
int y = 5;
Console.WriteLine(y++);   // 5, then y becomes 6 (postfix)
Console.WriteLine(++y);   // y becomes 7, then prints 7 (prefix)

Console.WriteLine(7 % 2 == 0);   // False (7 is odd)
```

## Assignment

| Operator | Same as |
|----------|---------|
| `=` | Assign |
| `+=` | `x = x + y` |
| `-=` | `x = x - y` |
| `*=` | `x = x * y` |
| `/=` | `x = x / y` |
| `%=` | `x = x % y` |

```csharp
int n = 10;
n += 5;    // 15
n *= 2;    // 30
```

## Comparison (result is `bool`)

| Operator | Meaning |
|----------|---------|
| `==` | Equal to |
| `!=` | Not equal to |
| `>` / `<` | Greater / less than |
| `>=` / `<=` | Greater / less than or equal to |

```csharp
int a = 10, b = 20;
Console.WriteLine(a == b);   // False
Console.WriteLine(a < b);    // True
```

> `=` assigns a value. `==` compares values.

## Logical

| Operator | Name | Meaning |
|----------|------|---------|
| `&&` | AND | True only if **both** are true |
| `\|\|` | OR | True if **at least one** is true |
| `!` | NOT | Reverses the value |

```csharp
int age = 22;
bool hasLicense = true;

Console.WriteLine(age >= 18 && hasLicense);   // True
Console.WriteLine(age < 18 || hasLicense);    // True
Console.WriteLine(!hasLicense);               // False
```

`&&` and `||` **short-circuit**: they stop as soon as the result is known.

## Ternary and null-coalescing

```csharp
// Ternary: condition ? valueIfTrue : valueIfFalse
int marks = 75;
string result = marks >= 40 ? "Pass" : "Fail";      // Pass

// Null-coalescing: use a default if the value is null
string name = null;
Console.WriteLine(name ?? "Unknown");               // Unknown
```

## Precedence (high to low)

1. `()`
2. `++`, `--`, `!`
3. `*`, `/`, `%`
4. `+`, `-`
5. `<`, `>`, `<=`, `>=`
6. `==`, `!=`
7. `&&`
8. `||`
9. `?:` and assignment

```csharp
Console.WriteLine(2 + 3 * 4);     // 14
Console.WriteLine((2 + 3) * 4);   // 20
```

---

# 5. Conditions

Conditions let the program **choose which code to run**. The condition must be a `bool`.

## `if`, `else if`, `else`

```csharp
int marks = 85;

if (marks >= 90)
{
    Console.WriteLine("A+");
}
else if (marks >= 80)
{
    Console.WriteLine("A");        // this runs
}
else if (marks >= 40)
{
    Console.WriteLine("Pass");
}
else
{
    Console.WriteLine("Fail");
}
```

The conditions are checked **from top to bottom**. The **first true** one runs, and the rest are skipped.

## Nested `if`

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
```

## `switch` statement

Compares **one value** against many fixed cases.

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
```

- Every case ends with `break`.
- `default` runs when nothing matches.
- Stack cases to share code:

```csharp
switch (month)
{
    case 12:
    case 1:
    case 2:
        Console.WriteLine("Winter");
        break;
}
```

## `switch` expression (C# 8+)

```csharp
string grade = marks switch
{
    >= 90 => "A+",
    >= 80 => "A",
    >= 40 => "Pass",
    _     => "Fail"       // _ is the default
};
```

| Use | When |
|-----|------|
| `if / else if` | Ranges and combined conditions |
| `switch` | One value compared with many fixed values |
| `? :` | A short choice between two values |

---

# 6. Loops

A **loop** repeats code while a condition is `true`.

## `for` loop

Best when you **know how many times** to repeat.

```csharp
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}
// 1 2 3 4 5
```

Parts: `for (start; condition; update)`.

```csharp
for (int i = 5; i >= 1; i--)      // count down
{
    Console.WriteLine(i);
}
```

## `while` loop

Checks the condition **first**. It can run **zero times**.

```csharp
int i = 1;
while (i <= 5)
{
    Console.WriteLine(i);
    i++;                     // do not forget the update
}
```

## `do-while` loop

Checks the condition **after** the body. It runs **at least once**.

```csharp
int i = 1;
do
{
    Console.WriteLine(i);
    i++;
}
while (i <= 5);              // note the semicolon
```

## `foreach` loop

Goes through **every item** of an array, list, or string.

```csharp
string[] names = { "Rahim", "Karim", "Nusrat" };

foreach (string name in names)
{
    Console.WriteLine(name);
}

foreach (char ch in "Hi")
{
    Console.WriteLine(ch);   // H, then i
}
```

## `break` and `continue`

| Keyword | Effect |
|---------|--------|
| `break` | Stops the loop immediately |
| `continue` | Skips to the next round |

```csharp
for (int i = 1; i <= 10; i++)
{
    if (i == 3) continue;    // skip 3
    if (i == 6) break;       // stop at 6
    Console.Write(i + " ");
}
// 1 2 4 5
```

## Nested loops

```csharp
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write("* ");
    }
    Console.WriteLine();
}
```

```
* 
* * 
* * * 
```

## Which loop to use

| Loop | Condition checked | Minimum runs | Best for |
|------|-------------------|--------------|----------|
| `for` | Before each run | 0 | Known number of repeats |
| `while` | Before each run | 0 | Unknown number of repeats |
| `do-while` | **After** each run | **1** | Menus, input validation |
| `foreach` | Goes through all items | 0 | Arrays, lists, strings |

> An **infinite loop** happens when the condition never becomes `false` (for example, forgetting `i++`). Press `Ctrl + C` to stop it.

---

# All Together: Small Program

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter how many students: ");
        int count = Convert.ToInt32(Console.ReadLine());      // casting: string to int

        decimal total = 0m;

        for (int i = 1; i <= count; i++)                       // loop
        {
            Console.Write($"Enter marks of student {i}: ");
            decimal marks = Convert.ToDecimal(Console.ReadLine());
            total += marks;                                    // operator

            string grade;
            if (marks >= 80) grade = "A+";                     // condition
            else if (marks >= 60) grade = "B";
            else if (marks >= 40) grade = "C";
            else grade = "F";

            Console.WriteLine($"Student {i}:\tMarks = {marks}\tGrade = {grade}");   // \t escape
        }

        decimal average = total / count;
        Console.WriteLine($"\nAverage marks: {average:F2}");   // \n escape
    }
}
```

**Sample run**
```
Enter how many students: 3
Enter marks of student 1: 85
Student 1:	Marks = 85	Grade = A+
Enter marks of student 2: 62
Student 2:	Marks = 62	Grade = B
Enter marks of student 3: 35
Student 3:	Marks = 35	Grade = F

Average marks: 60.67
```

---

# Cheat Sheet

| Topic | Key points |
|-------|------------|
| Escape sequences | `\n` new line, `\t` tab, `\\` backslash, `\"` quote, `@"..."` verbatim |
| Variables | Named memory. `int`, `double`, `decimal`, `bool`, `char`, `string`. `var`, `const` |
| Casting | Implicit: small to large. Explicit: `(int)x`. `Convert`, `Parse`, `TryParse` |
| Operators | Arithmetic, assignment, comparison, logical (`&&`, `\|\|`, `!`), ternary `?:` |
| Conditions | `if / else if / else`, `switch`, switch expression |
| Loops | `for`, `while`, `do-while`, `foreach`, `break`, `continue` |

---

## Practice Tasks

1. Print your name, city, and age on **separate lines** using `\n` and a **tab-separated** line using `\t`.
2. Print the file path `C:\Projects\Demo` using escape sequences, then using `@`.
3. Declare one variable of each type (`int`, `double`, `decimal`, `bool`, `char`, `string`) and print them with string interpolation.
4. Convert `9.99` to `int` using `(int)` and `Convert.ToInt32`, and explain the difference.
5. Read two numbers from the user and print their **sum, difference, product, and quotient** (as `decimal`).
6. Use `int.TryParse` to read a number safely, and print an error message for invalid input.
7. Check whether a number is **even or odd**, and whether it is **positive, negative, or zero**.
8. Write a **grade calculator** using `if / else if`, then rewrite it using a **switch expression**.
9. Print the **multiplication table** of a number using a `for` loop.
10. Print the sum of the numbers 1 to 100 using `while`, then using `do-while`.
11. Use `foreach` to print every item of a string array.
12. Print the **countdown** from `N` to `1`, then `Liftoff!`.

---

## Summary

- **Escape sequences** (`\n`, `\t`, `\\`, `\"`) add special characters to strings. `@"..."` turns them off, and `$"..."` inserts variables.
- **Variables** store values. Choose the right **data type**: `int`, `double`, `decimal` (money), `bool`, `char`, `string`.
- **Casting:** implicit (small to large) is automatic. Explicit (large to small) needs `( )` and can lose data. Use `Convert`, `Parse`, and `TryParse` for strings.
- **Operators:** arithmetic, assignment, comparison, and logical. Remember that `int / int` gives an `int`. Use `( )` to control the order.
- **Conditions:** `if / else if / else` for ranges and combined conditions, `switch` for one value against many.
- **Loops:** `for` (known count), `while` (unknown count), `do-while` (at least once), `foreach` (every item). Use `break` and `continue` to control them.