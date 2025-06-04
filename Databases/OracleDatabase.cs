using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using Oracle.ManagedDataAccess.Client;

namespace DBFusion.Databases
{
    public class OracleDatabase : IDatabase
    {
        private readonly string _connectionString;
        private OracleConnection _connection;
        private OracleTransaction _transaction;

        public OracleDatabase(DbAuth auth)
        {
            _connectionString = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _connection = new OracleConnection(_connectionString);
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
            using (var command = new OracleCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(string query)
        {
            using (var command = new OracleCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            using (var command = new OracleCommand(query, _connection, _transaction))
            using (var reader = await command.ExecuteReaderAsync())
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
        }

        public async Task<int> UpdateAsync(string query)
        {
            using (var command = new OracleCommand(query, _connection, _transaction))
                return await command.ExecuteNonQueryAsync();
        }

        public async Task ExecuteCommandAsync(string query)
        {
            using (var command = new OracleCommand(query, _connection, _transaction))
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