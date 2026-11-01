using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pignouf2.core
{


    internal struct Move
    {
        public ChessEnum.Cases From { get; set; }
        public ChessEnum.Cases To { get; set; }
        public ChessEnum.Piece Piece { get; set; }
        public ChessEnum.Piece CapturedPiece { get; set; }
        public bool IsPromotion { get; set; }
        public ChessEnum.Piece PromotionPiece { get; set; }
        public bool IsCastling { get; set; }
        public bool IsEnPassant { get; set; }
        public ChessEnum.Cases CaseEnPassant { get; set; }
        public bool IsDoublePawnPush { get; set; }

        //public Move(ChessEnum.Cases from, ChessEnum.Cases to, ChessEnum.Piece piece,
        //            ChessEnum.Piece capturedPiece = ChessEnum.Piece.NONE, bool isPromotion = false,
        //            ChessEnum.Piece promotionPiece = ChessEnum.Piece.NONE, bool isCastling = false,
        //            bool isEnPassant = false, bool isDoublePawnPush = false)
        public Move()
        {
            From = 0;
            To = 0;
            Piece = ChessEnum.Piece.NONE;
            CapturedPiece = ChessEnum.Piece.NONE;
            IsPromotion = false;
            PromotionPiece = ChessEnum.Piece.NONE;
            IsCastling = false;
            IsEnPassant = false;
            CaseEnPassant = ChessEnum.Cases.none;
            IsDoublePawnPush = false;
        }

        public string UciEncode()
        {
            // notation uci : minuscule, ex e2e4, promotion en 5e char ex e7e8q
            string s = CaseToUci(From) + CaseToUci(To);
            if (IsPromotion)
            {
                char p = PromotionPiece switch
                {
                    ChessEnum.Piece.ROOK => 'r',
                    ChessEnum.Piece.BISHOP => 'b',
                    ChessEnum.Piece.KNIGHT => 'n',
                    _ => 'q',
                };
                s += p;
            }
            return s;
        }

        private static string CaseToUci(ChessEnum.Cases c)
        {
            int i = (int)c;
            return "" + (char)('a' + i % 8) + (char)('1' + i / 8);
        }
        public override string ToString()
        {
            string str = "";
            str += $"Move from {From} to {To}, Piece: {Piece}, Captured: {CapturedPiece}, ";
            //if (IsPromotion)
            {
                str += $"Promotion: {IsPromotion}, Promotion Piece: {PromotionPiece}, ";
            }
            //if(Piece==ChessEnum.Piece.PAWN)
            {
                str += $"En Passant: {CaseEnPassant}, Double Pawn Push: {IsDoublePawnPush} ";
            }
            //if (IsPromotion)
            {
                str += $"Promotion: {IsPromotion}, Promotion Piece: {PromotionPiece}, ";
            }
            //if(IsCastling)
            {
                str += $"Castling: {IsCastling}";
            }
            return str;
        }
    }
}


