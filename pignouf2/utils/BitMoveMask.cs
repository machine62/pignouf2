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

                // le masque anti wrap s applique sur la case de depart (avant le decalage), pas sur le resultat
                // attaque vers le sud : >>7 = +1 col (wrap depuis h exclu par ~col_8), >>9 = -1 col (wrap depuis a exclu par ~col_1)
                ulong MaskB = ((pattern & ~BitCST.col_8) >> 7) | ((pattern & ~BitCST.col_1) >> 9);
                // attaque vers le nord : <<9 = +1 col (wrap depuis h exclu par ~col_8), <<7 = -1 col (wrap depuis a exclu par ~col_1)
                ulong MaskW  = ((pattern & ~BitCST.col_8) << 9) | ((pattern & ~BitCST.col_1) << 7);
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

                // chaque decalage a son propre masque anti wrap applique sur la case de depart avant le decalage
                UInt64 pattern = 0;
                // +2 ligues +1 col et -2 ligues +1 col : wrap depuis la colonne h
                pattern |= (Mask & ~BitCST.col_8) << 17;
                pattern |= (Mask & ~BitCST.col_8) >> 15;
                // +1 ligue +2 cols et -1 ligue +2 cols : wrap depuis les colonnes g et h
                pattern |= (Mask & ~(BitCST.col_7 | BitCST.col_8)) << 10;
                pattern |= (Mask & ~(BitCST.col_7 | BitCST.col_8)) >> 6;
                // +2 ligues -1 col et -2 ligues -1 col : wrap depuis la colonne a
                pattern |= (Mask & ~BitCST.col_1) << 15;
                pattern |= (Mask & ~BitCST.col_1) >> 17;
                // +1 ligue -2 cols et -1 ligue -2 cols : wrap depuis les colonnes a et b
                pattern |= (Mask & ~(BitCST.col_1 | BitCST.col_2)) << 6;
                pattern |= (Mask & ~(BitCST.col_1 | BitCST.col_2)) >> 10;

                _KnightMoveMask[i] = pattern;

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
                // le masque anti wrap s applique sur la case de depart avant le decalage
                // <<1 decale vers la colonne h : le wrap h vers a est exclu par ~col_8
                Mask = Mask | ((Mask & ~BitCST.col_8) << 1);
                // >>1 decale vers la colonne a : le wrap a vers h est exclu par ~col_1
                Mask = Mask | ((Mask & ~BitCST.col_1) >> 1);

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
