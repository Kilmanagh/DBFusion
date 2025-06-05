// File: Databases\RavenDatabase.cs
using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace DBFusion.Databases
{
    using DBFusion.Factories;

    public class RavenDatabase : IDatabase
    {
        private readonly string _url;
        private readonly string _databaseName;
        private IDocumentStore _store;

        public RavenDatabase(DbAuth auth)
        {
            // For example purposes, we assume auth.ConnectionString contains the server URL.
            // You may extend this to parse additional configuration.
            _url = auth.ConnectionString;
            // Using a default database name; this can also be part of the auth details.
            _databaseName = "DBFusion";
        }

        public async Task<bool> ConnectAsync()
        {
            _store = new DocumentStore
            {
                Urls = new[] { _url },
                Database = _databaseName
            };
            _store.Initialize();
            // RavenDB doesn't have a native connectivity test in this simple example.
            return await Task.FromResult(true);
        }

        public async Task DisconnectAsync()
        {
            _store?.Dispose();
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
                using (IAsyncDocumentSession session = _store.OpenAsyncSession())
                {
                    var item = new KeyValueEntity { Id = key, Value = value };
                    await session.StoreAsync(item);
                    await session.SaveChangesAsync();
                }
                return 1;
            }
            return 0;
        }

        public async Task<int> DeleteAsync(string query)
        {
            // Expecting a query like "DEL key"
            var parts = query.Split(' ');
            if (parts.Length >= 2 && parts[0].ToUpper() == "DEL")
            {
                string key = parts[1];
                using (IAsyncDocumentSession session = _store.OpenAsyncSession())
                {
                    var item = await session.LoadAsync<KeyValueEntity>(key);
                    if (item != null)
                    {
                        session.Delete(item);
                        await session.SaveChangesAsync();
                        return 1;
                    }
                }
            }
            return 0;
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            // Expecting a query like "GET key"
            var parts = query.Split(' ');
            var dataTable = new DataTable();
            dataTable.Columns.Add("Key");
            dataTable.Columns.Add("Value");

            if (parts.Length >= 2 && parts[0].ToUpper() == "GET")
            {
                string key = parts[1];
                using (IAsyncDocumentSession session = _store.OpenAsyncSession())
                {
                    var item = await session.LoadAsync<KeyValueEntity>(key);
                    if (item != null)
                    {
                        dataTable.Rows.Add(item.Id, item.Value);
                    }
                }
            }
            return dataTable;
        }

        public async Task<int> UpdateAsync(string query)
        {
            // For RavenDB, update can be handled the same as insert (overwriting any existing document)
            return await InsertAsync(query);
        }

        public async Task ExecuteCommandAsync(string query)
        {
            // No additional command execution logic for now.
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() => await Task.CompletedTask;
        public async Task CommitTransactionAsync() => await Task.CompletedTask;
        public async Task RollbackTransactionAsync() => await Task.CompletedTask;
    }

}