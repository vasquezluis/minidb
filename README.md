# MiniDB

MiniDB is a minimal educational database engine built with C# and .NET.

The goal of this project is to understand how relational database engines work internally by implementing:

- Schema management
- Fixed-length row storage
- Binary serialization
- Disk-based persistence
- Clean Architecture layering

This is NOT intended to compete with production databases.  
It is a learning-focused implementation.

---

## Architecture

The solution follows Clean Architecture principles:

- **Domain** — Core database concepts and business rules
- **Application** — Use cases that orchestrate operations
- **Infrastructure** — Disk persistence and binary serialization
- **CLI** — Console interface

Dependencies always point inward.

Infrastructure implements domain interfaces.

---

## Current Features

- Database metadata stored on disk
- Multiple tables supported
- Fixed-length row design
- Strong schema validation
- Disk-based storage (binary format)
- Streaming row reads (no full memory load)

---

## Storage Model

- One `database.meta` file for schema metadata
- One `.table` file per table
- Fixed-length rows for predictable offsets
- Soft delete flag per row

---

## Design Goals

- Clear aggregate boundaries
- Deterministic binary format
- No external database libraries
- Strong domain invariants
- Educational clarity over optimization

---

## Project Status

Current milestone:

- Domain model complete
- Binary storage format designed
- Row serializer planned
- FileStorageEngine pending implementation

---

## Known Limitations (Intentional Design Decisions)

MiniDB currently uses a full-table materialization strategy for UPDATE and DELETE operations.

This means:

- All rows of a table are loaded into memory
- The entire table file is rewritten after modifications
- Operations are O(n) in time
- Memory usage is O(n)

This design is intentional and chosen for:

- Architectural simplicity
- Clear separation of concerns
- Educational clarity
- Avoiding premature optimization

MiniDB is not designed to handle large datasets.

Future versions may implement:

- Streaming rewrites
- In-place row updates
- Page-based storage
- Index-based row lookup
