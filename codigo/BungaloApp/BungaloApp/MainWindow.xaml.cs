using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BungaloApp
{
    public partial class MainWindow : Window
    {
        private string actual = "panel";
        private bool suprimir;
        private DateTime ultimaActividad = DateTime.Now;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            imgAvatar.Source = Util.Img("avatar.jpg");
            txtUsuario.Text = AppSettings.Usuario;

            ConstruirTextos();
            ConstruirNav();
            Navegar("panel");

            Loaded += (s, e) => AppSettings.Aplicar();

            // Cierre de sesión automático por inactividad
            PreviewMouseMove += (s, e) => ultimaActividad = DateTime.Now;
            PreviewKeyDown += MainWindow_PreviewKeyDown;
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();
            Closed += (s, e) => timer.Stop();
        }

        // ---------- Textos y navegación ----------

        private void ConstruirTextos()
        {
            txtUltimoAcceso.Text = Loc.T("ultimo") + ": " + DateTime.Now.ToString("dd/MM/yyyy | HH:mm") + "h";
            btnIdioma.Content = AppSettings.Idioma;
            btnIdioma.ToolTip = Loc.T("cambiarIdioma");
            btnAyuda.ToolTip = Loc.T("ayuda");
            btnAjustes.ToolTip = Loc.T("ajustes");
            btnSalir.ToolTip = Loc.T("salir");
            ActualizarAvisos();
        }

        private void ConstruirNav()
        {
            suprimir = true;
            lstNav.Items.Clear();
            AddNav("panel", "📊");
            AddNav("reservas", "📅");
            AddNav("alojamientos", "🏕");
            AddNav("actividades", "🥾");
            AddNav("personal", "👥");
            suprimir = false;
        }

        private void AddNav(string clave, string icono)
        {
            var sp = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            sp.Children.Add(new TextBlock
            {
                Text = icono, FontSize = 28, HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new FontFamily("Segoe UI Emoji")
            });
            sp.Children.Add(new TextBlock
            {
                Text = Loc.T(clave), FontWeight = FontWeights.SemiBold, TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            });
            lstNav.Items.Add(new ListBoxItem { Tag = clave, Content = sp });
        }

        private void MarcarNav(string clave)
        {
            suprimir = true;
            foreach (ListBoxItem it in lstNav.Items)
                if ((string)it.Tag == clave) lstNav.SelectedItem = it;
            suprimir = false;
        }

        /// <summary>Cambia la vista central. Las vistas pueden llamar a este método (p. ej. "Ir al alojamiento").</summary>
        public void Navegar(string clave, string seleccion = null)
        {
            UserControl vista;
            switch (clave)
            {
                case "reservas": vista = new ReservasView(); break;
                case "alojamientos": vista = new AlojamientosView(seleccion); break;
                case "actividades": vista = new ActividadesView(); break;
                case "personal": vista = new PersonalView(); break;
                default: clave = "panel"; vista = new DashboardView(); break;
            }
            contenido.Content = vista;
            actual = clave;
            MarcarNav(clave);
            ActualizarAvisos();
        }

        private void lstNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suprimir) return;
            var it = lstNav.SelectedItem as ListBoxItem;
            if (it != null) Navegar((string)it.Tag);
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ultimaActividad = DateTime.Now;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == 0) return;
            switch (e.Key)
            {
                case Key.D1: Navegar("panel"); e.Handled = true; break;
                case Key.D2: Navegar("reservas"); e.Handled = true; break;
                case Key.D3: Navegar("alojamientos"); e.Handled = true; break;
                case Key.D4: Navegar("actividades"); e.Handled = true; break;
                case Key.D5: Navegar("personal"); e.Handled = true; break;
            }
        }

        // ---------- Cabecera ----------

        private void ActualizarAvisos()
        {
            btnAvisos.Content = "⚠ " + Loc.T("avisos") + ": " + DataStore.ObtenerAvisos().Count;
        }

        private void btnAvisos_Click(object sender, RoutedEventArgs e)
        {
            var menu = new ContextMenu { PlacementTarget = btnAvisos, Placement = PlacementMode.Bottom };
            var avisos = DataStore.ObtenerAvisos();
            if (avisos.Count == 0)
                menu.Items.Add(new MenuItem { Header = Loc.T("sinAvisos"), IsEnabled = false });
            foreach (string a in avisos)
                menu.Items.Add(new MenuItem { Header = a });
            menu.IsOpen = true;
        }

        private void btnIdioma_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Idioma = AppSettings.Idioma == "ES" ? "EN" : "ES";
            ConstruirTextos();
            ConstruirNav();
            Navegar(actual);
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            new ConfiguracionWindow(5) { Owner = this }.ShowDialog();
        }

        private void btnAjustes_Click(object sender, RoutedEventArgs e)
        {
            new ConfiguracionWindow(0) { Owner = this }.ShowDialog();
            ConstruirTextos();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Loc.T("confSalir"), "Bungalo APP", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            VolverALogin();
        }

        private void VolverALogin()
        {
            timer.Stop();
            new LoginWindow().Show();
            Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - ultimaActividad).TotalMinutes >= AppSettings.CierreSesionMin)
            {
                timer.Stop();
                MessageBox.Show("Sesión cerrada por inactividad.", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
                VolverALogin();
            }
        }
    }
}
