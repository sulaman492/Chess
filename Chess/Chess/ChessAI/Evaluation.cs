using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Core;
using Chess.Moves;
using Chess.Pieces;

namespace Chess.ChessAI
{
    public static class Evaluation
    {
        public static int Evaluate(Board board, Player aiPlayer)
        {
            int score = 0;

            foreach (Position pos in board.PiecePosition())
            {
                Piece piece = board[pos];

                if (piece == null) continue;

                int pieceScore = piece.Score;

                // ---------- MOBILITY BONUS ----------
                int mobility = piece.GetMoves(pos, board).Count();
                pieceScore += mobility * 1; // slightly lower weight

                // ---------- CENTRAL CONTROL BONUS ----------
                if ((pos.Row >= 2 && pos.Row <= 5) && (pos.Column >= 2 && pos.Column <= 5))
                {
                    if (piece.Type == PieceType.Knight || piece.Type == PieceType.Bishop)
                        pieceScore += 2; // smaller bonus
                }

                if (piece.Color == aiPlayer)
                    score += pieceScore;
                else
                    score -= pieceScore;
            }

            return score;
        }

    }
}
