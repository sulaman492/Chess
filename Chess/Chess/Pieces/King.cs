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
        private static bool IsUnMovedRook(Position pos,Board board)
        {
            if(board.IsEmpty(pos)) return false;
            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }
        private static bool ALlEmpty(IEnumerable<Position>positions,Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));
        }
        private bool CanCastleKingSide(Position from,Board board)
        {
            if (HasMoved) return false;
            Position rookPos = new Position(from.Row, 7);
            Position[] betweenPos = new Position[] { new(from.Row, 5), new(from.Row, 6) };
            return IsUnMovedRook(rookPos,board) && ALlEmpty(betweenPos, board); 
        }
        private bool CanCastleQueenSide(Position from,Board board)
        {
            if (HasMoved) return false;
            Position rookPos = new Position(from.Row, 0);
            Position[] betweenPos = new Position[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };
            return IsUnMovedRook(rookPos, board) && ALlEmpty(betweenPos, board);
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
            if(CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.castlingKS,from);
            }
            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.castlingQS, from);
            }
        }
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePosition(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
        
    }
}
