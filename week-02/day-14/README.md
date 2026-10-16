# Topic: Restoring a .bak File and Querying Customers, Purchases, and Invoices

**Date:** 16 October 2026

Documentation of today's work: restoring a SQL Server `.bak` backup file, exploring the database, and running queries on the **customers**, **purchases**, and **invoices** tables.

## Table of Contents

- [Part 1: Restore the .bak File](#part-1-restore-the-bak-file)
- [Part 2: Explore the Database](#part-2-explore-the-database)
- [Part 3: The Three Tables](#part-3-the-three-tables)
- [Part 4: Queries](#part-4-queries)
- [Summary](#summary)

---

# Part 1: Restore the .bak File

A `.bak` file is a **backup of a SQL Server database**. Restoring it recreates the database with all its tables and data.

## Method 1: Using SSMS (GUI)

1. Open **SSMS** and connect to the server.
2. Right-click **Databases** and choose **Restore Database...**
3. Select **Device**, then click the **`...`** button.
4. Click **Add**, choose the `.bak` file, and click **OK**.
5. The database name appears under **Destination**. Change the name here if needed.
6. Check the **Restore Plan** (the backup set is ticked).
7. Open the **Files** page and check where the data and log files will be saved.
8. Open the **Options** page. Tick **Overwrite the existing database (WITH REPLACE)** only if you are replacing an existing database.
9. Click **OK** and wait for the message **"Database restored successfully"**.
10. Right-click **Databases**, click **Refresh**, and expand the new database.

> The backup file should be in a folder that SQL Server can read, such as `C:\Backup\`. If SSMS cannot see the file, move it there.

## Method 2: Using T-SQL

**Step 1: See what is inside the backup (logical file names)**

```sql
RESTORE FILELISTONLY
FROM DISK = 'C:\Backup\mydatabase.bak';
```

Note the **LogicalName** values (one for data, one for log).

**Step 2: Restore the database**

```sql
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backup\mydatabase.bak'
WITH
    MOVE 'MyDatabase_Data' TO 'C:\SQLData\MyDatabase.mdf',
    MOVE 'MyDatabase_Log'  TO 'C:\SQLData\MyDatabase_log.ldf',
    REPLACE,
    STATS = 10;
```

| Part                    | Meaning                                                   |
| ----------------------- | --------------------------------------------------------- |
| `RESTORE DATABASE name` | The new (or existing) database name                       |
| `FROM DISK`             | Path of the `.bak` file                                   |
| `MOVE ... TO ...`       | New location for the data (`.mdf`) and log (`.ldf`) files |
| `REPLACE`               | Overwrite an existing database with the same name         |
| `STATS = 10`            | Show progress every 10 percent                            |

**Step 3: Check the result**

```sql
USE MyDatabase;
SELECT name FROM sys.tables ORDER BY name;
```

---

# Part 2: Explore the Database

Before writing queries, understand what is in the database.

**List all tables**

```sql
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

**See the columns of one table**

```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'purchases';
```

**Quick shortcut for table structure**

```sql
EXEC sp_help 'purchases';
```

**Peek at the data**

```sql
SELECT TOP 10 * FROM customers;
SELECT TOP 10 * FROM purchases;
SELECT TOP 10 * FROM invoices;
```

**Count rows**

```sql
SELECT COUNT(*) AS total_customers FROM customers;
SELECT COUNT(*) AS total_purchases FROM purchases;
SELECT COUNT(*) AS total_invoices  FROM invoices;
```

---

# Part 3: The Three Tables

Example structure (adjust to your real columns):

**customers**

| Column          | Type     | Meaning            |
| --------------- | -------- | ------------------ |
| `customer_id`   | INT (PK) | Unique customer ID |
| `customer_name` | VARCHAR  | Customer name      |
| `email`         | VARCHAR  | Email              |
| `phone`         | VARCHAR  | Phone              |
| `city`          | VARCHAR  | City               |

**purchases**

| Column          | Type     | Meaning              |
| --------------- | -------- | -------------------- |
| `purchase_id`   | INT (PK) | Unique purchase ID   |
| `customer_id`   | INT (FK) | Links to `customers` |
| `product_name`  | VARCHAR  | Product name         |
| `quantity`      | INT      | Units bought         |
| `unit_price`    | DECIMAL  | Price per unit       |
| `purchase_date` | DATETIME | Date of purchase     |

**invoices**

| Column         | Type     | Meaning              |
| -------------- | -------- | -------------------- |
| `invoice_id`   | INT (PK) | Unique invoice ID    |
| `purchase_id`  | INT (FK) | Links to `purchases` |
| `customer_id`  | INT (FK) | Links to `customers` |
| `invoice_date` | DATETIME | Invoice date         |
| `total_amount` | DECIMAL  | Invoice total        |

**Relationships**

```
customers (1) ----< purchases (many)
purchases (1) ----< invoices  (many or one)
customers (1) ----< invoices  (many)
```

---

# Part 4: Queries

All queries are for **SQL Server**.

## 1. Data within a date range

```sql
SELECT *
FROM purchases
WHERE purchase_date >= '2026-01-01'
  AND purchase_date <  '2026-02-01';       -- all of January 2026
```

Using `BETWEEN`:

```sql
SELECT *
FROM purchases
WHERE purchase_date BETWEEN '2026-01-01' AND '2026-01-31';
```

> **Be careful with `BETWEEN` on `DATETIME`.** `'2026-01-31'` means `2026-01-31 00:00:00`, so purchases made later on January 31 are **missed**. Use `>= start AND < next day` instead (the first example). Use `YYYY-MM-DD` format to avoid date-format problems.

**Invoices in a date range**

```sql
SELECT invoice_id, customer_id, invoice_date, total_amount
FROM invoices
WHERE invoice_date >= '2026-01-01'
  AND invoice_date <  '2026-04-01';        -- January to March
```

**Today, this month, last 30 days**

```sql
-- Today
SELECT * FROM purchases
WHERE CAST(purchase_date AS DATE) = CAST(GETDATE() AS DATE);

-- Last 30 days
SELECT * FROM purchases
WHERE purchase_date >= DATEADD(DAY, -30, GETDATE());

-- Current month
SELECT * FROM purchases
WHERE YEAR(purchase_date) = YEAR(GETDATE())
  AND MONTH(purchase_date) = MONTH(GETDATE());
```

## 2. Highest selling product

**By quantity sold**

```sql
SELECT TOP 1
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
GROUP BY product_name
ORDER BY total_sold DESC;
```

**Top 5 selling products**

```sql
SELECT TOP 5
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
GROUP BY product_name
ORDER BY total_sold DESC;
```

**Include ties** (if two products have the same top number)

```sql
SELECT TOP 1 WITH TIES
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
GROUP BY product_name
ORDER BY total_sold DESC;
```

**By revenue (money earned)**

```sql
SELECT TOP 1
    product_name,
    SUM(quantity * unit_price) AS total_revenue
FROM purchases
GROUP BY product_name
ORDER BY total_revenue DESC;
```

## 3. Lowest selling product

Same query, but sorted **ascending**.

```sql
SELECT TOP 1
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
GROUP BY product_name
ORDER BY total_sold ASC;
```

> This finds the lowest **among products that were sold at least once**. A product with **zero** sales is not in `purchases`. To include it, you need a `products` table and a `LEFT JOIN` from `products` to `purchases`.

## 4. Highest and lowest selling product in a date range

```sql
-- Highest selling product in January 2026
SELECT TOP 1
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
WHERE purchase_date >= '2026-01-01'
  AND purchase_date <  '2026-02-01'
GROUP BY product_name
ORDER BY total_sold DESC;

-- Lowest selling product in January 2026
SELECT TOP 1
    product_name,
    SUM(quantity) AS total_sold
FROM purchases
WHERE purchase_date >= '2026-01-01'
  AND purchase_date <  '2026-02-01'
GROUP BY product_name
ORDER BY total_sold ASC;
```

## 5. Sorting: DESC and ASC

| Sort                | Meaning                                            | Example                      |
| ------------------- | -------------------------------------------------- | ---------------------------- |
| `ORDER BY col ASC`  | Small to large, A to Z, oldest to newest (default) | `ORDER BY purchase_date ASC` |
| `ORDER BY col DESC` | Large to small, Z to A, newest to oldest           | `ORDER BY total_amount DESC` |

```sql
-- Newest purchases first
SELECT * FROM purchases
ORDER BY purchase_date DESC;

-- Smallest invoices first
SELECT * FROM invoices
ORDER BY total_amount ASC;

-- Sort by two columns: customer name A-Z, then newest purchase first
SELECT c.customer_name, p.product_name, p.purchase_date
FROM customers c
INNER JOIN purchases p ON c.customer_id = p.customer_id
ORDER BY c.customer_name ASC, p.purchase_date DESC;
```

## 6. Join the three tables

```sql
SELECT
    c.customer_name,
    p.product_name,
    p.quantity,
    i.invoice_id,
    i.invoice_date,
    i.total_amount
FROM customers c
INNER JOIN purchases p ON c.customer_id = p.customer_id
INNER JOIN invoices  i ON p.purchase_id = i.purchase_id
ORDER BY i.invoice_date DESC;
```

## 7. Smaller operations

**Total sales amount in a date range**

```sql
SELECT SUM(total_amount) AS total_sales
FROM invoices
WHERE invoice_date >= '2026-01-01'
  AND invoice_date <  '2026-02-01';
```

**Number of purchases per customer**

```sql
SELECT c.customer_name, COUNT(*) AS total_purchases
FROM customers c
INNER JOIN purchases p ON c.customer_id = p.customer_id
GROUP BY c.customer_name
ORDER BY total_purchases DESC;
```

**Total amount spent per customer (top 10)**

```sql
SELECT TOP 10
    c.customer_name,
    SUM(i.total_amount) AS total_spent
FROM customers c
INNER JOIN invoices i ON c.customer_id = i.customer_id
GROUP BY c.customer_name
ORDER BY total_spent DESC;
```

**Customers who never purchased**

```sql
SELECT c.customer_name
FROM customers c
WHERE NOT EXISTS (
    SELECT 1 FROM purchases p WHERE p.customer_id = c.customer_id
);
```

**Customers who purchased more than 5 times** (`HAVING`)

```sql
SELECT c.customer_name, COUNT(*) AS total_purchases
FROM customers c
INNER JOIN purchases p ON c.customer_id = p.customer_id
GROUP BY c.customer_name
HAVING COUNT(*) > 5;
```

**Monthly sales summary**

```sql
SELECT
    YEAR(invoice_date)  AS sale_year,
    MONTH(invoice_date) AS sale_month,
    COUNT(*)            AS total_invoices,
    SUM(total_amount)   AS total_sales
FROM invoices
GROUP BY YEAR(invoice_date), MONTH(invoice_date)
ORDER BY sale_year, sale_month;
```

**Average, minimum, and maximum invoice amount**

```sql
SELECT
    AVG(total_amount) AS average_invoice,
    MIN(total_amount) AS smallest_invoice,
    MAX(total_amount) AS largest_invoice
FROM invoices;
```

**Search customers by name or city**

```sql
SELECT * FROM customers WHERE customer_name LIKE 'A%';
SELECT * FROM customers WHERE city IN ('Dhaka', 'Chattogram');
```

**Unique products and cities**

```sql
SELECT DISTINCT product_name FROM purchases;
SELECT DISTINCT city FROM customers;
```

**Label invoices with `CASE`**

```sql
SELECT invoice_id, total_amount,
    CASE
        WHEN total_amount >= 50000 THEN 'High'
        WHEN total_amount >= 10000 THEN 'Medium'
        ELSE 'Low'
    END AS invoice_level
FROM invoices;
```

**Replace `NULL` values**

```sql
SELECT customer_name, COALESCE(phone, 'No phone') AS phone
FROM customers;
```

**Save a reusable result as a view**

```sql
CREATE VIEW vw_product_sales AS
SELECT product_name, SUM(quantity) AS total_sold
FROM purchases
GROUP BY product_name;

SELECT * FROM vw_product_sales ORDER BY total_sold DESC;
```

---

## Summary

- A **`.bak` file** is a SQL Server backup. Restore it with **SSMS** (Restore Database) or `RESTORE DATABASE ... FROM DISK`.
- **Explore first**: list tables and columns, peek at rows with `TOP 10`, and count rows.
- **Date range**: use `>= start AND < next day` for `DATETIME` columns.
- **Highest / lowest selling product**: `SUM(quantity)` + `GROUP BY product` + `ORDER BY ... DESC` (highest) or `ASC` (lowest) + `TOP 1`.
- **Sorting**: `DESC` for large to small and newest first, `ASC` for small to large and oldest first.
- Combine tables with **`JOIN`**, summarize with **`GROUP BY`** and **`HAVING`**, and reuse results with **views**.
