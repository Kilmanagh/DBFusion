using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFusion.Interfaces
{
    public interface IDatabase
    {
        // Connect to the database asynchronously
        Task<bool> ConnectAsync();

        // Standard CRUD operations
        Task<int> InsertAsync(string query);      // For INSERT operations
        Task<int> DeleteAsync(string query);      // For DELETE operations
        Task<DataTable> SelectAsync(string query); // For SELECT operations
        Task<int> UpdateAsync(string query);      // For UPDATE operations

        // Execute arbitrary commands
        Task ExecuteCommandAsync(string query);

        // Transaction management
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Disconnect from the database
        Task DisconnectAsync();
    }
}
