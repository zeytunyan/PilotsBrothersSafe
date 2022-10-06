using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe.GameRealizations
{
    internal abstract class GameRealization
    {
        private protected Random rnd = new();

        private protected readonly int m, n, mn, mHalf, nHalf, mnHalf;
        private protected int totalSolutionSum = 0;
        private protected readonly int[] solutionRowSums, solutionColumnSums;

        internal abstract bool[,] Configuration { get; }
        internal abstract bool[,] Solution { get; }
        internal abstract bool Victory { get; }

        internal GameRealization(int m, int n)
        {
            Program.CheckMNArguments(m, n);

            this.m = m;
            this.n = n;
            mn = m * n;
            mHalf = m / 2;
            nHalf = n / 2;
            mnHalf = mn / 2;
            solutionRowSums = new int[m];
            solutionColumnSums = new int[n];
        }

        private protected void MakeRandomConfiguration()
        {
            int[] rndMoves = MakeRandRangeArray(0, mn);
            int rndNumberOfMoves = rnd.Next(3, mnHalf + 1);

            foreach (int rndMove in rndMoves)
            {
                int rowIndex = rndMove / n;
                int columnIndex = rndMove % n;

                if (!TryAddMoveToSolution(rowIndex, columnIndex))
                    continue;

                MoveInConfiguration(rowIndex, columnIndex);

                if (totalSolutionSum == rndNumberOfMoves)
                    break;
            }
        }

        private protected abstract bool TryAddMoveToSolution(int rowIndex, int columnIndex);

        private protected abstract void MoveInConfiguration(int rowIndex, int columnIndex);

        internal void Move(int rowIndex, int columnIndex)
        {
            MoveInConfiguration(rowIndex, columnIndex);
            ChangeSolution(rowIndex, columnIndex);
        }

        private protected abstract void ChangeSolution(int rowIndex, int columnIndex);

        private protected void OptimizeSolution(int rowIndex, int columnIndex)
        {
            if (mn % 2 != 0)
                OptimizeUnevenSolution(rowIndex, columnIndex);

            if (totalSolutionSum > mnHalf)
                InvertSolution();
        }

        private protected void OptimizeUnevenSolution(int rowIndex, int columnIndex)
        {
            bool isRowInverted = TryInvertRowOrColumn(rowIndex);
            bool isColumnInverted = TryInvertRowOrColumn(columnIndex, true);

            if (isRowInverted || isColumnInverted)
                UseTryInvertForAllRowsOrColumns(isRowInverted);
        }

        private protected void UseTryInvertForAllRowsOrColumns(bool isColumns)
        {
            int dimensionSize = isColumns ? n : m;

            bool isOptimized = false;

            for (int index = 0; index < dimensionSize; index++)
                isOptimized |= TryInvertRowOrColumn(index, isColumns);

            if (isOptimized)
                UseTryInvertForAllRowsOrColumns(!isColumns);
        }

        private protected abstract void InvertSolution();

        private protected abstract bool TryInvertRowOrColumn(int index, bool isColumn = false);


        private protected int[] MakeRandRangeArray(int start, int count)
        {
            int[] rangeArray = Enumerable.Range(start, count).ToArray();
            RandomizeArray(rangeArray);
            return rangeArray;
        }

        private void RandomizeArray(int[] randomizedArray)
        {
            double[] rndOrder = new double[randomizedArray.Length];

            for (int rndIndex = 0; rndIndex < rndOrder.Length; rndIndex++)
                rndOrder[rndIndex] = rnd.NextDouble();

            Array.Sort(rndOrder, randomizedArray);
        }
    }
}
