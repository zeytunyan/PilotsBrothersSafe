namespace PilotsBrothersSafe.Controls
{
    partial class GameBoard
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameBoard));
            this.gameBoardTable = new System.Windows.Forms.TableLayoutPanel();
            this.gameControls = new System.Windows.Forms.TableLayoutPanel();
            this.numberOfMovesLabel = new System.Windows.Forms.Label();
            this.gameBoardTable.SuspendLayout();
            this.gameControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameBoardTable
            // 
            this.gameBoardTable.ColumnCount = 1;
            this.gameBoardTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gameBoardTable.Controls.Add(this.gameControls, 0, 0);
            this.gameBoardTable.Controls.Add(this.numberOfMovesLabel, 0, 1);
            this.gameBoardTable.Controls.Add(this.safe, 0, 2);
            this.gameBoardTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameBoardTable.Location = new System.Drawing.Point(0, 0);
            this.gameBoardTable.Name = "gameBoardTable";
            this.gameBoardTable.RowCount = 3;
            this.gameBoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.gameBoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.gameBoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82F));
            this.gameBoardTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.gameBoardTable.Size = new System.Drawing.Size(518, 632);
            this.gameBoardTable.TabIndex = 0;
            // 
            // gameControls
            // 
            this.gameControls.ColumnCount = 4;
            this.gameControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.gameControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.gameControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.gameControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.gameControls.Controls.Add(this.backToMenuButton, 0, 0);
            this.gameControls.Controls.Add(this.restartButton, 2, 0);
            this.gameControls.Controls.Add(this.solveButton, 1, 0);
            this.gameControls.Controls.Add(this.helpButton, 3, 0);
            this.gameControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameControls.Location = new System.Drawing.Point(3, 3);
            this.gameControls.Name = "gameControls";
            this.gameControls.RowCount = 1;
            this.gameControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gameControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.gameControls.Size = new System.Drawing.Size(512, 57);
            this.gameControls.TabIndex = 1;
            // 
            // backToMenuButton
            // 
            this.backToMenuButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.backToMenuButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backToMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backToMenuButton.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.backToMenuButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.backToMenuButton.Location = new System.Drawing.Point(3, 3);
            this.backToMenuButton.Name = "backToMenuButton";
            this.backToMenuButton.Size = new System.Drawing.Size(122, 51);
            this.backToMenuButton.TabIndex = 0;
            this.backToMenuButton.Text = "🠔Menu";
            this.backToMenuButton.UseVisualStyleBackColor = false;
            this.backToMenuButton.Click += new System.EventHandler(this.backToMenuButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.restartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restartButton.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.restartButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.restartButton.Location = new System.Drawing.Point(259, 3);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(122, 51);
            this.restartButton.TabIndex = 2;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = false;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // solveButton
            // 
            this.solveButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.solveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.solveButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.solveButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.solveButton.Location = new System.Drawing.Point(131, 3);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(122, 51);
            this.solveButton.TabIndex = 1;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = false;
            this.solveButton.Click += new System.EventHandler(this.solveButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.helpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.helpButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.helpButton.Location = new System.Drawing.Point(387, 3);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(122, 51);
            this.helpButton.TabIndex = 3;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = false;
            // 
            // numberOfMovesLabel
            // 
            this.numberOfMovesLabel.AutoSize = true;
            this.numberOfMovesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numberOfMovesLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numberOfMovesLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.numberOfMovesLabel.Location = new System.Drawing.Point(3, 63);
            this.numberOfMovesLabel.Name = "numberOfMovesLabel";
            this.numberOfMovesLabel.Size = new System.Drawing.Size(512, 50);
            this.numberOfMovesLabel.TabIndex = 2;
            this.numberOfMovesLabel.Text = "Number of moves: 0";
            this.numberOfMovesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // safe
            // 
            this.safe.BackColor = System.Drawing.Color.Transparent;
            this.safe.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("safe.BackgroundImage")));
            this.safe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.safe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.safe.Location = new System.Drawing.Point(3, 116);
            this.safe.Name = "safe";
            this.safe.Size = new System.Drawing.Size(512, 513);
            this.safe.TabIndex = 3;
            this.safe.TabStop = false;
            // 
            // GameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.gameBoardTable);
            this.DoubleBuffered = true;
            this.Name = "GameBoard";
            this.Size = new System.Drawing.Size(518, 632);
            this.Load += new System.EventHandler(this.GameBoard_Load);
            this.gameBoardTable.ResumeLayout(false);
            this.gameBoardTable.PerformLayout();
            this.gameControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel gameBoardTable;
        private TableLayoutPanel gameControls;
        private GameInterfaceButton backToMenuButton;
        private GameInterfaceButton solveButton;
        private Label numberOfMovesLabel;
        private Safe safe;
        private GameInterfaceButton restartButton;
        private HelpButton helpButton;
    }
}
