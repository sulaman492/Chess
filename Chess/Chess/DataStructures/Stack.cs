using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Moves;

namespace Chess.DataStructures
{
    public class stack
    {
        private Node head;

        public void push(MoveRecord moveRecord)
        {
            Node newNode = new Node(moveRecord);
            newNode.Next = head;
            head = newNode;
        }

        public MoveRecord pop()
        {
            if (head == null) return null;
            MoveRecord data = head.Data;
            head = head.Next;
            return data;
        }

        public MoveRecord top()
        {
            return head?.Data;
        }
    }

}
