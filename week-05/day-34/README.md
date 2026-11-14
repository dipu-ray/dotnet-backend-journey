# C# Problem Solving: Encapsulation and Inheritance

**Date:** 14 November 2026

Problem solving practice on **Encapsulation** (4 problems) and **Inheritance** (4 problems).

## Table of Contents
- [Part 1: Encapsulation](#part-1-encapsulation)
  - [Problem 1](#encapsulation-problem-1)
  - [Problem 2](#encapsulation-problem-2)
  - [Problem 3](#encapsulation-problem-3)
  - [Problem 4](#encapsulation-problem-4)
- [Part 2: Inheritance](#part-2-inheritance)
  - [Problem 1](#inheritance-problem-1)
  - [Problem 2](#inheritance-problem-2)
  - [Problem 3](#inheritance-problem-3)
  - [Problem 4](#inheritance-problem-4)

---

# Part 1: Encapsulation

## Encapsulation Problem 1

**Problem:** Create a `Student` class with a **private** field `age`. Add:

- a public method `SetAge(int newAge)` that stores the age **only if it is greater than 0**, otherwise prints `Invalid Age`
- a public method `GetAge()` that returns the age

In `Main`, create a student, set the age to `25`, and display it.

**Solution**
```csharp
using System;

class Student
{
    private int age;

    public void SetAge(int newAge)
    {
        if (newAge > 0)
        {
            age = newAge;
        }
        else
        {
            Console.WriteLine("Invalid Age");

        }
    }

    public int GetAge()
    {
        return age;
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student();
        student1.SetAge(25);
        Console.WriteLine($"Student Age: {student1.GetAge()}");
    }
}
```

**Output**
```
Student Age: 25
```

---

## Encapsulation Problem 2

**Problem:** Create a `Student` class with **private** fields `name` (string), `age` (int), and `marks` (double). Add:

- `SetName(string newName)`
- `SetAge(int newAge)`: accepts the age only if it is greater than 0, otherwise prints `Invalid Age`
- `SetMarks(double newMarks)`: accepts the marks only if they are between 0 and 100, otherwise prints `Invalid Marks`
- `GetName()`, `GetAge()`, and `GetMarks()` to read the values

In `Main`, set the name to `Rahim`, the age to `25`, and the marks to `85.5`, then display them.

**Solution**
```csharp
using System;

class Student
{
    private string name;
    private int age;
    private double marks;

    public void SetName(string newName)
    {
        name = newName;
    }

    public void SetAge(int newAge)
    {
        if (newAge > 0)
            age = newAge;
        else
            Console.WriteLine("Invalid Age");
    }

    public void SetMarks(double newMarks)
    {
        if (newMarks >= 0 && newMarks <= 100)
            marks = newMarks;
        else
            Console.WriteLine("Invalid Marks");
    }

    public string GetName()
    {
        return name;
    }

    public int GetAge()
    {
        return age;
    }

    public double GetMarks()
    {
        return marks;
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student();

        student1.SetName("Rahim");
        student1.SetAge(25);
        student1.SetMarks(85.5);

        Console.WriteLine($"Student Name: {student1.GetName()}");
        Console.WriteLine($"Student Age: {student1.GetAge()}");
        Console.WriteLine($"Student Marks: {student1.GetMarks()}");
    }
}
```

**Output**
```
Student Name: Rahim
Student Age: 25
Student Marks: 85.5
```

---

## Encapsulation Problem 3

**Problem:** Create a `Product` class with a **private** field `price` (double) and a public **property** `Price` with `get` and `set`. The setter must accept the value **only if it is greater than 0**, otherwise print `Invalid Price`.

In `Main`, set the price to `1500`, then try to set it to `-500`, and display the price.

**Solution**
```csharp
using System;

class Product
{
    private double price;

    public double Price
    {
        get
        {
            return price;
        }

        set
        {
            if (value > 0)
            {
                price = value;
            }
            else
            {
                Console.WriteLine("Invalid Price");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Product product1 = new Product();
        product1.Price = 1500;
        product1.Price = -500;
        Console.WriteLine($"Product Price: {product1.Price}");
    }
}
```

**Output**
```
Invalid Price
Product Price: 1500
```

---

## Encapsulation Problem 4

**Problem:** Create a `BankAccount` class with these **private** fields: `accountHolder` (string), `balance` (double), and `transactionCount` (int).

- Provide **read-only properties** `AccountHolder`, `Balance`, and `TransactionCount`.
- Add a **constructor** that takes the account holder and the initial balance. The initial balance is stored only if it is `0` or more.
- `Deposit(double amount)`: if the amount is greater than 0, add it to the balance and increase the transaction count. Otherwise print `Invalid Deposit Amount`.
- `Withdraw(double amount)`: if the amount is greater than 0 and not more than the balance, subtract it and increase the transaction count. Otherwise print `Invalid Withdraw Amount: Insufficient Balance`.
- `DisplayAccountInfo()`: print the account holder, the balance, and the transaction count.

In `Main`, create an account for `Rahim` with `5000`, then deposit `2000`, withdraw `1500`, withdraw `10000`, and deposit `-500`. Display the account information.

**Solution**
```csharp
using System;
using System.Runtime.Remoting.Messaging;

// Class
class BankAccount
{
    // Private fields
    private string accountHolder;
    private double balance;
    private int transactionCount;

    // Properties
    public string AccountHolder {
        get { return accountHolder; }
    }

    public double Balance {
        get { return balance; }
    }

    public int TransactionCount
    {
        get { return transactionCount; }
    }

    // Constructor
    public BankAccount(string accountHolder, double initialBalance)
    {
        this.accountHolder = accountHolder;
        if (initialBalance >= 0)
        {
            this.balance = initialBalance;
        }
    }

    // Methods
    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            balance += amount;
            transactionCount++;
        }
        else
        {
            Console.WriteLine("Invalid Deposit Amount");
        }
    }

    public void Withdraw(double amount)
    {
        if (amount > 0 && amount <= balance)
        {
            balance -= amount;
            transactionCount++;
        }
        else
        {
            Console.WriteLine("Invalid Withdraw Amount: Insufficient Balance");
        }
    }

    public void DisplayAccountInfo()
    {
        Console.WriteLine($"Account Holder: {accountHolder}");
        Console.WriteLine($"Balance: {balance}");
        Console.WriteLine($"Transaction Count: {transactionCount}");
    }
}

class Program
{
    static void Main()
    {
        BankAccount account = new BankAccount("Rahim", 5000);

        account.Deposit(2000);
        account.Withdraw(1500);
        account.Withdraw(10000);
        account.Deposit(-500);

        account.DisplayAccountInfo();
    }
}
```

**Output**
```
Invalid Withdraw Amount: Insufficient Balance
Invalid Deposit Amount
Account Holder: Rahim
Balance: 5500
Transaction Count: 2
```

---

# Part 2: Inheritance

## Inheritance Problem 1

**Problem:** Create a `Person` class with **private** fields `name` and `age`, and a method `DisplayInfo(string name, int age)` that stores the values and displays them. Create a `Student` class that **inherits** from `Person`, with a **private** field `studentId` and a method `studentInfo(int studentId)` that stores and displays the student ID.

In `Main`, create a `Student`, call `DisplayInfo("Rahim", 25)`, and then `studentInfo(101)`.

**Solution**
```csharp
using System;

class Person
{
    private string name;
    private int age;

    public void DisplayInfo(string name, int age)
    {
        this.name = name;
        this.age = age;
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Age: {age}");
    }
}

class Student : Person
{
    private int studentId;

    public void studentInfo(int studentId)
    {
        this.studentId = studentId;
        Console.WriteLine($"Student ID: {studentId}");
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student();
        student1.DisplayInfo("Rahim", 25);
        student1.studentInfo(101);
    }
}
```

**Output**
```
Name: Rahim
Age: 25
Student ID: 101
```

---

## Inheritance Problem 2

**Problem:** Create a `Vehicle` class with **private** fields `brand` and `model`, a method `SetVehicleInfo(string brand, string model)`, and a method `DisplayVehicleInfo()`. Create a `Car` class that **inherits** from `Vehicle`, with a **private** field `numberOfDoors` and a method `DisplayCarInfo(int numberOfDoors)` that displays the vehicle information and the number of doors.

In `Main`, create a `Car` with brand `Toyota` and model `Corolla`, and display its information with `4` doors.

**Solution**
```csharp
using System;

class Vehicle
{
    private string brand;
    private string model;

    public void SetVehicleInfo(string brand, string model)
    {
        this.brand = brand;
        this.model = model;
    }

    public void DisplayVehicleInfo()
    {
        Console.WriteLine($"Brand: {brand}");
        Console.WriteLine($"Model: {model}");
    }
}

class Car : Vehicle
{
    private int numberOfDoors;

    public void DisplayCarInfo(int numberOfDoors)
    {
        DisplayVehicleInfo();
        this.numberOfDoors = numberOfDoors;
        Console.WriteLine($"Number of Doors: {numberOfDoors}");
    }
}

class Program
{
    static void Main()
    {
        Car car1 = new Car();
        car1.SetVehicleInfo("Toyota", "Corolla");
        car1.DisplayCarInfo(4);
    }
}
```

**Output**
```
Brand: Toyota
Model: Corolla
Number of Doors: 4
```

---

## Inheritance Problem 3

**Problem:** Create an `Employee` class with a **protected** field `employeeName` and a **private** field `salary`, a method `SetEmployeeInfo(string employeeName, double salary)`, and a method `DisplaySalary()` that displays the name and the salary. Create a `Manager` class that **inherits** from `Employee` and has a method `DisplayManagerInfo()` that displays the employee information by calling `DisplaySalary()`.

In `Main`, create a `Manager` named `Rahim` with a salary of `50000`, and display the information.

**Solution**
```csharp
using System;

class Employee
{
    protected string employeeName;
    private double salary;

    public void SetEmployeeInfo(string employeeName, double salary)
    {
        this.employeeName = employeeName;
        this.salary = salary;
    }

    public void DisplaySalary()
    {
        Console.WriteLine($"Employee Name: {employeeName}");
        Console.WriteLine($"Employee Salary: {salary}");
    }
}

class Manager : Employee
{
    public void DisplayManagerInfo()
    {
        DisplaySalary();
    }
}

class Program
{
    static void Main()
    {
        Manager manager1 = new Manager();
        manager1.SetEmployeeInfo("Rahim", 50000);
        manager1.DisplayManagerInfo();
    }
}
```

**Output**
```
Employee Name: Rahim
Employee Salary: 50000
```

---

## Inheritance Problem 4

**Problem:** Create a `Person` class with a **protected** field `name`, a **constructor** that takes the name and prints `Person Constructor Called`, and a method `DisplayName()`. Create an `Employee` class that **inherits** from `Person`, with a **private** field `employeeId` and a constructor that takes the name and the ID, calls the **base class constructor** (`base`), and prints `Employee Constructor Called`. Add a method `DisplayEmployeeInfo()` that displays the name and the employee ID.

In `Main`, create an `Employee` with the name `Rahim` and ID `101`, and display the information.

**Solution**
```csharp
using System;

class Person
{
    protected string name;

    public Person(string name)
    {
        this.name = name;
        Console.WriteLine("Person Constructor Called");
    }

    public void DisplayName()
    {
        Console.WriteLine($"Name: {this.name}");
    }
}

class Employee : Person
{
    private int employeeId;

    public Employee(string name, int employeeId) : base(name)
    {
        this.employeeId = employeeId;
        Console.WriteLine("Employee Constructor Called");
    }

    public void DisplayEmployeeInfo()
    {
        DisplayName();
        Console.WriteLine($"Employee ID: {this.employeeId}");
    }
}

class Program
{
    static void Main()
    {
        Employee emp = new Employee("Rahim", 101);
        emp.DisplayEmployeeInfo();
    }
}
```

**Output**
```
Person Constructor Called
Employee Constructor Called
Name: Rahim
Employee ID: 101
```