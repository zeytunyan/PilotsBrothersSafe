using System;
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
    public partial class Safe : UserControl
    {
        private bool isOpen = false;

        public bool IsOpen
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

        public bool IsWide => Width > Height;

        public int SmallestDimension =>
            IsWide ? Height : Width;

        public int BiggestDimension =>
            IsWide ? Width : Height;

        public Safe()
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
            int tableDimensionSize = SmallestDimension * 53 / 100;
            int widthDivider = tableWithHandles.ColumnCount;
            int heightDivider = tableWithHandles.RowCount;

            int finalWidthSize = CalcFinalDimensionSize(tableDimensionSize, widthDivider);
            int finalHeightSize = CalcFinalDimensionSize(tableDimensionSize, heightDivider);

            tableWithHandles.Size = new Size(finalWidthSize, finalHeightSize);
        }

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
            int smallerIndent = SmallestDimension * 22 / 100;
            int additionalIndent = BiggestDimension  / 2 - SmallestDimension / 2;
            int biggerIndent = smallerIndent + additionalIndent;

            tableWithHandles.Location = new Point
            {
                X = IsWide ? biggerIndent : smallerIndent,
                Y = IsWide ? smallerIndent : biggerIndent,
            };
        }

        public void ClearDimensions() 
        {
            tableWithHandles.RowCount = 0;
            tableWithHandles.ColumnCount = 0;
            tableWithHandles.RowStyles.Clear();
            tableWithHandles.ColumnStyles.Clear();
        }

        public void SetDimensions(int m, int n)
        {
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

        public TableLayoutPanelCellPosition FindHandle(Handle handle) =>
            tableWithHandles.GetCellPosition(handle);

        public void ClearHandles() => tableWithHandles.Controls.Clear();

        public Handle this[int indexOfHandle]
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
