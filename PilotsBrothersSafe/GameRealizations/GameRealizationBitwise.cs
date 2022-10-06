using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe.GameRealizations
{
    internal class GameRealizationBitwise : GameRealization
    {
        private readonly int mMinOne, nMinOne, mnMinOne;
        private ulong configuration = 0, solution = 0;
        private readonly ulong filledConfiguration, twoNPowMinOne;

        private readonly ulong[] rows, columns;
        private readonly ulong[,] solutionMoves, moves;

        internal override bool[,] Configuration => UlongToBoolArray(configuration);
        internal override bool[,] Solution => UlongToBoolArray(solution);
        internal override bool Victory => 
            configuration == filledConfiguration || configuration == 0;

        internal GameRealizationBitwise(int n) : this(n, n) { }

        internal GameRealizationBitwise(int m, int n) : base(m, n)
        {
            if (mn > 64)
            {
                string errorMessage = "Переданные аргументы недопустимо велики";
                throw new ArgumentOutOfRangeException(errorMessage);
            }

            mMinOne = m - 1;
            nMinOne = n - 1;
            mnMinOne = mn - 1;
            twoNPowMinOne = (1ul << n) - 1ul;
            filledConfiguration = ((1ul << mn - 1) - 1ul << 1) + 1ul;
            
            rows = new ulong[m];
            columns = new ulong[n];
            moves = new ulong[m, n];
            solutionMoves = new ulong[m, n];
            
            MakeRows();
            MakeColumns();
            MakeRowColumnCrosses();
            MakeSolutionMoves();
            MakeRandomConfiguration();
        }

        private void MakeRows()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                int positionInNum = mMinOne - rowIndex;
                rows[rowIndex] = twoNPowMinOne << n * positionInNum;
            }
        }

        private void MakeColumns()
        {
            for (int columnIndex = 0; columnIndex < n; columnIndex++)
            {
                int positionInNum = nMinOne - columnIndex;
                columns[columnIndex] = filledConfiguration / twoNPowMinOne << positionInNum;
            }
        }

        private void MakeRowColumnCrosses()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                    moves[rowIndex, columnIndex] = rows[rowIndex] | columns[columnIndex];
        }

        private void MakeSolutionMoves()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                {
                    int positionInNum = mnMinOne - rowIndex * n - columnIndex;
                    solutionMoves[rowIndex, columnIndex] = 1ul << positionInNum;
                }
            }
        }

        private protected override bool TryAddMoveToSolution(int rowIndex, int columnIndex)
        {
            int rowSum = solutionRowSums[rowIndex];
            int columnSum = solutionColumnSums[columnIndex];
            bool canBeAdded = rowSum < nHalf && columnSum < mHalf;

            if (canBeAdded)
                MoveToSolution(rowIndex, columnIndex);

            return canBeAdded;
        }

        private protected override void MoveInConfiguration(int rowIndex, int columnIndex) => 
            configuration ^= moves[rowIndex, columnIndex];

        private protected override void ChangeSolution(int rowIndex, int columnIndex)
        {
            MoveToSolution(rowIndex, columnIndex);
            OptimizeSolution(rowIndex, columnIndex);
        }

        private void MoveToSolution(int rowIndex, int columnIndex)
        {
            ulong moveToSolution = solutionMoves[rowIndex, columnIndex];
            solution ^= moveToSolution;
            int changeInSums = (solution & moveToSolution) == 0 ? -1 : 1;
            solutionRowSums[rowIndex] += changeInSums;
            solutionColumnSums[columnIndex] += changeInSums;
            totalSolutionSum += changeInSums;
        }

        private protected override bool TryInvertRowOrColumn(int index, bool isColumn = false)
        {
            int maxSumValue = isColumn ? mHalf : nHalf;
            int[] sumsArray = isColumn ? solutionColumnSums : solutionRowSums;
            bool canBeInverted = sumsArray[index] > maxSumValue;

            if (canBeInverted)
                InvertSolutionRowOrColumn(index, isColumn);

            return canBeInverted;
        }

        private void InvertSolutionRowOrColumn(int index, bool isColumn)
        {
            int[] sameDimensionSumArray = isColumn ? solutionColumnSums : solutionRowSums; ;
            ulong invertionRowOrColumn = isColumn ? columns[index] : rows[index];

            solution ^= invertionRowOrColumn;
            sameDimensionSumArray[index] = OneBitsNumber(solution & invertionRowOrColumn);
            UpdateSolutionSumsArray(isColumn);
            totalSolutionSum = OneBitsNumber(solution);
        }

        private protected override void InvertSolution() =>
            solution ^= filledConfiguration;

        private void UpdateSolutionSumsArray(bool isRowSums)
        {
            int[] updatedSumsArray = isRowSums ? solutionRowSums : solutionColumnSums;
            ulong[] rowsOrColumns = isRowSums ? rows : columns;

            for (int index = 0; index < updatedSumsArray.Length; index++)
            {
                ulong rowOrColumnToCountSum = rowsOrColumns[index] & solution;
                updatedSumsArray[index] = OneBitsNumber(rowOrColumnToCountSum);
            }
        }

        private int OneBitsNumber(ulong num)
        {
            num -= num >> 1 & 0x5555555555555555ul;
            num = (num >> 2 & 0x3333333333333333ul) + (num & 0x3333333333333333ul);
            num = ((num >> 4) + num & 0x0F0F0F0F0F0F0F0Ful) * 0x0101010101010101 >> 56;
            return (int)num;
        }

        private bool[,] UlongToBoolArray(ulong ulNum)
        {
            bool[,] boolArray = new bool[m, n];

            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                {
                    ulong ulongPositionValue = solutionMoves[rowIndex, columnIndex] & ulNum;
                    boolArray[rowIndex, columnIndex] = ulongPositionValue != 0;
                }
            }

            return boolArray;
        }
    }
}
