using System;
using System.Threading.Tasks;
using System.Windows;

namespace BungaloApp
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            IniciarTemporizador();
        }

        private async void IniciarTemporizador()
        {
            await Task.Delay(5000);

            LoginWindow ventanaLogin = new LoginWindow();
            ventanaLogin.Show();

            this.Close();
        }
    }
}