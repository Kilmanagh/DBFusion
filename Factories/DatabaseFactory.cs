// File: Factories\DatabaseFactory.cs
using DBFusion.Databases;
using DBFusion.Interfaces;
using DBFusion.Models;
using MongoDB.Driver;



namespace DBFusion.Factories
{
    public static class DatabaseFactory
    {
        public static IDatabase GetDatabase(DatabaseType dbType, DbAuth auth)
        {
            // Validate DbAuth before using it
            auth.ValidateAuthDetails();

            return dbType switch
                {
                    DatabaseType.SQL_SERVER => new SqlDatabase(auth),
                    DatabaseType.MYSQL => new MySqlDatabase(auth),
                    DatabaseType.POSTGRESQL => new PostgreDatabase(auth),
                    DatabaseType.SQLITE => new SQLiteDatabase(auth),
                    DatabaseType.MONGODB => new MongoDatabase(auth),
                    DatabaseType.ACCESS => new AccessDatabase(auth),
                    DatabaseType.RAVENDB => new RavenDatabase(auth),
                    DatabaseType.BERKELEYDB => new BerkeleyDatabase(auth),
                    DatabaseType.LITEDB => new LiteDBDatabase(auth),
                    DatabaseType.IBMDB2 => new IBMDb2Database(auth),
                    DatabaseType.FIREBIRD => new FirebirdDatabase(auth),
                    _ => throw new NotSupportedException($"Unsupported database type: {dbType}")
                };
        }
    }
}