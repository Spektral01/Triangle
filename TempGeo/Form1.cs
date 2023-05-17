using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TempGeo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitCoords();
            DrawRGBTriangle();
        }

        public struct Coords
        {
            public double x;
            public double y;
            public UInt32 color;

            public Coords(double x, double y, UInt32 color)
            {
                this.x = x;
                this.y = y;
                this.color = color;
            }
        }

        public Coords A, B, C;

        private void InitCoords()
        {
            A = new Coords(370, 0, 0xff0000);
            B = new Coords(0, 450, 0x00ff00);
            C = new Coords(740, 450, 0x0000ff);
        }


        Bitmap picture;

        public void DrawRGBTriangle()
        {
            picture = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Draw();
            pictureBox1.Image = picture;
        }

        public void Draw()
        {
            for (int y = 0; y < pictureBox1.Height; y++)
                for (int x = 0; x < pictureBox1.Width; x++)
                    picture.SetPixel(x, y, Color.FromArgb((int)AffinPixelColor(x, y)));
        }

        public UInt32 AffinPixelColor(int x, int y)
        {
            UInt32 colPix;


            double lambda1, lambda2, lambda3;
            colPix = 0xffffff;

            lambda1 = ((B.y - C.y) * ((double)(x) - C.x) + (C.x - B.x) * ((double)(y) - C.y)) /
            ((B.y - C.y) * (A.x - C.x) + (C.x - B.x) * (A.y - C.y));
            lambda2 = ((C.y - A.y) * ((double)(x) - C.x) + (A.x - C.x) * ((double)(y) - C.y)) /
                ((B.y - C.y) * (A.x - C.x) + (C.x - B.x) * (A.y - C.y));

            lambda3 = 1 - lambda1 - lambda2;

            if (lambda1 >= 0 && lambda1 <= 1 && lambda2 >= 0 && lambda2 <= 1 && lambda3 >= 0 && lambda3 <= 1)
            {
                colPix = (UInt32)0xFF000000 |
                    ((UInt32)(lambda1 * ((A.color & 0x00FF0000) >> 16) + lambda2 * 
                    ((B.color & 0x00FF0000) >> 16) + lambda3 * ((C.color & 0x00FF0000) >> 16)) << 16) |
                    ((UInt32)(lambda1 * ((A.color & 0x0000FF00) >> 8) + lambda2 * 
                    ((B.color & 0x0000FF00) >> 8) + lambda3 * ((C.color & 0x0000FF00) >> 8)) << 8) |
                    (UInt32)(lambda1 * (A.color & 0x000000FF) + lambda2 * 
                    (B.color & 0x000000FF) + lambda3 * (C.color & 0x000000FF));
            }

            return colPix;
        }
    }

}

