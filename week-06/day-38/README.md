# Recap: C# OOP Relationships (Association, Aggregation, Composition)

**Date:** 15 November 2026

A quick recap of how classes are connected through **"uses-a"** and **"has-a"** relationships.

## Table of Contents
- [The Three Relationships](#the-three-relationships)
- [Association](#association)
- [Aggregation](#aggregation)
- [Composition](#composition)
- [Comparison](#comparison)
- [How to Choose](#how-to-choose)
- [Summary](#summary)

---

## The Three Relationships

| Relationship | Meaning | Strength | Example |
|--------------|---------|----------|---------|
| **Association** | One class **uses or knows** another | Weakest | `Doctor` treats `Patient` |
| **Aggregation** | The whole **has** parts, and the parts **can live alone** | Medium | `Team` has `Player`s |
| **Composition** | The whole **owns** parts, and the parts **cannot live without it** | Strongest | `Car` has an `Engine` |

```
Association   ->  uses / knows
Aggregation   ->  has-a (shared, independent parts)
Composition   ->  part-of (owned, dependent parts)
```

> Inheritance is **"is-a"** (`Dog` is an `Animal`). These three are **"has-a" / "uses-a"**.

---

## Association

Two classes **work together**. Neither owns the other, and each can **exist alone**.

```csharp
class Doctor
{
    public string Name { get; }
    public Doctor(string name) { Name = name; }

    public void Treat(Patient patient)            // uses a Patient
    {
        Console.WriteLine($"Dr. {Name} is treating {patient.Name}");
    }
}
```

- Used through a **method parameter** or a **stored reference** (`Order` keeps a `Customer`).
- Multiplicity: **one-to-one**, **one-to-many**, **many-to-many**.

---

## Aggregation

The whole **holds** its parts, but the parts are **created outside** and **passed in**. They can be **shared** and **survive** without the whole.

```csharp
class Team
{
    private readonly List<Player> _players = new List<Player>();

    public void AddPlayer(Player player)          // the player comes from outside
    {
        _players.Add(player);
    }
}

Player p1 = new Player("Rahim");                  // created outside
Team teamA = new Team();
Team teamB = new Team();
teamA.AddPlayer(p1);
teamB.AddPlayer(p1);                              // the same player in two teams
```

---

## Composition

The whole **creates and owns** its parts. The parts are **created inside**, belong to **one** whole, and **go away with it**.

```csharp
class Car
{
    private readonly Engine _engine;

    public Car(int horsepower)
    {
        _engine = new Engine(horsepower);         // the Car creates its own Engine
    }
}
```

A **private nested class** makes it strict: an `OrderItem` inside `Order` cannot exist outside the order.

---

## Comparison

| | Association | Aggregation | Composition |
|---|-------------|-------------|-------------|
| **Ownership** | None | Weak | **Strong** |
| **Lifetime** | Independent | Parts live **independently** | Parts **die with the whole** |
| **Part created** | Outside | **Outside**, passed in | **Inside** the whole |
| **Part shared** | Yes | Yes | **No** |
| **UML symbol** | Line or arrow | **Hollow** diamond | **Filled** diamond |
| **Example** | `Doctor` and `Patient` | `Team` and `Player` | `Car` and `Engine` |

---

## How to Choose

| Question | Answer |
|----------|--------|
| Do they only **use or know** each other? | **Association** |
| Does one **contain** the other, and the part is **independent** or shared? | **Aggregation** |
| Does one **own** the other, and the part is **meaningless alone**? | **Composition** |

| Pair | Relationship |
|------|--------------|
| `Teacher` teaches `Student` | Association |
| `Department` has `Teacher`s | Aggregation |
| `Library` has `Book`s | Aggregation |
| `House` has `Room`s | Composition |
| `Order` has `OrderItem`s | Composition |

**Quick rule:** part **created outside** = aggregation. Part **created inside** = composition. Only **uses** = association.

---

## Summary

- **Association:** uses or knows. No ownership, independent objects. The weakest relationship.
- **Aggregation:** has-a. Parts are created **outside**, can be **shared**, and **survive** without the whole.
- **Composition:** part-of. The whole **creates and owns** the parts, which live and die with it. The strongest relationship.
- Strength: **association < aggregation < composition**.