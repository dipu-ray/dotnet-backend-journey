# C# OOP Relationships: Association, Aggregation, and Composition

**Date:** 14 November 2026

Today's topic: the **relationships between classes** in OOP: **Association**, **Aggregation**, and **Composition**, with practice.

## Table of Contents
- [1. Relationships Between Classes](#1-relationships-between-classes)
- [2. Association](#2-association)
- [3. Aggregation](#3-aggregation)
- [4. Composition](#4-composition)
- [5. Comparison](#5-comparison)
- [6. How to Choose](#6-how-to-choose)
- [7. All Three Together](#7-all-three-together)
- [8. Practice Problems](#8-practice-problems)
- [Summary](#summary)

---

## 1. Relationships Between Classes

Real programs have many classes, and the classes **work together**. A relationship describes **how one class is connected to another**.

| Relationship | Meaning | Question |
|--------------|---------|----------|
| **Inheritance** | A class **is a** kind of another class | "Is a `Dog` an `Animal`?" |
| **Association** | A class **uses** or **knows** another class | "Does a `Doctor` treat a `Patient`?" |
| **Aggregation** | A class **has** another class, and the part **can live alone** | "Does a `Team` have `Player`s who exist without the team?" |
| **Composition** | A class **owns** another class, and the part **cannot live without** the whole | "Does a `House` have `Room`s that make no sense without the house?" |

```
Association   ->  uses / knows        (weakest)
Aggregation   ->  has-a (shared)      (medium)
Composition   ->  part-of (owned)     (strongest)
```

Association, aggregation, and composition are all **"has-a" / "uses-a"** relationships. They differ in **ownership** and in **how long the objects live**.

---

## 2. Association

**Association** is a **general relationship between two classes**. One object **uses**, **knows**, or **works with** another object.

| Property | Detail |
|----------|--------|
| Ownership | **None.** Neither object owns the other |
| Lifetime | **Independent.** Each object can exist alone |
| Strength | The **weakest** relationship |
| Example | A `Doctor` treats a `Patient` |

### Example: the object is used in a method

```csharp
class Patient
{
    public string Name { get; }

    public Patient(string name)
    {
        Name = name;
    }
}

class Doctor
{
    public string Name { get; }

    public Doctor(string name)
    {
        Name = name;
    }

    public void Treat(Patient patient)           // uses a Patient, but does not own it
    {
        Console.WriteLine($"Dr. {Name} is treating {patient.Name}");
    }
}
```

```csharp
Doctor doctor = new Doctor("Anwar");
Patient patient1 = new Patient("Rahim");
Patient patient2 = new Patient("Karim");

doctor.Treat(patient1);       // Dr. Anwar is treating Rahim
doctor.Treat(patient2);       // Dr. Anwar is treating Karim
```

The doctor and the patients are **separate objects**. The doctor only **uses** a patient when treating.

### Example: the object is stored as a reference

```csharp
class Customer
{
    public string Name { get; }

    public Customer(string name)
    {
        Name = name;
    }
}

class Order
{
    public int Id { get; }
    public Customer Customer { get; }             // the order knows its customer

    public Order(int id, Customer customer)
    {
        Id = id;
        Customer = customer;
    }

    public void Display()
    {
        Console.WriteLine($"Order {Id} placed by {Customer.Name}");
    }
}
```

```csharp
Customer customer = new Customer("Rahim");
Order order1 = new Order(101, customer);
Order order2 = new Order(102, customer);

order1.Display();       // Order 101 placed by Rahim
order2.Display();       // Order 102 placed by Rahim
```

One customer is **shared** by several orders. Neither owns the other.

### Direction and multiplicity

| Direction | Meaning | Example |
|-----------|---------|---------|
| **Unidirectional** | Only **one side** knows the other | `Order` knows `Customer`, but `Customer` does not know its orders |
| **Bidirectional** | **Both** sides know each other | `Student` has courses, and `Course` has students |

| Multiplicity | Meaning | Example |
|--------------|---------|---------|
| **One-to-one** | One object relates to one object | `Person` and `Passport` |
| **One-to-many** | One object relates to many objects | `Customer` and `Order`s |
| **Many-to-many** | Many objects relate to many objects | `Student`s and `Course`s |

```csharp
class Student
{
    public string Name { get; }
    public List<Course> Courses { get; } = new List<Course>();

    public Student(string name) { Name = name; }
}

class Course
{
    public string Title { get; }
    public List<Student> Students { get; } = new List<Student>();

    public Course(string title) { Title = title; }
}

Student s1 = new Student("Rahim");
Course c1 = new Course("C#");

s1.Courses.Add(c1);          // many-to-many: both sides reference each other
c1.Students.Add(s1);
```

---

## 3. Aggregation

**Aggregation** is a **"has-a" relationship** where one class (the **whole**) **contains** objects of another class (the **parts**), but the **parts can exist independently** of the whole.

| Property | Detail |
|----------|--------|
| Ownership | **Weak.** The whole **holds** the parts but does not own their life |
| Lifetime | The parts **survive** if the whole is removed |
| Creation | The parts are **created outside** and **passed in** |
| Sharing | A part can belong to **more than one** whole |
| Example | A `Team` has `Player`s. A `Department` has `Teacher`s |

### Example: Team and Player

```csharp
class Player
{
    public string Name { get; }

    public Player(string name)
    {
        Name = name;
    }
}

class Team
{
    public string Name { get; }
    private readonly List<Player> _players = new List<Player>();

    public Team(string name)
    {
        Name = name;
    }

    public void AddPlayer(Player player)           // the player is created outside and passed in
    {
        _players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }

    public void Show()
    {
        Console.Write($"{Name}: ");
        for (int i = 0; i < _players.Count; i++)
        {
            Console.Write(_players[i].Name);
            if (i < _players.Count - 1)
            {
                Console.Write(", ");
            }
        }
        Console.WriteLine();
    }
}
```

```csharp
Player p1 = new Player("Rahim");               // created outside the team
Player p2 = new Player("Karim");

Team teamA = new Team("Team A");
teamA.AddPlayer(p1);
teamA.AddPlayer(p2);
teamA.Show();                                  // Team A: Rahim, Karim

Team teamB = new Team("Team B");
teamB.AddPlayer(p1);                           // the same player joins another team
teamB.Show();                                  // Team B: Rahim

teamA.RemovePlayer(p2);                        // removing Karim from Team A...
teamA.Show();                                  // Team A: Rahim
Console.WriteLine(p2.Name);                    // Karim  (...but Karim still exists)
```

**What shows that it is aggregation**
- The `Player` objects are **created outside** the `Team` and **passed in**.
- A player can be in **several teams**.
- Removing a player from a team (or losing the team) **does not destroy the player**.

---

## 4. Composition

**Composition** is a **strong "has-a" ("part-of") relationship**. The **whole owns the parts**, and the **parts cannot exist without the whole**. When the whole is gone, the parts are gone too.

| Property | Detail |
|----------|--------|
| Ownership | **Strong.** The whole **owns** the parts |
| Lifetime | The parts **live and die with the whole** |
| Creation | The parts are **created inside** the whole (`new` in the constructor or in a method) |
| Sharing | A part belongs to **only one** whole |
| Example | A `Car` has an `Engine`. A `House` has `Room`s. An `Order` has `OrderItem`s |

### Example: Car and Engine

```csharp
class Engine
{
    public int Horsepower { get; }

    public Engine(int horsepower)
    {
        Horsepower = horsepower;
    }

    public void Start()
    {
        Console.WriteLine("Engine started");
    }
}

class Car
{
    private readonly Engine _engine;               // owned by the car
    public string Model { get; }

    public Car(string model, int horsepower)
    {
        Model = model;
        _engine = new Engine(horsepower);          // the Car creates its own Engine
    }

    public void Start()
    {
        Console.WriteLine($"{Model}: starting...");
        _engine.Start();
    }
}
```

```csharp
Car car = new Car("Toyota", 150);
car.Start();
```

**Output**
```
Toyota: starting...
Engine started
```

The `Engine` is created **inside** `Car`. No outside code owns it or shares it. When the `Car` is no longer used, its `Engine` goes with it.

### Example: Order and OrderItem (a nested private class)

Making the part a **private nested class** shows composition clearly: an `OrderItem` **cannot exist outside an `Order`**.

```csharp
class Order
{
    private class OrderItem                        // only the Order can see and use this class
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }

    private readonly List<OrderItem> _items = new List<OrderItem>();
    public int Id { get; }

    public Order(int id)
    {
        Id = id;
    }

    public void AddItem(string product, decimal price, int quantity)
    {
        _items.Add(new OrderItem                   // the Order creates its own items
        {
            Product = product,
            Price = price,
            Quantity = quantity
        });
    }

    public decimal GetTotal()
    {
        decimal total = 0m;
        foreach (OrderItem item in _items)
        {
            total += item.Total;
        }
        return total;
    }

    public void Display()
    {
        Console.WriteLine($"Order {Id}");
        foreach (OrderItem item in _items)
        {
            Console.WriteLine($"  {item.Product} x {item.Quantity} = {item.Total}");
        }
        Console.WriteLine($"  Total: {GetTotal()}");
    }
}
```

```csharp
Order order = new Order(101);
order.AddItem("Pen", 10m, 5);
order.AddItem("Notebook", 80m, 2);
order.Display();
```

**Output**
```
Order 101
  Pen x 5 = 50
  Notebook x 2 = 160
  Total: 210
```

**What shows that it is composition**
- The `Order` **creates** every `OrderItem` itself (`new OrderItem`).
- `OrderItem` is `private`, so no code outside `Order` can create or hold one.
- An item has **no meaning without its order**.

### How to recognize it in code

| | Aggregation | Composition |
|---|-------------|-------------|
| Where the part is created | **Outside** the whole | **Inside** the whole |
| How it reaches the whole | Passed in (parameter or constructor) | `new Part()` inside the whole |
| Part used elsewhere? | **Yes** | **No** |

---

## 5. Comparison

| | Association | Aggregation | Composition |
|---|-------------|-------------|-------------|
| **Meaning** | Uses / knows | Has-a (shared) | Part-of (owned) |
| **Strength** | Weakest | Medium | Strongest |
| **Ownership** | None | Weak | **Strong** |
| **Lifetime** | Independent | Parts live **independently** | Parts **die with the whole** |
| **Part created** | Outside | **Outside**, passed in | **Inside** the whole |
| **Part shared** | Yes | Yes | **No** |
| **UML symbol** | Plain line or arrow | **Hollow diamond** at the whole | **Filled diamond** at the whole |
| **Example** | `Doctor` and `Patient` | `Team` and `Player` | `Car` and `Engine` |

**UML notation**
```
Association:   Doctor ---------> Patient
Aggregation:   Team  <>--------- Player        (hollow diamond: the whole)
Composition:   Car   <#>-------- Engine        (filled diamond: the whole)
```

---

## 6. How to Choose

Ask these questions in order:

| Question | If yes |
|----------|--------|
| 1. Do the two classes only **use or know** each other, with no containing? | **Association** |
| 2. Does one class **contain** the other, and the part can **exist and be used on its own** (or shared)? | **Aggregation** |
| 3. Does one class **own** the other, and the part has **no meaning** without the whole? | **Composition** |

| Pair | Relationship | Reason |
|------|--------------|--------|
| `Teacher` teaches `Student` | Association | They only work together |
| `Department` has `Teacher`s | Aggregation | A teacher exists without the department |
| `Library` has `Book`s | Aggregation | Books can move to another library |
| `House` has `Room`s | Composition | A room does not exist without its house |
| `Computer` has a `Motherboard` | Composition | Created and owned as part of the computer |
| `Order` has `OrderItem`s | Composition | Items belong to one order only |

**Has-a vs is-a:** use inheritance for **"is-a"** (`Dog` is an `Animal`). Use association, aggregation, or composition for **"has-a" / "uses-a"** (`Car` has an `Engine`). A common design rule is to **prefer composition over inheritance** when a "has-a" relationship fits.

---

## 7. All Three Together

A small university model.

```csharp
using System;
using System.Collections.Generic;

class Student
{
    public string Name { get; }

    public Student(string name)
    {
        Name = name;
    }
}

class Teacher
{
    public string Name { get; }

    public Teacher(string name)
    {
        Name = name;
    }

    // ASSOCIATION: the teacher uses a student, but does not own it
    public void Teach(Student student)
    {
        Console.WriteLine($"{Name} is teaching {student.Name}");
    }
}

class Course
{
    public string Title { get; }

    public Course(string title)
    {
        Title = title;
    }
}

class Department
{
    public string Name { get; }

    // AGGREGATION: teachers are created outside and passed in
    private readonly List<Teacher> _teachers = new List<Teacher>();

    // COMPOSITION: the department creates and owns its courses
    private readonly List<Course> _courses = new List<Course>();

    public Department(string name)
    {
        Name = name;
    }

    public void AddTeacher(Teacher teacher)
    {
        _teachers.Add(teacher);
    }

    public void AddCourse(string title)
    {
        _courses.Add(new Course(title));
    }

    public void Display()
    {
        Console.WriteLine($"Department: {Name}");
        foreach (Teacher t in _teachers)
        {
            Console.WriteLine($"  Teacher: {t.Name}");
        }
        foreach (Course c in _courses)
        {
            Console.WriteLine($"  Course: {c.Title}");
        }
    }
}

class Program
{
    static void Main()
    {
        Student student = new Student("Rahim");
        Teacher teacher = new Teacher("Mr. Karim");

        teacher.Teach(student);                       // association

        Department cse = new Department("CSE");
        cse.AddTeacher(teacher);                      // aggregation
        cse.AddCourse("C# Programming");              // composition
        cse.AddCourse("Database");
        cse.Display();

        cse = null;                                   // the department is no longer used

        teacher.Teach(student);                       // the teacher still exists and works
    }
}
```

**Output**
```
Mr. Karim is teaching Rahim
Department: CSE
  Teacher: Mr. Karim
  Course: C# Programming
  Course: Database
Mr. Karim is teaching Rahim
```

| Relationship | Where | Why |
|--------------|-------|-----|
| **Association** | `Teacher.Teach(Student student)` | The teacher only uses a student |
| **Aggregation** | `Department` and `Teacher` | The teacher is created outside and still exists after `cse = null` |
| **Composition** | `Department` and `Course` | The department creates its courses with `new Course(...)`, and they can be reached only through the department |

---

## 8. Practice Problems

**Association**
1. `Doctor` and `Patient`: a `Doctor` has a method `Treat(Patient patient)`. Create two doctors and three patients.
2. `Customer` and `Order`: an `Order` stores its `Customer`. Create two orders for one customer.
3. `Student` and `Course`: a **many-to-many** association where each student has a list of courses and each course has a list of students.

**Aggregation**
4. `Team` and `Player`: add and remove players, and show that a removed player still exists and can join another team.
5. `Library` and `Book`: create books outside the library, add them, and move one book to another library.
6. `Department` and `Teacher`: add teachers, and show that a teacher still exists when the department is no longer used.

**Composition**
7. `Car` and `Engine`: the car creates its own engine in its constructor.
8. `House` and `Room`: the house creates its rooms, and prints the total area.
9. `Order` and `OrderItem`: use a private nested class, and calculate the total.
10. `Computer` with its own `Cpu` and `Ram`: the computer creates them, and a method prints the full configuration.

**Mixed**
11. **School system:** `Teacher` teaches `Student` (association), `School` has `Teacher`s (aggregation), and `School` owns its `Classroom`s (composition). Print the structure.
12. For each pair of classes you create, write one line explaining **why** it is association, aggregation, or composition.

---

## Summary

- **Association:** two classes **use or know** each other. There is **no ownership**, and the objects are **independent**. It is the weakest relationship (`Doctor` treats `Patient`).
- **Aggregation:** the whole **has** parts, but the parts **can exist independently**. They are **created outside** and **passed in**, and can be **shared** (`Team` and `Player`).
- **Composition:** the whole **owns** its parts. The parts are **created inside**, belong to **one** whole, and **cannot exist without it** (`Car` and `Engine`, `Order` and `OrderItem`).
- **Strength:** association < aggregation < composition.
- To decide: do they only **use** each other (association), is the part **independent** (aggregation), or is it **owned and meaningless alone** (composition)?
- Use inheritance for **"is-a"**, and these three for **"has-a" / "uses-a"**.