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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new GizmoViewModel();
            SelectConnectionStringForm connectionStringForm = new SelectConnectionStringForm(this.DataContext as GizmoViewModel);
            connectionStringForm.ShowDialog();

            if (connectionStringForm.DialogResult == null)
            {
                Application.Current.Shutdown();
            }

            if (!connectionStringForm.DialogResult.Value)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
