namespace DBFusion.Factories
{
    public enum DatabaseTypes
    {
        SQL_SERVER,           // Microsoft SQL Server
        MYSQL,                // MySQL
        POSTGRESQL,           // PostgreSQL
        SQLITE,               // SQLite
        MONGODB,              // MongoDB (NoSQL)
        ACCESS,               // Microsoft Access
        ORACLE,               // Oracle Database
        MARIADB,              // MariaDB
        CASSANDRA,            // Cassandra
        COUCHBASE,            // Couchbase
        DYNAMODB,             // DynamoDB
        FIRESTORE,            // Firestore / Firebase Realtime Database
        ELASTICSEARCH,        // Elasticsearch
        NEO4J,                // Neo4j
        DUCKDB,               // DuckDB
        TIMESCALEDB,          // TimescaleDB
        INFLUXDB,             // InfluxDB
        CLICKHOUSE,           // ClickHouse
        SNOWFLAKE,            // Snowflake
        SAP_HANA,             // SAP HANA
        IBM_DB2,              // IBM Db2
        REDIS,                // Redis
        AZURE_SQL,            // Azure SQL
        AWS_RDS,              // AWS RDS
        GOOGLE_CLOUD_SQL,     // Google Cloud SQL
        SYBASE,               // Sybase (SAP ASE)
        FIREBIRD,             // Firebird
        H2,                   // H2
        HSQLDB,               // HSQLDB
        ORIENTDB,             // OrientDB
        ARANGODB,             // ArangoDB
        RAVENDB,              // RavenDB
        VOLTDB,               // VoltDB
        TARANTOOL,            // Tarantool
        LEVELDB,              // LevelDB
        BERKELEYDB,           // Berkeley DB
        FAUNADB,              // FaunaDB
        SCYLLADB,             // ScyllaDB
        MEMCACHED,            // Memcached
        AMAZON_AURORA         // Amazon Aurora
    }
}