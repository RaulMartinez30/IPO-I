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

            // Headers de Reservas
            { "hdFechaEntrada", new[]{ "FECHA ENTRADA", "CHECK-IN DATE" } },
            { "hdIdCliente",    new[]{ "ID - CLIENTE", "ID - CLIENT" } },
            { "hdAlojamiento",  new[]{ "ALOJAMIENTO", "ACCOMMODATION" } },
            { "hdEstado",       new[]{ "ESTADO", "STATUS" } },
            { "hdNombre",       new[]{ "NOMBRE", "NAME" } },
            { "hdApellido",     new[]{ "APELLIDO", "SURNAME" } },
            { "hdFecha",        new[]{ "FECHA", "DATE" } },
            { "hdDia",          new[]{ "DÍA", "DAY" } },
            { "hdHora",         new[]{ "HORA", "TIME" } },
            { "hdActividad",    new[]{ "ACTIVIDAD", "ACTIVITY" } },
            { "hdMonitor",      new[]{ "MONITOR", "INSTRUCTOR" } },
            { "hdPlazas",       new[]{ "PLAZAS", "SEATS" } },
            { "hdTipo",         new[]{ "TIPO", "TYPE" } },
            { "hdTelefono",     new[]{ "TELÉFONO", "PHONE" } },
            { "hdEmail",        new[]{ "EMAIL", "EMAIL" } },
            { "hdEmpleado",     new[]{ "EMPLEADO", "EMPLOYEE" } },
            { "hdArea",         new[]{ "ÁREA", "AREA" } },

            // Labels de formularios
            { "lblNombre",      new[]{ "NOMBRE", "NAME" } },
            { "lblApellido",    new[]{ "APELLIDO", "SURNAME" } },
            { "lblTipo",        new[]{ "TIPO", "TYPE" } },
            { "lblEstado",      new[]{ "ESTADO", "STATUS" } },
            { "lblCapacidad",   new[]{ "CAPACIDAD MÁXIMA", "MAX CAPACITY" } },
            { "lblPersonas",    new[]{ "PERSONAS ALOJADAS", "PEOPLE LODGED" } },
            { "lblPrecio",      new[]{ "PRECIO/NOCHE", "PRICE/NIGHT" } },
            { "lblTamano",      new[]{ "TAMAÑO (m²)", "SIZE (m²)" } },
            { "lblTemporada",   new[]{ "TEMPORADA", "SEASON" } },
            { "lblUbicacion",   new[]{ "UBICACIÓN", "LOCATION" } },
            { "lblDescripcion", new[]{ "DESCRIPCIÓN", "DESCRIPTION" } },
            { "lblNotas",       new[]{ "NOTAS", "NOTES" } },
            { "lblWifi",        new[]{ "WiFi", "WiFi" } },
            { "lblAire",        new[]{ "Aire Acondicionado", "Air Conditioning" } },
            { "lblLavadora",    new[]{ "Lavadora", "Washing Machine" } },
            { "lblTV",          new[]{ "TV", "TV" } },
            { "lblMascotas",    new[]{ "Mascotas permitidas", "Pets allowed" } },

            // Botones estándar
            { "btnGuardar",     new[]{ "Guardar", "Save" } },
            { "btnEliminar",    new[]{ "Eliminar", "Delete" } },
            { "btnCancelar",    new[]{ "Cancelar", "Cancel" } },
            { "btnNuevo",       new[]{ "AÑADIR NUEVO", "ADD NEW" } },
            { "btnLimpiar",     new[]{ "Limpiar filtros", "Clear filters" } },

            // Tooltips
            { "ttMapa",         new[]{ "Haz clic para ver los alojamientos", "Click to view accommodations" } },
            { "ttAvisos",       new[]{ "Ver avisos y alertas del sistema", "View system alerts" } },
            { "ttIdioma",       new[]{ "Cambiar idioma de la aplicación", "Change application language" } },
            { "ttAyuda",        new[]{ "Obtener ayuda y documentación", "Get help and documentation" } },
            { "ttAjustes",      new[]{ "Configurar preferencias de la aplicación", "Configure app settings" } },
            { "ttSalir",        new[]{ "Cerrar séción", "Log out" } },
            { "ttGuardar",      new[]{ "Guardar cambios", "Save changes" } },
            { "ttEliminar",     new[]{ "Eliminar elemento", "Delete item" } },
            { "ttCancelar",     new[]{ "Cancelar cambios", "Cancel changes" } },
            { "ttFiltro",       new[]{ "Filtrar resultados", "Filter results" } },
            { "ttBuscar",       new[]{ "Buscar elemento", "Search item" } },
            { "ttRecargar",     new[]{ "Recargar datos", "Reload data" } },
            { "ttNuevo",        new[]{ "Crear nuevo elemento", "Create new item" } },
        };

        public static string T(string clave)
        {
            string[] v;
            if (!d.TryGetValue(clave, out v)) return clave;
            return v[AppSettings.Idioma == "EN" ? 1 : 0];
        }
    }
}
