// File: Factories\DatabaseType.cs
namespace DBFusion.Factories
{
    public enum DatabaseType
    {
        SQL_SERVER,
        MYSQL,
        POSTGRESQL,
        SQLITE,
        MONGODB,
        ACCESS,
        ORACLE,
        MARIADB,
        CASSANDRA,
        COUCHBASE,
        DYNAMODB,
        FIRESTORE,
        NEO4J,
        DUCKDB,
        TIMESCALEDB,
        INFLUXDB,
        CLICKHOUSE,
        SNOWFLAKE,
        SAP_HANA,
        IBMDB2,      // IBM Db2 (updated naming)
        REDIS,
        AZURE_SQL,
        AWS_RDS,
        GOOGLE_CLOUD_SQL,
        SYBASE,
        FIREBIRD,
        H2,
        HSQLDB,
        ORIENTDB,
        ARANGODB,
        RAVENDB,
        VOLTDB,
        TARANTOOL,
        LEVELDB,
        BERKELEYDB,
        FAUNADB,
        SCYLLADB,
        MEMCACHED,
        AMAZON_AURORA,
        LITEDB        // LiteDB (new entry)
    }
}