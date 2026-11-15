# C# Recap and Problem Solving: Polymorphism and Abstraction

**Date:** 15 November 2026

Recap and problem solving practice on **Polymorphism** (2 problems) and **Abstraction** (3 problems).

## Table of Contents
- [Part 1: Polymorphism](#part-1-polymorphism)
  - [Problem 1 (Easy)](#polymorphism-problem-1-easy)
  - [Problem 2 (Medium)](#polymorphism-problem-2-medium)
- [Part 2: Abstraction](#part-2-abstraction)
  - [Problem 1 (Easy)](#abstraction-problem-1-easy)
  - [Problem 2 (Medium)](#abstraction-problem-2-medium)
  - [Problem 3 (Easy, Using Interface)](#abstraction-problem-3-easy-using-interface)

---

# Part 1: Polymorphism

## Polymorphism Problem 1 (Easy)

**Problem:** Create an `Animal` class with a `virtual` method `Sound()` that prints `Animal Sound.`. Create two classes, `Dog` and `Cat`, that **inherit** from `Animal` and **override** `Sound()`:

- `Dog` prints `Dog says: Woof Woof!`
- `Cat` prints `Cat says: Meow Meow!`

In `Main`, create a `Cat` and a `Dog` using `Animal` references, and call `Sound()` on each.

**Solution**
```csharp
using System;

class Animal
{
    public virtual void Sound()
    {
        Console.WriteLine("Animal Sound.");
    }
}

class Dog : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Dog says: Woof Woof!");
    }
}

class Cat : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Cat says: Meow Meow!");
    }
}

class Program
{
    static void Main()
    {
        Animal cat = new Cat();
        cat.Sound();

        Animal dog = new Dog();
        dog.Sound();
    }
}
```

**Output**
```
Cat says: Meow Meow!
Dog says: Woof Woof!
```

---

## Polymorphism Problem 2 (Medium)

**Problem:** Create a `Payment` class with a `virtual` method `ProcessPayment(double amount)` that prints `Processing payment of $` followed by the amount. Create three classes that **inherit** from `Payment` and **override** `ProcessPayment`:

- `CashPayment` prints `Processing Cash Payment: {amount} BDT`
- `CardPayment` prints `Processing Card Payment: {amount} BDT`
- `MobileBankingPayment` prints `Processing Mobile Banking Payment: {amount} BDT`

In `Main`, use `Payment` references to process `1000` as cash, `2000` by card, and `1500` by mobile banking.

**Solution**
```csharp
using System;

class Payment
{
    public virtual void ProcessPayment(double amount)
    {
        Console.WriteLine("Processing payment of $" + amount);
    }
}

class CashPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing Cash Payment: {amount} BDT");
    }
}

class CardPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing Card Payment: {amount} BDT");
    }
}

class MobileBankingPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing Mobile Banking Payment: {amount} BDT");
    }
}

class Program
{
    static void Main()
    {
        Payment p1 = new CashPayment();
        p1.ProcessPayment(1000);

        Payment p2 = new CardPayment();
        p2.ProcessPayment(2000);

        Payment p3 = new MobileBankingPayment();
        p3.ProcessPayment(1500);
    }
}
```

**Output**
```
Processing Cash Payment: 1000 BDT
Processing Card Payment: 2000 BDT
Processing Mobile Banking Payment: 1500 BDT
```

---

# Part 2: Abstraction

## Abstraction Problem 1 (Easy)

**Problem:** Create an **abstract** class `Shape` with the fields `width = 10`, `height = 5`, `radius = 5`, and `pie = 3.1416`, and an **abstract** method `CalculateArea()`. Create two classes that **inherit** from `Shape` and implement `CalculateArea()`:

- `Rectangle` prints `Rectangle Area:` followed by `width * height`
- `Circle` prints `Circle Area:` followed by `pie * radius * radius`

In `Main`, create a `Rectangle` and a `Circle` using `Shape` references, and call `CalculateArea()` on each.

**Solution**
```csharp
using System;

abstract class Shape
{
    public int width = 10;
    public int height = 5;
    public int radius = 5;
    public double pie = 3.1416;
    public abstract void CalculateArea();
}

class Rectangle : Shape
{
    public override void CalculateArea()
    {
        Console.WriteLine($"Rectangle Area: {width * height}");
    }
}

class Circle : Shape
{
    public override void CalculateArea()
    {
        Console.WriteLine($"Circle Area: {pie * radius * radius}");
    }
}

class Program
{
    static void Main()
    {
        Shape rectangle = new Rectangle();
        Shape circle = new Circle();

        rectangle.CalculateArea();
        circle.CalculateArea();
    }
}
```

**Output**
```
Rectangle Area: 50
Circle Area: 78.54
```

---

## Abstraction Problem 2 (Medium)

**Problem:** Employee Salary Management System. Create an **abstract** class `Employee` with an **abstract property** `name` and an **abstract** method `CalculateSalary()`. Create three classes that **inherit** from `Employee`:

- `FullTimeEmployee` with `monthlySalary`: prints the monthly salary
- `PartTimeEmployee` with `hourlyRate` and `workingHours`: prints `hourlyRate * workingHours`
- `ContractEmployee` with `contractAmount`: prints the contract amount

Each class prints `{name} Salary: {amount} BDT`.

In `Main`, create `Dipu` (full time, `5000`), `Rahim` (part time, `500` per hour for `80` hours), and `Karim` (contract, `75000`), using `Employee` references, and calculate each salary.

**Solution**
```csharp
using System;

// Employee Salary Management System

abstract class Employee
{
    public abstract string name { get; set; }
    public abstract void CalculateSalary();
}

class FullTimeEmployee : Employee
{
    public override string name { get; set; }
    public decimal monthlySalary { get; set; }
    public override void CalculateSalary()
    {
        Console.WriteLine($"{name} Salary: {monthlySalary} BDT");
    }
}

class PartTimeEmployee : Employee
{
    public override string name { get; set; }
    public decimal hourlyRate { get; set; }
    public int workingHours { get; set; }
    public override void CalculateSalary()
    {
        decimal salary = hourlyRate * workingHours;
        Console.WriteLine($"{name} Salary: {salary} BDT");
    }
}

class ContractEmployee : Employee
{
    public override string name { get; set; }
    public decimal contractAmount { get; set; }
    public override void CalculateSalary()
    {
        Console.WriteLine($"{name} Salary: {contractAmount} BDT");
    }
}

class Program
{
    static void Main()
    {
        Employee emp1 = new FullTimeEmployee { name = "Dipu", monthlySalary = 5000 };
        Employee emp2 = new PartTimeEmployee { name = "Rahim", hourlyRate = 500, workingHours = 80 };
        Employee emp3 = new ContractEmployee { name = "Karim", contractAmount = 75000 };

        emp1.CalculateSalary();
        emp2.CalculateSalary();
        emp3.CalculateSalary();
    }
}
```

**Output**
```
Dipu Salary: 5000 BDT
Rahim Salary: 40000 BDT
Karim Salary: 75000 BDT
```

---

## Abstraction Problem 3 (Easy, Using Interface)

**Problem:** Create an **interface** `IAnimal` with a method `MakeSound()`. Create two classes that **implement** the interface:

- `Dog` prints `Dog is barking`
- `Cat` prints `Cat is meowing`

In `Main`, create a `Dog` and a `Cat` using `IAnimal` references, and call `MakeSound()` on each.

**Solution**
```csharp
using System;

interface IAnimal
{
    void MakeSound();
}

class Dog : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Dog is barking");
    }
}

class Cat : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Cat is meowing");
    }
}

class Program
{
    static void Main()
    {
        IAnimal dog = new Dog();
        IAnimal cat = new Cat();

        dog.MakeSound();
        cat.MakeSound();
    }
}
```

**Output**
```
Dog is barking
Cat is meowing
```