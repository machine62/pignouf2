using pignouf2.core;
using pignouf2.Protocol;
using pignouf2.search;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace pignouf2.bench
{
    internal static class Bench
    {
        // profondeur fixe par defaut pour le banc de recherche (on montera apres alpha-beta)
        public const int DefaultSearchDepth = 4;
        // nombre de lancers par cas, on garde le meilleur nps (reproductibilite)
        private const int Repeats = 2;

        private static readonly (string name, string fen, int depth, long expected)[] PerftCases =
        {
            ("startpos",   "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 5, 4865609),
            ("kiwipete",   "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1", 4, 4085603),
            ("pos3-pep",   "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1", 5, 674624),
            ("pos4-promo", "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", 4, 422333),
            ("pos5",       "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8", 3, 62379),
            ("pos6",       "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10", 4, 3894594),
        };

        private static readonly (string name, string fen)[] SearchCases =
        {
            ("startpos",   "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"),
            ("kiwipete",   "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1"),
            ("pos4-promo", "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1"),
            ("pos6",       "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10"),
            ("mat1",       "k7/8/1K6/8/8/8/8/7R w - - 0 1"),
            ("tactique",   "r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 4 4"),
        };

        // lance toujours perft + search a la profondeur de recherche demandee, retourne le rapport markdown
        public static List<string> Run(int searchDepth)
        {
            var md = new List<string>();
            md.Add($"# bench pignouf2 — {DateTime.Now:yyyy-MM-dd HH:mm:ss} — {RuntimeInformation.FrameworkDescription} — {RuntimeInformation.ProcessArchitecture}");
            md.Add("");
            md.Add($"repeats={Repeats} (meilleur nps retenu) — search depth fixe={searchDepth}");
            md.Add("");

            md.Add("## perft");
            md.Add("");
            md.Add("| pos | depth | nodes | expected | ok | time_ms | nps |");
            md.Add("|---|---|---|---|---|---|---|");
            long totalNodes = 0, totalTime = 0;
            int okCount = 0;
            foreach (var c in PerftCases)
            {
                long nodes = 0, bestTime = long.MaxValue;
                for (int r = 0; r < Repeats; r++)
                {
                    var board = new ChessBoard();
                    board.setFEN(new FenParser(c.fen));
                    var pm = new PerftManager(board);
                    var sw = Stopwatch.StartNew();
                    long n = pm.Perft2(c.depth);
                    sw.Stop();
                    nodes = n;
                    if (sw.ElapsedMilliseconds < bestTime)
                        bestTime = sw.ElapsedMilliseconds;
                }
                if (bestTime == long.MaxValue) bestTime = 0;
                long nps = bestTime > 0 ? nodes * 1000 / bestTime : nodes * 1000;
                bool ok = nodes == c.expected;
                if (ok) okCount++;
                totalNodes += nodes;
                totalTime += bestTime;
                md.Add($"| {c.name} | {c.depth} | {nodes} | {c.expected} | {(ok ? "OK" : "FAIL")} | {bestTime} | {nps} |");
            }
            long tnps = totalTime > 0 ? totalNodes * 1000 / totalTime : 0;
            md.Add("");
            md.Add($"**perft TOTAL** : {totalNodes} nodes en {totalTime} ms → **{tnps} nps** — ok {okCount}/{PerftCases.Length}");
            md.Add("");

            md.Add($"## search (depth {searchDepth})");
            md.Add("");
            md.Add("| pos | nodes | time_ms | nps | bestmove | score | branch |");
            md.Add("|---|---|---|---|---|---|---|");
            totalNodes = 0; totalTime = 0;
            foreach (var c in SearchCases)
            {
                var board = new ChessBoard();
                board.setFEN(new FenParser(c.fen));
                long nodes = 0, bestTime = long.MaxValue;
                SearchResult res = null;
                for (int r = 0; r < Repeats; r++)
                {
                    var searcher = new Searcher(board.Clone(), new SearchOptions { MaxDepth = searchDepth }, null);
                    var sw = Stopwatch.StartNew();
                    res = searcher.Search();
                    sw.Stop();
                    nodes = res.Nodes;
                    if (sw.ElapsedMilliseconds < bestTime)
                        bestTime = sw.ElapsedMilliseconds;
                }
                if (bestTime == long.MaxValue) bestTime = 0;
                long nps = bestTime > 0 ? nodes * 1000 / bestTime : nodes * 1000;
                // facteur de branchement effectif : a comparer aux ~35 coups legaux moyens
                double branch = Math.Pow(nodes, 1.0 / searchDepth);
                string best = res != null && res.HasBest ? res.BestMove.UciEncode() : "0000";
                int score = res != null ? res.Score : 0;
                totalNodes += nodes;
                totalTime += bestTime;
                md.Add($"| {c.name} | {nodes} | {bestTime} | {nps} | {best} | {score} | {branch:F1} |");
            }
            tnps = totalTime > 0 ? totalNodes * 1000 / totalTime : 0;
            md.Add("");
            md.Add($"**search TOTAL (d{searchDepth})** : {totalNodes} nodes en {totalTime} ms → **{tnps} nps**");
            md.Add("");

            return md;
        }

        // ecrit le rapport markdown dans bench/<name>.md a la racine du projet
        public static void Save(string name, List<string> lines)
        {
            string dir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "bench"));
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, name + ".md");
            File.WriteAllLines(path, lines);
            Console.WriteLine($"rapport markdown ecrit dans {path}");
        }
    }
}
