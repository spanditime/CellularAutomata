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
using CellularAutomata;

namespace CAS
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private MainWindow parent;
        private CellularAutomata.Grid GOL;
        Rectangle[,] grid;
        SolidColorBrush LiveCellBrush;
        SolidColorBrush DeadCellBrush;
        private System.Windows.Threading.DispatcherTimer TTimer = new System.Windows.Threading.DispatcherTimer();
        private Int32 counter = 0;
        public MainMenu(MainWindow w)
        {
            this.parent = w;
            GOL = new CellularAutomata.Grid(new Tuple<int, int, int, int>(0, 64, 0, 64), new Rules(2, "[0]([1]==3):1;[1]([1] < 2):0;[1]([1] > 3):0; "),0);//game of life rules
            grid = new Rectangle[64, 64];
            LiveCellBrush = new SolidColorBrush();
            DeadCellBrush = new SolidColorBrush();
            DeadCellBrush.Color = Color.FromRgb(0xFA,0xFA,0xEE);//#FAFAEE
            LiveCellBrush.Color = Color.FromRgb(0x04,0x03,0x10);//040310
            TTimer.Interval = TimeSpan.FromSeconds(0.1);
            TTimer.Tick += updateCA;
            reloadCA();
            InitializeComponent();
            TTimer.IsEnabled = true;
        }

        private void reloadCA()
        {
            Random rand = new Random();
            for(int x = 0; x< 64; x++)
                for (int y = 0; y < 64; y++)
                {
                    GOL.SetCell(x, y, (byte)rand.Next(0, 2));

                }
        }
        private void reloadGrid()
        {
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                {
                    grid[x,y].Fill = GOL.GetCell(x, y) == 1 ? LiveCellBrush : DeadCellBrush;
                }
        }
        private void updateCA(object sender, EventArgs e)
        {
            counter++;
            if (counter > 300)
            {
                counter = 0;
                reloadCA();
            }
            GOL.Step();
            reloadGrid();
        }
        private void changeGrid()
        {
            double max = BackgroundCanvas.RenderSize.Height > BackgroundCanvas.RenderSize.Width ? BackgroundCanvas.RenderSize.Height : BackgroundCanvas.RenderSize.Width;
            double size = max / 64;
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                {
                    grid[x, y].Height = size;
                    grid[x, y].Width = size;
                    double cx = (BackgroundCanvas.RenderSize.Width - max) / 2 + x * size;
                    double cy = (BackgroundCanvas.RenderSize.Height - max) / 2 + y * size;
                    Canvas.SetLeft(grid[x, y], cx);
                    Canvas.SetTop(grid[x, y], cy);
                }
        }
        private void StartButtonPressed(object sender, RoutedEventArgs e)
        {
            parent.SwitchToCAS();
        }

        private void SettingsButtonPressed(object sender, RoutedEventArgs e)
        {

        }

        private void OnCanvasInit(object sender, EventArgs e)
        {
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                {
                    grid[x, y] = new Rectangle();
                    grid[x, y].Fill = GOL.GetCell(x,y)==1 ? LiveCellBrush : DeadCellBrush;
                    BackgroundCanvas.Children.Add(grid[x, y]);
                }
            changeGrid();

        }

        private void OnCanvasResize(object sender, SizeChangedEventArgs e)
        {
            changeGrid();
        }
    }
}
