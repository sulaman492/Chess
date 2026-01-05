using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Pieces;

namespace Chess.Moves
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos {  get; }
        public abstract Position ToPos { get; }
        public abstract void Execute(Board board);
        
        public virtual bool IsLegal(Board board)
        {
            Piece movingPiece = board[FromPos];
            Piece CapturedPiece = board[ToPos];
            bool hasMovedBefore = movingPiece.HasMoved;

            Player player = movingPiece.Color;

            Execute(board);
            bool legal = !board.IsInCheck(player);

            board[FromPos] = movingPiece;
            board[ToPos] = CapturedPiece;
            movingPiece.HasMoved = hasMovedBefore;

            return legal;


        }
    }
}
