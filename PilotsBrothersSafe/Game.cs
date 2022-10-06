using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PilotsBrothersSafe.GameRealizations;

namespace PilotsBrothersSafe
{
    internal class Game
    {
        internal readonly int m, n;
        private readonly GameRealization gameRealization;

        internal List<int>[,] Moves { get; private set; }
        internal int NumberOfMoves { get; private set; } = 0;
        internal List<int>? Solution { get; private set; }
        internal bool[,] Configuration => gameRealization.Configuration;

        internal bool Victory => gameRealization.Victory;

        internal Game(int n) : this(n, n) { }

        internal Game(int m, int n)
        {
            Program.CheckMNArguments(m, n);

            this.m = m;
            this.n = n;
            Moves = new List<int>[m, n];
            MakeMoves();
            
            gameRealization = m * n < 65 ? 
                new GameRealizationBitwise(m, n) : 
                new GameRealizationArrays(m, n);
            
            PullSolution();
        }

        private void MakeMoves()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                    Moves[rowIndex, columnIndex] = MakeMove(rowIndex, columnIndex);
        }

        private List<int> MakeMove(int rowIndex, int columnIndex)
        {
            List<int> move = new();

            for (int i = 0; i < m; i++)
                if (i == rowIndex)
                    move.AddRange(Enumerable.Range(i * n, n));
                else
                    move.Add(i * n + columnIndex);

            return move;
        }

        internal void Move(int rowIndex, int columnIndex)
        {
            NumberOfMoves++;
            gameRealization.Move(rowIndex, columnIndex);
            PullSolution();
        }

        private void PullSolution()
        {
            Solution = new();

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (gameRealization.Solution[i, j])
                        Solution.Add(i * n + j);
        }
    }
}
