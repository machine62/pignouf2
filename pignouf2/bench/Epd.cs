using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pignouf2.bench
{
    internal class EpdPos
    {
        public string Fen;
        public string Id;
        public Dictionary<string, int> Points = new Dictionary<string, int>();
    }

    internal static class Epd
    {
        // charge tous les fichiers .epd d un dossier (STS1..13), 1 position par ligne
        public static List<EpdPos> LoadDir(string dir)
        {
            var list = new List<EpdPos>();
            foreach (var file in Directory.GetFiles(dir, "*.epd").OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    var pos = ParseLine(line);
                    if (pos != null)
                        list.Add(pos);
                }
            }
            return list;
        }

        public static EpdPos ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;
            var pos = new EpdPos();
            var segs = line.Split(';');
            // seg[0] = "<FEN 4 champs> bm <coups>"
            var first = segs[0];
            int bmIdx = first.IndexOf(" bm ");
            if (bmIdx < 0)
                return null;
            pos.Fen = first.Substring(0, bmIdx).Trim();
            // les EPD n ont que 4 champs, FenParser en exige 6 : on complete
            if (pos.Fen.Split(' ').Length == 4)
                pos.Fen += " 0 1";
            foreach (var seg in segs.Skip(1))
            {
                var s = seg.Trim();
                if (s.StartsWith("id "))
                    pos.Id = ExtractQuoted(s.Substring(3));
                else if (s.StartsWith("c0 "))
                {
                    string c0 = ExtractQuoted(s.Substring(3));
                    foreach (var pair in c0.Split(','))
                    {
                        var p = pair.Trim();
                        int eq = p.LastIndexOf('=');
                        if (eq <= 0)
                            continue;
                        string san = NormalizeSan(p.Substring(0, eq));
                        if (int.TryParse(p.Substring(eq + 1).Trim(), out int pts))
                            pos.Points[san] = pts;
                    }
                }
            }
            if (pos.Id == null)
                pos.Id = "?";
            return pos;
        }

        private static string ExtractQuoted(string s)
        {
            int a = s.IndexOf('"');
            int b = s.LastIndexOf('"');
            if (a < 0 || b <= a)
                return s.Trim().TrimEnd(';');
            return s.Substring(a + 1, b - a - 1);
        }

        // enleve + # ! ? et les espaces : "Be5+" -> "Be5"
        public static string NormalizeSan(string san)
        {
            return san.Trim().TrimEnd('+', '#', '!', '?').Replace(" ", "");
        }
    }
}
