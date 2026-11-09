# C# OOP: Constructor, Value vs Reference Types, Encapsulation, Properties, this, readonly

**Date:** 9 November 2026

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
- [Cheat Sheet](#cheat-sheet)
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

## Summary

- A **class** is a blueprint, an **object** is an instance made with `new`, and a **method** is a reusable action.
- A **constructor** runs when an object is created. It can be **overloaded** and **chained** with `: this(...)`.
- **Value types** (`int`, `struct`) hold the value and are **copied** on assignment. **Reference types** (`class`, arrays) hold a **reference**, so copies share the **same object**. `string` is a reference type, but it is **immutable**.
- **Encapsulation** hides data with `private` and exposes it only through controlled **methods or properties**, so invalid values are rejected.
- **Properties** give controlled **read** (`get`) and **write** (`set`) access. Use `get;` only for read-only, `private set` for "read outside, write inside", and validation inside `set`.
- **`this`** is the current object: it separates a field from a parameter, chains constructors, and can be passed or returned.
- **`readonly`** fields can be assigned only at the declaration or in the constructor. `const` is a compile-time constant. A `readonly` reference still allows the **object's contents** to change.
