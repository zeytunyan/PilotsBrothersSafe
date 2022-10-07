using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HandleSates = PilotsBrothersSafe.Controls.Handle.States;
using Timer = System.Windows.Forms.Timer;

namespace PilotsBrothersSafe.Controls
{
    // Игровое поле с кнопками и сейфом, на котором расположены рукоятки
    // Всё взаимодействие пользователя во время игры происходит с этим элементом
    public partial class GameBoard : UserControl
    {
        // Ссылка на главную форму
        private GameForm? gameForm;

        // Три состояния: дефолтное (поле скрыто), новая игра и победа
        internal enum BoardState
        {
            Default,
            GameStarted,
            Victory
        }

        // Три варианта взаимодействия мышкой: наведение, клик и удаление наведения
        private enum Interaction
        {
            Move,
            Hover,
            RemoveHover
        }

        private Game? game;

        // По умолчанию размеры поля - 4х4
        private int n = 4, m = 4;

        internal int M
        {
            get => m;
            set
            {
                Program.CheckMNArguments(value);

                m = value;
            }
        }

        internal int N
        {
            get => n;
            set 
            {
                Program.CheckMNArguments(value);
                
                n = value;
            }
        }

        private List<Handle> preparedHandles = new();

        private Timer victoryTimer = new();

        internal GameBoard()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            // После победы сейф открывается с задержкой,
            // чтобы пользователь успел увидеть, что все рукоятки параллельны
            victoryTimer.Interval = 750;
            victoryTimer.Tick += victoryTimer_Tick; 
        }

        private void victoryTimer_Tick(object sender, EventArgs e) =>
            State = BoardState.Victory;

        private void GameBoard_Load(object sender, EventArgs e)
        {
            gameForm = (GameForm)FindForm();
            gameForm.CancelButton = backToMenuButton;
        }

        // Свойства и методы для установки и изменения состояний поля 
        private BoardState state = BoardState.Default;

        internal BoardState State {
            
            get => state;
            
            set 
            {
                state = value;

                switch (state)
                {
                    case BoardState.Default:
                        SetDefaultState();
                        break;
                    case BoardState.Victory:
                        SetVictoryState();
                        break;
                    case BoardState.GameStarted:
                        SetGameStartedState();
                        break;
                    default:
                        throw new NotImplementedException("Недопустимое состояние поля");
                } 
            } 
        }

        private void SetDefaultState()
        {
            solveButton.Enabled = true;
            safe.IsOpen = false;
            IsSolutionShown = false;
            game = null;
            numberOfMovesLabel.Text = OnGoingGameMessage;
        }

        private void SetVictoryState()
        {
            victoryTimer.Stop();
            solveButton.Enabled = false;
            numberOfMovesLabel.Text = VictoryMessage;
            safe.IsOpen = true;
            safe.Enabled = true;
        }

        private void SetGameStartedState()
        {
            safe.Hide();
            IsSolutionShown = false;
            game = new Game(m, n);
            numberOfMovesLabel.Text = OnGoingGameMessage;
            safe.IsOpen = false;
            ClearSafe();
            AddHandlesToSafe();
            safe.Show();
            solveButton.Enabled = true;
        }

        // Показ и скрытие подсказки
        private bool isSolutionShown = false;

        private bool IsSolutionShown
        {
            get => isSolutionShown;

            set
            {
                if (isSolutionShown == value)
                    return;

                isSolutionShown = value;

                ResumeOrPauseSolutionShowing();
            }
        }

        private void ResumeOrPauseSolutionShowing()
        {
            ThrowIfGameNull();

            foreach (int handleIndex in game.Solution)
                safe[handleIndex].State ^= HandleSates.PaintedOver;
        }

        // Свойства для отображения количества ходов
        private int NumberOfMoves => game?.NumberOfMoves ?? 0;

        private string OnGoingGameMessage =>
            "Number of moves: " + NumberOfMoves;

        private string VictoryMessage =>
            $"Congratulations! You won in {NumberOfMoves} moves";

        // Методы для удаления и добавления рукояток
        private void ClearSafe()
        {
            safe.ClearHandles();
            safe.ClearDimensions();
        }

        internal void AddHandlesToSafe()
        {
            safe.SetDimensions(m, n);
            FillWithHandles();
        }

        private void FillWithHandles()
        {
            ThrowIfGameNull();

            // Если имеются заготовленные рукоятки в достаточном количестве, добавляем их
            // А иначе создаем новые
            bool notEnoughPreparedHandles = preparedHandles.Count < m * n;

            int index = 0;
            foreach (bool position in game.Configuration)
            {
                if (notEnoughPreparedHandles)
                {
                    safe[index++] = CreateHandleWithEvents(position);
                    continue;
                }

                preparedHandles[index].IsVertical = position;
                safe[index] = preparedHandles[index];
                index++;
            }    
        }

        internal void PrepareHandles(int numberOfHandles)
        {
            for (int i = 0; i < numberOfHandles; i++)
                preparedHandles.Add(CreateHandleWithEvents());
        }

        private Handle CreateHandleWithEvents(bool isVerical = false)
        {
            Handle handle = new Handle(isVerical);
            handle.Click += handle_Click;
            handle.MouseEnter += handle_MouseEnter;
            handle.MouseLeave += handle_MouseLeave;
            return handle;
        }

        // Методы для обработки взаимодействия пользователя с рукоятками
        private void handle_Click(object sender, EventArgs e) =>
            ProcessInteraction((Handle)sender, Interaction.Move);

        private void handle_MouseEnter(object sender, EventArgs e) =>
            ProcessInteraction((Handle)sender, Interaction.Hover);

        private void handle_MouseLeave(object sender, EventArgs e) =>
            ProcessInteraction((Handle)sender, Interaction.RemoveHover);

        private void ProcessInteraction(Handle handle, Interaction interaction)
        {
            var handlePosition = safe.FindHandle(handle);
            int handleRow = handlePosition.Row;
            int handleColumn = handlePosition.Column;

            if (interaction == Interaction.Move)
                MakeMoveActions(handleRow, handleColumn);

            InteractWithRowAndColumn(handleRow, handleColumn, interaction);  
        }

        private void InteractWithRowAndColumn(int row, int column, Interaction interaction)
        {
            ThrowIfGameNull();

            foreach (int handleIndex in game.Moves[row, column])
                InteractWithHandle(safe[handleIndex], interaction);
        }

        private void InteractWithHandle(Handle handle, Interaction interaction)
        {
            switch (interaction)
            {
                case Interaction.Move:
                    handle.IsVertical ^= true;
                    break;
                case Interaction.Hover:
                case Interaction.RemoveHover:
                    handle.State ^= HandleSates.Highlighted;
                    break;
                default:
                    throw new NotImplementedException("Недопустимый тип взаимодействия");
            }
        }

        // Ход игрока
        private void MakeMoveActions(int handleRow, int handleColumn)
        {
            ThrowIfGameNull();

            // Чтобы подсказка обновилась, скрываем её и после хода вновь показываем
            if (IsSolutionShown) ResumeOrPauseSolutionShowing();
            game.Move(handleRow, handleColumn);
            if (IsSolutionShown) ResumeOrPauseSolutionShowing();

            numberOfMovesLabel.Text = OnGoingGameMessage;

            if (game.Victory) 
                ShowVictory();
        }

        private void ShowVictory()
        {
            safe.Enabled = false;
            victoryTimer.Start();
        }

        // Обработчики нажатий на кнопки интерфейса
        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            DialogResult toMenuAskResult = gameForm.AskAttentionQuestion(
                "go to the menu",
                "Back to menu");

            if (toMenuAskResult == DialogResult.No)
                return;

            State = BoardState.Default;
            Hide();
            gameForm.Controls["mainMenu"].Show();
        }

        private void solveButton_Click(object sender, EventArgs e) =>
           IsSolutionShown ^= true;

        private void restartButton_Click(object sender, EventArgs e)
        {
            bool condition = State != BoardState.GameStarted ||
                gameForm.AskAttentionQuestion("restart", "Restart") == DialogResult.Yes;

            if (condition)
                State = BoardState.GameStarted;
        }

        // Проверка, создан ли объект game, если нет - исключение
        private void ThrowIfGameNull() 
        {
            if (game == null)
                throw new NullReferenceException("Объект game равен null");
        }
    }
}
