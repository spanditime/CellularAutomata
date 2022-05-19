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
using System.IO;
using CellularAutomata;
using Microsoft.Win32;

namespace UI.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            RulesOFD = new OpenFileDialog();
            RulesOFD.RestoreDirectory = true;
            RulesOFD.CheckFileExists = true;
            RulesSFD = new SaveFileDialog();
            RulesSFD.RestoreDirectory = true;

            GridOFD = new OpenFileDialog();
            GridOFD.RestoreDirectory = true;
            GridOFD.CheckFileExists = true;
            GridSFD = new SaveFileDialog();
            GridSFD.RestoreDirectory = true;

            PalletOFD = new OpenFileDialog();
            PalletOFD.RestoreDirectory = true;
            PalletOFD.CheckFileExists = true;
            PalletSFD = new SaveFileDialog();
            PalletSFD.RestoreDirectory = true;

        }
        Rules rules;
        CellularAutomata.Grid grid;


        bool RulesEdited;
        FileStream RulesFS;
        OpenFileDialog RulesOFD;
        SaveFileDialog RulesSFD;

        bool GirdEdited;
        FileStream GridFS;
        OpenFileDialog GridOFD;
        SaveFileDialog GridSFD;

        bool PalletEdited;
        FileStream PalletFS;
        OpenFileDialog PalletOFD;
        SaveFileDialog PalletSFD;

        private void RulesOpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (RulesFS == null && RulesEdited)
            {

            }
            bool? result = RulesOFD.ShowDialog(Window.GetWindow(this));
            if(result != null)
                if (result.Value)
                {
                    try
                    {
                        FileStream newFS = File.Open(RulesOFD.FileName,FileMode.Open);

                    } catch(Exception exc)
                    {

                    }
                }
        }

        private void RulesSaveFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RulesCreateOrEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PalletOpenFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PalletSaveFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PalletCreateOrEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PenToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectedColorRepresenter_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
