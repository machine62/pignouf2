using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using pignouf2.core;
using static pignouf2.core.ChessEnum;
using System.Numerics;

namespace pignouf2.utils
{
    internal class FenParser
    {
        private bool _isCorrectFen;
        private string[] _fenBrut;

        private ChessBoardState _CBS;
        private UInt64[] _WhitePiece = new UInt64[6];
        private UInt64[] _BlackPiece = new UInt64[6];




        public FenParser(string txt)
        {
            // Champ 1:  description de la position avec les lettres correspondant aux noms anglais 
            // Champ 2:  couleur au trait: w si c'est aux blancs de jouer, b pour les noirs
            // Champ 3:  validité des roques: la présence d'une lettre indique que le roque est possible; on utilise respectivement les lettres K et Q pour le petit et grand roque blanc, et les lettres k et q pour les noirs.Si aucun roque n'est possible, on utilise le caractère - 
            // Champ 4:  quand un pion a été joué, on indique la case d'arrivée d'un pion adverse qui pourrait le prendre en passant. Sinon - 
            // Champ 5:  nombre de demi-coups depuis une capture ou un mouvement de pion (utilisé pour la règle des 50 coups) 
            // Champ 6:  nombre de coups de la partie (incrémenté à chaque coup des blancs) 

            string[] champs = txt.Split(System.Convert.ToChar(" "));
            if (champs.Length != 6)
            {
                _isCorrectFen = false;
                return;
            }
            _isCorrectFen = true;
            _fenBrut = champs;

            AnalyseFEN();
        }

        public ChessBoardState getFENState()
        {
            return _CBS;
        }

        public UInt64[] getFENWhitePiece()
        {
            return _WhitePiece;
        }

        public UInt64[] getFENBlackPiece() {
            return _BlackPiece;
        }

        private void AnalyseFEN()
        {
            //champs 1 
            getPiecePositionToUint();

            // champs 2 a 6
            getTraitToCBS();
            getRoqueToCBS();
            getPAPToCBS();
            getDemiCoupToCBS();
            getCoupToCBS();
        }
        private string[] NormalizeFENPosition()
        {
            string pos = _fenBrut[0];
            pos = pos.Replace("1", "0");
            pos = pos.Replace("2", "00");
            pos = pos.Replace("3", "000");
            pos = pos.Replace("4", "0000");
            pos = pos.Replace("5", "00000");
            pos = pos.Replace("6", "000000");
            pos = pos.Replace("7", "0000000");
            pos = pos.Replace("8", "00000000");

            string[] ligne;
            ligne = pos.Split(System.Convert.ToChar("/"));
            // on remet dans un ordre lisible a8 => h8 ... a1 => h1
            //Array.Reverse(ligne);

            return ligne;
        }


        public override string ToString()
        {

            string str = "";


            str += "Position : \n";
           string[] ArrayTxt = NormalizeFENPosition();
            foreach (string ligne in ArrayTxt)
            {
                str += ligne + "\n";
            }

            str += _CBS.ToString();

            return str;
                    }

        public ChessBoardState getChessBoardState()
        {
            return _CBS;
        }
        private void getPiecePositionToUint()
        {
            _WhitePiece[((int)Piece.PAWN)] = UInt64.MinValue;
            _WhitePiece[((int)Piece.ROOK)] = UInt64.MinValue;
            _WhitePiece[((int)Piece.KNIGHT)] = UInt64.MinValue;
            _WhitePiece[((int)Piece.BISHOP)] = UInt64.MinValue;
            _WhitePiece[((int)Piece.QUEEN)] = UInt64.MinValue;
            _WhitePiece[((int)Piece.KING)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.PAWN)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.ROOK)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.KNIGHT)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.BISHOP)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.QUEEN)] = UInt64.MinValue;
            _BlackPiece[((int)Piece.KING)] = UInt64.MinValue;

            byte index = 0;
            // on inverse l'ordre pour exploitation bitboard
            string[] TabNormalizeFENPosition = NormalizeFENPosition();
            Array.Reverse(TabNormalizeFENPosition);

            string Pos =System.String.Concat(TabNormalizeFENPosition);
            foreach (char letter in Pos)
            {
                if (letter == 'R')
                {
                    _WhitePiece[((int)Piece.ROOK)] = BitOperation.SetBit(_WhitePiece[((int)Piece.ROOK)], index);
                    }
                if (letter == 'N')
               {
                    _WhitePiece[((int)Piece.KNIGHT)] = BitOperation.SetBit(_WhitePiece[((int)Piece.KNIGHT)], index);
                }
                if (letter == 'B')
                {
                    _WhitePiece[((int)Piece.BISHOP)] = BitOperation.SetBit(_WhitePiece[((int)Piece.BISHOP)], index);
                }
                if (letter == 'Q')
                {
                    _WhitePiece[((int)Piece.QUEEN)] = BitOperation.SetBit(_WhitePiece[((int)Piece.QUEEN)], index);
                }
                if (letter == 'K')
                {
                    _WhitePiece[((int)Piece.KING)] = BitOperation.SetBit(_WhitePiece[((int)Piece.KING)], index);
                }
                if (letter == 'P')
                {
                    _WhitePiece[((int)Piece.PAWN)] = BitOperation.SetBit(_WhitePiece[((int)Piece.PAWN)], index);

                }

                if (letter == 'r')
                {
                    _BlackPiece[((int)Piece.ROOK)] = BitOperation.SetBit(_BlackPiece[((int)Piece.ROOK)], index);
                }
                if (letter == 'n')
                {
                    _BlackPiece[((int)Piece.KNIGHT)] = BitOperation.SetBit(_BlackPiece[((int)Piece.KNIGHT)], index);
                }
                if (letter == 'b')
                {
                    _BlackPiece[((int)Piece.BISHOP)] = BitOperation.SetBit(_BlackPiece[((int)Piece.BISHOP)], index);
                }
                if (letter == 'q')
                {
                    _BlackPiece[((int)Piece.QUEEN)] = BitOperation.SetBit(_BlackPiece[((int)Piece.QUEEN)], index);
                }
                if (letter == 'k')
                {
                    _BlackPiece[((int)Piece.KING)] = BitOperation.SetBit(_BlackPiece[((int)Piece.KING)], index);
                }
                if (letter == 'p')
                {
                    _BlackPiece[((int)Piece.PAWN)] = BitOperation.SetBit(_BlackPiece[((int)Piece.PAWN)], index);
                }
                index++;
            }
  
        }

        private void getTraitToCBS()
        {
            
            if (_fenBrut[1] == "w")
            {
          _CBS.Trait = pignouf2.core.ChessEnum.Side.WHITE;
      
            }
            else
            {
            _CBS.Trait = pignouf2.core.ChessEnum.Side.BLACK;
            }
       
        }

        private void getRoqueToCBS()
        {

            if (_fenBrut[2].Contains("K"))
                _CBS.WhitePR = true;
            if (_fenBrut[2].Contains("k"))
                _CBS.BlackPR = true;
            if (_fenBrut[2].Contains("Q"))
                _CBS.WhiteGR = true;
            if (_fenBrut[2].Contains("q"))
                _CBS.BlackGR = true;
        }

        private void getDemiCoupToCBS()
        {
            _CBS.DemiCoup = Int32.Parse(_fenBrut[4]);
        }
        private void getCoupToCBS()
        {
            _CBS.Coup = Int32.Parse(_fenBrut[5]);
        }


        private void getPAPToCBS()
        {
            //TODO a tester
            _CBS.PeP = Cases.none; ;
            switch (_fenBrut[3])
            {
                case "a3":
                    {
                        _CBS.PeP = Cases.A3; ;
                        break;
                    }

                case "b3":
                    {
                        _CBS.PeP = Cases.B3; 
                                     break;
                    }

                case "c3":
                    {
                        _CBS.PeP = Cases.C3;
                        break;
                    }

                case "d3":
                    {
                        _CBS.PeP = Cases.D3;
                        break;
                    }

                case "e3":
                    {
                        _CBS.PeP = Cases.E3;
                        break;
                    }

                case "f3":
                    {
                        _CBS.PeP = Cases.F3;
                        break;
                    }

                case "g3":
                    {
                        _CBS.PeP = Cases.G3;
                        break;
                    }

                case "h3":
                    {
                        _CBS.PeP = Cases.H3;
                        break;
                    }

                case "a6":
                    {
                        _CBS.PeP = Cases.A6;
                        break;
                    }

                case "b6":
                    {
                        _CBS.PeP = Cases.B6;
                        break;
                    }

                case "c6":
                    {
                        _CBS.PeP = Cases.C6;
                        break;
                    }

                case "d6":
                    {
                        _CBS.PeP = Cases.D6;
                        break;
                    }

                case "e6":
                    {
                        _CBS.PeP = Cases.E6;
                        break;
                    }

                case "f6":
                    {
                        _CBS.PeP = Cases.F6;
                        break;
                    }
                case "g6":
                    {
                        _CBS.PeP = Cases.G6;
                        break;
                    }

                case "h6":
                    {
                        _CBS.PeP = Cases.H6;
                        break;
                    }
            }
        }
    }
}
