# DBFusion

DBFusion is an all-in-one database wrapper, middleware, and interface for .NET applications. It allows developers to access and interact with databases without changing their application code, making it easy to switch between different database types or work with multiple databases simultaneously.

## Features

- **Unified Database Interface:** Access various databases through a single, consistent API.
- **Database Agnostic:** Easily switch between different database engines (e.g., SQL Server, PostgreSQL, MySQL, SQLite, MongoDB, Oracle, IBM Db2, Firebird, LiteDB, etc.) without code changes.
- **Multi-Database Support:** Connect and operate on multiple databases, even if they are different types.
- **Middleware Layer:** Abstracts database logic, simplifying migrations and integrations.
- **Seamless Integration:** Drop-in replacement for direct database access in your .NET projects.
- **Extensible Plugin System:** (Planned) Add custom providers for new or proprietary databases.

## Supported Databases

- SQL Server
- PostgreSQL
- MySQL
- SQLite
- MongoDB
- Oracle
- MariaDB
- Cassandra
- Redis
- Neo4j
- DynamoDB
- Couchbase
- InfluxDB
- Snowflake
- Access
- RavenDB
- Berkeley DB
- IBM Db2 *(new, experimental)*
- Firebird *(new, experimental)*
- LiteDB *(new, experimental)*

**Planned / In Progress:**
- Firestore / Firebase Realtime Database
- Azure SQL
- AWS RDS
- Google Cloud SQL
- Sybase (SAP ASE)
- QuickBooks
- VoltDB, Tarantool, LevelDB, ScyllaDB, Memcached, and more

## Getting Started

### Prerequisites

- [.NET 9.0 SDK (Preview)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Supported database engines (see above)
- For IBM Db2, Firebird, and other enterprise databases, ensure native client libraries are installed if required.

### Installation

Clone the repository:
git clone https://github.com/yourusername/DBFusion.git
cd DBFusion