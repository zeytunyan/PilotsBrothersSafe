using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe
{
    internal class GameRealizationBitwise
    {
        Random rnd = new();

        internal readonly int m, n, mn;

        private readonly int mHalf, nHalf, mnHalf, mMinOne, nMinOne, mnMinOne;

        internal ulong configuration = 0, solution = 0;

        private ulong filledBoard, twoNPowMinOne, twoMNPowMinOne;

        private int totalSolutionSum = 0;

        private readonly int[] solutionRowSums, solutionColumnSums;

        internal bool victory = false;

        private readonly ulong[] rows, columns;

        private readonly ulong[,] solutionMoves, moves;

        internal GameRealizationBitwise(int n) : this(n, n) { }

        internal GameRealizationBitwise(int m, int n)
        {
            Program.CheckMNArguments(m, n);

            mn = m * n;

            if (mn > 64)
            {
                string errorMessage = "Переданные аргументы недопустимо велики";
                throw new ArgumentOutOfRangeException(errorMessage);
            }

            this.m = m;
            this.n = n;
            mHalf = m / 2;
            nHalf = n / 2;
            mnHalf = mn / 2;
            mMinOne = m - 1;
            nMinOne = n - 1;
            mnMinOne = mn - 1;

            filledBoard = (((1ul << (mn - 1)) - 1ul) << 1) + 1ul;
            twoNPowMinOne = (1ul << n) - 1ul;
            twoMNPowMinOne = (1ul << mn) - 1ul;
            rows = new ulong[m];
            columns = new ulong[n];
            moves = new ulong[m, n];
            solutionMoves = new ulong[m, n];
            solutionRowSums = new int[m];
            solutionColumnSums = new int[n];
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
                rows[rowIndex] = twoNPowMinOne << (n * positionInNum);
            }
        }

        private void MakeColumns()
        {
            for (int columnIndex = 0; columnIndex < n; columnIndex++)
            {
                int positionInNum = nMinOne - columnIndex;
                columns[columnIndex] = (twoMNPowMinOne / twoNPowMinOne) << positionInNum;
            }
        }

        private void MakeRowColumnCrosses()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; rowIndex++)
                    moves[rowIndex, columnIndex] = rows[rowIndex] | columns[columnIndex];
        }

        private void MakeSolutionMoves()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < n; rowIndex++)
                {
                    int positionInNum = mnMinOne - rowIndex * n - columnIndex;
                    solutionMoves[rowIndex, columnIndex] = 1ul << positionInNum;
                }
            }
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

                configuration ^= moves[rowIndex, columnIndex];

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
                MoveToSolution(rowIndex, columnIndex);

            return canBeAdded;
        }

        internal void Move(int rowIndex, int columnIndex)
        {
            configuration ^= moves[rowIndex, columnIndex];
            ChangeSolution(rowIndex, columnIndex);
            victory = configuration == filledBoard || configuration == 0;
        }

        private void ChangeSolution(int rowIndex, int columnIndex)
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

        private void OptimizeSolution(int rowIndex, int columnIndex)
        {
            if (mn % 2 != 0)
                OptimizeUnevenSolution(rowIndex, columnIndex);

            if (totalSolutionSum > mnHalf)
                solution ^= filledBoard;
        }



        private void OptimizeUnevenSolution(int rowIndex, int columnIndex)
        {
            bool isRowInverted = TryInvertRowOrColumn(rowIndex);
            bool isColumnInverted = TryInvertRowOrColumn(columnIndex, true);

            if (isRowInverted || isColumnInverted)
                TryInvertRowsOrColumns(isRowInverted);
        }

        private void TryInvertRowsOrColumns(bool isForColumns)
        {
            int dimensionSize = isForColumns ? n : m;

            bool isOptimized = false;

            for (int index = 0; index < dimensionSize; index++)
                isOptimized |= TryInvertRowOrColumn(index, isForColumns);

            if (isOptimized)
                TryInvertRowsOrColumns(!isForColumns);
        }

        private bool TryInvertRowOrColumn(int index, bool isItColumn = false)
        {
            int maxSumValue = isItColumn ? mHalf : nHalf;
            int[] sumsArray = isItColumn ? solutionColumnSums : solutionRowSums;
            bool canBeInverted = sumsArray[index] > maxSumValue;

            if (canBeInverted)
                totalSolutionSum += Invert(index, isItColumn);

            return canBeInverted;
        }

        private void InvertSolutionRowOrColumn(int index, bool isColumn = false)
        {
            int[] sameDimensionSumArray = isColumn ? solutionColumnSums : solutionRowSums;
            int[] anotherDimensionSumArray = isColumn ? solutionRowSums : solutionColumnSums;
            ulong invertionRowOrColumn = isColumn ? columns[index] : rows[index];

            solution ^= invertionRowOrColumn;
            sameDimensionSumArray[index] = OneBitsNumber(solution & invertionRowOrColumn);
            UpdateSolutionSumsArray(); // параметры добавить

            totalSolutionSum = OneBitsNumber(solution);
        }

        private void UpdateSolutionSumsArray()
        {

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


        private int OneBitsNumber(ulong num)
        {
            num -= (num >> 1) & 0x5555555555555555ul;
            num = ((num >> 2) & 0x3333333333333333ul) + (num & 0x3333333333333333ul);
            num = ((((num >> 4) + num) & 0x0F0F0F0F0F0F0F0Ful) * 0x0101010101010101) >> 56;
            return (int)num; 
        }
    }
}

//private void InvertSolution()
//{
//    for (int rowIndex = 0; rowIndex < m; rowIndex++)
//        totalSolutionSum += Invert(rowIndex, solution);
//}


//// i - строка от 0, j - столбец от 0, m - количество строк, n - количество столбцов
//static ulong Formula(int i, int j, int m, int n = 0)
//{
//    if (n == 0)
//    {
//        n = m;
//    }

//    int mn = m * n;

//    if (mn > 63 || i >= m || j >= n)
//    {
//        throw new ArgumentOutOfRangeException("Переданные аргументы недопустимо велики");
//    }
//    else if (i < 0 || j < 0 || m < 2 || n < 2)
//    {
//        throw new ArgumentOutOfRangeException("Переданные аргументы недопустимо малы");
//    }

//    int positionIInNumber = m - i - 1;
//    int positionJInNumber = n - j - 1;

//    ulong twoInNPowMimusOne = (1ul << n) - 1ul;
//    ulong twoInNMPowMinusOne = (1ul << mn) - 1ul;
//    ulong row = twoInNPowMimusOne << (n * positionIInNumber);
//    ulong column = (twoInNMPowMinusOne / twoInNPowMimusOne) << positionJInNumber;

//    return column | row;
//}
//}
