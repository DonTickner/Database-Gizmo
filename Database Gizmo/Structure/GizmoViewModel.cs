using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Database_Gizmo.Structure
{
    public class GizmoViewModel
    {
        #region Connections

        private const string SQLClientProviderName = "System.Data.SqlClient";

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
            set
            {
                _connectionStringSettings = value; 
                SignalDataBindingChange(ConnectionStrings);
            }
        }

        #endregion

        public GizmoViewModel()
        {
            _connectionStringSettings = new List<ConfiguredConnection>();

            LoadConfiguration();
        }

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

                if (!string.Equals(SQLClientProviderName, connectionStringSettings.ProviderName))
                {
                    continue;
                }

                _connectionStringSettings.Add(new ConfiguredConnection(connectionStringSettings));
            }

            SignalDataBindingChange(ConnectionStrings);
        }

        #region Data Binding
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Triggers the UI to update all necessary control or UI elements bound to the entity passed in.
        /// </summary>
        /// <param name="property">The property that is to be updated.</param>
        private void SignalDataBindingChange(object property)
        {
            OnPropertyChanged(nameof(property));
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

        #endregion
    }
}
