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

        // <summary>
        //// Constructor for GameOver window
        /// </summary>
        /// <param name="message">Winner message: "White wins!", "Black wins!" or "Draw!"</param>
        /// <param name="mainWindow">Reference to main window to reset or replay</param>
        public GameOver(string message, Window1 window1)
        {
            InitializeComponent();
            WinnerText.Text = message;
            this.window1= window1;
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
//            mainWindow.ResetGame(); // call a method in MainWindow to reset board & GameState
            this.Close();
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            window1.ResetToInitialForReplay();
            this.Close();
        }
    }
}
