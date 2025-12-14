using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Chess.Core;
using Chess.Pieces;

namespace Chess.UI
{
    public class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            {PieceType.Pawn,LoadImage("Assets/white-pawn.png") },
            {PieceType.Bishop,LoadImage("Assets/white-bishop.png") },
            {PieceType.Knight,LoadImage("Assets/white-knight.png") },
            {PieceType.Queen,LoadImage("Assets/white-queen.png") },
            {PieceType.King,LoadImage("Assets/white-king.png") },   
            {PieceType.Rook,LoadImage("Assets/white-rook.png") },
            //{PieceType.Bishop,LoadImage("Assets/white-bishop.png") },

        };
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Pawn,LoadImage("Assets/black-pawn.png") },
            {PieceType.Bishop,LoadImage("Assets/black-bishop.png") },
            {PieceType.Knight,LoadImage("Assets/black-knight.png") },
            {PieceType.Queen,LoadImage("Assets/black-queen.png") },
            {PieceType.King,LoadImage("Assets/black-king.png") },
            {PieceType.Rook,LoadImage("Assets/black-rook.png") },
            //{PieceType.Bishop,LoadImage("Assets/white-bishop.png") },

        };
        private static ImageSource LoadImage(string filePath)
        {
            return new System.Windows.Media.Imaging.BitmapImage(new Uri(filePath,UriKind.Relative));
        }
        public static ImageSource GetImage(Player color,PieceType type)
        {
            switch (color)
            {
                case (Player.White):
                    return whiteSources[type];
                case (Player.Black): return blackSources[type];
                default:
                    return null;
            }
        }
        public static ImageSource GetImage(Piece piece)
        {
            if(piece== null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.Type);
        }
    }
}
