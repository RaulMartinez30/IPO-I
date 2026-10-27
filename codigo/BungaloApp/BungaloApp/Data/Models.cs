using System;

namespace BungaloApp
{
    public class Reserva
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Vehiculo { get; set; }
        public string Matricula { get; set; }
        public bool Mascota { get; set; }
        public string Alojamiento { get; set; }
        public int Adultos { get; set; }
        public int Ninos { get; set; }
        public int Bebes { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }          // Confirmada, Pendiente, Eliminada, Cancelada
        public string Solicitudes { get; set; }

        public string IdCliente { get { return Id + " - " + Cliente; } }
        public string FechaEntradaTxt { get { return FechaEntrada.ToString("dd/MM/yy"); } }
        public string Aviso { get { return Estado == "Eliminada" ? "(Desaparece en 2 días)" : ""; } }
    }

    public class Alojamiento
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }            // Bungalow, Cabaña, Parcela
        public string Estado { get; set; }          // Disponible, Ocupado, En Mantenimiento, Esp. Limpieza
        public bool Limpio { get; set; }
        public int CapacidadMax { get; set; }
        public int PersonasAlojadas { get; set; }
        public decimal PrecioNoche { get; set; }
        public int Tamano { get; set; }
        public string Temporada { get; set; }       // Alta, Media, Baja
        public string Ubicacion { get; set; }
        public string Descripcion { get; set; }
        public string Notas { get; set; }
        public bool Wifi { get; set; }
        public bool Aire { get; set; }
        public bool Lavadora { get; set; }
        public bool Tv { get; set; }
        public bool Mascotas { get; set; }

        public string EquipamientoTxt
        {
            get
            {
                var l = new System.Collections.Generic.List<string>();
                if (Wifi) l.Add("Wi-Fi");
                if (Aire) l.Add("A/C");
                if (Lavadora) l.Add("Lavadora");
                if (Tv) l.Add("TV");
                if (Mascotas) l.Add("Admite mascotas");
                return l.Count == 0 ? "Sin equipamiento" : string.Join(" · ", l);
            }
        }
    }

    public class Actividad
    {
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }            // "10:00"
        public string HoraFin { get; set; }
        public string Nombre { get; set; }
        public string Monitor { get; set; }
        public int Inscritos { get; set; }
        public int InscAdultos { get; set; }
        public int InscNinos { get; set; }
        public int InscBebes { get; set; }
        public int CupoMin { get; set; }
        public int CupoMax { get; set; }
        public string Ubicacion { get; set; }
        public bool DestAdultos { get; set; }
        public bool DestNinos { get; set; }
        public bool DestBebes { get; set; }
        public decimal PrecioHora { get; set; }
        public decimal PrecioMes { get; set; }
        public string Consideraciones { get; set; }
        public string Material { get; set; }
        public string Descripcion { get; set; }

        public bool SinMonitor { get { return string.IsNullOrWhiteSpace(Monitor) || Monitor == "Sin asignar"; } }
        public string FechaTxt { get { return Fecha.ToString("dd/MM/yy"); } }
        public string DiaHora { get { return Fecha.ToString("dd/MM/yyyy") + " " + Hora; } }
        public string MonitorTxt { get { return SinMonitor ? "(Sin asignar)" : "(" + Monitor + ")"; } }
        public string Plazas { get { return Inscritos + "/" + CupoMax + (Inscritos >= CupoMax ? " (Lleno)" : ""); } }

        public string EstadoIcono
        {
            get
            {
                if (SinMonitor || Inscritos < CupoMin) return "!";
                if (Inscritos >= CupoMax) return "✖";
                return "✔";
            }
        }
        public string EstadoColor
        {
            get
            {
                if (SinMonitor || Inscritos < CupoMin) return "#F59E0B";
                if (Inscritos >= CupoMax) return "#D32F2F";
                return "#2E9E4F";
            }
        }
    }

    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Idiomas { get; set; }         // "ES / EN"
        public string Especialidad { get; set; }
        public string Estado { get; set; }          // Activo / Baja
        public string Formacion { get; set; }
        public string FotoPath { get; set; }        // ruta de archivo opcional

        public string NombreCompleto { get { return Nombre + " " + Apellidos; } }
        public string Contacto { get { return Telefono + "\n" + Email; } }
        public string EstadoTxt { get { return "● " + Estado; } }
        public string EstadoColor { get { return Estado == "Activo" ? "#2E9E4F" : "#D32F2F"; } }
        public string Iniciales
        {
            get
            {
                string a = string.IsNullOrEmpty(Nombre) ? "" : Nombre.Substring(0, 1);
                string b = string.IsNullOrEmpty(Apellidos) ? "" : Apellidos.Substring(0, 1);
                return (a + b).ToUpper();
            }
        }
    }
}
