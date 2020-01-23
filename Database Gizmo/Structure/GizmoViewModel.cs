using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Database_Gizmo.Structure
{
    public class GizmoViewModel: INotifyPropertyChanged
    {
        #region Constants

        private const string SqlClientProviderName = "System.Data.SqlClient";
        private const string DatabaseTableName = "Databases";
        private const string DatabaseNameColumnName = "database_name";
        private static readonly string[] SqlServerSystemDatabaseNames = { "master", "tempdb", "model", "msdb" };

        #endregion

        #region Connections

        private List<ConfiguredConnection> _connectionStringSettings;

        /// <summary>
        /// The list of all available Connections Strings configured via the App.Config file.
        /// </summary>
        public List<ConfiguredConnection> ConnectionStrings
        {
            get
            {
                return _connectionStringSettings;
            }
        }

        private ConfiguredConnection _currentConnection;
        public ConfiguredConnection CurrentConnection
        {
            get { return _currentConnection; }
        }

        #endregion

        #region Databases

        private List<string> _databaseNames;
        public List<string> DatabaseNames
        {
            get { return _databaseNames; }
        }

        private string _currentDatabase;

        public string CurrentDatabase
        {
            get { return _currentDatabase; }
            set
            {
                _currentDatabase = value;
                OnPropertyChanged(nameof(CurrentDatabase));
            }
        }

        #endregion

        public GizmoViewModel()
        {
            _connectionStringSettings = new List<ConfiguredConnection>();
            _databaseNames = new List<string>();

            LoadConfiguration();
        }

        #region Loading

        /// <summary>
        /// Loads all necessary configuration options.
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                LoadConnectionStringSettings();
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "load connection strings.");
                throw;
            }
        }

        /// <summary>
        /// Loads any <see cref="ConnectionStringSettings"/> from the <see cref="ConfigurationManager"/>.
        /// </summary>
        private void LoadConnectionStringSettings()
        {
            foreach (ConnectionStringSettings connectionStringSettings in ConfigurationManager.ConnectionStrings)
            {
                if (null == connectionStringSettings)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(connectionStringSettings.ConnectionString))
                {
                    continue;
                }

                if (!string.Equals(SqlClientProviderName, connectionStringSettings.ProviderName))
                {
                    continue;
                }

                _connectionStringSettings.Add(new ConfiguredConnection(connectionStringSettings));
            }

            OnPropertyChanged(nameof(ConnectionStrings));
        }

        #endregion

        #region Data Binding
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region UI

        /// <summary>
        /// Displays a simple error message box.
        /// </summary>
        /// <param name="e">The exception that was thrown when the error occured.</param>
        /// <param name="action">The name of the action that was being completed when the error occurred. e.g. 'Load Connections.'</param>
        private void ShowErrorMessage(Exception e, string action)
        {
            MessageBox.Show(
                $"An error occurred while attempting to {action}.{Environment.NewLine}The error message is:{Environment.NewLine}{e.Message}", $"Error: {action}", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays a simple error message box.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption for the error message box.</param>
        private void ShowErrorMessage(string message, string caption)
        {
            MessageBox.Show(
                $"{message}", $"{caption}", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region SQL Server Methods

        /// <summary>
        /// Sets the <see cref="GizmoViewModel"/>'s current <see cref="ConfiguredConnection"/>.
        /// </summary>
        /// <param name="newConnection">The new <see cref="ConfiguredConnection"/> to be used.</param>
        public void SetCurrentConnection(ConfiguredConnection newConnection)
        {
            if (null == newConnection)
            {
                ShowErrorMessage("An unknown error occurred while attempting to create a new connection. Please re-start the application and try again.", "Error with new Connection");
            }
            
            _currentConnection = newConnection;
            OnPropertyChanged(nameof(CurrentConnection));

            GetSQLDatabaseNames();
        }

        private void SetDatabaseNames(List<string> databaseNames)
        {
            _databaseNames = databaseNames;
            CurrentDatabase = databaseNames.First();

            OnPropertyChanged(nameof(DatabaseNames));
        }

        /// <summary>
        /// Tests that the connection string provided can be used, and that the database can be connected to.
        /// </summary>
        /// <param name="connectionStringToTest">The connection string to be tested.</param>
        public SQLConnectionTestResult TestSQLConnection(string connectionStringToTest)
        {
            SQLConnectionTestResult testResult = new SQLConnectionTestResult();

            try
            {
                SqlConnection testConnection = new SqlConnection(connectionStringToTest);
                testConnection.Open();

                if (testConnection.State == ConnectionState.Open)
                {
                    testConnection.Close();
                    testResult.Successful = true;
                    testResult.ResultMessage = "The test was successful!";
                }
                else
                {
                    testConnection.Close();
                    testResult.Successful = false;
                    testResult.ResultMessage =
                        "An unknown error occurred. Please check the connection string and try again.";
                }
            }
            catch (Exception e)
            {
                testResult.Successful = false;
                testResult.ResultMessage = e.Message;
            }

            return testResult;
        }

        /// <summary>
        /// Triggers the collection of the non-system SQL Database Names using the current connection.
        /// </summary>
        public void GetSQLDatabaseNames()
        {
            DataTable databasesTable = GetSQLDatabaseTable();

            if (null == databasesTable)
            {
                return;
            }

            List<string> databaseNames = databasesTable.Rows.Cast<DataRow>().Select(row => row[DatabaseNameColumnName].ToString()).Where(databaseName => !SqlServerSystemDatabaseNames.Contains(databaseName)).ToList();

            SetDatabaseNames(databaseNames);
        }

        /// <summary>
        /// Gets the content of the Database Table using the Current Connection string.
        /// </summary>
        private DataTable GetSQLDatabaseTable()
        {
            if (null == CurrentConnection)
            {
                return null;
            }

            DataTable databasesTable;

            using (var connection = new SqlConnection(CurrentConnection.ConnectionString))
            {
                connection.Open();

                databasesTable = connection.GetSchema(DatabaseTableName);

                connection.Close();
            }

            return databasesTable;
        }

        public void TestMethod()
        {
            _databaseNames.Add("TEST TEST TEST");

            OnPropertyChanged(nameof(DatabaseNames));
        }

        #endregion
    }
}
