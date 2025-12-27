using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;
using Chess.Pieces;

namespace Chess.ChessAI
{
    public static class MoveGeneration
    {
        public static IEnumerable<Move>GenerateAllMoves(Board board,Player player)
        {
            List<Move>allMoves = new List<Move>();
            foreach(Position pos in board.PiecePositionFor(player.Opponent()))
            {
                Piece piece = board[pos];
                foreach(Move move in piece.GetMoves(pos, board))
                {
                    if(move.IsLegal(board)) allMoves.Add(move);
                }
            }
            return allMoves;
        }
    }
}
