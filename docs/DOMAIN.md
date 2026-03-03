# Domain Layer Overview

The Domain layer represents the core database engine logic.

It contains no persistence logic, no file handling, and no infrastructure concerns.

Its responsibility is to model:

- Database structure
- Table schema
- Row validation
- Core invariants

---

## Core Concepts

### Database

Aggregate root that manages tables.

Responsibilities:

- Register tables
- Prevent duplicate table names
- Provide table lookup

It does NOT store rows in memory.

Rows live in storage.

---

### Table

Represents a relational table definition.

Contains:

- Table name
- Ordered list of columns

Responsibilities:

- Define schema
- Validate row structure
- Calculate fixed row size
- Enforce schema invariants

Table is responsible for ensuring:

- Correct column count
- Correct value types
- Correct string length constraints

---

### Column

Defines a single column in a table.

Properties:

- Name
- DataType
- MaxLength (required for string columns)

For fixed-length row storage, string columns must define a maximum length.

---

### Row

Represents a data record.

Contains:

- Ordered list of values

Responsibilities:

- Store values
- Provide indexed access

Row does NOT:

- Validate schema
- Perform serialization
- Know about disk storage

Schema validation occurs in Table.

---

### DataType

Enum defining supported column types.

Current supported types:

- Int (4 bytes)
- String (fixed length, defined per column)

---

## Design Principles Applied

- Clean Architecture
- Aggregate Root pattern
- Explicit invariants
- Separation of concerns
- Storage independence

---

## Current Mutation Strategy

UPDATE and DELETE operations do not mutate rows in place.

Instead:

1. All rows are read from storage.
2. Rows are filtered or modified in memory.
3. The table file is rewritten entirely.

This simplifies implementation and preserves architectural clarity.

This approach is not optimized for large datasets and is considered an intentional limitation for educational purposes.
