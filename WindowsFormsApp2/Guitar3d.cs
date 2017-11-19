using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Platform.Windows;
using OpenTK.Graphics.OpenGL;

namespace WindowsFormsApp2
{
    public class GuitarPart
    {
        public Matrix m;
        protected float linesWidth;
        protected Color color;
        public virtual void DrawLines()
        {
            GL.Color3(color);
            GL.LineWidth(linesWidth);
        }

        protected void TrUgol(ref float iX, ref float iY, int iU)
        {
            float X, Y, U = ((float)(2 * Math.PI) / 360) * (float)iU;
            X = (float)(iX * Math.Cos(U) - iY * Math.Sin(U));
            Y = (float)(iX * Math.Sin(U) + iY * Math.Cos(U));
            iX = X;
            iY = Y;
        }

        public virtual void tune(int ugolX, int ugolY, float len)
        {
            for (int f = 0; f < m.Row; f++)
            {
                TrUgol(ref m.matrix[f, 1], ref m.matrix[f, 2], ugolY);
                TrUgol(ref m.matrix[f, 0], ref m.matrix[f, 2], ugolX);
                m.matrix[f, 2] -= len;
            } 
        }

        public virtual void scale(float mx, float my, float mz)
        {
            Matrix t = new Matrix(4, 4);
            t.matrix = new float[4, 4]
            {
                {mx, 0, 0, 0 },
                {0, my, 0, 0 },
                {0, 0, mz, 0 },
                {0, 0, 0, 0 }
            };

            m = m.Multiple(t);
        }
    }
    public class GuitarParts: GuitarPart
    {
        public Matrix m1;

        public override void DrawLines()
        {
            base.DrawLines();
            GL.Begin(BeginMode.LineLoop);
            for (int f = 0; f < m.Row; f++)
                GL.Vertex3(m.matrix[f, 0], m.matrix[f, 1], m.matrix[f, 2]);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            for (int f = 0; f < m1.Row; f++)
                GL.Vertex3(m1.matrix[f, 0], m1.matrix[f, 1], m1.matrix[f, 2]);
            GL.End();

            for (int f = 0; f < m.Row; f++)
            {
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex3(m1.matrix[f, 0], m1.matrix[f, 1], m1.matrix[f, 2]);
                GL.Vertex3(m.matrix[f, 0], m.matrix[f, 1], m.matrix[f, 2]);
                GL.End();
            }
        }
        public override void tune(int ugolX, int ugolY, float len)
        {
            for (int f = 0; f < m.Row; f++)
            {
                TrUgol(ref m.matrix[f, 1], ref m.matrix[f, 2], ugolY);
                TrUgol(ref m.matrix[f, 0], ref m.matrix[f, 2], ugolX);
                m.matrix[f, 2] -= len;
                TrUgol(ref m1.matrix[f, 1], ref m1.matrix[f, 2], ugolY);
                TrUgol(ref m1.matrix[f, 0], ref m1.matrix[f, 2], ugolX);
                m1.matrix[f, 2] -= len;
            }
        }
        public override void scale(float mx, float my, float mz)
        {
            base.scale(mx, my, mz);
            Matrix t = new Matrix(4, 4);
            t.matrix = new float[4, 4]
            {
                {mx, 0, 0, 0 },
                {0, my, 0, 0 },
                {0, 0, mz, 0 },
                {0, 0, 0, 0 }
            };
            
            m1 = m1.Multiple(t);
        }
    }
    class GuitarBody : GuitarParts
    {
        public GuitarBody()
        {
            color = Color.Gray;
            linesWidth = 2;
            m = new Matrix(8, 4);
            m.matrix = new float[8, 4] { 
                {-11.5f,    4.0f,   0f, 0},
                {-7,        -0.5f,  0f, 0},
                {-8,        -3.5f,  0f, 0},
                {-3.5f,     -1.5f,  0f, 0},
                {1.5f,      -2.5f,  0f, 0},
                {-1,        0.5f,   0f, 0},
                {0.5f,      4,      0f, 0},
                {-4.5f,     2,      0f, 0} };

            m1 = new Matrix(8, 4);
            m1.matrix = new float[8, 4] {
                {-11.5f,    4.0f,   1f, 0},
                {-7,        -0.5f,  1f, 0},
                {-8,        -3.5f,  1f, 0},
                {-3.5f,     -1.5f,  1f, 0},
                {1.5f,      -2.5f,  1f, 0},
                {-1,        0.5f,   1f, 0},
                {0.5f,      4,      1f, 0},
                {-4.5f,     2,      1f, 0} };
        }
    }

    class GuitarSaddle: GuitarParts
    {
        public GuitarSaddle()
        {
            color = Color.Gray;
            linesWidth = 1.5f;
            m = new Matrix(4, 4);
            m1 = new Matrix(4, 4);
            m.matrix = new float[4, 4]
            {
                {-6.0f, 1.5f, -0.2f , 0},
                {-6.5f, 1.5f, -0.2f , 0},
                {-6.5f, -0.5f, -0.2f , 0},
                {-6.0f, -0.5f, -0.2f , 0}
                
            };
            m1.matrix = new float[4, 4]
            {
                {-6.0f, 1.5f, 0f , 0},
                {-6.5f, 1.5f, 0f , 0},
                {-6.5f, -0.5f, 0f , 0},
                {-6.0f, -0.5f, 0f , 0}
            };
        }

    }

    class GuitarNeck: GuitarParts
    {
        public GuitarNeck()
        {
            color = Color.RosyBrown;
            linesWidth = 1.5f;
            m = new Matrix(7, 4);
            m.matrix = new float[7, 4] {
                {-5.0f,     1.0f,   -0.1f, 0},
                {8.0f,      1.0f,   -0.1f, 0},
                {8.5f,      1.3f,   -0.1f, 0},
                {11.0f,     0.0f,   -0.1f, 0},
                {11.1f,     -1.0f,  -0.1f, 0},
                {8.0f,      0.0f,   -0.1f, 0},
                {-4.6f,     0.0f,   -0.1f, 0}};

            m1 = new Matrix(7, 4);
            m1.matrix = new float[7, 4] {
                {-5.0f,     1.0f,   0, 0},
                {8.0f,      1.0f,   0, 0},
                {8.5f,      1.3f,   0, 0},
                {11.0f,     0.0f,   0, 0},
                {11.1f,     -1.0f,  0, 0},
                {8.0f,      0.0f,   0, 0},
                {-4.6f,     0.0f,   0, 0}};
        }
    }

    class GuitarStrings: GuitarPart
    {
        public GuitarStrings()
        {
            color = Color.Black;
            linesWidth = 0.5f;
            m = new Matrix(14, 4);
            m.matrix = new float[14, 4] {
                {-6.3f,     0.8f,   -0.2f, 0},
                {8.8f,      0.8f,   -0.2f, 0},
                {-6.3f,     0.7f,   -0.2f, 0},
                {9.0f,      0.7f,   -0.2f, 0},
                {-6.3f,     0.6f,   -0.2f, 0},
                {9.2f,      0.6f,   -0.2f, 0},
                {-6.3f,     0.5f,   -0.2f, 0},
                {9.4f,      0.5f,   -0.2f, 0},
                {-6.3f,     0.4f,   -0.2f, 0},
                {9.6f,      0.4f,   -0.2f, 0},
                {-6.3f,     0.3f,   -0.2f, 0},
                {9.8f,      0.3f,   -0.2f, 0},
                {-6.3f,     0.2f,   -0.2f, 0},
                {10.0f,     0.2f,   -0.2f, 0}
            };
        }

        public override void DrawLines()
        {
            base.DrawLines();
            GL.Color3(color);
            GL.LineWidth(linesWidth);
            GL.Begin(BeginMode.Lines);
            for (int f = 0; f < m.Row; f++)
                GL.Vertex3(m.matrix[f, 0], m.matrix[f, 1], m.matrix[f, 2]);
            GL.End();
        }
    }

    /// <summary>
    /// Класс, содержащий поля и методы для отрисовки и преобразования гитары
    /// </summary>
    class Guitar
    {
        public GuitarBody body;
        public GuitarNeck neck;
        public GuitarStrings strings;
        public GuitarSaddle saddle;

        public Guitar()
        {
            body = new GuitarBody();
            neck = new GuitarNeck();
            strings = new GuitarStrings();
            saddle = new GuitarSaddle();
        }

        public void DrawLines()
        {
            body.DrawLines();
            neck.DrawLines();
            strings.DrawLines();
            saddle.DrawLines();
        }
        public void tune(int ugolX, int ugolY, float len)
        {
            body.tune(ugolX, ugolY, len);
            neck.tune(ugolX, ugolY, len);
            strings.tune(ugolX, ugolY, len);
            saddle.tune(ugolX, ugolY, len);
        }
        public void scale(float mx, float my, float mz)
        {
            body.scale(mx, my, mz);
            neck.scale(mx, my, mz);
            strings.scale(mx, my, mz);
            saddle.scale(mx, my, mz);
        }

        public void toDimetrix()
        {
            float Grad = (float)(2 * Math.PI) / 360;
            Matrix transform = new Matrix(4,4);
            float psi = 53.7f * Grad;
            float fi = 7f * Grad;
            transform.matrix = new float[4, 4]
            {
                {(float)Math.Cos(psi), (float)(Math.Sin(fi)*Math.Cos(psi)),     0, 0 },
                {0,                    (float)Math.Cos(psi),                    0, 0 },
                {(float)Math.Sin(psi), -(float)(Math.Sin(psi)*Math.Cos(psi)),   0, 0 },
                {0,                    0,                                       0, 1 }
            };
            //body.m = body.m.Multiple(transform);
            //body.m1 = body.m1.Multiple(transform);


        }

    }

}
