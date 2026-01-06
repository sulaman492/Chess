using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public enum EndReason
    {
        checkmate,
        stalemate,
        fiftyMoveRule,
        InsufficientMaterial,
        ThreeFoldRepetition,TimeOut
    }
}
