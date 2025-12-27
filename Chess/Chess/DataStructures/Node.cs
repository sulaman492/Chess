using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Moves;

namespace Chess.DataStructures
{
    public class Node
    {
        public MoveRecord Data;
        public Node Next;

        public Node(MoveRecord data)
        {
            Data = data;
            Next = null;
        }
    }
}
