using pignouf2.utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static pignouf2.core.ChessEnum;

namespace pignouf2.core
{
    internal class MoveGenerator
    {
        ChessBoard _CB;
        List<Move> _AllMoves = new List<Move>();

        UInt64 ATTKing = 0;
        UInt64 ATTQueen = 0;
        UInt64 ATTRook = 0;
        UInt64 ATTKnight = 0;
        UInt64 ATTBishop = 0;
        UInt64 ATTPawn = 0;
        UInt64 ATTAll = 0;

        UInt64 DEFKing = 0;
        UInt64 DEFQueen = 0;
        UInt64 DEFRook = 0;
        UInt64 DEFKnight = 0;
        UInt64 DEFBishop = 0;
        UInt64 DEFPawn = 0;
        UInt64 DEFAll = 0;

        UInt64 AllOcc = 0;




        public MoveGenerator(ref ChessBoard RefCB)
        {
            _CB = RefCB;
            _AllMoves = new List<Move>();

            if (_CB.getChessBoardState().Trait == ChessEnum.Side.WHITE) // si blanc au trait
            {
                ATTKing = _CB.GetWPieces(ChessEnum.Piece.KING);
                ATTQueen = _CB.GetWPieces(ChessEnum.Piece.QUEEN);
                ATTRook = _CB.GetWPieces(ChessEnum.Piece.ROOK);
                ATTKnight = _CB.GetWPieces(ChessEnum.Piece.KNIGHT);
                ATTBishop = _CB.GetWPieces(ChessEnum.Piece.BISHOP);
                ATTPawn = _CB.GetWPieces(ChessEnum.Piece.PAWN);
                ATTAll = _CB.GetWOcc();

                DEFKing = _CB.GetBPieces(ChessEnum.Piece.KING);
                DEFQueen = _CB.GetBPieces(ChessEnum.Piece.QUEEN);
                DEFRook = _CB.GetBPieces(ChessEnum.Piece.ROOK);
                DEFKnight = _CB.GetBPieces(ChessEnum.Piece.KNIGHT);
                DEFBishop = _CB.GetBPieces(ChessEnum.Piece.BISHOP);
                DEFPawn = _CB.GetBPieces(ChessEnum.Piece.PAWN);
                DEFAll = _CB.GetBOcc();

                AllOcc = _CB.AllOcc();
            }
            else // si noir au trait
            {
                ATTKing = _CB.GetBPieces(ChessEnum.Piece.KING);
                ATTQueen = _CB.GetBPieces(ChessEnum.Piece.QUEEN);
                ATTRook = _CB.GetBPieces(ChessEnum.Piece.ROOK);
                ATTKnight = _CB.GetBPieces(ChessEnum.Piece.KNIGHT);
                ATTBishop = _CB.GetBPieces(ChessEnum.Piece.BISHOP);
                ATTPawn = _CB.GetBPieces(ChessEnum.Piece.PAWN);
                ATTAll = _CB.GetBOcc();

                DEFKing = _CB.GetWPieces(ChessEnum.Piece.KING);
                DEFQueen = _CB.GetWPieces(ChessEnum.Piece.QUEEN);
                DEFRook = _CB.GetWPieces(ChessEnum.Piece.ROOK);
                DEFKnight = _CB.GetWPieces(ChessEnum.Piece.KNIGHT);
                DEFBishop = _CB.GetWPieces(ChessEnum.Piece.BISHOP);
                DEFPawn = _CB.GetWPieces(ChessEnum.Piece.PAWN);
                DEFAll = _CB.GetWOcc();

                AllOcc = _CB.AllOcc();
            }

        }

        public bool isLegalMove(Move move)
        {


            return true;
        }

        private void addLegalMove(Move move)
        {
            //Console.WriteLine(move.ToString());
            if (isLegalMove(move))
            {
                _AllMoves.Add(move);
                //Console.WriteLine(move.ToString());
            }
        }



        public ChessEnum.Piece GetPieceAt(ChessEnum.Cases Case)
        {
            UInt64 positionMask = 1UL << (int)Case;

            if ((ATTKing & positionMask) != 0) return ChessEnum.Piece.KING;
            if ((ATTQueen & positionMask) != 0) return ChessEnum.Piece.QUEEN;
            if ((ATTRook & positionMask) != 0) return ChessEnum.Piece.ROOK;
            if ((ATTKnight & positionMask) != 0) return ChessEnum.Piece.KNIGHT;
            if ((ATTBishop & positionMask) != 0) return ChessEnum.Piece.BISHOP;
            if ((ATTPawn & positionMask) != 0) return ChessEnum.Piece.PAWN;

            if ((DEFKing & positionMask) != 0) return ChessEnum.Piece.KING;
            if ((DEFQueen & positionMask) != 0) return ChessEnum.Piece.QUEEN;
            if ((DEFRook & positionMask) != 0) return ChessEnum.Piece.ROOK;
            if ((DEFKnight & positionMask) != 0) return ChessEnum.Piece.KNIGHT;
            if ((DEFBishop & positionMask) != 0) return ChessEnum.Piece.BISHOP;
            if ((DEFPawn & positionMask) != 0) return ChessEnum.Piece.PAWN;

            return ChessEnum.Piece.NONE;



        }



        public List<Move> generateALLMoves()
        {
            _AllMoves.Clear();
            generateKingMoves();
            generateKnightMoves();
            generateBishopMoves();
            generateRookMoves();
            generateQueenMoves();
            generatePawnMoves();





            return _AllMoves;
        }

        private void generateAbstractPieceMovesRoutine(ChessEnum.Piece CurrentPiece, byte FromSquare, ulong pattern, bool IsdblPush = false)
        {
            // Suppression des pieces de memes couleurs, on ne s'attaque pas
            pattern = pattern & ~ATTAll;

            UInt64 PatternATT = pattern & DEFAll; // calcul coup Attaque
            UInt64 PatternQuiet = pattern & ~DEFAll; // coup calme

            if (PatternQuiet != 0) // deplacement simple
            {
                do
                {
                    Move M = new Move();
                    M.From = (ChessEnum.Cases)FromSquare;
                    M.To = (ChessEnum.Cases)BitOperation.BitScanForwardWithReset(ref PatternQuiet);
                    M.Piece = CurrentPiece;
                    M.CapturedPiece = Piece.NONE;
                    M.IsDoublePawnPush = IsdblPush;

                    addLegalMove(M);


                } while (PatternQuiet != 0);
            }
            if (PatternATT != 0) //attaque
            {
                do
                {
                    Move M = new Move();
                    M.From = (ChessEnum.Cases)FromSquare;
                    M.To = (ChessEnum.Cases)BitOperation.BitScanForwardWithReset(ref PatternATT);
                    M.CapturedPiece = GetPieceAt(M.To);
                    M.Piece = CurrentPiece;
                    M.IsDoublePawnPush = IsdblPush;


                    addLegalMove(M);


                } while (PatternQuiet != 0);
            }

        }

        

        private void generateQueenMoves()
        {
            if (ATTQueen != 0)
            {
                byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTQueen); // on recupere la case de la piece
                ulong pattern = BitMoveMagic.GetBishopAttacks(FromSquare, AllOcc) | BitMoveMagic.GetRookAttacks(FromSquare, AllOcc); // pattern de mouvement

                generateAbstractPieceMovesRoutine(ChessEnum.Piece.QUEEN, FromSquare, pattern);

            }

        }
        private void generateKingMoves()
        {
            if (ATTKing != 0) // en meme temps si pas de roi la partie est réglée
            {
                byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTKing); // on recupere la case de la piece
                ulong pattern = BitMoveMask.KingMoveMask[FromSquare]; // pattern de mouvement

                generateAbstractPieceMovesRoutine(ChessEnum.Piece.KING, FromSquare, pattern);

            }

        }
        private void generateKnightMoves()
        {
            if (ATTKnight != 0) // en meme temps si pas de roi la partie est réglée
            {
                do
                {
                    byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTKnight); // on recupere la case de la piece
                    ulong pattern = BitMoveMask.KnightMoveMask[FromSquare]; // pattern de mouvement
                                                                            //HumanView.UlongToHumanView(pattern);

                    generateAbstractPieceMovesRoutine(ChessEnum.Piece.KNIGHT, FromSquare, pattern);
                } while (ATTKnight != 0);
            }

        }

        private void generateBishopMoves()
        {
            if (ATTBishop != 0) // en meme temps si pas de roi la partie est réglée
            {
                do
                {
                    byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTBishop); // on recupere la case de la piece
                                                                                           //HumanView.UlongToHumanView(pattern);
                    ulong pattern = BitMoveMagic.GetBishopAttacks(FromSquare, AllOcc);

                    generateAbstractPieceMovesRoutine(ChessEnum.Piece.BISHOP, FromSquare, pattern);
                } while (ATTBishop != 0);
            }

        }

        private void generateRookMoves()
        {
            if (ATTRook != 0) // en meme temps si pas de roi la partie est réglée
            {
                do
                {
                    byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTRook); // on recupere la case de la piece
                                                                                         //HumanView.UlongToHumanView(pattern);
                    ulong pattern = BitMoveMagic.GetRookAttacks(FromSquare, AllOcc);

                    generateAbstractPieceMovesRoutine(ChessEnum.Piece.ROOK, FromSquare, pattern);
                } while (ATTRook != 0);
            }

        }



        private void generatePawnMoves()
        {
            if (ATTPawn != 0) // seulement s'il y a des pions
            {
                do
                {
                    byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTPawn);
                    UInt64 pattern = BitOperation.SetBit(UInt64.MinValue, FromSquare);

                    bool isPromote = false;
                    if (_CB.getChessBoardState().Trait == ChessEnum.Side.WHITE)
                    {
                        isPromote = (pattern & BitCST.lig_7) != 0;
                    }
                    else
                    {
                        isPromote = (pattern & BitCST.lig_2) != 0;
                    }
                    // ajouter iPromoted  au simpele push et a l attaque

                    // tout ce qui se fait depuis la 7 eme rangée pour blanc et 2 


                    generatePawnMovesDblPush(pattern, FromSquare); // fusion possible double est simple // todo ?
                    generatePawnMovesSimplePush(pattern, FromSquare,isPromote);
                    generatePawnMovesAttack(FromSquare, isPromote);
                    generatePawnMovesAttackPEP(FromSquare);

                    //encore non genere
                    //pep
                    //promotion



                } while (ATTPawn != 0);
            }

        }

        private void generatePawnMovesAttackPEP(byte FromSquare)
        {
            // pep possible si case pep conforme au pattern 'attaque simple 
            if (_CB.getChessBoardState().PeP != Cases.none)
            {
                // todo 
                // penser a attaque  et a la modif niveau make move

            }
        }
        private void generatePawnMovesAttack(byte FromSquare, bool isPromote)
        {
            ulong pattern;
            if (_CB.getChessBoardState().Trait == ChessEnum.Side.WHITE)
            {
                pattern = BitMoveMask._PawnMoveMaskNorth[FromSquare];
            }
            else
            {
                pattern = BitMoveMask._PawnMoveMaskNorth[FromSquare];
            }

            pattern &= DEFAll; // le masque de prise n est valide que si un adversaire est sur la case de destination

            generateAbstractPieceMovesRoutine(ChessEnum.Piece.PAWN, FromSquare, pattern);
        }

        private void generatePawnMovesSimplePush(UInt64 pattern, byte FromSquare, bool isPromote)
        {
            UInt64 movePattern;
            if (_CB.getChessBoardState().Trait == ChessEnum.Side.WHITE)
            {
                movePattern = (pattern << 8) & ~AllOcc;
            }
            else
            {
                movePattern = (pattern >> 8) & ~AllOcc;
            }
           

            if (movePattern != 0)
            {
                
                if (isPromote)
                {
                    foreach (ChessEnum.Piece p in Enum.GetValues(typeof(ChessEnum.Piece)))
                    {
                    Move M = new Move();
                    M.From = (ChessEnum.Cases)FromSquare;
                    M.To = (ChessEnum.Cases)BitOperation.BitScanForward(movePattern);
                    M.Piece = ChessEnum.Piece.PAWN;
                    M.CapturedPiece = Piece.NONE;
                    M.IsDoublePawnPush = false;
                    M.IsPromotion = true;
                    M.PromotionPiece = p;

                    addLegalMove(M);
                    }
                }
                else
                {
                    Move M = new Move();
                    M.From = (ChessEnum.Cases)FromSquare;
                    M.To = (ChessEnum.Cases)BitOperation.BitScanForward(movePattern);
                    M.Piece = ChessEnum.Piece.PAWN;
                    M.CapturedPiece = Piece.NONE;
                    M.IsDoublePawnPush = false;

                    addLegalMove(M);
                }
            }


        }

        private void generatePawnMovesDblPush(UInt64 pattern, byte FromSquare)
        {
            // pour faire double push 
            // 1 sur la seconde ligne \ avant derniere
            // personne sur +8 +16 | -8 -16
            // cf https://www.chessprogramming.org/General_Setwise_Operations#OneStepOnly
            //https://www.chessprogramming.org/Pawn_Pushes_(Bitboards)
    
            UInt64 movePattern = 0;
            ChessEnum.Cases casforPEP = Cases.none;
            if (_CB.getChessBoardState().Trait == ChessEnum.Side.WHITE)
            {
                pattern = pattern & BitCST.lig_2;
                
                if (pattern != 0)
                {
                    // le pion doit etre sur la suxieme ligne
                    casforPEP = (ChessEnum.Cases)BitOperation.BitScanForward(pattern << 8);
                    movePattern = (pattern << 8) & ~AllOcc;
                    movePattern = (movePattern << 8) & ~AllOcc; // double push
                }
            }
            else
            {
                pattern = pattern & BitCST.lig_7;
                if (pattern != 0)
                {
                    // le pion doit etre sur la suxieme ligne
                    casforPEP = (ChessEnum.Cases)BitOperation.BitScanForward(pattern >> 8);
                    movePattern = (pattern >> 8) & ~AllOcc;
                    movePattern = (movePattern >> 8) & ~AllOcc; // double push

                }
            }
             if (movePattern != 0)
            {
                // il y a un seulm bit de present c un simple push
                Move M = new Move();
                M.From = (ChessEnum.Cases)FromSquare;
                M.To = (ChessEnum.Cases)BitOperation.BitScanForward(movePattern);
                M.Piece = ChessEnum.Piece.PAWN;
                M.IsDoublePawnPush = true;
                M.CaseEnPassant = casforPEP;
          
                addLegalMove(M);
            }

        }
        //      private void generateKingMoves2()
        //      {
        //UInt64 PatternATT;
        //UInt64 PatternQuiet;

        //  if (ATTKing != 0) // en meme temps si pas de roi la partie est réglée
        // {
        //byte FromSquare = BitOperation.BitScanForwardWithReset(ref ATTKing); // on recupere la case de la piece
        //ulong pattern = BitMoveMask.KingMoveMask[FromSquare]; // patterne de mouvement

        // Suppression des pieces de memes couleurs, on ne s'attaque pas
        //pattern = pattern & ~ATTAll;

        //PatternATT = pattern & DEFAll; // calcul coup Attaque
        //PatternQuiet = pattern & ~DEFAll; // coup calme

        // les coups simple
        //        if (PatternQuiet != 0)
        //        {
        //            do
        //            {
        //Move M = new Move();
        //M.From = (ChessEnum.Cases)FromSquare;
        //M.To = (ChessEnum.Cases)BitOperation.BitScanForwardWithReset(ref PatternQuiet);
        //M.Piece = ChessEnum.Piece.KING;


        //addLegalMove(M);


        //} while (PatternQuiet != 0);
        //}
        // les coups d'attaque
        //if (PatternATT != 0)
        //{
        //   do
        // {
        //Move M = new Move();
        //M.From = (ChessEnum.Cases)FromSquare;
        //M.To = (ChessEnum.Cases)BitOperation.BitScanForwardWithReset(ref PatternATT);
        //M.CapturedPiece = GetPieceAt(M.To);
        //M.Piece = ChessEnum.Piece.KING;


        //addLegalMove(M);


        //} while (PatternQuiet != 0);
        //}
        // autres



        //}

        //}
    }
}
