using pignouf2.core;
using pignouf2.Protocol;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace pignouf2
{
    internal class Pignouf
    {
        ChessBoard _CB;


        public Pignouf()
        {
            BitMoveMask.InitMasks();
            BitMoveMagic.InitMAgics();

            _CB = new ChessBoard();
        }

        public ChessBoard getChessBoard()
        {
            return _CB;
        }
        public void setFEN(FenParser FEN)
        {
            _CB.setFEN(FEN);
        }

        public void ShowBoard()
        {
            HumanView.chessboardToHumanView(_CB);
        }

        public String Perft(int depth)
        {
            PerftManager P = new PerftManager(_CB);
            return P.Perft(depth);
          
        }



        
    }
}
