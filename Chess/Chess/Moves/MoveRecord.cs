using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess.Moves
{
    public class MoveRecord
    {
        public Move Move { get; set; }
        public Piece MovingPiece {  get; set; }
        public Piece CapturedPiece { get; set; }
        public bool HasMovedBefore { get; set; }

        public MoveRecord(Move move, Piece movingPiece, Piece capturedPiece, bool hasMovedBefore)
        {
            Move = move;
            MovingPiece = movingPiece;
            CapturedPiece = capturedPiece;
            HasMovedBefore = hasMovedBefore;
        }
    }
}
