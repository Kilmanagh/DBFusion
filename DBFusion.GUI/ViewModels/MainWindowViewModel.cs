using Avalonia.Collections;
using DBFusion.Factories;
using DBFusion.Interfaces;
using DBFusion.Models; // Added for DbAuth
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics; // For Debug.WriteLine (optional)

// It's good practice to ensure RelayCommand updates happen on the UI thread if needed.
// For Avalonia, this might involve `Avalonia.Threading.Dispatcher.UIThread.Post` or `InvokeAsync`.
// For simplicity in this context, direct invocation is used, but in complex apps, consider thread affinity for UI updates.
// For `CanExecuteChanged`, it's often fine, but if `CanExecute` depends on async data, care is needed.
// Let's assume `App.Current.Dispatcher.InvokeAsync` for RaiseCanExecuteChanged as a good practice.
// This requires `using Avalonia;` in a context where `App.Current` is valid, or passing dispatcher.
// For a ViewModel, direct use of App.Current might be a slight coupling.
// A common pattern is to have a static class or service provide UI thread dispatching.
// For now, let's make a simplified version or ensure commands are created where dispatcher is available.
// Simplified RelayCommand for this specific subtask context:
namespace DBFusion.GUI.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        // Ensure UI thread for CanExecuteChanged updates if called from non-UI thread
        public void RaiseCanExecuteChanged() =>
            Avalonia.Threading.Dispatcher.UIThread.Post(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IDatabase _currentDatabase;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<DatabaseType> _databaseTypes;
        public ObservableCollection<DatabaseType> DatabaseTypes
        {
            get => _databaseTypes;
            set { _databaseTypes = value; OnPropertyChanged(); }
        }

        private DatabaseType _selectedDatabaseType;
        public DatabaseType SelectedDatabaseType
        {
            get => _selectedDatabaseType;
            set { _selectedDatabaseType = value; OnPropertyChanged(); UpdateConnectionStringHint(); }
        }

        private string _connectionString;
        public string ConnectionString
        {
            get => _connectionString;
            set { _connectionString = value; OnPropertyChanged(); }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _queryText = "SELECT * FROM your_table;"; // Default query
        public string QueryText
        {
            get => _queryText;
            set { _queryText = value; OnPropertyChanged(); UpdateCommandStates(); } // Update states if query emptiness affects CanExecute
        }

        private DataTable _dataGridResults;
        public DataTable DataGridResults
        {
            get => _dataGridResults;
            set { _dataGridResults = value; OnPropertyChanged(); }
        }

        private string _textResults;
        public string TextResults
        {
            get => _textResults;
            set { _textResults = value; OnPropertyChanged(); }
        }

        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand InsertCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ExecuteCommand { get; }
        public ICommand BeginTransactionCommand { get; }
        public ICommand CommitTransactionCommand { get; }
        public ICommand RollbackTransactionCommand { get; }

        public MainWindowViewModel()
        {
            DatabaseTypes = new ObservableCollection<DatabaseType>(Enum.GetValues(typeof(DatabaseType)).Cast<DatabaseType>());
            if (DatabaseTypes.Any()) // Check if enum has values
                SelectedDatabaseType = DatabaseTypes.FirstOrDefault();
            // UpdateConnectionStringHint(); // Called by SelectedDatabaseType setter

            ConnectCommand = new RelayCommand(async param => await ConnectAsync(), param => CanConnect());
            DisconnectCommand = new RelayCommand(async param => await DisconnectAsync(), param => CanDisconnect());
            SelectCommand = new RelayCommand(async param => await SelectDataAsync(), param => CanExecuteQuery());
            InsertCommand = new RelayCommand(async param => await InsertDataAsync(), param => CanExecuteQuery());
            UpdateCommand = new RelayCommand(async param => await UpdateDataAsync(), param => CanExecuteQuery());
            DeleteCommand = new RelayCommand(async param => await DeleteDataAsync(), param => CanExecuteQuery());
            ExecuteCommand = new RelayCommand(async param => await ExecuteGenericCommandAsync(), param => CanExecuteQuery());
            BeginTransactionCommand = new RelayCommand(async param => await BeginTransactionAsync(), param => CanManageTransaction());
            CommitTransactionCommand = new RelayCommand(async param => await CommitTransactionAsync(), param => CanManageTransaction());
            RollbackTransactionCommand = new RelayCommand(async param => await RollbackTransactionAsync(), param => CanManageTransaction());

            UpdateCommandStates(); // Initial state
        }

        private bool CanConnect() => _currentDatabase == null;
        private bool CanDisconnect() => _currentDatabase != null;
        private bool CanExecuteQuery() => _currentDatabase != null && !string.IsNullOrWhiteSpace(QueryText);
        private bool CanManageTransaction() => _currentDatabase != null;


        private void UpdateConnectionStringHint()
        {
            if (SelectedDatabaseType == DatabaseType.SQLITE)
            {
                // Only set hint if CS is empty or already an SQLite hint, to avoid overwriting user input
                if (string.IsNullOrWhiteSpace(ConnectionString) || ConnectionString.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    ConnectionString = "Data Source=mydatabase.db;";
                }
            }
        }

        private async Task ConnectAsync()
        {
            StatusMessage = $"Connecting to {SelectedDatabaseType}...";
            TextResults = string.Empty;
            DataGridResults = null;
            try
            {
                // Use provided connection string if available, otherwise construct from user/pass (though DbAuth expects CS)
                // The current DbAuth(user,pass,cs) seems to prioritize CS if provided.
                var auth = new DbAuth(Username, Password, ConnectionString);
                auth.ValidateAuthDetails();

                _currentDatabase = DatabaseFactory.GetDatabase(SelectedDatabaseType, auth);

                bool connected = await _currentDatabase.ConnectAsync();
                if (connected)
                {
                    StatusMessage = $"Successfully connected to {SelectedDatabaseType}.";
                }
                else
                {
                    _currentDatabase = null;
                    StatusMessage = $"Failed to connect to {SelectedDatabaseType}. Check connection details and database availability.";
                }
            }
            catch (NotSupportedException nsex)
            {
                _currentDatabase = null;
                StatusMessage = $"Configuration error: {SelectedDatabaseType} is not supported or enabled in the DatabaseFactory.";
                TextResults = $"Details: {nsex.Message}

Ensure this database type is handled in DatabaseFactory.cs and all required client libraries are present.";
                Debug.WriteLine(nsex);
            }
            catch (ArgumentException aex)
            {
                _currentDatabase = null;
                StatusMessage = $"Connection details error: {aex.Message}";
                TextResults = $"Details: {aex.Message}

Please check your connection string, username, and password.";
                Debug.WriteLine(aex);
            }
            catch (Exception ex)
            {
                _currentDatabase = null;
                StatusMessage = $"Connection error: {ex.Message}";
                TextResults = $"An unexpected error occurred while connecting:
{ex.ToString()}";
                Debug.WriteLine(ex);
            }
            finally
            {
                UpdateCommandStates();
            }
        }

        private async Task DisconnectAsync()
        {
            StatusMessage = "Disconnecting...";
            try
            {
                if (_currentDatabase != null)
                {
                    await _currentDatabase.DisconnectAsync();
                }
                StatusMessage = "Disconnected successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error during disconnect: {ex.Message}";
                TextResults = ex.ToString();
            }
            finally
            {
                _currentDatabase = null;
                UpdateCommandStates();
            }
        }

        private async Task ExecuteQueryAsyncWrapper(Func<IDatabase, Task> specificQueryAction, string successMessageBase)
        {
            if (!CanExecuteQuery())
            {
                StatusMessage = "Cannot execute query: Not connected or query is empty.";
                return;
            }
            StatusMessage = "Executing query...";
            TextResults = string.Empty; // Clear previous text results
            // DataGridResults = null; // Clear previous grid results, unless SELECT is special

            try
            {
                await specificQueryAction(_currentDatabase);
                // Success message will be set by specific actions
            }
            catch (Exception ex)
            {
                StatusMessage = $"Query error: {ex.Message}";
                TextResults = ex.ToString();
                DataGridResults = null; // Clear grid on error
            }
        }


        private async Task SelectDataAsync()
        {
            await ExecuteQueryAsyncWrapper(async (db) =>
            {
                DataGridResults = null; // Clear previous results first
                DataTable dt = await db.SelectAsync(QueryText);
                DataGridResults = dt; // Assign new results
                StatusMessage = $"SELECT query executed. Rows returned: {dt?.Rows?.Count ?? 0}";
                if (dt == null || dt.Rows.Count == 0)
                {
                    TextResults = "Query executed, but no data was returned.";
                }
            }, "SELECT query executed");
        }

        private async Task InsertDataAsync()
        {
            await ExecuteQueryAsyncWrapper(async (db) =>
            {
                DataGridResults = null;
                int affected = await db.InsertAsync(QueryText);
                StatusMessage = $"INSERT operation completed. Records affected: {affected}";
                TextResults = $"INSERT statement affected {affected} record(s).";
            }, "INSERT operation completed");
        }

        private async Task UpdateDataAsync()
        {
            await ExecuteQueryAsyncWrapper(async (db) =>
            {
                DataGridResults = null;
                int affected = await db.UpdateAsync(QueryText);
                StatusMessage = $"UPDATE operation completed. Records affected: {affected}";
                TextResults = $"UPDATE statement affected {affected} record(s).";
            }, "UPDATE operation completed");
        }

        private async Task DeleteDataAsync()
        {
            await ExecuteQueryAsyncWrapper(async (db) =>
            {
                DataGridResults = null;
                int affected = await db.DeleteAsync(QueryText);
                StatusMessage = $"DELETE operation completed. Records affected: {affected}";
                TextResults = $"DELETE statement affected {affected} record(s).";
            }, "DELETE operation completed");
        }

        private async Task ExecuteGenericCommandAsync()
        {
             await ExecuteQueryAsyncWrapper(async (db) =>
            {
                DataGridResults = null;
                await db.ExecuteCommandAsync(QueryText);
                StatusMessage = "Generic command executed successfully.";
                TextResults = "Command executed. Check database for results/effects if applicable.";
            }, "Generic command executed");
        }

        private async Task ManageTransactionAsync(Func<IDatabase, Task> transactionAction, string actionName)
        {
            if (!CanManageTransaction())
            {
                StatusMessage = "Cannot manage transaction: Not connected.";
                return;
            }
            StatusMessage = $"{actionName} transaction...";
            TextResults = string.Empty;
            try
            {
                await transactionAction(_currentDatabase);
                StatusMessage = $"Transaction {actionName.ToLower()} successful.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Transaction error ({actionName}): {ex.Message}";
                TextResults = ex.ToString();
            }
        }

        private async Task BeginTransactionAsync() => await ManageTransactionAsync((db) => db.BeginTransactionAsync(), "Begin");
        private async Task CommitTransactionAsync() => await ManageTransactionAsync((db) => db.CommitTransactionAsync(), "Commit");
        private async Task RollbackTransactionAsync() => await ManageTransactionAsync((db) => db.RollbackTransactionAsync(), "Rollback");

        private void UpdateCommandStates()
        {
            // This ensures buttons enable/disable based on connection state and query text
            (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DisconnectCommand as RelayCommand)?.RaiseCanExecuteChanged();

            var queryCommands = new[] { SelectCommand, InsertCommand, UpdateCommand, DeleteCommand, ExecuteCommand };
            foreach(var cmd in queryCommands)
            {
                (cmd as RelayCommand)?.RaiseCanExecuteChanged();
            }

            var transactionCommands = new[] { BeginTransactionCommand, CommitTransactionCommand, RollbackTransactionCommand };
            foreach(var cmd in transactionCommands)
            {
                (cmd as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }
}
