    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Chess.Core;
    using Chess.Pieces;
    using Chess.Moves;
    using Chess.ChessAI;
    using System.Windows.Threading;
using System.IO;
using System.Media;


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
        private SoundPlayer _moveSoundPlayer;


        private readonly Image[,] pieceImage = new Image[8, 8];
            private readonly Rectangle[,] highlights= new Rectangle[8, 8];
            private readonly Dictionary<Position,Move> moveCache= new Dictionary<Position,Move>();
            private GameState gameState;
            public int Depth;
            private Position selectedPos;
            private DispatcherTimer _timer;
            private TimeSpan _whiteTime;
            private TimeSpan _blackTime;
            private bool _isTimedGame;
            private Player _currentPlayer = Player.White;
            public event Action<Player, EndReason> GameOver;
            public Window1(bool Ai,string time,int depth)
            {
                isAI = Ai;
                Depth = depth;
                //Time = time;
                InitializeComponent();
                InitializeBoard();
                gameState = new GameState(Board.Initial());
            gameState.MoveMade += PlayMoveSound;
            InitializeMoveSound();



            gameState.GameOver += (winner, reason) =>
            {
                Dispatcher.Invoke(() =>
                {
                    // Disable board interaction
                    BoardGrid.IsEnabled = false;

                    // Hide buttons
                    UndoButton.Visibility = Visibility.Collapsed;
                    RedoButton.Visibility = Visibility.Collapsed;
                    Next.Visibility = Visibility.Collapsed;
                    Prev.Visibility = Visibility.Collapsed;

                    // Show GameOver window
                    GameOver gameOverWindow = new GameOver(winner, reason, this);
                    gameOverWindow.ShowDialog();
                });
            };

            DrawBoard(gameState.board);
                InitializeTimeControl(time);
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

            private Position ToSquarePosition(Point point)
            {
                double squareSize = BoardGrid.ActualWidth / 8;
                int row = (int)(point.Y / squareSize);
                int col = (int)(point.X / squareSize);
                return new Position(row, col);
            }
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
            AddMoveToUI(move);
            HighlightCheck();
            if (isAI && gameState.Currentplayer == aiPlayer)
                {
                    // AI's turn
                    MakeAIMove();
                }
                _currentPlayer = gameState.Currentplayer;
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
                AddMoveToUI(aiMove);


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
            private void InitializeTimeControl(string timeControl)
            {
                if (timeControl == "No Timer")
                {
                    _isTimedGame = false;
                    WhiteTimerText.Visibility = Visibility.Collapsed;
                    BlackTimerText.Visibility = Visibility.Collapsed;
                    return;
                }

                _isTimedGame = true;

                // Example: "5 Minutes" → 5
                int minutes = int.Parse(timeControl.Split(' ')[0]);

                _whiteTime = TimeSpan.FromMinutes(minutes);
                _blackTime = TimeSpan.FromMinutes(minutes);

                UpdateTimerUI();
                StartTimer();
            }
            private void StartTimer()
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }

            private void Timer_Tick(object? sender, EventArgs e)
            {
                if (!_isTimedGame) return;

                if (_currentPlayer == Player.White)
                {
                    _whiteTime = _whiteTime.Subtract(TimeSpan.FromSeconds(1));
                    if (_whiteTime <= TimeSpan.Zero)
                    {
                        EndGame(Player.Black, "White ran out of time!");
                    }
                }
                else
                {
                    _blackTime = _blackTime.Subtract(TimeSpan.FromSeconds(1));
                    if (_blackTime <= TimeSpan.Zero)
                    {
                        EndGame(Player.White, "Black ran out of time!");
                    }
                }

                UpdateTimerUI();
            }
            private void EndGame(Player winner, string reason)
            {
                // Stop the timer
                _timer?.Stop();

                // Disable board interaction
                BoardGrid.IsEnabled = false;

                // Hide buttons
                UndoButton.Visibility = Visibility.Collapsed;
                RedoButton.Visibility = Visibility.Collapsed;
                Next.Visibility = Visibility.Collapsed;
                Prev.Visibility = Visibility.Collapsed;

                // Determine which player ran out of time
                string message;
                if (winner == Player.White)
                {
                    message = "Black ran out of time! White wins!";
                }
                else if (winner == Player.Black)
                {
                    message = "White ran out of time! Black wins!";
                }
                else
                {
                    message = "Time ran out! Draw!";
                }

                // Show GameOver window EndReason endReason = EndReason.TimeOut; // add this enum value if you haven't yet
                // Use GameState's GameOver event
                EndReason endReason = EndReason.TimeOut; // add this enum value if you haven't yet
                gameState.RaiseGameOver(winner, EndReason.TimeOut);

            }

        

            private void UpdateTimerUI()
            {
                WhiteTimerText.Text = _whiteTime.ToString(@"mm\:ss");
                BlackTimerText.Text = _blackTime.ToString(@"mm\:ss");
            }


        private readonly MediaPlayer _movePlayer = new MediaPlayer();

        private void InitializeMoveSound()
        {
            try
            {
                string path = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "UI",
                    "Sounds",
                    "Move.wav"
                );

                if (!File.Exists(path))
                {
                    MessageBox.Show($"Move.wav not found at:\n{path}");
                    return;
                }

                _moveSoundPlayer = new SoundPlayer(path);
                _moveSoundPlayer.Load(); // 🔥 preload into memory
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PlayMoveSound()
        {
            try
            {
                _moveSoundPlayer?.Play(); // 🔥 instant
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AddMoveToUI(Move move)
        {
            try
            {
                // Calculate total move pairs (each row has 2 moves: white and black)
                int totalMovePairs = MovesPanel.Children.Count;

                // Get the last row if it exists
                StackPanel lastRow = totalMovePairs > 0 ?
                    MovesPanel.Children[totalMovePairs - 1] as StackPanel : null;

                // Check if we need to create a new row or update existing
                if (lastRow == null)
                {
                    // First move - create new row
                    CreateNewMoveRow(move, 1);
                }
                else
                {
                    // Check if the last row has a placeholder for black's move
                    if (lastRow.Children.Count >= 3)
                    {
                        TextBlock blackPlaceholder = lastRow.Children[2] as TextBlock;
                        if (blackPlaceholder != null && blackPlaceholder.Text == "...")
                        {
                            // Update black's move in existing row
                            blackPlaceholder.Text = move.ToSimpleString();
                            blackPlaceholder.Foreground = Brushes.Black;
                        }
                        else
                        {
                            // Black's move already filled, create new row for white
                            CreateNewMoveRow(move, totalMovePairs + 1);
                        }
                    }
                    else
                    {
                        // Row doesn't have 3 children, create new row
                        CreateNewMoveRow(move, totalMovePairs + 1);
                    }
                }

                // Auto-scroll to the latest move
                if (MovesPanel.Parent is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding move to UI: {ex.Message}");
            }
        }

        private void CreateNewMoveRow(Move move, int moveNumber)
        {
            // Create horizontal stack panel for this move pair
            StackPanel moveRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 2, 0, 2)
            };

            // Add move number
            TextBlock numberText = new TextBlock
            {
                Text = $"{moveNumber}. ",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Width = 30,
                Foreground = Brushes.DarkSlateGray
            };
            moveRow.Children.Add(numberText);

            // Add the current move (white's move when creating new row)
            TextBlock moveText = new TextBlock
            {
                Text = move.ToSimpleString(), // Shows "e2e4"
                FontSize = 14,
                Width = 60,
                Foreground = Brushes.Black,
                Margin = new Thickness(5, 0, 10, 0)
            };
            moveRow.Children.Add(moveText);

            // Add placeholder for the opposite color's move
            TextBlock placeholder = new TextBlock
            {
                Text = "...",
                FontSize = 14,
                Width = 60,
                Foreground = Brushes.Gray,  
                Margin = new Thickness(5, 0, 0, 0)
            };
            moveRow.Children.Add(placeholder);

            MovesPanel.Children.Add(moveRow);
        }
        private void ResignButton_Click(object sender, RoutedEventArgs e)
        {
            Player winner = gameState.Currentplayer.Opponent();
            MessageBox.Show($"{gameState.Currentplayer} resigned! {winner} wins!");
            gameState.RaiseGameOver(winner, EndReason.Resignation); // make sure EndReason.Resignation exists
        }
        private void HighlightCheck()
        {
            // First, clear previous check highlights (but keep move highlights)
            ClearCheckHighlights();

            // Check if current player's king is in check
            Position kingPos = gameState.board.FindKing(gameState.Currentplayer);

            if (kingPos != null && gameState.board.IsInCheck(gameState.Currentplayer))
            {
                // Highlight the king's square in DARK RED
                Color checkColor = Color.FromArgb(200, 139, 0, 0); // Dark red (Maroon) with high opacity
                highlights[kingPos.Row, kingPos.Column].Fill = new SolidColorBrush(checkColor);
            }
        }

        private void ClearCheckHighlights()
        {
            // Only clear squares that are not part of moveCache highlights
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    // Only clear if not a legal move highlight
                    if (!moveCache.ContainsKey(pos))
                    {
                        highlights[r, c].Fill = Brushes.Transparent;
                    }
                }
            }
        }



    }
}