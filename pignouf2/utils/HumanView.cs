using pignouf2.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static pignouf2.core.ChessEnum;

namespace pignouf2.utils
{
    internal class HumanView
    {

        public static void chessboardToHumanView(ChessBoard CB)
        {
            // Afficher les labels des colonnes
            Console.Write("  ");
            for (char col = 'a'; col <= 'h'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();

            for (int row = 7; row >= 0; row--)
            {
                // Afficher le label de la ligne
                Console.Write((row + 1) + " ");

                for (int col = 0; col < 8; col++)
                {
                    int index = row * 8 + col;
                    ulong masque = 1UL << index;
                    char piece = ObtenirCaracterePiece(CB,masque);

                    Console.Write(piece + " ");
                }
                // Afficher le label de la ligne à la fin de la ligne
                Console.WriteLine((row + 1));
            }

            // Afficher les labels des colonnes à nouveau au bas de l'échiquier
            Console.Write("  ");
            for (char col = 'a'; col <= 'h'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();
        }

        private static char ObtenirCaracterePiece(ChessBoard CB, ulong masque)
        {
            if ((CB.GetWPieces(Piece.PAWN) & masque) != 0) return 'P';
            if ((CB.GetWPieces(Piece.ROOK) & masque) != 0) return 'R';
            if ((CB.GetWPieces(Piece.KNIGHT) & masque) != 0) return 'N';
            if ((CB.GetWPieces(Piece.BISHOP) & masque) != 0) return 'B';
            if ((CB.GetWPieces(Piece.QUEEN) & masque) != 0) return 'Q';
            if ((CB.GetWPieces(Piece.KING) & masque) != 0) return 'K';

            if ((CB.GetBPieces(Piece.PAWN) & masque) != 0) return 'p';
            if ((CB.GetBPieces(Piece.ROOK) & masque) != 0) return 'r';
            if ((CB.GetBPieces(Piece.KNIGHT) & masque) != 0) return 'n';
            if ((CB.GetBPieces(Piece.BISHOP) & masque) != 0) return 'b';
            if ((CB.GetBPieces(Piece.QUEEN) & masque) != 0) return 'q';
            if ((CB.GetBPieces(Piece.KING) & masque) != 0) return 'k';

            return '.';
        }



        public static void UlongToHumanView(ulong echiquier)
        {
            Console.Write("  ");
            for (char col = 'a'; col <= 'h'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();

            for (int row = 7; row >= 0; row--)
            {
                Console.Write((row + 1) + " ");

                for (int col = 0; col < 8; col++)
                {
                    int index = row * 8 + col;
                    ulong masque = 1UL << index;
                    bool occupe = (echiquier & masque) != 0;
                    Console.Write(occupe ? "1 " : "0 ");
                }
                
                Console.WriteLine((row + 1));
            }


            Console.Write("  ");
            for (char col = 'a'; col <= 'h'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();
        }
    }
}
