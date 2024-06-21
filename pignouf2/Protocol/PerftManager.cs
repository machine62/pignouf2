using pignouf2.core;
using pignouf2.test;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pignouf2.Protocol
{
    internal class PerftManager
    {
        private ChessBoard _ChessBoard;

        public PerftManager(ChessBoard board)
        {
            _ChessBoard = board.Clone();
        }

        public string Perft(int depth)
        {
            long nodes = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            nodes = Perft2(depth);

            stopwatch.Stop();
            long elapsed = stopwatch.ElapsedMilliseconds;
            return $"Perft({depth}): {nodes} nodes, Time: {elapsed} ms";
        }

        public long Perft2(int depth)
        {
            long nodes = 0;
            //logique 
            if (depth == 0)
                nodes = 1;

            else
            {
                MoveGenerator MoveGen = new MoveGenerator(ref _ChessBoard);
                List<Move> moves = MoveGen.generateALLMoves();
                // on doit appeler une fn rescursive

                foreach (var move in moves)
                {
                    // vérification de makeunmake :
                    ChessBoard chessboardToTestmoveunmove = _ChessBoard.Clone();
                    ChessboardTest.TestMakeAndUnmakeMove(chessboardToTestmoveunmove, move);
                    // fin vérification de makeunmake :

                    _ChessBoard.MakeMove(move);
                    nodes += Perft2(depth - 1);

                    _ChessBoard.UnMakeMove();

                }
            }
            return nodes;
        }


        //      public void Divide(int depth)
        //{
        // List<Move> moves = chessBoard.GenerateAllMoves(ChessEnum.Player.WHITE); // or the current player

        //  foreach (var move in moves)
        // {
        //chessBoard.MakeMove(move);
        //long nodes = Perft(depth - 1);
        //  chessBoard.UnMakeMove();
        //   Console.WriteLine($"{move}: {nodes}");
        //      }
        //}

    }
}
