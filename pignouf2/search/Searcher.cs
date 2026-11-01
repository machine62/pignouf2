using pignouf2.core;
using pignouf2.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pignouf2.search
{
    internal class SearchResult
    {
        public Move BestMove;
        public bool HasBest;
        public int Score;
        public int DepthReached;
        public long Nodes;
    }

    internal class Searcher
    {
        private const int Mate = 32000;
        private const int Infinity = 32001;
        private const int MaxPly = 64;

        private ChessBoard _cb; // pas readonly : MoveGenerator prend le board en ref
        private readonly SearchOptions _options;
        private readonly Action<string> _sendInfo;
        private volatile bool _stop;
        private long _nodes;
        private readonly Move[][] _pv;      // pv triangulaire : _pv[ply][0..pvLength[ply]-1]
        private readonly int[] _pvLength;

        public Searcher(ChessBoard cb, SearchOptions options, Action<string> sendInfo)
        {
            _cb = cb;
            _options = options;
            _sendInfo = sendInfo;
            _pv = new Move[MaxPly + 1][];
            _pvLength = new int[MaxPly + 1];
            for (int i = 0; i <= MaxPly; i++)
                _pv[i] = new Move[MaxPly + 1];
        }

        public void Stop()
        {
            _stop = true;
        }

        public SearchResult Search()
        {
            var result = new SearchResult();
            int maxDepth = _options.Infinite ? MaxPly : _options.MaxDepth;
            var sw = Stopwatch.StartNew();
            // iterative deepening simple de 1 a maxDepth : le bestmove retenu est celui
            // de la derniere iteration complete (une iteration stoppee ne compte pas)
            for (int depth = 1; depth <= maxDepth; depth++)
            {
                int score = Negamax(depth, 0, -Infinity, Infinity);
                if (_stop)
                    break;
                result.HasBest = true;
                result.BestMove = _pv[0][0];
                result.Score = score;
                result.DepthReached = depth;
                result.Nodes = _nodes;
                SendInfo(depth, score, sw.ElapsedMilliseconds);
            }
            sw.Stop();
            return result;
        }

        private void SendInfo(int depth, int score, long timeMs)
        {
            if (_sendInfo == null)
                return;
            long nps = timeMs > 0 ? _nodes * 1000 / timeMs : _nodes;
            string scoreStr;
            if (score > Mate - MaxPly)
                scoreStr = $"score mate {(Mate - score + 1) / 2}";
            else if (score < -(Mate - MaxPly))
                scoreStr = $"score mate {-(Mate + score + 1) / 2}";
            else
                scoreStr = $"score cp {score}";
            var pvMoves = new List<string>();
            for (int i = 0; i < _pvLength[0]; i++)
                pvMoves.Add(_pv[0][i].UciEncode());
            _sendInfo($"info depth {depth} seldepth {depth} {scoreStr} nodes {_nodes} nps {nps} time {timeMs} pv {string.Join(" ", pvMoves)}");
        }

        // negamax pur : alpha et beta sont deja dans la signature mais il n y a
        // aucune coupe tant que UseAlphaBeta est false (minimax sans elagage)
        private int Negamax(int depth, int ply, int alpha, int beta)
        {
            _nodes++;
            if ((_nodes & 2047) == 0 && _stop)
                return 0; // on remonte vite, l iteration en cours sera ignoree

            if (ply > 0)
            {
                // regle des 50 coups
                if (_cb.getChessBoardState().DemiCoup >= 100)
                    return 0;
                // materiel insuffisant : k vs k, kn vs k, kb vs k
                if (IsInsufficientMaterial())
                    return 0;
                // plus tard : repetition de position si _options.UseZobristKey
            }

            if (depth == 0)
            {
                // plus tard : quiescence si _options.UseQuiescence (effet horizon)
                return Evaluator.Evaluate(_cb);
            }

            var gen = new MoveGenerator(ref _cb);
            List<Move> moves = gen.generateALLMoves();
            // plus tard : tri des coups si _options.UseMoveOrdering (mvv-lva, killers, history)

            if (moves.Count == 0)
            {
                var st = _cb.getChessBoardState();
                byte kingSq = st.Trait == ChessEnum.Side.WHITE
                    ? BitOperation.BitScanForward(_cb.GetWKing())
                    : BitOperation.BitScanForward(_cb.GetBKing());
                bool inCheck = _cb.isSquareAttacked(kingSq, st.Trait);
                // mat : score indexe par ply (mat tot prefere) ; pat : nul
                return inCheck ? -Mate + ply : 0;
            }

            int best = -Infinity;
            foreach (var m in moves)
            {
                _cb.MakeMove(m);
                int score = -Negamax(depth - 1, ply + 1, -beta, -alpha);
                _cb.UnMakeMove();
                if (_stop)
                    return 0;
                if (score > best)
                {
                    best = score;
                    // maj de la pv (tableau triangulaire)
                    _pv[ply][0] = m;
                    Array.Copy(_pv[ply + 1], 0, _pv[ply], 1, _pvLength[ply + 1]);
                    _pvLength[ply] = _pvLength[ply + 1] + 1;
                    // plus tard : coupe alpha-beta si _options.UseAlphaBeta et best >= beta
                }
            }
            return best;
        }

        private bool IsInsufficientMaterial()
        {
            ulong sansRois = (_cb.GetWOcc() | _cb.GetBOcc()) & ~(_cb.GetWKing() | _cb.GetBKing());
            if (sansRois == 0)
                return true;
            if (BitOperation.PopCount(sansRois) == 1)
            {
                ulong pieces = _cb.GetWKnight() | _cb.GetBKnight() | _cb.GetWBisho() | _cb.GetBBishop();
                if ((sansRois & pieces) != 0)
                    return true;
            }
            return false;
        }
    }
}
