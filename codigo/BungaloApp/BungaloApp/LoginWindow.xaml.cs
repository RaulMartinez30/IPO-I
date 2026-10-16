using System;
using System.Windows;
using System.Windows.Media;

namespace BungaloApp
{
    public partial class LoginWindow : Window
    {
        // Datos de prueba para el login (puedes cambiarlos a tu gusto)
        private string usuarioValido = "admin";
        private string passwordValido = "1234";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool hayErrores = false;

            lblErrorUsuario.Visibility = Visibility.Collapsed;
            lblErrorPassword.Visibility = Visibility.Collapsed;
            txtUsuario.BorderBrush = Brushes.Black;
            txtPassword.BorderBrush = Brushes.Black;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                lblErrorUsuario.Text = "El campo de usuario no puede estar vacío";
                lblErrorUsuario.Visibility = Visibility.Visible;
                txtUsuario.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                lblErrorPassword.Text = "La contraseña no puede estar vacía";
                lblErrorPassword.Visibility = Visibility.Visible;
                txtPassword.BorderBrush = Brushes.Red;
                hayErrores = true;
            }

            if (hayErrores) return;

            if (txtUsuario.Text.Equals(usuarioValido) && txtPassword.Password.Equals(passwordValido))
            {
                MessageBox.Show("¡Autenticación correcta! Entrando al sistema...", "BungaloApp", MessageBoxButton.OK, MessageBoxImage.Information);

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
            }
        }
    }
}