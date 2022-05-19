using System;
using CellularAutomata;
using System.Drawing;
using System.IO;

namespace CATEST
{
    class Program
    {
        static void Main(string[] args)
        {
            //Rules r = new Rules(2, "[0]([1]==3):1;[1]([1]<2):0;[1]([1]>3):0;");

            Rules r = Rules.LoadFromFile(File.Open("C:\\repos\\GameOfLife.car2",FileMode.Open));
            Grid grid = new Grid(new Tuple<uint, uint>( 64, 64), r, 0);
            grid.SetCell(20, 20, 1);
            grid.SetCell(21, 20, 1);
            grid.SetCell(22, 20, 1);
            grid.SetCell(22, 19, 1);
            grid.SetCell(21, 18, 1);
            Random random = new Random();
            
            for (int i = 1; i <= 10; i++)
            {
                Image img = grid.UpdateGraphics();
                img.Save(String.Format("C:\\repos\\{0}.png",i));
                grid.Step();
            }
        }
    }
}
