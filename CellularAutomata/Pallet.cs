using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CellularAutomata
{
    public class Pallet
    {
        private Color[] colors;
        public Color this[int key]
        {
            get { return colors[key]; }
            set
            {
                Pens[key] = new Pen(new SolidBrush(value));
                colors[key] = value;
            }
        }
        public Pen[] Pens
        {
            get;
            private set;
        }
        public Pallet() 
        {
            Pens = new Pen[99];
            colors = new Color[99];
        }
    }
}
