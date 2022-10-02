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
    public partial class HelpButton : GameInterfaceButton
    {
        public HelpButton()
        {
            InitializeComponent();
            Click += helpButton_Click;
        }

        private void helpButton_Click(object sender, EventArgs e) =>
            ShowHelp();

        private void ShowHelp()
        {
            string helpInformation = @"There are a lot of handles on the safe, arranged in a square, like a 2-dimensional array NxN.

By clicking the mouse, the position of the handle changes from vertical to horizontal and back.

When you turn the handle, all the handles turn in one row and in one column.

The safe opens only if it is possible to place all the handles parallel to each other (i.e. all vertically or all horizontally).";

            MessageBox.Show(helpInformation, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
