using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Core;

namespace Chess.DataStructures
{
    public class queue
    {
        private Player[] players=new Player[2];
        private int front = 0;
        private int rear = 0;
        public queue(Player p1,Player p2)
        {
            players[0]=p1;
            players[1]=p2;
            front = 2;
        }
        public void push(Player p)
        {
            players[front] = p;
            front++;
        }
        public void pop()
        {
            front--;
        }
        public void peek()
        {
             
        }
    }
}
