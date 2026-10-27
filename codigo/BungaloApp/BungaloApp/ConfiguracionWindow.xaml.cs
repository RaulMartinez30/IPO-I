using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace BungaloApp
{
    public partial class ConfiguracionWindow : Window
    {
        // Copia de lo que se puede previsualizar, para restaurar si se pulsa Cancelar
        private readonly string temaOriginal = AppSettings.Tema;
        private readonly double tamanoOriginal = AppSettings.TamanoTexto;
        private readonly bool contrasteOriginal = AppSettings.AltoContraste;

        public ConfiguracionWindow(int pestana = 0)
        {
            InitializeComponent();
            FontSize = AppSettings.TamanoTexto;
            tabs.SelectedIndex = pestana;

            lblUsuario.Text = "Usuario actual: " + AppSettings.Usuario;
            lblEmail.Text = "E-mail: " + AppSettings.Email;

            Llenar(cmbCierre, new[] { "5 mins", "10 mins", "15 mins", "30 mins", "60 mins" }, AppSettings.CierreSesionMin + " mins");
            Llenar(cmbDaltonico, new[] { "Desactivado", "Protanopia", "Deuteranopia", "Tritanopia" }, AppSettings.ModoDaltonico);
            Llenar(cmbHuso, new[] { "UTC+0 (Londres)", "UTC+1 (Europa/Madrid)", "UTC+2 (Atenas)" }, AppSettings.ZonaHoraria);
            Llenar(cmbCal, new[] { "dd/mm/aaaa", "mm/dd/aaaa", "aaaa-mm-dd" }, AppSettings.FormatoCalendario);
            Llenar(cmbHora, new[] { "24 horas", "12 AM/PM" }, AppSettings.FormatoHora);
            Llenar(cmbMoneda, new[] { "Euro (€)", "Dólar ($)", "Libra (£)" }, AppSettings.Moneda);
            Llenar(cmbMedidas, new[] { "Metros/m²", "Pies/ft²" }, AppSettings.Medidas);
            Llenar(cmbDecimal, new[] { "1.000,00 (Europa)", "1,000.00 (EE. UU.)" }, AppSettings.SeparadorDecimal);
            Llenar(cmbSonido, new[] { "ALARMA", "CAMPANA", "BEEP" }, AppSettings.Sonido);

            rbClaro.IsChecked = AppSettings.Tema == "Claro";
            rbOscuro.IsChecked = AppSettings.Tema == "Oscuro";
            rbSistema.IsChecked = AppSettings.Tema == "Sistema";

            sldTexto.Value = AppSettings.TamanoTexto;
            chkContraste.IsChecked = AppSettings.AltoContraste;
            chkAnimaciones.IsChecked = AppSettings.ReducirAnimaciones;

            chkNReservas.IsChecked = AppSettings.NotifReservas;
            chkNCancel.IsChecked = AppSettings.NotifCancelaciones;
            chkNMant.IsChecked = AppSettings.NotifMantenimiento;
            chkNLimp.IsChecked = AppSettings.NotifLimpieza;
            chkNCheck.IsChecked = AppSettings.NotifCheckins;
            chkNAct.IsChecked = AppSettings.NotifActSinMonitor;
            chkNEmerg.IsChecked = AppSettings.NotifEmergentes;
            chkNSonido.IsChecked = AppSettings.SonidoImportantes;
            chkSilencio.IsChecked = AppSettings.ModoSilencio;
        }

        private static void Llenar(ComboBox c, string[] items, string seleccion)
        {
            c.ItemsSource = items;
            c.SelectedItem = Array.IndexOf(items, seleccion) >= 0 ? seleccion : items[0];
        }

        /// <summary>Vuelca los controles a AppSettings y aplica tema/tamaño/contraste.</summary>
        private void Guardar()
        {
            AppSettings.CierreSesionMin = int.Parse(((string)cmbCierre.SelectedItem).Split(' ')[0]);
            AppSettings.Tema = rbOscuro.IsChecked == true ? "Oscuro" : rbSistema.IsChecked == true ? "Sistema" : "Claro";
            AppSettings.TamanoTexto = sldTexto.Value;
            AppSettings.AltoContraste = chkContraste.IsChecked == true;
            AppSettings.ModoDaltonico = (string)cmbDaltonico.SelectedItem;
            AppSettings.ReducirAnimaciones = chkAnimaciones.IsChecked == true;

            AppSettings.ZonaHoraria = (string)cmbHuso.SelectedItem;
            AppSettings.FormatoCalendario = (string)cmbCal.SelectedItem;
            AppSettings.FormatoHora = (string)cmbHora.SelectedItem;
            AppSettings.Moneda = (string)cmbMoneda.SelectedItem;
            AppSettings.Medidas = (string)cmbMedidas.SelectedItem;
            AppSettings.SeparadorDecimal = (string)cmbDecimal.SelectedItem;

            AppSettings.NotifReservas = chkNReservas.IsChecked == true;
            AppSettings.NotifCancelaciones = chkNCancel.IsChecked == true;
            AppSettings.NotifMantenimiento = chkNMant.IsChecked == true;
            AppSettings.NotifLimpieza = chkNLimp.IsChecked == true;
            AppSettings.NotifCheckins = chkNCheck.IsChecked == true;
            AppSettings.NotifActSinMonitor = chkNAct.IsChecked == true;
            AppSettings.NotifEmergentes = chkNEmerg.IsChecked == true;
            AppSettings.SonidoImportantes = chkNSonido.IsChecked == true;
            AppSettings.Sonido = (string)cmbSonido.SelectedItem;
            AppSettings.ModoSilencio = chkSilencio.IsChecked == true;

            AppSettings.Aplicar();
        }

        private void Aplicar_Click(object sender, RoutedEventArgs e) { Guardar(); }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            Guardar();
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Tema = temaOriginal;
            AppSettings.TamanoTexto = tamanoOriginal;
            AppSettings.AltoContraste = contrasteOriginal;
            AppSettings.Aplicar();
            Close();
        }

        private void CambiarPass_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pendiente de implementar: aquí irá el diálogo para cambiar la contraseña.", "Bungalo APP",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Atajos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Atajos disponibles:\n\nCtrl+1  Panel de Control\nCtrl+2  Reservas\nCtrl+3  Alojamientos\nCtrl+4  Actividades\nCtrl+5  Personal",
                "Atajos de teclado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Probar_Click(object sender, RoutedEventArgs e)
        {
            if (chkSilencio.IsChecked == true)
            {
                MessageBox.Show("El modo silencio está activado.", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            switch ((string)cmbSonido.SelectedItem)
            {
                case "CAMPANA": SystemSounds.Asterisk.Play(); break;
                case "BEEP": SystemSounds.Beep.Play(); break;
                default: SystemSounds.Exclamation.Play(); break;
            }
        }

        private void Abrir(string url)
        {
            try { Process.Start(url); }
            catch { MessageBox.Show("No se pudo abrir: " + url); }
        }

        private void Docs_Click(object sender, RoutedEventArgs e) { Abrir("https://github.com/RaulMartinez30/IPO-I"); }

        private void Soporte_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Soporte técnico: soporte@bungalo.example (dirección de ejemplo)", "Bungalo APP",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Terminos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Términos del servicio (pendiente de redactar).", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Privacidad_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Política de privacidad (pendiente de redactar).", "Bungalo APP", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
