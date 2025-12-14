using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public enum Player
    {
        Black,
        White,
        None
    }
    public static class PlayerExtension
    {
        public static Player Opponent(this Player player)
        {
            switch (player)
            {
                case Player.Black:
                    return Player.White;
                case Player.White:
                    return Player.Black;
                default:
                    return Player.None;
            }
        }
    }
}
