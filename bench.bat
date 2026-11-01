@echo off
REM build en Release puis bench (perft + search, 2 lancers par cas, meilleur nps retenu)
cd /d %~dp0
dotnet build pignouf2.sln -c Release
if errorlevel 1 (
    echo build en echec
    exit /b 1
)
(echo bench & echo quit) | pignouf2\bin\Release\net10.0\pignouf2.exe
