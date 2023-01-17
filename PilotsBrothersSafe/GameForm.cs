using PilotsBrothersSafe.Controls;


namespace PilotsBrothersSafe
{
    // Главная форма
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        // Подготавливаем рукоятки заранее, чтобы они не тормозили интерфейс
        private void GameForm_Load(object sender, EventArgs e)
        {
            int numberOfHandles = Convert.ToInt32(dimensionsNumericUpDown.Maximum);
            gameBoard.PrepareHandles(numberOfHandles * numberOfHandles);
        }

        // После нажатия Start задаются размеры поля,
        // устанавливается состояние начавшейся игры,
        // и поле показывается игроку.
        private void startButton_Click(object sender, EventArgs e)
        {
            int dimensionSize = Convert.ToInt32(dimensionsNumericUpDown.Value);
            (gameBoard.M, gameBoard.N) = (dimensionSize, dimensionSize);
            gameBoard.State = GameBoard.BoardState.GameStarted;
            mainMenu.Hide();
            gameBoard.Show();
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult exitAskResult = AskAttentionQuestion("exit", "Exit");
            e.Cancel = exitAskResult == DialogResult.No;
        }

        // Показ окошка с предупреждением о потере прогресса, используется при нажатии на различные кнопки
        internal DialogResult AskAttentionQuestion(string actionAskAbout, string caption)
        {
            string message = $"Are you sure you want to {actionAskAbout}?";

            if (gameBoard.State == GameBoard.BoardState.GameStarted)
                message += "\r\nAll game progress will be lost.";

            DialogResult askResult = MessageBox.Show(
                message,
                caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            return askResult;
        } 
    }
}
