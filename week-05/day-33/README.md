# C# OOP: Inheritance, Polymorphism, and Abstraction

**Date:** 10 November 2026

Today's topics (learning stage): **Inheritance**, **Polymorphism**, and **Abstraction**. Practice and problem solving are planned for the next day.

## Table of Contents

- [Quick Overview](#quick-overview)
- [1. Inheritance](#1-inheritance)
- [2. Polymorphism](#2-polymorphism)
- [3. Abstraction](#3-abstraction)
- [All Together: Employee Payroll](#all-together-employee-payroll)
- [Comparison and Cheat Sheet](#comparison-and-cheat-sheet)
- [Practice and Problem Solving Plan](#practice-and-problem-solving-plan)
- [Summary](#summary)

---

## Quick Overview

| Concept          | One-line meaning                                              | Key C# words                       |
| ---------------- | ------------------------------------------------------------- | ---------------------------------- |
| **Inheritance**  | A class **reuses** the members of another class               | `:`, `base`, `protected`, `sealed` |
| **Polymorphism** | The **same call behaves differently** depending on the object | `virtual`, `override`, overloading |
| **Abstraction**  | **Hide the details**, show only what is needed                | `abstract`, `interface`            |

```
Inheritance   ->  build new classes from existing ones          (reuse)
Polymorphism  ->  one method name, many behaviors               (flexibility)
Abstraction   ->  define WHAT must be done, not HOW             (simplicity)
```

---

# 1. Inheritance

**Inheritance** lets a class **take the fields, properties, and methods of another class**, so the code is written once and reused.

| Term              | Other names              |
| ----------------- | ------------------------ |
| **Base class**    | Parent class, superclass |
| **Derived class** | Child class, subclass    |

Inheritance models an **"is-a"** relationship: a `Dog` **is an** `Animal`.

### Syntax

```csharp
class DerivedClass : BaseClass
{
    // extra members
}
```

### Example

```csharp
class Animal                                   // base class
{
    public string Name { get; set; }

    public void Eat()
    {
        Console.WriteLine($"{Name} is eating.");
    }
}

class Dog : Animal                             // derived class
{
    public void Bark()
    {
        Console.WriteLine($"{Name} says Woof!");
    }
}

Dog d = new Dog { Name = "Tommy" };
d.Eat();        // inherited from Animal
d.Bark();       // defined in Dog
```

**Output**

```
Tommy is eating.
Tommy says Woof!
```

`Dog` did not write `Name` or `Eat()`, but it **has** them because it inherits from `Animal`.

### Access and inheritance

| Modifier    | Accessible in the derived class?           |
| ----------- | ------------------------------------------ |
| `public`    | Yes                                        |
| `protected` | **Yes** (and not from outside code)        |
| `private`   | **No** (only inside the base class itself) |

```csharp
class Account
{
    protected decimal balance;                 // visible to child classes
}

class SavingsAccount : Account
{
    public void AddInterest()
    {
        balance += balance * 0.05m;            // allowed: protected
    }
}
```

### Constructors and `base`

Constructors are **not inherited**. A derived constructor calls the base constructor with **`base(...)`**.

```csharp
class Person
{
    public string Name { get; set; }

    public Person(string name)
    {
        Name = name;
    }
}

class Student : Person
{
    public int Id { get; }

    public Student(string name, int id) : base(name)      // calls Person(name)
    {
        Id = id;
    }
}

Student s = new Student("Rahim", 101);
Console.WriteLine($"{s.Name}, {s.Id}");      // Rahim, 101
```

**Order of constructors:** the **base** constructor runs **first**, then the derived one.

```csharp
class A { public A() { Console.WriteLine("A constructor"); } }
class B : A { public B() { Console.WriteLine("B constructor"); } }

new B();
// A constructor
// B constructor
```

### The `base` keyword

`base` refers to the **parent class**. It is used to call the base constructor and to reach base members.

```csharp
base.MethodName();       // call the parent's version of a method
```

### Kinds of inheritance

| Kind                 | Shape                         | Supported in C#?                        |
| -------------------- | ----------------------------- | --------------------------------------- |
| **Single**           | `B : A`                       | Yes                                     |
| **Multilevel**       | `C : B`, `B : A`              | Yes                                     |
| **Hierarchical**     | `B : A` and `C : A`           | Yes                                     |
| **Multiple classes** | `C : A, B` (two base classes) | **No** (a class has **one** base class) |

> A class can implement **many interfaces** (see Abstraction), which is how C# gets multiple-inheritance-like behavior.

**Multilevel example**

```csharp
class Animal { public void Eat() { Console.WriteLine("Eating"); } }
class Dog : Animal { public void Bark() { Console.WriteLine("Barking"); } }
class Puppy : Dog { public void Weep() { Console.WriteLine("Weeping"); } }

Puppy p = new Puppy();
p.Eat();       // from Animal
p.Bark();      // from Dog
p.Weep();      // from Puppy
```

### `sealed`: stop inheritance

```csharp
sealed class FinalClass { }
// class Other : FinalClass { }     // Error: cannot inherit from a sealed class
```

### Good to know

- Every class in C# inherits from **`object`**, so every object has methods such as `ToString()`.
- Use inheritance for an **"is-a"** relationship (`Dog` is an `Animal`). For a **"has-a"** relationship (a `Car` has an `Engine`), use a **field or property** instead.
- Inheritance **reuses code** and lets you keep common members in one place.

---

# 2. Polymorphism

**Polymorphism** means **"many forms"**: the **same method name** can behave **differently** depending on the object that runs it.

| Type                      | When decided              | How                                              |
| ------------------------- | ------------------------- | ------------------------------------------------ |
| **Compile-time** (static) | When the code is compiled | **Method overloading**                           |
| **Run-time** (dynamic)    | When the program runs     | **Method overriding** (`virtual` and `override`) |

## 2.1 Compile-time polymorphism: method overloading

Several methods with the **same name** but **different parameters**.

```csharp
class Printer
{
    public void Print(int number)      { Console.WriteLine("Number: " + number); }
    public void Print(string text)     { Console.WriteLine("Text: " + text); }
    public void Print(double value)    { Console.WriteLine("Decimal: " + value); }
}

Printer p = new Printer();
p.Print(10);          // Number: 10
p.Print("Hello");     // Text: Hello
p.Print(3.5);         // Decimal: 3.5
```

The compiler chooses the method from the **arguments**. A different **return type alone** is not enough to overload.

## 2.2 Run-time polymorphism: method overriding

The **base class** marks a method **`virtual`**. The **derived class** changes it with **`override`**.

```csharp
class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine("Some sound");
    }
}

class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Woof!");
    }
}

class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Meow!");
    }
}
```

### The polymorphism in action

A **base class variable** can hold an object of **any derived class**. The method that runs is chosen by the **real object**, not by the variable type.

```csharp
Animal[] animals = { new Animal(), new Dog(), new Cat() };

foreach (Animal a in animals)
{
    a.Speak();
}
```

**Output**

```
Some sound
Woof!
Meow!
```

The loop calls the **same** `a.Speak()` each time, but the result is **different** for each object.

```csharp
Animal pet = new Dog();     // base type variable, Dog object (upcasting)
pet.Speak();                // Woof!
```

### Calling the base version with `base`

```csharp
class Dog : Animal
{
    public override void Speak()
    {
        base.Speak();                    // Some sound
        Console.WriteLine("Woof!");
    }
}
```

### Override `ToString()`

`ToString()` comes from `object` and can be overridden. `Console.WriteLine` calls it automatically.

```csharp
class Student
{
    public string Name { get; set; }
    public int Marks { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Marks})";
    }
}

Student s = new Student { Name = "Rahim", Marks = 85 };
Console.WriteLine(s);            // Rahim (85)
```

### `override` vs `new` (method hiding)

```csharp
class Base
{
    public virtual void Show() { Console.WriteLine("Base"); }
    public void Hello()        { Console.WriteLine("Base Hello"); }
}

class Derived : Base
{
    public override void Show() { Console.WriteLine("Derived"); }      // overrides
    public new void Hello()     { Console.WriteLine("Derived Hello"); } // hides
}

Base b = new Derived();
b.Show();        // Derived       (override: the real object decides)
b.Hello();       // Base Hello    (new: the variable type decides)
```

|                                | `override`                         | `new` (hiding)   |
| ------------------------------ | ---------------------------------- | ---------------- |
| Needs `virtual` in the base?   | **Yes**                            | No               |
| Called through a base variable | **Derived** version (polymorphism) | **Base** version |
| Real polymorphism?             | **Yes**                            | No               |

### Rules for overriding

- The base method must be **`virtual`** (or `abstract`, or already `override`).
- The derived method must have the **same name, parameters, and return type**, and use **`override`**.
- A method that is not `virtual` **cannot** be overridden.

|                  | Overloading                 | Overriding                   |
| ---------------- | --------------------------- | ---------------------------- |
| Where            | **Same class** (or derived) | **Base and derived** classes |
| Method signature | **Different** parameters    | **Same** parameters          |
| Decided          | Compile time                | **Run time**                 |
| Keywords         | None                        | `virtual` and `override`     |

---

# 3. Abstraction

**Abstraction** means **hiding the complex details** and showing only **what is necessary**. The user knows **what** an object does, not **how** it does it.

Real-life example: when you drive a car, you use the **steering wheel and pedals**. You do not need to know how the engine works inside.

In C#, abstraction is done with **abstract classes** and **interfaces**.

## 3.1 Abstract class

An **abstract class**:

- is declared with the **`abstract`** keyword,
- **cannot be instantiated** (no `new` on it),
- may contain **abstract methods** (no body, **must** be implemented by the child class) and **normal methods** (with a body),
- is used as a **base class**.

```csharp
abstract class Shape
{
    public string Name { get; }

    protected Shape(string name)
    {
        Name = name;
    }

    public abstract double CalculateArea();            // no body: children must implement it

    public void Display()                              // normal method: shared by all shapes
    {
        Console.WriteLine($"{Name}: Area = {CalculateArea():F2}");
    }
}

class Circle : Shape
{
    private double radius;

    public Circle(double radius) : base("Circle")
    {
        this.radius = radius;
    }

    public override double CalculateArea()
    {
        return Math.PI * radius * radius;
    }
}

class Rectangle : Shape
{
    private double width;
    private double height;

    public Rectangle(double width, double height) : base("Rectangle")
    {
        this.width = width;
        this.height = height;
    }

    public override double CalculateArea()
    {
        return width * height;
    }
}
```

```csharp
// Shape s = new Shape("x");        // Error: cannot create an instance of an abstract class

List<Shape> shapes = new List<Shape>
{
    new Circle(5),
    new Rectangle(4, 6)
};

foreach (Shape s in shapes)
{
    s.Display();
}
```

**Output**

```
Circle: Area = 78.54
Rectangle: Area = 24.00
```

**What happens**

- `Shape` says **what** every shape must do (`CalculateArea`) but not **how**.
- `Circle` and `Rectangle` give **their own** calculation.
- `Display()` is written **once** in `Shape` and works for every shape.
- This also uses **polymorphism**: `s.Display()` calls the right `CalculateArea()` for each object.

**Abstract class rules**

| Rule             | Detail                                                           |
| ---------------- | ---------------------------------------------------------------- |
| Instantiation    | Not allowed                                                      |
| Abstract method  | No body, ends with `;`, and must be implemented with `override`  |
| Normal members   | Allowed (fields, properties, constructors, methods with a body)  |
| Child class      | Must implement **all** abstract methods, or be `abstract` itself |
| Abstract members | Must be `public`, `protected`, or `internal` (not `private`)     |

## 3.2 Interface

An **interface** is a **contract**: it lists the members a class **must** provide, with no implementation.

- Declared with **`interface`**. Names usually start with **`I`** (`INotifier`).
- Members are **public** by default.
- A class **implements** an interface with `:` and must implement **every** member.
- A class can implement **many interfaces**.

```csharp
interface INotifier
{
    void Send(string message);
}

class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}
```

```csharp
INotifier n1 = new EmailNotifier();
INotifier n2 = new SmsNotifier();

n1.Send("Hello");     // Email sent: Hello
n2.Send("Hello");     // SMS sent: Hello
```

### Why this is abstraction

A class that **uses** the interface does not need to know **how** it works.

```csharp
class Order
{
    private readonly INotifier _notifier;

    public Order(INotifier notifier)
    {
        _notifier = notifier;
    }

    public void Place()
    {
        Console.WriteLine("Order placed.");
        _notifier.Send("Your order is confirmed.");
    }
}

new Order(new EmailNotifier()).Place();
// Order placed.
// Email sent: Your order is confirmed.

new Order(new SmsNotifier()).Place();
// Order placed.
// SMS sent: Your order is confirmed.
```

`Order` works with **any** `INotifier`. The way of sending can change without changing `Order`.

### Multiple interfaces

```csharp
interface IPrintable { void Print(); }
interface ISaveable  { void Save(); }

class Document : IPrintable, ISaveable
{
    public void Print() { Console.WriteLine("Printing..."); }
    public void Save()  { Console.WriteLine("Saving..."); }
}
```

## 3.3 Abstract class vs interface

|               | Abstract class                                         | Interface                                                               |
| ------------- | ------------------------------------------------------ | ----------------------------------------------------------------------- |
| Purpose       | A **base class** with shared code and required methods | A **contract** of what a class can do                                   |
| Instantiate   | No                                                     | No                                                                      |
| Fields        | **Yes**                                                | No                                                                      |
| Constructors  | **Yes**                                                | No                                                                      |
| Method bodies | Yes (normal methods) and no (abstract methods)         | Traditionally **no** (since C# 8, default implementations are possible) |
| Inheritance   | A class can inherit **only one**                       | A class can implement **many**                                          |
| Relationship  | **"is-a"** (`Circle` is a `Shape`)                     | **"can-do"** (`Document` can be printed)                                |
| Keywords      | `abstract` and `override`                              | `interface`                                                             |

**When to use which**

- **Abstract class:** related classes that **share code** and a common identity (`Shape`, `Employee`).
- **Interface:** **unrelated** classes that need the **same ability** (`IPrintable`, `INotifier`), or when a class needs **several** contracts.

---

# All Together: Employee Payroll

All three concepts in one program.

```csharp
using System;
using System.Collections.Generic;

// ABSTRACTION: the base class defines WHAT every employee must provide
abstract class Employee
{
    public string Name { get; }

    protected Employee(string name)
    {
        Name = name;
    }

    public abstract decimal CalculateSalary();

    public void Display()
    {
        Console.WriteLine($"{Name} | {GetType().Name} | Salary: {CalculateSalary()}");
    }
}

// INHERITANCE: both classes inherit from Employee
class FullTimeEmployee : Employee
{
    private readonly decimal _monthlySalary;

    public FullTimeEmployee(string name, decimal monthlySalary) : base(name)
    {
        _monthlySalary = monthlySalary;
    }

    // POLYMORPHISM: each class calculates salary in its own way
    public override decimal CalculateSalary()
    {
        return _monthlySalary;
    }
}

class PartTimeEmployee : Employee
{
    private readonly decimal _hourlyRate;
    private readonly int _hoursWorked;

    public PartTimeEmployee(string name, decimal hourlyRate, int hoursWorked) : base(name)
    {
        _hourlyRate = hourlyRate;
        _hoursWorked = hoursWorked;
    }

    public override decimal CalculateSalary()
    {
        return _hourlyRate * _hoursWorked;
    }
}

class Program
{
    static void Main()
    {
        List<Employee> employees = new List<Employee>
        {
            new FullTimeEmployee("Rahim", 50000m),
            new PartTimeEmployee("Karim", 500m, 80)
        };

        decimal total = 0m;

        foreach (Employee e in employees)
        {
            e.Display();                       // same call, different behavior
            total += e.CalculateSalary();
        }

        Console.WriteLine($"Total payroll: {total}");
    }
}
```

**Output**

```
Rahim | FullTimeEmployee | Salary: 50000
Karim | PartTimeEmployee | Salary: 40000
Total payroll: 90000
```

| Concept          | Where it appears                                                                                        |
| ---------------- | ------------------------------------------------------------------------------------------------------- |
| **Abstraction**  | `abstract class Employee` with `abstract CalculateSalary()`                                             |
| **Inheritance**  | `FullTimeEmployee : Employee`, `PartTimeEmployee : Employee`, and `: base(name)`                        |
| **Polymorphism** | `e.CalculateSalary()` and `e.Display()` give a different result for each object in the `List<Employee>` |

Adding a new employee type (for example `ContractEmployee`) needs only a **new class**. The loop in `Main` does **not** change.

---

## Comparison and Cheat Sheet

|                         | Inheritance                      | Polymorphism                        | Abstraction                    |
| ----------------------- | -------------------------------- | ----------------------------------- | ------------------------------ |
| **Idea**                | Reuse a parent's members         | One name, many behaviors            | Hide details, show essentials  |
| **Question it answers** | "What do I get from the parent?" | "Which version runs?"               | "What must be provided?"       |
| **Syntax**              | `class B : A`                    | `virtual` / `override`, overloading | `abstract`, `interface`        |
| **Relationship**        | "is-a"                           | Same call, different objects        | "what" without "how"           |
| **Benefit**             | Less repeated code               | Flexible, extendable code           | Simple, loosely coupled design |

| Keyword     | Meaning                                                                               |
| ----------- | ------------------------------------------------------------------------------------- |
| `:`         | Inherit from a class or implement an interface                                        |
| `base`      | Refer to the parent class (constructor or members)                                    |
| `protected` | Visible to the class and its children                                                 |
| `sealed`    | The class cannot be inherited                                                         |
| `virtual`   | The method **can** be overridden                                                      |
| `override`  | Replaces a `virtual` or `abstract` method                                             |
| `new`       | Hides a base member (not polymorphism)                                                |
| `abstract`  | A class that cannot be instantiated, or a method with no body that must be overridden |
| `interface` | A contract of members to implement                                                    |

---

## Practice and Problem Solving Plan

Planned for the next day.

**Inheritance**

1. Create `Person` (`Name`, `Age`) and `Student : Person` (`Id`, `Marks`). Use `base(...)` in the constructor.
2. Create a multilevel chain: `Vehicle` -> `Car` -> `ElectricCar`, and call a method from each level.
3. Make a field `protected` and use it in a derived class.

**Polymorphism** 4. Create `Animal` with a `virtual Speak()` and override it in three classes. Call it from a `List<Animal>`. 5. Write overloaded methods `Area` for a square, a rectangle, and a circle. 6. Override `ToString()` in a `Book` class. 7. Show the difference between `override` and `new` with your own example.

**Abstraction** 8. Create an abstract class `Shape` with `CalculateArea()` and `CalculatePerimeter()`, and implement it in `Circle`, `Rectangle`, and `Triangle`. 9. Create an interface `IPayable` and implement it in `Employee` and `Contractor`. 10. Write an interface `IPrintable` and an interface `ISaveable`, and implement both in one class.

**Problem solving** 11. **Payroll:** extend the employee example with a `ContractEmployee` and print the total payroll. 12. **Bank:** create an abstract `Account` with `Deposit`, `Withdraw`, and an abstract `CalculateInterest()`, then implement `SavingsAccount` and `CurrentAccount`. 13. **Notification:** use `INotifier` with email, SMS, and push implementations, and send one message through all of them.

---

## Summary

- **Inheritance:** a derived class (`class Dog : Animal`) **reuses** the members of a base class. Use `base(...)` to call the base constructor, `protected` for members that children may use, and remember that C# allows **one base class** (but many interfaces).
- **Polymorphism:** the same call behaves differently. **Overloading** (same name, different parameters) is decided at compile time. **Overriding** (`virtual` in the base, `override` in the child) is decided at run time by the **real object**.
- **Abstraction:** hide **how** and show **what**. An **abstract class** (cannot be instantiated, can hold abstract and normal members) is for related classes that share code. An **interface** (a contract) is for abilities, and a class can implement many.
- The three work together: **abstract** base class defines the contract, **inheritance** builds the child classes, and **polymorphism** lets one piece of code work with all of them.
