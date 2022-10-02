namespace PilotsBrothersSafe.Controls
{
    partial class Safe
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
            this.tableWithHandles = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tableWithHandles
            // 
            this.tableWithHandles.ColumnCount = 1;
            this.tableWithHandles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableWithHandles.Location = new System.Drawing.Point(113, 113);
            this.tableWithHandles.Name = "tableWithHandles";
            this.tableWithHandles.RowCount = 1;
            this.tableWithHandles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableWithHandles.Size = new System.Drawing.Size(0, 0);
            this.tableWithHandles.TabIndex = 0;
            // 
            // Safe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::PilotsBrothersSafe.Properties.Resources.closed_safe;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.tableWithHandles);
            this.DoubleBuffered = true;
            this.Name = "Safe";
            this.Size = new System.Drawing.Size(512, 512);
            this.Resize += new System.EventHandler(this.safe_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableWithHandles;
    }
}
