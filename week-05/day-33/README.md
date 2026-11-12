# C# OOP: Inheritance, Polymorphism, and Abstraction

**Date:** 12 November 2026

Today's topics (learning stage): **Inheritance**, **Polymorphism**, and **Abstraction**. Practice and problem solving are planned for the next day.

## Table of Contents

- [Quick Overview](#quick-overview)
- [1. Inheritance](#1-inheritance)
- [2. Polymorphism](#2-polymorphism)
- [3. Abstraction](#3-abstraction)
- [Comparison and Cheat Sheet](#comparison-and-cheat-sheet)
- [Summary](#summary)

---

## Quick Overview

| Concept          | One-line meaning                                              | Key C# words                       |
| ---------------- | ------------------------------------------------------------- | ---------------------------------- |
| **Inheritance**  | A class **reuses** the members of another class               | `:`, `base`, `protected`, `sealed` |
| **Polymorphism** | The **same call behaves differently** depending on the object | `virtual`, `override`, overloading |
| **Abstraction**  | **Hide the details**, show only what is needed                | `abstract`, `interface`            |

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

### `sealed`: stop inheritance

```csharp
sealed class FinalClass { }
// class Other : FinalClass { }     // Error: cannot inherit from a sealed class
```

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

**Output**

```
Some sound
Woof!
Meow!
```

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

---

## Comparison and Cheat Sheet

| Keyword     | Meaning                                                                               |
| ----------- | ------------------------------------------------------------------------------------- |
| `:`         | Inherit from a class or implement an interface                                        |
| `protected` | Visible to the class and its children                                                 |
| `sealed`    | The class cannot be inherited                                                         |
| `virtual`   | The method **can** be overridden                                                      |
| `override`  | Replaces a `virtual` or `abstract` method                                             |
| `abstract`  | A class that cannot be instantiated, or a method with no body that must be overridden |

---

## Summary

- **Inheritance:** a derived class (`class Dog : Animal`) **reuses** the members of a base class. Use `base(...)` to call the base constructor, `protected` for members that children may use, and remember that C# allows **one base class** (but many interfaces).
- **Polymorphism:** the same call behaves differently. **Overloading** (same name, different parameters) is decided at compile time. **Overriding** (`virtual` in the base, `override` in the child) is decided at run time by the **real object**.
- **Abstraction:** hide **how** and show **what**. An **abstract class** (cannot be instantiated, can hold abstract and normal members) is for related classes that share code. An **interface** (a contract) is for abilities, and a class can implement many.
