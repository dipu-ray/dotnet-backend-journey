# C# OOP: Constructor, Value vs Reference Types, Encapsulation, Properties, this, readonly

**Date:** 7 November 2026

Today's topics:

1. Recap of **class, object, and method**
2. **Constructor**, and **value type vs reference type**
3. **Encapsulation**, reading and writing **private** members, **properties**, **`this`**, and **`readonly`**

## Table of Contents

- [1. Recap: Class, Object, Method](#1-recap-class-object-method)
- [2. Constructor](#2-constructor)
- [3. Value Type vs Reference Type](#3-value-type-vs-reference-type)
- [4. Encapsulation](#4-encapsulation)
- [5. Reading and Writing Private Members](#5-reading-and-writing-private-members)
- [6. Properties](#6-properties)
- [7. The this Keyword](#7-the-this-keyword)
- [8. readonly](#8-readonly)
- [9. All Together: BankAccount](#9-all-together-bankaccount)
- [Cheat Sheet](#cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

## 1. Recap: Class, Object, Method

| Term       | Meaning                                                                        |
| ---------- | ------------------------------------------------------------------------------ |
| **Class**  | A **blueprint** that describes data (fields, properties) and actions (methods) |
| **Object** | A **real instance** created from the class with `new`                          |
| **Method** | A **named block of code** inside a class that does one task                    |

```csharp
class Car
{
    public string Brand { get; set; }       // data
    public int Speed { get; set; }

    public void Accelerate(int amount)      // method (action)
    {
        Speed += amount;
    }

    public void Show()
    {
        Console.WriteLine($"{Brand} is running at {Speed} km/h");
    }
}
```

```csharp
Car car1 = new Car();          // object 1
car1.Brand = "Toyota";
car1.Accelerate(40);
car1.Show();                   // Toyota is running at 40 km/h

Car car2 = new Car();          // object 2 (separate data)
car2.Brand = "Honda";
car2.Accelerate(60);
car2.Show();                   // Honda is running at 60 km/h
```

- `new Car()` **creates** an object.
- The **dot operator** (`car1.Speed`) reaches a member of the object.
- Each object has **its own data**. The **method code** is shared.

---

## 2. Constructor

A **constructor** is a special method that **runs automatically when an object is created**. It sets the starting values.

| Rule                | Detail                                          |
| ------------------- | ----------------------------------------------- |
| Name                | **Same as the class**                           |
| Return type         | **None** (not even `void`)                      |
| When it runs        | At `new ClassName(...)`                         |
| Default constructor | If you write none, C# gives a parameterless one |

### Parameterized constructor

```csharp
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Marks { get; set; }

    public Student(int id, string name, decimal marks)
    {
        Id = id;
        Name = name;
        Marks = marks;
    }
}

Student s1 = new Student(1, "Rahim", 85);      // values set while creating
Console.WriteLine(s1.Name);                    // Rahim
```

> Once you write a constructor with parameters, C# **no longer** provides the default `new Student()`. Write a parameterless one too if you still need it.

### Constructor overloading

Several constructors with **different parameters**.

```csharp
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Marks { get; set; }

    public Student() { Name = "Unknown"; }                       // no parameters
    public Student(string name) { Name = name; }                 // one parameter
    public Student(int id, string name, decimal marks)           // three parameters
    {
        Id = id;
        Name = name;
        Marks = marks;
    }
}

Student a = new Student();                    // Unknown
Student b = new Student("Karim");             // Karim
Student c = new Student(3, "Nusrat", 91);     // all values
```

### Constructor chaining with `this(...)`

One constructor **calls another** to avoid repeating code.

```csharp
public Student() : this(0, "Unknown", 0m) { }                  // calls the 3-parameter constructor
public Student(string name) : this(0, name, 0m) { }
public Student(int id, string name, decimal marks)
{
    Id = id;
    Name = name;
    Marks = marks;
}
```

### Object initializer (alternative)

```csharp
Student s = new Student { Id = 4, Name = "Sadia", Marks = 70 };    // needs a parameterless constructor
```

---

## 3. Value Type vs Reference Type

### Value types

The variable **holds the value itself**. Assigning copies the value.

`int`, `long`, `double`, `decimal`, `bool`, `char`, `struct`, `enum`

```csharp
int a = 10;
int b = a;         // b gets its own copy of 10
b = 20;

Console.WriteLine(a);    // 10  (unchanged)
Console.WriteLine(b);    // 20
```

### Reference types

The variable **holds a reference (address)** to the object. Assigning copies the **reference**, so both variables point to the **same object**.

`class`, `string`, arrays, `object`, `delegate`, `interface`

```csharp
Student s1 = new Student(1, "Rahim", 85);
Student s2 = s1;           // s2 points to the SAME object

s2.Name = "Karim";

Console.WriteLine(s1.Name);    // Karim  (s1 changed too)
```

```
Value type:       a [10]      b [20]            (two separate values)

Reference type:   s1 ──┐
                       ├──>  [ Student object: Name = "Karim" ]
                  s2 ──┘
```

To get an **independent copy**, create a **new object**:

```csharp
Student s3 = new Student(s1.Id, s1.Name, s1.Marks);    // a separate copy
s3.Name = "Nusrat";
Console.WriteLine(s1.Name);    // Karim  (unchanged)
```

### `struct` (value type) vs `class` (reference type)

```csharp
struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y) { X = x; Y = y; }
}

Point p1 = new Point(1, 2);
Point p2 = p1;          // copy
p2.X = 10;

Console.WriteLine(p1.X);    // 1   (unchanged, p2 is a copy)
Console.WriteLine(p2.X);    // 10
```

If `Point` were a `class`, `p1.X` would also become `10`.

### Passing to methods

| Case                                         | What the method receives                  | Effect on the caller's variable             |
| -------------------------------------------- | ----------------------------------------- | ------------------------------------------- |
| Value type                                   | A **copy** of the value                   | Not changed                                 |
| Reference type                               | A **copy of the reference** (same object) | The **object's data** can be changed        |
| Reference type, reassigned inside the method | A copy of the reference                   | The caller's variable is **not** re-pointed |
| `ref` parameter                              | The variable itself                       | The caller's variable **is** changed        |

```csharp
static void ChangeNumber(int n)            { n = 99; }
static void ChangeName(Student s)          { s.Name = "Changed"; }
static void Replace(Student s)             { s = new Student(9, "New", 0); }
static void ReplaceWithRef(ref Student s)  { s = new Student(9, "New", 0); }

int number = 5;
ChangeNumber(number);
Console.WriteLine(number);          // 5      (copy, unchanged)

Student st = new Student(1, "Rahim", 85);
ChangeName(st);
Console.WriteLine(st.Name);         // Changed (same object, data changed)

Replace(st);
Console.WriteLine(st.Name);         // Changed (reassigning inside did not affect st)

ReplaceWithRef(ref st);
Console.WriteLine(st.Name);         // New     (ref changed the caller's variable)
```

### `string` is a reference type, but behaves like a value

Strings are **immutable**: an operation creates a **new** string and never changes the old one.

```csharp
string a = "Hi";
string b = a;
b += " there";

Console.WriteLine(a);    // Hi
Console.WriteLine(b);    // Hi there
```

### Equality

```csharp
Student x = new Student(1, "Rahim", 85);
Student y = new Student(1, "Rahim", 85);
Student z = x;

Console.WriteLine(x == y);    // False (different objects, even with the same data)
Console.WriteLine(x == z);    // True  (the same object)

Console.WriteLine("abc" == "abc");    // True (string compares the content)
```

### `null`

```csharp
Student s = null;        // reference types can be null (no object)
// int n = null;         // Error: value types cannot be null
int? n = null;           // nullable value type: allowed
```

### Comparison table

|                    | Value type                                     | Reference type                                 |
| ------------------ | ---------------------------------------------- | ---------------------------------------------- |
| Examples           | `int`, `double`, `bool`, `struct`              | `class`, `string`, arrays                      |
| Variable holds     | The **value**                                  | A **reference** to the object                  |
| Assignment `b = a` | Copies the value                               | Copies the reference (**same object**)         |
| Can be `null`?     | No (unless `int?`)                             | Yes                                            |
| Where it lives     | Usually on the **stack** (for local variables) | Object on the **heap**, reference on the stack |
| Passed to a method | Copy of the value                              | Copy of the reference                          |

---

## 4. Encapsulation

**Encapsulation** means **bundling data and the methods that use it in one class, and hiding the data** so it can be changed **only in controlled ways**.

**Why it matters**

- **Protects data** from invalid values
- Lets you **add rules** (validation) in one place
- You can **change the inside** of a class without breaking the code that uses it
- Makes the class **easier to use**: other code sees only what it needs

### Without encapsulation (problem)

```csharp
class BadAccount
{
    public decimal Balance;       // anyone can change this
}

BadAccount acc = new BadAccount();
acc.Balance = -5000;              // invalid, but nothing stops it
```

### With encapsulation (solution)

```csharp
class GoodAccount
{
    private decimal balance;      // hidden

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
        }
    }

    public decimal GetBalance()
    {
        return balance;
    }
}

GoodAccount acc = new GoodAccount();
acc.Deposit(1000m);
// acc.balance = -5000;                    // Error: 'balance' is private
Console.WriteLine(acc.GetBalance());       // 1000
```

### Access modifiers

| Modifier    | Who can use the member                                     |
| ----------- | ---------------------------------------------------------- |
| `public`    | Any code, anywhere                                         |
| `private`   | **Only inside the same class** (default for class members) |
| `protected` | The class and its **child classes**                        |
| `internal`  | Any code in the **same project**                           |

**Rule of thumb:** make data **`private`**, and expose only what is needed through **`public` methods or properties**.

---

## 5. Reading and Writing Private Members

A private field cannot be reached from outside, so the class provides **controlled ways** to **read** and **write** it.

### With methods (getter and setter methods)

```csharp
class Student
{
    private string name;
    private decimal marks;

    // READ access
    public string GetName()
    {
        return name;
    }

    // WRITE access (with validation)
    public void SetName(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName))
        {
            name = newName;
        }
    }

    public decimal GetMarks()
    {
        return marks;
    }

    public void SetMarks(decimal newMarks)
    {
        if (newMarks >= 0 && newMarks <= 100)
        {
            marks = newMarks;
        }
    }
}

Student s = new Student();
s.SetName("Rahim");
s.SetMarks(85);
s.SetMarks(150);                       // ignored: invalid

Console.WriteLine(s.GetName());        // Rahim
Console.WriteLine(s.GetMarks());       // 85
```

This works, but needs **two methods per field**. C# provides **properties** to do the same more cleanly (next section).

### Read and write control: the options

| Goal                                               | How                                                         |
| -------------------------------------------------- | ----------------------------------------------------------- |
| **Read and write** from outside                    | `public` property with `get; set;`                          |
| **Read only** from outside                         | `public` property with `get;` only (set in the constructor) |
| **Read from outside, write only inside the class** | `public ... { get; private set; }`                          |
| **No access from outside**                         | `private` field                                             |
| **Write once** (in the constructor)                | `readonly` field or `get;`-only property                    |

---

## 6. Properties

A **property** is a member that **looks like a field** but **runs code** when it is read (`get`) or written (`set`). It gives **controlled access** to a private field.

### Full property with a backing field

```csharp
class Student
{
    private string _name;            // backing field (private)

    public string Name               // property (public)
    {
        get { return _name; }        // runs when you READ  s.Name
        set                          // runs when you WRITE s.Name = ...
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty.");
            }
            _name = value;           // 'value' is the value being assigned
        }
    }
}

Student s = new Student();
s.Name = "Rahim";                    // set
Console.WriteLine(s.Name);           // get: Rahim
// s.Name = "";                      // throws ArgumentException
```

### Auto-implemented property

When **no extra logic** is needed, C# creates the backing field for you.

```csharp
public int Id { get; set; }
public string City { get; set; } = "Dhaka";      // with a default value
```

### Property forms

| Form                 | Syntax                                         | Read from outside | Write from outside               |
| -------------------- | ---------------------------------------------- | ----------------- | -------------------------------- |
| Read and write       | `public string Name { get; set; }`             | Yes               | Yes                              |
| **Read only**        | `public string Id { get; }`                    | Yes               | **No** (only in the constructor) |
| **Private set**      | `public decimal Balance { get; private set; }` | Yes               | **No** (only inside the class)   |
| **Init only** (C# 9) | `public string Code { get; init; }`            | Yes               | Only when creating the object    |
| **Calculated**       | `public decimal Total => Price * Quantity;`    | Yes               | No                               |

```csharp
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public decimal Total => Price * Quantity;         // calculated, no storage
}

Product p = new Product { Name = "Pen", Price = 10m, Quantity = 5 };
Console.WriteLine(p.Total);          // 50
```

### Validation in a property

```csharp
class Student
{
    private decimal _marks;

    public decimal Marks
    {
        get { return _marks; }
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Marks must be between 0 and 100.");
            }
            _marks = value;
        }
    }
}
```

### Property vs field

|                                        | Field                | Property                          |
| -------------------------------------- | -------------------- | --------------------------------- |
| Logic when reading or writing          | No                   | **Yes** (validation, calculation) |
| Read-only / private set                | Only with `readonly` | **Yes**                           |
| Used in data binding and serialization | Usually not          | **Yes**                           |
| Recommended to be `public`?            | **No**               | **Yes**                           |

---

## 7. The this Keyword

`this` refers to **the current object**, the one whose method or constructor is running.

### 1. Separate a field from a parameter with the same name

```csharp
class Student
{
    private string name;
    private decimal marks;

    public Student(string name, decimal marks)
    {
        this.name = name;        // this.name = the field, name = the parameter
        this.marks = marks;
    }
}
```

Without `this`, `name = name;` would assign the parameter to itself and the field would stay empty.

### 2. Call another constructor of the same class

```csharp
public Student() : this("Unknown", 0m) { }
```

### 3. Pass the current object to another method

```csharp
class Logger
{
    public static void Print(Student s) => Console.WriteLine($"Logging {s.Name}");
}

class Student
{
    public string Name { get; set; }

    public void Register()
    {
        Logger.Print(this);        // passes the current Student
    }
}
```

### 4. Return the current object (method chaining)

```csharp
class Calculator
{
    public int Value { get; private set; }

    public Calculator Add(int n)      { Value += n; return this; }
    public Calculator Multiply(int n) { Value *= n; return this; }
}

Calculator c = new Calculator();
c.Add(5).Multiply(3).Add(1);
Console.WriteLine(c.Value);           // 16   ((0 + 5) * 3 + 1)
```

> `static` methods belong to the class, not to an object, so **`this` cannot be used in them**.

---

## 8. readonly

A **`readonly` field** can be assigned **only**:

1. at the **declaration**, or
2. inside a **constructor** of the same class.

After that, it **cannot change**.

```csharp
class Account
{
    public readonly string AccountNumber;                  // set in the constructor
    public readonly DateTime CreatedAt = DateTime.Now;     // set at the declaration

    public Account(string accountNumber)
    {
        AccountNumber = accountNumber;                     // allowed here
    }

    public void Change()
    {
        // AccountNumber = "X";      // Error: cannot assign a readonly field outside a constructor
    }
}

Account a = new Account("AC-1001");
// a.AccountNumber = "AC-9999";      // Error
Console.WriteLine(a.AccountNumber);   // AC-1001
```

### `readonly` vs `const`

|                             | `const`                          | `readonly`                                     |
| --------------------------- | -------------------------------- | ---------------------------------------------- |
| Value known at              | **Compile time**                 | **Run time** (can be set in the constructor)   |
| Assigned                    | **Only at the declaration**      | At the declaration **or in the constructor**   |
| Can differ between objects? | **No** (one value for the class) | **Yes** (each object can have its own)         |
| Allowed types               | Simple types and `string`        | **Any type**                                   |
| `static`?                   | Implicitly static                | Instance by default (can be `static readonly`) |

```csharp
public const double Pi = 3.14159;                        // same for everyone, fixed at compile time
public readonly string Id;                               // can differ for each object
public static readonly DateTime StartTime = DateTime.Now; // one value for the class, set at run time
```

### `readonly` with reference types

`readonly` stops the **variable** from pointing to another object, but the **object's contents can still change**.

```csharp
class Team
{
    private readonly List<string> _members = new List<string>();

    public void Add(string name)
    {
        _members.Add(name);              // allowed: changing the contents
        // _members = new List<string>();   // Error: cannot reassign a readonly field
    }

    public int Count => _members.Count;
}
```

### `readonly` field vs get-only property

|                        | `readonly` field           | `{ get; }` property        |
| ---------------------- | -------------------------- | -------------------------- |
| Set                    | Declaration or constructor | Declaration or constructor |
| Recommended visibility | Usually `private`          | Usually `public`           |
| Can add logic later    | No                         | Yes                        |

```csharp
private readonly int _id;                // internal fixed value
public string Code { get; }              // public read-only value
```

---

## 9. All Together: BankAccount

```csharp
using System;

class BankAccount
{
    // readonly: set once in the constructor, never changed
    public readonly string AccountNumber;

    // private backing field with a validated property
    private string _owner;
    public string Owner
    {
        get { return _owner; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Owner name cannot be empty.");
            }
            _owner = value;
        }
    }

    // readable from outside, changed only inside the class
    public decimal Balance { get; private set; }

    // constructor
    public BankAccount(string accountNumber, string owner, decimal openingBalance)
    {
        this.AccountNumber = accountNumber;     // this: current object
        this.Owner = owner;                     // goes through the property, so validation runs
        this.Balance = openingBalance;
    }

    // constructor chaining
    public BankAccount(string accountNumber, string owner)
        : this(accountNumber, owner, 0m) { }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit must be positive.");
            return;
        }
        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
        {
            return false;
        }
        Balance -= amount;
        return true;
    }

    public void Display()
    {
        Console.WriteLine($"{AccountNumber} | {Owner} | {Balance}");
    }
}

class Program
{
    static void Main()
    {
        BankAccount a = new BankAccount("AC-1001", "Rahim", 1000m);
        a.Deposit(500m);
        bool ok = a.Withdraw(2000m);
        Console.WriteLine($"Withdraw 2000: {ok}");
        a.Display();

        BankAccount b = new BankAccount("AC-1002", "Karim");
        b.Display();

        // a.Balance = 999999;           // Error: the setter is private
        // a.AccountNumber = "AC-0";     // Error: readonly field

        try
        {
            a.Owner = "";                // validation in the property
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Reference type behavior
        BankAccount c = a;               // c points to the same object as a
        c.Deposit(100m);
        a.Display();
    }
}
```

**Output**

```
Withdraw 2000: False
AC-1001 | Rahim | 1500
AC-1002 | Karim | 0
Owner name cannot be empty.
AC-1001 | Rahim | 1600
```

**What each concept does here**

| Concept                                | Where                                                                   |
| -------------------------------------- | ----------------------------------------------------------------------- |
| **Class, object, method**              | `BankAccount`, `new BankAccount(...)`, `Deposit`, `Withdraw`, `Display` |
| **Constructor, overloading, chaining** | Two constructors, the second calls `: this(...)`                        |
| **Encapsulation**                      | `_owner` is private. `Balance` can be changed only by the class         |
| **Property with validation**           | `Owner` rejects empty names                                             |
| **Private set**                        | `Balance { get; private set; }`                                         |
| **`this`**                             | `this.AccountNumber = accountNumber;` and constructor chaining          |
| **`readonly`**                         | `AccountNumber` cannot change after the constructor                     |
| **Reference type**                     | `c = a` makes both variables point to one account                       |

---

## Cheat Sheet

| Topic                       | Key points                                                                                              |
| --------------------------- | ------------------------------------------------------------------------------------------------------- |
| **Class / Object / Method** | Blueprint / instance made with `new` / named block of code                                              |
| **Constructor**             | Same name as the class, no return type, runs on `new`. Can be overloaded and chained with `: this(...)` |
| **Value type**              | Holds the value. Copy on assignment. `int`, `double`, `bool`, `struct`                                  |
| **Reference type**          | Holds a reference. Assignment shares the same object. `class`, `string`, arrays                         |
| **Encapsulation**           | Hide data (`private`), expose controlled access (`public` methods or properties)                        |
| **Access modifiers**        | `public`, `private`, `protected`, `internal`                                                            |
| **Property**                | `get` / `set` with a backing field. Forms: auto, read-only, `private set`, `init`, calculated           |
| **`this`**                  | The current object. Separates field and parameter, chains constructors, returns itself                  |
| **`readonly`**              | Assigned only at the declaration or in the constructor                                                  |
| **`const`**                 | Compile-time constant, assigned only at the declaration                                                 |

---

## Practice Tasks

1. Create a `Book` class with `Title`, `Author`, and `Price`. Add a constructor, and create two objects.
2. Add **constructor overloading** to `Book` (no parameters, title only, all values), and use **constructor chaining**.
3. Show the difference between a **value type** and a **reference type** by copying an `int` and a `Book`, then changing the copy.
4. Write a `struct Point` and a `class Person`. Copy each one, change the copy, and explain the output.
5. Write methods `ChangeNumber(int n)`, `ChangeBook(Book b)`, and `ReplaceBook(Book b)`, and predict what the caller sees. Then add a `ref` version.
6. Make a class with a **private** field and add **`GetX()` and `SetX()`** methods with validation. Then replace them with a **property**.
7. Create a `Student` class where `Marks` is a property that accepts only values from `0` to `100`.
8. Make a property with **`private set`**, and show that it cannot be changed from outside the class.
9. Add a **`readonly`** `Id` field assigned in the constructor, and try to change it in a method. Read the compiler error.
10. Write a **calculated** property `Total` (`Price * Quantity`).
11. Use **`this`** in a constructor, and write a method that returns `this` so calls can be chained.
12. Build a `BankAccount` class with `Deposit`, `Withdraw`, a read-only `AccountNumber`, and a balance that only the class can change.

---

## Summary

- A **class** is a blueprint, an **object** is an instance made with `new`, and a **method** is a reusable action.
- A **constructor** runs when an object is created. It can be **overloaded** and **chained** with `: this(...)`.
- **Value types** (`int`, `struct`) hold the value and are **copied** on assignment. **Reference types** (`class`, arrays) hold a **reference**, so copies share the **same object**. `string` is a reference type, but it is **immutable**.
- **Encapsulation** hides data with `private` and exposes it only through controlled **methods or properties**, so invalid values are rejected.
- **Properties** give controlled **read** (`get`) and **write** (`set`) access. Use `get;` only for read-only, `private set` for "read outside, write inside", and validation inside `set`.
- **`this`** is the current object: it separates a field from a parameter, chains constructors, and can be passed or returned.
- **`readonly`** fields can be assigned only at the declaration or in the constructor. `const` is a compile-time constant. A `readonly` reference still allows the **object's contents** to change.
