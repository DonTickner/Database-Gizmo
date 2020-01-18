using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Database_Gizmo.Structure;

namespace Database_Gizmo.Forms
{
    /// <summary>
    /// Interaction logic for SelectConnectionString.xaml
    /// </summary>
    public partial class SelectConnectionStringForm : ExtendedWindow
    {
        private ConfiguredConnection _configuredConnection;
        /// <summary>
        /// The Dialog's currently selected <see cref="Structure.ConfiguredConnection"/>
        /// </summary>
        public ConfiguredConnection ConfiguredConnection
        {
            get { return _configuredConnection; }
        }

        public SelectConnectionStringForm(GizmoViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ConnectionsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConnectionsListView.SelectedItem is ConfiguredConnection selectedConfiguredConnection)
            {
                _configuredConnection = selectedConfiguredConnection;
                TestConnectionButton.IsEnabled = true;
                return;
            }

            TestConnectionButton.IsEnabled = false;
            _configuredConnection = null;
        }

        private void TestConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!TestConnectionButton.IsEnabled)
            {
                return;
            }

            if (null == _configuredConnection)
            {
                ShowErrorMessage("Please select a connection from the List View before attempt to test.", "No Connection Selected");
            }


            try
            {
                SqlConnection testConnection = new SqlConnection(_configuredConnection.ConnectionString);

                if (testConnection.State == ConnectionState.Open)
                {
                    MessageBox.Show("Connection tested successfully!", "Success!", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                ShowErrorMessage($"An unknown error occurred. Please check the connection string and try again.", "Unknown Error");
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception, "test connection");
            }
        }
    }
}
