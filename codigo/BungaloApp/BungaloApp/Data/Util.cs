using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BungaloApp
{
    public static class Util
    {
        /// <summary>Carga una imagen de la carpeta Images. Si no existe devuelve null (no rompe la app).</summary>
        private static readonly System.Collections.Generic.Dictionary<string, BitmapImage> cache =
            new System.Collections.Generic.Dictionary<string, BitmapImage>();

        public static BitmapImage Img(string nombre)
        {
            BitmapImage img;
            if (cache.TryGetValue(nombre, out img)) return img;
            try
            {
                img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri("pack://application:,,,/Images/" + nombre, UriKind.Absolute);
                img.EndInit();
                img.Freeze();
            }
            catch
            {
                img = null;
            }
            cache[nombre] = img;
            return img;
        }

        public static Brush Rec(string clave)
        {
            return (Brush)Application.Current.Resources[clave];
        }
    }
}
