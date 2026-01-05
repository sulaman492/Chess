using System.Windows;
using System.Windows.Controls;
using Chess.Core;

namespace Chess.UI
{
    public partial class ComputerWindow : Window
    {
        public ComputerWindow()
        {
            InitializeComponent();
        }

        private int GetDepthFromDifficulty(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 3,
                "Medium" => 4,
                "Hard" => 5,
                _ => 3
            };
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            //Player humanColor = WhiteRadio.IsChecked == true
            //    ? Player.White
            //    : Player.Black;

            string difficulty = ((ComboBoxItem)DifficultyCombo.SelectedItem).Content.ToString();
            string timeControl = ((ComboBoxItem)TimeControlCombo.SelectedItem).Content.ToString();

            bool Ai = true;

            // Open actual game window
            int depth = GetDepthFromDifficulty(difficulty);
            Window1 gameWindow = new Window1(Ai,timeControl,depth);
            gameWindow.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainMenu().Show();
            this.Close();
        }
    }
}
