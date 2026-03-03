# Application Layer Overview

The Application layer orchestrates use cases of the MiniDB engine.

It coordinates Domain objects and delegates persistence to Infrastructure
through the `IStorageEngine` abstraction.

This layer contains no disk logic and no binary serialization code.

---

## Responsibilities

- Load the database at startup
- Expose CRUD use cases
- Coordinate Domain validation
- Delegate persistence to the storage engine
- Maintain separation between Domain and Infrastructure

---

## Core Components

### DatabaseService

Responsible for:

- Loading the database from storage
- Holding the in-memory `Database` aggregate (schema metadata only)
- Delegating persistence operations to `IStorageEngine`

It does NOT:

- Store rows in memory
- Perform serialization
- Perform file IO

---

### Use Cases

The Application layer defines explicit use cases:

- CreateTableUseCase
- InsertRowUseCase
- SelectAllUseCase
- UpdateUseCase
- DeleteUseCase

Each use case:

1. Retrieves the target table from the Database aggregate
2. Applies Domain rules
3. Delegates persistence operations to the storage engine

---

## Mutation Strategy

UPDATE and DELETE operations:

- Load all rows into memory
- Modify or filter rows
- Rewrite the entire table file

This is an intentional design decision for simplicity.

The system is not optimized for large datasets.

---

## Architectural Boundaries

Application depends only on:

- Domain
- IStorageEngine abstraction

It must never depend directly on Infrastructure implementations.
