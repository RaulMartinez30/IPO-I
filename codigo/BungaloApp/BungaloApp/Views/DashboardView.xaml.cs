using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BungaloApp
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            Cargar();
        }

        private void Cargar()
        {
            lblResumen.Text = Loc.T("resumen");
            lblMapa.Text = Loc.T("mapa");
            lblMapaTitulo.Text = Loc.T("mapaTitulo");
            lblLibre.Text = Loc.T("libre");
            lblOcupado.Text = Loc.T("ocupado");
            lblReservado.Text = Loc.T("reservado");
            lblPistaMapa.Text = Loc.T("pistaMapa");
            lblProxAct.Text = Loc.T("proxAct");
            btnGraficas.Content = Loc.T("graficas");
            imgMapa.Source = Util.Img("mapa.jpg");

            int total = DataStore.Alojamientos.Count;
            int libres = DataStore.Alojamientos.Count(a => a.Estado == "Disponible");
            int ocupados = DataStore.Alojamientos.Count(a => a.Estado == "Ocupado");
            int ocup = total == 0 ? 0 : (int)Math.Round(100.0 * ocupados / total);

            kpiEntradas.Text = "[ " + Loc.T("entradas") + " ]: " + DataStore.Reservas.Count(r => r.FechaEntrada.Date == DateTime.Today && r.Estado != "Eliminada" && r.Estado != "Cancelada");
            kpiLibres.Text = "[ " + Loc.T("parcelas") + " ]: " + libres;
            kpiOcupacion.Text = "[ " + Loc.T("ocupacion") + " ]: " + ocup + "%";
            kpiPersonal.Text = "[ " + Loc.T("activo") + " ]: " + DataStore.Empleados.Count(x => x.Estado == "Activo");

            dgReservas.ItemsSource = DataStore.Reservas.OrderByDescending(r => r.FechaEntrada).Take(6).ToList();
            dgActividades.ItemsSource = DataStore.Actividades
                .Where(a => a.Fecha >= DateTime.Today).OrderBy(a => a.Fecha).ThenBy(a => a.Hora).Take(6).ToList();
        }

        private void btnGraficas_Click(object sender, RoutedEventArgs e)
        {
            new GraficasWindow { Owner = Window.GetWindow(this) }.ShowDialog();
        }

        private void imgMapa_Click(object sender, MouseButtonEventArgs e)
        {
            var w = Window.GetWindow(this) as MainWindow;
            if (w != null) w.Navegar("alojamientos");
        }
    }
}
