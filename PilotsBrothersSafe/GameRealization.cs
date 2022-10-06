using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe
{
    internal class GameRealization
    {
        Random rnd = new();

        internal readonly int m, n, mn;
        
        private readonly int mHalf, nHalf, mnHalf;

        internal readonly bool[,] configuration, solution;

        private int numberOfVertical = 0, totalSolutionSum = 0;

        private readonly int[] solutionRowSums, solutionColumnSums;

        internal bool victory = false;

        internal GameRealization(int n) : this(n, n) { }

        internal GameRealization(int m, int n)
        {
            Program.CheckMNArguments(m, n);

            this.m = m;
            this.n = n;
            mn = m * n;
            mHalf = m / 2;
            nHalf = n / 2;
            mnHalf = mn / 2;
            configuration = new bool[m, n];
            solution = new bool[m, n];
            solutionRowSums = new int[m];
            solutionColumnSums = new int[n];
            MakeRandomConfiguration();
        }

        private void MakeRandomConfiguration()
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

        private bool TryAddMoveToSolution(int rowIndex, int columnIndex)
        {
            int rowSum = solutionRowSums[rowIndex];
            int columnSum = solutionColumnSums[columnIndex];
            bool canBeAdded = rowSum < nHalf && columnSum < mHalf;

            if (canBeAdded)
                totalSolutionSum += InvertArrayCell(rowIndex, columnIndex, solution);

            return canBeAdded;
        }

        internal void Move(int rowIndex, int columnIndex)
        {
            MoveInConfiguration(rowIndex, columnIndex);
            ChangeSolution(rowIndex, columnIndex);
            victory = numberOfVertical == 0 || numberOfVertical == mn;
        }

        private void MoveInConfiguration(int rowIndex, int columnIndex)
        {
            numberOfVertical += InvertRowOrColumn(rowIndex, configuration);
            numberOfVertical += InvertRowOrColumn(columnIndex, configuration, true);
            numberOfVertical += InvertArrayCell(rowIndex, columnIndex, configuration);
        }

        private void ChangeSolution(int rowIndex, int columnIndex)
        {
            totalSolutionSum += InvertArrayCell(rowIndex, columnIndex, solution);
            OptimizeSolution(rowIndex, columnIndex);
        }

        private void OptimizeSolution(int rowIndex, int columnIndex)
        {
            if (mn % 2 != 0)
                OptimizeUnevenSolution(rowIndex, columnIndex);

            if (totalSolutionSum > mnHalf)
                InvertSolution();
        }

        private void OptimizeUnevenSolution(int rowIndex, int columnIndex)
        {
            bool isRowInverted = TryInvertRowOrColumn(rowIndex);
            bool isColumnInverted = TryInvertRowOrColumn(columnIndex, true);

            if (isRowInverted || isColumnInverted)
                UseTryInvertForAllRowsOrColumns(isRowInverted);
        }
        
        private void UseTryInvertForAllRowsOrColumns(bool isColumns)
        {
            int dimensionSize = isColumns ? n : m;

            bool isOptimized = false;

            for (int index = 0; index < dimensionSize; index++)
                isOptimized |= TryInvertRowOrColumn(index, isColumns);

            if (isOptimized)
                UseTryInvertForAllRowsOrColumns(!isColumns);
        }

        private bool TryInvertRowOrColumn(int index, bool isColumn = false)
        {
            int maxSumValue = isColumn ? mHalf : nHalf;
            int[] sumsArray = isColumn ? solutionColumnSums : solutionRowSums;
            bool canBeInverted = sumsArray[index] > maxSumValue;

            if (canBeInverted)
                totalSolutionSum += InvertRowOrColumn(index, solution, isColumn);

            return canBeInverted;
        }

        private void InvertSolution()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                totalSolutionSum += InvertRowOrColumn(rowIndex, solution);
        }

        private int InvertRowOrColumn(int index, bool[,] invertibleArray, bool isColumn = false)
        {
            int invertedSumChange = 0;
            int dimensionSize = isColumn ? m : n;

            for (int invertedIndex = 0; invertedIndex < dimensionSize; invertedIndex++)
            {
                int firstIndex = isColumn ? invertedIndex : index;
                int secondIndex = isColumn ? index : invertedIndex;
                invertedSumChange += InvertArrayCell(firstIndex, secondIndex, invertibleArray);
            }
            
            return invertedSumChange;
        }

        private int InvertArrayCell(int rowIndex, int columnIndex, bool[,] array)
        {
            array[rowIndex, columnIndex] ^= true;
            int changeInSums = array[rowIndex, columnIndex] ? 1 : -1;
            
            if (array == solution)
            {
                solutionRowSums[rowIndex] += changeInSums;
                solutionColumnSums[columnIndex] += changeInSums;
            }

            return changeInSums;
        }

        private int[] MakeRandRangeArray(int start, int count)
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
