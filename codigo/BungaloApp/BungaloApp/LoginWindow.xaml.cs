using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BungaloApp
{
    public partial class LoginWindow : Window
    {
        private string usuarioValido = "admin";
        private string passwordValido = "1234";

        private readonly MonoAnimado mono = new MonoAnimado();

        public LoginWindow()
        {
            InitializeComponent();
            AppSettings.IdiomaChanged += ActualizarIdioma;
            Closed += (s, e) => AppSettings.IdiomaChanged -= ActualizarIdioma;

            hostMono.Child = mono;

            // Los ojos del mono siguen al puntero del ratón
            MouseMove += (s, e) => mono.Seguir(e.GetPosition(mono));

            // Las pupilas siguen al cursor de escritura mientras se teclea el usuario
            txtUsuario.SelectionChanged += (s, e) =>
                mono.Mirar(Math.Min(1.0, txtUsuario.CaretIndex / 22.0) * 2 - 1);

            Loc.ApplyVisualTree(this);
        }

        private void ActualizarIdioma(object sender, EventArgs e)
        {
            lblErrorUsuario.Text = Loc.T("rawRequiredUser");
            lblErrorPassword.Text = Loc.T("rawRequiredPassword");
            Loc.ApplyVisualTree(this);
        }

        // --- EVENTOS DEL MONO INTERACTIVO ---

        private void txtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            mono.Asomar();      // sale del coco (y se destapa los ojos si los tenía tapados)
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (btnMostrarPassword.IsChecked == true) mono.Espiar();
            else mono.Tapar();
        }

        private void txtPasswordVisible_GotFocus(object sender, RoutedEventArgs e)
        {
            mono.Espiar();      // mira entre los dedos porque se ve la clave
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            // Se decide cuando el foco ya ha llegado a su nuevo sitio, para que no haya parpadeos
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!txtUsuario.IsKeyboardFocusWithin && !txtPassword.IsKeyboardFocusWithin
                    && !txtPasswordVisible.IsKeyboardFocusWithin && !btnMostrarPassword.IsKeyboardFocusWithin
                    && !btnLogin.IsKeyboardFocusWithin)
                    mono.Esconder();
            }), DispatcherPriority.Background);
        }

        // --- LÓGICA DEL OJITO DE CONTRASEÑA ---

        private void btnMostrarPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtPasswordVisible.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtPasswordVisible.Visibility = Visibility.Visible;
            txtPasswordVisible.Focus();

            mono.Espiar();
        }

        private void btnMostrarPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtPasswordVisible.Text;
            txtPasswordVisible.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();

            mono.Tapar();
        }

        private void txtPasswordVisible_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (btnMostrarPassword.IsChecked == true)
                txtPassword.Password = txtPasswordVisible.Text;
        }

        // --- LÓGICA DEL LOGIN ---

        private void Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                btnLogin_Click(null, null);
                e.Handled = true;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool hayErrores = false;

            string passwordActual = btnMostrarPassword.IsChecked == true ? txtPasswordVisible.Text : txtPassword.Password;

            lblErrorUsuario.Visibility = Visibility.Collapsed;
            lblErrorPassword.Visibility = Visibility.Collapsed;
            txtUsuario.BorderBrush = Brushes.Black;
            txtPassword.BorderBrush = Brushes.Black;
            txtPasswordVisible.BorderBrush = Brushes.Black;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                lblErrorUsuario.Text = Loc.T("rawRequiredUser");
                lblErrorUsuario.Visibility = Visibility.Visible;
                txtUsuario.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (string.IsNullOrWhiteSpace(passwordActual))
            {
                lblErrorPassword.Text = Loc.T("rawRequiredPassword");
                lblErrorPassword.Visibility = Visibility.Visible;
                txtPassword.BorderBrush = Brushes.Red;
                txtPasswordVisible.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (hayErrores) return;

            if (txtUsuario.Text.Equals(usuarioValido) && passwordActual.Equals(passwordValido))
            {
                MainWindow principal = new MainWindow();
                principal.Show();
                this.Close();
            }
            else
            {
                lblErrorUsuario.Text = Loc.T("rawInvalidLogin");
                lblErrorUsuario.Visibility = Visibility.Visible;
                txtUsuario.BorderBrush = Brushes.Red;
                txtPassword.BorderBrush = Brushes.Red;
                txtPasswordVisible.BorderBrush = Brushes.Red;
                mono.Negar();   // el mono niega con la cabeza
            }
        }
    }

    /// <summary>
    /// Mono animado dibujado con formas vectoriales con el estilo del logo (contornos negros gruesos).
    /// Vive dentro de un coco con sombrilla y pajita y reacciona a lo que hace el usuario:
    /// Asomar (sale del coco), Tapar (se tapa los ojos), Espiar (mira por una rendija entre los dedos),
    /// Esconder (vuelve al coco). Sus ojos siguen al ratón y al cursor de escritura.
    /// </summary>
    public sealed class MonoAnimado : Canvas
    {
        private enum Pose { Escondido, Asomado, Tapando, Espiando }

        private const double YMonoArriba = 8;        // el mono queda algo más hundido en el coco
        private const double YMonoEscondido = 98;    // cuánto baja el mono para quedar dentro del coco
        private const double YManosAbajo = 74;       // cuánto bajan las manos
        private const double Abertura = 10;          // separación de los dedos al espiar
        private const double GiroSombrilla = -26;    // la sombrilla se inclina cuando sale el mono
        private const double DesvioPajita = 34;      // la pajita se desplaza hacia la derecha
        private const double Trazo = 3;              // grosor del contorno

        private Pose pose = Pose.Escondido;
        private double pupX, pupY;

        private readonly TranslateTransform mono = new TranslateTransform(0, YMonoEscondido);
        private readonly TranslateTransform manos = new TranslateTransform(0, YManosAbajo);
        private readonly RotateTransform giro = new RotateTransform(0, 110, 60);
        private readonly ScaleTransform parpadeo = new ScaleTransform(1, 1, 110, 54);
        private readonly TranslateTransform pupilas = new TranslateTransform(0, 0);
        private readonly RotateTransform sombrilla = new RotateTransform(0, 88, 90);
        private readonly TranslateTransform pajita = new TranslateTransform(0, 0);
        private readonly TranslateTransform dedoA = new TranslateTransform();   // dedos de la mano derecha que se separan
        private readonly TranslateTransform dedoB = new TranslateTransform();
        private readonly DispatcherTimer reloj = new DispatcherTimer();
        private readonly Random azar = new Random();

        public MonoAnimado()
        {
            Width = 220;
            Height = 185;
            ClipToBounds = true;
            IsHitTestVisible = false;

            Brush negro = Brushes.Black;
            Brush verde = Pincel("#5BAA47"), verdeOsc = Pincel("#3E8C3A"), rosa = Pincel("#F9CDB9");
            Brush pelo = Pincel("#B98B5F"), piel = Pincel("#F2C6A0");

            // ---- Coco: cuerpo verde con sombreado, rayas y abertura rosa ----
            const string cuenco = "M 52,84 C 48,140 78,178 110,178 C 142,178 172,140 168,84 Z";
            Children.Add(new Path { Data = Geometry.Parse(cuenco), Fill = verde });
            Children.Add(new Path { Data = Geometry.Parse("M 168,92 C 168,140 142,176 110,176 C 90,176 74,168 64,154 C 88,168 128,160 150,128 C 160,114 166,104 168,92 Z"), Fill = verdeOsc });
            Children.Add(new Path { Data = Geometry.Parse(cuenco), Stroke = negro, StrokeThickness = 3.5, StrokeLineJoin = PenLineJoin.Round });
            Children.Add(Trazar("M 72,98 C 68,108 68,120 72,130", 3));
            Children.Add(Trazar("M 88,100 C 85,110 85,122 88,132", 3));
            Children.Add(Trazar("M 140,100 C 142,116 148,132 156,148", 3));
            Elipse(this, 110, 84, 58, 12, rosa, negro, 3.5);

            // ---- Zona donde se ve lo que sale del coco: por encima del borde delantero de la abertura ----
            var salida = new Canvas { Width = 220, Height = 185 };
            salida.Clip = new CombinedGeometry(GeometryCombineMode.Union,
                new RectangleGeometry(new Rect(0, 0, 220, 84)),
                new EllipseGeometry(new Point(110, 84), 57, 12));
            Children.Add(salida);

            // Pajita (negra, doblada en la punta)
            var paj = new Canvas { Width = 220, Height = 185, RenderTransform = new TransformGroup { Children = { new ScaleTransform(1, 0.84, 123, 98), pajita } } };
            paj.Children.Add(new Polyline
            {
                Points = new PointCollection { new Point(123, 98), new Point(123, 19), new Point(154, 19) },
                Stroke = negro, StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round, StrokeEndLineCap = PenLineCap.Round, StrokeLineJoin = PenLineJoin.Round
            });
            salida.Children.Add(paj);

            // Sombrilla (naranja y amarilla)
            var sombr = new Canvas { Width = 220, Height = 185, RenderTransform = new TransformGroup { Children = { new ScaleTransform(0.92, 0.84, 88, 94), sombrilla } } };
            sombr.Children.Add(Trazar("M 88,94 L 88,14", 3));
            var gradiente = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(1, 0) };
            gradiente.GradientStops.Add(new GradientStop(Color.FromRgb(0xF2, 0x8C, 0x28), 0));
            gradiente.GradientStops.Add(new GradientStop(Color.FromRgb(0xF2, 0x8C, 0x28), 0.5));
            gradiente.GradientStops.Add(new GradientStop(Color.FromRgb(0xFB, 0xD3, 0x6B), 0.5));
            gradiente.GradientStops.Add(new GradientStop(Color.FromRgb(0xFB, 0xD3, 0x6B), 1));
            sombr.Children.Add(new Path
            {
                Data = Geometry.Parse("M 62,42 Q 66,18 88,16 Q 110,18 114,42 Q 107,47 101,42 Q 94,48 88,42 Q 82,48 75,42 Q 69,47 62,42 Z"),
                Fill = gradiente, Stroke = negro, StrokeThickness = 2.8, StrokeLineJoin = PenLineJoin.Round
            });
            salida.Children.Add(sombr);

            // ---- Mono ----
            var cuerpo = new Canvas { Width = 220, Height = 185 };
            cuerpo.RenderTransform = new TransformGroup { Children = { giro, mono } };
            salida.Children.Add(cuerpo);

            // hombros, orejas, mechón y cabeza
            cuerpo.Children.Add(new Path
            {
                Data = Geometry.Parse("M 60,92 C 62,72 82,70 110,72 C 138,70 158,72 160,92 Z"),
                Fill = pelo, Stroke = negro, StrokeThickness = Trazo, StrokeLineJoin = PenLineJoin.Round
            });
            Elipse(cuerpo, 64, 52, 12, 12, pelo, negro, Trazo);
            Elipse(cuerpo, 64, 53, 7, 7, piel);
            Elipse(cuerpo, 156, 52, 12, 12, pelo, negro, Trazo);
            Elipse(cuerpo, 156, 53, 7, 7, piel);
            cuerpo.Children.Add(new Path
            {
                Data = Geometry.Parse("M 98,18 Q 98,5 108,10 Q 112,0 118,11 Q 127,7 123,20 Z"),
                Fill = pelo, Stroke = negro, StrokeThickness = Trazo, StrokeLineJoin = PenLineJoin.Round
            });
            Elipse(cuerpo, 110, 50, 41, 35, pelo, negro, Trazo);

            // máscara de la cara (forma de corazón)
            cuerpo.Children.Add(new Path
            {
                Data = Geometry.Parse("M 110,42 C 104,32 86,34 84,48 C 83,60 92,72 104,80 C 108,83 112,83 116,80 C 128,72 137,60 136,48 C 134,34 116,32 110,42 Z"),
                Fill = piel, Stroke = negro, StrokeThickness = 2.5, StrokeLineJoin = PenLineJoin.Round
            });

            // ojos: puntos negros con brillo (parpadean y se mueven)
            var ojos = new Canvas { Width = 220, Height = 185, RenderTransform = parpadeo };
            var pup = new Canvas { Width = 220, Height = 185, RenderTransform = pupilas };
            Elipse(pup, 96, 54, 4.6, 4.8, negro);
            Elipse(pup, 124, 54, 4.6, 4.8, negro);
            Elipse(pup, 94.6, 52.4, 1.5, 1.5, Brushes.White);
            Elipse(pup, 122.6, 52.4, 1.5, 1.5, Brushes.White);
            ojos.Children.Add(pup);
            cuerpo.Children.Add(ojos);

            // nariz y sonrisa
            Elipse(cuerpo, 107.5, 66, 1.3, 1.6, negro);
            Elipse(cuerpo, 112.5, 66, 1.3, 1.6, negro);
            cuerpo.Children.Add(Trazar("M 100,72 Q 110,80 120,72", 2.5));

            // manos y brazos (suben desde el coco para taparle los ojos)
            var capaManos = new Canvas { Width = 220, Height = 185, RenderTransform = manos };
            cuerpo.Children.Add(capaManos);
            CrearMano(capaManos, 79.5, piel, pelo, null, null);          // izquierda: sobre el ojo en x=96
            CrearMano(capaManos, 102.5, piel, pelo, dedoA, dedoB);       // derecha: la rendija queda sobre el ojo en x=124

            // borde delantero del coco (por encima de todo lo que sale)
            Children.Add(Trazar("M 52,84 A 58,12 0 0 0 168,84", 3.5));

            // Parpadeo ocasional (solo mientras el mono está a la vista)
            reloj.Interval = TimeSpan.FromSeconds(3);
            reloj.Tick += (s, e) =>
            {
                reloj.Interval = TimeSpan.FromSeconds(2.5 + azar.NextDouble() * 3);
                if (pose == Pose.Asomado || pose == Pose.Espiando) Parpadear();
            };
            Loaded += (s, e) => reloj.Start();
            Unloaded += (s, e) => reloj.Stop();
        }

        // ------------------------------------------------------------------ poses

        public void Asomar() { Ir(Pose.Asomado); }
        public void Tapar() { Ir(Pose.Tapando); }
        public void Espiar() { Ir(Pose.Espiando); }
        public void Esconder() { Ir(Pose.Escondido); }

        /// <summary>Mira a izquierda/derecha (-1..1) siguiendo el cursor de escritura.</summary>
        public void Mirar(double x)
        {
            if (pose != Pose.Asomado) return;
            MoverPupilas(Math.Max(-1, Math.Min(1, x)) * 2.6, 2);
        }

        /// <summary>Sigue con los ojos un punto (posición del ratón relativa a este control).</summary>
        public void Seguir(Point p)
        {
            if (pose != Pose.Asomado && pose != Pose.Espiando) return;
            double dx = p.X - 110, dy = p.Y - 62;
            double d = Math.Sqrt(dx * dx + dy * dy);
            if (d < 1) d = 1;
            double fuerza = Math.Min(1.0, d / 110.0);
            MoverPupilas(dx / d * fuerza * 2.8, dy / d * fuerza * 2.6);
        }

        private void MoverPupilas(double x, double y)
        {
            if (Math.Abs(x - pupX) < 0.25 && Math.Abs(y - pupY) < 0.25) return;
            pupX = x;
            pupY = y;
            Anim(pupilas, TranslateTransform.XProperty, x, 110, 0, null);
            Anim(pupilas, TranslateTransform.YProperty, y, 110, 0, null);
        }

        /// <summary>Niega con la cabeza (login incorrecto). Si estaba escondido o tapado, primero se asoma.</summary>
        public void Negar()
        {
            Pose antes = pose;
            Asomar();
            int espera = antes == Pose.Escondido ? 600 : (antes == Pose.Asomado ? 0 : 400);

            var k = new DoubleAnimationUsingKeyFrames { BeginTime = TimeSpan.FromMilliseconds(espera) };
            double[] angulos = { 0, -9, 9, -7, 7, -3, 0 };
            for (int i = 0; i < angulos.Length; i++)
                k.KeyFrames.Add(new LinearDoubleKeyFrame(angulos[i], KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * 90))));
            giro.BeginAnimation(RotateTransform.AngleProperty, k);
        }

        private void Ir(Pose nueva)
        {
            if (nueva == pose) return;
            Pose antes = pose;
            pose = nueva;

            bool baja = nueva == Pose.Escondido;
            bool manosArriba = nueva == Pose.Tapando || nueva == Pose.Espiando;

            var salto = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.5 };
            var suave = new CubicEase { EasingMode = EasingMode.EaseInOut };

            // Al esconderse, primero bajan las manos y luego el mono; al taparse desde dentro, primero sale el mono
            int retrasoMono = (baja && antes != Pose.Asomado) ? 280 : 0;
            int retrasoManos = (manosArriba && antes == Pose.Escondido) ? 430 : 0;
            int duracion = baja ? 380 : 560;
            IEasingFunction curva = baja ? (IEasingFunction)suave : salto;

            Anim(mono, TranslateTransform.YProperty, baja ? YMonoEscondido : YMonoArriba, duracion, retrasoMono, curva);
            Anim(manos, TranslateTransform.YProperty, manosArriba ? 0 : YManosAbajo, 340, retrasoManos, manosArriba ? (IEasingFunction)salto : suave);

            // La sombrilla se inclina y la pajita se desplaza para dejar sitio al mono
            Anim(sombrilla, RotateTransform.AngleProperty, baja ? 0 : GiroSombrilla, duracion, retrasoMono, curva);
            Anim(pajita, TranslateTransform.XProperty, baja ? 0 : DesvioPajita, duracion, retrasoMono, curva);

            // Al espiar, la mano derecha separa los dedos y deja ver el ojo derecho
            double abre = nueva == Pose.Espiando ? Abertura : 0;
            Anim(dedoA, TranslateTransform.XProperty, abre, 230, retrasoManos, suave);
            Anim(dedoB, TranslateTransform.XProperty, abre, 230, retrasoManos, suave);

            // Pupilas: miran un poco hacia los campos de texto; al taparse o esconderse vuelven al centro
            bool sigue = nueva == Pose.Asomado || nueva == Pose.Espiando;
            pupY = baja ? 0 : 2;
            Anim(pupilas, TranslateTransform.YProperty, pupY, 250, 0, suave);
            if (!sigue)
            {
                pupX = 0;
                Anim(pupilas, TranslateTransform.XProperty, 0, 200, 0, suave);
            }
        }

        private void Parpadear()
        {
            var a = new DoubleAnimation(1, 0.1, TimeSpan.FromMilliseconds(90)) { AutoReverse = true };
            parpadeo.BeginAnimation(ScaleTransform.ScaleYProperty, a);
        }

        // ------------------------------------------------------------------ utilidades de dibujo

        private static void Anim(Animatable destino, DependencyProperty propiedad, double hasta, int ms, int retrasoMs, IEasingFunction suavizado)
        {
            var a = new DoubleAnimation
            {
                To = hasta,
                Duration = TimeSpan.FromMilliseconds(ms),
                BeginTime = TimeSpan.FromMilliseconds(retrasoMs),
                EasingFunction = suavizado
            };
            destino.BeginAnimation(propiedad, a);
        }

        private static Brush Pincel(string hex)
        {
            var b = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
            b.Freeze();
            return b;
        }

        private static Path Trazar(string datos, double grosor)
        {
            return new Path
            {
                Data = Geometry.Parse(datos), Stroke = Brushes.Black, StrokeThickness = grosor,
                StrokeStartLineCap = PenLineCap.Round, StrokeEndLineCap = PenLineCap.Round, StrokeLineJoin = PenLineJoin.Round
            };
        }

        private static Ellipse Elipse(Panel destino, double cx, double cy, double rx, double ry, Brush relleno, Brush borde = null, double grosor = 0)
        {
            var e = new Ellipse { Width = rx * 2, Height = ry * 2, Fill = relleno, Stroke = borde, StrokeThickness = grosor };
            SetLeft(e, cx - rx);
            SetTop(e, cy - ry);
            destino.Children.Add(e);
            return e;
        }

        private static Rectangle Caja(Panel destino, double x, double y, double w, double h, double radio, Brush relleno)
        {
            var r = new Rectangle { Width = w, Height = h, RadiusX = radio, RadiusY = radio, Fill = relleno, Stroke = Brushes.Black, StrokeThickness = 2.5 };
            SetLeft(r, x);
            SetTop(r, y);
            destino.Children.Add(r);
            return r;
        }

        /// <summary>
        /// Mano de cuatro dedos con su antebrazo. Si se pasan t2 y t3, los dos dedos de la derecha pueden separarse
        /// para abrir una rendija sobre el ojo.
        /// </summary>
        private static void CrearMano(Canvas destino, double izq, Brush piel, Brush pelo, TranslateTransform t2, TranslateTransform t3)
        {
            var mano = new Canvas { Width = 34, Height = 150 };
            SetLeft(mano, izq);
            SetTop(mano, 38);
            destino.Children.Add(mano);

            Caja(mano, 10, 26, 14, 124, 6, pelo);                          // antebrazo
            double[] largos = { 22, 26, 26, 22 };
            for (int i = 0; i < 4; i++)                                     // dedos
            {
                Rectangle dedo = Caja(mano, i * 8, 26 - largos[i], 9, largos[i], 4.5, piel);
                if (i == 2 && t2 != null) dedo.RenderTransform = t2;
                if (i == 3 && t3 != null) dedo.RenderTransform = t3;
            }
            Caja(mano, 0, 18, 33, 14, 6, piel);                             // palma
        }
    }
}
