using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3_im
{
    public partial class Form1 : Form
    {
        const int Weight = 20, Hight = 20;
        int sizeW_panel, sizeH_panel;
        int sizeW_cell, sizeH_cell;

        bool[,] CellularMartix;

        Bitmap bitmap;
        Graphics graph;
        int lastGeneration;
        bool step = false;

        public Form1()
        {
            InitializeComponent();
            InitCellGrid();
        }

        private void CreateFirstGeneration()
        {
            CellularMartix = new bool[Hight, Weight];
            lastGeneration = 0;
            CellularMartix[2, 4] = true;
            CellularMartix[3, 4] = true;
            CellularMartix[3, 6] = true;
            CellularMartix[2, 6] = true;
            CellularMartix[1, 5] = true;

            CellularMartix[7, 3] = true;
            CellularMartix[7, 4] = true;
            CellularMartix[6, 6] = true;
            CellularMartix[6, 5] = true;
            CellularMartix[5, 5] = true;
        }

        private void InitCellGrid()
        {
            sizeW_panel = panel1.Width;
            sizeH_panel = panel1.Height;

            sizeW_cell = sizeW_panel / Weight;
            sizeH_cell = sizeH_panel / Hight;

            bitmap = new Bitmap(sizeW_panel, sizeH_panel);
            graph = Graphics.FromImage(bitmap);

            DrawGridTemplate();
        }

        private void DrawCells()
        {
            SolidBrush solidBrush = new SolidBrush(Color.GreenYellow);

            for (int i = 0; i < Hight; i++)
            {
                for (int j = 0; j < Weight; j++)
                {
                    if (CellularMartix[i, j])
                    {
                        graph.FillRectangle(solidBrush, j * sizeW_cell, i * sizeH_cell, sizeW_cell, sizeH_cell);
                    }
                }
            }
        }

        private void DrawGridTemplate()
        {
            Pen pen = new Pen(Color.Black);

            for (int i = 0; i < Weight + 1; i++)
            {
                graph.DrawLine(pen, i * sizeW_cell, 0, i * sizeW_cell, sizeH_panel);
            }

            for (int i = 0; i < Hight + 1; i++)
            {
                graph.DrawLine(pen, 0, i * sizeH_cell, sizeW_panel, i * sizeH_cell);
            }
        }

        private int SetRules(int i, int j)
        {
            int aliveNeighbours = 0;
            for (int x = -1; x <= 1; x++)
            {
                int l = i + x;

                if ((l >= 0) && (l < Hight))
                    for (int y = -1; y <= 1; y++)
                    {
                        int r = j + y;

                        if ((r >= 0) && (r < Weight))
                            aliveNeighbours += CellularMartix[l, r] ? 1 : 0;
                    }
            }

            aliveNeighbours -= CellularMartix[i, j] ? 1 : 0;

            return aliveNeighbours;
        }

        private void CreateNextGeneration()
        {
            int prevGen = lastGeneration;
            lastGeneration++;

            bool[,] NextMatrix = new bool[Hight, Weight];
            for (int i = 0; i < Hight; i++)
            {
                for (int j = 0; j < Weight; j++)
                {
                    if (CellularMartix[i, j])
                    {
                        int alive = SetRules(i, j);
                        NextMatrix[i, j] = (alive == 2) || (alive == 3);
                    }
                    else
                    {
                        NextMatrix[i, j] = SetRules(i, j) == 3;
                    }
                }
            }
            CellularMartix = NextMatrix;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (step)
            {
                timer1.Stop();
            }
            else
            {
                CreateFirstGeneration();

                graph.Clear(Color.White);
                timer1.Start();
            }
            step = !step;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lastGeneration >= Weight - 1)
            {
                timer1.Stop();
                return;
            }


            graph.Clear(Color.White);
            CreateNextGeneration();
            DrawCells();
            DrawGridTemplate();
            panel1.Invalidate();
        }
        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

    private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, Point.Empty);
        }
    }
}
