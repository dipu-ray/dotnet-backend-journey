# Accounts Module and C# Method, Class, Object

**Date:** 1 November 2026

Today's work:

1. **Recap of the business management software**, and understanding its **most important module: Accounts**.
2. **C#:** Method, Class, and Object.

## Table of Contents

- [Part 1: Business Software Recap and the Accounts Module](#part-1-business-software-recap-and-the-accounts-module)
  - [Basic Accounting Concepts](#1-basic-accounting-concepts)
  - [Debit and Credit](#2-debit-and-credit)
  - [Worked Example: Transactions and Journal Entries](#3-worked-example-transactions-and-journal-entries)
  - [Ledger and Trial Balance](#4-ledger-and-trial-balance)
  - [Accounts for a Farm](#5-accounts-for-a-farm)
  - [Maintaining Accounts](#11-maintaining-accounts)
  - [What I Saw in the Software](#12-what-i-saw-in-the-software)
- [Part 2: C# Method, Class, Object](#part-2-c-method-class-object)
  - [Methods](#1-methods)
  - [Classes and Objects](#2-classes-and-objects)
- [Summary](#summary)

---

# Part 1: Business Software Recap and the Accounts Module

## 1. Basic Accounting Concepts

### The five account types

| Type                 | Meaning                             | Examples                                      |
| -------------------- | ----------------------------------- | --------------------------------------------- |
| **Asset**            | What the business **owns**          | Cash, Bank, Inventory, Receivables, Equipment |
| **Liability**        | What the business **owes**          | Payables, Loans                               |
| **Equity**           | The owner's share                   | Owner's Capital, Retained Earnings            |
| **Income (Revenue)** | Money **earned**                    | Sales, Service Income                         |
| **Expense**          | Money **spent** to run the business | Rent, Salary, Fuel, Labor                     |

**The accounting equation**

```
Assets  =  Liabilities  +  Equity
```

**Profit**

```
Profit  =  Income  -  Expenses
```

## 2. Debit and Credit

**Double-entry bookkeeping:** every transaction is recorded **twice**: a **debit** in one account and a **credit** in another, for the **same amount**. So the books always balance.

| Account type  | Increases with | Decreases with |
| ------------- | -------------- | -------------- |
| **Asset**     | **Debit**      | Credit         |
| **Expense**   | **Debit**      | Credit         |
| **Liability** | **Credit**     | Debit          |
| **Equity**    | **Credit**     | Debit          |
| **Income**    | **Credit**     | Debit          |

Memory aid: **Assets and Expenses** are increased by **Debit**. **Liabilities, Equity, and Income** are increased by **Credit**.

**Debit** and **credit** are just the **left** and **right** side of an account. They do not mean "good" or "bad", and they are not "plus" and "minus" on their own.

## 3. Worked Example: Transactions and Journal Entries

A small farm business records these transactions.

| #   | Date       | Transaction                        | Debit               | Credit              | Amount |
| --- | ---------- | ---------------------------------- | ------------------- | ------------------- | ------ |
| 1   | 2026-10-01 | Owner puts money into the business | Cash                | Owner's Capital     | 100000 |
| 2   | 2026-10-03 | Buy seed for cash                  | Inventory           | Cash                | 5000   |
| 3   | 2026-10-05 | Buy fertilizer on credit           | Inventory           | Accounts Payable    | 8000   |
| 4   | 2026-10-10 | Sell rice on credit                | Accounts Receivable | Sales               | 50000  |
| 5   | 2026-10-15 | Customer pays part of the due      | Cash                | Accounts Receivable | 30000  |
| 6   | 2026-10-18 | Pay labor wages in cash            | Labor Expense       | Cash                | 6000   |
| 7   | 2026-10-20 | Pay the fertilizer supplier        | Accounts Payable    | Cash                | 8000   |

**Reading entry 4:** we sold rice for 50000 but have not been paid yet. The **customer owes us** (asset `Accounts Receivable` goes up, so **debit**), and we **earned income** (`Sales` goes up, so **credit**).

**Reading entry 7:** we paid the supplier. The **payable decreases** (a liability decreases with **debit**), and **cash decreases** (an asset decreases with **credit**).

> This simple example does not record the **cost of goods sold** (the cost of the rice sold). Real software also records it, so profit is more accurate.

## 4. Ledger and Trial Balance

### Cash ledger (account `Cash`)

| Date       | Description      | Debit  | Credit | Balance |
| ---------- | ---------------- | ------ | ------ | ------- |
| 2026-10-01 | Owner investment | 100000 |        | 100000  |
| 2026-10-03 | Buy seed         |        | 5000   | 95000   |
| 2026-10-15 | Customer payment | 30000  |        | 125000  |
| 2026-10-18 | Labor wages      |        | 6000   | 119000  |
| 2026-10-20 | Pay supplier     |        | 8000   | 111000  |

### Trial balance (after the 7 entries)

| Account             | Type      | Debit total | Credit total | Balance       |
| ------------------- | --------- | ----------- | ------------ | ------------- |
| Cash                | Asset     | 130000      | 19000        | **111000 Dr** |
| Inventory           | Asset     | 13000       | 0            | **13000 Dr**  |
| Accounts Receivable | Asset     | 50000       | 30000        | **20000 Dr**  |
| Accounts Payable    | Liability | 8000        | 8000         | **0**         |
| Owner's Capital     | Equity    | 0           | 100000       | **100000 Cr** |
| Sales               | Income    | 0           | 50000        | **50000 Cr**  |
| Labor Expense       | Expense   | 6000        | 0            | **6000 Dr**   |
| **Total**           |           | **207000**  | **207000**   | Balanced      |

- Total debits (`207000`) equal total credits (`207000`), so the books **balance**.
- **Profit** (simple) = Sales `50000` - Labor Expense `6000` = **`44000`**.
- If the trial balance does **not** balance, an entry is wrong or missing.

## 5. Accounts for a Farm

| Account type  | Farm accounts                                                                                               |
| ------------- | ----------------------------------------------------------------------------------------------------------- |
| **Asset**     | Cash, Bank, Seed Stock, Fertilizer Stock, Feed Stock, Livestock, Tractor and Equipment, Accounts Receivable |
| **Liability** | Accounts Payable (suppliers), Bank Loan                                                                     |
| **Equity**    | Owner's Capital, Retained Earnings                                                                          |
| **Income**    | Crop Sales, Milk Sales, Egg Sales, Other Income                                                             |
| **Expense**   | Seed, Fertilizer, Pesticide, Feed, Labor, Fuel, Irrigation, Veterinary, Repair and Maintenance, Transport   |

Useful farm reports from the accounts:

- **Expense per category** (where the money goes)
- **Income per product** (which crop or product earns most)
- **Profit per season or per field** (use a field or cost center tag on each entry)
- **Receivables** (buyers who still owe money)
- **Cash position** each week

---

# Part 2: C# Method, Class, Object

## 1. Methods

A **method** is a **named block of code** that does one task. You write it once and **call** it as many times as you need.

**Syntax**

```csharp
accessModifier returnType MethodName(parameters)
{
    // code
    return value;     // not needed when the return type is void
}
```

| Part        | Meaning                                                                 |
| ----------- | ----------------------------------------------------------------------- |
| `static`    | Belongs to the class, so it can be called without an object             |
| Return type | The type of the result (`int`, `string`, `bool`, or `void` for nothing) |
| Name        | PascalCase (`CalculateTotal`)                                           |
| Parameters  | Inputs the method needs                                                 |
| `return`    | Sends the result back                                                   |

### Method with no return value (`void`)

```csharp
static void Greet(string name)
{
    Console.WriteLine($"Hello, {name}!");
}

Greet("Rahim");      // Hello, Rahim!
```

### Method that returns a value

```csharp
static int Add(int a, int b)
{
    return a + b;
}

int sum = Add(10, 5);
Console.WriteLine(sum);      // 15
```

### Short form (expression-bodied)

```csharp
static int Square(int x) => x * x;

Console.WriteLine(Square(6));    // 36
```

### Default and named parameters

```csharp
static decimal CalculatePrice(decimal price, decimal discountPercent = 0)
{
    return price - (price * discountPercent / 100);
}

Console.WriteLine(CalculatePrice(1000m));                          // 1000
Console.WriteLine(CalculatePrice(1000m, 10m));                     // 900
Console.WriteLine(CalculatePrice(discountPercent: 20m, price: 500m));   // 400 (named arguments)
```

### Method overloading

Several methods with the **same name** but **different parameters**.

```csharp
static int Add(int a, int b) => a + b;
static double Add(double a, double b) => a + b;
static int Add(int a, int b, int c) => a + b + c;

Console.WriteLine(Add(2, 3));          // 5
Console.WriteLine(Add(2.5, 3.5));      // 6
Console.WriteLine(Add(1, 2, 3));       // 6
```

### Returning a result with `out`

```csharp
static bool TryDivide(int a, int b, out int result)
{
    if (b == 0)
    {
        result = 0;
        return false;
    }
    result = a / b;
    return true;
}

if (TryDivide(10, 2, out int answer))
{
    Console.WriteLine(answer);      // 5
}
```

### Parameters vs arguments, and scope

| Term          | Meaning                                                            |
| ------------- | ------------------------------------------------------------------ |
| **Parameter** | The variable in the method definition (`int a`)                    |
| **Argument**  | The value you pass when calling (`Add(10, 5)`)                     |
| **Scope**     | A variable declared inside a method exists only inside that method |

### Why use methods?

- **Reuse** the same code
- **Organize** a big program into small parts
- Easier to **test** and **fix**
- Easier to **read**

---

## 2. Classes and Objects

A **class** is a **blueprint** (a template). An **object** is a **real thing created from that blueprint**.

|          | Class                  | Object                                        |
| -------- | ---------------------- | --------------------------------------------- |
| Meaning  | The blueprint / design | A real instance made from the blueprint       |
| Example  | `Student`              | Rahim, Karim, and Nusrat as `Student` objects |
| Memory   | No data of its own     | Has its own data                              |
| How many | One definition         | As many objects as you need                   |

Real-life example: a **house plan** is the class. Each **house built from the plan** is an object.

A class has:

| Member                  | Meaning                                | Example                   |
| ----------------------- | -------------------------------------- | ------------------------- |
| **Fields / Properties** | The data (what the object **has**)     | `Name`, `Marks`           |
| **Methods**             | The actions (what the object **does**) | `GetGrade()`, `Display()` |
| **Constructor**         | Sets up a new object                   | `Student(...)`            |

### Creating a class

```csharp
class Student
{
    // Properties (data)
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Marks { get; set; }

    // Method (action)
    public string GetGrade()
    {
        if (Marks >= 80) return "A+";
        if (Marks >= 70) return "A";
        if (Marks >= 40) return "Pass";
        return "Fail";
    }

    public void Display()
    {
        Console.WriteLine($"{Id} | {Name} | {Marks} | {GetGrade()}");
    }
}
```

### Creating objects

```csharp
Student s1 = new Student();      // new creates the object
s1.Id = 1;
s1.Name = "Rahim";
s1.Marks = 85;

Student s2 = new Student();
s2.Id = 2;
s2.Name = "Karim";
s2.Marks = 62;

s1.Display();      // 1 | Rahim | 85 | A+
s2.Display();      // 2 | Karim | 62 | Pass
```

- `new Student()` **creates** the object.
- `s1.Name` uses the **dot operator** to reach a member.
- `s1` and `s2` are **separate objects**, each with its own data.

### Object initializer (short way)

```csharp
Student s3 = new Student
{
    Id = 3,
    Name = "Nusrat",
    Marks = 91
};

s3.Display();      // 3 | Nusrat | 91 | A+
```

---

## Summary

- **Accounts** is the most important module, because it records the **money side** of every other module and produces the **profit, cash, and balance** reports.
- Every transaction is recorded with **double entry**: a **debit** and a **credit** of the same amount. Debits equal credits in the **trial balance**.
- **Assets and expenses** increase with debit. **Liabilities, equity, and income** increase with credit.
- Correct mistakes with a **reversing or correcting entry**, never by deleting, and keep regular **backups**.
- A **method** is a reusable block of code with parameters and an optional return value. **Overloading** allows the same name with different parameters.
- A **class** is a blueprint. An **object** is a real instance made with `new`. A **constructor** sets the starting values.
