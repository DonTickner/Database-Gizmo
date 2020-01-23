using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
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
        private GizmoViewModel _gizmoViewModel;

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
            _gizmoViewModel = viewModel;
            this.DataContext = _gizmoViewModel;

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
                SelectConnectionButton.IsEnabled = true;
                return;
            }

            TestConnectionButton.IsEnabled = false;
            SelectConnectionButton.IsEnabled = false;
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

            SQLConnectionTestResult testResult = _gizmoViewModel.TestSQLConnection(_configuredConnection.ConnectionString);

            if (testResult.Successful)
            {
                MessageBox.Show($"{testResult.ResultMessage}", "Test successful!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            ShowErrorMessage($"The test was not successful. The error message is:{Environment.NewLine}{testResult.ResultMessage}", "Test Failed");
        }

        private void SelectConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SelectConnectionButton.IsEnabled)
            {
                return;
            }

            if (null == _configuredConnection)
            {
                ShowErrorMessage("Please select a connection from the List View before attempt to test.", "No Connection Selected");
            }

            SQLConnectionTestResult testResult = _gizmoViewModel.TestSQLConnection(_configuredConnection.ConnectionString);

            if (testResult.Successful)
            {
                this.DialogResult = true;
                this.Close();
                return;
            }

            ShowErrorMessage($"There was an error with the connection. Please check the connection string and try again. The error returned is:{Environment.NewLine}{testResult.ResultMessage}", "Connection Error");
        }

    }
}
