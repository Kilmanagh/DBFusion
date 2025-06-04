using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;

namespace DBFusion.Databases
{
    public class DynamoDbDatabase : IDatabase
    {
        public DynamoDbDatabase(DbAuth auth) { /* Initialize connection here */ }
        public async Task<bool> ConnectAsync() => await Task.FromResult(true);
        public async Task DisconnectAsync() => await Task.CompletedTask;
        public async Task<int> InsertAsync(string query) => await Task.FromResult(0);
        public async Task<int> DeleteAsync(string query) => await Task.FromResult(0);
        public async Task<DataTable> SelectAsync(string query) => await Task.FromResult(new DataTable());
        public async Task<int> UpdateAsync(string query) => await Task.FromResult(0);
        public async Task ExecuteCommandAsync(string query) => await Task.CompletedTask;
        public async Task BeginTransactionAsync() => await Task.CompletedTask;
        public async Task CommitTransactionAsync() => await Task.CompletedTask;
        public async Task RollbackTransactionAsync() => await Task.CompletedTask;
    }
}