using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace pignouf2.utils
{
    internal class BitMoveMagic2
    {
        //cf https://github.com/maksimKorzh/chess_programming
        // ne fn pas
        public static void InitMAgics()
        {
          InitializeBishopMoveTables();
           InitializeRookMoveTables();
        }

        private static readonly ulong[] RookMagicNumbers = new ulong[64]
    {
        0x8a80104000800020UL, 0x140002000100040UL, 0x2801880a0017001UL, 0x100081001000420UL,
        0x200020010080420UL, 0x3001c0002010008UL, 0x8480008002000100UL, 0x2080088004402900UL,
        0x800098204000UL, 0x2024401000200040UL, 0x100802000801000UL, 0x120800800801000UL,
        0x208808088000400UL, 0x2802200800400UL, 0x2200800100020080UL, 0x801000060821100UL,
        0x80044006422000UL, 0x100808020004000UL, 0x12108a0010204200UL, 0x140848010000802UL,
        0x481828014002800UL, 0x8094004002004100UL, 0x4010040010010802UL, 0x20008806104UL,
        0x100400080208000UL, 0x2040002120081000UL, 0x21200680100081UL, 0x20100080080080UL,
        0x2000a00200410UL, 0x20080800400UL, 0x80088400100102UL, 0x80004600042881UL,
        0x4040008040800020UL, 0x440003000200801UL, 0x4200011004500UL, 0x188020010100100UL,
        0x14800401802800UL, 0x2080040080800200UL, 0x124080204001001UL, 0x200046502000484UL,
        0x480400080088020UL, 0x1000422010034000UL, 0x30200100110040UL, 0x100021010009UL,
        0x2002080100110004UL, 0x202008004008002UL, 0x20020004010100UL, 0x2048440040820001UL,
        0x101002200408200UL, 0x40802000401080UL, 0x4008142004410100UL, 0x2060820c0120200UL,
        0x1001004080100UL, 0x20c020080040080UL, 0x2935610830022400UL, 0x44440041009200UL,
        0x280001040802101UL, 0x2100190040002085UL, 0x80c0084100102001UL, 0x4024081001000421UL,
        0x20030a0244872UL, 0x12001008414402UL, 0x2006104900a0804UL, 0x1004081002402UL
    };

        private static readonly int[] RookShifts = new int[64]
        {
        12, 11, 11, 11, 11, 11, 11, 12,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        12, 11, 11, 11, 11, 11, 11, 12
        };

        private static readonly ulong[][] RookMoveTables = new ulong[64][];

        private static void InitializeRookMoveTables()
        {
            string rookFileName = "rookMoveTables.bin";
            if (File.Exists(rookFileName))
            {
                DeserializeMoveTables(rookFileName, RookMoveTables, RookShifts);
            }
            else
            {
                InitializeRookMoveTablesRealTime();
                SerializeMoveTables(rookFileName, RookMoveTables, RookShifts);
            }

        }
            private static void InitializeRookMoveTablesRealTime()
        {

            for (int square = 0; square < 64; square++)
            {
                int shift = RookShifts[square];
                int tableSize = 1 << shift;
                RookMoveTables[square] = new ulong[tableSize];

                for (int index = 0; index < tableSize; index++)
                {
                    ulong occupied = IndexToOccupied(index, shift);
                    RookMoveTables[square][index] = GenerateRookMoves(square, occupied);
                }
            }
        }

        private static ulong IndexToOccupied(int index, int shift)
        {
            ulong occupied = 0;
            for (int i = 0; i < shift; i++)
            {
                if ((index & (1 << i)) != 0)
                {
                    occupied |= 1UL << i;
                }
            }
            return occupied;
        }

        private static void SerializeMoveTables(string fileName, ulong[][] moveTables, int[] shifts)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                for (int square = 0; square < 64; square++)
                {
                    int tableSize = 1 << shifts[square];
                    for (int index = 0; index < tableSize; index++)
                    {
                        writer.Write(moveTables[square][index]);
                    }
                }
            }
        }

        private static void DeserializeMoveTables(string fileName, ulong[][] moveTables, int[] shifts)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                for (int square = 0; square < 64; square++)
                {
                    int tableSize = 1 << shifts[square];
                    moveTables[square] = new ulong[tableSize];
                    for (int index = 0; index < tableSize; index++)
                    {
                        moveTables[square][index] = reader.ReadUInt64();
                    }
                }
            }
        }




        private static ulong GenerateRookMoves(int square, ulong occupied)
        {
            ulong moves = 0;

            //mvt 1
            for (int i = square + 1; i < (square | 7); i++)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            //mvt 2 
            for (int i = square - 1; i >= (square & ~7); i--)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            //mvt 3
            for (int i = square + 8; i < 64; i += 8)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            //mvt 4
            for (int i = square - 8; i >= 0; i -= 8)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }

            return moves;
        }

        public static ulong GetRookMoves(int square, ulong occupied)
        {
            ulong magicIndex = (occupied * RookMagicNumbers[square]) >> (64 - RookShifts[square]);
            return RookMoveTables[square][magicIndex];
        }

        private static readonly ulong[] BishopMagicNumbers = new ulong[64]
   {
        0x007fbfbfbfbfbfffUL, 0x0000a060401007fcUL, 0x0001004008020000UL, 0x0000806004000000UL,
        0x0000100400000000UL, 0x000021c040000000UL, 0x0000040100400000UL, 0x0000020080200800UL,
        0x0000040002080410UL, 0x0000020004200020UL, 0x0000208000400020UL, 0x0000100080800040UL,
        0x0002000811002100UL, 0x0000001000100200UL, 0x0000000800088080UL, 0x0000000800042000UL,
        0x0000011040008020UL, 0x0000008020008020UL, 0x0000004120001000UL, 0x0000000802000040UL,
        0x0000000801000020UL, 0x0000000800800020UL, 0x0000000800400002UL, 0x0000000200100000UL,
        0x0000000200080000UL, 0x0000000200040000UL, 0x0000000100020000UL, 0x0000000100010000UL,
        0x0000000100008020UL, 0x0000000100004020UL, 0x0000000100002000UL, 0x0000000100001000UL,
        0x0000000100000800UL, 0x0000000100000400UL, 0x0000000100000200UL, 0x0000000100000100UL,
        0x0000000100000080UL, 0x0000000100000040UL, 0x0000000100000020UL, 0x0000000100000010UL,
        0x0000000100000008UL, 0x0000000100000004UL, 0x0000000100000002UL, 0x0000000100000001UL,
        0x0000000008000000UL, 0x0000000004000000UL, 0x0000000002000000UL, 0x0000000001000000UL,
        0x0000000000800000UL, 0x0000000000400000UL, 0x0000000000200000UL, 0x0000000000100000UL,
        0x0000000000080000UL, 0x0000000000040000UL, 0x0000000000020000UL, 0x0000000000010000UL,
        0x0000000000008000UL, 0x0000000000004000UL, 0x0000000000002000UL, 0x0000000000001000UL,
        0x0000000000000800UL, 0x0000000000000400UL, 0x0000000000000200UL, 0x0000000000000100UL
   };

        private static readonly int[] BishopShifts = new int[64]
        {
        6, 5, 5, 5, 5, 5, 5, 6,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        6, 5, 5, 5, 5, 5, 5, 6
        };

        private static readonly ulong[][] BishopMoveTables = new ulong[64][];

        private static void InitializeBishopMoveTables()
        {
            string fileName = "BishopMoveTables.bin";
            if (File.Exists(fileName))
            {
                DeserializeMoveTables(fileName, BishopMoveTables, BishopShifts);
            }
            else
            {
                InitializeBishopMoveTablesrealtime();
                SerializeMoveTables(fileName, BishopMoveTables, BishopShifts);
            }
        }



        private static void InitializeBishopMoveTablesrealtime()
        {
         
            for (int square = 0; square < 64; square++)
            {  
   
                int shift = 64 - BishopShifts[square];
                int tableSize = 1 << BishopShifts[square];
                BishopMoveTables[square] = new ulong[tableSize];

                for (int index = 0; index < tableSize; index++)
                {
                    ulong occupied = IndexToOccupied(index, shift);
                    BishopMoveTables[square][index] = GenerateBishopMoves(square, occupied);
                         
                }
            }
        }



        private static ulong GenerateBishopMoves(int square, ulong occupied)
        {
            ulong moves = 0;

            // mvt 1
            for (int i = square + 9; i < 64 && (i & 7) != 0; i += 9)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            // mvt 2
            for (int i = square - 9; i >= 0 && (i & 7) != 7; i -= 9)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            // mvt 3
            for (int i = square + 7; i < 64 && (i & 7) != 7; i += 7)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }
            // mvt 4
            for (int i = square - 7; i >= 0 && (i & 7) != 0; i -= 7)
            {
                moves |= 1UL << i;
                if ((occupied & (1UL << i)) != 0) break;
            }

            return moves;
        }

        public static ulong GetBishopMoves(int square, ulong occupied)
        {
            ulong magicIndex = (occupied * BishopMagicNumbers[square]) >> (64 - BishopShifts[square]);
            return BishopMoveTables[square][magicIndex];
        }

    }
}
