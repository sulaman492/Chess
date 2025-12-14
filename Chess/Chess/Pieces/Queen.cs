using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Queen:Piece
    {
        public override PieceType Type=>PieceType.Queen;
        public override Player Color { get; }
        private Direction[] dirs = new Direction[]
        {
            Direction.South,
            Direction.North,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast,
            Direction.NorthWest,
        };
        public Queen(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Queen copy=new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetMoves(Position from,Board board)
        {
            return MovePositionInDirs(from, board,dirs).Select(to=>new NormalMove(from,to));
        }
    }
}
