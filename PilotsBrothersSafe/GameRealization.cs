using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsBrothersSafe
{
    internal class GameRealization
    {
        Random rnd = new();

        internal readonly int m, n, mn, mnHalf;

        internal readonly bool[,] configuration;

        internal readonly bool[,] solution;

        internal bool victory = false;

        internal GameRealization(int n) : this(n, n) { }

        internal GameRealization(int m, int n)
        {
            if (m < 2 || n < 2)
            {
                string errorMessage = "Переданные аргументы недопустимо малы";
                throw new ArgumentOutOfRangeException(errorMessage);
            }

            this.m = m;
            this.n = n;
            mn = m * n;
            mnHalf = mn / 2;
            solution = new bool[m, n];
            configuration = new bool[m, n];
            MakeRandomConfiguration();
        }





        private void MakeRandomConfiguration()
        {
            MakeRandomSolution();

            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                for (int columnIndex = 0; columnIndex < n; columnIndex++)
                    if (solution[rowIndex, columnIndex])
                        InvertRowColumn(rowIndex, columnIndex, configuration);
        }

        private void MakeRandomSolution()
        {
            int[] rndMoves = MakeRandRangeArray(0, mn);
            int rndNumberOfMoves = rnd.Next(3, mnHalf + 1);
            int madeMovesNumber = 0;

            foreach (int rndMove in rndMoves)
            {
                int rowIndex = rndMove / n;
                int columnIndex = rndMove % n;

                if (!isGoodMove(rowIndex, columnIndex, rndNumberOfMoves))
                    continue;

                solution[rowIndex, columnIndex] = true;
                madeMovesNumber++;

                if (madeMovesNumber == rndNumberOfMoves)
                    break;
            }
        }

        private bool isGoodMove(int rowIndex, int columnIndex, int numberOfMoves)
        {
            int rowSum = RowSum(rowIndex, solution);
            int columnSum = ColumnSum(columnIndex, solution);
            int m1 = m - 1;
            int n1 = n - 1;

            if (rowSum == n1 || columnSum == m1)
                return false;

            if (numberOfMoves == m1 && columnSum == m1 - 1)
                return false;

            if (numberOfMoves == n1 && rowSum == n1 - 1)
                return false;

            return true;
        }




        internal void Move(int rowIndex, int columnIndex)
        {
            InvertRowColumn(rowIndex, columnIndex, configuration);
            ChangeSolution(rowIndex, columnIndex);
            SetVictory();
        }

        private void SetVictory()
        {
            int numberOfVertical = ArraySum(configuration);
            victory = numberOfVertical == 0 || numberOfVertical == mn;
        }





        //!!!
        private void ChangeSolution(int rowIndex, int columnIndex)
        {
            solution[rowIndex, columnIndex] ^= true;

            if (ArraySum(solution) > mnHalf)
                InvertArray(solution);

            if (mn % 2 != 0)
                OptimizeSolution();
        }

        //!!!
        private void OptimizeSolution()
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
            {
                int rowSum = RowSum(rowIndex, solution);

                if (rowSum > n / 2)
                    InvertRow(rowIndex, solution);
            }

            for (int columnIndex = 0; columnIndex < n; columnIndex++)
            {
                int columnSum = ColumnSum(columnIndex, solution);

                if (columnSum > m / 2)
                    InvertColumn(columnIndex, solution);
            }
        }





        private void InvertRow(int rowIndex, bool[,] invertibleArray)
        {
            for (int columnIndex = 0; columnIndex < n; columnIndex++)
                invertibleArray[rowIndex, columnIndex] ^= true;
        }

        private void InvertColumn(int columnIndex, bool[,] invertibleArray)
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                invertibleArray[rowIndex, columnIndex] ^= true;
        }

        private void InvertRowColumn(int rowIndex, int columnIndex, bool[,] invertibleArray)
        {
            InvertRow(rowIndex, invertibleArray);
            InvertColumn(columnIndex, invertibleArray);
            invertibleArray[rowIndex, columnIndex] ^= true;
        }

        private void InvertArray(bool[,] invertibleArray)
        {
            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                InvertRow(rowIndex, invertibleArray);
        }


        private int RowSum(int rowIndex, bool[,] summableArray)
        {
            int rowSum = 0;

            for (int columnIndex = 0; columnIndex < n; columnIndex++)
                if (summableArray[rowIndex, columnIndex]) rowSum++;

            return rowSum;
        }

        private int ColumnSum(int columnIndex, bool[,] summableArray)
        {
            int columnSum = 0;

            for (int rowIndex = 0; rowIndex < m; rowIndex++)
                if (summableArray[rowIndex, columnIndex]) columnSum++;

            return columnSum;
        }

        private int ArraySum(bool[,] summableArray)
        {
            int arraySum = 0;

            foreach (bool cell in summableArray)
                if (cell) arraySum++;

            return arraySum;
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
