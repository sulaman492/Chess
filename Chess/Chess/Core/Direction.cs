using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Direction
    {
        public readonly static Direction North = new Direction(-1, 0);
        public readonly static Direction South = new Direction(1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;
        public int rowDelta { get; }
        public int columnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            this.rowDelta = rowDelta;
            this.columnDelta = columnDelta;
        }

        public static Direction operator +(Direction a, Direction b) { 
            return new Direction(a.rowDelta+b.rowDelta,a.columnDelta+b.columnDelta);
        }
        public static Direction operator*(int scalar,Direction dir)
        {
            return new Direction (scalar*dir.rowDelta,scalar*dir.columnDelta);
        }
    }
}
