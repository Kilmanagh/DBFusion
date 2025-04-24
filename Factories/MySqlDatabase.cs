using DBFusion.Interfaces;
using DBFusion.Models;
using MySql.Data.MySqlClient;
using System.Data;


namespace DBFusion.Factories
    {
        public class MySqlDatabase : IDatabase
        {
            private readonly string _connectionString;
            private MySqlConnection _connection;
            private MySqlTransaction _transaction;

            public MySqlDatabase(DbAuth auth)
            {
                if (auth == null)
                    throw new ArgumentNullException(nameof(auth), "Authentication details cannot be null.");

                // Use decrypted password for the connection string
                _connectionString = auth.ConnectionString.Replace("your_password", auth.GetDecryptedPassword());
            }

            // Connect asynchronously to the MySQL database
            public async Task<bool> ConnectAsync()
            {
                _connection = new MySqlConnection(_connectionString);
                await _connection.OpenAsync();
                return _connection.State == ConnectionState.Open;
            }

            // Disconnect from the MySQL database
            public async Task DisconnectAsync()
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    await _connection.CloseAsync();
                    _connection.Dispose();
                    _connection = null;
                }
            }

            // Insert operation
            public async Task<int> InsertAsync(string query)
            {
                using (var command = new MySqlCommand(query, _connection, _transaction))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }

            // Delete operation
            public async Task<int> DeleteAsync(string query)
            {
                using (var command = new MySqlCommand(query, _connection, _transaction))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }

            // Select operation
            public async Task<DataTable> SelectAsync(string query)
            {
                using (var command = new MySqlCommand(query, _connection, _transaction))
                using (var adapter = new MySqlDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }

            // Update operation
            public async Task<int> UpdateAsync(string query)
            {
                using (var command = new MySqlCommand(query, _connection, _transaction))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }

            // Execute arbitrary command
            public async Task ExecuteCommandAsync(string query)
            {
                using (var command = new MySqlCommand(query, _connection, _transaction))
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
                _transaction = await Task.FromResult(_connection.BeginTransaction()); // Start transaction
            }

            // Commit transaction
            public async Task CommitTransactionAsync()
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("No active transaction to commit.");
                }
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
                await Task.CompletedTask;
            }

            // Rollback transaction
            public async Task RollbackTransactionAsync()
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("No active transaction to roll back.");
                }
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
                await Task.CompletedTask;
            }
        }
    }

    /*
    public class MySqlDatabase : IDatabase
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;
        private MySqlTransaction _transaction;

        public MySqlDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        // Connect asynchronously to the MySQL database
        public async Task<bool> ConnectAsync()
        {
            _connection = new MySqlConnection(_connectionString);
            await _connection.OpenAsync();
            return _connection.State == ConnectionState.Open;
        }

        // Disconnect from the MySQL database
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
            using (var command = new MySqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Delete operation
        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Select operation
        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction))
            using (var adapter = new MySqlDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Update operation
        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Execute arbitrary command
        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction))
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
            _transaction = await Task.FromResult(_connection.BeginTransaction()); // Start transaction
        }

        // Commit transaction
        public async Task CommitTransactionAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }

        // Rollback transaction
        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }
    } */

