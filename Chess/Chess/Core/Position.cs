using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Position
    {
        public int Row;
        public int Column;
        public Position(int row,int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }
        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0) return Player.White;
            return Player.Black;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
        public static bool operator ==(Position? left, Position? right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position? left, Position? right)
        {
            return !(left == right);
        }
        public static Position operator+(Position pos,Direction dir)
        {
            return new Position(pos.Row + dir.rowDelta, pos.Column + dir.columnDelta); 
        }
    }
}
