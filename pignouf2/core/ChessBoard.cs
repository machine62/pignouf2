using pignouf2.utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static pignouf2.core.ChessEnum;

namespace pignouf2.core
{
    internal class ChessBoard
    {
        private UInt64 _WKing;
        private UInt64 _WQueen;
        private UInt64 _WRook;
        private UInt64 _WKnight;
        private UInt64 _WBishop;
        private UInt64 _WPawn;
        //Noir
        private UInt64 _BKing;
        private UInt64 _BQueen;
        private UInt64 _BRook;
        private UInt64 _BKnight;
        private UInt64 _BBishop;
        private UInt64 _BPawn;



        private ChessBoardState _CBState;
        private Stack<Move> moveHistory = new Stack<Move>();
        private Stack<ChessBoardState> CBStateHistory = new Stack<ChessBoardState>();

        // Accesseurs pour les bitboards des pièces
        public UInt64 GetWKing() => _WKing;
        public UInt64 GetWQueen() => _WQueen;
        public UInt64 GetWRook() => _WRook;
        public UInt64 GetWKnight() => _WKnight;
        public UInt64 GetWBisho() => _WBishop;
        public UInt64 GetWPawn() => _WPawn;
        public UInt64 GetBKing() => _BKing;
        public UInt64 GetBQueen() => _BQueen;
        public UInt64 GetBRook() => _BRook;
        public UInt64 GetBKnight() => _BKnight;
        public UInt64 GetBBishop() => _BBishop;
        public UInt64 GetBPawn() => _BPawn;

        // Méthode pour obtenir l'état de l'échiquier
        public ChessBoardState GetChessBoardState() => _CBState;

        public UInt64 GetWOcc()
        {
            return _WKing | _WQueen | _WRook | _WKnight | _WBishop | _WPawn;
        }
        public UInt64 GetBOcc()
        {
            return _BKing | _BQueen | _BRook | _BKnight | _BBishop | _BPawn;
        }
        public UInt64 AllOcc()
        {
            return GetWOcc() | GetBOcc();
        }
        public UInt64 GetWPieces(ChessEnum.Piece piece)
        {
            switch (piece)
            {
                case Piece.PAWN:
                    return _WPawn;
                case Piece.ROOK:
                    return _WRook;
                case Piece.KNIGHT:
                    return _WKnight;
                case Piece.BISHOP:
                    return _WBishop;
                case Piece.QUEEN:
                    return _WQueen;
                case Piece.KING:
                    return _WKing;

            }
            return UInt64.MinValue;
        }

        public UInt64 GetBPieces(ChessEnum.Piece piece)
        {
            switch (piece)
            {
                case Piece.PAWN:
                    return _BPawn;
                case Piece.ROOK:
                    return _BRook;
                case Piece.KNIGHT:
                    return _BKnight;
                case Piece.BISHOP:
                    return _BBishop;
                case Piece.QUEEN:
                    return _BQueen;
                case Piece.KING:
                    return _BKing;
            }
            return UInt64.MinValue;
        }


        public void ResetChessBoard()
        {
            _WKing = UInt64.MinValue;
            _WQueen = UInt64.MinValue;
            _WRook = UInt64.MinValue;
            _WKnight = UInt64.MinValue;
            _WBishop = UInt64.MinValue;
            _WPawn = UInt64.MinValue;
            //Noir
            _BKing = UInt64.MinValue;
            _BQueen = UInt64.MinValue;
            _BRook = UInt64.MinValue;
            _BKnight = UInt64.MinValue;
            _BBishop = UInt64.MinValue;
            _BPawn = UInt64.MinValue;

            _CBState = new ChessBoardState();
            moveHistory = new Stack<Move>(); // reset history
            CBStateHistory = new Stack<ChessBoardState>();


        }
        public ChessBoardState getChessBoardState()
        {
            return _CBState;
        }

        public void MakeMove(Move move)
        {

            // Extraire les informations du mouvement
            ChessEnum.Piece piece = move.Piece;
            ChessEnum.Cases from = move.From;
            ChessEnum.Cases to = move.To;

            ChessEnum.Side side = _CBState.Trait;
            // Déterminer les bitboards à mettre à jour
            UInt64 fromMask = 1UL << (int)from;
            UInt64 toMask = 1UL << (int)to;

            // historisation
            moveHistory.Push(move);

            // Mettre à jour les bitboards

            // ----------------  WHITE SIDE ------------------------
            if (side == ChessEnum.Side.WHITE)
            {
                // ----------------  COUP SIMPLE ------------------------
                switch (piece)
                {

                    case ChessEnum.Piece.KING:
                        _WKing &= ~fromMask;
                        _WKing |= toMask;
                        break;
                    case ChessEnum.Piece.QUEEN:
                        _WQueen &= ~fromMask;
                        _WQueen |= toMask;
                        break;
                    case ChessEnum.Piece.ROOK:
                        _WRook &= ~fromMask;
                        _WRook |= toMask;
                        break;
                    case ChessEnum.Piece.KNIGHT:
                        _WKnight &= ~fromMask;
                        _WKnight |= toMask;
                        break;
                    case ChessEnum.Piece.BISHOP:
                        _WBishop &= ~fromMask;
                        _WBishop |= toMask;
                        break;
                    case ChessEnum.Piece.PAWN:
                        _WPawn &= ~fromMask;
                        _WPawn |= toMask;
                        break;
                }
                // ----------------  FIN COUP SIMPLE ------------------------

                // ----------------  PRISE ------------------------
                if (move.CapturedPiece != ChessEnum.Piece.NONE)
                {
                    switch (move.CapturedPiece)
                    {
                        case ChessEnum.Piece.KING: // Pour roi impossible puisque le generateur de coup ne le permettra pas
                            _BKing &= ~toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _BQueen &= ~toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _BRook &= ~toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _BKnight &= ~toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _BBishop &= ~toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _BPawn &= ~toMask;
                            break;
                    }

                }
                // ---------------- FIN PRISE ------------------------
            }
            // ----------------  WHITE SIDE FIN  ------------------------

            // ----------------  BLACK SIDE ------------------------
            else
            {
                switch (piece)
                {
                    case ChessEnum.Piece.KING:
                        _BKing &= ~fromMask;
                        _BKing |= toMask;
                        break;
                    case ChessEnum.Piece.QUEEN:
                        _BQueen &= ~fromMask;
                        _BQueen |= toMask;
                        break;
                    case ChessEnum.Piece.ROOK:
                        _BRook &= ~fromMask;
                        _BRook |= toMask;
                        break;
                    case ChessEnum.Piece.KNIGHT:
                        _BKnight &= ~fromMask;
                        _BKnight |= toMask;
                        break;
                    case ChessEnum.Piece.BISHOP:
                        _BBishop &= ~fromMask;
                        _BBishop |= toMask;
                        break;
                    case ChessEnum.Piece.PAWN:
                        _BPawn &= ~fromMask;
                        _BPawn |= toMask;
                        break;
                }
                if (move.CapturedPiece != ChessEnum.Piece.NONE)
                {
                    switch (move.CapturedPiece)
                    {
                        case ChessEnum.Piece.KING: // Pour roi impossible puisque le generateur de coup ne le permettra pas
                            _WKing &= ~toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _WQueen &= ~toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _WRook &= ~toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _WKnight &= ~toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _WBishop &= ~toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _WPawn &= ~toMask;
                            break;
                    }


                }

            }

            



             // ----------------  BLACK SIDE FIN ------------------------

            // prise en passant : le pion capture n est pas sur la case d arrivee mais derriere
            if (move.IsEnPassant)
            {
                ChessEnum.Cases capturedSquare = side == ChessEnum.Side.WHITE ? (ChessEnum.Cases)((int)to - 8) : (ChessEnum.Cases)((int)to + 8);
                ulong capMask = 1UL << (int)capturedSquare;
                if (side == ChessEnum.Side.WHITE)
                    _BPawn &= ~capMask;
                else
                    _WPawn &= ~capMask;
            }

            // roque : la tour accompagne le roi
            if (move.IsCastling)
            {
                if (side == ChessEnum.Side.WHITE)
                {
                    if (to == ChessEnum.Cases.G1) // petit roque : h1 -> f1
                    {
                        _WRook &= ~(1UL << (int)ChessEnum.Cases.H1);
                        _WRook |= (1UL << (int)ChessEnum.Cases.F1);
                    }
                    else // grand roque : a1 -> d1
                    {
                        _WRook &= ~(1UL << (int)ChessEnum.Cases.A1);
                        _WRook |= (1UL << (int)ChessEnum.Cases.D1);
                    }
                }
                else
                {
                    if (to == ChessEnum.Cases.G8) // petit roque : h8 -> f8
                    {
                        _BRook &= ~(1UL << (int)ChessEnum.Cases.H8);
                        _BRook |= (1UL << (int)ChessEnum.Cases.F8);
                    }
                    else // grand roque : a8 -> d8
                    {
                        _BRook &= ~(1UL << (int)ChessEnum.Cases.A8);
                        _BRook |= (1UL << (int)ChessEnum.Cases.D8);
                    }
                }
            }

            // todo actualisation et historisation du state
            _CBState.MakeMove(move);
            CBStateHistory.Push(_CBState);

                //     HumanView.chessboardToHumanView(this);

            }

            public void UnMakeMove()
            {

                if (moveHistory.Count == 0)
                    return;

                Move lastMove = moveHistory.Pop();

                // Extraire les informations du mouvement
                ChessEnum.Piece piece = lastMove.Piece;
                ChessEnum.Side side = _CBState.Trait == ChessEnum.Side.WHITE ? ChessEnum.Side.BLACK : ChessEnum.Side.WHITE; // on doit inverser le trait puisque c le copp d'avant qu'on va appliquer
                                                                                                                            // ChessEnum.Side side = _CBState.Trait;
                ChessEnum.Cases from = lastMove.From;
                ChessEnum.Cases to = lastMove.To;

                UInt64 fromMask = 1UL << (int)from;
                UInt64 toMask = 1UL << (int)to;

                if (side == ChessEnum.Side.WHITE)
                {
                    switch (piece)
                    {

                        case ChessEnum.Piece.KING:
                            _WKing |= fromMask;
                            _WKing &= ~toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _WQueen |= fromMask;
                            _WQueen &= ~toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _WRook |= fromMask;
                            _WRook &= ~toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _WKnight |= fromMask;
                            _WKnight &= ~toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _WBishop |= fromMask;
                            _WBishop &= ~toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _WPawn |= fromMask;
                            _WPawn &= ~toMask;
                            break;
                    }

                if (lastMove.CapturedPiece != ChessEnum.Piece.NONE)
                {
                    switch (lastMove.CapturedPiece)
                    {
                        case ChessEnum.Piece.KING: // Pour roi impossible puisque le generateur de coup ne le permettra pas
                            _BKing |= toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _BQueen |= toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _BRook |= toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _BKnight |= toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _BBishop |= toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _BPawn |= toMask;
                            break;
                    }


                }
            }
                else
                {
                    switch (piece)
                    {

                        case ChessEnum.Piece.KING:
                            _BKing |= fromMask;
                            _BKing &= ~toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _BQueen |= fromMask;
                            _BQueen &= ~toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _BRook |= fromMask;
                            _BRook &= ~toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _BKnight |= fromMask;
                            _BKnight &= ~toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _BBishop |= fromMask;
                            _BBishop &= ~toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _BPawn |= fromMask;
                            _BPawn &= ~toMask;
                            break;
                    }


                if (lastMove.CapturedPiece != ChessEnum.Piece.NONE)
                {
                    switch (lastMove.CapturedPiece)
                    {
                        case ChessEnum.Piece.KING: // Pour roi impossible puisque le generateur de coup ne le permettra pas
                            _WKing |=  toMask;
                            break;
                        case ChessEnum.Piece.QUEEN:
                            _WQueen |= toMask;
                            break;
                        case ChessEnum.Piece.ROOK:
                            _WRook |= toMask;
                            break;
                        case ChessEnum.Piece.KNIGHT:
                            _WKnight |= toMask;
                            break;
                        case ChessEnum.Piece.BISHOP:
                            _WBishop |= toMask;
                            break;
                        case ChessEnum.Piece.PAWN:
                            _WPawn |= toMask;
                            break;  
                    }


                }
            }

                // prise en passant : on remet le pion capture sur la case derriere
                if (lastMove.IsEnPassant)
                {
                    ChessEnum.Cases capturedSquare = side == ChessEnum.Side.WHITE ? (ChessEnum.Cases)((int)lastMove.To - 8) : (ChessEnum.Cases)((int)lastMove.To + 8);
                    ulong capMask = 1UL << (int)capturedSquare;
                    if (side == ChessEnum.Side.WHITE)
                        _BPawn |= capMask;
                    else
                        _WPawn |= capMask;
                }

                // roque : on remet la tour sur sa case de depart
                if (lastMove.IsCastling)
                {
                    if (side == ChessEnum.Side.WHITE)
                    {
                        if (lastMove.To == ChessEnum.Cases.G1) // petit roque : f1 -> h1
                        {
                            _WRook |= (1UL << (int)ChessEnum.Cases.H1);
                            _WRook &= ~(1UL << (int)ChessEnum.Cases.F1);
                        }
                        else // grand roque : d1 -> a1
                        {
                            _WRook |= (1UL << (int)ChessEnum.Cases.A1);
                            _WRook &= ~(1UL << (int)ChessEnum.Cases.D1);
                        }
                    }
                    else
                    {
                        if (lastMove.To == ChessEnum.Cases.G8) // petit roque : f8 -> h8
                        {
                            _BRook |= (1UL << (int)ChessEnum.Cases.H8);
                            _BRook &= ~(1UL << (int)ChessEnum.Cases.F8);
                        }
                        else // grand roque : d8 -> a8
                        {
                            _BRook |= (1UL << (int)ChessEnum.Cases.A8);
                            _BRook &= ~(1UL << (int)ChessEnum.Cases.D8);
                        }
                    }
                }

                /// si capture

             //   if (move.CapturedPiece != ChessEnum.Piece.NONE)
               // {
                    //todo
                //}



                // récuperation de l ancien state
                // Récupérer le dernier mouvement
                CBStateHistory.Pop();
                _CBState = CBStateHistory.Peek().Clone();

            }

     


        public void setFEN(FenParser FEN)
            {
                ResetChessBoard();
                // mise en place des bitboard
                UInt64[] FENWhitePiece = FEN.getFENWhitePiece();
                UInt64[] FENWBlackPiece = FEN.getFENBlackPiece();

                _WKing = FENWhitePiece[(int)ChessEnum.Piece.KING];
                _WQueen = FENWhitePiece[(int)ChessEnum.Piece.QUEEN];
                _WRook = FENWhitePiece[(int)ChessEnum.Piece.ROOK];
                _WKnight = FENWhitePiece[(int)ChessEnum.Piece.KNIGHT];
                _WBishop = FENWhitePiece[(int)ChessEnum.Piece.BISHOP];
                _WPawn = FENWhitePiece[(int)ChessEnum.Piece.PAWN];

                _BKing = FENWBlackPiece[(int)ChessEnum.Piece.KING];
                _BQueen = FENWBlackPiece[(int)ChessEnum.Piece.QUEEN];
                _BRook = FENWBlackPiece[(int)ChessEnum.Piece.ROOK];
                _BKnight = FENWBlackPiece[(int)ChessEnum.Piece.KNIGHT];
                _BBishop = FENWBlackPiece[(int)ChessEnum.Piece.BISHOP];
                _BPawn = FENWBlackPiece[(int)ChessEnum.Piece.PAWN];


                _CBState = FEN.getFENState();

                // historisation // permettra le unmake
                CBStateHistory.Push(_CBState);

            }

            public ChessBoard Clone()
            {
                ChessBoard clone = (ChessBoard)this.MemberwiseClone();
                clone._CBState = this._CBState.Clone();

                // histoire de la partie :
                clone.moveHistory = new Stack<Move>(new Stack<Move>(this.moveHistory));

                return clone;
            }

        public bool isSquareAttacked(byte Case, ChessEnum.Side side)
        {
            ulong CaseMask = BitOperation.SetBit(0, Case);

            //on considere qu'il y a attaque, quand en positionnant sur cette case une piece, cette meme piece attaque son posisionnement (exception pion (north/south inversé))
            // pions
            // ttaque hors pep
            if (side == ChessEnum.Side.WHITE)
            {
                //pions : un pion noir attaque vers le sud, il attaque Case s il est au nord de Case
                if ((BitMoveMask.PawnMoveMaskNorth[Case] & _BPawn) != 0)
                {
                    return true;
                }

                // cavalier
                if ((BitMoveMask.KnightMoveMask[Case] & _BKnight) != 0)
                {
                    return true;
                }
         
                // roi
                if ((BitMoveMask.KingMoveMask[Case] & _BKing) != 0)
                {
                    return true;
                }

                // fou et reine
                if ((BitMoveMagic.GetBishopAttacks(Case, AllOcc()) & (_BBishop | _BQueen)) != 0)
                {
                    return true;
                }

                // tour et reine
                if ((BitMoveMagic.GetRookAttacks(Case, AllOcc()) & (_BRook | _BQueen)) != 0)
                {
                    return true;
                }


            }
            if (side == ChessEnum.Side.BLACK)
            {
                // un pion blanc attaque vers le nord, il attaque Case s il est au sud de Case
                if ((BitMoveMask.PawnMoveMaskSouth[Case] & _WPawn) != 0)
                {
                    return true;
                }

                // cavalier
                if ((BitMoveMask.KnightMoveMask[Case] & _WKnight) != 0)
                {
                    return true;
                }

                //roi
                if ((BitMoveMask.KingMoveMask[Case] & _WKing) != 0)
                {
                    return true;
                }

                // fou et reine
                if ((BitMoveMagic.GetBishopAttacks(Case, AllOcc()) & (_WBishop | _WQueen)) != 0)
                {
                    return true;
                }

                // tour et reine
                if ((BitMoveMagic.GetRookAttacks(Case, AllOcc()) & (_WRook | _WQueen)) != 0)
                {
                    return true;
                }


            }

            return false;
        }

        public override string ToString()
        {

            string str = "";

            str += _CBState.ToString();
            HumanView.chessboardToHumanView(this);
            return str;



        }

    }
}
