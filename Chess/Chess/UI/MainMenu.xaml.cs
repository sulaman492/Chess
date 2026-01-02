using System.Windows;

namespace Chess.UI
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        // 🧑‍🤝‍🧑 Multiplayer
        private void Multiplayer_Click(object sender, RoutedEventArgs e)
        {
            //MultiplayerWindow multiplayerWindow = new MultiplayerWindow();
            //multiplayerWindow.Show();
            //this.Close();
        }

        // 🤖 Vs Computer
        private void Computer_Click(object sender, RoutedEventArgs e)
        {
            ComputerWindow computerWindow = new ComputerWindow();
            computerWindow.Show();
            this.Close();
        }

        // ❌ Exit
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
