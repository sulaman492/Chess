using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.Pieces
{
    class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }
        public Knight(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        private IEnumerable<Position> PotentialToPosition(Position from)
        {
            foreach (Direction vdir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hdir in new Direction[] { Direction.East, Direction.West })
                {
                    yield return from + 2 * vdir + hdir;
                    yield return from + 2 * hdir + vdir;
                }
            }
        }
        // not from
        private IEnumerable<Position> MovePosition(Position from, Board board)
        {
            foreach(Position p in PotentialToPosition(from))
            {
                if(board.IsEmpty(p) && (board.IsEmpty(p) || board[p].Color != Color))
                {
                    yield return p;
                }
            }
        } 
        public override IEnumerable<Move>GetMoves(Position from,Board board)
        {
            return MovePosition(from, board).Select(to=>new NormalMove(from,to));
        }
    }
}
