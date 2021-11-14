using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetSize();
            numericUpDown1.Value = 2;
            numericUpDown1.Minimum = 1;
            pictureBox1.BackColor = Color.White;
            MouseMove += pictureBox1_MouseMove;
            colorDialog1.FullOpen = true;
            colorDialog1.Color = Color.Black;
        }
        private class Points
        {
            private int index = 0;
            private Point[] points;

            public Points(int size)
            {
                if (size <= 0)
                    size = 1;
                points = new Point[size]; 
            }
            public void SetPoint(int x, int y)
            {
                if(index >= points.Length)
                {
                    index = 0;
                }
                points[index] = new Point(x, y);
                index++;
            }
            public void ResetPoints()
            {
                index = 0;
            }
            public int GetCountPoints()
            {
                return index;
            }
            public Point[] GetPoints()
            {
                return points;
            }
        }

        private Points points = new Points(2);
        bool isPaint = false;

        Bitmap bmp = new Bitmap(100,100);
        Graphics g;

        Pen pen = new Pen(Color.Black, 3f);
        Pen erase = new Pen(Color.White, 3f);
        int index;
        int x, y, sX, sY, cX, cY;

        private void SetSize()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            bmp = new Bitmap(rectangle.Width, rectangle.Height);
            g = Graphics.FromImage(bmp);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isPaint = true;
            cX = e.X;
            cY = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isPaint = false;
            points.ResetPoints();

            sX = x - cX;
            sY = y - cY;

            if (index == 3)
            {
                g.DrawEllipse(pen, cX, cY, sX, sY);
                pictureBox1.Image = bmp;
                points.SetPoint(e.X, e.Y);
            }
            if (index == 4)
            {
                g.DrawRectangle(pen, cX, cY, sX, sY);
                pictureBox1.Image = bmp;
                points.SetPoint(e.X, e.Y);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            l_MousePos.Text = $"X {e.X} - Y {e.Y}";

            if (!isPaint)
                return;
            points.SetPoint(e.X, e.Y);
            if (points.GetCountPoints() >= 2)
            {
                if (index == 1)
                {
                    g.DrawLines(pen, points.GetPoints());
                    pictureBox1.Image = bmp;
                    points.SetPoint(e.X, e.Y);
                }
                if (index == 2)
                {
                    g.DrawLines(erase, points.GetPoints());
                    pictureBox1.Image = bmp;
                    points.SetPoint(e.X, e.Y);
                }
                
            }
            pictureBox1.Refresh();
            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

        private void chooseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pen.Color = colorDialog1.Color;
        }

        private void b_Clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Transparent);
            pictureBox1.Image = bmp;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = (float)numericUpDown1.Value;
            erase.Width = (float)numericUpDown1.Value;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG(*.JPG)|*.jpg";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                if(pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName);
                }
            }
        }

        private void b_Ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void b_Rectangle_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void b_Line_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void b_Erase_Click(object sender, EventArgs e)
        {
            index = 2;
        }
    }
}
