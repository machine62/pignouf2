using pignouf2.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pignouf2.Protocol
{
    internal class DivideManager
    {
        private ChessBoard _ChessBoard;

        public DivideManager(ChessBoard board)
        {
            _ChessBoard = board.Clone();
        }

        public string Divide(int depth)
        {
            // divide, on lite les coups possible, et pour chaque coup^on perft
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (depth == 0)
                Console.WriteLine("impossible");
               
            // tous les coup possible sur le noeud
            MoveGenerator MoveGen = new MoveGenerator(ref _ChessBoard);
            List<Move> moves = MoveGen.generateALLMoves();
            long TotalNode = 0;
            foreach (var move in moves)
            {
                _ChessBoard.MakeMove(move);
                PerftManager P = new PerftManager(_ChessBoard);
                long nodes = P.Perft2(depth - 1);
                string line  = $"{move.UciEncode()} {nodes}";
                Console.WriteLine(line);
                TotalNode += nodes;
                _ChessBoard.UnMakeMove();
            }

                stopwatch.Stop();
            Console.WriteLine($"Moves ({moves.Count()})");
            long elapsed = stopwatch.ElapsedMilliseconds;
            return $"Divide({depth}): {TotalNode} nodes, Time: {elapsed} ms";
        }

    }
}
