using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

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

            // Secciones y títulos
            { "secListado",        new[]{ "LISTADO DE ALOJAMIENTOS", "ACCOMMODATION LIST" } },
            { "secDetalles",       new[]{ "DETALLES DEL ALOJAMIENTO", "ACCOMMODATION DETAILS" } },
            { "secGestion",        new[]{ "GESTIÓN DE ACTIVIDADES", "ACTIVITIES MANAGEMENT" } },
            { "secDetallesAct",     new[]{ "DETALLES DE LA ACTIVIDAD", "ACTIVITY DETAILS" } },
            { "secReservas",        new[]{ "GESTIÓN DE RESERVAS", "BOOKINGS MANAGEMENT" } },
            { "secDetallesReserva",  new[]{ "NUEVA / DETALLES DE RESERVA", "NEW / BOOKING DETAILS" } },
            { "secPersonal",        new[]{ "GESTIÓN DE PERSONAL", "STAFF MANAGEMENT" } },
            { "secDetallesEmpl",     new[]{ "DETALLES DE EMPLEADO", "EMPLOYEE DETAILS" } },
            { "secCliente",         new[]{ "👤 CLIENTE", "👤 CLIENT" } },
            { "secAlojamiento",     new[]{ "🏠 ALOJAMIENTO", "🏠 ACCOMMODATION" } },
            { "secDatosTecnicos",   new[]{ "Datos técnicos", "Technical data" } },
            { "secDestinatarios",   new[]{ "DESTINATARIOS:", "RECIPIENTS:" } },

            // Filtros y búsquedas
            { "lblFiltroTipo",      new[]{ "Filtrar por tipo:", "Filter by type:" } },
            { "lblFiltroDia",       new[]{ "DÍA", "DAY" } },
            { "lblBuscar",          new[]{ "Buscar...", "Search..." } },
            { "lblFiltroEstado",    new[]{ "Filtrar por estado:", "Filter by status:" } },
            { "lblBuscarPor",       new[]{ "Buscar por nombre de cliente o ID de reserva", "Search by client name or booking ID" } },
            { "lblBuscarPersonal",  new[]{ "Buscar por nombre o especialidad del empleado", "Search by employee name or specialty" } },
            { "lblBuscarMonitor",   new[]{ "Buscar por nombre de monitor", "Search by instructor name" } },
            { "lblFiltrarAloj",     new[]{ "Filtrar alojamientos por tipo", "Filter accommodations by type" } },
            { "lblFiltrarActividad",new[]{ "Filtrar por tipo de actividad", "Filter by activity type" } },
            { "lblFiltrarEmpleado", new[]{ "Filtrar por estado del empleado", "Filter by employee status" } },
            { "lblFiltrarReserva",  new[]{ "Filtrar por estado de reserva", "Filter by booking status" } },

            // Botones específicos
            { "btnAnadirAloj",      new[]{ "AÑADIR NUEVO ALOJAMIENTO", "ADD NEW ACCOMMODATION" } },
            { "btnAnadirAct",       new[]{ "Nueva Actividad  +", "New Activity  +" } },
            { "btnAnadirCliente",    new[]{ "AÑADIR NUEVO CLIENTE", "ADD NEW CLIENT" } },
            { "btnAnadirEmpl",       new[]{ "Nuevo empleado  +", "New employee  +" } },
            { "btnCambiarEstado",    new[]{ "CAMBIAR ESTADO", "CHANGE STATUS" } },
            { "btnLimpiarRecinto",   new[]{ "SOLICITAR LIMPIEZA", "REQUEST CLEANING" } },
            { "btnLimpiarFiltros",   new[]{ "Limpiar filtros", "Clear filters" } },
            { "btnBaja",            new[]{ "DAR BAJA", "DEACTIVATE" } },
            { "btnSubirFoto",       new[]{ "Subir/cambiar foto", "Upload/change photo" } },
            { "btnCrearNueva",      new[]{ "Crear una nueva reserva", "Create a new booking" } },
            { "btnCrearNuevoAloj",  new[]{ "Crear un nuevo alojamiento", "Create a new accommodation" } },
            { "btnCrearNuevaAct",   new[]{ "Crear una nueva actividad", "Create a new activity" } },
            { "btnCrearNuevoEmpl",  new[]{ "Añadir un nuevo empleado", "Add a new employee" } },

            // Campos de formulario específicos
            { "lblDni",            new[]{ "DNI:", "ID:" } },
            { "lblTelefono",        new[]{ "Teléfono:", "Phone:" } },
            { "lblEmail",           new[]{ "E-mail:", "E-mail:" } },
            { "lblVehiculo",        new[]{ "Vehículo", "Vehicle" } },
            { "lblMatricula",       new[]{ "Matrícula:", "License plate:" } },
            { "lblMascota",         new[]{ "Mascota", "Pet" } },
            { "lblPrecioNoche",      new[]{ "PRECIO NOCHE (€)", "PRICE PER NIGHT (€)" } },
            { "lblCapMaxPersonas",    new[]{ "CAPAC. MÁX. PERSONAS", "MAX. CAPACITY" } },
            { "lblPersonasAlojadas",  new[]{ "PERSONAS ALOJADAS", "LODGED PERSONS" } },
            { "lblEquipamiento",    new[]{ "Equipamiento:", "Equipment:" } },
            { "lblEstadoAloj",      new[]{ "Estado del alojamiento", "Accommodation status" } },
            { "lblLimpioAloj",      new[]{ "Estado de limpieza", "Cleaning status" } },
            { "lblNotasInternas",   new[]{ "Notas (INTERNO):", "Notes (INTERNAL):" } },
            { "lblActividad",       new[]{ "ACTIVIDAD:", "ACTIVITY:" } },
            { "lblMonitor",         new[]{ "MONITOR:", "INSTRUCTOR:" } },
            { "lblDia",             new[]{ "DÍA:", "DAY:" } },
            { "lblInicio",          new[]{ "INICIO (HH:mm)", "START (HH:mm)" } },
            { "lblFin",             new[]{ "FIN (HH:mm)", "END (HH:mm)" } },
            { "lblCupoMin",         new[]{ "CUPO MÍN", "MIN QUOTA" } },
            { "lblCupoMax",         new[]{ "CUPO MÁX", "MAX QUOTA" } },
            { "lblTemporadaForm",   new[]{ "TEMPORADA", "SEASON" } },
            { "lblUbicacionForm",   new[]{ "UBICACIÓN", "LOCATION" } },
            { "lblDescripcionForm", new[]{ "DESCRIPCIÓN", "DESCRIPTION" } },
            { "lblNombreForm",      new[]{ "Nombre", "Name" } },
            { "lblApellidos",       new[]{ "Apellidos", "Surnames" } },
            { "lblIdiomas",         new[]{ "IDIOMAS (ej. ES / EN)", "LANGUAGES (e.g. ES / EN)" } },
            { "lblEspecialidad",    new[]{ "ESPECIALIDADES", "SPECIALTIES" } },
            { "lblFormacion",       new[]{ "FORMACIÓN Y TITULACIÓN", "EDUCATION AND QUALIFICATIONS" } },
            { "lblIniciales",       new[]{ "Iniciales", "Initials" } },
            { "lblTama",            new[]{ "Ingresa el tamaño", "Enter the size" } },
            { "lblTama2",           new[]{ "TAMAÑO (m²)", "SIZE (m²)" } },

            // Configuración - General
            { "tabGeneral",         new[]{ "General", "General" } },
            { "tabAspecto",         new[]{ "Aspecto", "Appearance" } },
            { "tabAccesibilidad",   new[]{ "Accesibilidad", "Accessibility" } },
            { "tabPreferencias",    new[]{ "Preferencias", "Preferences" } },
            { "tabNotificaciones",  new[]{ "Notificaciones", "Notifications" } },
            { "tabSobreNosotros",   new[]{ "Sobre nosotros", "About us" } },
            { "lblInformacionComplejo", new[]{ "Información del Complejo", "Resort Information" } },
            { "lblCuenta",          new[]{ "Cuenta", "Account" } },
            { "lblNombreComplejo",  new[]{ "Nombre del complejo: Bungalo S.L.", "Resort name: Bungalo S.L." } },
            { "lblCif",             new[]{ "CIF/NIF: B80192727", "CIF/NIF: B80192727" } },
            { "lblDireccion",       new[]{ "Dirección física: C/ Ave del paraíso, Nº 7.", "Physical address: C/ Ave del paraíso, Nº 7." } },
            { "lblSeguridad",       new[]{ "Seguridad:", "Security:" } },
            { "btnCambiarPass",     new[]{ "🔒 Cambiar contraseña", "🔒 Change password" } },
            { "lblCerrarSesion",    new[]{ "Cerrar sesión automáticamente en:", "Auto-logout in:" } },

            // Configuración - Aspecto
            { "lblTemaInterfaz",    new[]{ "Tema de la Interfaz", "Interface Theme" } },
            { "lblModoColor",       new[]{ "Modo de color:", "Color mode:" } },
            { "rbClaro",            new[]{ "Modo Claro", "Light Mode" } },
            { "rbOscuro",           new[]{ "Modo Oscuro", "Dark Mode" } },
            { "rbSistema",          new[]{ "Sincronizar con el Sistema", "Sync with System" } },

            // Configuración - Accesibilidad
            { "lblAjustesVisuales", new[]{ "Ajustes Visuales", "Visual Settings" } },
            { "lblTamTexto",        new[]{ "Tamaño de texto:", "Text size:" } },
            { "lblModoDaltonico",   new[]{ "Modo daltónico:", "Color-blind mode:" } },
            { "chkContraste",       new[]{ "Forzar modo de alto contraste", "Force high contrast mode" } },
            { "lblComprension",     new[]{ "Comprensión y Motricidad", "Comprehension and Motor" } },
            { "chkAnimaciones",     new[]{ "Reducir animaciones y transiciones", "Reduce animations and transitions" } },
            { "lblAtajos",          new[]{ "Atajos de teclado:", "Keyboard shortcuts:" } },
            { "btnAtajos",          new[]{ "⌨ Configurar atajos", "⌨ Configure shortcuts" } },

            // Configuración - Preferencias
            { "lblZonaHoraria",     new[]{ "Zona Horaria y Calendario", "Time Zone and Calendar" } },
            { "lblHusoHorario",     new[]{ "Huso horario:", "Time zone:" } },
            { "lblFormatoCalendario",new[]{ "Formato de calendario:", "Calendar format:" } },
            { "lblFormatoHora",     new[]{ "Formato de hora:", "Time format:" } },
            { "lblMonedaUnidades",  new[]{ "Moneda y Unidades", "Currency and Units" } },
            { "lblMonedaSistema",   new[]{ "Moneda del sistema:", "System currency:" } },
            { "lblSistemaMedidas",  new[]{ "Sistema de medidas:", "Measurement system:" } },
            { "lblSeparador",       new[]{ "Separador decimal:", "Decimal separator:" } },

            // Configuración - Notificaciones
            { "lblEventosSistema",  new[]{ "Eventos del Sistema", "System Events" } },
            { "chkNReservas",       new[]{ "Nuevas reservas online", "New online bookings" } },
            { "chkNCancel",         new[]{ "Cancelación de clientes", "Customer cancellations" } },
            { "chkNMant",           new[]{ "Alertas urgentes de mantenimiento", "Urgent maintenance alerts" } },
            { "chkNLimp",           new[]{ "Cambio de estado de limpieza", "Cleaning status changes" } },

            // Estados y valores
            { "statReservado",      new[]{ "Reservado", "Reserved" } },
            { "statOcupado",        new[]{ "Ocupado", "Occupied" } },
            { "statLibre",          new[]{ "Libre", "Free" } },
            { "statActivo",         new[]{ "Activo", "Active" } },
            { "statInactivo",       new[]{ "Inactivo", "Inactive" } },

            // Total de registros
            { "lblTotal",           new[]{ "Total registro(s):", "Total record(s):" } },

            // Mapa y leyenda
            { "lblFree",            new[]{ "Libre", "Free" } },
            { "lblOccupied",        new[]{ "Ocupado", "Occupied" } },
            { "lblReserved",        new[]{ "Reservado", "Reserved" } },

            // Textos declarados directamente en XAML o generados por controles.
            { "rawLogin",            new[]{ "LOGIN", "LOGIN" } },
            { "rawPassword",         new[]{ "PASSWORD", "PASSWORD" } },
            { "rawForgot",           new[]{ "¿Contraseña Olvidada?", "Forgot password?" } },
            { "rawLoading",          new[]{ "Cargando...", "Loading..." } },
            { "rawAll",              new[]{ "Todos", "All" } },
            { "rawNew",              new[]{ "Nuevo", "New" } },
            { "rawClear",            new[]{ "Limpiar", "Clear" } },
            { "rawSave",             new[]{ "Guardar", "Save" } },
            { "rawCancel",           new[]{ "Cancelar", "Cancel" } },
            { "rawApply",            new[]{ "Aplicar", "Apply" } },
            { "rawAccept",           new[]{ "Aceptar", "Accept" } },
            { "rawUser",             new[]{ "USUARIO", "USERNAME" } },
            { "rawError",            new[]{ "MENSAJE DE ERROR", "ERROR MESSAGE" } },
            { "rawRequiredUser",     new[]{ "El campo de usuario no puede estar vacío", "Username cannot be empty" } },
            { "rawRequiredPassword", new[]{ "La contraseña no puede estar vacía", "Password cannot be empty" } },
            { "rawInvalidLogin",     new[]{ "Usuario o contraseña incorrectos", "Incorrect username or password" } },
            { "rawSearch",           new[]{ "Buscar...", "Search..." } },
            { "rawActive",           new[]{ "Activo", "Active" } },
            { "rawInactive",         new[]{ "Inactivo", "Inactive" } },
            { "rawAvailable",        new[]{ "Disponible", "Available" } },
            { "rawOccupied",         new[]{ "Ocupado", "Occupied" } },
            { "rawReserved",         new[]{ "Reservado", "Reserved" } },
            { "rawClean",            new[]{ "Limpio", "Clean" } },
            { "rawDirty",            new[]{ "Sucio", "Dirty" } },
            { "rawNotAssigned",      new[]{ "Sin asignar", "Unassigned" } },
            { "rawMaintenance",      new[]{ "En Mantenimiento", "Under maintenance" } },
            { "rawCleaning",         new[]{ "Esp. Limpieza", "Awaiting cleaning" } },
            { "rawInCleaning",       new[]{ "En limpieza", "Being cleaned" } },
            { "rawAdults",           new[]{ "Adultos", "Adults" } },
            { "rawChildren",         new[]{ "Niños", "Children" } },
            { "rawBabies",           new[]{ "Bebés", "Babies" } },
            { "rawConfirm",          new[]{ "Confirmar", "Confirm" } },
            { "rawInvalidData",      new[]{ "Datos no válidos", "Invalid data" } },
            { "rawBungalo",          new[]{ "Bungalo APP", "Bungalo APP" } },
            { "totalAlojamientos",   new[]{ "Alojamientos totales: {0} | disponibles: {1}", "Total accommodations: {0} | available: {1}" } },
            { "estado",              new[]{ "ESTADO", "STATUS" } },
            { "limpio",              new[]{ "LIMPIO", "CLEAN" } },
            { "totActividades",      new[]{ "Actividades totales: {0}", "Total activities: {0}" } },
            { "totReservas",         new[]{ "Reservas totales: {0}", "Total bookings: {0}" } },
            { "totPersonal",         new[]{ "Personal total: {0}", "Total staff: {0}" } },
            { "ocupantes",           new[]{ "{0} Adultos · {1} Niños · {2} Bebés  ▾", "{0} Adults · {1} Children · {2} Babies  ▾" } },
            { "inscritos",            new[]{ "{0}  (Adultos: {1} · Niños: {2} · Bebés: {3})", "{0}  (Adults: {1} · Children: {2} · Babies: {3})" } },
            { "metricEntries",       new[]{ "NÚMERO DE ENTRADAS", "NUMBER OF CHECK-INS" } },
            { "metricFree",          new[]{ "PARCELAS LIBRES", "FREE UNITS" } },
            { "metricOccupancy",     new[]{ "OCUPACIÓN (%)", "OCCUPANCY (%)" } },
            { "metricStaff",         new[]{ "PERSONAL ACTIVO", "ACTIVE STAFF" } },
            { "chartLines",          new[]{ "Líneas", "Lines" } },
            { "chartBars",           new[]{ "Barras", "Bars" } },
            { "chartCircle",         new[]{ "Círculo", "Pie" } },
            { "chartTitle",          new[]{ "GRÁFICAS", "CHARTS" } },
            { "rawConfigTitle",      new[]{ "CONFIGURACIÓN", "SETTINGS" } },
            { "rawAppearance",       new[]{ "Aspecto", "Appearance" } },
            { "rawAccessibility",    new[]{ "Accesibilidad", "Accessibility" } },
            { "rawGeneral",          new[]{ "General", "General" } },
            { "rawPreferences",      new[]{ "Preferencias", "Preferences" } },
            { "rawNotifications",    new[]{ "Notificaciones", "Notifications" } },
            { "rawAccommodationList",new[]{ "LISTADO DE ALOJAMIENTOS", "ACCOMMODATION LIST" } },
            { "rawAccommodationDetails",new[]{ "DETALLES DEL ALOJAMIENTO", "ACCOMMODATION DETAILS" } },
            { "rawBookingManagement",new[]{ "GESTIÓN DE RESERVAS", "BOOKING MANAGEMENT" } },
            { "rawBookingDetails",   new[]{ "NUEVA / DETALLES DE RESERVA", "NEW / BOOKING DETAILS" } },
            { "rawActivityManagement",new[]{ "GESTIÓN DE ACTIVIDADES", "ACTIVITIES MANAGEMENT" } },
            { "rawActivityDetails",  new[]{ "DETALLES DE LA ACTIVIDAD", "ACTIVITY DETAILS" } },
            { "rawStaffManagement",  new[]{ "GESTIÓN DE PERSONAL", "STAFF MANAGEMENT" } },
            { "rawStaffDetails",     new[]{ "DETALLES DE EMPLEADO", "EMPLOYEE DETAILS" } },
            { "rawClient",            new[]{ "👤 CLIENTE", "👤 CLIENT" } },
            { "rawAccommodation",     new[]{ "🏠 ALOJAMIENTO", "🏠 ACCOMMODATION" } },
            { "rawFilterType",        new[]{ "Filtrar por tipo:", "Filter by type:" } },
            { "rawFilterDay",         new[]{ "Filtrar por día", "Filter by day" } },
            { "rawFilterBookingStatus",new[]{ "Filtrar por estado de reserva", "Filter by booking status" } },
            { "rawFilterStaffStatus", new[]{ "Filtrar por estado del empleado", "Filter by employee status" } },
            { "rawClearFilters",      new[]{ "Limpiar filtros", "Clear filters" } },
            { "rawAddClient",         new[]{ "AÑADIR NUEVO CLIENTE", "ADD NEW CLIENT" } },
            { "rawNewActivity",       new[]{ "Nueva Actividad  ＋", "New Activity  ＋" } },
            { "rawNewStaff",          new[]{ "Nuevo empleado  ＋", "New employee  ＋" } },
            { "rawChangeStatus",      new[]{ "CAMBIAR ESTADO", "CHANGE STATUS" } },
            { "rawRequestCleaning",   new[]{ "SOLICITAR LIMPIEZA", "REQUEST CLEANING" } },
            { "rawTechnicalData",     new[]{ "Datos técnicos", "Technical data" } },
            { "rawRecipients",        new[]{ "DESTINATARIOS:", "RECIPIENTS:" } },
            { "rawStart",             new[]{ "INICIO (HH:mm)", "START (HH:mm)" } },
            { "rawEnd",               new[]{ "FIN (HH:mm)", "END (HH:mm)" } },
            { "rawContact",           new[]{ "CONTACTO", "CONTACT" } },
            { "rawLanguages",         new[]{ "IDIOMAS", "LANGUAGES" } },
            { "rawSpecialty",         new[]{ "ESPECIALIDAD", "SPECIALTY" } },
            { "rawEducation",         new[]{ "FORMACIÓN Y TITULACIÓN", "EDUCATION AND QUALIFICATIONS" } },
            { "rawUploadPhoto",       new[]{ "Subir/cambiar foto", "Upload/change photo" } },
            { "rawCurrentUser",       new[]{ "Usuario actual:", "Current user:" } },
            { "rawEmailLabel",        new[]{ "E-mail:", "E-mail:" } },
            { "rawNameLabel",         new[]{ "Nombre:", "Name:" } },
            { "rawPhoneLabel",        new[]{ "Teléfono:", "Phone:" } },
            { "rawEmailField",        new[]{ "E-mail", "E-mail" } },
            { "rawIdLabel",           new[]{ "DNI:", "ID:" } },
            { "rawAccommodationLabel",new[]{ "Alojamiento:", "Accommodation:" } },
            { "rawEquipment",         new[]{ "Equipamiento:", "Equipment:" } },
            { "rawVehicle",           new[]{ "Vehículo", "Vehicle" } },
            { "rawPet",               new[]{ "Mascota", "Pet" } },
            { "rawRegistration",      new[]{ "Matrícula:", "Registration:" } },
            { "rawCheckIn",           new[]{ "Fecha entrada:", "Check-in date:" } },
            { "rawCheckOut",          new[]{ "Fecha salida:", "Check-out date:" } },
            { "rawPrice",             new[]{ "Precio total (€):", "Total price (€):" } },
            { "rawRequests",          new[]{ "Solicitudes especiales del cliente:", "Customer special requests:" } },

            // ---- Mensajes de diálogos ----
            { "msgInactividad",   new[]{ "Sesión cerrada por inactividad.", "Session closed due to inactivity." } },
            { "msgCambiarPass",   new[]{ "Pendiente de implementar: aquí irá el diálogo para cambiar la contraseña.", "Not implemented yet: the change-password dialog will go here." } },
            { "msgAtajosTitulo",  new[]{ "Atajos de teclado", "Keyboard shortcuts" } },
            { "msgAtajos",        new[]{ "Atajos disponibles:\n\nCtrl+1  Panel de Control\nCtrl+2  Reservas\nCtrl+3  Alojamientos\nCtrl+4  Actividades\nCtrl+5  Personal", "Available shortcuts:\n\nCtrl+1  Dashboard\nCtrl+2  Bookings\nCtrl+3  Accommodation\nCtrl+4  Activities\nCtrl+5  Staff" } },
            { "msgSilencio",      new[]{ "El modo silencio está activado.", "Silent mode is on." } },
            { "msgNoAbrir",       new[]{ "No se pudo abrir: ", "Could not open: " } },
            { "msgSoporte",       new[]{ "Soporte técnico: soporte@bungalo.example (dirección de ejemplo)", "Technical support: soporte@bungalo.example (example address)" } },
            { "msgTerminos",      new[]{ "Términos del servicio (pendiente de redactar).", "Terms of service (to be written)." } },
            { "msgPrivacidad",    new[]{ "Política de privacidad (pendiente de redactar).", "Privacy policy (to be written)." } },

            // ---- Validaciones: alojamientos ----
            { "vNombre",          new[]{ "El nombre es obligatorio.", "Name is required." } },
            { "vAlojDup",         new[]{ "Ya existe un alojamiento con ese nombre.", "An accommodation with that name already exists." } },
            { "vAlojTipo",        new[]{ "Selecciona un tipo.", "Select a type." } },
            { "vAlojCap",         new[]{ "La capacidad máxima debe ser un número mayor que 0.", "Maximum capacity must be a number greater than 0." } },
            { "vAlojPers",        new[]{ "Las personas alojadas deben estar entre 0 y la capacidad máxima.", "People lodged must be between 0 and the maximum capacity." } },
            { "vPrecio",          new[]{ "El precio no es válido.", "The price is not valid." } },
            { "vAlojTam",         new[]{ "El tamaño debe ser un número mayor que 0.", "Size must be a number greater than 0." } },
            { "vAlojTemp",        new[]{ "Selecciona la temporada.", "Select the season." } },
            { "alojGuardado",     new[]{ "Alojamiento guardado correctamente.", "Accommodation saved successfully." } },
            { "alojNoElim",       new[]{ "No se puede eliminar: tiene reservas activas asociadas.", "Cannot delete: it has active bookings associated." } },
            { "alojElim",         new[]{ "¿Eliminar el alojamiento {0}?", "Delete accommodation {0}?" } },
            { "alojLimpiezaMsg",  new[]{ "Limpieza solicitada. Pulsa GUARDAR para confirmar el cambio.", "Cleaning requested. Press SAVE to confirm the change." } },

            // ---- Validaciones: actividades ----
            { "vActNombre",       new[]{ "El nombre de la actividad es obligatorio.", "The activity name is required." } },
            { "vActDia",          new[]{ "Selecciona el día.", "Select the day." } },
            { "vHoras",           new[]{ "Las horas deben tener formato HH:mm.", "Times must use the HH:mm format." } },
            { "vActFin",          new[]{ "La hora de fin debe ser posterior a la de inicio.", "The end time must be later than the start time." } },
            { "vActDest",         new[]{ "Selecciona al menos un destinatario.", "Select at least one recipient group." } },
            { "vActCupo",         new[]{ "El cupo debe cumplir 0 ≤ mínimo ≤ máximo.", "Quota must satisfy 0 ≤ minimum ≤ maximum." } },
            { "vPrecioHora",      new[]{ "El precio/hora no es válido.", "The price/hour is not valid." } },
            { "vPrecioMes",       new[]{ "El precio/mes no es válido.", "The price/month is not valid." } },
            { "vActUbi",          new[]{ "Indica la ubicación.", "Enter the location." } },
            { "actGuardada",      new[]{ "Actividad guardada correctamente.", "Activity saved successfully." } },
            { "actElim",          new[]{ "¿Eliminar la actividad {0}?", "Delete activity {0}?" } },

            // ---- Validaciones: personal ----
            { "vPersNombre",      new[]{ "Nombre y apellidos son obligatorios.", "First name and surname are required." } },
            { "vTel",             new[]{ "El teléfono debe tener 9 dígitos.", "The phone number must have 9 digits." } },
            { "vEmail",           new[]{ "El e-mail no es válido.", "The e-mail is not valid." } },
            { "vIdiomas",         new[]{ "Indica al menos un idioma.", "Enter at least one language." } },
            { "persGuardado",     new[]{ "Empleado guardado correctamente.", "Employee saved successfully." } },
            { "persBaja",         new[]{ "¿Dar de baja a {0}?", "Deactivate {0}?" } },
            { "persAlta",         new[]{ "¿Dar de alta a {0}?", "Reactivate {0}?" } },
            { "btnAlta",          new[]{ "DAR DE ALTA", "REACTIVATE" } },
            { "fotoFiltro",       new[]{ "Imágenes|*.jpg;*.jpeg;*.png;*.bmp", "Images|*.jpg;*.jpeg;*.png;*.bmp" } },
            { "fotoTitulo",       new[]{ "Selecciona una foto", "Select a photo" } },

            // ---- Validaciones: reservas ----
            { "vDni",             new[]{ "El DNI debe tener 8 números y una letra.", "The ID (DNI) must have 8 digits and a letter." } },
            { "vMatricula",       new[]{ "Indica la matrícula del vehículo.", "Enter the vehicle's license plate." } },
            { "vAloj",            new[]{ "Selecciona un alojamiento.", "Select an accommodation." } },
            { "vFechas",          new[]{ "Indica las fechas de entrada y salida.", "Enter the check-in and check-out dates." } },
            { "vFechaOrden",      new[]{ "La fecha de salida no puede ser anterior a la de entrada.", "The check-out date cannot be earlier than the check-in date." } },
            { "resGuardada",      new[]{ "Reserva guardada correctamente.", "Booking saved successfully." } },
            { "resElim",          new[]{ "¿Eliminar la reserva de {0} del listado?", "Delete {0}'s booking from the list?" } },
            { "recordatorio",     new[]{ "Recordatorio: la reserva de {0} se ELIMINARÁ en [2 DÍAS]", "Reminder: {0}'s booking will be DELETED in [2 DAYS]" } },
        };

        public static string T(string clave)
        {
            string[] v;
            if (!d.TryGetValue(clave, out v)) return clave;
            return v[AppSettings.Idioma == "EN" ? 1 : 0];
        }

        public static string F(string clave, params object[] argumentos)
        {
            return string.Format(T(clave), argumentos);
        }

        // Textos escritos directamente en los XAML y valores de datos (pares ES, EN, ES, EN...).
        private static readonly string[] lit =
        {
            "Entrar App", "Enter App",
            "Sin asignar", "Unassigned", "Sin equipamiento", "No equipment", "(Lleno)", "(Full)",
            "(Desaparece en 2 días)", "(Disappears in 2 days)",
            "Teléfono", "Phone",
            "ID", "ID", "CLIENTE", "CLIENT", "ALOJ.", "ACCOMM.", "DÍA-HORA", "DAY-TIME",
            "GUARDAR", "SAVE", "ELIMINAR", "DELETE", "CANCELAR", "CANCEL",
            "EQUIPAMIENTO", "EQUIPMENT", "Wi-Fi", "Wi-Fi", "A/C", "A/C", "Admite mascotas", "Pets allowed",

            // Estados, tipos y temporadas
            "Confirmada", "Confirmed", "Pendiente", "Pending", "Eliminada", "Deleted", "Cancelada", "Cancelled",
            "Bungalow", "Bungalow", "Cabaña", "Cabin", "Parcela", "Plot",
            "Alta", "High", "Media", "Medium", "Baja", "Low",

            // Actividades y ubicaciones
            "Senderismo", "Hiking", "Escalada", "Climbing", "Yoga", "Yoga", "Cine Infantil", "Kids' Cinema", "Ruta BTT", "MTB Route",
            "Zona Norte - Lago", "North Area - Lake", "Zona Sur", "South Area", "Piscina", "Pool",
            "Pistas deportivas", "Sports courts", "Sala Multiusos", "Multipurpose Room", "Recepción", "Reception",

            // Especialidades del personal
            "Senderismo / BTT", "Hiking / MTB", "Senderismo / Escalada", "Hiking / Climbing", "Yoga / Meditación", "Yoga / Meditation",
            "Cine Infantil / Juegos", "Kids' Cinema / Games", "Ruta BTT / Aventura", "MTB Route / Adventure",

            // Panel de control
            "Ver gráficas detalladas de ocupación", "View detailed occupancy charts",
            "Haz clic para ver detalles de ocupación", "Click to view occupancy details",
            "Haz doble clic en una reserva para ir a detalles", "Double-click a booking to see its details",
            "Haz doble clic en una actividad para ir a detalles", "Double-click an activity to see its details",

            // Reservas
            "Guardar cambios de la reserva", "Save booking changes", "Eliminar esta reserva", "Delete this booking",
            "Descartar cambios", "Discard changes", "Nombre completo del cliente", "Client's full name",
            "Documento nacional de identidad", "National ID document", "Número de teléfono", "Phone number",
            "Correo electrónico del cliente", "Client's e-mail address",
            "Indicar si el cliente trae vehículo", "Indicate whether the client brings a vehicle",
            "Notas (INTERNAS) del alojamiento:", "Accommodation notes (INTERNAL):", "IR AL ALOJAMIENTO", "GO TO ACCOMMODATION",
            "DETALLES DE LA RESERVA", "BOOKING DETAILS", "Ocupantes:", "Occupants:",
            "Adultos:", "Adults:", "Niños:", "Children:", "Bebés:", "Babies:",
            "Hora entrada (HH:mm):", "Check-in time (HH:mm):", "Hora salida (HH:mm):", "Check-out time (HH:mm):",

            // Alojamientos
            "Guardar cambios del alojamiento", "Save accommodation changes", "Eliminar este alojamiento", "Delete this accommodation",
            "Nombre único del alojamiento", "Unique accommodation name", "Categoría del alojamiento", "Accommodation category",
            "Cambiar estado del alojamiento", "Change accommodation status", "Solicitar limpieza del alojamiento", "Request accommodation cleaning",
            "Número máximo de personas que puede alojar", "Maximum number of people it can host",
            "Número actual de personas alojadas", "Current number of people lodged",

            // Actividades
            "Limpiar todos los filtros", "Clear all filters", "Guardar cambios de la actividad", "Save activity changes",
            "Eliminar esta actividad", "Delete this activity", "UBICACIÓN:", "LOCATION:",
            "NÚMERO DE INSCRITOS:", "NUMBER OF ENROLLED:", "Detalles avanzados", "Advanced details",
            "PRECIO/HORA (€)", "PRICE/HOUR (€)", "PRECIO/MES (€)", "PRICE/MONTH (€)",
            "CONSIDERACIONES:", "CONSIDERATIONS:", "🎒 MATERIAL NECESARIO", "🎒 REQUIRED EQUIPMENT",
            "📝 DESCRIPCIÓN DE LA ACTIVIDAD", "📝 ACTIVITY DESCRIPTION",

            // Personal
            "Guardar cambios del empleado", "Save employee changes", "Dar de baja al empleado", "Deactivate employee",

            // Login
            "Introduce tu nombre de usuario para acceder", "Enter your username to log in",
            "Introduce tu contraseña de acceso", "Enter your password",

            // Configuración
            "Check-ins pendientes", "Pending check-ins", "Actividades sin monitor", "Activities without instructor",
            "Canales y Sonidos", "Channels and Sounds", "Notificaciones emergentes", "Pop-up notifications",
            "Activar sonido (para notificaciones importantes)", "Enable sound (for important notifications)",
            "Sonido:", "Sound:", "🔊 Probar notificaciones", "🔊 Test notifications",
            "Modo Silencio", "Silent Mode", "Activar modo silencio", "Enable silent mode",
            "Sobre Nosotros", "About Us", "Identidad del Software", "Software Identity",
            "Nombre del programa: Bungalo APP", "Program name: Bungalo APP",
            "Versión y licencia: V0.2.0-ALPHA, Licencia corporativa", "Version and license: V0.2.0-ALPHA, Corporate license",
            "Créditos: Raúl Martínez, Silviu-Daniel Zamfir", "Credits: Raúl Martínez, Silviu-Daniel Zamfir",
            "Contexto: Proyecto de la asignatura Interacción Persona-Ordenador (IPO). Escuela Superior de Informática - UCLM",
            "Context: Course project for Human-Computer Interaction (IPO). School of Computer Science (ESI) - UCLM",
            "Ayuda y Documentación", "Help and Documentation",
            "📖 Descargar documentación del proyecto", "📖 Download project documentation",
            "📞 Contactar con soporte técnico", "📞 Contact technical support",
            "Aspectos Legales", "Legal Notices",
            "Términos del servicio de software", "Software terms of service", "Política de privacidad", "Privacy policy",
            "El sistema está actualizado a la última versión: V0.2.0-ALPHA", "The system is up to date: V0.2.0-ALPHA",

            // Valores de los desplegables de Configuración (internamente se guardan en español)
            "Desactivado", "Off",
            "UTC+0 (Londres)", "UTC+0 (London)", "UTC+1 (Europa/Madrid)", "UTC+1 (Europe/Madrid)", "UTC+2 (Atenas)", "UTC+2 (Athens)",
            "dd/mm/aaaa", "dd/mm/yyyy", "mm/dd/aaaa", "mm/dd/yyyy", "aaaa-mm-dd", "yyyy-mm-dd",
            "24 horas", "24 hours", "Dólar ($)", "Dollar ($)", "Libra (£)", "Pound (£)",
            "Metros/m²", "Meters/m²", "Pies/ft²", "Feet/ft²",
            "1.000,00 (Europa)", "1.000,00 (Europe)", "1,000.00 (EE. UU.)", "1,000.00 (USA)",
            "ALARMA", "ALARM", "CAMPANA", "BELL"
        };

        private static Dictionary<string, string> toEn, toEs;

        private static void Build()
        {
            toEn = new Dictionary<string, string>();
            toEs = new Dictionary<string, string>();
            foreach (string[] v in d.Values) Add(v[0], v[1]);
            for (int i = 0; i + 1 < lit.Length; i += 2) Add(lit[i], lit[i + 1]);
        }

        private static void Add(string es, string en)
        {
            if (!toEn.ContainsKey(es)) toEn[es] = en;
            if (!toEs.ContainsKey(en)) toEs[en] = es;
        }

        public static bool EsIngles { get { return AppSettings.Idioma == "EN"; } }

        /// <summary>Traduce un texto al idioma actual (ES -> EN o EN -> ES). Si no lo conoce, lo devuelve igual.</summary>
        public static string Translate(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            if (toEn == null) Build();
            string r;
            if (EsIngles)
            {
                if (toEs.ContainsKey(texto) && !toEn.ContainsKey(texto)) return texto; // ya está en inglés
                return toEn.TryGetValue(texto, out r) ? r : texto;
            }
            if (toEn.ContainsKey(texto)) return texto; // ya está en español
            return toEs.TryGetValue(texto, out r) ? r : texto;
        }

        /// <summary>Devuelve el valor interno (español) de un texto mostrado en cualquier idioma.</summary>
        public static string ToEs(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            if (toEn == null) Build();
            string r;
            if (!toEn.ContainsKey(texto) && toEs.TryGetValue(texto, out r)) return r;
            return texto;
        }

        /// <summary>Estado de empleado: "Baja" se muestra como "Inactive" (y no como "Low", que es la temporada).</summary>
        public static string TranslateEmp(string estado)
        {
            if (estado == "Baja") return EsIngles ? "Inactive" : "Baja";
            return Translate(estado);
        }

        // ---------- ComboBox: los valores internos siguen en español, solo cambia lo que se ve ----------

        private static DataTemplate plantilla, plantillaEmp;

        /// <summary>Hace que el ComboBox muestre sus textos traducidos (se actualizan solos al cambiar de idioma).</summary>
        public static void Combo(ComboBox c, bool empleado = false)
        {
            DataTemplate t = empleado ? plantillaEmp : plantilla;
            if (t == null)
            {
                var f = new FrameworkElementFactory(typeof(TextBlock));
                var mb = new MultiBinding { Converter = LocMultiConverter.Instance, ConverterParameter = empleado ? "emp" : null };
                mb.Bindings.Add(new Binding("."));
                mb.Bindings.Add(new Binding("Version") { Source = LocNotifier.Instance, Mode = BindingMode.OneWay });
                f.SetBinding(TextBlock.TextProperty, mb);
                t = new DataTemplate { VisualTree = f };
                if (empleado) plantillaEmp = t; else plantilla = t;
            }
            c.ItemTemplate = t;
        }

        // ---------- Recorrido del árbol: traduce TODO, también las pestañas que aún no se han mostrado ----------

        public static void ApplyVisualTree(DependencyObject root)
        {
            Recorrer(root, new HashSet<DependencyObject>());
        }

        private static void Recorrer(DependencyObject o, HashSet<DependencyObject> visitados)
        {
            if (o == null || !visitados.Add(o)) return;
            Aplicar(o);

            // Se copian los hijos ANTES de recorrerlos: al traducir un elemento WPF puede
            // modificar colecciones (pestañas, inlines...) y eso rompería el enumerador.
            var hijos = new List<DependencyObject>();
            foreach (object hijo in LogicalTreeHelper.GetChildren(o))
            {
                DependencyObject dep = hijo as DependencyObject;
                if (dep != null) hijos.Add(dep);
            }
            foreach (DependencyObject dep in hijos) Recorrer(dep, visitados);
        }

        private static void Aplicar(DependencyObject o)
        {
            // Los textos enlazados a datos NO se tocan (si no, se perdería el binding)
            TextBlock tb = o as TextBlock;
            if (tb != null && !BindingOperations.IsDataBound(tb, TextBlock.TextProperty)
                && (tb.Inlines.Count == 0 || (tb.Inlines.Count == 1 && tb.Inlines.FirstInline is Run)))
            {
                string n = Translate(tb.Text);
                if (n != tb.Text) tb.Text = n;
            }

            Run run = o as Run;
            if (run != null && !BindingOperations.IsDataBound(run, Run.TextProperty))
            {
                string n = Translate(run.Text);
                if (n != run.Text) run.Text = n;
            }

            ContentControl cc = o as ContentControl;
            if (cc != null && cc.Content is string && !BindingOperations.IsDataBound(cc, ContentControl.ContentProperty))
            {
                string n = Translate((string)cc.Content);
                if (n != (string)cc.Content) cc.Content = n;
            }

            Window w = o as Window;
            if (w != null)
            {
                string n = Translate(w.Title);
                if (n != w.Title) w.Title = n;
            }

            HeaderedContentControl hc = o as HeaderedContentControl;
            if (hc != null && hc.Header is string && !BindingOperations.IsDataBound(hc, HeaderedContentControl.HeaderProperty))
            {
                string n = Translate((string)hc.Header);
                if (n != (string)hc.Header) hc.Header = n;
            }

            FrameworkElement fe = o as FrameworkElement;
            if (fe != null && fe.ToolTip is string)
            {
                string n = Translate((string)fe.ToolTip);
                if (n != (string)fe.ToolTip) fe.ToolTip = n;
            }

            DataGrid grid = o as DataGrid;
            if (grid != null)
                foreach (DataGridColumn column in new List<DataGridColumn>(grid.Columns))
                    if (column.Header is string)
                    {
                        string n = Translate((string)column.Header);
                        if (n != (string)column.Header) column.Header = n;
                    }
        }
    }

    /// <summary>Avisa a los bindings de que el idioma ha cambiado.</summary>
    public sealed class LocNotifier : INotifyPropertyChanged
    {
        public static readonly LocNotifier Instance = new LocNotifier();
        private int version;
        public event PropertyChangedEventHandler PropertyChanged;

        private LocNotifier()
        {
            AppSettings.IdiomaChanged += (s, e) =>
            {
                version++;
                PropertyChangedEventHandler h = PropertyChanged;
                if (h != null) h(this, new PropertyChangedEventArgs("Version"));
            };
        }

        public int Version { get { return version; } }
    }

    /// <summary>Conversor para plantillas con MultiBinding (valor, versión de idioma).</summary>
    public sealed class LocMultiConverter : IMultiValueConverter
    {
        public static readonly LocMultiConverter Instance = new LocMultiConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null || values[0] == DependencyProperty.UnsetValue) return "";
            string s = values[0] as string ?? values[0].ToString();
            return (parameter as string) == "emp" ? Loc.TranslateEmp(s) : Loc.Translate(s);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>Conversor simple para usar en XAML: Converter="{x:Static local:LocConverter.Instance}".</summary>
    public sealed class LocConverter : IValueConverter
    {
        public static readonly LocConverter Instance = new LocConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            string s = value as string ?? value.ToString();
            return (parameter as string) == "emp" ? Loc.TranslateEmp(s) : Loc.Translate(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
