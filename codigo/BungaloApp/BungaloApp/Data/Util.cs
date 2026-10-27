using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BungaloApp
{
    public static class Util
    {
        /// <summary>Carga una imagen de la carpeta Images. Si no existe devuelve null (no rompe la app).</summary>
        public static BitmapImage Img(string nombre)
        {
            try
            {
                return new BitmapImage(new Uri("pack://application:,,,/Images/" + nombre, UriKind.Absolute));
            }
            catch
            {
                return null;
            }
        }

        public static Brush Rec(string clave)
        {
            return (Brush)Application.Current.Resources[clave];
        }
    }
}
