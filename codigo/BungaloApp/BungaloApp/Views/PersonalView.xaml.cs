using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace BungaloApp
{
    public partial class PersonalView : UserControl
    {
        private ICollectionView vista;
        private Empleado actual;
        private bool nuevo;
        private bool cargando;
        private string fotoTmp;

        public PersonalView()
        {
            InitializeComponent();
            cmbFiltroEstado.ItemsSource = new[] { "Todos", "Activo", "Baja" };
            cmbFiltroEstado.SelectedIndex = 0;

            vista = CollectionViewSource.GetDefaultView(DataStore.Empleados);
            vista.Filter = Filtrar;
            dgPersonal.ItemsSource = vista;
            ActualizarTotal();

            var primero = DataStore.Empleados.FirstOrDefault();
            if (primero != null) dgPersonal.SelectedItem = primero; else NuevoFormulario();
        }

        private bool Filtrar(object o)
        {
            var emp = o as Empleado;
            if (emp == null) return false;
            string t = (txtBuscar.Text ?? "").Trim().ToLower();
            string est = cmbFiltroEstado.SelectedItem as string ?? "Todos";
            bool okT = t == "" || emp.NombreCompleto.ToLower().Contains(t) || (emp.Especialidad ?? "").ToLower().Contains(t);
            return okT && (est == "Todos" || emp.Estado == est);
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            if (vista != null) vista.Refresh();
        }

        private void ActualizarTotal() { lblTotal.Text = "Personal total: " + DataStore.Empleados.Count; }

        private void dgPersonal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            var emp = dgPersonal.SelectedItem as Empleado;
            if (emp != null) Mostrar(emp);
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            dgPersonal.SelectedItem = null;
            NuevoFormulario();
        }

        private void NuevoFormulario()
        {
            int id = DataStore.Empleados.Count == 0 ? 1001 : DataStore.Empleados.Max(x => x.Id) + 1;
            Mostrar(new Empleado { Id = id, Nombre = "", Apellidos = "", Telefono = "", Email = "", Idiomas = "ES", Especialidad = "", Estado = "Activo", Formacion = "" });
        }

        private void Mostrar(Empleado emp)
        {
            cargando = true;
            actual = emp;
            nuevo = !DataStore.Empleados.Contains(emp);
            fotoTmp = emp.FotoPath;

            txtNombre.Text = emp.Nombre; txtApellidos.Text = emp.Apellidos;
            txtTelefono.Text = emp.Telefono; txtEmail.Text = emp.Email;
            txtIdiomas.Text = emp.Idiomas; txtEspecialidad.Text = emp.Especialidad;
            txtFormacion.Text = emp.Formacion;
            btnBaja.Content = emp.Estado == "Baja" ? "DAR DE ALTA" : "DAR BAJA";
            MostrarFoto();
            cargando = false;
        }

        private void MostrarFoto()
        {
            imgFoto.Source = null;
            lblIniciales.Text = actual != null ? actual.Iniciales : "";
            if (string.IsNullOrEmpty(fotoTmp)) return;
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(fotoTmp);
                bmp.EndInit();
                imgFoto.Source = bmp;
            }
            catch { fotoTmp = null; }
        }

        private void Foto_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp", Title = "Selecciona una foto" };
            if (dlg.ShowDialog() == true)
            {
                fotoTmp = dlg.FileName;
                MostrarFoto();
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string msg = null;
            if (txtNombre.Text.Trim() == "" || txtApellidos.Text.Trim() == "") msg = "Nombre y apellidos son obligatorios.";
            else if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{9}$")) msg = "El teléfono debe tener 9 dígitos.";
            else if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) msg = "El e-mail no es válido.";
            else if (txtIdiomas.Text.Trim() == "") msg = "Indica al menos un idioma.";

            if (msg != null)
            {
                MessageBox.Show(msg, "Datos no válidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var emp = actual;
            emp.Nombre = txtNombre.Text.Trim(); emp.Apellidos = txtApellidos.Text.Trim();
            emp.Telefono = txtTelefono.Text.Trim(); emp.Email = txtEmail.Text.Trim();
            emp.Idiomas = txtIdiomas.Text.Trim(); emp.Especialidad = txtEspecialidad.Text.Trim();
            emp.Formacion = txtFormacion.Text; emp.FotoPath = fotoTmp;

            if (nuevo) DataStore.Empleados.Add(emp);
            vista.Refresh();
            dgPersonal.Items.Refresh();
            ActualizarTotal();
            dgPersonal.SelectedItem = emp;
            MessageBox.Show("Empleado guardado correctamente.", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Baja_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) { NuevoFormulario(); return; }
            bool baja = actual.Estado != "Baja";
            string pregunta = baja ? "¿Dar de baja a " + actual.NombreCompleto + "?" : "¿Dar de alta a " + actual.NombreCompleto + "?";
            if (MessageBox.Show(pregunta, "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            actual.Estado = baja ? "Baja" : "Activo";
            if (baja)
            {
                // Las actividades futuras de este monitor quedan sin asignar
                foreach (var a in DataStore.Actividades.Where(a => a.Monitor == actual.Nombre && a.Fecha >= DateTime.Today))
                    a.Monitor = "Sin asignar";
            }
            vista.Refresh();
            dgPersonal.Items.Refresh();
            btnBaja.Content = baja ? "DAR DE ALTA" : "DAR BAJA";
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (nuevo) NuevoFormulario(); else Mostrar(actual);
        }
    }
}
