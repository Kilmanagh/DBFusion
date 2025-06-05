using DBFusion.Factories;
using DBFusion.Interfaces;
using DBFusion.Models;
using LiteDB;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DBFusion.Databases
{
    public class LiteDBDatabase : IDatabase
    {
        private LiteDatabase _db;
        private const string CollectionName = "KeyValueStore";
        private readonly string _dbPath;

        public LiteDBDatabase(DbAuth auth)
        {
            // Here, we assume auth.ConnectionString holds the file path to the LiteDB database.
            _dbPath = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _db = new LiteDatabase(_dbPath);
            return await Task.FromResult(true);
        }

        public async Task DisconnectAsync()
        {
            _db?.Dispose();
            await Task.CompletedTask;
        }

        public async Task<int> InsertAsync(string query)
        {
            // Expecting a query in the form "SET key value"
            var parts = query.Split(' ');
            if (parts.Length >= 3 && parts[0].ToUpper() == "SET")
            {
                string key = parts[1];
                string value = parts[2];
                var col = _db.GetCollection<KeyValueEntity>(CollectionName);
                col.Upsert(new KeyValueEntity { Id = key, Value = value });
                return await Task.FromResult(1);
            }
            return 0;
        }

        public async Task<int> DeleteAsync(string query)
        {
            var parts = query.Split(' ');
            if (parts.Length >= 2 && parts[0].ToUpper() == "DEL")
            {
                string key = parts[1];
                var col = _db.GetCollection<KeyValueEntity>(CollectionName);
                col.Delete(key);
                return await Task.FromResult(1);
            }
            return 0;
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            var parts = query.Split(' ');
            var dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("Value");
            if (parts.Length >= 2 && parts[0].ToUpper() == "GET")
            {
                string key = parts[1];
                var col = _db.GetCollection<KeyValueEntity>(CollectionName);
                var result = col.FindById(key);
                if (result != null)
                {
                    dt.Rows.Add(result.Id, result.Value);
                }
            }
            return await Task.FromResult(dt);
        }

        public async Task<int> UpdateAsync(string query)
        {
            return await InsertAsync(query);
        }

        public async Task ExecuteCommandAsync(string query)
        {
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() => await Task.CompletedTask;
        public async Task CommitTransactionAsync() => await Task.CompletedTask;
        public async Task RollbackTransactionAsync() => await Task.CompletedTask;
    }

}