using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BungaloApp
{
    public partial class ActividadesView : UserControl
    {
        private ICollectionView vista;
        private Actividad actual;
        private bool nuevo;
        private bool cargando;

        public ActividadesView()
        {
            InitializeComponent();
            Loc.Combo(cmbFiltroTipo);
            Loc.Combo(cmbMonitor);
            Loaded += (s, e) => { AppSettings.IdiomaChanged -= OnIdioma; AppSettings.IdiomaChanged += OnIdioma; };
            Unloaded += (s, e) => AppSettings.IdiomaChanged -= OnIdioma;

            CargarUbicaciones();
            RecargarMonitores();
            RecargarTipos();

            vista = CollectionViewSource.GetDefaultView(DataStore.Actividades);
            vista.SortDescriptions.Clear();
            vista.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Ascending));
            vista.SortDescriptions.Add(new SortDescription("Hora", ListSortDirection.Ascending));
            vista.Filter = Filtrar;
            dgAct.ItemsSource = vista;
            ActualizarTotal();

            var primera = DataStore.Actividades.OrderBy(a => a.Fecha).ThenBy(a => a.Hora).FirstOrDefault();
            if (primera != null) dgAct.SelectedItem = primera; else NuevoFormulario();
            Loc.ApplyVisualTree(this);
        }

        private static readonly string[] ubicaciones = { "Zona Norte - Lago", "Zona Sur", "Piscina", "Pistas deportivas", "Sala Multiusos", "Recepción" };

        private void OnIdioma(object sender, EventArgs e)
        {
            dgAct.Items.Refresh();
            ActualizarHeaders();
            ActualizarTotal();
            MostrarInscritos();
            CargarUbicaciones();
            Loc.ApplyVisualTree(this);
        }

        // Combo editable: se muestran los nombres traducidos y se guarda siempre el valor en español
        private void CargarUbicaciones()
        {
            string es = Loc.ToEs(cmbUbicacion.Text);
            cmbUbicacion.ItemsSource = ubicaciones.Select(u => Loc.Translate(u)).ToList();
            cmbUbicacion.Text = Loc.Translate(es);
        }

        public void ActualizarHeaders()
        {
            if (dgAct.Columns.Count >= 5)
            {
                dgAct.Columns[0].Header = Loc.T("hdDia");
                dgAct.Columns[1].Header = Loc.T("hdHora");
                dgAct.Columns[2].Header = Loc.T("hdActividad");
                dgAct.Columns[3].Header = Loc.T("hdPlazas");
                dgAct.Columns[4].Header = Loc.T("hdEstado");
            }
        }

        private void RecargarMonitores()
        {
            var l = DataStore.Empleados.Where(x => x.Estado == "Activo").Select(x => x.Nombre).ToList();
            l.Insert(0, "Sin asignar");
            cmbMonitor.ItemsSource = l;
        }

        private void RecargarTipos()
        {
            var l = DataStore.Actividades.Select(a => a.Nombre).Distinct().OrderBy(s => s).ToList();
            l.Insert(0, "Todos");
            string sel = cmbFiltroTipo.SelectedItem as string;
            cmbFiltroTipo.ItemsSource = l;
            cmbFiltroTipo.SelectedItem = (sel != null && l.Contains(sel)) ? sel : "Todos";
        }

        private bool Filtrar(object o)
        {
            var a = o as Actividad;
            if (a == null) return false;
            DateTime? dia = dpFiltroDia.SelectedDate;
            string tipo = cmbFiltroTipo.SelectedItem as string ?? "Todos";
            string mon = (txtFiltroMonitor.Text ?? "").Trim().ToLower();
            return (dia == null || a.Fecha.Date == dia.Value.Date)
                && (tipo == "Todos" || a.Nombre == tipo)
                && (mon == "" || (a.Monitor ?? "").ToLower().Contains(mon));
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            if (vista != null) vista.Refresh();
        }

        private void LimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            dpFiltroDia.SelectedDate = null;
            cmbFiltroTipo.SelectedItem = "Todos";
            txtFiltroMonitor.Text = "";
        }

        private void ActualizarTotal() { lblTotal.Text = Loc.F("totActividades", DataStore.Actividades.Count); }

        private void MostrarInscritos()
        {
            if (actual != null)
                lblInscritos.Text = Loc.F("inscritos", actual.Inscritos, actual.InscAdultos, actual.InscNinos, actual.InscBebes);
        }

        private void dgAct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            var a = dgAct.SelectedItem as Actividad;
            if (a != null) Mostrar(a);
        }

        private void Nueva_Click(object sender, RoutedEventArgs e)
        {
            dgAct.SelectedItem = null;
            NuevoFormulario();
        }

        private void NuevoFormulario()
        {
            Mostrar(new Actividad
            {
                Fecha = DateTime.Today, Hora = "10:00", HoraFin = "11:00", Nombre = "", Monitor = "Sin asignar",
                CupoMin = 1, CupoMax = 10, Ubicacion = "", DestAdultos = true, DestNinos = true, DestBebes = false,
                PrecioHora = 0, PrecioMes = 0, Consideraciones = "N/A", Material = "", Descripcion = ""
            });
        }

        private void Mostrar(Actividad a)
        {
            cargando = true;
            actual = a;
            nuevo = !DataStore.Actividades.Contains(a);

            txtNombre.Text = a.Nombre;
            cmbMonitor.SelectedItem = a.SinMonitor ? "Sin asignar" : a.Monitor;
            dpDia.SelectedDate = a.Fecha;
            txtInicio.Text = a.Hora; txtFin.Text = a.HoraFin;
            chkAdultos.IsChecked = a.DestAdultos; chkNinos.IsChecked = a.DestNinos; chkBebes.IsChecked = a.DestBebes;
            txtMin.Text = a.CupoMin.ToString(); txtMax.Text = a.CupoMax.ToString();
            cmbUbicacion.Text = Loc.Translate(a.Ubicacion);
            MostrarInscritos();
            txtPrecioHora.Text = a.PrecioHora.ToString("0.##", CultureInfo.CurrentCulture);
            txtPrecioMes.Text = a.PrecioMes.ToString("0.##", CultureInfo.CurrentCulture);
            txtConsideraciones.Text = a.Consideraciones;
            txtMaterial.Text = a.Material;
            txtDescripcion.Text = a.Descripcion;
            cargando = false;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan ini, fin; int min, max; decimal ph, pm;
            string msg = null;
            NumberStyles ns = NumberStyles.Number;
            CultureInfo ci = CultureInfo.CurrentCulture;

            if (txtNombre.Text.Trim() == "") msg = Loc.T("vActNombre");
            else if (dpDia.SelectedDate == null) msg = Loc.T("vActDia");
            else if (!TimeSpan.TryParse(txtInicio.Text.Trim(), out ini) || !TimeSpan.TryParse(txtFin.Text.Trim(), out fin)) msg = Loc.T("vHoras");
            else if (TimeSpan.Parse(txtFin.Text.Trim()) <= TimeSpan.Parse(txtInicio.Text.Trim())) msg = Loc.T("vActFin");
            else if (chkAdultos.IsChecked != true && chkNinos.IsChecked != true && chkBebes.IsChecked != true) msg = Loc.T("vActDest");
            else if (!int.TryParse(txtMin.Text, out min) || !int.TryParse(txtMax.Text, out max) || min < 0 || max < 1 || min > max) msg = Loc.T("vActCupo");
            else if (!decimal.TryParse(txtPrecioHora.Text.Replace("€", "").Trim(), ns, ci, out ph) || ph < 0) msg = Loc.T("vPrecioHora");
            else if (!decimal.TryParse(txtPrecioMes.Text.Replace("€", "").Trim(), ns, ci, out pm) || pm < 0) msg = Loc.T("vPrecioMes");
            else if (cmbUbicacion.Text.Trim() == "") msg = Loc.T("vActUbi");

            if (msg != null)
            {
                MessageBox.Show(msg, Loc.T("rawInvalidData"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var a = actual;
            a.Nombre = txtNombre.Text.Trim();
            a.Monitor = (string)cmbMonitor.SelectedItem ?? "Sin asignar";
            a.Fecha = dpDia.SelectedDate.Value;
            a.Hora = txtInicio.Text.Trim(); a.HoraFin = txtFin.Text.Trim();
            a.DestAdultos = chkAdultos.IsChecked == true; a.DestNinos = chkNinos.IsChecked == true; a.DestBebes = chkBebes.IsChecked == true;
            a.CupoMin = int.Parse(txtMin.Text); a.CupoMax = int.Parse(txtMax.Text);
            a.Ubicacion = Loc.ToEs(cmbUbicacion.Text.Trim());
            a.PrecioHora = decimal.Parse(txtPrecioHora.Text.Replace("€", "").Trim(), ns, ci);
            a.PrecioMes = decimal.Parse(txtPrecioMes.Text.Replace("€", "").Trim(), ns, ci);
            a.Consideraciones = txtConsideraciones.Text;
            a.Material = txtMaterial.Text;
            a.Descripcion = txtDescripcion.Text;

            if (nuevo) DataStore.Actividades.Add(a);
            vista.Refresh();
            dgAct.Items.Refresh();
            RecargarTipos();
            ActualizarTotal();
            dgAct.SelectedItem = a;
            MessageBox.Show(Loc.T("actGuardada"), "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) { NuevoFormulario(); return; }
            if (MessageBox.Show(Loc.F("actElim", actual.Nombre), Loc.T("rawConfirm"), MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            DataStore.Actividades.Remove(actual);
            ActualizarTotal();
            RecargarTipos();
            dgAct.SelectedItem = null;
            NuevoFormulario();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) NuevoFormulario(); else Mostrar(actual);
        }

        private void dgAct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var actividad = dgAct.SelectedItem as Actividad;
            if (actividad != null)
            {
                Mostrar(actividad);
            }
        }
    }
}
