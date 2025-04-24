using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;


namespace DBFusion.Databases
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class SqlDatabase : IDatabase
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public SqlDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        // Connect asynchronously to the database
        public async Task<bool> ConnectAsync()
        {
            _connection = new SqlConnection(_connectionString);
            await _connection.OpenAsync();
            return _connection.State == ConnectionState.Open;
        }

        // Disconnect from the database
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
            using (var command = new SqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Delete operation
        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new SqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Select operation
        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new SqlCommand(query, _connection, _transaction))
            using (var adapter = new SqlDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Update operation
        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new SqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Execute arbitrary SQL command
        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new SqlCommand(query, _connection, _transaction))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        // Begin transaction
        public async Task BeginTransactionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection must be open to begin a transaction.");
            }
            _transaction = _connection.BeginTransaction();
            await Task.CompletedTask; // Placeholder for async transaction support
        }

        // Commit transaction
        public async Task CommitTransactionAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask; // Placeholder for async transaction support
        }

        // Rollback transaction
        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask; // Placeholder for async transaction support
        }
    }
}

