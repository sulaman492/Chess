using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.Pieces
{
    class Rook:Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override int Score { get; }
        public override Player Color { get; }
        private Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
        };
        public Rook(Player color)
        {
            Score = 50;
            Color = color;
        }
        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        public override IEnumerable<Move>GetMoves(Position from, Board board)
        {
            return MovePositionInDirs(from, board,dirs).Select(to=>new NormalMove(from,to));
        }
    }
}
