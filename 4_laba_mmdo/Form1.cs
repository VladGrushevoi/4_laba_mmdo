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
            chart1.Series[0].BorderWidth = 3;
            double x_min = -2.0;
            double x_max = 2.0;
            try
            {
                x_min = Convert.ToDouble(numericUpDown1.Text);
                x_max = Convert.ToDouble(numericUpDown2.Text);
            }
            catch
            {
                MessageBox.Show("Перевірте поля для вводу, можливо там некоректні дані");
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
                    //case 0: MethodLocalPoint(); break;
                    case 0: Dihotomia(); break;
                    case 1: GoldSlice(); break;
                    case 2: Fibonachi(); break;
                    case 3: Parabol(); break;

                }
            }
        }

        private int count_callFunc = 0;
        private int iterCount = 0;
        private double E = 0.00000001;
        private double Function(double x)
        {
            count_callFunc++;

            //return Math.Log10(x) + Math.Sin(x);
            //return Math.Pow(x, 4) + 4 * Math.Pow(x, 3) + 3 * Math.Pow(x, 2) - 36 * x + 45;
           return Math.Pow(x, 6) - 3 * Math.Pow(x, 2) + 5 * x + 1;
        }

        private void MethodLocalPoint()
        {
            chart1.Series[1].Points.Clear();
            double x_min, x_max;
            double x0 = 1, x2, x1, h = 0.001, eps = 0.0001;
            double x_res = 0, fx_res = 0;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            double f1 = Function(x0), f2 = 0;
            do
            {
                h = h / 2;
                x2 = x0 + h;
                f2 = Function(x2);
                if (f1 <= f2)
                {
                    h = -h;
                    x2 = x0;
                    f2 = Function(f2);
                }

            } while (f1 <= f2 || Math.Abs(h) < eps);

            if (Math.Abs(h) > eps)
            {
                do
                {
                    x1 = x2;
                    f1 = f2;
                    x2 = x1 + h;
                    f2 = Function(x2);

                } while (f1 < f2);

                if (h > 0)
                {
                    x_min = x1 - h;
                    x_max = x2;
                }
                else
                {
                    x_min = x1;
                    x_max = x2 - h;
                }
            }
            else
            {
                x_res = x0;
                fx_res = Function(x0);
                chart1.Series[1].Points.AddXY(x_res, fx_res);
                chart1.Series[1].Color = Color.Red;
                x_text.Text = "Мінімум " + fx_res.ToString() + " 1";
                chart1.Series[1].MarkerSize = 8;
            }
        }

        private void Dihotomia()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            count_callFunc = 0;
            iterCount = 0;
            chart1.Series[1].Points.Clear();
            double x_min, x_max;
            double eps = E, d = eps / 3, x1, x2, f1, f2;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            do
            {
                x1 = (x_min + x_max - d) / 2;
                x2 = (x_min + x_max + d) / 2;

                f1 = Function(x1);
                f2 = Function(x2);


                Console.WriteLine("х1 = " + Math.Round(x1,4) + " x2 = " + Math.Round(x2,4) + " f(x1) = " + Math.Round(f1,2) + " f(x2) = " + Math.Round(f2,2));

                if (f1 <= f2)
                {
                    x_max = x2;
                }
                else
                {
                    x_min = x1;
                }
                iterCount++;
            } while (Math.Abs(x_max - x_min) > eps);
            time.Text = watch.Elapsed.TotalMilliseconds.ToString() + " мс";
            watch.Stop();
            double x_res = (x_min + x_max) / 2, fx_res = Function(x_res);

            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x_text.Text = "Мінімум " + fx_res.ToString() + " 3";
            x_rres_text.Text = "Значення Х: " + x_res.ToString();
            iter_text.Text = "Кількість обчислень функцій " + count_callFunc.ToString();
            iter_count.Text = "Кількість ітерацій " + iterCount.ToString();
            chart1.Series[1].MarkerSize = 8;
        }

        

        private void GoldSlice()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            count_callFunc = 0;
            iterCount = 0;
            chart1.Series[1].Points.Clear();
            double x_min, x_max, eps = E, u, v, fu, fv;
            x_min = Convert.ToDouble(numericUpDown1.Value);
            x_max = Convert.ToDouble(numericUpDown2.Value);

            u = x_min + (3 - Math.Sqrt(5)) / 2 * (x_max - x_min);
            v = x_min + x_max - u;

            fu = Function(u);
            fv = Function(v);

            do
            {
                iterCount++;
                if (fu <= fv)
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

                Console.WriteLine("u = " + Math.Round(u,4) + " v = " + Math.Round(v,4) + " f(u) = " + Math.Round(fu,2) + " f(v) = " + Math.Round(fv,2));

            } while (Math.Abs(x_max - x_min) > eps);
            time.Text = watch.Elapsed.TotalMilliseconds.ToString() + " мс";
            watch.Stop();
            double x_res = (x_min + x_max) / 2, fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x_text.Text = "Мінімум " + fx_res.ToString() + " 3";
            x_rres_text.Text = "Значення Х: " + x_res.ToString();
            iter_text.Text = "Кількість обчислень функцій " + count_callFunc.ToString();
            iter_count.Text = "Кількість ітерацій " + iterCount.ToString();
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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            count_callFunc = 0;
            iterCount = 0;
            chart1.Series[1].Points.Clear();
            double a, b, n = 15, u, v, fu, fv;
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
                iterCount++;
                Console.WriteLine("u = " + Math.Round(u,4) + " v = " + Math.Round(v,4) + " f(u) = " + Math.Round(fu,2) + " f(v) = " + Math.Round(fv,2));
            }
            time.Text = watch.Elapsed.TotalMilliseconds.ToString() + " мс";
            watch.Stop();
            double x_res = (a + b) / 2, fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x_text.Text = "Мінімум " + fx_res.ToString() + " 4";
            x_rres_text.Text = "Значення Х: " + x_res.ToString();
            iter_text.Text = "Кількість обчислень функцій " + count_callFunc.ToString();
            iter_count.Text = "Кількість ітерацій " + iterCount.ToString();
            chart1.Series[1].MarkerSize = 8;
        }

        //private void Parabol()
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    count_callFunc = 0;
        //    iterCount = 0;
        //    chart1.Series[1].Points.Clear();
        //    double h = 0.001;
        //    double x = 2, eps = E;
        //    x = Convert.ToDouble(numericUpDown1.Value);
        //    if (x == 0) x += 0.1;
        //    //while ((Function(x + h) - 2 * Function(x) + Function(x - h)) / (h * h) <= 0) 
        //    //{
        //    //    x += 0.1;
        //    //    iterCount++;
        //    //}
        //    double x1 = 7;
        //    x1 = x - 0.5 * h * (Function(x + h) - Function(x - h)) / (Function(x + h) - 2 * Function(x) + Function(x - h));
        //    while (Math.Abs(x1 - x) > eps)
        //    {
        //        x = x1;
        //        x1 = x - 0.5 * h * (Function(x + h) - Function(x - h)) / (Function(x + h) - 2 * Function(x) + Function(x - h));
        //        iterCount++;
        //        Console.WriteLine("f(x+h) = " + Math.Round(Function(x + h), 4) + "f(x-h) = " + Math.Round(Function(x + h), 4) + "f(x) = " + Math.Round(Function(x)), 4);
        //    }
        //    time.Text = watch.Elapsed.TotalMilliseconds.ToString() + " мс";
        //    watch.Stop();
        //    double x_res = x1, fx_res = Function(x_res);
        //    chart1.Series[1].Points.AddXY(x_res, fx_res);
        //    chart1.Series[1].Color = Color.Red;
        //    x_text.Text = "Мінімум " + fx_res.ToString() + " 5";
        //    x_rres_text.Text = "Значення Х: " + x_res.ToString();
        //    iter_text.Text = "Кількість обчислень функцій " + count_callFunc.ToString();
        //    iter_count.Text = "Кількість ітерацій " + iterCount.ToString();
        //    chart1.Series[1].MarkerSize = 8;
        //}

        private void Parabol()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            double a = Convert.ToDouble(numericUpDown1.Value);
            double b = Convert.ToDouble(numericUpDown2.Value);
            count_callFunc = 0;
            iterCount = 0;
            chart1.Series[1].Points.Clear();
            double x_res = 0, fx3, xMin, fMin, fRes, delta = 10 * E, Nf = 0, x3, iters = 0, fx1, fx2;
            double x1 = (a + b) / 2;
            double x2 = x1 + delta;
            while (true)
            {
                fx1 = Function(x1);
                fx2 = Function(x2);
                Nf = Nf + 1;
                if (fx1 > fx2)
                {
                    x3 = x1 + 2 * delta;
                }
                else
                {
                    x3 = x1 - delta;
                }
                fx3 = Function(x3);
                Nf = Nf + 1;
                fMin = min(fx1, fx2, fx3);
                if (fMin == fx1)
                {
                    xMin = x1;
                }
                else if (fMin == fx2)
                {
                    xMin = x2;
                }
                else
                {
                    xMin = x3;
                }
                iters++;
                if (((x2 - x3) * fx1 + (x1 - x2) * fx3 + (x3 - x1) * fx2) == 0)
                {
                    x1 = xMin;
                    x2 = x1 + delta;
                    continue;
                }
                x_res = 0.5 * ((Math.Pow(x2, 2) - Math.Pow(x3, 2)) * fx1 + (Math.Pow(x1, 2) - Math.Pow(x2, 2)) * fx3 + (Math.Pow(x3, 2) - Math.Pow(x1, 2)) * fx2) /
                                ((x2 - x3) * fx1 + (x1 - x2) * fx3 + (x3 - x1) * fx2);
                fRes = Function(x_res);
                Nf = Nf + 1;
                if ((Math.Abs((fMin - fRes) / fRes)) < E)
                {
                    break;
                }
                if (xMin >= x1 && xMin <= x3)
                {
                    x2 = xMin;
                    x1 = x2 - delta;
                }
                else
                {
                    x1 = xMin;
                    x2 = x1 + delta;
                }
                iterCount++;
            }
            time.Text = watch.Elapsed.TotalMilliseconds.ToString() + " мс";
            watch.Stop();
            x_res = x1;
            double fx_res = Function(x_res);
            chart1.Series[1].Points.AddXY(x_res, fx_res);
            chart1.Series[1].Color = Color.Red;
            x_text.Text = "мінімум " + fx_res.ToString() + " 5";
            x_rres_text.Text = "значення х: " + x_res.ToString();
            iter_text.Text = "кількість обчислень функцій " + count_callFunc.ToString();
            iter_count.Text = "кількість ітерацій " + iterCount.ToString();
            chart1.Series[1].MarkerSize = 8;
            Console.WriteLine(x_res + "\n" + Function(x_res));
        }

        public double min(double x1, double x2, double x3)
        {
            double tmp = Math.Min(x1, x2);
            return Math.Min(tmp, x3);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.Add("Корінь");
        }

        
    }
}
