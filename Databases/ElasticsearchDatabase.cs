using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using Elasticsearch.Net;
using Nest;
using DBFusion.Databases;
using DBFusion.Factories;

namespace DBFusion.Databases
{
    public class ElasticsearchDatabase : IDatabase
    {
        private readonly string _connectionString;
        private ElasticClient _client;

        public ElasticsearchDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            var settings = new ConnectionSettings(new Uri(_connectionString));
            _client = new ElasticClient(settings);
            var ping = await _client.PingAsync();
            return ping.IsValid;
        }

        public async Task DisconnectAsync()
        {
            // No disconnect needed for Elasticsearch client
            await Task.CompletedTask;
        }

        public async Task<int> InsertAsync(string query)
        {
            // Not implemented: Use _client.IndexAsync<T>()
            await Task.CompletedTask;
            return 0;
        }

        public async Task<int> DeleteAsync(string query)
        {
            // Not implemented: Use _client.DeleteAsync<T>()
            await Task.CompletedTask;
            return 0;
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            // Not implemented: Use _client.SearchAsync<T>()
            await Task.CompletedTask;
            return new DataTable();
        }

        public async Task<int> UpdateAsync(string query)
        {
            // Not implemented: Use _client.UpdateAsync<T>()
            await Task.CompletedTask;
            return 0;
        }

        public async Task ExecuteCommandAsync(string query)
        {
            // Not implemented
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() => await Task.CompletedTask; // Not supported
        public async Task CommitTransactionAsync() => await Task.CompletedTask;
        public async Task RollbackTransactionAsync() => await Task.CompletedTask;
    }

    public static IDatabase Create(DatabaseTypes type, DbAuth auth)
    {
        switch (type)
        {
            case DatabaseTypes.ORACLE:
                return new OracleDatabase(auth);
            case DatabaseTypes.MARIADB:
                return new MariaDbDatabase(auth);
            case DatabaseTypes.CASSANDRA:
                return new CassandraDatabase(auth);
            case DatabaseTypes.REDIS:
                return new RedisDatabase(auth);
            case DatabaseTypes.ELASTICSEARCH:
                return new ElasticsearchDatabase(auth);
            default:
                throw new NotSupportedException($"Database type {type} is not supported.");
        }
    }
}