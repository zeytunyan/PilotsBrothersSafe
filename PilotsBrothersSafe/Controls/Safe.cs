﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PilotsBrothersSafe.Controls
{
    // Элемент интерфейса - сейф, в котором расположены рукоятки
    public partial class Safe : UserControl
    {
        // При победе сейф открывается
        private bool isOpen = false;

        internal bool IsOpen
        {
            get => isOpen;

            set
            {
                if (isOpen == value)
                    return;

                isOpen = value;

                tableWithHandles.Visible = !isOpen;

                BackgroundImage = isOpen ?
                    Properties.Resources.open_safe :
                    Properties.Resources.closed_safe;
            }
        }

        // Свойства для расчета размеров и положения таблицы с рукоятками
        private bool IsWide => Width > Height;

        private int SmallestDimension =>
            IsWide ? Height : Width;

        private int BiggestDimension =>
            IsWide ? Width : Height;

        internal Safe()
        {
            InitializeComponent();
            IsOpen = false;
            Dock = DockStyle.Fill;
            SetLayoutForTableWithHandles();
        }

        private void safe_Resize(object sender, EventArgs e)
        {
            Hide();
            SetLayoutForTableWithHandles();
            Show();
        }

        private void SetLayoutForTableWithHandles()
        {
            SetSizeForTableWithHandles();
            SetLocationForTableWithHandles();
        }

        private void SetSizeForTableWithHandles()
        {
            // Таблица с рукоятками должна занимать около 53% сейфа
            int tableDimensionSize = SmallestDimension * 53 / 100;
            int widthDivider = tableWithHandles.ColumnCount;
            int heightDivider = tableWithHandles.RowCount;

            // Окончательное значение должно делиться на количество строк и столбцов
            int finalWidthSize = CalcFinalDimensionSize(tableDimensionSize, widthDivider);
            int finalHeightSize = CalcFinalDimensionSize(tableDimensionSize, heightDivider);

            tableWithHandles.Size = new Size(finalWidthSize, finalHeightSize);
        }

        // Подсчет итогового размера, при котором все столбцы и строки будут одинаковы
        private int CalcFinalDimensionSize(int dimensionSize, int divider)
        {
            int minSize = dimensionSize / divider * divider;
            int maxSize = minSize + divider;
            int spreadForMin = dimensionSize - minSize;
            int spreadForMax = maxSize - dimensionSize;

            return spreadForMin < spreadForMax ? minSize : maxSize;
        }
 
        private void SetLocationForTableWithHandles()
        {
            // Таблица с рукоятками должна быть сдвинута
            // где-то на 22%, чтобы быть по центру сейфа
            int smallerIndent = SmallestDimension * 22 / 100;
            int additionalIndent = BiggestDimension  / 2 - SmallestDimension / 2;
            int biggerIndent = smallerIndent + additionalIndent;

            tableWithHandles.Location = new Point
            {
                X = IsWide ? biggerIndent : smallerIndent,
                Y = IsWide ? smallerIndent : biggerIndent,
            };
        }

        internal void ClearDimensions() 
        {
            tableWithHandles.RowCount = 0;
            tableWithHandles.ColumnCount = 0;
            tableWithHandles.RowStyles.Clear();
            tableWithHandles.ColumnStyles.Clear();
        }

        internal void SetDimensions(int m, int n)
        {
            Program.CheckMNArguments(m, n);

            tableWithHandles.RowCount = m;
            tableWithHandles.ColumnCount = n;

            float rowSize = 100F / m;
            float columnSize = 100F / n;

            for (int i = 0; i < m; i++)
            {
                RowStyle rowStyle = new RowStyle(SizeType.Percent, rowSize);
                tableWithHandles.RowStyles.Add(rowStyle);
            }

            for (int i = 0; i < n; i++)
            {
                ColumnStyle columnStyle = new ColumnStyle(SizeType.Percent, columnSize);
                tableWithHandles.ColumnStyles.Add(columnStyle);
            }

            SetSizeForTableWithHandles();
        }

        internal TableLayoutPanelCellPosition FindHandle(Handle handle) =>
            tableWithHandles.GetCellPosition(handle);

        internal void ClearHandles() => tableWithHandles.Controls.Clear();

        internal Handle this[int indexOfHandle]
        {
            get => (Handle)tableWithHandles.Controls[indexOfHandle];

            set
            {
                int column = indexOfHandle % tableWithHandles.ColumnCount;
                int row = indexOfHandle / tableWithHandles.ColumnCount;
                tableWithHandles.Controls.Add(value, column, row);
            }  
        }
    }
}
