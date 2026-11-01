# Accounts Module and C# Method, Class, Object

**Date:** 1 November 2026

Today's work:

1. **Recap of the business management software**, and understanding its **most important module: Accounts**.
2. **C#:** Method, Class, and Object.

> **Note:** I do not have the details of the exact software screens that were shown. The Accounts section explains how an accounts module works in most business software, with a worked example. Replace the `[ ... ]` placeholders with what you actually saw.

## Table of Contents

- [Part 1: Business Software Recap and the Accounts Module](#part-1-business-software-recap-and-the-accounts-module)
  - [Quick Recap](#1-quick-recap)
  - [Why Accounts is the Most Important Module](#2-why-accounts-is-the-most-important-module)
  - [Basic Accounting Concepts](#3-basic-accounting-concepts)
  - [Debit and Credit](#4-debit-and-credit)
  - [Worked Example: Transactions and Journal Entries](#5-worked-example-transactions-and-journal-entries)
  - [Ledger and Trial Balance](#6-ledger-and-trial-balance)
  - [How Other Modules Post to Accounts](#7-how-other-modules-post-to-accounts)
  - [Accounts Reports](#8-accounts-reports)
  - [Accounts Tables and SQL](#9-accounts-tables-and-sql)
  - [Accounts for a Farm](#10-accounts-for-a-farm)
  - [Maintaining Accounts](#11-maintaining-accounts)
  - [What I Saw in the Software](#12-what-i-saw-in-the-software)
- [Part 2: C# Method, Class, Object](#part-2-c-method-class-object)
  - [Methods](#1-methods)
  - [Classes and Objects](#2-classes-and-objects)
  - [Constructors](#3-constructors)
  - [Properties and Access Modifiers](#4-properties-and-access-modifiers)
  - [Static and Instance Members](#5-static-and-instance-members)
  - [Objects are References](#6-objects-are-references)
  - [A List of Objects](#7-a-list-of-objects)
  - [Putting It Together: Accounts in C#](#8-putting-it-together-accounts-in-c)
  - [C# Cheat Sheet](#c-cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# Part 1: Business Software Recap and the Accounts Module

## 1. Quick Recap

A business management software keeps **master data** (items, customers, suppliers) and **transactions** (purchase, sale, payment, expense, stock movement) in one database, and turns them into **reports**.

```
Master data  +  Transactions  =  Reports
```

| Module                    | Purpose                                                 |
| ------------------------- | ------------------------------------------------------- |
| Dashboard                 | Business summary                                        |
| Customers, Suppliers      | Who we sell to and buy from                             |
| Items / Products          | What we buy and sell                                    |
| Purchase                  | Buying goods                                            |
| Sales / Invoices          | Selling goods                                           |
| Inventory / Stock         | Quantity in the store                                   |
| **Accounts**              | **Money: income, expenses, payments, balances, profit** |
| HR / Payroll              | Employees and salary                                    |
| Reports                   | Summaries                                               |
| Users and Roles, Settings | Control and configuration                               |

## 2. Why Accounts is the Most Important Module

Every other module deals with **things** (items, stock, customers). The Accounts module deals with **money**, and almost every other module ends up creating an accounting entry.

| Reason                       | Explanation                                                       |
| ---------------------------- | ----------------------------------------------------------------- |
| **Shows the real result**    | Profit or loss, cash in hand, and what the business owns and owes |
| **Everything feeds into it** | Sales, purchases, payroll, and expenses all post here             |
| **Basis for decisions**      | Prices, budgets, and loans depend on the numbers                  |
| **Required by law**          | Tax and audit need correct books                                  |
| **Builds trust**             | Owners, banks, and partners rely on the reports                   |
| **Catches errors**           | Balances that do not match reveal missing or wrong entries        |

```
Sales, Purchase, Stock, Payroll, Expenses  ->  ACCOUNTS  ->  Profit and Loss, Balance Sheet, Cash Position
```

## 3. Basic Accounting Concepts

| Term                        | Meaning                                                                   |
| --------------------------- | ------------------------------------------------------------------------- |
| **Account**                 | A named record that tracks one kind of money (Cash, Sales, Rent Expense)  |
| **Chart of Accounts**       | The full list of accounts a business uses                                 |
| **Transaction**             | A money event (a sale, a payment)                                         |
| **Voucher / Journal entry** | The record of a transaction: which accounts, how much, which side         |
| **Ledger**                  | All entries of one account, with its running balance                      |
| **Trial balance**           | A list of every account's balance. Total debits must equal total credits. |
| **Cash book / Bank book**   | Ledger of cash and bank money                                             |
| **Receivable (due)**        | Money customers owe us                                                    |
| **Payable**                 | Money we owe suppliers                                                    |
| **Opening balance**         | Balance at the start (new business or new year)                           |
| **Financial year**          | The 12-month accounting period                                            |
| **Reconciliation**          | Matching our records with the bank statement or physical cash             |

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

## 4. Debit and Credit

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

## 5. Worked Example: Transactions and Journal Entries

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

## 6. Ledger and Trial Balance

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

## 7. How Other Modules Post to Accounts

| Module action                       | Accounting effect (simplified)         |
| ----------------------------------- | -------------------------------------- |
| **Purchase** on credit              | Dr Inventory, Cr Accounts Payable      |
| **Purchase** paid in cash           | Dr Inventory, Cr Cash                  |
| **Sale** on credit                  | Dr Accounts Receivable, Cr Sales       |
| **Sale** in cash                    | Dr Cash, Cr Sales                      |
| **Customer payment**                | Dr Cash / Bank, Cr Accounts Receivable |
| **Supplier payment**                | Dr Accounts Payable, Cr Cash / Bank    |
| **Expense** (rent, fuel)            | Dr Expense account, Cr Cash / Bank     |
| **Salary**                          | Dr Salary Expense, Cr Cash / Bank      |
| **Stock adjustment** (loss, damage) | Dr Loss / Expense, Cr Inventory        |

## 8. Accounts Reports

| Report                                 | What it shows                             | Why it matters                      |
| -------------------------------------- | ----------------------------------------- | ----------------------------------- |
| **Ledger**                             | All entries of one account                | Trace a balance back to its entries |
| **Cash book / Bank book**              | Money in and out                          | Know the cash position every day    |
| **Trial balance**                      | Balance of every account                  | Checks that debits equal credits    |
| **Income statement (Profit and Loss)** | Income minus expenses for a period        | Is the business making money?       |
| **Balance sheet**                      | Assets, liabilities, and equity on a date | What the business owns and owes     |
| **Receivable (due) report**            | Who owes us and how much                  | Collect money on time               |
| **Payable report**                     | Whom we owe                               | Pay on time                         |
| **Expense report**                     | Spending by category                      | Find where money goes               |

## 9. Accounts Tables and SQL

A simple accounts design:

| Table            | Columns                                             |
| ---------------- | --------------------------------------------------- |
| `Accounts`       | `AccountId`, `AccountName`, `AccountType`           |
| `JournalEntries` | `EntryId`, `EntryDate`, `Description`               |
| `JournalLines`   | `LineId`, `EntryId`, `AccountId`, `Debit`, `Credit` |

```
JournalEntries (1) ----< JournalLines (many) >---- (1) Accounts
```

Each **entry** has **two or more lines**, and the **debit total equals the credit total** for every entry.

```sql
CREATE TABLE Accounts (
    AccountId   INT PRIMARY KEY,
    AccountName VARCHAR(50) NOT NULL,
    AccountType VARCHAR(20) NOT NULL    -- Asset, Liability, Equity, Income, Expense
);

CREATE TABLE JournalEntries (
    EntryId     INT PRIMARY KEY,
    EntryDate   DATE NOT NULL,
    Description VARCHAR(100)
);

CREATE TABLE JournalLines (
    LineId    INT PRIMARY KEY,
    EntryId   INT NOT NULL REFERENCES JournalEntries(EntryId),
    AccountId INT NOT NULL REFERENCES Accounts(AccountId),
    Debit     DECIMAL(12,2) NOT NULL DEFAULT 0,
    Credit    DECIMAL(12,2) NOT NULL DEFAULT 0
);

INSERT INTO Accounts VALUES
(1, 'Cash', 'Asset'),
(2, 'Inventory', 'Asset'),
(3, 'Accounts Receivable', 'Asset'),
(4, 'Accounts Payable', 'Liability'),
(5, 'Owner''s Capital', 'Equity'),
(6, 'Sales', 'Income'),
(7, 'Labor Expense', 'Expense');

INSERT INTO JournalEntries VALUES
(1, '2026-10-01', 'Owner investment'),
(2, '2026-10-03', 'Buy seed for cash'),
(3, '2026-10-05', 'Buy fertilizer on credit'),
(4, '2026-10-10', 'Sell rice on credit'),
(5, '2026-10-15', 'Customer payment'),
(6, '2026-10-18', 'Labor wages'),
(7, '2026-10-20', 'Pay supplier');

INSERT INTO JournalLines (LineId, EntryId, AccountId, Debit, Credit) VALUES
(1,  1, 1, 100000, 0),  (2,  1, 5, 0, 100000),
(3,  2, 2, 5000, 0),    (4,  2, 1, 0, 5000),
(5,  3, 2, 8000, 0),    (6,  3, 4, 0, 8000),
(7,  4, 3, 50000, 0),   (8,  4, 6, 0, 50000),
(9,  5, 1, 30000, 0),   (10, 5, 3, 0, 30000),
(11, 6, 7, 6000, 0),    (12, 6, 1, 0, 6000),
(13, 7, 4, 8000, 0),    (14, 7, 1, 0, 8000);
```

**Trial balance**

```sql
SELECT
    a.AccountName,
    a.AccountType,
    SUM(l.Debit)  AS TotalDebit,
    SUM(l.Credit) AS TotalCredit,
    SUM(l.Debit) - SUM(l.Credit) AS Balance      -- positive = debit balance, negative = credit balance
FROM Accounts a
JOIN JournalLines l ON a.AccountId = l.AccountId
GROUP BY a.AccountName, a.AccountType;
```

**Ledger of one account with a running balance** (Cash)

```sql
SELECT
    e.EntryDate,
    e.Description,
    l.Debit,
    l.Credit,
    SUM(l.Debit - l.Credit) OVER (ORDER BY e.EntryDate, l.LineId) AS RunningBalance
FROM JournalLines l
JOIN JournalEntries e ON l.EntryId = e.EntryId
JOIN Accounts a       ON l.AccountId = a.AccountId
WHERE a.AccountName = 'Cash'
ORDER BY e.EntryDate, l.LineId;
```

The last running balance is `111000`.

**Profit and loss**

```sql
SELECT
    SUM(CASE WHEN a.AccountType = 'Income'  THEN l.Credit - l.Debit ELSE 0 END) AS TotalIncome,
    SUM(CASE WHEN a.AccountType = 'Expense' THEN l.Debit - l.Credit ELSE 0 END) AS TotalExpense,
    SUM(CASE WHEN a.AccountType = 'Income'  THEN l.Credit - l.Debit ELSE 0 END)
  - SUM(CASE WHEN a.AccountType = 'Expense' THEN l.Debit - l.Credit ELSE 0 END) AS Profit
FROM JournalLines l
JOIN Accounts a ON l.AccountId = a.AccountId;
```

| TotalIncome | TotalExpense | Profit |
| ----------- | ------------ | ------ |
| 50000       | 6000         | 44000  |

**Check that every entry balances** (no rows means all entries are correct)

```sql
SELECT EntryId, SUM(Debit) AS TotalDebit, SUM(Credit) AS TotalCredit
FROM JournalLines
GROUP BY EntryId
HAVING SUM(Debit) <> SUM(Credit);
```

**Expenses in a date range**

```sql
SELECT a.AccountName, SUM(l.Debit - l.Credit) AS Amount
FROM JournalLines l
JOIN JournalEntries e ON l.EntryId = e.EntryId
JOIN Accounts a       ON l.AccountId = a.AccountId
WHERE a.AccountType = 'Expense'
  AND e.EntryDate >= '2026-10-01'
  AND e.EntryDate <  '2026-11-01'
GROUP BY a.AccountName;
```

## 10. Accounts for a Farm

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

## 11. Maintaining Accounts

| Frequency   | Task                                                                                |
| ----------- | ----------------------------------------------------------------------------------- |
| **Daily**   | Enter every income, expense, and payment on the same day. Check the cash balance.   |
| **Weekly**  | Check dues and payables. Count the physical cash and compare.                       |
| **Monthly** | Run the trial balance. Reconcile the bank. Review profit and loss. Review expenses. |
| **Yearly**  | Closing: finalize the year, carry forward balances, and archive.                    |

| Good practice                                              | Why                                       |
| ---------------------------------------------------------- | ----------------------------------------- |
| **Correct with a reversing entry**, do not delete          | Keeps an audit trail                      |
| **Lock closed periods**                                    | Prevents changes to finished months       |
| **Use clear account names and a fixed chart of accounts**  | Reports stay consistent                   |
| **Attach a voucher number and description to every entry** | Easy to trace and audit                   |
| **Limit who can post or edit entries** (roles)             | Prevents fraud and mistakes               |
| **Back up often**                                          | Accounting data is the most critical data |

| Common mistake                     | Effect                                           |
| ---------------------------------- | ------------------------------------------------ |
| Entry on the wrong account         | Wrong reports. Fix with a correcting entry.      |
| One-sided entry                    | Trial balance does not balance                   |
| Entering transactions late         | Cash and stock numbers are wrong in the meantime |
| Deleting old entries               | History and audit trail are lost                 |
| Mixing personal and business money | Profit becomes unclear                           |

## 12. What I Saw in the Software

| Accounts feature                 | Seen? | Notes |
| -------------------------------- | ----- | ----- |
| Chart of accounts                | [ ]   |       |
| Voucher / journal entry screen   | [ ]   |       |
| Receipts and payments            | [ ]   |       |
| Expense entry                    | [ ]   |       |
| Cash book / bank book            | [ ]   |       |
| Ledger report                    | [ ]   |       |
| Trial balance                    | [ ]   |       |
| Profit and loss / balance sheet  | [ ]   |       |
| Customer due / supplier payable  | [ ]   |       |
| Opening balance and year closing | [ ]   |       |

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

## 3. Constructors

A **constructor** is a special method that **runs when an object is created**. It has the **same name as the class** and **no return type**. It is used to set the starting values.

```csharp
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Marks { get; set; }

    // Constructor
    public Student(int id, string name, decimal marks)
    {
        Id = id;
        Name = name;
        Marks = marks;
    }

    public void Display()
    {
        Console.WriteLine($"{Id} | {Name} | {Marks}");
    }
}

Student s1 = new Student(1, "Rahim", 85);     // values set while creating
s1.Display();                                  // 1 | Rahim | 85
```

- If you write **no** constructor, C# gives a default one with no parameters (`new Student()`).
- If you write **your own** constructor with parameters, the default one is **no longer** provided. Write a parameterless one too if you still need it.
- You can have **several constructors** (overloading).

```csharp
public Student() { }                                       // no parameters
public Student(string name) { Name = name; }               // one parameter
```

### The `this` keyword

`this` means **the current object**. It separates a field from a parameter with the same name.

```csharp
public Student(int id, string name)
{
    this.Id = id;        // this.Id is the property, id is the parameter
    this.Name = name;
}
```

---

## 4. Properties and Access Modifiers

### Access modifiers

| Modifier    | Who can use the member                                         |
| ----------- | -------------------------------------------------------------- |
| `public`    | **Anyone**, from anywhere                                      |
| `private`   | **Only inside the same class** (the default for class members) |
| `protected` | The class and its child classes                                |
| `internal`  | Anywhere in the same project                                   |

### Encapsulation

**Encapsulation** means **hiding the data** and allowing access only through **controlled methods or properties**. It protects the object from invalid values.

```csharp
class BankAccount
{
    public string Owner { get; }                       // read-only: set in the constructor
    public decimal Balance { get; private set; }       // anyone can read, only this class can change

    public BankAccount(string owner, decimal openingBalance = 0)
    {
        Owner = owner;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Amount must be positive.");
            return;
        }
        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
        {
            return false;                  // not allowed
        }
        Balance -= amount;
        return true;
    }
}
```

```csharp
BankAccount acc = new BankAccount("Rahim", 1000m);

acc.Deposit(500m);
Console.WriteLine(acc.Balance);              // 1500

bool ok = acc.Withdraw(2000m);
Console.WriteLine(ok);                       // False (not enough balance)
Console.WriteLine(acc.Balance);              // 1500

// acc.Balance = 999999;                     // Error: the setter is private
```

The balance can **only change through `Deposit` and `Withdraw`**, so the rules are always checked.

### Property forms

```csharp
public string Name { get; set; }                  // read and write
public string Id { get; }                         // read-only (set in the constructor)
public decimal Balance { get; private set; }      // public read, private write
public string City { get; set; } = "Dhaka";       // with a default value
public decimal Total => Price * Quantity;         // calculated property
```

---

## 5. Static and Instance Members

|                  | Instance member     | Static member                          |
| ---------------- | ------------------- | -------------------------------------- |
| Belongs to       | **Each object**     | **The class itself**                   |
| Called with      | `objectName.Member` | `ClassName.Member`                     |
| Needs an object? | Yes                 | No                                     |
| Example          | `s1.Display()`      | `Math.Sqrt(16)`, `Console.WriteLine()` |

```csharp
class BankAccount
{
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public static int TotalAccounts { get; private set; }     // shared by all objects

    public BankAccount(string owner, decimal openingBalance = 0)
    {
        Owner = owner;
        Balance = openingBalance;
        TotalAccounts++;
    }
}

BankAccount a1 = new BankAccount("Rahim", 1000m);
BankAccount a2 = new BankAccount("Karim");

Console.WriteLine(BankAccount.TotalAccounts);      // 2  (through the class name)
Console.WriteLine(a1.Balance);                      // 1000
Console.WriteLine(a2.Balance);                      // 0
```

- Each object has its **own** `Owner` and `Balance`.
- `TotalAccounts` is **one shared value** for the whole class.
- `Main` is `static`, so it can call other `static` methods directly. To use an instance method, **create an object first**.

---

## 6. Objects are References

A class is a **reference type**. A variable holds a **reference (address)** to the object, not the object itself.

```csharp
Student s1 = new Student(1, "Rahim", 85);
Student s2 = s1;           // s2 points to the SAME object

s2.Name = "Karim";

Console.WriteLine(s1.Name);      // Karim  (s1 changed too, it is the same object)
```

To get an independent object, create a **new** one:

```csharp
Student s3 = new Student(s1.Id, s1.Name, s1.Marks);    // a separate copy
s3.Name = "Nusrat";
Console.WriteLine(s1.Name);      // Karim  (unchanged)
```

|                    | Value types (`int`, `double`, `bool`) | Reference types (classes, `string`, arrays) |
| ------------------ | ------------------------------------- | ------------------------------------------- |
| Assignment `b = a` | **Copies the value**                  | **Copies the reference** (same object)      |
| Can be `null`?     | No (unless `int?`)                    | Yes                                         |

---

## 7. A List of Objects

```csharp
using System.Collections.Generic;

List<Student> students = new List<Student>
{
    new Student(1, "Rahim", 85),
    new Student(2, "Karim", 62),
    new Student(3, "Nusrat", 91)
};

foreach (Student s in students)
{
    s.Display();
}
```

**Output**

```
1 | Rahim | 85
2 | Karim | 62
3 | Nusrat | 91
```

Finding the top student:

```csharp
Student top = students[0];
foreach (Student s in students)
{
    if (s.Marks > top.Marks)
    {
        top = s;
    }
}
Console.WriteLine($"Top student: {top.Name}");     // Top student: Nusrat
```

---

## 8. Putting It Together: Accounts in C#

Connecting both parts: a **class** for a journal line, and a **method** that checks whether an entry balances (debit total equals credit total).

```csharp
using System;
using System.Collections.Generic;

class JournalLine
{
    public string Account { get; }
    public decimal Debit { get; }
    public decimal Credit { get; }

    public JournalLine(string account, decimal debit, decimal credit)
    {
        Account = account;
        Debit = debit;
        Credit = credit;
    }
}

class Program
{
    static bool IsBalanced(List<JournalLine> lines)
    {
        decimal totalDebit = 0m;
        decimal totalCredit = 0m;

        foreach (JournalLine line in lines)
        {
            totalDebit += line.Debit;
            totalCredit += line.Credit;
        }

        return totalDebit == totalCredit;
    }

    static void Main()
    {
        // Entry 3: buy fertilizer on credit
        List<JournalLine> entry = new List<JournalLine>
        {
            new JournalLine("Inventory", 8000m, 0m),
            new JournalLine("Accounts Payable", 0m, 8000m)
        };

        Console.WriteLine($"Entry balanced? {IsBalanced(entry)}");        // True

        // A wrong entry
        List<JournalLine> wrong = new List<JournalLine>
        {
            new JournalLine("Cash", 5000m, 0m),
            new JournalLine("Sales", 0m, 4000m)
        };

        Console.WriteLine($"Entry balanced? {IsBalanced(wrong)}");        // False
    }
}
```

**Output**

```
Entry balanced? True
Entry balanced? False
```

---

## C# Cheat Sheet

| Topic                | Key points                                                                                    |
| -------------------- | --------------------------------------------------------------------------------------------- |
| **Method**           | `returnType Name(parameters) { ... return value; }`. `void` returns nothing. Call it by name. |
| **Overloading**      | Same name, different parameters                                                               |
| **Class**            | A blueprint with properties (data) and methods (actions)                                      |
| **Object**           | An instance of a class, created with `new`                                                    |
| **Constructor**      | Runs on `new`. Same name as the class, no return type.                                        |
| **`this`**           | The current object                                                                            |
| **Access modifiers** | `public` (anyone), `private` (only the class)                                                 |
| **Encapsulation**    | Hide data (`private set`) and change it only through methods                                  |
| **`static`**         | Belongs to the class, used as `ClassName.Member`                                              |
| **Reference type**   | `b = a` makes both point to the same object                                                   |

---

## Practice Tasks

**Accounts**

1. List the five account types and give two examples of each for a farm.
2. Write the journal entry (debit and credit) for: selling vegetables for cash 3000, buying feed on credit 4000, and paying electricity 1200 in cash.
3. Build a trial balance for those three entries and check that it balances.
4. Create the three accounts tables, insert your entries, and run the trial balance and profit and loss queries.
5. Write a query that shows the ledger of the `Accounts Receivable` account with a running balance.
6. Find the entries in the software that created a `Sales` income and a `Cash` receipt.

**C#** 7. Write a method `IsEven(int n)` that returns `true` or `false`, and another that returns the **larger of two numbers**. 8. Write overloaded methods `Area` for a **rectangle** (two parameters) and a **circle** (one parameter). 9. Create a `Student` class with a constructor, a `GetGrade()` method, and a `Display()` method. Create three objects and display them. 10. Create a `BankAccount` class with `Deposit` and `Withdraw`, and test an invalid withdrawal. 11. Add a **static counter** that counts how many objects were created. 12. Create a `Product` class (`Name`, `Price`, `Quantity`) with a calculated `Total` property, put 5 products in a `List<Product>`, and print the **grand total**. 13. Create a `JournalLine` class and a method that checks whether a list of lines balances.

---

## Summary

- **Accounts** is the most important module, because it records the **money side** of every other module and produces the **profit, cash, and balance** reports.
- Every transaction is recorded with **double entry**: a **debit** and a **credit** of the same amount. Debits equal credits in the **trial balance**.
- **Assets and expenses** increase with debit. **Liabilities, equity, and income** increase with credit.
- Correct mistakes with a **reversing or correcting entry**, never by deleting, and keep regular **backups**.
- A **method** is a reusable block of code with parameters and an optional return value. **Overloading** allows the same name with different parameters.
- A **class** is a blueprint. An **object** is a real instance made with `new`. A **constructor** sets the starting values.
- **Encapsulation** keeps data safe: use `private set` and change values through methods.
- **`static`** members belong to the class, and instance members belong to each object. Classes are **reference types**, so `b = a` points to the same object.
