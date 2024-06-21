using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.utils
{
    internal class BitCST
    {
        public const UInt64 lig_1 = (uint)255;
        public const UInt64 lig_2 = (uint)65280;
        public const UInt64 lig_3 = 16711680;
        public const UInt64 lig_4 = 4278190080;
        public const UInt64 lig_5 = 1095216660480;
        public const UInt64 lig_6 = 280375465082880;
        public const UInt64 lig_7 = 71776119061217280;
        public const UInt64 lig_8 = lig_1 << 56;
        public const UInt64 col_1 = 72340172838076673;
        public const UInt64 col_2 = 144680345676153346;
        public const UInt64 col_3 = 289360691352306692;
        public const UInt64 col_4 = 578721382704613384;
        public const UInt64 col_5 = 1157442765409226768;
        public const UInt64 col_6 = 2314885530818453536;
        public const UInt64 col_7 = 4629771061636907072;
        public const UInt64 col_8 = col_1 << 7;

        public const UInt64 SquareEmpty = (uint)0;
        public const UInt64 SquareFull = lig_1 | lig_2 | lig_3 | lig_4 | lig_5 | lig_6 | lig_7 | lig_8;

        public const UInt64 SquareFullNoCol_1 = SquareFull & ~col_1;
        public const UInt64 SquareFullNoCol_8 = SquareFull & ~col_8;
    }
}
