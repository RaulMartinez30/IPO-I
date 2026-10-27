using System.Collections.Generic;

namespace BungaloApp
{
    /// <summary>Textos ES/EN para la ventana principal y el panel de control.</summary>
    public static class Loc
    {
        private static readonly Dictionary<string, string[]> d = new Dictionary<string, string[]>
        {
            { "panel",        new[]{ "Panel de Control", "Dashboard" } },
            { "reservas",     new[]{ "Reservas", "Bookings" } },
            { "alojamientos", new[]{ "Alojamientos", "Accommodation" } },
            { "actividades",  new[]{ "Actividades", "Activities" } },
            { "personal",     new[]{ "Personal", "Staff" } },
            { "resumen",      new[]{ "RESUMEN DEL COMPLEJO", "RESORT OVERVIEW" } },
            { "entradas",     new[]{ "ENTRADAS HOY", "CHECK-INS TODAY" } },
            { "parcelas",     new[]{ "ALOJ. LIBRES", "FREE UNITS" } },
            { "ocupacion",    new[]{ "OCUPACIÓN", "OCCUPANCY" } },
            { "activo",       new[]{ "PERSONAL ACTIVO", "ACTIVE STAFF" } },
            { "graficas",     new[]{ "VER GRÁFICAS", "VIEW CHARTS" } },
            { "mapa",         new[]{ "MAPA RECINTO", "RESORT MAP" } },
            { "mapaTitulo",   new[]{ "MAPA DE DISPONIBILIDAD", "AVAILABILITY MAP" } },
            { "proxAct",      new[]{ "Próximas Actividades", "Upcoming Activities" } },
            { "ultimo",       new[]{ "Último acceso", "Last login" } },
            { "avisos",       new[]{ "AVISOS", "ALERTS" } },
            { "sinAvisos",    new[]{ "Sin avisos pendientes", "No pending alerts" } },
            { "cambiarIdioma",new[]{ "Cambiar idioma a INGLÉS", "Switch language to SPANISH" } },
            { "ayuda",        new[]{ "Ayuda", "Help" } },
            { "ajustes",      new[]{ "Configuración", "Settings" } },
            { "salir",        new[]{ "Cerrar sesión", "Log out" } },
            { "libre",        new[]{ "Libre", "Free" } },
            { "ocupado",      new[]{ "Ocupado", "Occupied" } },
            { "reservado",    new[]{ "Reservado", "Reserved" } },
            { "pistaMapa",    new[]{ "Haz clic en el mapa para ir a Alojamientos", "Click the map to open Accommodation" } },
            { "avLimpieza",   new[]{ "{0} alojamiento(s) a la espera de limpieza", "{0} unit(s) waiting for cleaning" } },
            { "avCheckins",   new[]{ "{0} check-in(s) pendientes", "{0} pending check-in(s)" } },
            { "avActividad",  new[]{ "{0} actividad(es) sin monitor", "{0} activity(ies) without instructor" } },
            { "confSalir",    new[]{ "¿Seguro que quieres cerrar sesión?", "Do you really want to log out?" } },
        };

        public static string T(string clave)
        {
            string[] v;
            if (!d.TryGetValue(clave, out v)) return clave;
            return v[AppSettings.Idioma == "EN" ? 1 : 0];
        }
    }
}
