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
        public Piece MovingPiece { get; set; }
        public Piece CapturedPiece { get; set; }
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

        private string PosToString(Position pos)
        {
            char file = (char)('a' + pos.Column); // Column 0 → 'a', 1 → 'b'
            int rank = 8 - pos.Row;                // Row 0 → 8, Row 7 → 1
            return $"{file}{rank}";
        }

        // Simple move string: e2e4
        public string ToSimpleString()
        {
            return PosToString(FromPos) + PosToString(ToPos);
        }

        // Optional: Algebraic notation: e.g., Nf3, exd5
        public string ToAlgebraic()
        {
            string pieceChar = MovingPiece.Type switch
            {
                PieceType.Pawn => "",
                PieceType.Knight => "N",
                PieceType.Bishop => "B",
                PieceType.Rook => "R",
                PieceType.Queen => "Q",
                PieceType.King => "K",
                _ => ""
            };

            string capture = CapturedPiece != null ? "x" : "";
            return $"{pieceChar}{PosToString(ToPos)}";
        }
    }
}
