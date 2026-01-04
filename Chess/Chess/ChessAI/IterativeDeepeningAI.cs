using System;
using System.Diagnostics;
using System.Linq;
using Chess.Core;
using Chess.DataStructures;
using Chess.Moves;

namespace Chess.ChessAI
{
    public static class IterativeDeepeningAI
    {
        // maxDepth = maximum depth to search
        // timeLimitMs = optional, stop if AI runs out of time
        public static Move FindBestMove(Board board, Player aiPlayer, int maxDepth, int timeLimitMs = 0)
        {
            Move bestMove = null;
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int depth = 1; depth <= maxDepth; depth++)
            {
                Tree root = Minimax.BuildGameTree(board, aiPlayer, depth, true);
                Move moveAtThisDepth = Minimax.FindBestMove(root, aiPlayer);

                if (moveAtThisDepth != null)
                    bestMove = moveAtThisDepth;

                // Stop if time limit reached
                if (timeLimitMs > 0 && stopwatch.ElapsedMilliseconds >= timeLimitMs)
                    break;
            }

            return bestMove;
        }
    }
}
