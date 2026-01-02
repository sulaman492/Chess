using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Result
    {
        public EndReason EndReason { get; set; }
        public Player Winner { get; set; }
        public Result(EndReason endReason,Player winner) 
        {
            EndReason = endReason;
            Winner = winner;
        }
        public static Result Win(Player winner) 
        {
            return new Result(EndReason.checkmate, winner);
        }
        public static Result Draw(EndReason reason)
        {
            return new Result(reason,Player.None);
        }
    }
}
