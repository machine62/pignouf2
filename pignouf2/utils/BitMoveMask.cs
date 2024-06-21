using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.utils
{
    internal class BitMoveMask
    {
        static public UInt64[] _PawnMoveMaskNorth = new UInt64[64];
        static public UInt64[] _PawnMoveMaskSouth = new UInt64[64];
        public static UInt64[] PawnMoveMaskSouth
        {
            get
            {
                return _PawnMoveMaskSouth;
            }
        }
        public static UInt64[] PawnMoveMaskNorth
        {
            get
            {
                return _PawnMoveMaskNorth;
            }
        }
      static UInt64[] _KingMoveMask = new UInt64[64];
        public static UInt64[] KingMoveMask
        {
            get
            {
                return _KingMoveMask;
            }
        }

        static public UInt64[] _KnightMoveMask = new UInt64[64];
        public static UInt64[] KnightMoveMask
        {
            get
            {
                return _KnightMoveMask;
            }
        }


        public static void InitMasks()
        {
            InitMasksKing();
            InitMasksKnight();
            InitMasksPawn();
        }

        private static void InitMasksPawn()
        {
            for (int i = 0; i < 64; i++)
            {
                UInt64 pattern = BitOperation.SetBit(UInt64.MinValue, (byte)i);

                ulong MaskB = ((pattern << 7) | (pattern << 9)) & ~(BitCST.col_8);
                ulong MaskW  = ((pattern >> 7) | (pattern >> 9)) & ~(BitCST.col_1);
                // les blancs peuvent attaquer de la ligne deux a sept :/
                _PawnMoveMaskNorth[i] = MaskW;
                _PawnMoveMaskSouth[i] = MaskB;
            }
        }
                private static void InitMasksKnight()
        {
            for (int i = 0; i < 64; i++)
            {

                // Knight Masks
                UInt64 Mask = 0;
                Mask |= ((ulong)1 << i);

                UInt64 pattern1 = 0;
                UInt64 pattern2 = 0;


                pattern1 = (((Mask << 10) | (Mask >> 6)) & ~(BitCST.col_2));
                pattern1 = (pattern1 | ((Mask << 17) | (Mask >> 15))) & ~(BitCST.col_1);

                pattern2 = (((Mask >> 10) | (Mask << 6)) & ~(BitCST.col_7));
                pattern2 = (pattern2 | ((Mask >> 17) | (Mask << 15))) & ~(BitCST.col_8);


                Mask = pattern1 | pattern2;
                _KnightMoveMask[i] = Mask;

               // HumanView.UlongToHumanView(_KnightMoveMask[i]);

            }
        }


        private static void InitMasksKing()
        {
            // King Masks
            for (int i = 0; i < 64; i++)
            {
                UInt64 Mask = 0;
                Mask |= ((ulong)1 << i);

                // on effectue une horizontale de 3 bit
                Mask = Mask | (Mask << 1) & ~BitCST.col_1;
                Mask = Mask | (Mask >> 1) & ~BitCST.col_8;

                // on decale de +8 et de - 8
                Mask = Mask | (Mask << 8) | (Mask >> 8);

                // on supp la place du roi du pattern
                Mask = Mask ^ ((ulong)1 << i);

                KingMoveMask[i] = Mask;
                //HumanView.UlongToHumanView(KingMoveMask[i]);
            }
        }



    }
}
