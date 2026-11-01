using pignouf2.core;
using pignouf2.search;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace pignouf2.bench
{
    internal static class Sts
    {
        public const int DefaultDepth = 4;

        // lance le test STS sur tous les fichiers epd du dossier testsuite, a profondeur fixe
        // (aucune horloge : meme profondeur = meme conditions, pas de biais entre versions)
        public static List<string> Run(int depth, string saveName)
        {
            string dir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "bench", "testsuite"));
            var positions = Epd.LoadDir(dir);
            var md = new List<string>();
            md.Add($"# STS pignouf2 — {DateTime.Now:yyyy-MM-dd HH:mm:ss} — depth fixe={depth} — {RuntimeInformation.FrameworkDescription} — {positions.Count} positions");
            md.Add("");

            int total = 0, max = 0;
            var themes = new SortedDictionary<string, int[]>();
            int n = 0;
            foreach (var pos in positions)
            {
                n++;
                var board = new ChessBoard();
                board.setFEN(new FenParser(pos.Fen));
                // chaque position est jouee sur un clone frais (equivalent ucinewgame, pas de TT chez nous)
                var searcher = new Searcher(board.Clone(), new SearchOptions { MaxDepth = depth }, null);
                var res = searcher.Search();
                int pts = 0;
                if (res.HasBest)
                {
                    // on traduit le bestmove UCI en SAN pour chercher ses points dans c0
                    var table = San.BuildTable(board);
                    string san = null;
                    foreach (var kv in table)
                    {
                        if (kv.Value.UciEncode() == res.BestMove.UciEncode())
                        {
                            san = kv.Key;
                            break;
                        }
                    }
                    if (san != null && pos.Points.TryGetValue(san, out int p))
                        pts = p;
                }
                total += pts;
                max += 10;
                string theme = ThemeOf(pos.Id);
                if (!themes.ContainsKey(theme))
                    themes[theme] = new int[2];
                themes[theme][0] += pts;
                themes[theme][1] += 10;
                if (n % 100 == 0)
                    Console.Error.WriteLine($"sts {n}/{positions.Count}...");
            }

            double pct = max > 0 ? 100.0 * total / max : 0;
            double elo = 44.523 * pct - 242.85;
            md.Add("| theme | score | % |");
            md.Add("|---|---|---|");
            foreach (var kv in themes)
                md.Add($"| {kv.Key} | {kv.Value[0]}/{kv.Value[1]} | {100.0 * kv.Value[0] / kv.Value[1]:F1}% |");
            md.Add("");
            md.Add($"**STS TOTAL : {total}/{max} = {pct:F1}% — Elo estimé ≈ {elo:F0}** (formule 44.523*pct - 242.85)");
            md.Add("");

            if (saveName != null)
            {
                string benchDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "bench"));
                Directory.CreateDirectory(benchDir);
                string path = Path.Combine(benchDir, "sts-" + saveName + ".md");
                File.WriteAllLines(path, md);
                md.Add($"rapport ecrit dans {path}");
            }
            return md;
        }

        // "STS(v6.0) Recapturing.009" -> "Recapturing" ; "Undermine.001" -> "Undermine"
        private static string ThemeOf(string id)
        {
            string t = id;
            int p = t.LastIndexOf(") ");
            if (p >= 0)
                t = t.Substring(p + 2);
            return t.Split('.')[0];
        }
    }
}
