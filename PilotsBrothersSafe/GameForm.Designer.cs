namespace PilotsBrothersSafe
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.gameBoard = new PilotsBrothersSafe.Controls.GameBoard();
            this.helpButton = new PilotsBrothersSafe.Controls.HelpButton();
            this.startButton = new PilotsBrothersSafe.Controls.GameInterfaceButton();
            this.menuLabel = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.TableLayoutPanel();
            this.dimensionsRow = new System.Windows.Forms.TableLayoutPanel();
            this.dimensionsLabel = new System.Windows.Forms.Label();
            this.dimensionsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mainMenu.SuspendLayout();
            this.dimensionsRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dimensionsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // gameBoard
            // 
            this.gameBoard.BackColor = System.Drawing.Color.AliceBlue;
            resources.ApplyResources(this.gameBoard, "gameBoard");
            this.gameBoard.Name = "gameBoard";
            this.gameBoard.State = PilotsBrothersSafe.Controls.GameBoard.BoardState.Default;
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.helpButton, "helpButton");
            this.helpButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.helpButton.Name = "helpButton";
            this.helpButton.UseVisualStyleBackColor = false;
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.startButton.Name = "startButton";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // menuLabel
            // 
            resources.ApplyResources(this.menuLabel, "menuLabel");
            this.mainMenu.SetColumnSpan(this.menuLabel, 3);
            this.menuLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.menuLabel.Name = "menuLabel";
            // 
            // mainMenu
            // 
            this.mainMenu.BackColor = System.Drawing.Color.AliceBlue;
            resources.ApplyResources(this.mainMenu, "mainMenu");
            this.mainMenu.Controls.Add(this.dimensionsRow, 1, 1);
            this.mainMenu.Controls.Add(this.menuLabel, 0, 1);
            this.mainMenu.Controls.Add(this.startButton, 1, 3);
            this.mainMenu.Controls.Add(this.helpButton, 1, 4);
            this.mainMenu.Name = "mainMenu";
            // 
            // dimensionsRow
            // 
            resources.ApplyResources(this.dimensionsRow, "dimensionsRow");
            this.mainMenu.SetColumnSpan(this.dimensionsRow, 3);
            this.dimensionsRow.Controls.Add(this.dimensionsLabel, 1, 0);
            this.dimensionsRow.Controls.Add(this.dimensionsNumericUpDown, 2, 0);
            this.dimensionsRow.Name = "dimensionsRow";
            // 
            // dimensionsLabel
            // 
            resources.ApplyResources(this.dimensionsLabel, "dimensionsLabel");
            this.dimensionsLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.dimensionsLabel.Name = "dimensionsLabel";
            // 
            // dimensionsNumericUpDown
            // 
            resources.ApplyResources(this.dimensionsNumericUpDown, "dimensionsNumericUpDown");
            this.dimensionsNumericUpDown.ForeColor = System.Drawing.Color.DodgerBlue;
            this.dimensionsNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.dimensionsNumericUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.dimensionsNumericUpDown.Name = "dimensionsNumericUpDown";
            this.dimensionsNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // GameForm
            // 
            this.AcceptButton = this.startButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.gameBoard);
            this.DoubleBuffered = true;
            this.Name = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.dimensionsRow.ResumeLayout(false);
            this.dimensionsRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dimensionsNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.GameBoard gameBoard;
        private Controls.HelpButton helpButton;
        private Controls.GameInterfaceButton startButton;
        private TableLayoutPanel mainMenu;
        private Label menuLabel;
        private TableLayoutPanel dimensionsRow;
        private Label dimensionsLabel;
        private NumericUpDown dimensionsNumericUpDown;
    }
}