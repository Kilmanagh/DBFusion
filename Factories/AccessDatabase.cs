using DBFusion.Interfaces;
using DBFusion.Models;
using System.Data;
using System.Data.OleDb;

namespace DBFusion.Factories
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

        // Connect asynchronously to the Access database
        public async Task<bool> ConnectAsync()
        {
            _connection = new OleDbConnection(_connectionString);
            await Task.Run(() => _connection.Open());
            return _connection.State == ConnectionState.Open;
        }

        // Disconnect from the Access database
        public async Task DisconnectAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                await Task.Run(() => _connection.Close());
            }
        }

        // Insert operation
        public async Task<int> InsertAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await Task.Run(() => command.ExecuteNonQuery());
            }
        }

        // Delete operation
        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await Task.Run(() => command.ExecuteNonQuery());
            }
        }

        // Select operation
        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection))
            using (var adapter = new OleDbDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Update operation
        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                return await Task.Run(() => command.ExecuteNonQuery());
            }
        }

        // Execute arbitrary command
        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new OleDbCommand(query, _connection, _transaction))
            {
                await Task.Run(() => command.ExecuteNonQuery());
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
            await Task.CompletedTask;
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
    }
}
