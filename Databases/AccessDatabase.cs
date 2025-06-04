using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using System.Data.OleDb;

namespace DBFusion.Databases
{
    public class AccessDatabase : IDatabase
    {
        private readonly string _connectionString;
        private OleDbConnection _connection;
        private OleDbTransaction _transaction;

        public AccessDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _connection = new OleDbConnection(_connectionString);
            await _connection.OpenAsync();
            return _connection.State == ConnectionState.Open;
        }

        public async Task DisconnectAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> InsertAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            using (var adapter = new OleDbDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Connection must be open to begin a transaction.");
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