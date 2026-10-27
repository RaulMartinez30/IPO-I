using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BungaloApp
{
    public partial class ReservasView : UserControl
    {
        private ICollectionView vista;
        private Reserva actual;
        private bool nuevo;
        private bool cargando;
        private int ad = 2, ni = 0, be = 0;

        public ReservasView()
        {
            InitializeComponent();

            cmbFiltroEstado.ItemsSource = new[] { "Todos", "Confirmada", "Pendiente", "Eliminada", "Cancelada" };
            cmbFiltroEstado.SelectedIndex = 0;
            cmbAloj.ItemsSource = DataStore.Alojamientos.Select(a => a.Nombre).ToList();

            vista = CollectionViewSource.GetDefaultView(DataStore.Reservas);
            vista.Filter = Filtrar;
            dgReservas.ItemsSource = vista;
            ActualizarTotal();
            NuevoFormulario();
        }

        // ---------- Lista y filtros ----------

        private bool Filtrar(object o)
        {
            var r = o as Reserva;
            if (r == null) return false;
            string t = (txtBuscar.Text ?? "").Trim().ToLower();
            string est = cmbFiltroEstado.SelectedItem as string ?? "Todos";
            bool okTexto = t == "" || r.Cliente.ToLower().Contains(t) || r.Id.ToString().Contains(t);
            bool okEstado = est == "Todos" || r.Estado == est;
            return okTexto && okEstado;
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            if (vista == null) return;
            vista.Refresh();
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = "Reservas totales: " + DataStore.Reservas.Count;
        }

        private void dgReservas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            var r = dgReservas.SelectedItem as Reserva;
            if (r != null) Mostrar(r);
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            dgReservas.SelectedItem = null;
            NuevoFormulario();
        }

        // ---------- Formulario ----------

        private void NuevoFormulario()
        {
            int id = DataStore.Reservas.Count == 0 ? 1001 : DataStore.Reservas.Max(x => x.Id) + 1;
            Mostrar(new Reserva
            {
                Id = id, Cliente = "", Dni = "", Telefono = "", Email = "", Matricula = "", Solicitudes = "",
                Estado = "Pendiente", FechaEntrada = DateTime.Today, FechaSalida = DateTime.Today.AddDays(1),
                HoraEntrada = "14:00", HoraSalida = "12:00", Adultos = 2
            });
        }

        private void Mostrar(Reserva r)
        {
            cargando = true;
            actual = r;
            nuevo = !DataStore.Reservas.Contains(r);

            txtNombre.Text = r.Cliente;
            txtDni.Text = r.Dni;
            txtTelefono.Text = r.Telefono;
            txtEmail.Text = r.Email;
            chkVehiculo.IsChecked = r.Vehiculo;
            txtMatricula.Text = r.Matricula;
            txtMatricula.IsEnabled = r.Vehiculo;
            chkMascota.IsChecked = r.Mascota;
            cmbAloj.SelectedItem = r.Alojamiento;
            ad = r.Adultos; ni = r.Ninos; be = r.Bebes;
            ActualizarOcupantes();
            dpEntrada.SelectedDate = r.FechaEntrada;
            dpSalida.SelectedDate = r.FechaSalida;
            txtHoraEntrada.Text = r.HoraEntrada;
            txtHoraSalida.Text = r.HoraSalida;
            txtPrecio.Text = r.Precio.ToString("0.##", CultureInfo.CurrentCulture);
            rbConfirmada.IsChecked = r.Estado == "Confirmada";
            rbPendiente.IsChecked = r.Estado == "Pendiente";
            rbEliminada.IsChecked = r.Estado == "Eliminada";
            rbCancelada.IsChecked = r.Estado == "Cancelada";
            txtSolicitudes.Text = r.Solicitudes;
            cargando = false;

            ActualizarInfoAloj();
            ActualizarRecordatorio();
        }

        private void ActualizarOcupantes()
        {
            lblAd.Text = ad.ToString(); lblNi.Text = ni.ToString(); lblBe.Text = be.ToString();
            btnOcupantes.Content = ad + " Adultos · " + ni + " Niños · " + be + " Bebés  ▾";
        }

        private void btnOcupantes_Click(object sender, RoutedEventArgs e) { popOcup.IsOpen = true; }

        private void Ocup_Click(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Tag;
            int d = tag.EndsWith("+") ? 1 : -1;
            if (tag.StartsWith("A")) ad = Math.Max(1, Math.Min(20, ad + d));
            else if (tag.StartsWith("N")) ni = Math.Max(0, Math.Min(20, ni + d));
            else be = Math.Max(0, Math.Min(10, be + d));
            ActualizarOcupantes();
        }

        private void chkVehiculo_Changed(object sender, RoutedEventArgs e)
        {
            if (txtMatricula == null) return;
            txtMatricula.IsEnabled = chkVehiculo.IsChecked == true;
            if (chkVehiculo.IsChecked != true) txtMatricula.Text = "";
        }

        private Alojamiento AlojSeleccionado()
        {
            string n = cmbAloj.SelectedItem as string;
            return DataStore.Alojamientos.FirstOrDefault(a => a.Nombre == n);
        }

        private void cmbAloj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            ActualizarInfoAloj();
        }

        private void ActualizarInfoAloj()
        {
            var a = AlojSeleccionado();
            if (a == null)
            {
                lblEquip.Text = ""; lblEstadoAloj.Text = ""; lblLimpioAloj.Text = "";
                imgAloj.Source = null; txtNotasAloj.Text = "";
                return;
            }
            lblEquip.Text = a.EquipamientoTxt;
            lblEstadoAloj.Text = "ESTADO: " + a.Estado;
            lblLimpioAloj.Text = "LIMPIO: " + (a.Limpio ? "Limpio" : "Sucio");
            imgAloj.Source = Util.Img(a.Tipo == "Parcela" ? "parcela.jpg" : "bungalow.jpg");
            txtNotasAloj.Text = a.Notas;
        }

        private void rbEstado_Checked(object sender, RoutedEventArgs e)
        {
            if (cargando || lblRecordatorio == null) return;
            ActualizarRecordatorio();
        }

        private void ActualizarRecordatorio()
        {
            lblRecordatorio.Text = rbEliminada.IsChecked == true
                ? "Recordatorio: la reserva de " + txtNombre.Text.ToUpper() + " se ELIMINARÁ en [2 DÍAS]"
                : "";
        }

        // ---------- Acciones ----------

        private string Estado()
        {
            if (rbConfirmada.IsChecked == true) return "Confirmada";
            if (rbEliminada.IsChecked == true) return "Eliminada";
            if (rbCancelada.IsChecked == true) return "Cancelada";
            return "Pendiente";
        }

        private bool Validar(out string msg)
        {
            TimeSpan t;
            decimal p;
            msg = null;
            if (txtNombre.Text.Trim() == "") msg = "El nombre es obligatorio.";
            else if (!Regex.IsMatch(txtDni.Text.Trim(), @"^\d{8}[A-Za-z]$")) msg = "El DNI debe tener 8 números y una letra.";
            else if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{9}$")) msg = "El teléfono debe tener 9 dígitos.";
            else if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) msg = "El e-mail no es válido.";
            else if (chkVehiculo.IsChecked == true && txtMatricula.Text.Trim() == "") msg = "Indica la matrícula del vehículo.";
            else if (AlojSeleccionado() == null) msg = "Selecciona un alojamiento.";
            else if (dpEntrada.SelectedDate == null || dpSalida.SelectedDate == null) msg = "Indica las fechas de entrada y salida.";
            else if (dpSalida.SelectedDate < dpEntrada.SelectedDate) msg = "La fecha de salida no puede ser anterior a la de entrada.";
            else if (!TimeSpan.TryParse(txtHoraEntrada.Text.Trim(), out t) || !TimeSpan.TryParse(txtHoraSalida.Text.Trim(), out t)) msg = "Las horas deben tener formato HH:mm.";
            else if (!decimal.TryParse(txtPrecio.Text.Replace("€", "").Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out p) || p < 0) msg = "El precio no es válido.";
            return msg == null;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string msg;
            if (!Validar(out msg))
            {
                MessageBox.Show(msg, "Datos no válidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var r = actual;
            r.Cliente = txtNombre.Text.Trim();
            r.Dni = txtDni.Text.Trim().ToUpper();
            r.Telefono = txtTelefono.Text.Trim();
            r.Email = txtEmail.Text.Trim();
            r.Vehiculo = chkVehiculo.IsChecked == true;
            r.Matricula = txtMatricula.Text.Trim();
            r.Mascota = chkMascota.IsChecked == true;
            r.Alojamiento = (string)cmbAloj.SelectedItem;
            r.Adultos = ad; r.Ninos = ni; r.Bebes = be;
            r.FechaEntrada = dpEntrada.SelectedDate.Value;
            r.FechaSalida = dpSalida.SelectedDate.Value;
            r.HoraEntrada = txtHoraEntrada.Text.Trim();
            r.HoraSalida = txtHoraSalida.Text.Trim();
            r.Precio = decimal.Parse(txtPrecio.Text.Replace("€", "").Trim(), NumberStyles.Number, CultureInfo.CurrentCulture);
            r.Estado = Estado();
            r.Solicitudes = txtSolicitudes.Text;

            var a = AlojSeleccionado();
            if (a != null) a.Notas = txtNotasAloj.Text;

            if (nuevo) DataStore.Reservas.Add(r);
            vista.Refresh();
            dgReservas.Items.Refresh();
            ActualizarTotal();
            dgReservas.SelectedItem = r;
            MessageBox.Show("Reserva guardada correctamente.", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) { NuevoFormulario(); return; }
            if (MessageBox.Show("¿Eliminar la reserva de " + actual.Cliente + " del listado?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            DataStore.Reservas.Remove(actual);
            ActualizarTotal();
            dgReservas.SelectedItem = null;
            NuevoFormulario();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) NuevoFormulario(); else Mostrar(actual);
        }

        private void IrAloj_Click(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this) as MainWindow;
            if (w != null) w.Navegar("alojamientos", cmbAloj.SelectedItem as string);
        }
    }
}
