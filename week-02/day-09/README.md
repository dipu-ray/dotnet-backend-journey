# Topics: Software visit, Work on Data and Practice SQL

**Date:** 11 October 2026

## 1. Software visit

- I am reviewing this software, but I am having trouble understanding all of its features.
- I want to learn how this software works.
- Just trying to check out this software and get the basics.

## 2. Work on Data

- Initialize data environments and set up active workspaces.
- Streamline data pipelines and incoming data streams.
- Maintain data backups, access controls, and overall system governance.
- Append new records safely into active tables.
- Modify existing records based on specific unique identifiers.

## 3. Practice SQL

### 1. SELECT

- **Purpose:** Retrieves data from a database table.
- **Syntax:** `SELECT column1, column2 FROM table_name;`

### 2. AND

- **Purpose:** Filters records based on multiple conditions (all conditions must be true).
- **Syntax:** `SELECT * FROM table_name WHERE condition1 AND condition2;`

### 3. OR

- **Purpose:** Filters records based on multiple conditions (at least one condition must be true).
- **Syntax:** `SELECT * FROM table_name WHERE condition1 OR condition2;`

### 4. NOT

- **Purpose:** Excludes records that meet a specific condition.
- **Syntax:** `SELECT * FROM table_name WHERE NOT condition;`

### 5. INSERT INTO

- **Purpose:** Adds new records to a table.
- **Syntax:** `INSERT INTO table_name (col1, col2) VALUES (val1, val2);`

### 6. NULL Values

- **Purpose:** Checks for empty or missing data fields.
- **Syntax:** `SELECT * FROM table_name WHERE column_name IS NULL;`

### 7. UPDATE

- **Purpose:** Modifies existing records in a table.
- **Syntax:** `UPDATE table_name SET col1 = val1 WHERE condition;`

### 8. DELETE

- **Purpose:** Removes existing records from a table.
- **Syntax:** `DELETE FROM table_name WHERE condition;`

### 9. SELECT TOP

- **Purpose:** Specifies the maximum number of records to return from the result set.
- **Syntax:** `SELECT TOP number column_name FROM table_name;`
