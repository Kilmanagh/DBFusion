using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;

namespace DBFusion.Databases
{
    using IBM.Data.Db2;

    public class IBMDb2Database : IDatabase
    {
        private readonly string _connectionString;
        private DB2Connection _connection;
        private DB2Transaction _transaction;

        public IBMDb2Database(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _connection = new DB2Connection(_connectionString);
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
            using (var command = new DB2Command(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new DB2Command(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new DB2Command(query, _connection, _transaction))
            using (var adapter = new DB2DataAdapter(command))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new DB2Command(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new DB2Command(query, _connection, _transaction))
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