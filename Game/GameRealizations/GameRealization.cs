using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe.GameRealizations
{
    // Абстрактный класс с общими для обеих реализаций полями и методами
    internal abstract class GameRealization
    {
        // Данные для вычислений
        private Random rnd = new();
        private protected readonly int m, n, mn, mHalf, nHalf, mnHalf;
        private protected int totalSolutionSum = 0;
        private protected readonly int[] solutionRowSums, solutionColumnSums;

        // Свойства для взаимодействия с реализацией
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

        // Создание случайного поля
        private protected void MakeRandomConfiguration()
        {
            // Перемешанный массив возможных ходов
            int[] rndMoves = MakeRandRangeArray(0, mn);

            // Вычисляется минимум три хода, чтобы игрок сразу не выиграл
            int rndNumberOfMoves = rnd.Next(3, mnHalf + 1);

            foreach (int rndMove in rndMoves)
            {
                int rowIndex = rndMove / n;
                int columnIndex = rndMove % n;

                // Если ход соответствует требованиям, он добавляется в массив с решением
                if (!TryAddMoveToSolution(rowIndex, columnIndex))
                    continue;

                // Если ход добавлен в массив с решением, он добавляется и на поле
                MoveInConfiguration(rowIndex, columnIndex);

                // Когда в решение добавлено достаточно ходов, цикл заканчивается
                if (totalSolutionSum == rndNumberOfMoves)
                    break;
            }
        }

        // Попытка добавить ход в решение
        private protected bool TryAddMoveToSolution(int rowIndex, int columnIndex)
        {
            int rowSum = solutionRowSums[rowIndex];
            int columnSum = solutionColumnSums[columnIndex];

            // Ход добавляется в решение, только если данная строка
            // и столбец заняты ходами менее, чем наполовину. 
            bool canBeAdded = rowSum < nHalf && columnSum < mHalf;

            if (canBeAdded)
                MoveToSolution(rowIndex, columnIndex);

            return canBeAdded;
        }

        private protected abstract void MoveInConfiguration(int rowIndex, int columnIndex);

        // Ход
        internal void Move(int rowIndex, int columnIndex)
        {
            MoveInConfiguration(rowIndex, columnIndex);
            ChangeSolution(rowIndex, columnIndex);
        }


        private protected void ChangeSolution(int rowIndex, int columnIndex)
        {
            MoveToSolution(rowIndex, columnIndex);
            OptimizeSolution(rowIndex, columnIndex);
        }


        private protected abstract void MoveToSolution(int rowIndex, int columnIndex);

        private protected abstract void InvertSolutionRowOrColumn(int index, bool isColumn);
        // Алгоритм оптимизации подсказки
        private protected void OptimizeSolution(int rowIndex, int columnIndex)
        {
            // Более серьезная оптимизация для полей с обеими нечетными размерностями
            if (mn % 2 != 0)
                OptimizeUnevenSolution(rowIndex, columnIndex);

            // Если подсказка включает в себя больше половины
            // от числа всех возможных ходов, то она инвертируется
            if (totalSolutionSum > mnHalf)
                InvertSolution();
        }

        // Оптимизация подсказки для нечетных полей
        private void OptimizeUnevenSolution(int rowIndex, int columnIndex)
        {
            // Попытка инвертировать строку или столбец
            bool isRowInverted = TryInvertRowOrColumn(rowIndex);
            bool isColumnInverted = TryInvertRowOrColumn(columnIndex, true);

            // Если инвертируется одна размерность, то другая тоже меняется
            // теперь нужно проверить, что ещё можно оптимизировать
            if (isRowInverted || isColumnInverted)
                UseTryInvertForAllRowsOrColumns(isRowInverted);
        }

        // Оптимизация происходит рекурсивно, пока все не будет оптимизировано
        private void UseTryInvertForAllRowsOrColumns(bool isColumns)
        {
            int dimensionSize = isColumns ? n : m;

            bool isOptimized = false;

            for (int index = 0; index < dimensionSize; index++)
                isOptimized |= TryInvertRowOrColumn(index, isColumns);

            if (isOptimized)
                UseTryInvertForAllRowsOrColumns(!isColumns);
        }

        private protected abstract void InvertSolution();

        // Строка или столбец решения инвертируются, если они более,
        // чем наполовину, заняты сделанными ходами.
        private protected bool TryInvertRowOrColumn(int index, bool isColumn = false)
        {
            int maxSumValue = isColumn ? mHalf : nHalf;
            int[] sumsArray = isColumn ? solutionColumnSums : solutionRowSums;
            bool canBeInverted = sumsArray[index] > maxSumValue;

            if (canBeInverted)
                InvertSolutionRowOrColumn(index, isColumn);

            return canBeInverted;
        }


        // Методы для создания массива со случайно расположенными неповторяющимися числами
        // Это будет массив возможных ходов
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
