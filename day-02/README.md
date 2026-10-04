# Operators in C# (Arithmetic, Comparison, Logical)

Notes on **arithmetic**, **comparison**, and **logical** operators with runnable examples.

## Table of Contents

- [What is an Operator?](#what-is-an-operator)
- [Arithmetic Operators](#arithmetic-operators)
- [Comparison Operators](#comparison-operators)
- [Logical Operators](#logical-operators)
- [Operator Precedence](#operator-precedence)
- [All Together](#all-together)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)
- [Problem Solving](#problem-solving)

---

## What is an Operator?

An **operator** is a symbol that performs an operation on one or more values (called **operands**).

```csharp
int result = 10 + 5;
//           ^  ^  ^
//           |  |  operand
//           |  operator
//           operand
```

| Category   | Purpose               | Result type |
| ---------- | --------------------- | ----------- |
| Arithmetic | Math calculations     | number      |
| Comparison | Compare two values    | `bool`      |
| Logical    | Combine `bool` values | `bool`      |

---

## Arithmetic Operators

Used for mathematical calculations.

| Operator | Name                | Example  | Result    |
| -------- | ------------------- | -------- | --------- |
| `+`      | Addition            | `10 + 3` | `13`      |
| `-`      | Subtraction         | `10 - 3` | `7`       |
| `*`      | Multiplication      | `10 * 3` | `30`      |
| `/`      | Division            | `10 / 3` | `3` (int) |
| `%`      | Modulus (remainder) | `10 % 3` | `1`       |
| `++`     | Increment           | `x++`    | `x + 1`   |
| `--`     | Decrement           | `x--`    | `x - 1`   |

```csharp
int a = 10;
int b = 3;

Console.WriteLine(a + b);  // 13
Console.WriteLine(a - b);  // 7
Console.WriteLine(a * b);  // 30
Console.WriteLine(a / b);  // 3  (integer division drops the decimal part)
Console.WriteLine(a % b);  // 1
```

### Integer division vs decimal division

If **both** operands are `int`, the result is `int`. Make at least one operand `decimal` (or `double`) to keep the fractional part.

```csharp
Console.WriteLine(10 / 3);      // 3
Console.WriteLine(10 / 3.0);    // 3.3333333333333335 (double)
Console.WriteLine(10 / 3m);     // 3.3333333333333333333333333333 (decimal)

decimal price = 100m;
int people = 3;
Console.WriteLine(price / people); // 33.333333333333333333333333333
```

> Dividing an integer by `0` throws `DivideByZeroException`.

### Modulus (%) use cases

```csharp
// Even or odd
int number = 7;
Console.WriteLine(number % 2 == 0);  // False (odd)

// Last digit
Console.WriteLine(12345 % 10);       // 5
```

### Increment and decrement

```csharp
int x = 5;

x++;                    // x = 6
x--;                    // x = 5

// Prefix vs postfix
int y = 5;
Console.WriteLine(y++); // prints 5, then y becomes 6 (postfix)
Console.WriteLine(y);   // 6

int z = 5;
Console.WriteLine(++z); // z becomes 6, then prints 6 (prefix)
```

### Compound assignment

Shortcut for "calculate and store back".

```csharp
int n = 10;

n += 5;   // n = n + 5  -> 15
n -= 3;   // n = n - 3  -> 12
n *= 2;   // n = n * 2  -> 24
n /= 4;   // n = n / 4  -> 6
n %= 4;   // n = n % 4  -> 2
```

### String concatenation with `+`

```csharp
string first = "Hello";
string second = "World";
Console.WriteLine(first + " " + second); // Hello World
Console.WriteLine("Age: " + 25);         // Age: 25
```

---

## Comparison Operators

Compare two values. The result is always a **`bool`** (`true` or `false`).

| Operator | Meaning                  | Example  | Result  |
| -------- | ------------------------ | -------- | ------- |
| `==`     | Equal to                 | `5 == 5` | `true`  |
| `!=`     | Not equal to             | `5 != 3` | `true`  |
| `>`      | Greater than             | `5 > 3`  | `true`  |
| `<`      | Less than                | `5 < 3`  | `false` |
| `>=`     | Greater than or equal to | `5 >= 5` | `true`  |
| `<=`     | Less than or equal to    | `4 <= 3` | `false` |

```csharp
int a = 10;
int b = 20;

Console.WriteLine(a == b);  // False
Console.WriteLine(a != b);  // True
Console.WriteLine(a > b);   // False
Console.WriteLine(a < b);   // True
Console.WriteLine(a >= 10); // True
Console.WriteLine(b <= 15); // False
```

### `=` vs `==`

```csharp
int x = 5;          // = assigns a value
bool same = x == 5; // == compares values -> true
```

### Comparing strings

```csharp
string s1 = "apple";
string s2 = "apple";
string s3 = "Apple";

Console.WriteLine(s1 == s2); // True
Console.WriteLine(s1 == s3); // False (case sensitive)
Console.WriteLine(s1.Equals(s3, StringComparison.OrdinalIgnoreCase)); // True
```

### Using comparison in `if`

```csharp
int marks = 75;

if (marks >= 40)
{
    Console.WriteLine("Pass");
}
else
{
    Console.WriteLine("Fail");
}
// Output: Pass
```

---

## Logical Operators

Combine or invert **`bool`** values.

| Operator | Name | Meaning                                   |
| -------- | ---- | ----------------------------------------- |
| `&&`     | AND  | `true` only if **both** sides are `true`  |
| `\|\|`   | OR   | `true` if **at least one** side is `true` |
| `!`      | NOT  | Reverses the value                        |

### Truth tables

**AND (`&&`)**

| A     | B     | A && B   |
| ----- | ----- | -------- |
| true  | true  | **true** |
| true  | false | false    |
| false | true  | false    |
| false | false | false    |

**OR (`||`)**

| A     | B     | A \|\| B |
| ----- | ----- | -------- |
| true  | true  | **true** |
| true  | false | **true** |
| false | true  | **true** |
| false | false | false    |

**NOT (`!`)**

| A     | !A    |
| ----- | ----- |
| true  | false |
| false | true  |

```csharp
bool hasId = true;
bool hasTicket = false;

Console.WriteLine(hasId && hasTicket); // False
Console.WriteLine(hasId || hasTicket); // True
Console.WriteLine(!hasId);             // False
```

### Combining comparison and logical operators

```csharp
int age = 22;
bool hasLicense = true;

bool canDrive = age >= 18 && hasLicense;
Console.WriteLine(canDrive); // True

// Range check
int score = 85;
bool isB = score >= 80 && score < 90;
Console.WriteLine(isB); // True

// OR example
string day = "Saturday";
bool isWeekend = day == "Saturday" || day == "Sunday";
Console.WriteLine(isWeekend); // True
```

### Short-circuit evaluation

`&&` and `||` stop as soon as the result is known.

```csharp
int a = 0;

// a != 0 is false, so the right side is never evaluated (no divide by zero)
if (a != 0 && 10 / a > 1)
{
    Console.WriteLine("Safe");
}
else
{
    Console.WriteLine("Skipped"); // Skipped
}
```

---

## Operator Precedence

Operators with higher precedence run first. When unsure, use parentheses `()`.

| Order       | Operators            |
| ----------- | -------------------- |
| 1 (highest) | `()`                 |
| 2           | `!`, `++`, `--`      |
| 3           | `*`, `/`, `%`        |
| 4           | `+`, `-`             |
| 5           | `<`, `>`, `<=`, `>=` |
| 6           | `==`, `!=`           |
| 7           | `&&`                 |
| 8 (lowest)  | `\|\|`               |

```csharp
Console.WriteLine(2 + 3 * 4);    // 14  (multiplication first)
Console.WriteLine((2 + 3) * 4);  // 20  (parentheses first)

bool r = true || false && false;
Console.WriteLine(r);            // True  (&& before ||)

bool r2 = (true || false) && false;
Console.WriteLine(r2);           // False
```

---

## All Together

```csharp
using System;

class Program
{
    static void Main()
    {
        decimal price = 250m;
        int quantity = 4;
        decimal budget = 1200m;
        bool hasCoupon = true;

        // Arithmetic
        decimal total = price * quantity;
        if (hasCoupon)
        {
            total -= 100m;
        }

        // Comparison
        bool withinBudget = total <= budget;

        // Logical
        bool canBuy = withinBudget && quantity > 0;

        Console.WriteLine($"Total      : {total}");
        Console.WriteLine($"In budget  : {withinBudget}");
        Console.WriteLine($"Can buy    : {canBuy}");
    }
}
```

**Output**

```
Total      : 900
In budget  : True
Can buy    : True
```

---

## Practice Tasks

1. Take two numbers and print their sum, difference, product, quotient (as `decimal`), and remainder.
2. Check whether a number is even or odd using `%`.
3. Take an `age` and print whether the person is a child (`< 13`), teenager (`13-19`), or adult (`>= 20`) using comparison and logical operators.
4. Check if a year is a leap year: divisible by 4 and not by 100, or divisible by 400.
5. Predict the output before running: `Console.WriteLine(10 + 2 * 5 > 15 && !(4 == 5));`

---

## Summary

- **Arithmetic** (`+ - * / %`, `++ --`): calculations; `int / int` gives `int`.
- **Comparison** (`== != > < >= <=`): compare values and return `bool`.
- **Logical** (`&& || !`): combine `bool` values; `&&` and `||` short-circuit.
- Use `()` to make the order of evaluation clear.

## Problem Solving

### Problem 1

### Description

Write a C# program to check an order's balance and decide the delivery status.

1. Declare variables for the total cost, premium member status, and account balance.
2. Calculate the remaining balance using `accountBalance - totalCost` and display it.
3. Check whether the account has the minimum balance (at least 500 TK) and display the result.
4. If the minimum balance is available:
   - If the total cost is more than 1000 TK **or** the customer is a premium member, display "Free Delivery".
   - Otherwise, display "Paid Delivery".
5. If the minimum balance is not available, display "Order Failed: Insufficient account balance."

### Code

```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        decimal totalCost = 1200m;
        bool isPremium = false;
        decimal accountBalance = 1500m;

        decimal remainingBalance = accountBalance - totalCost;
        Console.WriteLine("Remaining Balance: " + remainingBalance + " TK");

        bool hasEnoughBalance = accountBalance >= 500m;
        Console.WriteLine("Has minimum balance? " + hasEnoughBalance);

        if (hasEnoughBalance)
        {
            if (totalCost > 1000m || isPremium == true)
            {
                Console.WriteLine("Delivery Status: Free Delivery");
            }
            else
            {
                Console.WriteLine("Delivery Status: Paid Delivery");
            }
        }
        else
        {
            Console.WriteLine("Order Failed: Insufficient account balance.");
        }
    }
}
```

### Problem 2

### Description

Write a C# program to calculate a ride fare and check discount eligibility.

1. Declare variables for the base fare, distance, rating, VIP status, and peak hour status.
2. If it is peak hour, add a 50 TK fee to the base fare and display the total fare with the peak hour fee. Otherwise, display the normal total fare.
3. A customer is eligible for a discount if the distance is more than 10 and the rating is more than 4.5, **or** the customer is a VIP. Display whether the customer is eligible.
4. If eligible, display "Congratulations! You got a discount." Otherwise, display "Regular Fare Applied."

### Code

```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        decimal baseFare = 250m;
        int distance = 12;
        double rating = 4.7;
        bool isVIP = false;
        bool isPeakHour = true;

        decimal totalFare = baseFare;

        if (isPeakHour == true)
        {
            totalFare = baseFare + 50m;
            Console.WriteLine($"Total Fare (with Peak Hour fee): {totalFare} TK");
        }
        else
        {
            Console.WriteLine($"Total Fare: {totalFare} TK");
        }

        bool isEligibleForDiscount = (distance > 10 && rating > 4.5) || isVIP == true;

        Console.WriteLine($"Is eligible for discount? {isEligibleForDiscount}");

        if (isEligibleForDiscount == true)
        {
            Console.WriteLine("Congratulations! You got a discount.");
        }
        else
        {
            Console.WriteLine("Regular Fare Applied.");
        }
    }
}
```
