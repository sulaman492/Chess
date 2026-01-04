using System.Windows;

namespace Chess.UI
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Multiplayer_Click(object sender, RoutedEventArgs e)
        {
            MultiplayerWindow multiplayerWindow = new MultiplayerWindow();
            multiplayerWindow.Show();

            this.Close(); // optional (remove if you want menu to stay)
        }

        private void Computer_Click(object sender, RoutedEventArgs e)
        {
            ComputerWindow computerWindow = new ComputerWindow();
            computerWindow.Show();

            this.Close(); // optional
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
