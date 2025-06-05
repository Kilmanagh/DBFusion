using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using StackExchange.Redis;
using DBFusion.Databases;
using DBFusion.Factories;

namespace DBFusion.Databases
{
    public class RedisDatabase : DBFusion.Interfaces.IDatabase
    {
        private readonly string _connectionString;

        private ConnectionMultiplexer _redis;

        private StackExchange.Redis.IDatabase _db;

        public RedisDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);
            _db = _redis.GetDatabase();
            return _redis.IsConnected;
        }

        public async Task DisconnectAsync()
        {
            if (_redis != null)
                await _redis.CloseAsync();
        }

        public async Task<int> InsertAsync(string query)
        {
            // Example: SET key value
            var parts = query.Split(' ');
            if (parts.Length >= 3 && parts[0].ToUpper() == "SET")
            {
                bool result = await _db.StringSetAsync(parts[1], parts[2]);
                return result ? 1 : 0;
            }

            return 0;
        }

        public async Task<int> DeleteAsync(string query)
        {
            // Example: DEL key
            var parts = query.Split(' ');
            if (parts.Length >= 2 && parts[0].ToUpper() == "DEL")
            {
                bool result = await _db.KeyDeleteAsync(parts[1]);
                return result ? 1 : 0;
            }

            return 0;
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            // Example: GET key
            var parts = query.Split(' ');
            var dataTable = new DataTable();
            dataTable.Columns.Add("Key");
            dataTable.Columns.Add("Value");
            if (parts.Length >= 2 && parts[0].ToUpper() == "GET")
            {
                var value = await _db.StringGetAsync(parts[1]);
                dataTable.Rows.Add(parts[1], value.ToString());
            }

            return dataTable;
        }

        public async Task<int> UpdateAsync(string query)
        {
            // Same as Insert for Redis
            return await InsertAsync(query);
        }

        public async Task ExecuteCommandAsync(string query)
        {
            // Not implemented for generic Redis
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() => await Task.CompletedTask; // Not supported natively

        public async Task CommitTransactionAsync() => await Task.CompletedTask;

        public async Task RollbackTransactionAsync() => await Task.CompletedTask;

        public static DBFusion.Interfaces.IDatabase Create(DatabaseType type, DbAuth auth)
        {
            switch (type)
            {
                case DatabaseType.ORACLE:
                    return new OracleDatabase(auth);
                case DatabaseType.MARIADB:
                    return new MariaDbDatabase(auth);
                case DatabaseType.CASSANDRA:
                    return new CassandraDatabase(auth);
                case DatabaseType.REDIS:
                    return new RedisDatabase(auth);

                // ...existing cases...
                default:
                    throw new NotSupportedException($"Database type {type} is not supported.");
            }
        }

    }
}