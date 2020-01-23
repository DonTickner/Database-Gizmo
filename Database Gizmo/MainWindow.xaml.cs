using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Database_Gizmo.Forms;
using Database_Gizmo.Structure;

namespace Database_Gizmo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GizmoViewModel GizmoViewModel;
        public MainWindow()
        {
            this.DataContext = GizmoViewModel = new GizmoViewModel();

            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!GizmoViewModel.ConnectionStrings.Any())
            {
                MessageBox.Show(
                    "No ConnectionStrings have been configured. Please configure a ConnectionString and re-start the application.",
                    "No Connection Strings", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            if (GizmoViewModel.ConnectionStrings.Count() > 1)
            {
                SelectConnectionStringForm connectionStringForm = new SelectConnectionStringForm(GizmoViewModel);
                connectionStringForm.ShowDialog();

                if (connectionStringForm.DialogResult == null)
                {
                    Application.Current.Shutdown();
                }

                if (!connectionStringForm.DialogResult.Value)
                {
                    Application.Current.Shutdown();
                }

                GizmoViewModel.SetCurrentConnection(connectionStringForm.ConfiguredConnection);
                return;
            }

            GizmoViewModel.SetCurrentConnection(GizmoViewModel.ConnectionStrings.FirstOrDefault());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            GizmoViewModel.TestMethod();
        }
    }
}
