using Chess.Core;

namespace Chess.DataStructures
{
    public class Queue
    {
        private readonly Player[] players = new Player[2];
        private int front = 0;

        public Queue(Player p1, Player p2)
        {
            players[0] = p1;
            players[1] = p2;
        }

        // Rotate the turn and return current player
        public Player NextTurn()
        {
            Player current = players[front];
            front = (front + 1) % 2;
            return current;
        }

        public Player Peek()
        {
            return players[front];
        }
    }
}
