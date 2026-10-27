using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BungaloApp
{
    /// <summary>Datos de ejemplo en memoria (sustituir por BD/ficheros cuando haga falta).</summary>
    public static class DataStore
    {
        public static ObservableCollection<Reserva> Reservas = new ObservableCollection<Reserva>();
        public static ObservableCollection<Alojamiento> Alojamientos = new ObservableCollection<Alojamiento>();
        public static ObservableCollection<Actividad> Actividades = new ObservableCollection<Actividad>();
        public static ObservableCollection<Empleado> Empleados = new ObservableCollection<Empleado>();

        static DataStore()
        {
            DateTime hoy = DateTime.Today;

            // ---- Alojamientos ----
            Alojamientos.Add(A("B101", "Bungalow", "Disponible", true, 8, 0, 90, 27, "Bungalow caribeño y acogedor, perfecto para recreación y vacaciones."));
            Alojamientos.Add(A("P205", "Parcela", "En Mantenimiento", true, 6, 0, 50, 20, "Parcela establecida en la naturaleza, perfecta para la desconexión mental y relajación."));
            Alojamientos.Add(A("C201", "Cabaña", "Esp. Limpieza", false, 4, 0, 70, 22, "Cabaña de madera con porche."));
            Alojamientos.Add(A("B102", "Bungalow", "Disponible", true, 6, 0, 85, 25, "Bungalow con terraza."));
            Alojamientos.Add(A("C301", "Cabaña", "Ocupado", true, 4, 4, 70, 22, "Cabaña junto al río."));
            Alojamientos.Add(A("P206", "Parcela", "Disponible", true, 6, 0, 50, 20, "Parcela sombreada."));
            Alojamientos.Add(A("C302", "Cabaña", "Disponible", true, 4, 0, 70, 22, "Cabaña acogedora."));
            Alojamientos.Add(A("B103", "Bungalow", "Ocupado", false, 6, 2, 85, 25, "Bungalow familiar."));
            Alojamientos.Add(A("C303", "Cabaña", "Disponible", true, 4, 0, 70, 22, "Cabaña con chimenea."));
            Alojamientos.Add(A("P207", "Parcela", "En limpieza", false, 6, 0, 50, 20, "Parcela cerca de la zona de restauración."));
            Alojamientos[1].Ubicacion = "Parcela ubicada cerca de la restauración";

            // ---- Personal ----
            Empleados.Add(new Empleado { Id = 1001, Nombre = "Pedro", Apellidos = "Ruiz", Telefono = "654321098", Email = "pedro.ruiz@esi.com", Idiomas = "ES / EN", Especialidad = "Senderismo / BTT", Estado = "Activo", Formacion = "Grado en CCAFD, Técnico de Senderismo y Escalada, Cursos de Primeros Auxilios (DESA)." });
            Empleados.Add(new Empleado { Id = 1002, Nombre = "Laura", Apellidos = "García", Telefono = "612345678", Email = "laura.g@esi.com", Idiomas = "ES / EN / FR", Especialidad = "Senderismo / Escalada", Estado = "Activo", Formacion = "Técnico deportivo en media montaña." });
            Empleados.Add(new Empleado { Id = 1003, Nombre = "Carlos", Apellidos = "Ruiz", Telefono = "687654321", Email = "carlos.r@esi.com", Idiomas = "ES", Especialidad = "Yoga / Meditación", Estado = "Activo", Formacion = "Instructor de Yoga certificado." });
            Empleados.Add(new Empleado { Id = 1004, Nombre = "Ana", Apellidos = "Moral", Telefono = "699887766", Email = "ana.m@esi.com", Idiomas = "ES / EN", Especialidad = "Cine Infantil / Juegos", Estado = "Activo", Formacion = "Monitor de ocio y tiempo libre." });
            Empleados.Add(new Empleado { Id = 1005, Nombre = "David", Apellidos = "Martínez", Telefono = "611223344", Email = "david.m@esi.com", Idiomas = "ES / FR", Especialidad = "Ruta BTT / Aventura", Estado = "Baja", Formacion = "Guía de BTT." });

            // ---- Actividades ----
            Actividades.Add(Act(hoy, "10:00", "12:00", "Senderismo", "Laura", 15, 20, 3, "Zona Norte - Lago", "Mochila, Agua, Zapatillas"));
            Actividades.Add(Act(hoy, "12:30", "14:00", "Escalada", "Pedro", 20, 20, 3, "Pistas deportivas", "Arnés, Casco, Zapatillas de escalada"));
            Actividades.Add(Act(hoy, "16:30", "17:30", "Yoga", "Carlos", 8, 15, 3, "Sala Multiusos", "Esterilla, Agua"));
            Actividades.Add(Act(hoy, "18:00", "19:30", "Cine Infantil", "Sin asignar", 12, 30, 5, "Sala Multiusos", "Ninguno"));
            Actividades.Add(Act(hoy.AddDays(1), "09:00", "12:00", "Ruta BTT", "Carlos", 2, 10, 4, "Zona Sur", "Bici, Casco, Agua"));

            // ---- Reservas ----
            Reservas.Add(R(1001, "Pedro Ruiz", "11111111A", "654321098", "pedro.ruiz@mail.com", "B103", "Confirmada", hoy, 2, 2, 0, 0, 180));
            Reservas.Add(R(1002, "Laura García", "22222222B", "612345678", "laura.g@mail.com", "C201", "Eliminada", hoy.AddDays(-3), 2, 2, 0, 0, 140));
            Reservas.Add(R(1003, "Ana Moral", "33333333C", "699887766", "ana.m@mail.com", "C302", "Pendiente", hoy.AddDays(1), 3, 2, 1, 0, 210));
            Reservas.Add(R(1004, "Amra Ali", "44444444D", "677889900", "amra.a@mail.com", "B101", "Pendiente", hoy, 2, 3, 0, 0, 180));
            Reservas.Add(R(1005, "David Martínez", "55555555E", "611223344", "david.m@mail.com", "P206", "Confirmada", hoy.AddDays(-1), 4, 2, 0, 0, 200));
            Reservas.Add(R(1006, "Sara Sánchez", "06273997W", "643283910", "SaraSanchez67@outlook.es", "B102", "Confirmada", hoy.AddDays(2), 2, 2, 0, 0, 170));
            Reservas[5].Vehiculo = true; Reservas[5].Matricula = "2880 HDP"; Reservas[5].Solicitudes = "Necesitan cuna\nProblemas de movilidad";
            Reservas[5].Estado = "Confirmada";
        }

        static Alojamiento A(string n, string t, string estado, bool limpio, int cap, int pers, decimal precio, int m2, string desc)
        {
            return new Alojamiento
            {
                Nombre = n, Tipo = t, Estado = estado, Limpio = limpio, CapacidadMax = cap, PersonasAlojadas = pers,
                PrecioNoche = precio, Tamano = m2, Descripcion = desc, Temporada = "Baja", Ubicacion = "", Notas = "",
                Wifi = true, Aire = t != "Parcela", Lavadora = t == "Bungalow", Tv = t != "Parcela", Mascotas = t != "Cabaña"
            };
        }

        static Actividad Act(DateTime f, string h, string hf, string n, string m, int insc, int max, int min, string ubi, string material)
        {
            return new Actividad
            {
                Fecha = f, Hora = h, HoraFin = hf, Nombre = n, Monitor = m, Inscritos = insc, CupoMax = max, CupoMin = min,
                InscAdultos = insc, InscNinos = 0, InscBebes = 0,
                Ubicacion = ubi, DestAdultos = true, DestNinos = true, DestBebes = false, PrecioHora = 30, PrecioMes = 120,
                Consideraciones = "N/A", Material = material, Descripcion = "Descripción de la actividad " + n + "."
            };
        }

        static Reserva R(int id, string cli, string dni, string tel, string mail, string aloj, string estado,
                         DateTime entrada, int noches, int ad, int ni, int be, decimal precio)
        {
            return new Reserva
            {
                Id = id, Cliente = cli, Dni = dni, Telefono = tel, Email = mail, Alojamiento = aloj, Estado = estado,
                FechaEntrada = entrada, FechaSalida = entrada.AddDays(noches), HoraEntrada = "14:00", HoraSalida = "12:00",
                Adultos = ad, Ninos = ni, Bebes = be, Precio = precio, Matricula = "", Solicitudes = ""
            };
        }

        /// <summary>Avisos que se muestran al pulsar el botón AVISOS.</summary>
        public static List<string> ObtenerAvisos()
        {
            var l = new List<string>();
            int limp = Alojamientos.Count(a => a.Estado == "Esp. Limpieza");
            int chk = Reservas.Count(r => r.Estado == "Pendiente");
            int act = Actividades.Count(a => a.Fecha >= DateTime.Today && a.SinMonitor);
            if (limp > 0) l.Add("⚠ " + string.Format(Loc.T("avLimpieza"), limp));
            if (chk > 0) l.Add("⏰ " + string.Format(Loc.T("avCheckins"), chk));
            if (act > 0) l.Add("⚠ " + string.Format(Loc.T("avActividad"), act));
            return l;
        }
    }
}
