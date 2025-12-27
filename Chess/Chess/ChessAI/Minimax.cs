using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;
using Chess.DataStructures;

namespace Chess.ChessAI
{
    public static class Minimax
    {
        public static Tree BuildGameTree(Board board, Player aiPlayer, int depth, bool isMaximizingPlayer)
        {
            Tree node = new Tree(board);

            if (depth == 0 || board.IsInCheck(Player.White) || board.IsInCheck(Player.Black))
            {
                node.Value = Evaluation.Evaluate(board, aiPlayer);
                return node;
            }

            IEnumerable<Move> moves=isMaximizingPlayer?MoveGeneration.GenerateAllMoves(board,aiPlayer):MoveGeneration.GenerateAllMoves(board,aiPlayer.Opponent());
            foreach (Move move in moves) 
            {
                Board boardCopy = board.Copy();
                move.Execute(boardCopy);
                Tree child=BuildGameTree(boardCopy,aiPlayer,depth-1,!isMaximizingPlayer);
                child.Move=move;
                node.Children.Add(child);
            }
            node.Value = isMaximizingPlayer ? node.Children.Max(c => c.Value) : node.Children.Min(c => c.Value);
            return node;
        }
        public static Move FindBestMove(Tree root, Player aiPlayer)
        {
            if (root.Children.Count == 0) return null;

            return root.Children
                .OrderByDescending(c => c.Value)
                .First().Move;
        }
    }
}
