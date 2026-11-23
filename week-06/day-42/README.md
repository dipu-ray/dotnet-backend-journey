# C# Advanced: Exception Handling

**Date:** 23 November 2026

Exception handling practice in C#: three step-by-step examples (from no handling to a full `try`, multiple `catch`, `finally`, and `throw`), followed by four problems (two easy and two medium).

## Table of Contents
- [1. Quick Recap](#1-quick-recap)
- [2. Step-by-Step Examples](#2-step-by-step-examples)
  - [Example 1: Normal Code (No Handling)](#example-1-normal-code-no-handling)
  - [Example 2: try, catch, finally](#example-2-try-catch-finally)
  - [Example 3: try, Multiple catch, finally, throw](#example-3-try-multiple-catch-finally-throw)
- [3. Problems](#3-problems)
  - [Problem 1 (Easy): Age Eligibility](#problem-1-easy-age-eligibility)
  - [Problem 2 (Easy): Calculator](#problem-2-easy-calculator)
  - [Problem 3 (Medium): Student Result](#problem-3-medium-student-result)
  - [Problem 4 (Medium): Order Summary](#problem-4-medium-order-summary)

---

## 1. Quick Recap

| Keyword | Purpose |
|---------|---------|
| `try` | The code that **might** cause an error |
| `catch` | Handles a **specific** kind of error |
| `finally` | Runs **always**, whether or not an error happened |
| `throw` | **Raises** an exception on purpose, with a custom message |

---

## 2. Step-by-Step Examples

### Example 1: Normal Code (No Handling)

```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to our application!");

        Console.Write("Enter number 1: ");
        int number1 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter number 2: ");
        int number2 = Convert.ToInt32(Console.ReadLine());

        int result = number1 / number2;
        Console.WriteLine($"Division result: {result}");

        Console.WriteLine("Goodbye!");
    }
}
```

**Problem:** any wrong input stops the whole program.

---

### Example 2: try, catch, finally

```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to our application!");

        try
        {
            Console.Write("Enter number 1: ");
            int number1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter number 2: ");
            int number2 = Convert.ToInt32(Console.ReadLine());

            int result = number1 / number2;
            Console.WriteLine($"Division result: {result}");
        }

        catch (Exception e)
        {
            Console.WriteLine($"{e}");
        }

        finally
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
```

**Sample runs**

| Input | Output |
|-------|--------|
| `10` and `2` | `Division result: 5` then `Goodbye!` |
| `10` and `0` | The exception details (type, message, and stack trace), then `Goodbye!` |
| `abc` | The exception details, then `Goodbye!` |

```
Welcome to our application!
Enter number 1: 10
Enter number 2: 0
System.DivideByZeroException: Attempted to divide by zero.
   at Program.Main(String[] args) ...
Goodbye!
```

---

### Example 3: try, Multiple catch, finally, throw

```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to our application!");

        try
        {
            Console.Write("Enter number 1: ");
            string input1 = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(input1))
            {
                throw new ArgumentException("Input 1 can not be null or empty.");
            }
            int number1 = Convert.ToInt32(input1);

            Console.Write("Enter number 2: ");
            string input2 = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(input2))
            {
                throw new ArgumentException("Input 2 can not be null or empty.");
            }
            int number2 = Convert.ToInt32(input2);

            if (number2 > 1000)
            {
                throw new ArgumentException("Number 2 can not be greater than 1000.");
            }

            int result = number1 / number2;
            Console.WriteLine($"Division result: {result}");
        }

        catch (ArgumentNullException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (OverflowException)
        {
            Console.WriteLine($"Your enter was too big. Please enter under ToInt32 under. Thank you!");
        }

        catch (FormatException)
        {
            Console.WriteLine($"Please enter number. Thank you!");
        }

        catch (DivideByZeroException)
        {
            Console.WriteLine($"You can not divide by 0. Please enter correct value. Thank you!");
        }

        catch (ArgumentException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (Exception e)
        {
            Console.WriteLine($"{e}");
        }

        finally
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
```

**Sample runs** (`Goodbye!` is printed at the end of every run)

| Input 1 | Input 2 | Exception | Output |
|---------|---------|-----------|--------|
| `10` | `2` | None | `Division result: 5` |
| *(empty)* | | `ArgumentException` (thrown by the code) | `Input 1 can not be null or empty.` |
| `10` | *(empty)* | `ArgumentException` (thrown by the code) | `Input 2 can not be null or empty.` |
| `abc` | | `FormatException` | `Please enter number. Thank you!` |
| `99999999999` | | `OverflowException` | `Your enter was too big. Please enter under ToInt32 under. Thank you!` |
| `10` | `0` | `DivideByZeroException` | `You can not divide by 0. Please enter correct value. Thank you!` |
| `10` | `2000` | `ArgumentException` (thrown by the code) | `Number 2 can not be greater than 1000.` |

---

## 3. Problems

### Problem 1 (Easy): Age Eligibility

**Problem:** Read the user's **age** and tell whether the user is **eligible** (age `18` or more).

- An **empty** input is an error.
- An age **below 0** is an error.
- A **non-numeric** input is an error.
- A closing message must be printed **every time**.

**Solution**
```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter your age: ");
            string input = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input can not null or empty.");
            }
            int age = Convert.ToInt32(input);

            if (age < 0)
            {
                throw new ArgumentException("Age can not under 0. Enter correct age.");
            }

            else if (age >= 18)
            {
                Console.WriteLine("You are eligible.");
            }
            else
            {
                Console.WriteLine("You are not eligible.");
            }
        }

        catch (ArgumentException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (FormatException)
        {
            Console.WriteLine("Please enter a valid number.");
        }

        finally
        {
            Console.WriteLine("Thank you for using our application!");
        }
    }
}
```

**Sample runs**

| Input | Output |
|-------|--------|
| `20` | `You are eligible.` |
| `18` | `You are eligible.` |
| `15` | `You are not eligible.` |
| `-5` | `Age can not under 0. Enter correct age.` |
| `abc` | `Please enter a valid number.` |
| *(empty)* | `Input can not null or empty.` |

Every run ends with `Thank you for using our application!`.

---

### Problem 2 (Easy): Calculator

**Problem:** Read **two numbers** and an **operator** (`+`, `-`, `*`, `/`), and print the **result**.

- An **empty** number input is an error.
- A **non-numeric** number is an error.
- **Division by zero** is an error.
- An **invalid operator** is an error.
- A closing message must be printed **every time**.

**Solution**
```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter first number: ");
            string input1 = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input1))
            {
                throw new ArgumentException("Input cannot be empty.");
            }
            double number1 = Convert.ToInt32(input1);

            Console.Write("Enter second number: ");
            string input2 = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input2))
            {
                throw new ArgumentException("Input cannot be empty.");
            }
            double number2 = Convert.ToInt32(input2);

            Console.Write("Enter operator (+, -, *, /): ");
            string opt = Console.ReadLine() ?? "";

            double value = 0;

            switch (opt)
            {
                case "+":
                    value = number1 + number2;
                    break;
                case "-":
                    value = number1 - number2;
                    break;
                case "*":
                    value = number1 * number2;
                    break;
                case "/":
                    if (number2 == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    value = number1 / number2;
                    break;
                default:
                    throw new ArgumentException("Invalid operator!");
            }
            Console.WriteLine($"Result: {value}");
        }

        catch (ArgumentException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (FormatException)
        {
            Console.WriteLine("Enter numbers.");
        }

        catch (DivideByZeroException)
        {
            Console.WriteLine("You can't divide by 0.");
        }

        finally
        {
            Console.WriteLine("Calculator closed. Thank you!");
        }
    }
}
```

**Sample runs**

| First | Second | Operator | Output |
|-------|--------|----------|--------|
| `10` | `5` | `+` | `Result: 15` |
| `10` | `5` | `-` | `Result: 5` |
| `10` | `5` | `*` | `Result: 50` |
| `10` | `3` | `/` | `Result: 3.3333333333333335` |
| `10` | `0` | `/` | `You can't divide by 0.` |
| `10` | `5` | `%` | `Invalid operator!` |
| `abc` | | | `Enter numbers.` |
| *(empty)* | | | `Input cannot be empty.` |

Every run ends with `Calculator closed. Thank you!`.

---

### Problem 3 (Medium): Student Result

**Problem:** Read a **student name** and **marks**, and print the **grade**:

| Marks | Grade |
|-------|-------|
| 80 to 100 | A+ |
| 70 to 79 | A |
| 60 to 69 | A- |
| 50 to 59 | B |
| 40 to 49 | C |
| Below 40 | Fail |

- An **empty name** or **empty marks** is an error.
- **Non-numeric** marks are an error.
- Marks **outside 0 to 100** are an error.
- A closing message must be printed **every time**.

**Solution**
```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter student name: ");
            string nameValue = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(nameValue))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            Console.Write("Enter marks: ");
            string markValue = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(markValue))
            {
                throw new ArgumentException("Mark cannot be empty.");
            }
            int marks = Convert.ToInt32(markValue);

            Console.WriteLine($"\nStudent Name: {nameValue}");

            if (marks < 0 || marks > 100)
            {
                throw new ArgumentException("Enter 0 to 100 range.");
            }

            else if (marks >= 80 && marks <= 100)
            {
                Console.WriteLine("Result: A+");
            }

            else if (marks >= 70 && marks <= 79)
            {
                Console.WriteLine("Result: A");
            }

            else if (marks >= 60 && marks <= 69)
            {
                Console.WriteLine("Result: A-");
            }

            else if (marks >= 50 && marks <= 59)
            {
                Console.WriteLine("Result: B");
            }

            else if (marks >= 40 && marks <= 49)
            {
                Console.WriteLine("Result: C");
            }

            else
            {
                Console.WriteLine("Fail");
            }
        }

        catch (ArgumentException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (FormatException)
        {
            Console.WriteLine("Please enter numeric marks.");
        }

        finally
        {
            Console.WriteLine("\nResult checking completed.");
        }
    }
}
```

**Sample runs**

| Name | Marks | Output |
|------|-------|--------|
| `Rahim` | `85` | `Student Name: Rahim`, `Result: A+` |
| `Rahim` | `72` | `Student Name: Rahim`, `Result: A` |
| `Rahim` | `65` | `Student Name: Rahim`, `Result: A-` |
| `Rahim` | `55` | `Student Name: Rahim`, `Result: B` |
| `Rahim` | `45` | `Student Name: Rahim`, `Result: C` |
| `Rahim` | `30` | `Student Name: Rahim`, `Fail` |
| `Rahim` | `150` | `Student Name: Rahim`, `Enter 0 to 100 range.` |
| `Rahim` | `abc` | `Please enter numeric marks.` |
| *(empty)* | | `Name cannot be empty.` |
| `Rahim` | *(empty)* | `Mark cannot be empty.` |

Every run ends with a blank line and `Result checking completed.`

**Full output of one run** (`Rahim`, `85`)
```
Enter student name: Rahim
Enter marks: 85

Student Name: Rahim
Result: A+

Result checking completed.
```

---

### Problem 4 (Medium): Order Summary

**Problem:** Read a **product name**, a **quantity**, and a **price**, and print an **order summary** with the **total amount**.

| Rule | Error message |
|------|---------------|
| Product name is empty | `You can not name empty.` |
| Quantity is empty | `Error: Quantity can not be empty.` |
| Quantity is `0` or less | `Error: Quantity must be greater than zero.` |
| Quantity is more than `100` | `Error: Enter quantity under 100` |
| Price is empty | `Error: Price can not be empty` |
| Price is `0` or less | `Error: Enter price above 0` |
| A value is not a number | `Error: Please enter a valid numeric value.` |
| A number is too large | `Error: Number is too large.` |
| Any other error | `Unexpected error: ` and the message |

A closing message must be printed **every time**.

**Solution**
```csharp
using System;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter product name: ");
            string product = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(product))
            {
                throw new ArgumentException("You can not name empty.");
            }

            Console.Write("Enter quantity: ");
            string quantityString = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(quantityString))
            {
                throw new ArgumentException("Error: Quantity can not be empty.");
            }
            int quantity = Convert.ToInt32(quantityString);
            if (quantity <= 0)
            {
                throw new ArgumentException("Error: Quantity must be greater than zero.");
            }
            else if (quantity > 100)
            {
                throw new ArgumentException("Error: Enter quantity under 100");
            }

            Console.Write("Enter price: ");
            string priceString = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(priceString))
            {
                throw new ArgumentException("Error: Price can not be empty");
            }
            double price = Convert.ToDouble(priceString);
            if (price <= 0)
            {
                throw new ArgumentException("Error: Enter price above 0");
            }

            double total = quantity * price;

            Console.WriteLine("\n===== ORDER SUMMARY =====");
            Console.WriteLine($"Product: {product}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Price: {price}");
            Console.WriteLine($"Total Amount: {total}");
            Console.WriteLine("Order completed successfully!");
        }

        catch (ArgumentException e)
        {
            Console.WriteLine($"{e.Message}");
        }

        catch (FormatException)
        {
            Console.WriteLine("Error: Please enter a valid numeric value.");
        }

        catch (OverflowException)
        {
            Console.WriteLine("Error: Number is too large.");
        }

        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        finally
        {
            Console.WriteLine("\nThank you for shopping!");
        }
    }
}
```

**Sample runs**

| Product | Quantity | Price | Output |
|---------|----------|-------|--------|
| `Pen` | `5` | `12.5` | The order summary, with `Total Amount: 62.5` |
| `Pen` | `0` | | `Error: Quantity must be greater than zero.` |
| `Pen` | `150` | | `Error: Enter quantity under 100` |
| `Pen` | `5` | `-5` | `Error: Enter price above 0` |
| `Pen` | `abc` | | `Error: Please enter a valid numeric value.` |
| `Pen` | `99999999999` | | `Error: Number is too large.` |
| *(empty)* | | | `You can not name empty.` |
| `Pen` | *(empty)* | | `Error: Quantity can not be empty.` |
| `Pen` | `5` | *(empty)* | `Error: Price can not be empty` |

Every run ends with a blank line and `Thank you for shopping!`

**Full output of a successful run** (`Pen`, `5`, `12.5`)
```
Enter product name: Pen
Enter quantity: 5
Enter price: 12.5

===== ORDER SUMMARY =====
Product: Pen
Quantity: 5
Price: 12.5
Total Amount: 62.5
Order completed successfully!

Thank you for shopping!
```