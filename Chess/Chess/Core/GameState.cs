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

        private stack undoStack=new stack();
        private stack redoStack=new stack();

        public GameState(Board board, Player player)
        {
            this.board = board;
            this.Currentplayer = player;
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


            move.Execute(board);
            
            MoveRecord record=new MoveRecord(move, movingPiece, CapturedPiece,HasMovedBefore);
            undoStack.push(record);

            redoStack=new stack();

            Currentplayer=Currentplayer.Opponent();
        }
        public void UndoMove()
        {
            MoveRecord record=undoStack.pop();
            if (record == null) { return ; }
            board[record.Move.FromPos] = record.MovingPiece;
            board[record.Move.ToPos] = record.CapturedPiece;
            record.MovingPiece.HasMoved=record.HasMovedBefore;

            redoStack.push(record);
            Currentplayer= Currentplayer.Opponent();
        }
        public void RedoMove()
        {
            MoveRecord record = redoStack.pop();
            if (record == null) return;

            record.Move.Execute(board);
            record.MovingPiece.HasMoved = true;

            undoStack.push(record);

            Currentplayer = Currentplayer.Opponent();
        }

    }
}
