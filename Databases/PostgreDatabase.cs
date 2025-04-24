using Npgsql;
using System.Data;
using DBFusion.Interfaces;
using DBFusion.Models;

namespace DBFusion.Databases
{
    

    public class PostgreDatabase : IDatabase
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public PostgreDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        // Connect asynchronously to the database
        public async Task<bool> ConnectAsync()
        {
            _connection = new NpgsqlConnection(_connectionString);
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
            using (var command = new NpgsqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Delete operation
        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new NpgsqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Select operation
        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new NpgsqlCommand(query, _connection, _transaction))
            using (var adapter = new NpgsqlDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Update operation
        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new NpgsqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Execute arbitrary command
        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new NpgsqlCommand(query, _connection, _transaction))
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