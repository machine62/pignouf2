using pignouf2.Protocol;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static pignouf2.test.PerftTest;

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



        }






        private static void TEST()
        {

            Console.WriteLine("KingTest");
            perftTestStructs = new List<PerftTestStruct>();

            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 3, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 9, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 54, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 324, deep = 4 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 1890, deep = 5 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 11024, deep = 6 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/8/8/8/8/K7 w - - 0 1", nodes = 71758, deep = 7 });

            //  Console.WriteLine("KnightTest");
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/nn6/kn6 b - - 0 1", nodes = 10, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/nn6/kn6 b - - 0 1", nodes = 100, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/nn6/kn6 b - - 0 1", nodes = 1320, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/nn6/kn6 b - - 0 1", nodes = 17424, deep = 4 });

            // king + knghit - pb trait / switch perft2

            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/8/kn6 b - - 0 1", nodes = 5, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/8/kn6 b - - 0 1", nodes = 50, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/8/kn6 b - - 0 1", nodes = 440, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6NK/6NN/8/8/8/8/8/kn6 b - - 0 1", nodes = 5808, deep = 4 });

            //perftTestStructs.Add(new PerftTestStruct() { fen = "6N1/6K1/8/8/8/8/8/k7 w - - 0 1", nodes = 10, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6N1/6K1/8/8/8/8/8/k7 w - - 0 1", nodes = 30, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6N1/6K1/8/8/8/8/8/k7 w - - 0 1", nodes = 291, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6N1/6K1/8/8/8/8/8/k7 w - - 0 1", nodes = 1743, deep = 4 });


            /// bishop'B7/8/8/8/8/8/8/8 w - - 0 1'
            //perftTestStructs.Add(new PerftTestStruct() { fen = "B7/8/8/8/8/8/8/b7 b - - 0 1", nodes = 7, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6bk/6bb/8/4n3/8/2N5/BB6/KB6 w - - 0 1", nodes = 20, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "6bk/6bb/8/4n3/8/2N5/BB6/KB6 w - - 0 1", nodes = 416, deep = 2 });

            //perftTestStructs.Add(new PerftTestStruct() { fen = "7b/6k1/8/8/K7/8/8/8 b - - 0 1", nodes = 35, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7b/6k1/8/8/K7/8/8/8 b - - 0 1", nodes = 425, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7b/6k1/8/8/K7/8/8/8 b - - 0 1", nodes = 2546, deep = 4 });


            //rookr7/8/8/8/8/8/8/8 w - - 0 1
            // perftTestStructs.Add(new PerftTestStruct() { fen = "4nr2/5k2/8/8/8/K7/8/8 b - - 0 1", nodes = 12, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4nr2/5k2/8/8/8/K7/8/8 b - - 0 1", nodes = 60, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4nr2/5k2/8/8/8/K7/8/8 b - - 0 1", nodes = 1135, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4nr2/5k2/8/8/8/K7/8/8 w - - 0 1", nodes = 7589, deep = 4 });

            //queen 3k4/8/3q4/8/8/8/8/8 w - - 0 1
            //perftTestStructs.Add(new PerftTestStruct() { fen = "3k4/8/3q4/8/8/8/8/8 b - - 0 1", nodes = 23, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "q6k/8/8/8/8/NNNN4/NNNN4/NKNN4 w - - 0 1", nodes = 23, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "q6k/8/8/8/8/NNNN4/NNNN4/NKNN4 w - - 0 1", nodes = 474, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "q6k/8/8/8/8/NNNN4/NNNN4/NKNN4 w - - 0 1", nodes = 11558, deep = 3 });

            //pions 7k/7p/8/8/8/P7/8/K7 w - - 0 1 => push / dbl push
            //            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/7p/8/8/8/P7/8/K7 w - - 0 1", nodes = 4, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/7p/8/8/8/P7/8/K7 w - - 0 1", nodes = 16, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/7p/8/8/8/P7/8/K7 w - - 0 1", nodes = 92, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "7k/7p/8/8/8/P7/8/K7 w - - 0 1", nodes = 529, deep = 4 });

            //pions 7k/7p/8/8/8/P7/8/K7 w - - 0 1 => push / dbl push / prise / prise en passant 
            //4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 18, deep = 1 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 324, deep = 2 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 5658, deep = 3 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 98766, deep = 4 });
            //perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 1683599, deep = 5 });
            //  perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 28677559, deep = 6 });
            //    perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/pppppppp/8/8/8/8/PPPPPPPP/3K4 w - - 0 1", nodes = 479771205, deep = 7 });

            // pion prise et push
            // "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1"
            perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/2p5/8/8/3P4/K7 w - - 0 1", nodes = 5, deep = 1 });
            perftTestStructs.Add(new PerftTestStruct() { fen = "7k/8/8/2p5/3P4/8/8/K7 b - d3 0 1", nodes = 5, deep = 1 });
           
            
            
            
             perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1", nodes = 10, deep = 1 });
             perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1", nodes = 100, deep = 2 });
            // perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1", nodes = 985, deep = 3 });
            // perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1", nodes = 10061, deep = 4 });
            // perftTestStructs.Add(new PerftTestStruct() { fen = "4k3/1p6/1p6/1p5p/P1P3P1/6P1/8/3K4 b - - 0 1", nodes = 98480, deep = 5 });


            // speed : Test 1/11 - SUCCESS : attendu 4 / resultat 4  - Time: 62 ms
            //Test 2 / 11 - SUCCESS : attendu 16 / resultat 16 - Time: 45 ms
            //Test 3 / 11 - SUCCESS : attendu 92 / resultat 92 - Time: 45 ms
            //Test 4 / 11 - SUCCESS : attendu 529 / resultat 529 - Time: 47 ms
            //Test 5 / 11 - SUCCESS : attendu 18 / resultat 18 - Time: 46 ms
            //Test 6 / 11 - SUCCESS : attendu 324 / resultat 324 - Time: 46 ms
            //Test 7 / 11 - FAIL : attentu 5658 / resultat 5652 - Time: 74 ms
            //Test 8 / 11 - FAIL : attentu 98766 / resultat 98596 - Time: 283 ms
            //Test 9 / 11 - FAIL : attentu 1683599 / resultat 1674844 - Time: 2643 ms
            //Test 10 / 11 - FAIL : attentu 28677559 / resultat 28448404 - Time: 46466 ms



            int nb = 1;
      
            foreach (var perftTest in perftTestStructs)
            {

                Pignouf pif = new Pignouf();
                FenParser FenP = new utils.FenParser(perftTest.fen);

                pif.setFEN(FenP);
               
                PerftManager P = new PerftManager(pif.getChessBoard());
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
              long NodesResult =   P.Perft2(perftTest.deep);

                stopwatch.Stop();
                long elapsed = stopwatch.ElapsedMilliseconds ;
                long speed = 0;
                if (elapsed != 0)
                {
                    speed = NodesResult * 1000 / elapsed;
                }
                if (NodesResult==perftTest.nodes)
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