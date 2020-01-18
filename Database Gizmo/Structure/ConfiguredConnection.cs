using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Database_Gizmo.Structure
{
    /// <summary>
    /// Holds all necessary information for an SQL Database Connection.
    /// </summary>
    public class ConfiguredConnection
    {
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        /// The configured name of the Connection.
        /// </summary>
        public string Name => _connectionStringSettings.Name;

        /// <summary>
        /// The raw text for the Connection String.
        /// </summary>
        public string ConnectionString => _connectionStringSettings.ConnectionString;

        /// <summary>
        /// The name of the Provider for the Connection.
        /// </summary>
        public string ProviderName => _connectionStringSettings.ProviderName;

        public ConfiguredConnection(ConnectionStringSettings connectionStringSettings)
        {
            _connectionStringSettings = connectionStringSettings;
        }

        public override string ToString()
        {
            return $"{_connectionStringSettings.Name} - {_connectionStringSettings.ConnectionString}";
        }
    }
}
