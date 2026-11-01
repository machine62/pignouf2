using pignouf2.utils;
using System.Collections.Generic;
using System.Linq;

namespace pignouf2.core
{
    internal static class San
    {
        // construit la table SAN -> Move pour la position (tous les coups legaux convertis en SAN)
        public static Dictionary<string, Move> BuildTable(ChessBoard board)
        {
            var cb = board;
            var gen = new MoveGenerator(ref cb);
            var moves = gen.generateALLMoves();
            var dict = new Dictionary<string, Move>();
            foreach (var m in moves)
                dict[Of(board, m, moves, false)] = m;
            return dict;
        }

        public static string Of(ChessBoard board, Move move, List<Move> allLegal, bool withCheck)
        {
            if (move.IsCastling)
                return (int)move.To % 8 == 6 ? "O-O" : "O-O-O";

            string s = "";
            bool isPawn = move.Piece == ChessEnum.Piece.PAWN;
            if (!isPawn)
            {
                s += PieceLetter(move.Piece);
                var others = allLegal.Where(m => m.Piece == move.Piece && m.To == move.To && m.From != move.From).ToList();
                if (others.Count > 0)
                {
                    bool sameFile = others.Any(m => (int)m.From % 8 == (int)move.From % 8);
                    bool sameRank = others.Any(m => (int)m.From / 8 == (int)move.From / 8);
                    if (!sameFile)
                        s += (char)('a' + (int)move.From % 8);
                    else if (!sameRank)
                        s += (char)('1' + (int)move.From / 8);
                    else
                        s += CaseName(move.From);
                }
            }
            bool capture = move.CapturedPiece != ChessEnum.Piece.NONE || move.IsEnPassant;
            if (capture)
            {
                if (isPawn)
                    s += (char)('a' + (int)move.From % 8);
                s += "x";
            }
            s += CaseName(move.To);
            if (move.IsPromotion)
                s += "=" + PieceLetter(move.PromotionPiece);

            if (withCheck)
            {
                // on joue le coup sur un clone pour savoir si c est echec ou mat
                var clone = board.Clone();
                clone.MakeMove(move);
                var st = clone.getChessBoardState();
                byte kingSq = st.Trait == ChessEnum.Side.WHITE ? BitOperation.BitScanForward(clone.GetWKing()) : BitOperation.BitScanForward(clone.GetBKing());
                if (clone.isSquareAttacked(kingSq, st.Trait))
                {
                    var g2 = new MoveGenerator(ref clone);
                    s += g2.generateALLMoves().Count == 0 ? "#" : "+";
                }
            }
            return s;
        }

        private static char PieceLetter(ChessEnum.Piece p)
        {
            switch (p)
            {
                case ChessEnum.Piece.KNIGHT: return 'N';
                case ChessEnum.Piece.BISHOP: return 'B';
                case ChessEnum.Piece.ROOK: return 'R';
                case ChessEnum.Piece.QUEEN: return 'Q';
                case ChessEnum.Piece.KING: return 'K';
            }
            return ' ';
        }

        private static string CaseName(ChessEnum.Cases c)
        {
            int i = (int)c;
            return "" + (char)('a' + i % 8) + (char)('1' + i / 8);
        }
    }
}
