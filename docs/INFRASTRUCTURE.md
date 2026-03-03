# Infrastructure Layer Overview

The Infrastructure layer provides disk-based persistence for MiniDB.

It implements the `IStorageEngine` interface defined in the Domain layer.

This layer is responsible for:

- Schema persistence
- Table file management
- Row serialization
- Binary file operations

It contains no business rules.

---

## Responsibilities

- Load database metadata from disk
- Persist table definitions
- Append rows to table files
- Read rows from disk
- Rewrite table files for UPDATE and DELETE operations
- Maintain binary format integrity

---

## Storage Model

MiniDB uses a simple file-based storage model:

```
/DatabasePath
database.meta
{tableName}.table
```

### database.meta

Stores:

- Number of tables
- Table names
- Column definitions
- Column data types
- String column max lengths

This file represents the database schema.

---

### Table Files

Each table has its own `.table` file.

Structure:

[Header]
[Row1]
[Row2]
...

Header:

- RowSize (Int32)
- ColumnCount (Int32)

Rows are fixed-length.

---

## Row Format

Each row is written as:

[1 byte] IsDeleted flag  
[Column1]  
[Column2]  
...

Supported column encodings:

- Int → 4 bytes (Int32)
- String → Fixed-length UTF8, padded with zero bytes

Row size is deterministic and derived from the table schema.

---

## Mutation Strategy

UPDATE and DELETE operations:

1. Load all rows into memory (via Application layer)
2. Modify or filter rows
3. Rewrite entire table file

This is an intentional design decision.

The engine is not optimized for large datasets.

---

## Components

### FileStorageEngine

Implements `IStorageEngine`.

Coordinates:

- MetadataSerializer
- RowSerializer
- File operations

---

### MetadataSerializer

Handles:

- Reading and writing `database.meta`
- Converting schema objects to binary format

---

### RowSerializer

Handles:

- Converting `Row` objects to binary
- Reading binary rows from disk
- Applying fixed-length encoding rules

---

## Architectural Boundaries

Infrastructure depends on:

- Domain

Infrastructure must not:

- Contain business rules
- Modify domain invariants
- Perform validation beyond binary correctness
