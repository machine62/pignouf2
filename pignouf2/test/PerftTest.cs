using pignouf2.Protocol;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.test
{
    public class PerftTest
    {
        static int _fail = 0;
        static int _success = 0;

        static List<PerftTestStruct> perftTestStructs = new List<PerftTestStruct>();

        public static void ALLTest()
        {
            perftTestStructs = new List<PerftTestStruct>();
            Console.WriteLine("PerftTest");
            TEST();
            Console.WriteLine($"Resultat : {_success} SUCCESS / {_fail} FAIL");
        }

        private static void TEST()
        {
            // les 6 positions de reference chessprogramming.org (perft results)
            // startpos
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", nodes = 20, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", nodes = 400, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", nodes = 8902, deep = 3 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", nodes = 197281, deep = 4 });
            // kiwipete (roques + echecs)
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1", nodes = 48, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1", nodes = 2039, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1", nodes = 97862, deep = 3 });
            // position 3 (ep + epingles)
            perftTestStructs.Add(new PerftTestStruct() { fen = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1", nodes = 14, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1", nodes = 191, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1", nodes = 2812, deep = 3 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1", nodes = 43238, deep = 4 });
            // position 4 (promotions massives)
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", nodes = 6, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", nodes = 264, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", nodes = 9467, deep = 3 });
            // position 5 (ep + roque combines)
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8", nodes = 44, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8", nodes = 1486, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8", nodes = 62379, deep = 3 });
            // position 6 (mediane)
            perftTestStructs.Add(new PerftTestStruct() { fen = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10", nodes = 46, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10", nodes = 2079, deep = 2 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10", nodes = 89890, deep = 3 });

            int nb = 1;
            foreach (var perftTest in perftTestStructs)
            {
                Pignouf pif = new Pignouf();
                FenParser FenP = new utils.FenParser(perftTest.fen);
                pif.setFEN(FenP);

                PerftManager P = new PerftManager(pif.getChessBoard());
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                long NodesResult = P.Perft2(perftTest.deep);
                stopwatch.Stop();
                long elapsed = stopwatch.ElapsedMilliseconds;
                long speed = 0;
                if (elapsed != 0)
                {
                    speed = NodesResult * 1000 / elapsed;
                }
                if (NodesResult == perftTest.nodes)
                {
                    Console.WriteLine($"Test {nb}/{perftTestStructs.Count} - SUCCESS : attendu {perftTest.nodes} / resultat {NodesResult}  - Time: {elapsed} ms - vitesse  ({speed} n/s)");
                    _success++;
                }
                else
                {
                    Console.WriteLine($"Test {nb}/{perftTestStructs.Count} - FAIL : attentu {perftTest.nodes} / resultat {NodesResult}  - Time: {elapsed} ms - vitesse ({speed} n/s)");
                    _fail++;
                }
                nb++;
            }
        }

        public struct PerftTestStruct()
        {
            public string fen;
            public long nodes;
            public int deep;
        }
    }
}
