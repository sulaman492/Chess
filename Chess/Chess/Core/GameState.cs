using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class GameState
    {
        public Board board { get; }
        public Player player { get; }

        public GameState(Board board, Player player)
        {
            this.board = board;
            this.player = player;
        }
    }
}
