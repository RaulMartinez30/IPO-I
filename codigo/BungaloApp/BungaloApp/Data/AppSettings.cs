using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace BungaloApp
{
    /// <summary>Ajustes de la aplicación (en memoria mientras la app está abierta).</summary>
    public static class AppSettings
    {
        // General
        public static string Usuario = "Pascual Duarte";
        public static string Email = "pascualDuarte@esi.com";
        public static int CierreSesionMin = 15;
        public static string Idioma = "ES";            // ES / EN

        // Aspecto y accesibilidad
        public static string Tema = "Claro";           // Claro / Oscuro / Sistema
        public static double TamanoTexto = 13;
        public static bool AltoContraste = false;
        public static string ModoDaltonico = "Desactivado";
        public static bool ReducirAnimaciones = false;

        // Preferencias
        public static string ZonaHoraria = "UTC+1 (Europa/Madrid)";
        public static string FormatoCalendario = "dd/mm/aaaa";
        public static string FormatoHora = "24 horas";
        public static string Moneda = "Euro (€)";
        public static string Medidas = "Metros/m²";
        public static string SeparadorDecimal = "1.000,00 (Europa)";

        // Notificaciones
        public static bool NotifReservas = true, NotifCancelaciones = true, NotifMantenimiento = true,
                           NotifLimpieza = true, NotifCheckins = true, NotifActSinMonitor = true,
                           NotifEmergentes = true, SonidoImportantes = true, ModoSilencio = false;
        public static string Sonido = "ALARMA";

        private static Brush B(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        private static bool SistemaEsClaro()
        {
            try
            {
                object v = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1);
                return v == null || (int)v != 0;
            }
            catch { return true; }
        }

        /// <summary>Aplica tema, contraste y tamaño de texto a toda la aplicación.</summary>
        public static void Aplicar()
        {
            var r = Application.Current.Resources;
            string t = Tema == "Sistema" ? (SistemaEsClaro() ? "Claro" : "Oscuro") : Tema;

            if (AltoContraste)
            {
                r["AppBg"] = B("#000000"); r["PanelBg"] = B("#000000"); r["TextBrush"] = B("#FFFF00");
                r["LineBrush"] = B("#FFFFFF"); r["MutedBrush"] = B("#FFFFFF");
            }
            else if (t == "Oscuro")
            {
                r["AppBg"] = B("#1E2128"); r["PanelBg"] = B("#2A2E37"); r["TextBrush"] = B("#EDEDED");
                r["LineBrush"] = B("#8A8F9A"); r["MutedBrush"] = B("#A0A5B0");
            }
            else
            {
                r["AppBg"] = B("#F2F4F7"); r["PanelBg"] = B("#FFFFFF"); r["TextBrush"] = B("#1E1E1E");
                r["LineBrush"] = B("#000000"); r["MutedBrush"] = B("#6B6B6B");
            }

            foreach (Window w in Application.Current.Windows) w.FontSize = TamanoTexto;
        }
    }
}
