namespace pignouf2.search
{
    internal class SearchOptions
    {
        // profondeur fixe demandee (go depth N), ou profondeur par defaut si pas de depth
        public int MaxDepth = 4;
        public int DefaultDepth = 4;
        public bool Infinite = false;

        public bool UseAlphaBeta = false;      // coupes alpha-beta dans Negamax
        public bool UseQuiescence = false;     // qsearch a depth 0 (effet horizon)
        public bool UseMoveOrdering = false;   // mvv-lva / killers / history dans OrderMoves
        public bool UseTT = false;             // table de transposition
        public bool UseZobristKey = false;     // cle zobrist + detection de repetition
    }
}
