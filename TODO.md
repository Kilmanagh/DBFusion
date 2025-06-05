# TODO for DBFusion

This document tracks the progress and requirements for the DBFusion project. Tasks are sorted by priority and status.

---

## Core Requirements

- [x] Unified database interface for multiple database types
- [x] Middleware layer to abstract database logic
- [x] Support for switching between different database engines without code changes
- [x] Multi-database support (connect and operate on multiple databases)
- [ ] Implement robust error handling and logging
- [ ] Comprehensive documentation and usage examples
- [ ] Automated tests for all supported database types
- [ ] Support for database transactions across multiple engines
- [ ] Connection pooling and performance optimization
- [ ] Configuration system for managing multiple connection strings
- [ ] Release stable NuGet package

---

## Essential Tasks To Complete

- [ ] Finalize and document the public API
- [ ] Add support for additional databases:
    - [x] Oracle Database (implemented, needs real CRUD logic)
    - [x] MariaDB (implemented, needs real CRUD logic)
    - [x] Cassandra (implemented, needs real CRUD logic)
    - [x] Redis (implemented, needs real CRUD logic)
    - [x] Neo4j (implemented, needs real CRUD logic)
    - [x] DynamoDB (implemented, needs real CRUD logic)
    - [x] Couchbase (implemented, needs real CRUD logic)
    - [x] InfluxDB (implemented, needs real CRUD logic)
    - [x] Snowflake (implemented, needs real CRUD logic)
    - [x] MongoDB (implemented, needs real CRUD logic)
    - [x] PostgreSQL (implemented, needs real CRUD logic)
    - [x] MySQL (implemented, needs real CRUD logic)
    - [x] SQLite (implemented, needs real CRUD logic)
    - [x] Access (implemented, needs real CRUD logic)
    - [x] IBM Db2 (experimental, needs real CRUD logic)
    - [x] Firebird (experimental, needs real CRUD logic)
    - [x] LiteDB (experimental, needs real CRUD logic)
    - [ ] Firestore / Firebase Realtime Database 
    - [x] RavenDB (implemented, needs real CRUD logic)
    - [x] Berkeley DB (implemented, needs real CRUD logic)
    - [ ] VoltDB
    - [ ] Tarantool
    - [ ] LevelDB
    - [ ] ScyllaDB
    - [ ] Memcached

- [ ] Implement migration helpers (schema migration, data migration)
- [ ] Add CLI tool for managing and testing connections
- [ ] Write integration tests for real-world scenarios
- [ ] Create example projects for common use cases
- [ ] Set up CI/CD for automated builds and tests
- [ ] Write a detailed migration guide for switching databases

---

## Suggestions & Feature Ideas

- [ ] Provide a plugin system for custom database providers
- [ ] Plugin system will decouple database logic from application code and allow for easy addition of new database types, referencing external libraries or NuGet packages.
- [ ] Plugin system must identify the types of database: NOSQL, SQL, Graph, etc., and allow for custom CRUD operations and queries, memory or disk-based storage options.
- [ ] Plugin system must identify required NuGet packages for each database type and automatically install them if not present, matching the current .NET version.
- [ ] Plugin system is designed so that references to databases are not needed in the main project, but only in the plugin project.
- [ ] Implement a caching layer for frequently accessed data
- [ ] Add support for stored procedures and functions
- [ ] Implement advanced query features (joins, aggregations, etc.)
- [ ] Support for database migrations and schema evolution
- [ ] Implement a fluent API for building queries
- [ ] Add support for database triggers and events
- [ ] Implement a data validation layer
- [ ] Add support for database views
- [ ] Implement a data seeding mechanism
- [ ] Add support for database backups and restores
- [ ] Implement a data export/import feature
- [ ] Add support for database replication and clustering
- [ ] Implement a data synchronization feature
- [ ] Plugin system must allow for custom data types and serialization methods.
- [ ] Plugin system must support async CRUD operations and queries.
- [ ] Implement query builder/ORM-like features
- [ ] Add monitoring and metrics (query performance, connection health)
- [ ] Support for database sharding and replication scenarios
- [ ] Web-based admin dashboard for managing connections and monitoring
- [ ] Integration with cloud database services (Azure, AWS, GCP)
- [ ] Localization and internationalization support
- [ ] Security features (encryption, secrets management, auditing)
- [ ] Auto-detection of database type from connection string
- [ ] Azure SQL
- [ ] AWS RDS
- [ ] Google Cloud SQL
- [ ] Sybase (SAP ASE)
- [ ] QuickBooks 

---

## Review Summary Table

| Area                | Status        | Notes                                                      |
|---------------------|---------------|------------------------------------------------------------|
| Solution/Project    | ✅ Complete    |                                                            |
| Core DBs            | ✅ Complete    | SQL Server, PostgreSQL, MongoDB, MySQL, SQLite, Access     |
| New DBs (Top 5)     | ✅ Complete    | Oracle, MariaDB, Cassandra, Redis, Neo4j                   |
| Next 5 DBs          | ✅ Complete    | DynamoDB, Couchbase, InfluxDB, Snowflake, (Pending real CRUD)|
| Other DBs           | 🚧 In Progress | IBM Db2, Firebird, LiteDB, Firestore/Firebase, Azure SQL, AWS RDS, Google Cloud SQL, Sybase, QuickBooks, VoltDB, Tarantool, LevelDB, ScyllaDB, Memcached |

---

## How to Check Your Solution

1. **Build the Solution**
    - Run `dotnet build` to catch missing files, references, or interface mismatches.
2. **Check Factory Coverage**
    - Review `DatabaseFactory.cs` to ensure every `DatabaseTypes` enum value is handled and returns the correct database class.
    - Add a unit test that tries to create each type and asserts it does not throw.
3. **Interface Implementation**
    - Ensure every database and factory class implements `IDatabase` and all required async methods.
4. **Solution Explorer/Tree**
    - Use Visual Studio’s file explorer to confirm all files are present in the expected directories and included in the project file.
5. **.csproj File Review**
    - Open `DBFusion.csproj` and check that all `.cs` files are included (unless you use `<Compile Include="**\*.cs" />`).
6. **Unit/Smoke Tests**
    - Write a simple test for each database type that calls `ConnectAsync()` and `DisconnectAsync()` to ensure instantiation works.
7. **Static Analysis**
    - Use `dotnet format` or `dotnet analyzers` to catch code issues, missing usings, or style problems.
8. **IntelliSense/IDE Errors**
    - Open each file in Visual Studio and look for red squiggles or warnings.
9. **Documentation Cross-Check**
    - Compare your `TODO.md` and `DatabaseTypes.cs` to ensure all listed databases have corresponding files and factory entries.

---

*Update this checklist as progress is made or new requirements are discovered.*