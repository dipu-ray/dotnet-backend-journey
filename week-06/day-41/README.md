# C# OOP: SOLID Principles, Seasons 3 and 4 (ISP and DIP)

**Date:** 22 November 2026

Introductory understanding of the **SOLID** principles, continued:

- **Season 3:** **I**, the **Interface Segregation Principle (ISP)**
- **Season 4:** **D**, the **Dependency Inversion Principle (DIP)**

Each season has an example and three problems (easy, medium, and hard).

## Table of Contents
- [1. Overview](#1-overview)
- [Part 1: Season 3, Interface Segregation Principle](#part-1-season-3-interface-segregation-principle)
  - [The Principle](#the-principle)
  - [What Goes Wrong Without ISP](#what-goes-wrong-without-isp)
  - [Example: Dog and Fish](#example-dog-and-fish)
  - [Easy Problem: Printers](#easy-problem-printers)
  - [Medium Problem: Employee Roles](#medium-problem-employee-roles)
  - [Hard Problem: Order Types](#hard-problem-order-types)
  - [ISP Summary](#isp-summary)
- [Part 2: Season 4, Dependency Inversion Principle](#part-2-season-4-dependency-inversion-principle)
  - [The Principle](#the-principle-1)
  - [What Goes Wrong Without DIP](#what-goes-wrong-without-dip)
  - [Example: Computer and Speaker](#example-computer-and-speaker)
  - [Easy Problem: Switch and Light](#easy-problem-switch-and-light)
  - [Medium Problem: Payment Service](#medium-problem-payment-service)
  - [Hard Problem: Order Notifications](#hard-problem-order-notifications)
  - [DIP Summary](#dip-summary)
- [SOLID: All Five Principles](#solid-all-five-principles)
- [Summary](#summary)

---

## 1. Overview

| Season | Letter | Principle | Core idea |
|--------|--------|-----------|-----------|
| 1 | **S** | Single Responsibility | One class, one job |
| 1 | **O** | Open/Closed | Open for extension, closed for modification |
| 2 | **L** | Liskov Substitution | A child class can replace its parent safely |
| **3** | **I** | **Interface Segregation** | **Many small, specific interfaces instead of one large interface** |
| **4** | **D** | **Dependency Inversion** | **Depend on abstractions (interfaces), not on concrete classes** |

---

# Part 1: Season 3, Interface Segregation Principle

## The Principle

**Interface Segregation Principle (ISP):** a class should **not be forced to implement methods it does not use**. Instead of one **large** interface, create **several small, focused interfaces**, and let each class implement **only the ones it needs**.

## Example: Dog and Fish

Small interfaces, one for each **ability**. Each animal implements only the abilities it has.

```csharp
using System;

interface IEatable
{
    void Eat();
}

interface IWalkable
{
    void Walk();
}

interface ISwimmable
{
    void Swim();
}

class Dog : IEatable, IWalkable
{
    public void Eat()
    {
        Console.WriteLine("Dog is Eat.");
    }

    public void Walk()
    {
        Console.WriteLine("Dog is Walk.");
    }
}

class Fish : IEatable, ISwimmable
{
    public void Eat()
    {
        Console.WriteLine("Fish is eat.");
    }
    
    public void Swim()
    {
        Console.WriteLine("Fish is swimming.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Dog dog = new Dog();
        dog.Eat();
        dog.Walk();

        Fish fish = new Fish();
        fish.Eat();
        fish.Swim();
    }
}
```

**Output**
```
Dog is Eat.
Dog is Walk.
Fish is eat.
Fish is swimming.
```

---

## Easy Problem: Printers

**Problem:** A normal printer can only **print**. An advanced printer can **print, scan, and fax**. Design the interfaces so that the normal printer is **not forced** to implement scanning or faxing.

```csharp
using System;

interface IPrint
{
    void Print();
}

interface IScan
{
    void Scan();
}

interface IFax
{
    void Fax();
}

class NormalPrinter : IPrint
{
    public void Print()
    {
        Console.WriteLine("Printer is printing.");
    }
}

class AdvancedPrinter : IPrint, IScan, IFax
{
    public void Print()
    {
        Console.WriteLine("Printer is printing.");
    }

    public void Scan()
    {
        Console.WriteLine("Printer is scaning.");
    }

    public void Fax()
    {
        Console.WriteLine("Printer is Faxing.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        NormalPrinter normal = new NormalPrinter();
        normal.Print();

        AdvancedPrinter advanced = new AdvancedPrinter();
        advanced.Print();
        advanced.Scan();
        advanced.Fax();
    }
}
```

**Output**
```
Printer is printing.
Printer is printing.
Printer is scaning.
Printer is Faxing.
```

---

## Medium Problem: Employee Roles

**Problem:** A company has three roles with different duties:

- A **Developer** writes code, conducts meetings, and generates reports.
- A **Manager** conducts meetings and generates reports, but does **not** write code.
- An **Intern** only writes code.

Design the interfaces so that **each role implements only its own duties**.

```csharp
using System;

interface ICodeWriter
{
    void WriteCode();
}

interface IMeetingManager
{
    void ConductMeeting();
}

interface IReportGenerator
{
    void GenerateReport();
}

class Developer : ICodeWriter, IMeetingManager, IReportGenerator
{
    public void WriteCode()
    {
        Console.WriteLine("Developer is writing code.");
    }

    public void ConductMeeting()
    {
        Console.WriteLine("Developer is conducting meeting.");
    }

    public void GenerateReport()
    {
        Console.WriteLine("Developer is generating report.");
    }
}

class Manager : IMeetingManager, IReportGenerator
{
    public void ConductMeeting()
    {
        Console.WriteLine("Manager is conducting meeting.");
    }

    public void GenerateReport()
    {
        Console.WriteLine("Manager is generating report.");
    }
}

class Intern : ICodeWriter
{
    public void WriteCode()
    {
        Console.WriteLine("Intern is writing code.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Developer developer = new Developer();
        developer.WriteCode();
        developer.ConductMeeting();
        developer.GenerateReport();
        Console.WriteLine("\n");

        Manager manager = new Manager();
        manager.ConductMeeting();
        manager.GenerateReport();
        Console.WriteLine("\n");

        Intern intern = new Intern();
        intern.WriteCode();
    }
}
```

**Output**
```
Developer is writing code.
Developer is conducting meeting.
Developer is generating report.


Manager is conducting meeting.
Manager is generating report.


Intern is writing code.
```

---

## Hard Problem: Order Types

**Problem:** An online shop has three kinds of orders. Every order needs **payment and invoice**, but the other steps differ:

| Order type | Steps |
|------------|-------|
| **Online order** | Payment and invoice, **ship the product**, **send an email notification** |
| **Store pickup order** | Payment and invoice, **collect from the store** |
| **Digital order** | Payment and invoice, **send a download link** |

Design the interfaces so that **no order type has a step it does not use**.

```csharp
using System;

// Interfaces
interface IPaymentInvoice
{
    void ProcessPayment();
    void GenerateInvoice();
}

interface IShipProduct
{
    void ShipProduct();
}

interface ISendEmailNotification
{
    void SendEmailNotification();
}

interface ICollectFromStore
{
    void CollectFromStore();
}

interface ISendDownloadLink
{
    void SendDownloadLink();
}

// Classes
class OnlineOrder : IPaymentInvoice, IShipProduct, ISendEmailNotification
{
    public void ProcessPayment()
    {
        Console.WriteLine("Payment processed.");
    }

    public void GenerateInvoice()
    {
        Console.WriteLine("Invoice generated.");
    }

    public void ShipProduct()
    {
        Console.WriteLine("Product shipped.");
    }

    public void SendEmailNotification()
    {
        Console.WriteLine("Email notification sent.\n");
    }
}

class StorePickupOrder : IPaymentInvoice, ICollectFromStore
{
    public void ProcessPayment()
    {
        Console.WriteLine("Payment processed.");
    }

    public void GenerateInvoice()
    {
        Console.WriteLine("Invoice generated.");
    }

    public void CollectFromStore()
    {
        Console.WriteLine("Customer collected product from store.\n");
    }
}

class DigitalOrder : IPaymentInvoice, ISendDownloadLink
{
    public void ProcessPayment()
    {
        Console.WriteLine("Payment processed.");
    }

    public void GenerateInvoice()
    {
        Console.WriteLine("Invoice generated.");
    }

    public void SendDownloadLink()
    {
        Console.WriteLine("Download link sent.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        // Online Order
        OnlineOrder onlineOrder = new OnlineOrder();
        Console.WriteLine("--- Online Order ---");
        onlineOrder.ProcessPayment();
        onlineOrder.GenerateInvoice();
        onlineOrder.ShipProduct();
        onlineOrder.SendEmailNotification();

        // Store Pickup Order
        StorePickupOrder storePickupOrder = new StorePickupOrder();
        Console.WriteLine("--- Store Pickup Order ---");
        storePickupOrder.ProcessPayment();
        storePickupOrder.GenerateInvoice();
        storePickupOrder.CollectFromStore();

        // Digital Order
        DigitalOrder digitalOrder = new DigitalOrder();
        Console.WriteLine("--- Digital Order ---");
        digitalOrder.ProcessPayment();
        digitalOrder.GenerateInvoice();
        digitalOrder.SendDownloadLink();
    }
}
```

**Output**
```
--- Online Order ---
Payment processed.
Invoice generated.
Product shipped.
Email notification sent.

--- Store Pickup Order ---
Payment processed.
Invoice generated.
Customer collected product from store.

--- Digital Order ---
Payment processed.
Invoice generated.
Download link sent.
```

---

## ISP Summary

| Problem | Interfaces | Classes | What ISP prevented |
|---------|------------|---------|--------------------|
| Dog and Fish | `IEatable`, `IWalkable`, `ISwimmable` | `Dog`, `Fish` | A fish forced to walk |
| Printers | `IPrint`, `IScan`, `IFax` | `NormalPrinter`, `AdvancedPrinter` | A normal printer forced to scan and fax |
| Employee roles | `ICodeWriter`, `IMeetingManager`, `IReportGenerator` | `Developer`, `Manager`, `Intern` | A manager or intern forced into duties they do not have |
| Order types | `IPaymentInvoice`, `IShipProduct`, `ISendEmailNotification`, `ICollectFromStore`, `ISendDownloadLink` | `OnlineOrder`, `StorePickupOrder`, `DigitalOrder` | A digital or pickup order forced to ship |

---

# Part 2: Season 4, Dependency Inversion Principle

## The Principle

**Dependency Inversion Principle (DIP):**

1. **High-level modules** (the classes with the main logic) should **not depend on low-level modules** (the classes with the details). **Both** should depend on **abstractions** (interfaces).
2. **Abstractions** should not depend on details. **Details** should depend on abstractions.

| Term | Meaning | In the examples |
|------|---------|-----------------|
| **High-level module** | The class that **uses** something | `Computer`, `Switch`, `PaymentService`, `OrderService` |
| **Low-level module** | The class that does the **detailed work** | `NormalSpeaker`, `Bulb`, `BkashPayment`, `EmailNotification` |
| **Abstraction** | An interface that sits between them | `ISpeaker`, `ILight`, `IPayment`, `INotification` |

```
Without DIP:   High-level class  ───────►  Concrete low-level class

With DIP:      High-level class  ───────►  Interface  ◄───────  Low-level classes
```

**Related terms**

| Term | Meaning |
|------|---------|
| **Dependency Inversion Principle** | The **design rule**: depend on abstractions |
| **Dependency Injection** | A **technique**: the dependency is **given to the class from outside**, here through the **constructor** (constructor injection) |

## What Goes Wrong Without DIP

An illustration of **tight coupling**:

```csharp
class Computer
{
    private NormalSpeaker speaker = new NormalSpeaker();      // tied to one concrete class

    public void PlayMusic()
    {
        speaker.PlaySound();
    }
}
```

- `Computer` creates `NormalSpeaker` itself, so it can use **only** that speaker.
- To use a `BluetoothSpeaker`, the code of `Computer` **must be edited**.
- `Computer` cannot be tested with a different (for example fake) speaker.

With DIP, `Computer` depends on the **`ISpeaker` interface** and receives the speaker **from outside**.

---

## Example: Computer and Speaker

```csharp
using System;

interface ISpeaker
{
    void PlaySound();
}

class NormalSpeaker : ISpeaker
{
    public void PlaySound()
    {
        Console.WriteLine("Normal Speaker is playing.");
    }
}

class BluetoothSpeaker : ISpeaker
{
    public void PlaySound()
    {
        Console.WriteLine("Bluetooth Speaker is playing.");
    }
}

class Computer
{
    ISpeaker speaker;

    public Computer(ISpeaker speaker)
    {
        this.speaker = speaker;
    }

    public void PlayMusic()
    {
        speaker.PlaySound();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Computer c1 = new Computer(new NormalSpeaker());
        c1.PlayMusic();

        Computer c2 = new Computer(new BluetoothSpeaker());
        c2.PlayMusic();
    }
}
```

**Output**
```
Normal Speaker is playing.
Bluetooth Speaker is playing.
```

**How the code applies DIP**

| Part | Role |
|------|------|
| `Computer` | **High-level module**. It holds an `ISpeaker` and never mentions a concrete speaker |
| `ISpeaker` | The **abstraction** |
| `NormalSpeaker`, `BluetoothSpeaker` | **Low-level modules** that implement the abstraction |
| `new Computer(new NormalSpeaker())` | The speaker is **injected through the constructor** |

A new speaker type needs only **a new class** that implements `ISpeaker`. `Computer` does **not change**.

---

## Easy Problem: Switch and Light

**Problem:** A `Switch` turns on a light. The switch should **not depend directly on a bulb**, so that **other kinds of lights** can be used later without changing the switch.

```csharp
using System;

interface ILight
{
    void TurnOn();
}

class Bulb : ILight
{
    public void TurnOn()
    {
        Console.WriteLine("Bulb is ON.");
    }
}

class Switch
{
    ILight light;

    public Switch(ILight light)
    {
        this.light = light;
    }

    public void TurnOnLight()
    {
        light.TurnOn();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Switch b1 = new Switch(new Bulb());
        b1.TurnOnLight();
    }
}
```

**Output**
```
Bulb is ON.
```

**How the code applies DIP**

| Part | Role |
|------|------|
| `Switch` | High-level module, depends on `ILight` |
| `ILight` | Abstraction |
| `Bulb` | Low-level module |

Another light (for example a tube light) can be added with a new `ILight` class, and `Switch` stays the same.

---

## Medium Problem: Payment Service

**Problem:** A `PaymentService` must complete a payment, and the customer can pay by **Bkash**, **Nagad**, or **Card**. The service must **not depend on a specific payment method**.

```csharp
using System;

interface IPayment
{
    void PaymentWay();
}

class BkashPayment : IPayment
{
    public void PaymentWay()
    {
        Console.WriteLine("Payment completed using Bkash.");
    }
}

class NagadPayment : IPayment
{
    public void PaymentWay()
    {
        Console.WriteLine("Payment completed using Nagad.");
    }
}

class CardPayment : IPayment
{
    public void PaymentWay()
    {
        Console.WriteLine("Payment completed using Card.");
    }
}

class PaymentService
{
    IPayment payment;

    public PaymentService(IPayment payment)
    {
        this.payment = payment;
    }

    public void PaymentNewWay()
    {
        payment.PaymentWay();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        PaymentService bkash = new PaymentService(new BkashPayment());
        bkash.PaymentNewWay();

        PaymentService nagad = new PaymentService(new NagadPayment());
        nagad.PaymentNewWay();

        PaymentService card = new PaymentService(new CardPayment());
        card.PaymentNewWay();
    }
}
```

**Output**
```
Payment completed using Bkash.
Payment completed using Nagad.
Payment completed using Card.
```

**How the code applies DIP**

| Part | Role |
|------|------|
| `PaymentService` | High-level module, depends only on `IPayment` |
| `IPayment` | Abstraction |
| `BkashPayment`, `NagadPayment`, `CardPayment` | Low-level modules (the payment methods) |
| `new PaymentService(new BkashPayment())` | The payment method is chosen **from outside** and injected |

Adding a new payment method (for example a bank transfer) needs only **a new class** that implements `IPayment`. `PaymentService` is **not edited**. This also follows the **Open/Closed** principle.

---

## Hard Problem: Order Notifications

**Problem:** After an order is placed, the customer receives a confirmation by **Email**, **SMS**, or **WhatsApp**. The `OrderService` must **not depend on a specific notification method**.

```csharp
using System;

// Interface
interface INotification
{
    void Send();
}

// Classes
class EmailNotification : INotification
{
    public void Send()
    {
        Console.WriteLine("Order placed successfully.");
        Console.WriteLine("Order confirmation sent by Email.\n");
    }
}

class SMSNotification : INotification
{
    public void Send()
    {
        Console.WriteLine("Order placed successfully.");
        Console.WriteLine("Order confirmation sent by SMS.\n");
    }
}

class WhatsAppNotification : INotification
{
    public void Send()
    {
        Console.WriteLine("Order placed successfully.");
        Console.WriteLine("Order confirmation sent by WhatsApp.");
    }
}

class OrderService
{
    INotification notification;
    public OrderService(INotification notification)
    {
        this.notification = notification;
    }

    public void PlaceOrder()
    {
        notification.Send();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        // Email Order
        OrderService email = new OrderService(new EmailNotification());
        Console.WriteLine("--- Email Order ---");
        email.PlaceOrder();

        // SMS Order
        OrderService sms = new OrderService(new SMSNotification());
        Console.WriteLine("--- SMS Order ---");
        sms.PlaceOrder();

        // WhatsApp Order
        OrderService wtp = new OrderService(new WhatsAppNotification());
        Console.WriteLine("--- WhatsApp Order ---");
        wtp.PlaceOrder();
    }
}
```

**Output**
```
--- Email Order ---
Order placed successfully.
Order confirmation sent by Email.

--- SMS Order ---
Order placed successfully.
Order confirmation sent by SMS.

--- WhatsApp Order ---
Order placed successfully.
Order confirmation sent by WhatsApp.
```

**How the code applies DIP**

| Part | Role |
|------|------|
| `OrderService` | High-level module. `PlaceOrder()` calls `notification.Send()` and does not know **how** the message is sent |
| `INotification` | Abstraction |
| `EmailNotification`, `SMSNotification`, `WhatsAppNotification` | Low-level modules |
| Constructor injection | `new OrderService(new EmailNotification())` chooses the channel from outside |

The order logic and the way of notifying are **separate**. A new channel (for example push notification) needs only **a new `INotification` class**, and `OrderService` does not change.

---

## DIP Summary

| Problem | High-level module | Abstraction | Low-level modules |
|---------|-------------------|-------------|-------------------|
| Computer and Speaker | `Computer` | `ISpeaker` | `NormalSpeaker`, `BluetoothSpeaker` |
| Switch and Light | `Switch` | `ILight` | `Bulb` |
| Payment Service | `PaymentService` | `IPayment` | `BkashPayment`, `NagadPayment`, `CardPayment` |
| Order Notifications | `OrderService` | `INotification` | `EmailNotification`, `SMSNotification`, `WhatsAppNotification` |

**The pattern in every example**
1. Create an **interface** (the abstraction).
2. Write the **detailed classes** that implement it.
3. Let the **main class** hold the **interface** (not a concrete class).
4. **Pass** the chosen implementation **through the constructor**.

---

## SOLID: All Five Principles

| Letter | Principle | Season | Key point from the examples |
|--------|-----------|--------|-----------------------------|
| **S** | Single Responsibility | 1 | `SalarySlipPrinter` prints and `EmployeeRepository` saves. Each class has one job |
| **O** | Open/Closed | 1 | A new class is added for a new behavior. Existing code is not edited |
| **L** | Liskov Substitution | 2 | `Intern` is not bonus eligible, and still works wherever an `Employee` is expected |
| **I** | Interface Segregation | 3 | Small interfaces (`IPrint`, `IScan`, `IFax`). A class implements only what it needs |
| **D** | Dependency Inversion | 4 | `PaymentService` depends on `IPayment`, and the implementation is injected |

**ISP and DIP both rely on interfaces**, but for different goals:

| | ISP | DIP |
|---|-----|-----|
| **Question** | "How should I **design** the interface?" | "What should a class **depend on**?" |
| **Rule** | Make interfaces **small and focused** | Depend on **interfaces**, not on concrete classes |
| **Benefit** | No forced, unused methods | Easy to **swap and extend** implementations |

---

## Summary

- **Interface Segregation (ISP):** do not force a class to implement methods it does not use. **Split a large interface into small, focused ones**, and let each class implement only what it needs (`Dog` implements `IEatable` and `IWalkable`, and `Fish` implements `IEatable` and `ISwimmable`).
- ISP examples: `NormalPrinter` vs `AdvancedPrinter`, the `Developer`, `Manager`, and `Intern` roles, and the order types that share `IPaymentInvoice` but differ in the rest.
- **Dependency Inversion (DIP):** high-level classes should **depend on abstractions**, not on concrete classes. The concrete class is **injected from outside**, through the constructor.
- DIP examples: `Computer` with `ISpeaker`, `Switch` with `ILight`, `PaymentService` with `IPayment`, and `OrderService` with `INotification`. New behavior is added with **new classes**, and the high-level class does not change.
- With **S, O, L, I, and D** covered, the introductory understanding of **SOLID** is complete.