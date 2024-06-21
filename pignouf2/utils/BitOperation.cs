using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.utils
{
    internal class BitOperation
    {
 
            /// <summary>
            ///  passe la valeur bit à 1 sur l'index defini 
            /// </summary>
            /// <param name="value">valeur à modifier</param>
            /// <param name="index">index du bit à modifier</param>
            public static UInt64 SetBit(UInt64 value, byte index)
            {
                value |= ((ulong)1 << index);
                return value;
            }

            /// <summary>
            ///  passe la valeur bit à 1 sur l'index defini 
            /// </summary>
            /// <param name="value">valeur à modifier</param>
            /// <param name="index">index du bit à modifier</param>
            public static void SetBit(ref UInt64 value, byte index)
            {
                value |= ((ulong)1 << index);
            }

        private const UInt64 DEBRUIJN64 = 0x03f79d71b4cb0a89;
        private static readonly Byte[] INDEX64 =
        {
             0, 47,  1, 56, 48, 27,  2, 60,
            57, 49, 41, 37, 28, 16,  3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11,  4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30,  9, 24,
            13, 18,  8, 12,  7,  6,  5, 63
            };

        public static Byte BitScanForwardDBJ(UInt64 bitmap)
        {
            //todo evaluer (System.Numerics.BitOperations.TrailingZeroCount(value);)
            // ne doit pas etre a 0(bitmap != 0);
            return INDEX64[((bitmap ^ (bitmap - 1)) * DEBRUIJN64) >> 58];
        }


        public static Byte BitScanForwardWithresetdDBJ(ref UInt64 bitmap)
        {
            // ne doit pas etre a 0(bitmap != 0);
            byte index = INDEX64[((bitmap ^ (bitmap - 1)) * DEBRUIJN64) >> 58];
            //reset bit
            bitmap &= bitmap - 1;
            return index;

        }
        public static byte BitScanForward(ulong bitmap)
        {
            return (byte)BitOperations.TrailingZeroCount(bitmap);
        }

        public static byte BitScanForwardWithReset(ref ulong bitmap)
        {
            byte index = (byte)BitOperations.TrailingZeroCount(bitmap);
            bitmap &= bitmap - 1; // Reset the bit
            return index;
        }
        public static int PopCount(ulong value)
        {
            return BitOperations.PopCount(value);
        }

    }
}
