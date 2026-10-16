# Topic: Restoring a .bak File and Querying Customers, Purchases, and Invoices

**Date:** 16 October 2026

Documentation of today's work: restoring a SQL Server `.bak` backup file, exploring the database, and running queries on the **customers**, **purchases**, and **invoices** tables.

## Table of Contents

- [Overview](#overview)
- [Part 1: Restore the .bak File](#part-1-restore-the-bak-file)
- [Part 2: Explore the Database](#part-2-explore-the-database)
- [Part 3: The Three Tables](#part-3-the-three-tables)
- [Part 4: Queries](#part-4-queries)
- [Common Problems and Fixes](#common-problems-and-fixes)
- [Practice Tasks](#practice-tasks)
- [Summary](#summary)

---

## Overview

| Item          | Details                                                                                               |
| ------------- | ----------------------------------------------------------------------------------------------------- |
| Backup file   | `.bak` (SQL Server backup)                                                                            |
| Tool          | SQL Server Management Studio (SSMS)                                                                   |
| Database size | About 15 to 20 tables                                                                                 |
| Tables used   | `customers`, `purchases`, `invoices`                                                                  |
| Tasks         | Date range data, highest and lowest selling product, sorting (`DESC` / `ASC`), and smaller operations |

---
