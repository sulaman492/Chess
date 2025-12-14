using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.Pieces
{
    class King:Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }
        private Direction[] dirs=new Direction[]
        {
            Direction.East, Direction.North, Direction.South, Direction.West,
            Direction.NorthEast, Direction.SouthEast, Direction.NorthWest,Direction.SouthWest,
        };
        public King(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        private IEnumerable<Position>MovePosition(Position from,Board board)
        {
            foreach (Direction dir in dirs) 
            {
                Position to = from + dir;
                if (!Board.IsInside(to)) continue;

                if(board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePosition(from, board))
            {
                yield return new NormalMove(from,to);
            }
        }
    }
}
