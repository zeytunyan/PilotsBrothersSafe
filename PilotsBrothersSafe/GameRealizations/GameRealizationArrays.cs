using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe.GameRealizations
{
    internal class GameRealizationArrays : GameRealization
    {
        private readonly bool[,] configuration, solution;

        private int numberOfVertical = 0;

        internal override bool[,] Configuration => configuration;
        internal override bool[,] Solution => solution;
        internal override bool Victory => 
            numberOfVertical == mn || numberOfVertical == 0;

        internal GameRealizationArrays(int n) : this(n, n) { }

        internal GameRealizationArrays(int m, int n) : base(m, n)
        {
            configuration = new bool[m, n];
            solution = new bool[m, n];

            MakeRandomConfiguration();
        }

        private protected override bool TryAddMoveToSolution(int rowIndex, int columnIndex)
        {
            int rowSum = solutionRowSums[rowIndex];
            int columnSum = solutionColumnSums[columnIndex];
            bool canBeAdded = rowSum < nHalf && columnSum < mHalf;

            if (canBeAdded)
                InvertArrayCell(rowIndex, columnIndex, solution);

            return canBeAdded;
        }

        private protected override void MoveInConfiguration(int rowIndex, int columnIndex)
        {
            InvertRowOrColumn(rowIndex, configuration);
            InvertRowOrColumn(columnIndex, configuration, true);
            InvertArrayCell(rowIndex, columnIndex, configuration);
        }

        private protected override void ChangeSolution(int rowIndex, int columnIndex)
        {
            InvertArrayCell(rowIndex, columnIndex, solution);
            OptimizeSolution(rowIndex, columnIndex);
        }

        private protected override bool TryInvertRowOrColumn(int index, bool isColumn = false)
        {
            int maxSumValue = isColumn ? mHalf : nHalf;
            int[] sumsArray = isColumn ? solutionColumnSums : solutionRowSums;
            bool canBeInverted = sumsArray[index] > maxSumValue;

            if (canBeInverted)
                InvertRowOrColumn(index, solution, isColumn);

            return canBeInverted;
        }

        private void InvertRowOrColumn(int index, bool[,] invertibleArray, bool isColumn = false)
        {
            int dimensionSize = isColumn ? m : n;

            for (int invertedIndex = 0; invertedIndex < dimensionSize; invertedIndex++)
            {
                int firstIndex = isColumn ? invertedIndex : index;
                int secondIndex = isColumn ? index : invertedIndex;
                InvertArrayCell(firstIndex, secondIndex, invertibleArray);
            }
        }

        private void InvertArrayCell(int rowIndex, int columnIndex, bool[,] array)
        {
            array[rowIndex, columnIndex] ^= true;
            int changeInSums = array[rowIndex, columnIndex] ? 1 : -1;

            if (array == configuration)
            {
                numberOfVertical += changeInSums;
                return;
            }

            solutionRowSums[rowIndex] += changeInSums;
            solutionColumnSums[columnIndex] += changeInSums;
            totalSolutionSum += changeInSums;
        }

        private protected override void InvertSolution()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                InvertRowOrColumn(rowIndex, solution);
        }

    }
}
