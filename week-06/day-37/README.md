# C# OOP Relationships: Association, Aggregation, and Composition

**Date:** 17 November 2026

## Table of Contents
- [1. Relationships Between Classes](#1-relationships-between-classes)
- [2. Association](#2-association)
- [3. Aggregation](#3-aggregation)
- [4. Composition](#4-composition)

---

## 1. Relationships Between Classes

| Relationship | Meaning | Question |
|--------------|---------|----------|
| **Association** | A class **uses** or **knows** another class | "Does a `Doctor` treat a `Patient`?" |
| **Aggregation** | A class **has** another class, and the part **can live alone** | "Does a `Team` have `Player`s who exist without the team?" |
| **Composition** | A class **owns** another class, and the part **cannot live without** the whole | "Does a `House` have `Room`s that make no sense without the house?" |

```
Association   ->  uses / knows        (weakest)
Aggregation   ->  has-a (shared)      (medium)
Composition   ->  part-of (owned)     (strongest)
```

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

| Multiplicity | Meaning | Example |
|--------------|---------|---------|
| **One-to-one** | One object relates to one object | `Person` and `Passport` |
| **One-to-many** | One object relates to many objects | `Customer` and `Order`s |
| **Many-to-many** | Many objects relate to many objects | `Student`s and `Course`s |

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
