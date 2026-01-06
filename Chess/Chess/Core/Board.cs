using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Chess.Pieces;
using Chess.Core;

namespace Chess.Core
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
    public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }
        public Piece this[Position pos]
        {
            get { return pieces[pos.Row,pos.Column]; }
            set { pieces[pos.Row,pos.Column] = value; }
        }
        public static Board Initial()
        {
            Board board = new Board();
            board.setAllPieces();
            return board;
        }
        public void setAllPieces()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    this[r, c] = null;
                }
            }
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0,2]=new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7,0]=new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for(int c = 0; c < 8; c++)
            {
                this[1,c]=new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.White);
            }

        }
        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }
        public static bool IsInside(Position pos)
        {
            return pos.Row>=0 && pos.Row<8 && pos.Column>=0 && pos.Column<8;
        }
        public IEnumerable<Position> PiecePosition()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Position pos=new Position(i,j);
                    if(!IsEmpty(pos)) yield return pos;
                }
            }
        }
        public IEnumerable<Position>PiecePositionFor(Player player)
        {
            return PiecePosition().Where(pos => this[pos].Color!=player);   
        }
        public bool IsInCheck(Player player)
        {
            return PiecePositionFor(player).Any(pos => {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos,this);
                }
            );
        }
        public Board Copy()
        {
            Board copy = new Board();
            foreach (Position pos in PiecePosition())
            {
                copy[pos] = this[pos].Copy();
            }
            return copy;
        }

        public string ToFen()
        {
            StringBuilder fen = new StringBuilder();

            for (int r = 0; r < 8; r++)
            {
                int emptyCount = 0;

                for (int c = 0; c < 8; c++)
                {
                    Piece piece = this[r, c];

                    if (piece == null)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            fen.Append(emptyCount);
                            emptyCount = 0;
                        }

                        char p = piece.Type switch
                        {
                            PieceType.Pawn => 'p',
                            PieceType.Rook => 'r',
                            PieceType.Knight => 'n',
                            PieceType.Bishop => 'b',
                            PieceType.Queen => 'q',
                            PieceType.King => 'k',
                            _ => '?'
                        };

                        fen.Append(piece.Color == Player.White
                            ? char.ToUpper(p)
                            : p);
                    }
                }

                if (emptyCount > 0)
                    fen.Append(emptyCount);

                if (r != 7)
                    fen.Append('/');
            }

            return fen.ToString();
        }
        public Position FindKing(Player player)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = this[r, c];
                    if (piece != null && piece.Type == PieceType.King && piece.Color == player)
                        return new Position(r, c);
                }
            }
            return null;
        }


    }
}
