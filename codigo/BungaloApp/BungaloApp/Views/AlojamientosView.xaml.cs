using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace BungaloApp
{
    public partial class AlojamientosView : UserControl
    {
        private static readonly string[] Estados = { "Disponible", "Ocupado", "En Mantenimiento", "Esp. Limpieza", "En limpieza" };

        private ICollectionView vista;
        private Alojamiento actual;
        private bool nuevo;
        private bool cargando;
        private string estadoTmp = "Disponible";
        private bool limpioTmp = true;

        public AlojamientosView(string seleccion = null)
        {
            InitializeComponent();

            cmbFiltroTipo.ItemsSource = new[] { "Todos", "Bungalow", "Cabaña", "Parcela" };
            cmbFiltroTipo.SelectedIndex = 0;
            cmbTipo.ItemsSource = new[] { "Bungalow", "Cabaña", "Parcela" };
            cmbTemporada.ItemsSource = new[] { "Alta", "Media", "Baja" };
            Loc.Combo(cmbFiltroTipo);
            Loc.Combo(cmbTipo);
            Loc.Combo(cmbTemporada);

            vista = CollectionViewSource.GetDefaultView(DataStore.Alojamientos);
            vista.Filter = Filtrar;
            dgAloj.ItemsSource = vista;
            ActualizarTotales();

            var sel = DataStore.Alojamientos.FirstOrDefault(a => a.Nombre == seleccion) ?? DataStore.Alojamientos.FirstOrDefault();
            if (sel != null) dgAloj.SelectedItem = sel;
            else NuevoFormulario();

            Loaded += (s, e) => { AppSettings.IdiomaChanged -= OnIdioma; AppSettings.IdiomaChanged += OnIdioma; };
            Unloaded += (s, e) => AppSettings.IdiomaChanged -= OnIdioma;
            Loc.ApplyVisualTree(this);
        }

        private void OnIdioma(object sender, EventArgs e) { ActualizarIdioma(); }

        private void ActualizarIdioma()
        {
            dgAloj.Items.Refresh();
            ActualizarHeaders();
            ActualizarTotales();
            ActualizarEstadoUI();
            Loc.ApplyVisualTree(this);
        }

        public void ActualizarHeaders()
        {
            if (dgAloj.Columns.Count >= 3)
            {
                dgAloj.Columns[0].Header = Loc.T("hdNombre");
                dgAloj.Columns[1].Header = Loc.T("hdTipo");
                dgAloj.Columns[2].Header = Loc.T("hdEstado");
            }
        }

        private bool Filtrar(object o)
        {
            var a = o as Alojamiento;
            string t = cmbFiltroTipo.SelectedItem as string ?? "Todos";
            return a != null && (t == "Todos" || a.Tipo == t);
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (vista != null) vista.Refresh();
        }

        private void ActualizarTotales()
        {
            lblTotales.Text = Loc.F("totalAlojamientos", DataStore.Alojamientos.Count,
                DataStore.Alojamientos.Count(a => a.Estado == "Disponible"));
        }

        private void dgAloj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            var a = dgAloj.SelectedItem as Alojamiento;
            if (a != null) Mostrar(a);
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            dgAloj.SelectedItem = null;
            NuevoFormulario();
        }

        private void NuevoFormulario()
        {
            Mostrar(new Alojamiento
            {
                Nombre = "", Tipo = "Bungalow", Estado = "Disponible", Limpio = true, CapacidadMax = 4, PersonasAlojadas = 0,
                PrecioNoche = 50, Tamano = 20, Temporada = "Baja", Ubicacion = "", Descripcion = "", Notas = "", Wifi = true
            });
        }

        private void Mostrar(Alojamiento a)
        {
            cargando = true;
            actual = a;
            nuevo = !DataStore.Alojamientos.Contains(a);
            estadoTmp = a.Estado;
            limpioTmp = a.Limpio;

            txtNombre.Text = a.Nombre;
            cmbTipo.SelectedItem = a.Tipo;
            txtCap.Text = a.CapacidadMax.ToString();
            txtPers.Text = a.PersonasAlojadas.ToString();
            txtPrecio.Text = a.PrecioNoche.ToString("0.##", CultureInfo.CurrentCulture);
            txtM2.Text = a.Tamano.ToString();
            cmbTemporada.SelectedItem = a.Temporada;
            chkWifi.IsChecked = a.Wifi; chkAire.IsChecked = a.Aire; chkLav.IsChecked = a.Lavadora;
            chkTv.IsChecked = a.Tv; chkMasc.IsChecked = a.Mascotas;
            txtUbicacion.Text = a.Ubicacion;
            txtDesc.Text = a.Descripcion;
            ActualizarEstadoUI();
            ActualizarImagen();
            cargando = false;
        }

        private void ActualizarEstadoUI()
        {
            lblEstado.Text = Loc.T("estado") + ": " + Loc.Translate(estadoTmp);
            lblLimpio.Text = Loc.T("limpio") + ": " + Loc.T(limpioTmp ? "rawClean" : "rawDirty");
        }

        private void ActualizarImagen()
        {
            string tipo = cmbTipo.SelectedItem as string;
            imgAloj.Source = Util.Img(tipo == "Parcela" ? "parcela.jpg" : "bungalow.jpg");
        }

        private void cmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!cargando) ActualizarImagen();
        }

        private void CambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            var menu = new ContextMenu { PlacementTarget = btnEstado, Placement = PlacementMode.Bottom };
            foreach (string s in Estados)
            {
                string estado = s;
                var mi = new MenuItem { Header = Loc.Translate(estado), IsChecked = estado == estadoTmp };
                mi.Click += (o, a) => { estadoTmp = estado; ActualizarEstadoUI(); };
                menu.Items.Add(mi);
            }
            menu.IsOpen = true;
        }

        private void Limpieza_Click(object sender, RoutedEventArgs e)
        {
            estadoTmp = "Esp. Limpieza";
            limpioTmp = false;
            ActualizarEstadoUI();
            MessageBox.Show(Loc.T("alojLimpiezaMsg"), "Bungalo APP",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            int cap, pers, m2; decimal precio;
            string nombre = txtNombre.Text.Trim();
            string msg = null;

            if (nombre == "") msg = Loc.T("vNombre");
            else if (DataStore.Alojamientos.Any(x => x != actual && x.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))) msg = Loc.T("vAlojDup");
            else if (cmbTipo.SelectedItem == null) msg = Loc.T("vAlojTipo");
            else if (!int.TryParse(txtCap.Text, out cap) || cap <= 0) msg = Loc.T("vAlojCap");
            else if (!int.TryParse(txtPers.Text, out pers) || pers < 0 || pers > int.Parse(txtCap.Text)) msg = Loc.T("vAlojPers");
            else if (!decimal.TryParse(txtPrecio.Text.Replace("€", "").Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio) || precio < 0) msg = Loc.T("vPrecio");
            else if (!int.TryParse(txtM2.Text, out m2) || m2 <= 0) msg = Loc.T("vAlojTam");
            else if (cmbTemporada.SelectedItem == null) msg = Loc.T("vAlojTemp");

            if (msg != null)
            {
                MessageBox.Show(msg, Loc.T("rawInvalidData"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Si cambia el nombre, actualizar también las reservas que lo usan
            if (!nuevo && actual.Nombre != nombre)
                foreach (var r in DataStore.Reservas.Where(r => r.Alojamiento == actual.Nombre)) r.Alojamiento = nombre;

            var a = actual;
            a.Nombre = nombre;
            a.Tipo = (string)cmbTipo.SelectedItem;
            a.Estado = estadoTmp; a.Limpio = limpioTmp;
            a.CapacidadMax = int.Parse(txtCap.Text);
            a.PersonasAlojadas = int.Parse(txtPers.Text);
            a.PrecioNoche = decimal.Parse(txtPrecio.Text.Replace("€", "").Trim(), NumberStyles.Number, CultureInfo.CurrentCulture);
            a.Tamano = int.Parse(txtM2.Text);
            a.Temporada = (string)cmbTemporada.SelectedItem;
            a.Wifi = chkWifi.IsChecked == true; a.Aire = chkAire.IsChecked == true; a.Lavadora = chkLav.IsChecked == true;
            a.Tv = chkTv.IsChecked == true; a.Mascotas = chkMasc.IsChecked == true;
            a.Ubicacion = txtUbicacion.Text; a.Descripcion = txtDesc.Text;

            if (nuevo) DataStore.Alojamientos.Add(a);
            vista.Refresh();
            dgAloj.Items.Refresh();
            ActualizarTotales();
            dgAloj.SelectedItem = a;
            MessageBox.Show(Loc.T("alojGuardado"), "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) { NuevoFormulario(); return; }
            if (DataStore.Reservas.Any(r => r.Alojamiento == actual.Nombre && r.Estado != "Eliminada" && r.Estado != "Cancelada"))
            {
                MessageBox.Show(Loc.T("alojNoElim"), "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show(Loc.F("alojElim", actual.Nombre), Loc.T("rawConfirm"), MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            DataStore.Alojamientos.Remove(actual);
            ActualizarTotales();
            dgAloj.SelectedItem = null;
            NuevoFormulario();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) NuevoFormulario(); else Mostrar(actual);
        }

        private void dgAloj_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var alojamiento = dgAloj.SelectedItem as Alojamiento;
            if (alojamiento != null)
            {
                Mostrar(alojamiento);
            }
        }
    }
}
