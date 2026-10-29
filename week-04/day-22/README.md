# Business Management Software and SQL Recap

**Date:** 29 October 2026

Today's work:

1. **Business Management Software:** visiting and understanding the software, and how it can be adapted to **your own business type** (for example, **farm management**), and how to **maintain** it.
2. **SQL recap:** a revision of the SQL topics from `SQL Home` to `SQL Group By`.

> **Note:** I do not have the details of the software that was shown, so Part 1 mixes a **fill-in template** (`[ ... ]`, for what you observed) with **general knowledge** of how business management software works and how to adapt it to a farm. Replace the placeholders with what you actually saw.

## Table of Contents

- [Part 1: Business Management Software](#part-1-business-management-software)
  - [Visit Details](#1-visit-details)
  - [What is Business Management Software?](#2-what-is-business-management-software)
  - [Common Modules](#3-common-modules)
  - [How I Explored the Software](#4-how-i-explored-the-software)
  - [Adapting the Software to Different Business Types](#5-adapting-the-software-to-different-business-types)
  - [Farm Management in Detail](#6-farm-management-in-detail)
  - [Maintaining the Software](#7-maintaining-the-software)
  - [What I Learned, Questions, and Next Steps](#8-what-i-learned-questions-and-next-steps)
- [Part 2: SQL Recap](#part-2-sql-recap)
- [Cheat Sheet](#cheat-sheet)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

# Part 1: Business Management Software

## 1. Visit Details

| Field                      | Details                           |
| -------------------------- | --------------------------------- |
| Software name              | [Name]                            |
| Company / vendor           | [Name]                            |
| Visit date                 | 29 October 2026                   |
| Shown by                   | [Name, role]                      |
| Type                       | [Web / Desktop / Mobile]          |
| Technology (if known)      | [For example ASP.NET, SQL Server] |
| Business types it supports | [Retail, wholesale, service, ...] |

## 2. What is Business Management Software?

Software that helps a business **record, manage, and report** its daily work in **one place**, instead of using paper, Excel, and separate tools. It is often called **ERP** (Enterprise Resource Planning) when it covers many areas.

| Without the software             | With the software                  |
| -------------------------------- | ---------------------------------- |
| Paper notebooks and Excel sheets | One central **database**           |
| Hard to find old records         | Quick **search** and **filters**   |
| Manual totals, frequent mistakes | **Automatic** calculations         |
| Reports take hours               | **Instant reports** and dashboards |
| Anyone can change anything       | **Users and roles** control access |

**Basic idea of every business software**

```
Master data (items, customers, suppliers)
        +
Transactions (purchase, sale, expense, stock movement)
        =
Reports (stock, profit, due, performance)
```

## 3. Common Modules

| Module                | What it does                                               | Example data                                |
| --------------------- | ---------------------------------------------------------- | ------------------------------------------- |
| **Dashboard**         | Summary of the business at a glance (sales, stock, profit) | Today's sales, low stock alerts             |
| **Customers**         | Customer information and dues                              | Name, phone, due amount                     |
| **Suppliers**         | Supplier information and payables                          | Name, phone, payable amount                 |
| **Items / Products**  | List of goods and services                                 | Name, category, unit, price                 |
| **Purchase**          | Buying goods from suppliers                                | Purchase order, invoice, quantity           |
| **Sales / Invoices**  | Selling to customers                                       | Invoice, items, discount, payment           |
| **Inventory / Stock** | Stock in, stock out, current balance                       | Item, quantity, reorder level               |
| **Accounts**          | Income, expenses, payments, balance                        | Cash, bank, expense categories              |
| **HR / Payroll**      | Employees, attendance, salary                              | Employee, salary, leave                     |
| **Reports**           | Printable and exportable summaries                         | Sales report, stock report, profit and loss |
| **Users and Roles**   | Who can see and do what                                    | Admin, manager, cashier                     |
| **Settings**          | Company information, business type, tax, backup            | Company name, currency, VAT                 |

Features I saw in the software:

| Feature     | Seen?      | Notes   |
| ----------- | ---------- | ------- |
| [Feature 1] | [Yes / No] | [Notes] |
| [Feature 2] | [Yes / No] | [Notes] |

## 4. How I Explored the Software

| Step | What to do                                                                         | Done |
| ---- | ---------------------------------------------------------------------------------- | ---- |
| 1    | Log in as **admin** and look at the **dashboard** and menus                        | [ ]  |
| 2    | Open **Settings** to see how the business and company are configured               | [ ]  |
| 3    | Check the **master data**: items, customers, suppliers, categories                 | [ ]  |
| 4    | Add a **new item** and a **new customer**                                          | [ ]  |
| 5    | Create a **purchase**, then check the **stock** increases                          | [ ]  |
| 6    | Create a **sale / invoice**, then check the **stock** decreases                    | [ ]  |
| 7    | Record an **expense** and a **payment**                                            | [ ]  |
| 8    | Open the **reports** (stock, sales, profit) and compare them with what was entered | [ ]  |
| 9    | Look at **users and roles** and log in as a normal user to see the difference      | [ ]  |
| 10   | Find out where the **data** is stored and how **backup** is done                   | [ ]  |

A good test of understanding: **follow one item through the whole system.**

```
Purchase 100 units  ->  Stock = 100  ->  Sell 30 units  ->  Stock = 70  ->  Reports show it
```

## 5. Adapting the Software to Different Business Types

Most business software uses the **same core**: items, customers, suppliers, purchase, sale, stock, expense, reports. A different business type mainly changes the **names, categories, and a few extra modules**.

| Business type                | Items are called         | Extra things to track                           |
| ---------------------------- | ------------------------ | ----------------------------------------------- |
| **Retail / grocery shop**    | Products                 | Barcode, discount, expiry date                  |
| **Pharmacy**                 | Medicines                | Batch number, expiry date, prescriptions        |
| **Restaurant**               | Menu items, ingredients  | Recipes, tables, orders, kitchen stock          |
| **Service company**          | Services                 | Projects, hours, quotations, recurring invoices |
| **School / training center** | Courses                  | Students, fees, attendance, results             |
| **Farm**                     | Crops, livestock, inputs | Fields, harvest, feed, production, equipment    |

**How to adapt the software to a new business type**

| Step | Action                                                                                      |
| ---- | ------------------------------------------------------------------------------------------- |
| 1    | **Understand the business:** what is bought, produced, stored, sold, and what the costs are |
| 2    | **Map the generic modules** to the business (see the farm example below)                    |
| 3    | **Set up master data:** categories, items, units, customers, suppliers                      |
| 4    | **Add custom fields or modules** only where the standard ones are not enough                |
| 5    | **Set up users and roles** (owner, manager, worker)                                         |
| 6    | **Enter opening balances** (starting stock, dues, cash)                                     |
| 7    | **Test with real examples** and compare the reports with manual calculations                |
| 8    | **Train the people** who will use it                                                        |

## 6. Farm Management in Detail

### Mapping generic modules to a farm

| Generic module                      | Farm version                          | Examples                                         |
| ----------------------------------- | ------------------------------------- | ------------------------------------------------ |
| Items / Products                    | **Crops, livestock products, inputs** | Rice, potato, milk, eggs, seed, fertilizer, feed |
| Suppliers                           | **Input suppliers**                   | Seed shop, fertilizer dealer, feed supplier      |
| Customers                           | **Buyers**                            | Wholesalers, market traders, restaurants         |
| Purchase                            | **Buying inputs**                     | Seed, fertilizer, pesticide, feed, medicine      |
| Inventory                           | **Store**                             | Seed stock, fertilizer stock, feed stock         |
| Sales                               | **Selling produce**                   | Selling rice, milk, vegetables                   |
| Accounts                            | **Farm income and expenses**          | Labor, fuel, irrigation, vet cost                |
| HR                                  | **Workers**                           | Daily laborers, wages, attendance                |
| (new) **Fields / Plots**            | Land areas                            | North Field (5 acres), South Field (3.5 acres)   |
| (new) **Crop cycles**               | Sowing to harvest                     | Sowing date, expected harvest date               |
| (new) **Harvest / Production**      | Output                                | Quantity harvested, milk per day, eggs per day   |
| (new) **Livestock**                 | Animals                               | Cows, goats, poultry: health, feed, vaccination  |
| (new) **Equipment and maintenance** | Machines                              | Tractor, pump: service dates, repair cost        |

### Farm workflow

```
Buy inputs (seed, fertilizer, feed)
        |
Store (inventory)
        |
Use in the field / for livestock  ->  Record expenses (labor, fuel, water)
        |
Harvest / production
        |
Sell  ->  Record income
        |
Reports: cost, revenue, profit per crop and per field
```

### Data to track

| Area       | Data                                                   |
| ---------- | ------------------------------------------------------ |
| Fields     | Name, area, location, soil type, current crop          |
| Crops      | Crop name, season, sowing date, harvest date           |
| Inputs     | Seed, fertilizer, pesticide: quantity, price, supplier |
| Livestock  | Animal ID, type, age, health, vaccination dates        |
| Production | Harvest quantity, milk and egg count, date, field      |
| Expenses   | Date, category, amount, field or animal                |
| Sales      | Date, buyer, product, quantity, price, payment         |
| Workers    | Name, daily wage, attendance                           |
| Equipment  | Name, purchase date, service dates, repair cost        |

### Sample farm database (small)

**Fields**

| FieldId | FieldName   | AreaAcre |
| ------- | ----------- | -------- |
| 1       | North Field | 5.0      |
| 2       | South Field | 3.5      |

**Crops**

| CropId | CropName |
| ------ | -------- |
| 1      | Rice     |
| 2      | Potato   |

**Harvests**

| HarvestId | FieldId | CropId | HarvestDate | QuantityKg |
| --------- | ------- | ------ | ----------- | ---------- |
| 1         | 1       | 1      | 2026-09-15  | 1200       |
| 2         | 2       | 2      | 2026-09-20  | 800        |
| 3         | 1       | 1      | 2026-10-05  | 600        |

**Expenses**

| ExpenseId | ExpenseDate | Category   | Amount | FieldId |
| --------- | ----------- | ---------- | ------ | ------- |
| 1         | 2026-09-01  | Seed       | 5000   | 1       |
| 2         | 2026-09-03  | Fertilizer | 8000   | 1       |
| 3         | 2026-09-10  | Labor      | 6000   | 2       |
| 4         | 2026-10-02  | Fuel       | 2000   | NULL    |

**Sales**

| SaleId | SaleDate   | CropId | QuantityKg | UnitPrice |
| ------ | ---------- | ------ | ---------- | --------- |
| 1      | 2026-09-20 | 1      | 1000       | 50        |
| 2      | 2026-09-25 | 2      | 500        | 30        |
| 3      | 2026-10-10 | 1      | 500        | 52        |

**Inventory**

| ItemId | ItemName    | Quantity | ReorderLevel |
| ------ | ----------- | -------- | ------------ |
| 1      | Seed        | 40       | 50           |
| 2      | Fertilizer  | 200      | 100          |
| 3      | Cattle Feed | 30       | 60           |

(The prices and quantities are only examples.)

### Example queries (the SQL topics applied to a farm)

**Total harvest per crop** (`JOIN`, `GROUP BY`, `SUM`)

```sql
SELECT c.CropName, SUM(h.QuantityKg) AS TotalKg
FROM Harvests h
JOIN Crops c ON h.CropId = c.CropId
GROUP BY c.CropName
ORDER BY TotalKg DESC;
```

| CropName | TotalKg |
| -------- | ------- |
| Rice     | 1800    |
| Potato   | 800     |

**Total harvest per field**

```sql
SELECT f.FieldName, SUM(h.QuantityKg) AS TotalKg
FROM Harvests h
JOIN Fields f ON h.FieldId = f.FieldId
GROUP BY f.FieldName;
```

Result: North Field `1800`, South Field `800`

**Expenses per category** (`GROUP BY`, `ORDER BY`)

```sql
SELECT Category, SUM(Amount) AS TotalAmount
FROM Expenses
GROUP BY Category
ORDER BY TotalAmount DESC;
```

| Category   | TotalAmount |
| ---------- | ----------- |
| Fertilizer | 8000        |
| Labor      | 6000        |
| Seed       | 5000        |
| Fuel       | 2000        |

**Expenses in a date range** (September 2026)

```sql
SELECT SUM(Amount) AS TotalExpense
FROM Expenses
WHERE ExpenseDate >= '2026-09-01'
  AND ExpenseDate <  '2026-10-01';
```

Result: `19000`

**Revenue per crop**

```sql
SELECT c.CropName, SUM(s.QuantityKg * s.UnitPrice) AS Revenue
FROM Sales s
JOIN Crops c ON s.CropId = c.CropId
GROUP BY c.CropName;
```

| CropName | Revenue |
| -------- | ------- |
| Rice     | 76000   |
| Potato   | 15000   |

**Total profit** (revenue minus expenses)

```sql
SELECT
    (SELECT SUM(QuantityKg * UnitPrice) FROM Sales)
  - (SELECT SUM(Amount) FROM Expenses) AS Profit;
```

Result: `91000 - 21000 = 70000`

**Low stock alert**

```sql
SELECT ItemName, Quantity, ReorderLevel
FROM Inventory
WHERE Quantity <= ReorderLevel;
```

Result: Seed (`40` of `50`) and Cattle Feed (`30` of `60`)

**Best selling crop** (`TOP 1`)

```sql
SELECT TOP 1 c.CropName, SUM(s.QuantityKg) AS TotalSold
FROM Sales s
JOIN Crops c ON s.CropId = c.CropId
GROUP BY c.CropName
ORDER BY TotalSold DESC;
```

Result: `Rice | 1500`

### Farm dashboard and reports

| Report / indicator                         | Why it matters                              |
| ------------------------------------------ | ------------------------------------------- |
| Production per crop and per field          | Which crops and fields perform best         |
| Expense per category                       | Where the money goes                        |
| Revenue, expense, and profit (monthly)     | Is the farm making money?                   |
| Cost per field                             | Profit per acre                             |
| Low stock alerts                           | Do not run out of seed, feed, or fertilizer |
| Livestock health and vaccination due dates | Do not miss treatment dates                 |
| Equipment service due dates                | Avoid machine breakdowns                    |
| Customer dues                              | Money still to collect                      |

## 7. Maintaining the Software

"Maintain" means keeping the software and its data **correct, safe, and working**.

| Frequency              | Task                                                                           |
| ---------------------- | ------------------------------------------------------------------------------ |
| **Daily**              | Enter all transactions (purchase, sale, expense, production) on the same day   |
| **Daily**              | Check the dashboard and low stock alerts                                       |
| **Daily or automatic** | Back up the database                                                           |
| **Weekly**             | Check dues and payments, compare physical stock with system stock              |
| **Monthly**            | Review the profit and expense reports, check the user list, and fix wrong data |
| **Yearly**             | Closing: opening balances, archive old data, review users and prices           |
| **When needed**        | Install updates and fixes, add new items, users, and categories                |

### Good practices

| Area                | Practice                                                                                                    |
| ------------------- | ----------------------------------------------------------------------------------------------------------- |
| **Data quality**    | Enter data on time, use clear names and categories, avoid duplicates                                        |
| **Users and roles** | One login per person, give only the permissions they need, remove users who leave                           |
| **Backup**          | Automatic daily backup, stored in a **second place** (another disk or cloud). **Test a restore** sometimes. |
| **Security**        | Strong passwords, no shared logins, keep the server updated                                                 |
| **Corrections**     | Fix mistakes with a proper correction entry. Do not silently edit or delete records.                        |
| **Support log**     | Record problems, who reported them, and how they were fixed                                                 |

### Simple backup command (SQL Server)

```sql
BACKUP DATABASE BusinessDB
TO DISK = 'C:\Backup\BusinessDB.bak'
WITH INIT, STATS = 10;
```

## 8. What I Learned, Questions, and Next Steps

**What I learned**

- A business software has the **same core** (items, customers, suppliers, purchase, sale, stock, expense, reports) for most businesses.
- A different business type changes the **names, categories, and a few extra modules** (for a farm: fields, crops, harvest, livestock).
- Everything depends on the **database**, so SQL is the base skill.
- [Add your own points]

**Questions to ask**

- [ ] [Question 1]
- [ ] [Question 2]

**Next steps**

- [ ] Choose a business type (for example, a small farm) and list its items, customers, and costs
- [ ] Design the tables and relationships
- [ ] Enter sample data and run the report queries
- [ ] Try adding a new module in the software, or customize an existing one

---

# Part 2: SQL Recap

## Sample Tables

**products**

| id  | name     | category    | price | quantity |
| --- | -------- | ----------- | ----- | -------- |
| 1   | Laptop   | Electronics | 55000 | 5        |
| 2   | Mouse    | Electronics | 500   | 50       |
| 3   | Keyboard | Electronics | 1200  | 30       |
| 4   | Notebook | Stationery  | 80    | 200      |
| 5   | Pen      | Stationery  | 10    | 500      |
| 6   | Chair    | Furniture   | 4500  | NULL     |

**departments**

| dept_id | dept_name |
| ------- | --------- |
| 1       | IT        |
| 2       | HR        |
| 3       | Finance   |
| 4       | Marketing |

**employees**

| emp_id | name   | dept_id | manager_id | salary |
| ------ | ------ | ------- | ---------- | ------ |
| 1      | Rahim  | 1       | NULL       | 90000  |
| 2      | Karim  | 1       | 1          | 60000  |
| 3      | Nusrat | 2       | 1          | 50000  |
| 4      | Sadia  | 3       | 1          | 55000  |
| 5      | Tanvir | NULL    | 2          | 40000  |
| 6      | Mitu   | 1       | 2          | 45000  |

---

## SQL Home and SQL Intro

**SQL** (Structured Query Language) is the standard language to **store, read, change, and delete data** in a **relational database**. Data lives in **tables** made of **rows** and **columns**.

| CRUD       | SQL command   |
| ---------- | ------------- |
| **C**reate | `INSERT INTO` |
| **R**ead   | `SELECT`      |
| **U**pdate | `UPDATE`      |
| **D**elete | `DELETE`      |

## SQL Syntax

- Keywords are not case sensitive. UPPERCASE is the common style.
- A statement ends with `;`.
- Text values use **single quotes**.
- Comments: `-- single line` and `/* multi-line */`.

Order of clauses in a `SELECT`:

```sql
SELECT   columns
FROM     table
WHERE    condition        -- filter rows
GROUP BY columns          -- group rows
HAVING   condition        -- filter groups
ORDER BY columns;         -- sort
```

## SQL Select

```sql
SELECT * FROM products;
SELECT name, price FROM products;
```

## SQL Select Distinct

Returns only unique values.

```sql
SELECT DISTINCT category FROM products;      -- Electronics, Stationery, Furniture
```

## SQL Where

```sql
SELECT * FROM products WHERE category = 'Electronics';
SELECT * FROM products WHERE price > 1000;
```

## SQL Order By

```sql
SELECT name, price FROM products ORDER BY price DESC;    -- large to small
SELECT name, price FROM products ORDER BY price ASC;     -- small to large (default)
```

## SQL And, Or, Not

```sql
SELECT * FROM products WHERE category = 'Electronics' AND price < 1000;   -- Mouse
SELECT * FROM products WHERE category = 'Furniture' OR price < 50;        -- Pen, Chair
SELECT * FROM products WHERE NOT category = 'Electronics';                -- Notebook, Pen, Chair
```

## SQL Insert Into

```sql
INSERT INTO products (id, name, category, price, quantity)
VALUES (7, 'Monitor', 'Electronics', 15000, 10);
```

## SQL Null Values

`NULL` means no value. Use `IS NULL` and `IS NOT NULL`, never `= NULL`.

```sql
SELECT name FROM products WHERE quantity IS NULL;       -- Chair
SELECT name FROM products WHERE quantity IS NOT NULL;
```

## SQL Update

```sql
UPDATE products SET price = 600 WHERE id = 2;
```

## SQL Delete

```sql
DELETE FROM products WHERE id = 7;
```

> **Always use `WHERE` with `UPDATE` and `DELETE`.**

## SQL Select Top

```sql
SELECT TOP 3 name, price FROM products ORDER BY price DESC;               -- SQL Server
SELECT name, price FROM products ORDER BY price DESC LIMIT 3;             -- MySQL / PostgreSQL
```

Result: Laptop, Chair, Keyboard

## SQL Aggregate Functions

Work on many rows and return **one value**. They ignore `NULL` (except `COUNT(*)`).

| Function  | Example                               | Result  |
| --------- | ------------------------------------- | ------- |
| `MIN()`   | `SELECT MIN(price) FROM products;`    | `10`    |
| `MAX()`   | `SELECT MAX(price) FROM products;`    | `55000` |
| `COUNT()` | `SELECT COUNT(*) FROM products;`      | `6`     |
| `SUM()`   | `SELECT SUM(quantity) FROM products;` | `785`   |
| `AVG()`   | `SELECT AVG(price) FROM products;`    | `10215` |

`COUNT(*)` gives `6`, but `COUNT(quantity)` gives `5` because Chair has `NULL`.

## SQL Like and Wildcards

| Wildcard | Meaning                               |
| -------- | ------------------------------------- |
| `%`      | Any number of characters              |
| `_`      | Exactly one character                 |
| `[ ]`    | One character from a set (SQL Server) |

```sql
SELECT name FROM products WHERE name LIKE 'P%';       -- Pen
SELECT name FROM products WHERE name LIKE '%book';    -- Notebook
SELECT name FROM products WHERE name LIKE '_en';      -- Pen
SELECT name FROM products WHERE name LIKE '[LM]%';    -- Laptop, Mouse (SQL Server)
```

## SQL In

```sql
SELECT * FROM products WHERE category IN ('Electronics', 'Furniture');
SELECT * FROM products WHERE category NOT IN ('Electronics', 'Furniture');
```

## SQL Between

Range, both ends included.

```sql
SELECT * FROM products WHERE price BETWEEN 500 AND 5000;      -- Mouse, Keyboard, Chair
```

## SQL Aliases

```sql
SELECT name AS product_name, price * quantity AS stock_value
FROM products AS p;
```

---

## SQL Joins

A **JOIN** combines rows from two tables using a **related column**.

| Join         | Returns                                               |
| ------------ | ----------------------------------------------------- |
| `INNER JOIN` | Only matching rows                                    |
| `LEFT JOIN`  | All rows from the left table + matches from the right |
| `RIGHT JOIN` | All rows from the right table + matches from the left |
| `FULL JOIN`  | All rows from both tables                             |
| `SELF JOIN`  | A table joined with itself                            |

Missing matches show as `NULL`.

### SQL Inner Join

```sql
SELECT e.name, d.dept_name
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id;
```

Result: Rahim-IT, Karim-IT, Nusrat-HR, Sadia-Finance, Mitu-IT. (Tanvir has no department, and Marketing has no employee, so they are left out.)

### SQL Left Join

```sql
SELECT e.name, d.dept_name
FROM employees e
LEFT JOIN departments d ON e.dept_id = d.dept_id;
```

All 6 employees. Tanvir shows `NULL` for the department.

### SQL Right Join

```sql
SELECT e.name, d.dept_name
FROM employees e
RIGHT JOIN departments d ON e.dept_id = d.dept_id;
```

All 4 departments. Marketing shows `NULL` for the employee.

### SQL Full Join

```sql
SELECT e.name, d.dept_name
FROM employees e
FULL JOIN departments d ON e.dept_id = d.dept_id;
```

Everything from both tables. (MySQL has no `FULL JOIN`. Use `LEFT JOIN UNION RIGHT JOIN`.)

### SQL Self Join

A table joined with itself, using **different aliases**.

```sql
SELECT e.name AS employee, m.name AS manager
FROM employees e
LEFT JOIN employees m ON e.manager_id = m.emp_id;
```

| employee | manager |
| -------- | ------- |
| Rahim    | NULL    |
| Karim    | Rahim   |
| Nusrat   | Rahim   |
| Sadia    | Rahim   |
| Tanvir   | Karim   |
| Mitu     | Karim   |

---

## SQL Union and Union All

Combine the results of two `SELECT` statements. They need the **same number of columns** and **compatible data types**.

| Customers: city | Suppliers: city |
| --------------- | --------------- |
| Dhaka           | Dhaka           |
| Chattogram      | Sylhet          |
| Dhaka           | Chattogram      |

```sql
SELECT city FROM customers
UNION
SELECT city FROM suppliers;        -- removes duplicates: Dhaka, Chattogram, Sylhet (3 rows)

SELECT city FROM customers
UNION ALL
SELECT city FROM suppliers;        -- keeps duplicates (6 rows), faster
```

|            | `UNION` | `UNION ALL` |
| ---------- | ------- | ----------- |
| Duplicates | Removed | **Kept**    |
| Speed      | Slower  | **Faster**  |

## SQL Group By

Groups rows with the same value, so aggregate functions work **per group**.

```sql
SELECT d.dept_name, COUNT(*) AS total, AVG(e.salary) AS avg_salary
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name;
```

| dept_name | total | avg_salary |
| --------- | ----- | ---------- |
| IT        | 3     | 65000      |
| HR        | 1     | 50000      |
| Finance   | 1     | 55000      |

- Every column in `SELECT` must be in `GROUP BY` or inside an aggregate function.
- `WHERE` filters rows **before** grouping. `HAVING` filters groups **after** grouping:

```sql
SELECT d.dept_name, COUNT(*) AS total
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id
GROUP BY d.dept_name
HAVING COUNT(*) > 1;          -- IT | 3
```

---

## Cheat Sheet

| Topic                               | One-line meaning                          |
| ----------------------------------- | ----------------------------------------- |
| `SELECT` / `DISTINCT`               | Read data / unique values                 |
| `WHERE`                             | Filter rows                               |
| `ORDER BY`                          | Sort (`ASC` / `DESC`)                     |
| `AND / OR / NOT`                    | Combine or reverse conditions             |
| `INSERT INTO` / `UPDATE` / `DELETE` | Add / change / remove rows                |
| `NULL`                              | No value, test with `IS NULL`             |
| `TOP` / `LIMIT`                     | Limit rows                                |
| `MIN MAX COUNT SUM AVG`             | One value from many rows                  |
| `LIKE` + wildcards                  | Search a text pattern                     |
| `IN` / `BETWEEN`                    | Match a list / a range                    |
| Alias (`AS`)                        | Temporary name                            |
| `INNER JOIN`                        | Matching rows only                        |
| `LEFT / RIGHT / FULL JOIN`          | All rows from one side / both sides       |
| `SELF JOIN`                         | Table joined with itself                  |
| `UNION` / `UNION ALL`               | Combine results without / with duplicates |
| `GROUP BY`                          | Summarize per group                       |

---

## Practice Tasks

1. Pick a business type (for example, a farm, a shop, or a pharmacy) and list its **items, customers, suppliers, and expenses**.
2. Design 5 tables for that business, with primary keys and foreign keys, and insert sample data.
3. Write a query that finds the **total expense per category** and the **total expense in a date range**.
4. Write a query that finds the **highest** and **lowest selling item**.
5. Write a `LEFT JOIN` query that lists customers who have **no sales**.
6. Write a `SELF JOIN` query on any table that has a parent and child relationship (employee and manager).
7. Combine two lists with `UNION` and with `UNION ALL`, and compare the row counts.
8. Use `GROUP BY` and `HAVING` to find items sold **more than a given quantity**.
9. Write a **low stock** query using a reorder level.
10. Write a **profit** query: total sales minus total expenses.

---

## Summary

- **Business management software** keeps master data (items, customers, suppliers) and transactions (purchase, sale, expense, stock) in one database, and produces reports.
- Most business types share the **same core**. A different type (for example, a **farm**) changes the **names, categories, and a few extra modules** such as fields, crops, harvest, and livestock.
- To **maintain** it: enter data daily, **back up** regularly (and test a restore), control **users and roles**, check reports, and fix mistakes with proper corrections.
- **SQL** is the base skill: `SELECT`, `WHERE`, `ORDER BY`, aggregates, `JOIN`, `UNION`, and `GROUP BY` produce most business reports.
- Always use `WHERE` with `UPDATE` and `DELETE`, and use `LEFT JOIN` when you must keep rows that have no match.
