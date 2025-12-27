using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Moves;
using Chess.Pieces;

namespace Chess.Core
{
    public class GameState
    {
        public Board board { get; }
        public Player Currentplayer { get; set; }

        public GameState(Board board, Player player)
        {
            this.board = board;
            this.Currentplayer = player;
        }

        public IEnumerable<Move> LegalMoveForPieces(Position pos) 
        {
            if(board.IsEmpty(pos) || board[pos].Color != Currentplayer) 
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = board[pos];
            IEnumerable<Move>moveCandidate= piece.GetMoves(pos,board);
            return moveCandidate.Where(move => move.IsLegal(board));
        }
        public void MakeMove(Move move)
        {
            move.Execute(board);
            Currentplayer=Currentplayer.Opponent();
        }

    }
}
