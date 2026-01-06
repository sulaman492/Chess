using System;

namespace Chess.Core
{
    public static class Zobrist
    {
        // [color, pieceType, square]
        public static ulong[,,] pieceRandom = new ulong[2, 6, 64];
        public static ulong turnRandom;

        static Zobrist()
        {
            Random rand = new Random();
            for (int color = 0; color < 2; color++)
            {
                for (int type = 0; type < 6; type++)
                {
                    for (int square = 0; square < 64; square++)
                    {
                        pieceRandom[color, type, square] = RandomUlong(rand);
                    }
                }
            }

            turnRandom = RandomUlong(rand);
        }

        private static ulong RandomUlong(Random rand)
        {
            byte[] buffer = new byte[8];
            rand.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
