using pignouf2.core;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.test
{
    internal class ChessboardTest
    {
        public static bool CompareBoardState(ChessBoardState state1, ChessBoardState state2)
        {
            if (state1.WhiteGR != state2.WhiteGR ||
                state1.WhitePR != state2.WhitePR ||
                state1.BlackGR != state2.BlackGR ||
                state1.BlackPR != state2.BlackPR ||
                state1.Trait != state2.Trait ||
                state1.PeP != state2.PeP ||
                state1.DemiCoup != state2.DemiCoup ||
                state1.Coup != state2.Coup)
            {
                return false;
            }
            return true;
        }

        public static bool ComparePieceBitboards(ChessBoard board1, ChessBoard board2)
        {
            return board1.GetWKing() == board2.GetWKing() &&
                   board1.GetWQueen() == board2.GetWQueen() &&
                   board1.GetWRook() == board2.GetWRook() &&
                   board1.GetWKnight() == board2.GetWKnight() &&
                   board1.GetWBisho() == board2.GetWBisho() &&
                   board1.GetWPawn() == board2.GetWPawn() &&
                   board1.GetBKing() == board2.GetBKing() &&
                   board1.GetBQueen() == board2.GetBQueen() &&
                   board1.GetBRook() == board2.GetBRook() &&
                   board1.GetBKnight() == board2.GetBKnight() &&
                   board1.GetBBishop() == board2.GetBBishop() &&
                   board1.GetBPawn() == board2.GetBPawn();
        }

        public static void CompareChessBoard(ChessBoard initialBoard, ChessBoard unmakeBoard, ChessBoard intermediateBoard)
        {
            bool statesMatch = CompareBoardState(initialBoard.GetChessBoardState(), unmakeBoard.GetChessBoardState());
            bool piecesMatch = ComparePieceBitboards(initialBoard, unmakeBoard);

            if (!statesMatch)
            {
                Console.WriteLine("erreur d etat:");
                Console.WriteLine("Initial :\n" + initialBoard.GetChessBoardState().ToString());
                Console.WriteLine("apres makemove :\n" + intermediateBoard.GetChessBoardState().ToString());
                Console.WriteLine("Apres UnMakeMove :\n" + unmakeBoard.GetChessBoardState().ToString());
            }

            if (!piecesMatch)
            {
                Console.WriteLine("erreur piece positions:");
                Console.WriteLine("Initial :\n");
                HumanView.chessboardToHumanView(initialBoard);
                Console.WriteLine("Apres mvt :\n");
                HumanView.chessboardToHumanView(intermediateBoard);
                Console.WriteLine("Apres UnMakeMove board:\n");
                HumanView.chessboardToHumanView(unmakeBoard);
            }

            if (statesMatch && piecesMatch)
            {
              // tout est bon
            }
        }

        public static void TestMakeAndUnmakeMove(ChessBoard board, Move move)
        {
            ChessBoard initialBoard = board.Clone();
            board.MakeMove(move);
            ChessBoard afterMakeBoard = board.Clone();
            ChessBoard intermediate = board.Clone();
            board.UnMakeMove();

            CompareChessBoard(initialBoard, board , intermediate);
        }
    }
}
