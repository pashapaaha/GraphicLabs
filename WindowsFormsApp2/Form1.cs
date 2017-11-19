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

    public partial class mainForm : Form
    {

#region//FUTU-U-U-URE

        float Grad = (float)(2 * Math.PI) / 360; //радиан в градусе
        bool glload = false;
        float R = -100;
        bool TrMouseD = false; //нажата ли мышь
        int TrMouseDX; //координата по Х, где нажата мышь
        int TrMouseDY;
        int TrMouseX; //координата по Х, где мышь находится в данный  момент
        int TrMouseY;
        int TrUgolEX; //углы наклона экрана по Х
        int TrUgolEY;
        int TrUgolDEX; //углы наклона экрана по Х в момент нажатия мыши
        int TrUgolDEY;

        /// <summary>
        /// преобразует координаты, поворачивая вокруг 0.0 на указанный угол
        /// </summary>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <param name="iU"></param>
        void TrUgol(ref float iX, ref float iY, int iU)
        {
            float X, Y, U = Grad * (float)iU;
            X = (float)(iX * Math.Cos(U) - iY * Math.Sin(U));
            Y = (float)(iX * Math.Sin(U) + iY * Math.Cos(U));
            iX = X;
            iY = Y;

        }
#endregion        

        Guitar guitar = new Guitar();

        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// движение мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            TrMouseX = e.X;
            TrMouseY = e.Y;

            if (TrMouseD)
            {
                TrUgolEX = TrUgolDEX + (TrMouseX - TrMouseDX);
                TrUgolEY = TrUgolDEY + (TrMouseY - TrMouseDY);

            }
        }

        /// <summary>
        /// прокрутка колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0) { R = R - 1; }
            if (e.Delta > 0) { R = R + 1; }
            R = (R < -99) ? -99 : R;
        }

        /// <summary>
        /// нажатие мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            TrMouseD = true;
            TrMouseDX = e.X;
            TrMouseDY = e.Y;
            TrUgolDEX = TrUgolEX;
            TrUgolDEY = TrUgolEY;
        }

        /// <summary>
        /// отпустить мышь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            TrMouseD = false;
        }

        /// <summary>
        /// загрузка glControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Load(object sender, EventArgs e)
        {
            glload = true;
             

        }

        /// <summary>
        /// отрисовка 'элементов glControl'а
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.Ortho(-1, 1, -1, 1, -1, 1); // Указываю систему координат 
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height); // Использовать всю поверхность GLControl под рисование
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);// Указываем матрицу с котрой будем работать
            
            GL.LoadIdentity(); // Сброс в еденичную матрицу

            Matrix4 dimetry = new Matrix4(
                new Vector4(0.866f,     0,      -0.5f,      0),
                new Vector4(-0.223f,    0.894f, -0.387f,    0),
                new Vector4(0,          0,      0,          0),
                new Vector4(0,          0,      0,          1)
                );
            

            //Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(0.5f, glControl1.Width / glControl1.Height, 1, 100);
            //GL.LoadMatrix(ref perspective); // Загрузка настройки проекции
            GL.LoadMatrix(ref dimetry);

            GL.ClearColor(Color.White);            

            guitar = new Guitar();
            guitar.tune(TrUgolEX, TrUgolEY, R);
            guitar.scale(0.1f, 0.1f, 0.1f);
            guitar.DrawLines();

            

            glControl1.SwapBuffers(); // Выводжу содержиоме буффкера OPenGl в буффер OpenTK компанента 
            glControl1.Invalidate(); // Перерисовка компанента на форме

        }

        /// <summary>
        /// процедура для управления программой с клавиатуры
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.R:
                    TrUgolEX = TrUgolEY = 0;
                    R = -50;
                    break;
                case Keys.Up:
                    TrUgolEY += 10;
                    break;
                case Keys.Down:
                    TrUgolEY -= 10;
                    break;
                case Keys.Right:
                    TrUgolEX += 10;
                    break;
                case Keys.Left:
                    TrUgolEX -= 10;
                    break;
                default: return false;
            }
            return true;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {

        }
        

    }
}
