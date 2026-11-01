using pignouf2.core;
using pignouf2.utils;

namespace pignouf2.search
{
    internal static class Evaluator
    {
        // valeurs materielles en centipawns (index = ChessEnum.Piece)
        private static readonly int[] Value = { 100, 320, 330, 500, 900, 0, 0 };
        // poids de mobilite par piece (la dame pèse moins, elle est deja tres forte)
        private static readonly int[] Mobility = { 1, 4, 4, 2, 1, 0, 0 };

        // retourne le score du point de vue du camp au trait (pour le negamax)
        public static int Evaluate(ChessBoard cb)
        {
            ulong occ = cb.AllOcc();
            int score = ScoreSide(cb, true, occ) - ScoreSide(cb, false, occ);
            return cb.getChessBoardState().Trait == ChessEnum.Side.WHITE ? score : -score;
        }

        private static int ScoreSide(ChessBoard cb, bool white, ulong occ)
        {
            int score = 0;
            for (int p = 0; p <= (int)ChessEnum.Piece.KING; p++)
            {
                ulong bb = white ? cb.GetWPieces((ChessEnum.Piece)p) : cb.GetBPieces((ChessEnum.Piece)p);
                score += BitOperation.PopCount(bb) * Value[p];
                if (Mobility[p] == 0)
                    continue;
                ulong bits = bb;
                while (bits != 0)
                {
                    byte sq = BitOperation.BitScanForwardWithReset(ref bits);
                    ulong att = Attacks((ChessEnum.Piece)p, white, sq, occ);
                    // une case vide attaquee = de la mobilite
                    score += Mobility[p] * BitOperation.PopCount(att & ~occ);
                }
            }
            return score;
        }

        private static ulong Attacks(ChessEnum.Piece p, bool white, byte sq, ulong occ)
        {
            switch (p)
            {
                case ChessEnum.Piece.PAWN:
                    return white ? BitMoveMask.PawnMoveMaskNorth[sq] : BitMoveMask.PawnMoveMaskSouth[sq];
                case ChessEnum.Piece.KNIGHT:
                    return BitMoveMask.KnightMoveMask[sq];
                case ChessEnum.Piece.KING:
                    return BitMoveMask.KingMoveMask[sq];
                case ChessEnum.Piece.BISHOP:
                    return BitMoveMagic.GetBishopAttacks(sq, occ);
                case ChessEnum.Piece.ROOK:
                    return BitMoveMagic.GetRookAttacks(sq, occ);
                case ChessEnum.Piece.QUEEN:
                    return BitMoveMagic.GetBishopAttacks(sq, occ) | BitMoveMagic.GetRookAttacks(sq, occ);
            }
            return 0;
        }
    }
}
