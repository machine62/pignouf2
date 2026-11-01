# bench pignouf2 — 2026-11-01 18:58:30 — .NET 10.0.12 — X64

repeats=2 (meilleur nps retenu) — search depth fixe=4

## perft

| pos | depth | nodes | expected | ok | time_ms | nps |
|---|---|---|---|---|---|---|
| startpos | 5 | 4865609 | 4865609 | OK | 655 | 7428410 |
| kiwipete | 4 | 4085603 | 4085603 | OK | 618 | 6611008 |
| pos3-pep | 5 | 674624 | 674624 | OK | 97 | 6954886 |
| pos4-promo | 4 | 422333 | 422333 | OK | 66 | 6398984 |
| pos5 | 3 | 62379 | 62379 | OK | 8 | 7797375 |
| pos6 | 4 | 3894594 | 3894594 | OK | 584 | 6668825 |

**perft TOTAL** : 14005142 nodes en 2028 ms → **6905888 nps** — ok 6/6

## search (depth 4)

| pos | nodes | time_ms | nps | bestmove | score | branch |
|---|---|---|---|---|---|---|
| startpos | 216369 | 88 | 2458738 | e2e3 | -6 | 21,6 |
| kiwipete | 4287640 | 1312 | 3268018 | e2a6 | 33 | 45,5 |
| pos4-promo | 442087 | 138 | 3203528 | g1h1 | -628 | 25,8 |
| pos6 | 4080799 | 1318 | 3096205 | e2d1 | -92 | 44,9 |
| mat1 | 2685 | 0 | 2685000 | h1h8 | 31999 | 7,2 |
| tactique | 1385348 | 444 | 3120153 | h5f7 | 31999 | 34,3 |

**search TOTAL (d4)** : 10414928 nodes en 3300 ms → **3156038 nps**

