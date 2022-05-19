using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace CellularAutomata
{
    public class Grid
    {
        public Grid(Tuple<uint,uint>Bounds, Rules Rules, Pallet Pallet, byte DefaultState) 
        {
            if (Bounds == null || Rules == null)
            {
                //exception
                return;
            }
            if (!Rules.IsStableForGivenDefaultState(DefaultState))
            {
                //exception
                return;
            }
            if (Bounds.Item1 == 0 || Bounds.Item2 == 0)
            {
                //exception
                return;
            }
            rules = Rules;
            bounds = new Tuple<uint, uint>(Bounds.Item1, Bounds.Item2);
            grid = new byte[bounds.Item1, bounds.Item2];
            for(int x = 0; x < bounds.Item1; x++)
            {
                for(int y = 0; y < bounds.Item2; y++)
                    grid[x, y] = DefaultState;
            }

            zoom = 1;
            centerpos = new Tuple<float, float>(100, 100);
            size = new Tuple<Int32, Int32>(100, 100);
            gridMutex = new Mutex();
            graphicsMutex = new Mutex();
            pallet = Pallet;
        }
        
        
        private byte[,] grid;
        private Tuple<uint, uint> bounds;
        private Rules rules;
        private Mutex gridMutex;

        private float cellSize = 5.0f;
        private float zoom;
        private Tuple<float, float> centerpos;
        private Tuple<Int32, Int32> size;
        private Mutex graphicsMutex;

        private Pallet pallet;

        public byte DefaultState { private set; get; }
        
        public void SetColor(byte State, Color clr)
        {
            pallet[State] = clr;
        }

        public byte GetCell(uint x, uint y)
        {
            if (x < bounds.Item1 && y < bounds.Item2)
            {
                gridMutex.WaitOne();
                byte returnValue = grid[x, y];
                gridMutex.ReleaseMutex();
                return returnValue;
            }
            else
            {
                throw new Exception();
                //exception
            }
        }

        public void SetCell(uint x, uint y, byte value)
        {
            if(x < bounds.Item1 && y < bounds.Item2)
            {
                gridMutex.WaitOne();
                grid[x, y] = value;
                gridMutex.ReleaseMutex();
            }
            else
            {
                throw new Exception();
            }
        }

        public Image UpdateGraphics()
        {
            graphicsMutex.WaitOne();
            gridMutex.WaitOne();
            float dx_b, dx_e, dy_b, dy_e;
            dx_b = centerpos.Item1 - (size.Item1 / 2.0f) / zoom;
            dx_e = centerpos.Item1 + (size.Item1 / 2.0f) / zoom;
            dy_b = centerpos.Item2 - (size.Item2 / 2.0f) / zoom;
            dy_e = centerpos.Item2 + (size.Item2 / 2.0f) / zoom;
            Image returnValue = new Bitmap(size.Item1, size.Item2);
            Graphics g = Graphics.FromImage(returnValue);
            g.Clear(pallet[DefaultState]);
            int x_b, x_e, y_b, y_e;
            x_b = (int)(dx_b / cellSize) - 1;
            x_e = (int)(dx_e / cellSize) + 1;
            y_b = (int)(dy_b / cellSize) - 1;
            y_e = (int)(dy_e / cellSize) + 1;
            if (x_b < 0) x_b = 0;
            if (x_e >= bounds.Item1) x_e = (int)bounds.Item1;
            if (y_b < 0) y_b = 0;
            if (y_e >= bounds.Item1) y_e = (int)bounds.Item2;
            for (int y = y_b; y < y_e; y++)
            {
                for(int x = x_b; x < x_e; x++)
                {
                    g.DrawRectangle(pallet.Pens[grid[x, y]], x * cellSize - dx_b, y * cellSize - dy_b, cellSize*zoom, cellSize*zoom);
                }
            }
            gridMutex.ReleaseMutex();
            graphicsMutex.ReleaseMutex();
            return returnValue;
         }

        public void Step()
        {
            gridMutex.WaitOne();
            byte[] buff = new byte[bounds.Item1];
            for(int i = 0; i < bounds.Item1; i++)
            {
                buff[i] = DefaultState;
            }
            byte[] sbuff = new byte[bounds.Item1];
            for (int y = 0; y < bounds.Item2; y++)
            {
                for(int x = 0; x < bounds.Item1; x++)
                {
                    sbuff[x] = grid[x, y];
                    byte[] c = new byte[9];
                    bool[] IsOnBound =
                    {
                        x == 0,
                        x == bounds.Item1 - 1,
                        y == bounds.Item2 - 1
                    };
                    if (IsOnBound[0]) // x == 0
                    {
                        c[0] = DefaultState;
                        c[3] = DefaultState;
                        c[6] = DefaultState;
                    }
                    else // x != 0
                    {
                        c[0] = buff[x - 1]; // x-1,y-1
                        c[3] = sbuff[x - 1]; // x-1,y
                        if (IsOnBound[2]) 
                            c[6] = DefaultState;
                        else
                            c[6] = grid[x - 1, y + 1];
                    }

                    if (IsOnBound[1])
                    {
                        c[2] = DefaultState;
                        c[5] = DefaultState;
                        c[8] = DefaultState;
                    }
                    else
                    {
                        c[2] = buff[x + 1]; // x+1,y-1
                        c[5] = grid[x + 1, y];
                        if (IsOnBound[2])
                            c[8] = DefaultState;
                        else
                            c[8] = grid[x + 1, y + 1];
                    }

                    c[1] = buff[x];
                    c[4] = grid[x, y];
                    if (IsOnBound[2])
                        c[7] = DefaultState;
                    else
                        c[7] = grid[x, y + 1];

                    grid[x,y]=rules.Step(c);
                }

                buff = sbuff;
                sbuff = new byte[bounds.Item1];
            }
            //update graphics

            gridMutex.ReleaseMutex();
        }
    }
}
