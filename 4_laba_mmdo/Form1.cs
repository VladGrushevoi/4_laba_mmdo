using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _4_laba_mmdo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[1].ChartType = SeriesChartType.Point;
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Points.Clear();
            double x_min = -2.0;
            double x_max = 2.0;
            try
            {
                x_min = Convert.ToDouble(numericUpDown1.Text);
                x_max = Convert.ToDouble(numericUpDown2.Text);
            }
            catch
            {
                MessageBox.Show("Перевірьте поля для вводу, можливо там некоректні дані");
            }
            if (x_min > x_max)
            {
                MessageBox.Show("Нижня межа не може бути більшою за верхню!!!!");
            }
            if (x_max > x_min)
            {
                for (double x = x_min; x <= x_max; x += 0.01)
                {
                    double y = Function(x);
                    chart1.Series[0].Points.AddXY(x, y);

                }
                switch (comboBox1.SelectedIndex)
                {
                    case 0: MethodLocalPoint(); break;
                    case 1: Dihotomia(); break;
                    case 2: GoldSlice(); break;
                    case 3: Fibonachi(); break;
                    //case 4: Newton(); break;

                }
            }
        }

        private double Function(double x)
        {
            return Math.Pow(x, 6) - 3 * Math.Pow(x, 2) + 5 * x + 1;
        }

        private void MethodLocalPoint()
        {
            chart1.Series[1].Points.Clear();
            double x_min, x_max;
            double x0 = 1, f0 = Function(x0), x2=0, x1 = 0, h = 0.001, eps = 0.0001;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            double f1 = Function(x0), f2 = 0;
            do
            {
                h = h / 2;
                x2 = x0 + h;
                f2 = Function(x2);
                if (f1<=f2)
                {
                    h = -h;
                    x2 = x0;
                    f2 = Function(f2);
                }

            } while (f1 <= f2 || Math.Abs(h) < eps);

            if (Math.Abs(h)>eps)
            {
                do
                {
                    x1 = x2;
                    f1 = f2;
                    x2 = x1 + h;
                    f2 = Function(x2);

                } while (f1<f2);
            }
            else
            {

            }

            if (h > 0)
            {
                x_min = x1 - h;
                x_max = x2;
            }
            else
            {
                x_min = x1;
                x_max = x2-h;
            }
            
        }

        private void Dihotomia()
        {
            chart1.Series[1].Points.Clear();
            double x_min, x_max;
            double eps = 0.0001, d = eps / 3, x1 = 0, x2 = 0, f1 = 0, f2 = 0;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            while (x_max - x_min < eps)
            {
                x1 = (x_min + x_max - d) / 2;
                x2 = (x_min + x_max + d) / 2;

                f1 = Function(x1);
                f2 = Function(x2);

                if (f1 <= f2)
                {
                    x_max = x2;
                }
                else
                {
                    x_max = x1;
                }
            }
            double x_res = (x_min + x_max) / 2, fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x.Text = "Мінімум " + fx_res.ToString() + " 2";
            chart1.Series[1].MarkerSize = 8;
        }

        private void GoldSlice()
        {
            chart1.Series[1].Points.Clear();
            double x_min, x_max, eps = 0.0001, u = 0, v = 0, fu = 0, fv = 0;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            u = x_min + (3 - Math.Sqrt(5)) / 2 * (x_max - x_min);
            v = x_min + x_max - u;

            fu = Function(u);
            fv = Function(v);

            while (x_max - x_min < eps)
            {
                if (fu<=fv)
                {
                    x_max = v;
                    v = u;
                    fv = fu;
                    u = x_min + x_max - v;
                    fu = Function(u);
                }
                else
                {
                    x_min = u;
                    u = v;
                    fu = fv;
                    v = x_min + x_max - u;
                    fv = Function(v);
                }
                if (u > v)
                {
                    u = x_min + (3 - Math.Sqrt(5)) / 2 * (x_max - x_min);
                    v = x_min + x_max - u;

                    fu = Function(u);
                    fv = Function(v);
                }
            }
            double x_res = (x_min + x_max) / 2, fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x.Text = "Мінімум " + fx_res.ToString()+" 3";
            chart1.Series[1].MarkerSize = 8;
        }

        private double F(double n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            else
            {
                return F(n - 1) + F(n - 2);
            }
        }

        private void Fibonachi()
        {
            chart1.Series[1].Points.Clear();
            double a, b, n=30, u = 0, v = 0, fu = 0, fv = 0;
            a = Convert.ToDouble(numericUpDown1.Value);
            b = Convert.ToDouble(numericUpDown2.Value);

            u = a + F(n) / F(n + 2) * (b - a);
            v = a + b - u;

            fu = Function(u);
            fv = Function(v);

            for (int i = 0; i < n; i++)
            {
                if (fu <= fv)
                {
                    b = v;
                    v = u;
                    fv = fu;
                    u = a + b - v;
                    fu = Function(u);
                }
                else
                {
                    a = u;
                    u = v;
                    fu = fv;
                    v = a + b - u;
                    fv = Function(v);
                }

                if (u > v)
                {
                    u = a + F(n - i + 1) / F(n - i + 3) * (b - a);
                    v = a + b - u;
                    fu = Function(u);
                    fv = Function(v);
                }
            }
            double x_res = (a + b) / 2, fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x.Text = "Мінімум " + fx_res.ToString()+" 4";
            chart1.Series[1].MarkerSize = 8;
        }

        private void Parabol()
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            chart1.Series.Add("Корінь");
        }
    }
}
