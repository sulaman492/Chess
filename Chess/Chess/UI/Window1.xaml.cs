using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chess.Core;
using Chess.Pieces;
using Chess.Moves;
using Chess.ChessAI;
using Chess.ChessAI;
using Chess.DataStructures;

namespace Chess.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool isAI ; // set true to enable AI, false for Human vs Human
        private Player aiPlayer = Player.Black; // AI plays Black
        string Difficulty;
        string Time;

        private readonly Image[,] pieceImage = new Image[8, 8];
        private readonly Rectangle[,] highlights= new Rectangle[8, 8];
        private readonly Dictionary<Position,Move> moveCache= new Dictionary<Position,Move>();
        private GameState gameState;
        public int Depth;
        private Position selectedPos;
        public Window1(bool Ai,string time,int depth)
        {
            isAI = Ai;
            Depth = depth;
            //Time = time;
            InitializeComponent();
            InitializeBoard();
            gameState = new GameState(Board.Initial());
            gameState.GameOver += winner =>
            {
                string message;
                if (winner == Player.None)
                    message = "Game Over: Draw/Stalemate!";
                else
                    message = $"Game Over: {winner} wins!";

                // Show your GameOver window
                GameOver gameOverWindow = new GameOver(message, this);
                gameOverWindow.ShowDialog();
            };


            DrawBoard(gameState.board);
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    pieceImage[i, j] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle hightlight = new Rectangle();
                    highlights[i,j]= hightlight;
                    HighlightedGrid.Children.Add(hightlight);
                }
            }


        }
        private void DrawBoard(Board board)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Piece piece = board[i, j];
                    pieceImage[i, j].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);
            Console.WriteLine($"Mouse: X={point.X}, Y={point.Y}");

            // Check if point is outside bounds
            if (point.X < 0 || point.Y < 0 ||
                point.X >= BoardGrid.ActualWidth ||
                point.Y >= BoardGrid.ActualHeight)
            {
                Console.WriteLine("Click outside board!");
                return;
            }
            Position pos = ToSquarePosition(point);

            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }


        //private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Point point = e.GetPosition(BoardGrid);

        //        // Debug with MessageBox to see coordinates
        //        MessageBox.Show($"Click at: X={point.X}, Y={point.Y}\nBoard Size: {BoardGrid.ActualWidth}x{BoardGrid.ActualHeight}");

        //        Position pos = ToSquarePosition(point);

        //        // Check if position is valid
        //        if (pos == null)
        //        {
        //            MessageBox.Show("Position is null - click was outside board");
        //            return;
        //        }

        //        MessageBox.Show($"Converted to: Row={pos.Row}, Col={pos.Column}");

        //        if (selectedPos == null)
        //        {
        //            OnFromPositionSelected(pos);
        //        }
        //        else
        //        {
        //            OnToPositionSelected(pos);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
        //    }
        //}

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);
        }
        //private Position ToSquarePosition(Point point)
        //{
        //    double squareSize = BoardGrid.ActualWidth / 8;

        //    // Calculate row and column using Math.Floor
        //    int col = (int)Math.Floor(point.X / squareSize);
        //    int row = (int)Math.Floor(point.Y / squareSize);

        //    // Debug with MessageBox
        //    MessageBox.Show($"Raw calc: row={row}, col={col} from X={point.X}, Y={point.Y}, squareSize={squareSize}");

        //    // Clamp values to 0-7
        //    col = Math.Clamp(col, 0, 7);
        //    row = Math.Clamp(row, 0, 7);

        //    return new Position(row, col);
        //}
        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMoveForPieces(pos);
            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();
            if(moveCache.TryGetValue(pos,out Move move))
            {
                HandleMove(move);
            }
        }
        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.board);
            UpdateCapturedPiecesUI();
            if (isAI && gameState.Currentplayer == aiPlayer)
            {
                // AI's turn
                MakeAIMove();
            }
        }
        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row,to.Column].Fill=new SolidColorBrush(color);
            }
        }
        private void HideHighlights()
        {
            foreach(Position to in moveCache.Keys) 
            {
                highlights[to.Row,to.Column].Fill=Brushes.Transparent;
            }
        }
        private async void MakeAIMove()
        {

            Move aiMove = await Task.Run(() =>
        IterativeDeepeningAI.FindBestMove(gameState.board, aiPlayer, maxDepth: Depth, timeLimitMs: 7000)
    );

            if (aiMove != null)
            {
                gameState.MakeMove(aiMove);
                DrawBoard(gameState.board);
                UpdateCapturedPiecesUI();
            }
        }
        // Inside Window1.xaml.cs
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
             gameState.UndoMove();  // Make sure you have implemented this
            DrawBoard(gameState.board);
            UpdateCapturedPiecesUI();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            gameState.RedoMove();  // Make sure you have implemented this
            DrawBoard(gameState.board);
            UpdateCapturedPiecesUI();
        }
        private void UpdateCapturedPiecesUI()
        {
            WhiteCapturedPanel.Children.Clear();
            BlackCapturedPanel.Children.Clear();

            foreach (Piece piece in gameState.CapturedWhitePieces)
            {
                Image img = new Image
                {
                    Source = Images.GetImage(piece),
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(2)
                };
                WhiteCapturedPanel.Children.Add(img);
            }

            foreach (Piece piece in gameState.CapturedBlackPieces)
            {
                Image img = new Image
                {
                    Source = Images.GetImage(piece),
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(2)
                };
                BlackCapturedPanel.Children.Add(img);
            }
        }
        public void ResetToInitialForReplay()
        {
            gameState.ResetBoard();      // ← THIS is what you were missing
            DrawBoard(gameState.board);
            UpdateCapturedPiecesUI();

            StartReplayMode();
        }

        public void StartReplayMode()
        {

            gameState.StartReplay();

            DrawBoard(gameState.board);
            UpdateCapturedPiecesUI();

            BoardGrid.IsEnabled = false; // disable normal board interaction

            // Hide normal play buttons
            UndoButton.Visibility = Visibility.Collapsed;
            RedoButton.Visibility = Visibility.Collapsed;

            // Show replay buttons
            Next.Visibility = Visibility.Visible;
            Prev.Visibility = Visibility.Visible;

            MessageBox.Show("Replay mode started. Use Next/Previous buttons to review moves.");

            selectedPos = null;
            HideHighlights();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (gameState.NextMoveInReplay())
            {
                DrawBoard(gameState.board);
                UpdateCapturedPiecesUI();
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (gameState.PreviousMoveInReplay())
            {
                DrawBoard(gameState.board);
                UpdateCapturedPiecesUI();
            }
        }

    }
}