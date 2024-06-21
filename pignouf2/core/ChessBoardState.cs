using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static pignouf2.core.ChessEnum;

namespace pignouf2.core
{
    internal struct ChessBoardState
    {
        public bool WhiteGR;
        public bool WhitePR;
        public bool BlackGR;
        public bool BlackPR;

        public Side Trait;

        public Cases PeP;

        public int DemiCoup;
        public int Coup;

        public ChessBoardState Clone()
        {
            return (ChessBoardState)this.MemberwiseClone();
        }

        public void MakeMove(Move move)
        {
            if (Trait == ChessEnum.Side.BLACK)
            {
                Coup++; // c est a la fin du coup noir qu'on actualise le nb de coup joué
            }

            // si c'est un cou push double on ajoute la pep
            if (move.IsDoublePawnPush)
            {
                // changer la pep 
                PeP = move.CaseEnPassant;

            }
            else
            {
                //Tout autre coup annule la possibilité de pep
                PeP = Cases.none;  
            }

            if (Trait == ChessEnum.Side.WHITE)
            {
                // si coup de roi le roque est flingé
                if (move.Piece == ChessEnum.Piece.KING)
                {
                    WhiteGR = false;
                    WhiteGR = false;
                }
            }
            else
            {
                // si coup de roi le roque est flingé
                if (move.Piece == ChessEnum.Piece.KING)
                {
                    BlackGR = false;
                    BlackPR = false;
                }
            }





            
            
            
            
            
            
            // commeun a tous les coups
            // changement de trait apres tous les calcul de legalité et d actualisation etat
            Trait = Trait == ChessEnum.Side.WHITE ? ChessEnum.Side.BLACK : ChessEnum.Side.WHITE;
            DemiCoup++;
            // si trait au blanc c'est un nouveau coup
           




        }
        public void UnMakeMove(Move move)
        {
            // non utilisé
            //non non utilise pour le moment un hoistorique de mvt

        }

        public override string ToString()
        {

            string str = "";

            str += "Trait : " + Trait.ToString() + "\n";
            str += "\n";
            str += "Blanc GR : " + WhiteGR.ToString() + "\n";
            str += "Blanc PR : " + WhitePR.ToString() + "\n";
            str += "Black GR : " + BlackGR.ToString() + "\n";
            str += "Black PR : " + BlackPR.ToString() + "\n";
            str += "\n";
            str += "Prise en passant : " + PeP.ToString() + "\n";
            str += "\n";
            str += "Demi coup  : " + DemiCoup.ToString() + "\n";
            str += "\n";
            str += "Nombre Coup : " + Coup.ToString() + "\n";
            str += "\n";
            return str;



        }



    }

}
