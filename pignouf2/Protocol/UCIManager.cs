using pignouf2.core;
using pignouf2.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pignouf2.Protocol
{
    public class UCIManager
    {

        private bool isReady;
        private Pignouf _pif;



        public UCIManager()
        {
            _pif = new Pignouf();
            isReady = false;
        }

        public void Start()
        {
            HandleUci();

            while (true)
            {
                string input = Console.ReadLine();
                if (input == null)
                    continue;

                string[] tokens = input.Split(' ');

                switch (tokens[0])
                {
                    case "uci":
                        HandleUci();
                        break;
                    case "isready":
                        HandleIsReady();
                        break;
                    //   case "ucinewgame":

                    //      break;
                    case "position":
                        HandlePosition(tokens);
                        break;
                    // case "go":

                    //     break;
                    case "quit":
                        return;


                    // en dehors de l'UCI
                    case "ShowBoard":
                        _pif.ShowBoard();
                        break;

                    case "perft":
                        if (tokens.Length > 1 && int.TryParse(tokens[1], out int depth))
                        {
                            Console.WriteLine(_pif.Perft(depth));
                        }
                        break;

                    case "divide":
                        if (tokens.Length > 1 && int.TryParse(tokens[1], out int depths))
                        {
                            Console.WriteLine(_pif.Divide(depths));
                        }
                        break;
                    case "test":
                        PerftTest.ALLTest();
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {input}");
                        break;
                }
            }
        }

        private void HandleUci()
        {
            Console.WriteLine("Pignouf");
            Console.WriteLine("Machine62");
            Console.WriteLine("uciok");
        }

        private void HandleIsReady()
        {
            isReady = true;
            Console.WriteLine("readyok");
        }

        private void HandleUciNewGame()
        {
            //todo
        }

        private void HandlePosition(string[] tokens)
        {
            int index = Array.IndexOf(tokens, "position");
            if (index == -1)
                return;

            if (tokens.Length > index + 1 && tokens[index + 1] == "startpos")
            {
                _pif.setFEN(new utils.FenParser("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"));
                index += 2;
            }
            else if (tokens.Length > index + 2 && tokens[index + 1] == "fen")
            {
                string FENstr = string.Join(" ", tokens, index + 2, tokens.Length - (index + 2));
                _pif.setFEN(new utils.FenParser(FENstr));
            }

            if (index < tokens.Length && tokens[index] == "moves")
            {
                for (int i = index + 1; i < tokens.Length; i++)
                {
                    //todo
                    //ajouter les mvt
                }
            }
        }

    }


}
