using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PilotsBrothersSafe.Controls
{
    public partial class Handle : PictureBox
    {
        private bool isVertical;

        internal bool IsVertical
        {
            get => isVertical;

            set
            {
                isVertical = value;

                Image = isVertical ?
                    Properties.Resources.handle_vertical :
                    Properties.Resources.handle_horizontal;
            }
        }

        internal enum States
        {
            Default,
            PaintedOver,
            Highlighted,
            PaintedOverHighlighted
        }

        private States state = States.Default;

        internal States State 
        {
            get => state;

            set
            {
                if (state == value)
                    return;

                state = value;

                BackColor = state switch
                {
                    States.Default => Color.Transparent,
                    States.PaintedOver => Color.FromArgb(76, 99, 107),
                    States.Highlighted => Color.FromArgb(192, 255, 255),
                    States.PaintedOverHighlighted => Color.Gray,
                    _ => throw new NotImplementedException("Недопустимое состояние")
                };
            }
        }

        internal Handle() : this(false) { }

        internal Handle(bool position)
        {
            InitializeComponent();
            Cursor = Cursors.Hand;
            TabStop = true;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);
            Padding = new Padding(1);
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
            IsVertical = position;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
