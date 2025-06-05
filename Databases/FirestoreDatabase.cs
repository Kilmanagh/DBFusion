using System;
using System.Data;
using System.Threading.Tasks;
using DBFusion.Interfaces;
using DBFusion.Models;
using Google.Cloud.Firestore;

namespace DBFusion.Databases
{
    public class FirestoreDatabase : IDatabase
    {
        private readonly string _projectId;
        private FirestoreDb _db;
        private const string CollectionName = "KeyValueStore";

        public FirestoreDatabase(DbAuth auth)
        {
            // We expect the connection string to contain the Firestore project ID.
            _projectId = auth.ConnectionString;
        }

        public async Task<bool> ConnectAsync()
        {
            _db = FirestoreDb.Create(_projectId);
            // Firestore doesn't have a connectivity test so we assume the connection is valid.
            return true;
        }

        public async Task DisconnectAsync()
        {
            // Disconnection is not applicable to Firestore.
            await Task.CompletedTask;
        }

        public async Task<int> InsertAsync(string query)
        {
            // Expecting a query like "SET key value"
            var parts = query.Split(' ');
            if (parts.Length >= 3 && parts[0].ToUpper() == "SET")
            {
                string key = parts[1];
                string value = parts[2];
                DocumentReference docRef = _db.Collection(CollectionName).Document(key);
                await docRef.SetAsync(new { value });
                return 1;
            }
            return 0;
        }

        public async Task<int> DeleteAsync(string query)
        {
            // Expecting a query like "DEL key"
            var parts = query.Split(' ');
            if (parts.Length >= 2 && parts[0].ToUpper() == "DEL")
            {
                string key = parts[1];
                DocumentReference docRef = _db.Collection(CollectionName).Document(key);
                await docRef.DeleteAsync();
                return 1;
            }
            return 0;
        }

        public async Task<DataTable> SelectAsync(string query)
        {
            // Expecting a query like "GET key"
            var parts = query.Split(' ');
            var dataTable = new DataTable();
            dataTable.Columns.Add("Key");
            dataTable.Columns.Add("Value");

            if (parts.Length >= 2 && parts[0].ToUpper() == "GET")
            {
                string key = parts[1];
                DocumentReference docRef = _db.Collection(CollectionName).Document(key);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    var docData = snapshot.ToDictionary();
                    string value = docData.ContainsKey("value") ? docData["value"].ToString() : "";
                    dataTable.Rows.Add(key, value);
                }
            }
            return dataTable;
        }

        public async Task<int> UpdateAsync(string query)
        {
            // For simplicity update is the same as insert.
            return await InsertAsync(query);
        }

        public async Task ExecuteCommandAsync(string query)
        {
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync() => await Task.CompletedTask;
        public async Task CommitTransactionAsync() => await Task.CompletedTask;
        public async Task RollbackTransactionAsync() => await Task.CompletedTask;
    }
}