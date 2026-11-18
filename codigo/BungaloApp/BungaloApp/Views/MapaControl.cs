using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace BungaloApp
{
    /// <summary>
    /// Mapa interactivo (Leaflet dentro de WebView2) con la ubicación de bungalows, cabañas y parcelas.
    /// Se arrastra para moverse, rueda para zoom, y cada marcador abre una ficha con botón "Ver alojamiento".
    /// </summary>
    public sealed class MapaControl : Grid
    {
        private const string Host = "bungalo.local";

        // El entorno de WebView2 es lo lento de crear: se crea una vez y se reutiliza en cada visita al panel
        private static Task<CoreWebView2Environment> entorno;
        private static bool htmlEscrito;
        private static string ultimaVista;   // "lat,lng,zoom": se recuerda al salir y volver al panel

        /// <summary>Se lanza con el nombre del alojamiento cuando se pulsa "Ver alojamiento" en un marcador.</summary>
        public event Action<string> Abrir;

        // ---- Modo "elegir ubicación": el usuario hace clic en el mapa y se devuelven las coordenadas ----
        public bool ModoSeleccion { get; set; }
        public double? PuntoLat { get; set; }
        public double? PuntoLng { get; set; }
        public string Excluir { get; set; }      // alojamiento que se está editando (no se dibuja aparte)
        public event Action<double, double> Ubicado;

        private readonly WebView2 web = new WebView2();
        private readonly TextBlock aviso = new TextBlock();
        private bool iniciado, listo, pendiente = true, liberado;

        public MapaControl()
        {
            Background = new SolidColorBrush(Color.FromRgb(0xE8, 0xEE, 0xF2));
            aviso.Text = Loc.T("mapCargando");
            aviso.HorizontalAlignment = HorizontalAlignment.Center;
            aviso.VerticalAlignment = VerticalAlignment.Center;
            aviso.TextAlignment = TextAlignment.Center;
            aviso.TextWrapping = TextWrapping.Wrap;
            aviso.Margin = new Thickness(16);
            aviso.Foreground = Brushes.Black;

            web.Visibility = Visibility.Hidden;   // hasta que el mapa esté listo se ve el texto "Cargando..."
            Children.Add(aviso);
            Children.Add(web);

            Loaded += (s, e) => Iniciar();
            Unloaded += (s, e) => Liberar();
        }

        // ------------------------------------------------------------------ arranque

        private static Task<CoreWebView2Environment> Entorno()
        {
            if (entorno == null)
            {
                string datos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BungaloApp", "WebView2");
                entorno = CoreWebView2Environment.CreateAsync(null, datos);
            }
            return entorno;
        }

        private static string EscribirHtml()
        {
            string carpeta = Path.Combine(Path.GetTempPath(), "BungaloMapa");
            if (!htmlEscrito)
            {
                Directory.CreateDirectory(carpeta);
                File.WriteAllText(Path.Combine(carpeta, "index.html"), MapaHtml.Pagina, new UTF8Encoding(false));
                htmlEscrito = true;
            }
            return carpeta;
        }

        private async void Iniciar()
        {
            if (iniciado || liberado) return;
            iniciado = true;
            try
            {
                CoreWebView2Environment env = await Entorno();
                await web.EnsureCoreWebView2Async(env);
                if (liberado) return;

                string carpeta = EscribirHtml();
                CoreWebView2 core = web.CoreWebView2;
                // Servir la página desde un "host" propio hace que los mapas reciban un Referer válido
                core.SetVirtualHostNameToFolderMapping(Host, carpeta, CoreWebView2HostResourceAccessKind.Allow);
                core.Settings.AreDefaultContextMenusEnabled = false;
                core.Settings.IsStatusBarEnabled = false;
                core.WebMessageReceived += Mensaje;
                core.Navigate("https://" + Host + "/index.html");
            }
            catch (Exception ex)
            {
                entorno = null;
                if (!liberado) MostrarError(Loc.T("mapError") + "\n\n" + ex.Message);
            }
        }

        private void MostrarError(string texto)
        {
            web.Visibility = Visibility.Hidden;
            aviso.Text = texto;
            aviso.Visibility = Visibility.Visible;
        }

        private void Liberar()
        {
            if (liberado) return;
            liberado = true;
            try { web.Dispose(); } catch (Exception) { }
        }

        // ------------------------------------------------------------------ comunicación con la página

        private void Mensaje(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string m;
            try { m = e.TryGetWebMessageAsString(); }
            catch (Exception) { return; }
            if (m == null) return;

            if (m == "ready")
            {
                listo = true;
                aviso.Visibility = Visibility.Collapsed;
                web.Visibility = Visibility.Visible;
                if (pendiente) Enviar();
            }
            else if (m == "offline")
            {
                MostrarError(Loc.T("mapError"));
            }
            else if (m.StartsWith("view:"))
            {
                ultimaVista = m.Substring(5);
            }
            else if (m.StartsWith("pick:"))
            {
                string[] p = m.Substring(5).Split(',');
                double la, lo;
                if (p.Length == 2
                    && double.TryParse(p[0], NumberStyles.Float, CultureInfo.InvariantCulture, out la)
                    && double.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lo))
                {
                    Action<double, double> avisar = Ubicado;
                    if (avisar != null) avisar(la, lo);
                }
            }
            else if (m.StartsWith("open:"))
            {
                string nombre = m.Substring(5);
                // Se aplaza: al navegar se destruye este control y no conviene hacerlo dentro de su propio evento
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Action<string> h = Abrir;
                    if (h != null) h(nombre);
                }));
            }
        }

        /// <summary>Vuelve a enviar los alojamientos y los textos (p. ej. al cambiar de idioma).</summary>
        public void Actualizar()
        {
            pendiente = true;
            if (listo) Enviar();
        }

        private void Enviar()
        {
            pendiente = false;
            try { web.CoreWebView2.PostWebMessageAsJson(ConstruirJson()); }
            catch (Exception) { }
        }

        // ------------------------------------------------------------------ datos -> JSON

        private static string EstadoMapa(Alojamiento a)
        {
            if (a.Estado == "Ocupado") return "occupied";
            if (a.Estado == "Disponible")
            {
                bool reservado = DataStore.Reservas.Any(r => r.Alojamiento == a.Nombre
                    && (r.Estado == "Confirmada" || r.Estado == "Pendiente")
                    && r.FechaSalida.Date >= DateTime.Today);
                return reservado ? "reserved" : "free";
            }
            return "unavailable";   // mantenimiento / limpieza
        }

        private static string Q(string s)
        {
            if (s == null) s = "";
            var sb = new StringBuilder("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': break;
                    case '\t': sb.Append(' '); break;
                    default: sb.Append(c); break;
                }
            }
            return sb.Append('"').ToString();
        }

        private static string Num(double d)
        {
            return d.ToString("0.000000", CultureInfo.InvariantCulture);
        }

        private string ConstruirJson()
        {
            var sb = new StringBuilder();
            sb.Append("{\"labels\":{");
            sb.Append("\"map\":" + Q(Loc.T("mapPlano")));
            sb.Append(",\"sat\":" + Q(Loc.T("mapSatelite")));
            sb.Append(",\"fit\":" + Q(Loc.T("mapCentrar")));
            sb.Append(",\"type\":" + Q(Loc.T("mapTipo")));
            sb.Append(",\"capacity\":" + Q(Loc.T("mapCapacidad")));
            sb.Append(",\"price\":" + Q(Loc.T("mapPrecio")));
            sb.Append(",\"client\":" + Q(Loc.T("mapCliente")));
            sb.Append(",\"view\":" + Q(Loc.T("mapVer")));
            sb.Append(",\"hint\":" + Q(Loc.T("mapUbicarAyuda")));
            sb.Append("},");

            if (ultimaVista != null && !ModoSeleccion) sb.Append("\"view\":" + Q(ultimaVista) + ",");

            sb.Append("\"items\":[");
            bool primero = true;
            foreach (Alojamiento a in DataStore.Alojamientos)
            {
                if (Excluir != null && a.Nombre == Excluir) continue;
                if (a.Lat == 0 && a.Lng == 0) continue;   // sin coordenadas: no se dibuja

                string estado = EstadoMapa(a);
                string textoEstado = estado == "reserved" ? Loc.T("reservado") : Loc.Translate(a.Estado);
                Reserva res = DataStore.Reservas.FirstOrDefault(r => r.Alojamiento == a.Nombre && r.Estado == "Confirmada"
                    && r.FechaEntrada.Date <= DateTime.Today && DateTime.Today < r.FechaSalida.Date);
                string tipo = string.IsNullOrEmpty(a.Tipo) ? "?" : a.Tipo.Substring(0, 1).ToUpper();

                if (!primero) sb.Append(',');
                primero = false;
                sb.Append("{\"name\":" + Q(a.Nombre));
                sb.Append(",\"kind\":" + Q(tipo));
                sb.Append(",\"typeText\":" + Q(Loc.Translate(a.Tipo)));
                sb.Append(",\"state\":" + Q(estado));
                sb.Append(",\"stateText\":" + Q(textoEstado));
                sb.Append(",\"lat\":" + Num(a.Lat));
                sb.Append(",\"lng\":" + Num(a.Lng));
                sb.Append(",\"cap\":" + a.CapacidadMax.ToString(CultureInfo.InvariantCulture));
                sb.Append(",\"pers\":" + a.PersonasAlojadas.ToString(CultureInfo.InvariantCulture));
                sb.Append(",\"price\":" + Q(a.PrecioNoche.ToString("0.##", CultureInfo.CurrentCulture) + " €"));
                sb.Append(",\"client\":" + Q(res != null ? res.Cliente : ""));
                sb.Append("}");
            }
            sb.Append("]");
            if (ModoSeleccion)
            {
                bool tiene = PuntoLat.HasValue && PuntoLng.HasValue;
                sb.Append(",\"pick\":{\"has\":" + (tiene ? "true" : "false"));
                if (tiene) sb.Append(",\"lat\":" + Num(PuntoLat.Value) + ",\"lng\":" + Num(PuntoLng.Value));
                sb.Append("}");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    /// <summary>Ventana para elegir la ubicación de un alojamiento haciendo clic (o arrastrando el marcador) en el mapa.</summary>
    public sealed class UbicacionWindow : Window
    {
        private readonly MapaControl mapa = new MapaControl();
        private readonly TextBlock lblCoords = new TextBlock { VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap };
        private readonly Button btnOk = new Button { IsEnabled = false, IsDefault = true, MinWidth = 90, Padding = new Thickness(10, 4, 10, 4), Margin = new Thickness(8, 0, 0, 0) };
        private double lat, lng;
        private bool elegido;

        private UbicacionWindow(double? lat0, double? lng0, string excluir)
        {
            Title = Loc.T("mapUbicarTitulo");
            Width = 780;
            Height = 580;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            if (Application.Current.Resources.Contains("AppBg")) Background = (Brush)Application.Current.Resources["AppBg"];
            if (Application.Current.Resources.Contains("TextBrush")) Foreground = (Brush)Application.Current.Resources["TextBrush"];

            mapa.ModoSeleccion = true;
            mapa.PuntoLat = lat0;
            mapa.PuntoLng = lng0;
            mapa.Excluir = excluir;
            if (lat0.HasValue && lng0.HasValue)
            {
                lat = lat0.Value;
                lng = lng0.Value;
                elegido = true;
                btnOk.IsEnabled = true;
                MostrarCoords();
            }
            mapa.Ubicado += (la, lo) =>
            {
                lat = la;
                lng = lo;
                elegido = true;
                btnOk.IsEnabled = true;
                MostrarCoords();
            };

            btnOk.Content = Loc.T("mapOk");
            btnOk.Click += (s, e) => { DialogResult = true; };
            var btnCancelar = new Button
            {
                Content = Loc.T("mapCancel"), IsCancel = true, MinWidth = 90,
                Padding = new Thickness(10, 4, 10, 4), Margin = new Thickness(8, 0, 0, 0)
            };

            var botones = new StackPanel { Orientation = Orientation.Horizontal };
            botones.Children.Add(btnOk);
            botones.Children.Add(btnCancelar);

            var barra = new DockPanel { Margin = new Thickness(10) };
            DockPanel.SetDock(botones, Dock.Right);
            barra.Children.Add(botones);
            barra.Children.Add(lblCoords);

            var raiz = new DockPanel();
            DockPanel.SetDock(barra, Dock.Bottom);
            raiz.Children.Add(barra);
            raiz.Children.Add(mapa);
            Content = raiz;
        }

        private void MostrarCoords()
        {
            lblCoords.Text = Loc.F("mapCoords", lat.ToString("0.00000", CultureInfo.InvariantCulture), lng.ToString("0.00000", CultureInfo.InvariantCulture));
        }

        /// <summary>Abre el selector. Devuelve true si el usuario acepta una ubicación.</summary>
        public static bool Elegir(Window propietario, double? lat, double? lng, string excluir, out double nuevaLat, out double nuevaLng)
        {
            var w = new UbicacionWindow(lat, lng, excluir) { Owner = propietario };
            bool ok = w.ShowDialog() == true && w.elegido;
            nuevaLat = w.lat;
            nuevaLng = w.lng;
            return ok;
        }
    }

    /// <summary>Página HTML del mapa. Solo usa comillas simples para poder ir en un literal de C#.</summary>
    internal static class MapaHtml
    {
        public const string Pagina = @"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.min.css'>
<script src='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.min.js'></script>
<style>
html, body, #map { height: 100%; margin: 0; background: #e8eef2; font-family: 'Segoe UI', sans-serif; }
.pin { width: 30px; height: 30px; border-radius: 50%; border: 3px solid #fff; box-shadow: 0 1px 5px rgba(0,0,0,.6);
       color: #fff; font-weight: 700; font-size: 13px; display: flex; align-items: center; justify-content: center; box-sizing: border-box; }
.free { background: #43A047; } .occupied { background: #E53935; } .reserved { background: #FDD835; color: #333; } .unavailable { background: #757575; }
.pin.reserved { color: #333; }
.pick { background: #1A73E8; }
.hint { background: rgba(255,255,255,.93); padding: 6px 10px; border-radius: 4px; font-size: 12px; border: 1px solid #888; max-width: 340px; }
.dot { display: inline-block; width: 10px; height: 10px; border-radius: 50%; margin-right: 5px; }
.pop b { font-size: 14px; }
.pop table { border-collapse: collapse; margin-top: 4px; }
.pop td { padding: 1px 10px 1px 0; font-size: 12px; }
.pop button { margin-top: 8px; width: 100%; padding: 6px; background: #1A73E8; color: #fff; border: 0; border-radius: 3px; font-weight: 700; cursor: pointer; }
</style>
</head>
<body>
<div id='map'></div>
<script>
var map, markers, osm, sat, layersCtl, items = [], labels = {}, dibujado = false, pickMode = false, pickMarker = null;

function post(m) { window.chrome.webview.postMessage(m); }

function esc(s) {
  return String(s).replace(/[&<>']/g, function (c) {
    return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '\'': '&#39;' }[c];
  });
}

function fit() {
  if (!items.length) return;
  var b = L.latLngBounds(items.map(function (i) { return [i.lat, i.lng]; }));
  map.fitBounds(b, { padding: [40, 40], maxZoom: 17 });
}

function popup(it) {
  var h = `<div class='pop'><b>${esc(it.name)}</b><br><span class='dot ${it.state}'></span>${esc(it.stateText)}<table>` +
    `<tr><td>${esc(labels.type)}</td><td>${esc(it.typeText)}</td></tr>` +
    `<tr><td>${esc(labels.capacity)}</td><td>${it.pers}/${it.cap}</td></tr>` +
    `<tr><td>${esc(labels.price)}</td><td>${esc(it.price)}</td></tr>`;
  if (it.client) h += `<tr><td>${esc(labels.client)}</td><td>${esc(it.client)}</td></tr>`;
  h += '</table>';
  if (!pickMode) h += `<button data-n='${esc(it.name)}'>${esc(labels.view)}</button>`;
  return h + '</div>';
}

function render(d) {
  labels = d.labels;
  items = d.items;
  pickMode = !!d.pick;

  var base = {};
  base[labels.map] = osm;
  base[labels.sat] = sat;
  if (layersCtl) map.removeControl(layersCtl);
  layersCtl = L.control.layers(base, null, { position: 'topright' }).addTo(map);
  document.getElementById('fitbtn').title = labels.fit;

  markers.clearLayers();
  items.forEach(function (it) {
    var icon = L.divIcon({
      className: '',
      html: `<div class='pin ${it.state}'>${esc(it.kind)}</div>`,
      iconSize: [30, 30], iconAnchor: [15, 15], popupAnchor: [0, -16]
    });
    L.marker([it.lat, it.lng], { icon: icon, title: it.name }).bindPopup(popup(it)).addTo(markers);
  });

  if (!dibujado) {
    if (d.pick && d.pick.has) {
      map.setView([d.pick.lat, d.pick.lng], 17, { animate: false });
    } else if (!d.pick && d.view) {
      var p = d.view.split(',');
      map.setView([parseFloat(p[0]), parseFloat(p[1])], parseInt(p[2], 10), { animate: false });
    } else {
      fit();
    }
    dibujado = true;
  }
  if (d.pick && !pickMarker) iniciarSeleccion(d);
}

function iniciarSeleccion(d) {
  var icono = L.divIcon({ className: '', html: `<div class='pin pick'>&#9733;</div>`, iconSize: [30, 30], iconAnchor: [15, 15] });
  pickMarker = L.marker([0, 0], { draggable: true, icon: icono, zIndexOffset: 1000 });
  function avisar() {
    var p = pickMarker.getLatLng();
    post('pick:' + p.lat.toFixed(6) + ',' + p.lng.toFixed(6));
  }
  pickMarker.on('dragend', avisar);
  map.on('click', function (ev) {
    pickMarker.setLatLng(ev.latlng);
    if (!map.hasLayer(pickMarker)) pickMarker.addTo(map);
    avisar();
  });
  if (d.pick.has) { pickMarker.setLatLng([d.pick.lat, d.pick.lng]); pickMarker.addTo(map); }
  var Ayuda = L.Control.extend({
    options: { position: 'bottomleft' },
    onAdd: function () { var c = L.DomUtil.create('div', 'hint'); c.textContent = labels.hint; return c; }
  });
  new Ayuda().addTo(map);
}

function init() {
  map = L.map('map', { worldCopyJump: true, minZoom: 2 }).setView([38.966, -2.87], 15);
  osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19, attribution: '&copy; OpenStreetMap' }).addTo(map);
  sat = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', { maxZoom: 19, attribution: 'Tiles &copy; Esri' });
  markers = L.layerGroup().addTo(map);
  L.control.scale({ imperial: false }).addTo(map);

  var Fit = L.Control.extend({
    options: { position: 'topleft' },
    onAdd: function () {
      var d = L.DomUtil.create('div', 'leaflet-bar');
      var a = L.DomUtil.create('a', '', d);
      a.id = 'fitbtn'; a.href = '#'; a.innerHTML = '&#8962;';
      a.style.fontSize = '18px'; a.style.lineHeight = '30px';
      L.DomEvent.on(a, 'click', function (ev) { L.DomEvent.stop(ev); fit(); });
      return d;
    }
  });
  new Fit().addTo(map);

  map.on('moveend', function () {
    if (!dibujado || pickMode) return;
    var c = map.getCenter();
    post('view:' + c.lat.toFixed(6) + ',' + c.lng.toFixed(6) + ',' + map.getZoom());
  });
  map.on('popupopen', function (e) {
    var b = e.popup.getElement().querySelector('button');
    if (b) b.onclick = function () { post('open:' + b.getAttribute('data-n')); };
  });
  window.chrome.webview.addEventListener('message', function (ev) { render(ev.data); });
  post('ready');
}

if (typeof L === 'undefined') { post('offline'); } else { init(); }
</script>
</body>
</html>";
    }
}
