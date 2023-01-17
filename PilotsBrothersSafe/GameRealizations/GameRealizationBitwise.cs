using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe.GameRealizations
{
    // Реализация с использованием ulong-чисел и побитовых операций
    internal class GameRealizationBitwise : GameRealization
    {
        // Данные для работы
        private readonly int mMinOne, nMinOne, mnMinOne;
        private ulong configuration = 0, solution = 0;
        private readonly ulong filledConfiguration, twoNPowMinOne;

        private readonly ulong[] rows, columns;
        private readonly ulong[,] solutionMoves, moves;

        // Реализация свойств для взаимодействия с другими классами
        internal override bool[,] Configuration => UlongToBoolArray(configuration);
        internal override bool[,] Solution => UlongToBoolArray(solution);
        internal override bool Victory => 
            configuration == filledConfiguration || configuration == 0;

        internal GameRealizationBitwise(int n) : this(n, n) { }

        internal GameRealizationBitwise(int m, int n) : base(m, n)
        {
            // Проверка, чтобы поле не было больше, чем может поместиться в 64 бита
            if (mn > 64)
            {
                string errorMessage = "Переданные аргументы недопустимо велики";
                throw new ArgumentOutOfRangeException(errorMessage);
            }

            mMinOne = m - 1;
            nMinOne = n - 1;
            mnMinOne = mn - 1;
            twoNPowMinOne = (1ul << n) - 1ul;

            // То же, что и (1ul << mn) - 1ul
            // Так сделано, чтобы влезть в 64 бита
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

        // Далее следуют 4 метода, создающие массивы с числами, требуемыми для различных 
        // побитовых операций, соответствующих тем или иным преобразованиям поля или решения 

        // Строки
        private void MakeRows()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                int positionInNum = mMinOne - rowIndex;
                rows[rowIndex] = twoNPowMinOne << n * positionInNum;
            }
        }

        // Столбцы
        private void MakeColumns()
        {
            for (int columnIndex = 0; columnIndex < n; columnIndex++)
            {
                int positionInNum = nMinOne - columnIndex;
                columns[columnIndex] = filledConfiguration / twoNPowMinOne << positionInNum;
            }
        }

        // Соответствующие ходам пересечения строк и столбцов
        private void MakeRowColumnCrosses()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                    moves[rowIndex, columnIndex] = rows[rowIndex] | columns[columnIndex];
        }

        // Сами ходы (отдельные позиции в числе, установленные в 1)
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

        private protected override void MoveInConfiguration(int rowIndex, int columnIndex) => 
            configuration ^= moves[rowIndex, columnIndex];

        // Ход в решении
        private protected override void MoveToSolution(int rowIndex, int columnIndex)
        {
            ulong moveToSolution = solutionMoves[rowIndex, columnIndex];
            solution ^= moveToSolution;

            // Пересчет сумм ходов в решении
            int changeInSums = (solution & moveToSolution) == 0 ? -1 : 1;
            solutionRowSums[rowIndex] += changeInSums;
            solutionColumnSums[columnIndex] += changeInSums;
            totalSolutionSum += changeInSums;
        }

        // Инвертирование какой-либо строки или столбца решения
        private protected override void InvertSolutionRowOrColumn(int index, bool isColumn)
        {
            int[] sameDimensionSumArray = isColumn ? solutionColumnSums : solutionRowSums; ;
            ulong invertionRowOrColumn = isColumn ? columns[index] : rows[index];

            solution ^= invertionRowOrColumn;

            // Пересчет сумм ходов в решении
            sameDimensionSumArray[index] = OneBitsNumber(solution & invertionRowOrColumn);
            UpdateSolutionSumsArray(isColumn);
            totalSolutionSum = OneBitsNumber(solution);
        }

        // Инвертирование всего решения
        private protected override void InvertSolution() =>
            solution ^= filledConfiguration;

        // Пересчитывает количество ходов в строках или столбцах решения
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

        // Подсчитывает количество единичных битов в ulong-числе
        private int OneBitsNumber(ulong num)
        {
            num -= num >> 1 & 0x5555555555555555ul;
            num = (num >> 2 & 0x3333333333333333ul) + (num & 0x3333333333333333ul);
            num = ((num >> 4) + num & 0x0F0F0F0F0F0F0F0Ful) * 0x0101010101010101 >> 56;
            return (int)num;
        }

        // Преобразование ulong-числа в двумерный массив для передачи другим классам
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
