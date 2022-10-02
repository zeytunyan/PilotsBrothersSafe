using PilotsBrothersSafe.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PilotsBrothersSafe
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();

            CancelButton = (Button)gameBoard
                .Controls["gameBoardTable"]
                .Controls["gameControls"]
                .Controls["backToMenuButton"];
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            int numberOfHandles = Convert.ToInt32(dimensionsNumericUpDown.Maximum);
            gameBoard.PrepareHandles(numberOfHandles * numberOfHandles);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            gameBoard.N = Convert.ToInt32(dimensionsNumericUpDown.Value);
            gameBoard.State = GameBoard.BoardState.GameStarted;
            mainMenu.Hide();
            gameBoard.Show();
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string closingMessage = "Are you sure you want to exit?";

            if (gameBoard.State == GameBoard.BoardState.GameStarted)
                closingMessage += "\r\nAll game progress will be lost.";

            DialogResult exitBoxResult = MessageBox.Show(
                closingMessage,
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            e.Cancel = exitBoxResult == DialogResult.No;
        }
    }
}
