using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Pieces;

namespace Chess.ChessAI
{
    public static class Evaluation
    {
        public static int Evaluate(Board board,Player aiplayer)
        {
            int score = 0;
            foreach(Position pos in board.PiecePosition())
            {
                Piece piece = board[pos];
                if(piece.Color==aiplayer)score=score+ piece.Score;
                else score=score- piece.Score;
            }
            return score;
        }
    }
}
