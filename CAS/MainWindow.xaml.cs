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

namespace CAS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainMenu mainMenuInstance;
       // private Sim CellularAutomata;
        public MainWindow()
        {
            InitializeComponent();
            mainMenuInstance = new MainMenu(this);
            MainWindowFrame.Content = mainMenuInstance;
        }
        public void SwitchToMainMenu()
        {
            MainWindowFrame.Content = mainMenuInstance;
        }
        public void SwitchToCAS()
        {
            //if(CellularAutomata == null)
            {
               // CellularAutomata = new Sim(this);
            }
           // MainWindowFrame.Content = CellularAutomata;
        }
    }
}
