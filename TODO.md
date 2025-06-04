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
    - [x] Elasticsearch (implemented, needs real CRUD logic)
    - [x] Neo4j (**stub**)
    - [x] DynamoDB (**stub**)
    - [x] Couchbase (**stub**)
    - [x] InfluxDB (**stub**)
    - [x] Snowflake (**stub**)
    - [ ] MongoDB
    - [ ] PostgreSQL
    - [ ] MySQL
    - [ ] SQLite
    - [ ] Access
    - [ ] Couchbase
    - [ ] Firestore / Firebase Realtime Database
    - [ ] DuckDB
    - [ ] TimescaleDB
    - [ ] ClickHouse
    - [ ] SAP HANA
    - [ ] IBM Db2
    - [ ] Azure SQL
    - [ ] AWS RDS
    - [ ] Google Cloud SQL
    - [ ] Sybase (SAP ASE)
    - [ ] Firebird
    - [ ] H2
    - [ ] HSQLDB
    - [ ] OrientDB
    - [ ] ArangoDB
    - [ ] RavenDB
    - [ ] VoltDB
    - [ ] Tarantool
    - [ ] LevelDB
    - [ ] Berkeley DB
    - [ ] FaunaDB
    - [ ] ScyllaDB
    - [ ] Memcached
    - [ ] Amazon Aurora

> **Legend:**  
> - **[x] Name (implemented, needs real CRUD logic)**: Class exists, but may need more robust implementation.  
> - **[x] Name (**stub**)**: Class exists as a stub (skeleton), needs full implementation.  
> - **[ ] Name**: Not yet implemented.

- [ ] Implement migration helpers (schema migration, data migration)
- [ ] Add CLI tool for managing and testing connections
- [ ] Write integration tests for real-world scenarios
- [ ] Create example projects for common use cases
- [ ] Set up CI/CD for automated builds and tests
- [ ] Write a detailed migration guide for switching databases

---

## Suggestions & Feature Ideas

- [ ] Provide a plugin system for custom database providers
- [ ] Implement query builder/ORM-like features
- [ ] Add monitoring and metrics (query performance, connection health)
- [ ] Support for database sharding and replication scenarios
- [ ] Web-based admin dashboard for managing connections and monitoring
- [ ] Integration with cloud database services (Azure, AWS, GCP)
- [ ] Localization and internationalization support
- [ ] Security features (encryption, secrets management, auditing)
- [ ] Auto-detection of database type from connection string

---

## Review Summary Table

| Area                | Status        | Notes                                                      |
|---------------------|--------------|------------------------------------------------------------|
| Solution/Project    | ✅ Complete   |                                                            |
| Core DBs            | ✅ Complete   | SQL Server, PostgreSQL, MongoDB, MySQL, SQLite, Access     |
| New DBs (Top 5)     | ✅ Complete   | Oracle, MariaDB, Cassandra, Redis, Elasticsearch           |
| Next 5 DBs          | ⚠️ Stubs      | Neo4j, DynamoDB, Couchbase, InfluxDB, Snowflake            |
| Factories           | ✅ Complete   | Wrappers for each DB implementation                        |
| Enum                | ✅ Complete   | All DB types listed                                        |
| Auth Model          | ✅ Complete   | Secure, extensible                                         |
| Tests               | ❌ Missing    | Add unit/integration tests                                 |
| Docs                | ✅ In progress| README, TODO present                                       |
| NuGet Packages      | ⚠️ Check     | Ensure all required packages are referenced                 |
| Other DBs           | ❌ Missing    | Listed in TODO, not yet implemented                        |

---

## How to Check Your Solution

1. **Build the Solution**
    - Run `dotnet build` to catch missing files, references, or interface mismatches.
2. **Check Factory Coverage**
    - Review `DatabaseFactory.cs` to ensure every `DatabaseTypes` enum value is handled and returns the correct factory class.
    - Add a unit test that tries to create each type and asserts it does not throw.
3. **Interface Implementation**
    - Ensure every database and factory class implements `IDatabase` and all required async methods.
4. **Solution Explorer/Tree**
    - Use Visual Studio Code’s file explorer to confirm all files are present in the expected directories and included in the `.csproj`.
5. **.csproj File Review**
    - Open `DBFusion.csproj` and check that all `.cs` files are included (unless you use `<Compile Include="**\*.cs" />`).
6. **Unit/Smoke Tests**
    - Write a simple test for each database type that calls `ConnectAsync()` and `DisconnectAsync()` to ensure instantiation works.
7. **Static Analysis**
    - Use `dotnet format` or `dotnet analyzers` to catch code issues, missing usings, or style problems.
8. **IntelliSense/IDE Errors**
    - Open each file in VS Code and look for red squiggles or warnings.
9. **Documentation Cross-Check**
    - Compare your `TODO.md` and `DatabaseTypes.cs` to ensure all listed DBs have corresponding files and factory entries.

---

*Update this checklist as progress is made or new requirements are discovered.*