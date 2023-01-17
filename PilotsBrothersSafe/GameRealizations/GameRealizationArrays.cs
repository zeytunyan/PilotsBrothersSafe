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
    // Реализация с использованием массивов и циклов
    internal class GameRealizationArrays : GameRealization
    {
        // Данные для работы
        private readonly bool[,] configuration, solution;
        private int numberOfVertical = 0;

        // Реализация свойств для взаимодействия с другими классами
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

        private protected override void MoveInConfiguration(int rowIndex, int columnIndex)
        {
            InvertRowOrColumn(rowIndex, configuration);
            InvertRowOrColumn(columnIndex, configuration, true);
            InvertArrayCell(rowIndex, columnIndex, configuration);
        }

        // Ход в решении
        private protected override void MoveToSolution(int rowIndex, int columnIndex) => 
            InvertArrayCell(rowIndex, columnIndex, solution);


        private protected override void InvertSolutionRowOrColumn(int index, bool isColumn) =>
            InvertRowOrColumn(index, solution, isColumn);

        // Инвертирование какой-либо строки или столбца
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

        // Инвертирование конкретной ячейки массива
        private void InvertArrayCell(int rowIndex, int columnIndex, bool[,] array)
        {
            array[rowIndex, columnIndex] ^= true;
            int changeInSums = array[rowIndex, columnIndex] ? 1 : -1;


            // После инвертирования пересчитываются все суммы, которые необходимо 
            if (array == configuration)
            {
                numberOfVertical += changeInSums;
                return;
            }

            solutionRowSums[rowIndex] += changeInSums;
            solutionColumnSums[columnIndex] += changeInSums;
            totalSolutionSum += changeInSums;
        }

        // Инвертирование всего решения
        private protected override void InvertSolution()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                InvertRowOrColumn(rowIndex, solution);
        }
    }
}
