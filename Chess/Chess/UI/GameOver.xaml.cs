using System.Windows;
using Chess.Core;

namespace Chess.UI
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver : Window
    {
        private readonly Window1 window1;

        /// <summary>
        /// Constructor for GameOver window
        /// </summary>
        /// <param name="winner">Player who won or Player.None for draw</param>
        /// <param name="reason">End reason (checkmate, stalemate, timeout, etc.)</param>
        /// <param name="window1">Reference to main window to reset or replay</param>
        public GameOver(Player winner, EndReason reason, Window1 window1)
        {
            InitializeComponent();
            this.window1 = window1;

            string message = "";

            if (winner == Player.None)
            {
                // Draw scenarios
                message = reason switch
                {
                    EndReason.stalemate => "Draw by stalemate",
                    EndReason.fiftyMoveRule => "Draw by fifty-move rule",
                    EndReason.InsufficientMaterial => "Draw by insufficient material",
                    EndReason.ThreeFoldRepitition => "Draw by threefold repetition",
                    EndReason.TimeOut => "Draw by timeout", // <-- added timeout
                    _ => "Draw"
                };
            }
            else
            {
                // Win scenarios
                message = reason switch
                {
                    EndReason.checkmate => $"{winner} wins by checkmate",
                    EndReason.TimeOut => $"{winner} wins by timeout", // <-- added timeout
                    _ => $"{winner} wins"
                };
            }

            WinnerText.Text = message;
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Restart the game
        /// </summary>
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // You should have a ResetGame method in Window1 to reset GameState and UI
            // window1.ResetGame(); 
            this.Close();
        }

        /// <summary>
        /// Replay the game
        /// </summary>
        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            window1.ResetToInitialForReplay();
            this.Close();
        }
    }
}
