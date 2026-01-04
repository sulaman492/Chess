using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Core;
using Chess.Moves;
using Chess.DataStructures;
using Chess.Pieces;

namespace Chess.ChessAI
{
    public static class Minimax
    {
        public static Tree BuildGameTree(Board board, Player aiPlayer, int depth, bool isMaximizingPlayer, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            Tree node = new Tree(board);

            // Leaf node or check condition
            if (depth == 0 || GameState.IsTerminal(board, Player.White) || GameState.IsTerminal(board, Player.Black))
            {
                node.Value = Evaluation.Evaluate(board, aiPlayer);
                return node;
            }

            Player currentPlayer = isMaximizingPlayer ? aiPlayer : aiPlayer.Opponent();
            IEnumerable<Move> moves = MoveGeneration.GenerateAllMoves(board, currentPlayer);

            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (Move move in moves)
                {
                    Board boardCopy = board.Copy();
                    move.Execute(boardCopy);
                    Tree child = BuildGameTree(boardCopy, aiPlayer, depth - 1, false, alpha, beta);
                    child.Move = move;
                    node.Children.Add(child);

                    maxEval = Math.Max(maxEval, child.Value);
                    alpha = Math.Max(alpha, maxEval);

                    // Alpha-beta pruning
                    if (beta <= alpha)
                        break;
                }
                node.Value = maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (Move move in moves)
                {
                    Board boardCopy = board.Copy();
                    move.Execute(boardCopy);
                    Tree child = BuildGameTree(boardCopy, aiPlayer, depth - 1, true, alpha, beta);
                    child.Move = move;
                    node.Children.Add(child);

                    minEval = Math.Min(minEval, child.Value);
                    beta = Math.Min(beta, minEval);

                    // Alpha-beta pruning
                    if (beta <= alpha)
                        break;
                }
                node.Value = minEval;
            }

            return node;
        }

        public static Move FindBestMove(Tree root,Player aiPlayer)
        {
            if (root.Children.Count == 0) return null;

            return root.Children
                .OrderByDescending(c => c.Value)
                .First().Move;
        }
        private static int ScoreMove(Board board,Move move)
        {
            int score = 0;
            if (!board.IsEmpty(move.ToPos))
            {
                Piece CapturedPiece = board[move.ToPos];
                score = 10 + CapturedPiece.Score;
            }
            Board temp=board.Copy();
            move.Execute(temp);
            if (temp.IsInCheck(board[move.FromPos].Color.Opponent()))
            {
                score += 50;
            }

            return score;
        }
    }
}
