using Chess.Moves;
using Chess.Pieces;
using Chess.DataStructures;
using Chess.UI;
using System.Text;



namespace Chess.Core
{
    public class GameState
    {
        public Board board { get; }
        public Player Currentplayer { get; set; }
        public Result Result { get; set; }
        public event Action MoveMade;
        private Dictionary<string, int> positionCount = new Dictionary<string, int>();
        Graph<string> moveGraph;

        private Stack<MoveRecord> undoStack = new Stack<MoveRecord>();
        private Stack<MoveRecord> redoStack = new Stack<MoveRecord>();
        private Chess.DataStructures.LinkedList<MoveRecord> moveHistory
       = new Chess.DataStructures.LinkedList<MoveRecord>();
        private Node<MoveRecord> replayCurrent;

        public event Action<Player, EndReason> GameOver;

        Queue turnQueue;
        public List<Piece> CapturedWhitePieces { get; } = new List<Piece>();
        public List<Piece> CapturedBlackPieces { get; } = new List<Piece>();
        public GameState(Board board)
        {
            this.board = board;
            turnQueue = new Queue(Player.White, Player.Black);
            Currentplayer = turnQueue.NextTurn();
            replayCurrent = null;
            Result = null;
            moveGraph = new Graph<string>(board.ToFen());
        }

        public IEnumerable<Move> LegalMoveForPieces(Position pos) 
        {
            if(board.IsEmpty(pos) || board[pos].Color != Currentplayer) 
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = board[pos];
            IEnumerable<Move>moveCandidate= piece.GetMoves(pos,board);
            return moveCandidate.Where(move => move.IsLegal(board));
        }
        public void MakeMove(Move move)
        {
            Piece movingPiece = board[move.FromPos];
            Piece CapturedPiece = board[move.ToPos];
            bool HasMovedBefore = movingPiece.HasMoved;

            if (CapturedPiece != null)
            {
                if (CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.Add(CapturedPiece);
                else
                    CapturedBlackPieces.Add(CapturedPiece);
            }

            // Execute the move on board
            move.Execute(board);

            // ======= ADD TO GRAPH =======
            string fen = board.ToFen();
            moveGraph.AddMove(move, fen);

            UpdateRepetition();

            MoveRecord record = new MoveRecord(move, movingPiece, CapturedPiece, HasMovedBefore);
            moveHistory.AddLast(record);
            undoStack.Push(record);

            redoStack = new Stack<MoveRecord>();

            checkForGameOver();

            Currentplayer = turnQueue.NextTurn();
            MoveMade?.Invoke();
        }


        public void UndoMove()
        {
            MoveRecord record=undoStack.Pop();
            if (record == null) { return ; }
            board[record.Move.FromPos] = record.MovingPiece;
            board[record.Move.ToPos] = record.CapturedPiece;
            record.MovingPiece.HasMoved=record.HasMovedBefore;

            if (record.CapturedPiece != null)
            {
                if (record.CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.RemoveAt(CapturedWhitePieces.Count - 1);
                else
                    CapturedBlackPieces.RemoveAt(CapturedBlackPieces.Count - 1);
            }

            redoStack.Push(record);
            Currentplayer= Currentplayer.Opponent();
            moveGraph.MoveBack();
        }
        public void RedoMove()
        {
            MoveRecord record = redoStack.Pop();
            if (record == null) return;

            if (record.CapturedPiece != null)
            {
                if (record.CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.Add(record.CapturedPiece);
                else
                    CapturedBlackPieces.Add(record.CapturedPiece);
            }

            record.Move.Execute(board);
            record.MovingPiece.HasMoved = true;

            undoStack.Push(record);

            Currentplayer = Currentplayer.Opponent();

            // ADD THIS LINE to trigger the event
            MoveMade?.Invoke();
        }
        public List<Move> AllLegalMovesFor(Player player)
        {
            List<Move>moves=new List<Move>();
            foreach(Position pos in board.PiecePositionFor(player))
            {
                Piece piece = board[pos];
                moves.AddRange(piece.GetMoves(pos, board));
            }
            return moves.Where(m=>m.IsLegal(board)).ToList();
        }
        private void checkForGameOver()
        {
            if (!AllLegalMovesFor(Currentplayer).Any())
            {
                if (board.IsInCheck(Currentplayer))
                {
                    Result = Result.Win(Currentplayer.Opponent());
                    GameOver?.Invoke(Currentplayer.Opponent(), EndReason.checkmate);
                }
                else
                {
                    Result = Result.Draw(EndReason.stalemate);
                    GameOver?.Invoke(Player.None, EndReason.stalemate);
                }
            }

            // TODO: later add other rules like fifty-move rule, threefold repetition, etc.
        }


        public bool IsGameOver()
        {
            return Result != null;
        }
        // Minimal MakeMove for search
        public void MakeMoveForSearch(Move move)
        {
            Piece movingPiece = board[move.FromPos];
            Piece capturedPiece = board[move.ToPos];
            bool hasMovedBefore = movingPiece.HasMoved;

            board[move.ToPos] = movingPiece;
            board[move.FromPos] = null;
            movingPiece.HasMoved = true;

            // Push minimal info for undo
            undoStack.Push(new MoveRecord(move, movingPiece, capturedPiece, hasMovedBefore));
        }

        // Minimal UndoMove for search
        public void UndoMoveForSearch()
        {
            MoveRecord record = undoStack.Pop();
            if (record == null) return;

            board[record.Move.FromPos] = record.MovingPiece;
            board[record.Move.ToPos] = record.CapturedPiece;
            record.MovingPiece.HasMoved = record.HasMovedBefore;
        }
        public void StartReplay()
        {
            ResetBoard();
            replayCurrent = moveHistory.Head; 
        }

        public bool NextMoveInReplay()
        {
            if (replayCurrent == null) return false;

            MoveRecord record = replayCurrent.Data;
            record.Move.Execute(board);
            record.MovingPiece.HasMoved = true;

            if (record.CapturedPiece != null)
            {
                if (record.CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.Add(record.CapturedPiece);
                else
                    CapturedBlackPieces.Add(record.CapturedPiece);
            }

            replayCurrent = replayCurrent.Next;
            return true;
        }

        public bool PreviousMoveInReplay()
        {
            // if replayCurrent is null, start from Tail (end of history)
            Node<MoveRecord> prevNode = replayCurrent != null ? replayCurrent.Previous : moveHistory.Tail;
            if (prevNode == null) return false;

            MoveRecord record = prevNode.Data;

            board[record.Move.FromPos] = record.MovingPiece;
            board[record.Move.ToPos] = record.CapturedPiece;
            record.MovingPiece.HasMoved = record.HasMovedBefore;

            if (record.CapturedPiece != null)
            {
                if (record.CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.RemoveAt(CapturedWhitePieces.Count - 1);
                else
                    CapturedBlackPieces.RemoveAt(CapturedBlackPieces.Count - 1);
            }

            replayCurrent = prevNode;
            return true;
        }
        public void ResetBoard()
        {
            board.setAllPieces(); // sets pieces at starting positions

            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();

            undoStack.Clear();
            redoStack.Clear();

            turnQueue = new Queue(Player.White, Player.Black);
            Currentplayer = turnQueue.NextTurn();

            replayCurrent = moveHistory.Head;

            Result = null;
        }
        public static bool IsTerminal(Board board, Player player)
        {
            return !board.PiecePositionFor(player).Any() ||
                   !new GameState(board).AllLegalMovesFor(player).Any();
        }
        public void RaiseGameOver(Player winner, EndReason reason)
        {
            GameOver?.Invoke(winner, reason);
        }
        private string GetBoardHash()
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    if (piece == null)
                    {
                        sb.Append(".");
                    }
                    else
                    {
                        char pieceChar = piece.Type.ToString()[0]; // P, R, N, B, Q, K
                        char colorChar = piece.Color == Player.White ? 'W' : 'B';
                        sb.Append(pieceChar).Append(colorChar);
                    }
                }
            }

            // Include which player's turn it is
            sb.Append(Currentplayer == Player.White ? 'W' : 'B');

            return sb.ToString();
        }
        private void UpdateRepetition()
        {
            string hash = GetBoardHash();

            if (positionCount.ContainsKey(hash))
                positionCount[hash]++;
            else
                positionCount[hash] = 1;

            if (positionCount[hash] >= 3)
            {
                // Threefold repetition detected → Draw
                Result = Result.Draw(EndReason.ThreeFoldRepetition);
                GameOver?.Invoke(Player.None, EndReason.ThreeFoldRepetition);
            }
        }

    }
}
