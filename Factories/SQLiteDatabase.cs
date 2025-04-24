using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFusion.Factories
{
    using DBFusion.Interfaces;
    using DBFusion.Models;
    using System.Data;
    using System.Data.SQLite;
    using System.Threading.Tasks;

    public class SQLiteDatabase : IDatabase
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public SQLiteDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        // Connect asynchronously to the SQLite database
        public async Task<bool> ConnectAsync()
        {
            _connection = new SQLiteConnection(_connectionString);
            await _connection.OpenAsync();
            return _connection.State == ConnectionState.Open;
        }

        // Disconnect from the SQLite database
        public async Task DisconnectAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }

        // Insert operation
        public async Task<int> InsertAsync(string query)
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Delete operation
        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Select operation
        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new SQLiteCommand(query, _connection))
            using (var adapter = new SQLiteDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Update operation
        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Execute arbitrary command
        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        // Transaction methods (SQLite supports transactions)
        public async Task BeginTransactionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection must be open to begin a transaction.");
            }

            using var transaction = _connection.BeginTransaction(); // Start transaction
            await Task.CompletedTask;
        }

        public async Task CommitTransactionAsync()
        {
            using var transaction = _connection.BeginTransaction(); // Commit transaction
            transaction.Commit();
            await Task.CompletedTask;
        }

        public async Task RollbackTransactionAsync()
        {
            using var transaction = _connection.BeginTransaction(); // Rollback transaction
            transaction.Rollback();
            await Task.CompletedTask;
        }
    }
}
