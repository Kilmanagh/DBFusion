using DBFusion.Interfaces;
using DBFusion.Models;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Databases;

namespace DBFusion.Factories
{
    public class DynamoDbDatabase : IDatabase
    {
        private readonly DBFusion.Databases.DynamoDbDatabase _db;
        public DynamoDbDatabase(DbAuth auth) => _db = new DBFusion.Databases.DynamoDbDatabase(auth);
        public Task<bool> ConnectAsync() => _db.ConnectAsync();
        public Task DisconnectAsync() => _db.DisconnectAsync();
        public Task<int> InsertAsync(string query) => _db.InsertAsync(query);
        public Task<int> DeleteAsync(string query) => _db.DeleteAsync(query);
        public Task<DataTable> SelectAsync(string query) => _db.SelectAsync(query);
        public Task<int> UpdateAsync(string query) => _db.UpdateAsync(query);
        public Task ExecuteCommandAsync(string query) => _db.ExecuteCommandAsync(query);
        public Task BeginTransactionAsync() => _db.BeginTransactionAsync();
        public Task CommitTransactionAsync() => _db.CommitTransactionAsync();
        public Task RollbackTransactionAsync() => _db.RollbackTransactionAsync();
    }
}