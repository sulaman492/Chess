using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Moves;
using Chess.Pieces;
using Chess.DataStructures;


namespace Chess.Core
{
    public class GameState
    {
        public Board board { get; }
        public Player Currentplayer { get; set; }
        public Result Result { get; set; }

        private stack undoStack=new stack();
        private stack redoStack=new stack();
        Queue turnQueue;
        public List<Piece> CapturedWhitePieces { get; } = new List<Piece>();
        public List<Piece> CapturedBlackPieces { get; } = new List<Piece>();
        public GameState(Board board)
        {
            this.board = board;
            turnQueue = new Queue(Player.White, Player.Black);
            Currentplayer = turnQueue.NextTurn();

            Result = null;
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
            Piece CapturedPiece=board[move.ToPos];
            bool HasMovedBefore = movingPiece.HasMoved;

            if (CapturedPiece != null)
            {
                if (CapturedPiece.Color == Player.White)
                    CapturedWhitePieces.Add(CapturedPiece);
                else
                    CapturedBlackPieces.Add(CapturedPiece);
            }
            move.Execute(board);
            
            MoveRecord record=new MoveRecord(move, movingPiece, CapturedPiece,HasMovedBefore);
            undoStack.push(record);

            redoStack=new stack();

            Currentplayer = turnQueue.NextTurn();
            checkForGameOver();
        }
        public void UndoMove()
        {
            MoveRecord record=undoStack.pop();
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

            redoStack.push(record);
            Currentplayer= Currentplayer.Opponent();
        }
        public void RedoMove()
        {
            MoveRecord record = redoStack.pop();
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

            undoStack.push(record);

            Currentplayer = Currentplayer.Opponent();
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
            if(!AllLegalMovesFor(Currentplayer).Any())
            {
                if(board.IsInCheck(Currentplayer))
                {
                    Result = Result.Win(Currentplayer.Opponent());
                }
                else
                {
                    Result=Result.Draw(EndReason.stalemate);
                }
            }
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
            undoStack.push(new MoveRecord(move, movingPiece, capturedPiece, hasMovedBefore));
        }

        // Minimal UndoMove for search
        public void UndoMoveForSearch()
        {
            MoveRecord record = undoStack.pop();
            if (record == null) return;

            board[record.Move.FromPos] = record.MovingPiece;
            board[record.Move.ToPos] = record.CapturedPiece;
            record.MovingPiece.HasMoved = record.HasMovedBefore;
        }


    }
}
