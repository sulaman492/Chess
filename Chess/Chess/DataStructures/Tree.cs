using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Moves;

namespace Chess.DataStructures
{
    public class Tree
    {
        public Board BoardState { get; set; }
        public Move Move { get; set; }

        public int Value {  get; set; }
        public List<Tree> Children { get; set; }=new List<Tree>();
        public Tree(Board board, Move move = null)
        {
            BoardState = board;
            Move = move;
        }

    }
}
