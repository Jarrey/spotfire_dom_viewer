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

namespace SpotfireDomViewer
{
    using Spotfire.Dxp.Application;
    using Spotfire.Dxp.Application.Calculations;

    /// <summary>
    /// Interaction logic for DomViewerWindow.xaml
    /// </summary>
    public partial class DomViewerWindow : Window
    {
        public DomViewerWindow(DomViewerViewModel vm)
        {
            InitializeComponent();

            this.DataContext = vm;
        }

        private void InvokeMethod(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Warning, invoke method crash current system! Do you want to continue?",
                "Warinig",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                var button = sender as Button;
                if (button != null)
                {
                    var method = button.DataContext as MethodNode;
                    if (method != null)
                    {
                        var obj = method.Invoke();
                        if (obj != null)
                        {
                            var vm = new DomViewerViewModel(obj);
                            var win = new DomViewerWindow(vm) { Title = obj.ToString() };
                            win.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Null", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }
    }
}
