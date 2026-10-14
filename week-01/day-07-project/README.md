# 🧮 Full Calculator with Menu

**Date:** 9 October 2026

A simple menu-based calculator built as a C# console application. It is written in a beginner-friendly style using only `while` and `for` loops, `if / else` conditions, and a `switch` statement.

## Features

| Option | Feature             | Description                                              |
| ------ | ------------------- | -------------------------------------------------------- |
| 1      | Addition            | Adds two numbers                                         |
| 2      | Subtraction         | Subtracts the second number from the first               |
| 3      | Multiplication      | Multiplies two numbers                                   |
| 4      | Division            | Divides the first number by the second (checks for zero) |
| 5      | Remainder           | Finds the remainder of a division (checks for zero)      |
| 6      | Even/Odd Check      | Tells whether a number is even or odd                    |
| 7      | Find Largest Number | Finds the largest among the numbers you enter            |
| 8      | Exit                | Closes the program                                       |

## Menu Preview

```
===== Calculator Menu =====
1. Addition
2. Subtraction
3. Multiplication
4. Division
5. Remainder
6. Even/Odd Check
7. Find Largest Number
8. Exit
Enter your choice:
```

## Concepts Used

- `while` loop to keep showing the menu until the user exits
- `for` loop to read multiple numbers (Find Largest Number)
- `switch` statement with a separate `case` for each menu option
- `if / else` conditions for zero checks and the even/odd check
- `Console.ReadLine()` with `Convert.ToInt32()` / `Convert.ToDouble()` for input
- Comments explaining each part of the code

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or later (or Visual Studio with the .NET desktop workload)

## How to Run

### Using the command line

```bash
dotnet new console -n CalculatorApp
cd CalculatorApp
```

Replace the contents of the generated `Program.cs` with the code from this project, then run:

```bash
dotnet run
```

### Using Visual Studio

1. Create a new **Console App** project.
2. Paste the code from `Program.cs` into the project's `Program.cs`.
3. Press **F5** (or click **Start**) to run.

## Example Run

```
Enter your choice: 1
Enter first number: 10
Enter second number: 5
Addition = 15

Enter your choice: 6
Enter a number: 7
7 is Odd

Enter your choice: 7
How many numbers? 3
Enter number 1: 4
Enter number 2: 9
Enter number 3: 2
Largest number = 9

Enter your choice: 8
Program closed. Goodbye!
```

## Known Limitations

- Entering letters or leaving an input empty will crash the program, because input is not validated.
- Entering a non-integer for the menu choice or for the even/odd check is not handled.

## Possible Improvements

- Add input validation with `TryParse`
- Add more operations such as power and square root
- Keep a history of past results

## Project Structure

```
CalculatorApp/
├── Program.cs
└── README.md
```
