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

        internal readonly int m, n, mn, mHalf, nHalf, mnHalf;

        private int numberOfVertical = 0, totalSolutionSum = 0;

        internal readonly bool[,] configuration;

        internal readonly bool[,] solution;

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
            MakeRandomSolution();

            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                    if (solution[rowIndex, columnIndex])
                        MoveInConfiguration(rowIndex, columnIndex);
        }

        private void MakeRandomSolution()
        {
            int[] rndMoves = MakeRandRangeArray(0, mn);
            int rndNumberOfMoves = rnd.Next(3, mnHalf + 1);

            foreach (int rndMove in rndMoves)
            {
                AddMoveToSolution(rndMove / n, rndMove % n);
                
                if (totalSolutionSum == rndNumberOfMoves)
                    break;
            }
        }

        private void AddMoveToSolution(int rowIndex, int columnIndex)
        {
            int rowSum = solutionRowSums[rowIndex];
            int columnSum = solutionColumnSums[columnIndex];

            if (rowSum < nHalf && columnSum < mHalf)
                totalSolutionSum += InvertArrayCell(rowIndex, columnIndex, solution);
        }

        internal void Move(int rowIndex, int columnIndex)
        {
            MoveInConfiguration(rowIndex, columnIndex);
            ChangeSolution(rowIndex, columnIndex);
            victory = numberOfVertical == 0 || numberOfVertical == mn;
        }

        private void MoveInConfiguration(int rowIndex, int columnIndex)
        {
            numberOfVertical += Invert(rowIndex, configuration);
            numberOfVertical += Invert(columnIndex, configuration, true);
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
                OptimizeUnevenSolution();
        }
        
        private void OptimizeUnevenSolution()
        {
            bool isOptimized = false;

            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                isOptimized |= TryInvertRowOrColumn(rowIndex);

            for (int columnIndex = 0; columnIndex < n; columnIndex++)
                isOptimized |= TryInvertRowOrColumn(columnIndex, true);

            if (isOptimized) 
                OptimizeUnevenSolution();
        }

        private bool TryInvertRowOrColumn(int index, bool isItColumn = false)
        {
            int maxSumValue = isItColumn ? mHalf : nHalf;
            int[] sumsArray = isItColumn ? solutionColumnSums : solutionRowSums;

            if (sumsArray[index] <= maxSumValue)
                return false;

            totalSolutionSum += Invert(index, solution, isItColumn);
            
            return true;
        }

        private void InvertSolution()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                totalSolutionSum += Invert(rowIndex, solution);
        }

        private int Invert(int index, bool[,] invertibleArray, bool vertically = false)
        {
            int invertedSumChange = 0;
            int maxIndexValue = vertically ? m : n;

            for (int invertedIndex = 0; invertedIndex < maxIndexValue; invertedIndex++)
            {
                int firstIndex = vertically ? invertedIndex : index;
                int secondIndex = vertically ? index : invertedIndex;
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
