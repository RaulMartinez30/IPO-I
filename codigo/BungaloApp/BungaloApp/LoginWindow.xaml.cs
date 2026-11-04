using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BungaloApp
{
    public partial class LoginWindow : Window
    {
        private string usuarioValido = "admin";
        private string passwordValido = "1234";

        // Declaramos las variables pero NO las instanciamos aquí
        private BitmapImage imgIdle;
        private BitmapImage imgFocus;
        private BitmapImage imgCerrados;
        private BitmapImage imgAbiertos;

        public LoginWindow()
        {
            InitializeComponent();

            // Las inicializamos dentro del constructor
            try
            {
                // Si tienes la carpeta Images en la raíz del proyecto, estas rutas son las buenas
                imgIdle = new BitmapImage(new Uri("pack://application:,,,/Images/mono_idle.png"));
                imgFocus = new BitmapImage(new Uri("pack://application:,,,/Images/mono_focus.png"));
                imgCerrados = new BitmapImage(new Uri("pack://application:,,,/Images/mono_ojos_cerrados.png"));
                imgAbiertos = new BitmapImage(new Uri("pack://application:,,,/Images/mono_ojos_abiertos.png"));
            }
            catch (Exception ex)
            {
                // Si peta, te saltará este aviso y sabrás por qué no cargaba
                MessageBox.Show("Fallo al cargar las imágenes del mono. Revisa las rutas.\nError: " + ex.Message, "Modo Debug", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        // --- EVENTOS DEL MONO INTERACTIVO ---

        private void txtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            imgMono.Source = imgFocus; // Mono atento mirando el usuario
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            // Si el ojito está activado, asoma. Si no, se tapa los ojos.
            if (btnMostrarPassword.IsChecked == true)
                imgMono.Source = imgAbiertos;
            else
                imgMono.Source = imgCerrados;
        }

        private void txtPasswordVisible_GotFocus(object sender, RoutedEventArgs e)
        {
            imgMono.Source = imgAbiertos; // Se asoma porque estás viendo la clave
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            // Si hacemos click fuera de los cuadros de texto, el mono vuelve a su estado normal
            if (!txtUsuario.IsKeyboardFocusWithin && !txtPassword.IsKeyboardFocusWithin && !txtPasswordVisible.IsKeyboardFocusWithin)
            {
                imgMono.Source = imgIdle;
            }
        }

        // --- LÓGICA DEL OJITO DE CONTRASEÑA ---

        private void btnMostrarPassword_Checked(object sender, RoutedEventArgs e)
        {
            // Pasa el texto, oculta los puntitos y muestra el texto claro
            txtPasswordVisible.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtPasswordVisible.Visibility = Visibility.Visible;
            txtPasswordVisible.Focus();

            imgMono.Source = imgAbiertos;
        }

        private void btnMostrarPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            // Devuelve el texto al campo oculto y lo muestra
            txtPassword.Password = txtPasswordVisible.Text;
            txtPasswordVisible.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();

            imgMono.Source = imgCerrados;
        }

        private void txtPasswordVisible_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Mantiene sincronizadas ambas contraseñas si el usuario escribe mientras la ve
            if (btnMostrarPassword.IsChecked == true)
                txtPassword.Password = txtPasswordVisible.Text;
        }

        // --- LÓGICA DEL LOGIN ---

        private void Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                btnLogin_Click(null, null);
                e.Handled = true;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool hayErrores = false;

            // Pillamos la password dependiendo de si el ojo está activado o no
            string passwordActual = btnMostrarPassword.IsChecked == true ? txtPasswordVisible.Text : txtPassword.Password;

            lblErrorUsuario.Visibility = Visibility.Collapsed;
            lblErrorPassword.Visibility = Visibility.Collapsed;
            txtUsuario.BorderBrush = Brushes.Black;
            txtPassword.BorderBrush = Brushes.Black;
            txtPasswordVisible.BorderBrush = Brushes.Black;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                lblErrorUsuario.Text = "El campo de usuario no puede estar vacío";
                lblErrorUsuario.Visibility = Visibility.Visible;
                txtUsuario.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (string.IsNullOrWhiteSpace(passwordActual))
            {
                lblErrorPassword.Text = "La contraseña no puede estar vacía";
                lblErrorPassword.Visibility = Visibility.Visible;
                txtPassword.BorderBrush = Brushes.Red;
                txtPasswordVisible.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (hayErrores) return;

            if (txtUsuario.Text.Equals(usuarioValido) && passwordActual.Equals(passwordValido))
            {
                MainWindow principal = new MainWindow();
                principal.Show();
                this.Close();
            }
            else
            {
                lblErrorUsuario.Text = "Usuario o contraseña incorrectos";
                lblErrorUsuario.Visibility = Visibility.Visible;
                txtUsuario.BorderBrush = Brushes.Red;
                txtPassword.BorderBrush = Brushes.Red;
                txtPasswordVisible.BorderBrush = Brushes.Red;
            }
        }
    }
}