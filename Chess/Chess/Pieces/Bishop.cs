using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthEast,
            Direction.NorthWest, Direction.SouthEast, Direction.SouthWest,
        };
        public override PieceType Type => PieceType.Bishop;
        public override Player Color { get; }
        public override int Score{get;}
        public Bishop(Player color)
        {
            Score = 30;
            Color = color;
        }
        public override Piece Copy()
        {
            Bishop copy = new Bishop(Color);
            copy.HasMoved = HasMoved;
            return copy; 
        }
        public override IEnumerable<Move>GetMoves(Position from,Board board)
        {
            List<Move>moves = new List<Move>();
            return MovePositionInDirs(from, board,dirs).Select(to=>new NormalMove(from,to));
        }
    }
}