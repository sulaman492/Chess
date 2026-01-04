using System.Windows;
using System.Windows.Controls;

namespace Chess.UI
{
    public partial class MultiplayerWindow : Window
    {
        public MultiplayerWindow()
        {
            InitializeComponent();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close();
        }
        private void StartMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            // Multiplayer → NO AI
            bool Ai = false;

            string timeControl =
                ((ComboBoxItem)MultiplayerTimeCombo.SelectedItem)
                .Content.ToString();

            int depth = 0; // No AI search in multiplayer

            // Open actual game window
            Window1 gameWindow = new Window1(Ai, timeControl, depth);
            gameWindow.Show();
            this.Close();
        }
    }
}
