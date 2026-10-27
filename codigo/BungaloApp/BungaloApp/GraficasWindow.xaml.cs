using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BungaloApp
{
    public partial class GraficasWindow : Window
    {
        // Datos de ejemplo (3 días). Sustituir por datos reales cuando haya histórico.
        private readonly string[] dias;
        private readonly double[] entradas = { 8, 10, 12 };
        private readonly double[] libres = { 22, 25, 19 };
        private readonly double[] ocupacion = { 80, 85, 88 };
        private readonly double[] personal = { 9, 10, 10 };
        private static readonly string[] colores = { "#1A73E8", "#E53935", "#43A047", "#FB8C00" };

        public GraficasWindow()
        {
            InitializeComponent();
            DateTime h = DateTime.Today;
            dias = new[] { h.AddDays(-2).ToString("dd/MM"), h.AddDays(-1).ToString("dd/MM"), h.ToString("dd/MM") };
            lblRango.Text = h.AddDays(-2).ToString("dd/MM/yyyy") + " - " + h.ToString("dd/MM/yyyy");
            cmbMetrica.ItemsSource = new[] { "NÚMERO DE ENTRADAS", "PARCELAS LIBRES", "OCUPACIÓN (%)", "PERSONAL ACTIVO" };
            cmbMetrica.SelectedIndex = 0;
        }

        private void Cambio(object sender, RoutedEventArgs e)
        {
            if (lienzo == null || cmbMetrica.SelectedIndex < 0 || rbCirculo == null) return;
            Dibujar();
        }

        private double[] Serie()
        {
            switch (cmbMetrica.SelectedIndex)
            {
                case 1: return libres;
                case 2: return ocupacion;
                case 3: return personal;
                default: return entradas;
            }
        }

        private void Texto(string s, double x, double y, double size = 12, bool negrita = false)
        {
            var t = new TextBlock { Text = s, FontSize = size, Foreground = Brushes.Black };
            if (negrita) t.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(t, x); Canvas.SetTop(t, y);
            lienzo.Children.Add(t);
        }

        private static Brush Color(int i)
        {
            return (Brush)new BrushConverter().ConvertFromString(colores[i % colores.Length]);
        }

        private void Dibujar()
        {
            lienzo.Children.Clear();
            double[] v = Serie();
            if (rbCirculo.IsChecked == true) DibujarCirculo(v);
            else DibujarEjes(v, rbBarras.IsChecked == true);
        }

        private void DibujarEjes(double[] v, bool barras)
        {
            double izq = 50, abajo = 300, arriba = 30, ancho = 620;
            double max = Math.Max(1, v.Max() * 1.2);

            for (int i = 0; i <= 4; i++)
            {
                double y = abajo - (abajo - arriba) * i / 4.0;
                lienzo.Children.Add(new Line { X1 = izq, X2 = izq + ancho, Y1 = y, Y2 = y, Stroke = Brushes.LightGray, StrokeThickness = 1 });
                Texto(Math.Round(max * i / 4.0).ToString(), 15, y - 8, 11);
            }
            lienzo.Children.Add(new Line { X1 = izq, X2 = izq, Y1 = arriba, Y2 = abajo, Stroke = Brushes.Black, StrokeThickness = 2 });
            lienzo.Children.Add(new Line { X1 = izq, X2 = izq + ancho, Y1 = abajo, Y2 = abajo, Stroke = Brushes.Black, StrokeThickness = 2 });

            double paso = ancho / v.Length;
            var linea = new Polyline { Stroke = Color(0), StrokeThickness = 3 };

            for (int i = 0; i < v.Length; i++)
            {
                double cx = izq + paso * (i + 0.5);
                double y = abajo - (abajo - arriba) * v[i] / max;
                Texto(dias[i], cx - 18, abajo + 6);
                Texto(v[i].ToString(), cx - 8, y - 20, 12, true);

                if (barras)
                {
                    var r = new Rectangle { Width = paso * 0.5, Height = abajo - y, Fill = Color(i), Stroke = Brushes.Black, StrokeThickness = 1 };
                    Canvas.SetLeft(r, cx - paso * 0.25); Canvas.SetTop(r, y);
                    lienzo.Children.Add(r);
                }
                else
                {
                    linea.Points.Add(new Point(cx, y));
                    var p = new Ellipse { Width = 10, Height = 10, Fill = Color(0), Stroke = Brushes.Black };
                    Canvas.SetLeft(p, cx - 5); Canvas.SetTop(p, y - 5);
                    lienzo.Children.Add(p);
                }
            }
            if (!barras) lienzo.Children.Insert(0, linea);
        }

        private void DibujarCirculo(double[] v)
        {
            double total = v.Sum();
            Point c = new Point(220, 170);
            double radio = 130, ang = -Math.PI / 2;

            for (int i = 0; i < v.Length; i++)
            {
                double barrido = 2 * Math.PI * v[i] / total;
                Point ini = new Point(c.X + radio * Math.Cos(ang), c.Y + radio * Math.Sin(ang));
                Point fin = new Point(c.X + radio * Math.Cos(ang + barrido), c.Y + radio * Math.Sin(ang + barrido));

                var fig = new PathFigure { StartPoint = c, IsClosed = true };
                fig.Segments.Add(new LineSegment(ini, true));
                fig.Segments.Add(new ArcSegment(fin, new Size(radio, radio), 0, barrido > Math.PI, SweepDirection.Clockwise, true));
                var geo = new PathGeometry(); geo.Figures.Add(fig);
                lienzo.Children.Add(new Path { Data = geo, Fill = Color(i), Stroke = Brushes.White, StrokeThickness = 2 });

                // Leyenda
                var cuadro = new Rectangle { Width = 16, Height = 16, Fill = Color(i), Stroke = Brushes.Black };
                Canvas.SetLeft(cuadro, 430); Canvas.SetTop(cuadro, 120 + i * 32);
                lienzo.Children.Add(cuadro);
                Texto(dias[i] + ": " + v[i] + " (" + Math.Round(100 * v[i] / total) + "%)", 456, 119 + i * 32, 13);

                ang += barrido;
            }
        }
    }
}
