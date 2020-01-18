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
        private List<ConnectionStringSettings> _connectionStringSettings;

        /// <summary>
        /// The list of all available Connections Strings configured via the App.Config file.
        /// </summary>
        public List<ConnectionStringSettings> ConnectionStrings
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

        public GizmoViewModel()
        {
            _connectionStringSettings = new List<ConnectionStringSettings>();

            LoadConfiguration();
        }

        /// <summary>
        /// Loads all necessary configuration options.
        /// </summary>
        private void LoadConfiguration()
        {
            LoadConnectionStringSettings();
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

                _connectionStringSettings.Add(connectionStringSettings);
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
    }
}
