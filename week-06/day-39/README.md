# C# OOP: SOLID Principles (Introductory Understanding)

**Date:** 19 November 2026

- **Season 1:** **S** (Single Responsibility) and **O** (Open/Closed)
- **Season 2:** **L** (Liskov Substitution Principle)

## Table of Contents
- [1. SOLID Overview](#1-solid-overview)
- [2. Season 1: S and O](#2-season-1-s-and-o)
- [3. Season 2: L (Liskov Substitution Principle)](#3-season-2-l-liskov-substitution-principle)
- [Summary](#summary)

---

## 1. SOLID Overview

| Letter | Principle | Idea |
|--------|-----------|------|
| **S** | **Single Responsibility** | A class should have **one job** (one reason to change) |
| **O** | **Open/Closed** | Open for **extension**, closed for **modification** |
| **L** | **Liskov Substitution** | A child class must be usable **wherever its parent is expected**, without breaking the program |
| **I** | Interface Segregation | Many small, specific interfaces are better than one large one |
| **D** | Dependency Inversion | Depend on **abstractions**, not on concrete classes |

---

## 2. Season 1: S and O

### The principles

| Principle | Meaning |
|-----------|---------|
| **S: Single Responsibility** | Every class has **one responsibility**. Different jobs go into different classes |
| **O: Open/Closed** | New behavior is added by **writing new classes**, not by **editing existing, working code** |

### Code: Employee Bonus and Slip System

```csharp
using System;

// 1. ABSTRACTION + OPEN/CLOSED (O)
public abstract class Employee
{
    // 2. ENCAPSULATION
    public string Name { get; private set; }
    public double BaseSalary { get; private set; }
    public Employee(string name, double baseSalary)
    {
        Name = name;
        if (baseSalary > 0)
        {
            BaseSalary = baseSalary;
        }
        else
        {
            BaseSalary = 10000;
        }
    }
    public abstract double CalculateBonus();
}

// 3. INHERITANCE + POLYMORPHISM
public class Developer : Employee
{
    public Developer(string name, double baseSalary) : base(name, baseSalary) { }
    public override double CalculateBonus()
    {
        return BaseSalary * 0.20;
    }
}

public class TeamLead : Employee
{
    public TeamLead(string name, double baseSalary) : base(name, baseSalary) { }
    public override double CalculateBonus()
    {
        return BaseSalary * 0.25;
    }
}

public class Manager : Employee
{
    public Manager(string name, double baseSalary) : base(name, baseSalary) { }
    public override double CalculateBonus()
    {
        return BaseSalary * 0.30;
    }
}

// 4. SINGLE RESPONSIBILITY PRINCIPLE (S)
public class SalarySlipPrinter
{
    public void PrintSlip(Employee emp)
    {
        double bonus = emp.CalculateBonus();
        Console.WriteLine($"Employee: {emp.Name}");
        Console.WriteLine($"Base Salary: {emp.BaseSalary} BDT");
        Console.WriteLine($"Bonus: {bonus} BDT");
        Console.WriteLine($"Total: {emp.BaseSalary + bonus} BDT");
    }
}

public class EmployeeRepository
{
    public void SaveToDatabase(Employee emp)
    {
        Console.WriteLine($"Saved {emp.Name} to database!");
        Console.WriteLine("-----------------------------------");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Employee dev = new Developer("Dipu Ray", 0);
        Employee tmld = new Manager("Karim Sagor", 50000);
        Employee mgr = new TeamLead("Rahim Bhuiyan", 60000);
        SalarySlipPrinter printer = new SalarySlipPrinter();
        EmployeeRepository repo = new EmployeeRepository();

        printer.PrintSlip(dev);
        repo.SaveToDatabase(dev);
        printer.PrintSlip(tmld);
        repo.SaveToDatabase(tmld);
        printer.PrintSlip(mgr);
        repo.SaveToDatabase(mgr);
    }
}
```

### Output

```
Employee: Dipu Ray
Base Salary: 10000 BDT
Bonus: 2000 BDT
Total: 12000 BDT
Saved Dipu Ray to database!
-----------------------------------
Employee: Karim Sagor
Base Salary: 50000 BDT
Bonus: 15000 BDT
Total: 65000 BDT
Saved Karim Sagor to database!
-----------------------------------
Employee: Rahim Bhuiyan
Base Salary: 60000 BDT
Bonus: 15000 BDT
Total: 75000 BDT
Saved Rahim Bhuiyan to database!
-----------------------------------
```

### OOP concepts used in this code

| Concept | Where |
|---------|-------|
| **Abstraction** | `abstract class Employee` with `abstract CalculateBonus()` |
| **Encapsulation** | `private set` on `Name` and `BaseSalary`, and the check in the constructor |
| **Inheritance** | `Developer`, `TeamLead`, and `Manager` inherit `Employee` (with `base(...)`) |
| **Polymorphism** | `emp.CalculateBonus()` gives a different result for each employee type |

---

## 3. Season 2: L (Liskov Substitution Principle)

### The principle

**Liskov Substitution Principle (LSP):** objects of a **child class** must be able to **replace** objects of the **parent class** **without breaking the program**. A child class must honor everything the parent class promises.

### The problem LSP points out in the Season 1 design

In Season 1, `Employee` **promises** that every employee has `CalculateBonus()`.

If an `Intern` (who gets **no bonus**) inherits from `Employee`, it must still provide `CalculateBonus()`. The only honest implementation is to **throw an exception**:

```csharp
public override double CalculateBonus()
{
    throw new NotImplementedException("Interns do not get bonuses!");
}
```

Then `SalarySlipPrinter.PrintSlip(Employee emp)` **crashes** when it receives an `Intern`, even though an `Intern` **is an** `Employee`. The child **cannot safely replace** the parent, which **breaks LSP**.

### The solution in Season 2

- **Remove** `CalculateBonus()` from `Employee`. Now `Employee` promises only what **every** employee has: a name and a base salary.
- Put the bonus into a **separate interface**, `IBonusEligible`.
- Only the types that **get a bonus** (`Developer`, `TeamLead`, `Manager`) implement it. `Intern` does **not**.
- `SalarySlipPrinter` asks: *"Is this employee bonus eligible?"* with `emp is IBonusEligible`. If not, the bonus is `0`.

### Code

```csharp
using System;

// 1. ABSTRACTION + OPEN/CLOSED (O)
public abstract class Employee
{
    // 2. ENCAPSULATION
    public string Name { get; private set; }
    public double BaseSalary { get; private set; }
    public Employee(string name, double baseSalary)
    {
        Name = name;
        if (baseSalary > 0)
        {
            BaseSalary = baseSalary;
        }
        else
        {
            BaseSalary = 10000;
        }
    }
}

public interface IBonusEligible
{
    double CalculateBonus();
}

// 3. INHERITANCE + POLYMORPHISM
public class Developer : Employee, IBonusEligible
{
    public Developer(string name, double baseSalary) : base(name, baseSalary) { }
    public double CalculateBonus()
    {
        return BaseSalary * 0.20;
    }
}

public class TeamLead : Employee, IBonusEligible
{
    public TeamLead(string name, double baseSalary) : base(name, baseSalary) { }
    public double CalculateBonus()
    {
        return BaseSalary * 0.25;
    }
}

public class Manager : Employee, IBonusEligible
{
    public Manager(string name, double baseSalary) : base(name, baseSalary) { }
    public double CalculateBonus()
    {
        return BaseSalary * 0.30;
    }
}

public class Intern : Employee
{
    public Intern(string name, double baseSalary) : base(name, baseSalary) { }
    public double CalculateBonus()
    {
        throw new NotImplementedException("Interns do not get bonuses!");
    }
}

// 4. SINGLE RESPONSIBILITY PRINCIPLE (S)
public class SalarySlipPrinter
{
    public void PrintSlip(Employee emp)
    {
        double bonus = 0;
        if (emp is IBonusEligible bonusEmp)
        {
            bonus = bonusEmp.CalculateBonus();
        }
        Console.WriteLine($"Employee: {emp.Name}");
        Console.WriteLine($"Base Salary: {emp.BaseSalary} BDT");
        Console.WriteLine($"Bonus: {bonus} BDT");
        Console.WriteLine($"Total: {emp.BaseSalary + bonus} BDT");
    }
}



public class EmployeeRepository
{
    public void SaveToDatabase(Employee emp)
    {
        Console.WriteLine($"Saved {emp.Name} to database!");
        Console.WriteLine("-----------------------------------");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Employee dev = new Developer("Thor", 40000);
        Employee tmld = new TeamLead("Steve", 60000);
        Employee mgr = new Manager("Stark", 80000);
        Employee itn = new Intern("Peter", 20000);

        SalarySlipPrinter printer = new SalarySlipPrinter();
        EmployeeRepository repo = new EmployeeRepository();

        printer.PrintSlip(dev);
        repo.SaveToDatabase(dev);
        printer.PrintSlip(tmld);
        repo.SaveToDatabase(tmld);
        printer.PrintSlip(mgr);
        repo.SaveToDatabase(mgr);
        printer.PrintSlip(itn);
        repo.SaveToDatabase(itn);
    }
}
```

### Output

```
Employee: Thor
Base Salary: 40000 BDT
Bonus: 8000 BDT
Total: 48000 BDT
Saved Thor to database!
-----------------------------------
Employee: Steve
Base Salary: 60000 BDT
Bonus: 15000 BDT
Total: 75000 BDT
Saved Steve to database!
-----------------------------------
Employee: Stark
Base Salary: 80000 BDT
Bonus: 24000 BDT
Total: 104000 BDT
Saved Stark to database!
-----------------------------------
Employee: Peter
Base Salary: 20000 BDT
Bonus: 0 BDT
Total: 20000 BDT
Saved Peter to database!
-----------------------------------
```

---

## Summary

- **SOLID** has five principles: **S**ingle Responsibility, **O**pen/Closed, **L**iskov Substitution, **I**nterface Segregation, and **D**ependency Inversion. Today covered **S, O, and L**.
- **S (Single Responsibility):** one class, one job. `Employee` handles employee data and bonus, `SalarySlipPrinter` prints, and `EmployeeRepository` saves.
- **O (Open/Closed):** add new behavior with **new classes**, not by editing working code. A new employee type needs only a new class that overrides `CalculateBonus()`.
- **L (Liskov Substitution):** a child class must work **wherever its parent is expected**. A child that has to **throw an exception** for something the parent promises (an `Intern` with no bonus) breaks the principle.
- **Fix used in Season 2:** move the bonus into the **`IBonusEligible` interface**, implemented only by the types that get a bonus, and let `SalarySlipPrinter` check `emp is IBonusEligible`.