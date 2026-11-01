using pignouf2.core;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace pignouf2.Protocol
{
    public class UCIManager
    {
        private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        private Pignouf _pif;
        private readonly object _boardLock = new object();
        private volatile bool _stopRequested;
        private Thread _searchThread;
        private readonly Random _random = new Random();
        private int _hashMb = 16;
        private int _moveOverheadMs = 50;

        public UCIManager()
        {
            _pif = new Pignouf();
            lock (_boardLock)
            {
                _pif.setFEN(new FenParser(StartFen));
            }
        }

        public void Start()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == null)
                    return; // fin de stdin
                string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                    continue;

                switch (tokens[0])
                {
                    case "uci":
                        HandleUci();
                        break;
                    case "isready":
                        Console.WriteLine("readyok");
                        break;
                    case "ucinewgame":
                        HandleUciNewGame();
                        break;
                    case "setoption":
                        HandleSetOption(tokens);
                        break;
                    case "position":
                        HandlePosition(tokens);
                        break;
                    case "go":
                        HandleGo(tokens);
                        break;
                    case "stop":
                        _stopRequested = true;
                        break;
                    case "quit":
                        _stopRequested = true;
                        _searchThread?.Join();
                        return;
                    // commandes debug hors uci
                    case "ShowBoard":
                        lock (_boardLock) { _pif.ShowBoard(); }
                        break;
                    case "perft":
                        if (tokens.Length > 1 && int.TryParse(tokens[1], out int depth))
                        {
                            string res;
                            lock (_boardLock) { res = _pif.Perft(depth); }
                            Console.WriteLine(res);
                        }
                        break;
                    case "divide":
                        if (tokens.Length > 1 && int.TryParse(tokens[1], out int d2))
                        {
                            string res;
                            lock (_boardLock) { res = _pif.Divide(d2); }
                            Console.WriteLine(res);
                        }
                        break;
                    case "test":
                        test.PerftTest.ALLTest();
                        break;
                    default:
                        // commande inconnue : on ignore silencieusement (robustesse uci)
                        break;
                }
            }
        }

        private void HandleUci()
        {
            Console.WriteLine("id name pignouf2");
            Console.WriteLine("id author pignouf2");
            Console.WriteLine($"option name Hash type spin default {_hashMb} min 1 max 2048");
            Console.WriteLine("option name Threads type spin default 1 min 1 max 1");
            Console.WriteLine($"option name Move Overhead type spin default {_moveOverheadMs} min 0 max 1000");
            Console.WriteLine("option name Ponder type check default false");
            Console.WriteLine("uciok");
        }

        private void HandleUciNewGame()
        {
            // plus tard : vider TT, killers, history. rien pour l instant (pas d etat de recherche)
        }

        private void HandleSetOption(string[] tokens)
        {
            // setoption name <id> [value <x>]
            int nameIdx = Array.IndexOf(tokens, "name");
            int valueIdx = Array.IndexOf(tokens, "value");
            if (nameIdx == -1)
                return;
            int nameLen = valueIdx != -1 ? valueIdx - nameIdx - 1 : tokens.Length - nameIdx - 1;
            string name = string.Join(" ", tokens, nameIdx + 1, nameLen);
            string value = valueIdx != -1 && valueIdx + 1 < tokens.Length ? tokens[valueIdx + 1] : null;
            switch (name.ToLowerInvariant())
            {
                case "hash":
                    if (value != null && int.TryParse(value, out int h)) _hashMb = h;
                    break;
                case "move overhead":
                    if (value != null && int.TryParse(value, out int mo)) _moveOverheadMs = mo;
                    break;
                default:
                    break; // option declaree mais sans effet pour l instant
            }
        }

        private void HandlePosition(string[] tokens)
        {
            lock (_boardLock)
            {
                int i = 1;
                if (i < tokens.Length && tokens[i] == "startpos")
                {
                    _pif.setFEN(new FenParser(StartFen));
                    i++;
                }
                else if (i < tokens.Length && tokens[i] == "fen")
                {
                    i++;
                    var fenParts = new List<string>();
                    while (i < tokens.Length && tokens[i] != "moves")
                    {
                        fenParts.Add(tokens[i]);
                        i++;
                    }
                    _pif.setFEN(new FenParser(string.Join(" ", fenParts)));
                }

                // on rejoue les coups recus un par un via makeMove
                if (i < tokens.Length && tokens[i] == "moves")
                {
                    for (int k = i + 1; k < tokens.Length; k++)
                    {
                        string uci = tokens[k];
                        ChessBoard cb = _pif.getChessBoard();
                        var gen = new MoveGenerator(ref cb);
                        List<Move> moves = gen.generateALLMoves();
                        Move found = default;
                        bool ok = false;
                        foreach (var m in moves)
                        {
                            if (m.UciEncode() == uci)
                            {
                                found = m;
                                ok = true;
                                break;
                            }
                        }
                        if (!ok)
                        {
                            Console.Error.WriteLine($"coup non reconnu: {uci}");
                            break;
                        }
                        cb.MakeMove(found);
                    }
                }
            }
        }

        private void HandleGo(string[] tokens)
        {
            // on parse les parametres mais pour l instant la "recherche" choisit un coup legal au hasard
            bool infinite = tokens.Contains("infinite");
            _stopRequested = false;
            _searchThread = new Thread(() => Search(infinite));
            _searchThread.IsBackground = true;
            _searchThread.Start();
        }

        private void Search(bool infinite)
        {
            Move best;
            lock (_boardLock)
            {
                ChessBoard cb = _pif.getChessBoard();
                var gen = new MoveGenerator(ref cb);
                List<Move> moves = gen.generateALLMoves();
                if (moves.Count == 0)
                {
                    // mat ou pat : plus aucun coup legal
                    Console.WriteLine("bestmove 0000");
                    return;
                }
                best = moves[_random.Next(moves.Count)];
            }
            if (infinite)
            {
                while (!_stopRequested)
                    Thread.Sleep(10);
            }
            Console.WriteLine($"bestmove {best.UciEncode()}");
        }
    }
}
