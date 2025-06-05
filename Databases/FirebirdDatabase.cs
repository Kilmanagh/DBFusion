using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using FirebirdSql.Data.FirebirdClient;

namespace DBFusion.Databases
{
    public class FirebirdDatabase : IDatabase
    {
        private readonly string _connectionString;
        private FbConnection _connection;
        private FbTransaction _transaction;

        public FirebirdDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _connection = new FbConnection(_connectionString);
            await _connection.OpenAsync();
            return _connection.State == ConnectionState.Open;
        }

        public async Task DisconnectAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                await _connection.CloseAsync();
        }

        public async Task<int> InsertAsync(string query)
        {
            using (var command = new FbCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new FbCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new FbCommand(query, _connection, _transaction))
            using (var adapter = new FbDataAdapter(command))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new FbCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new FbCommand(query, _connection, _transaction))
                await command.ExecuteNonQueryAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = _connection.BeginTransaction();
            await Task.CompletedTask;
        }

        public async Task CommitTransactionAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }

        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }
    }
}