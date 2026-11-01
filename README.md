# pignouf2

Moteur d'échecs UCI en C# / .NET 10 (bitboards 12x64 bits + magic bitboards, make/unmake historisé).

## Build et lancement

```bat
dotnet build pignouf2.sln -c Release
pignouf2\bin\Release\net10.0\pignouf2.exe                  rem mode interactif UCI (commandes sur stdin)
pignouf2\bin\Release\net10.0\pignouf2.exe bench save v1      rem commande directement en argument
bench.bat                                                   rem build Release + bench
```

## Commandes

### Protocole UCI (standard)

`uci`, `isready`, `ucinewgame`, `setoption`, `position [startpos|fen <FEN>] [moves <coups>]`, `go ...`, `stop`, `quit`.

Exemple : `position startpos moves e2e4 e7e5` puis `go depth 5` → lignes `info ...` puis `bestmove ...`.

### Commandes internes (debug / tests / benchs)

| commande | effet |
|---|---|
| `perft N` | perft à profondeur N depuis la position courante |
| `divide N` | perft détaillé coup par coup |
| `test` | suite perft CPW (20 tests, 6 positions de référence) |
| `bench [depth N] [save <nom>]` | banc perft + search à profondeur N fixe (défaut 4), rapport markdown dans `bench\<nom>.md` |
| `sts [depth N] [save <nom>]` | test de force STS (1300 positions, profondeur fixe), score par thème + Elo estimé, rapport dans `bench\sts-<nom>.md` |
| `ShowBoard` | affiche le plateau |

Exemples en ligne de commande :

```bat
.\pignouf2.exe perft 5
.\pignouf2.exe bench save v1
.\pignouf2.exe sts depth 4 save v1
```

## Tests de non-régression

- `test` : perft exact sur les 6 positions CPW (doit être 20/20 SUCCESS).
- `bench` : les nœuds perft doivent rester identiques entre versions (ok=6/6), le nps mesure la vitesse.
- `sts depth N` : même profondeur fixe entre versions pour comparer sans biais ; le rapport indique `depth=N` en en-tête.
