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

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BarMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                    if (this.WindowState == WindowState.Normal)
                        this.WindowState = WindowState.Maximized;
                    else
                        this.WindowState = WindowState.Normal;
                else
                    this.DragMove();
        }

        private void OnMinimizeBTNClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeBTNClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void OnCloseBTNClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
