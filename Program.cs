using DBFusion.Factories;
using DBFusion.Models;
using DBFusion.Utilities;
using System;
using System.Data;
using System.Threading.Tasks;

using System;
using System.Data;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Welcome to the Database Wrapper Application!");

            // Step 1: Prompt the user to select a database type
            Console.WriteLine("Please select a database type:");
            Console.WriteLine("1. SQL Server");
            Console.WriteLine("2. MySQL");
            Console.WriteLine("3. PostgreSQL");
            Console.WriteLine("4. SQLite");
            Console.WriteLine("5. MongoDB");
            Console.WriteLine("6. Microsoft Access");

            var choice = Console.ReadLine();

            // Map user input to DatabaseType
            DatabaseType selectedDbType = choice switch
            {
                "1" => DatabaseType.SQL_SERVER,
                "2" => DatabaseType.MYSQL,
                "3" => DatabaseType.POSTGRESQL,
                "4" => DatabaseType.SQLITE,
                "5" => DatabaseType.MONGODB,
                "6" => DatabaseType.ACCESS,
                _ => throw new InvalidOperationException("Invalid database type selected.")
            };

            // Step 2: Prompt the user to provide connection details
            Console.WriteLine("Enter your database connection string:");
            var connectionString = Console.ReadLine();

            Console.WriteLine("Enter your username:");
            var username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();

            // Step 3: Create a DbAuth object
            var auth = new DbAuth(username, password, connectionString);

            // Validate the authentication details
            auth.ValidateAuthDetails();
            Console.WriteLine("Authentication details validated successfully!");

            // Step 4: Create a database instance using the factory
            var database = DatabaseFactory.GetDatabase(selectedDbType, auth);
            Console.WriteLine($"Initializing connection to {selectedDbType}...");

            // Step 5: Connect to the database
            var isConnected = await database.ConnectAsync();
            if (!isConnected)
            {
                throw new InvalidOperationException("Failed to connect to the database.");
            }
            Console.WriteLine("Database connection established successfully!");

            // Step 6: Perform a sample operation (Insert and Select)
            Console.WriteLine("Performing a sample INSERT operation...");
            await database.InsertAsync("INSERT INTO test_table (column1, column2) VALUES ('value1', 'value2')");
            Console.WriteLine("INSERT operation completed.");

            Console.WriteLine("Performing a sample SELECT operation...");
            var results = await database.SelectAsync("SELECT * FROM test_table");

            Console.WriteLine("Retrieved data:");
            foreach (DataRow row in results.Rows)
            {
                Console.WriteLine(string.Join(", ", row.ItemArray));
            }

            // Step 7: Disconnect from the database
            Console.WriteLine("Disconnecting from the database...");
            await database.DisconnectAsync();
            Console.WriteLine("Disconnected successfully.");
        }
        catch (Exception ex)
        {
            // Step 8: Handle errors
            ErrorHandler.HandleException(ex, "Main Program");
        }
    }
}