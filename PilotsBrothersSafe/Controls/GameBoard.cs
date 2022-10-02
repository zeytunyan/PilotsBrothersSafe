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
    public partial class GameBoard : UserControl
    {
        public enum BoardState
        {
            Default,
            GameStarted,
            Victory
        }

        private enum Interaction
        {
            Move,
            Hover,
            RemoveHover
        }

        private Game? game;

        public int N = 4;

        private List<Handle> preparedHandles = new();

        private Timer victoryTimer = new();

        public GameBoard()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            victoryTimer.Interval = 750;
            victoryTimer.Tick += victoryTimer_Tick;
        }

        private void victoryTimer_Tick(object sender, EventArgs e) =>
            State = BoardState.Victory;

        private BoardState state = BoardState.Default;

        public BoardState State {
            
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
            safe.Enabled = true;
            safe.IsOpen = true;
        }

        private void SetGameStartedState()
        {
            IsSolutionShown = false;
            game = new Game(N);
            numberOfMovesLabel.Text = OnGoingGameMessage;
            safe.IsOpen = false;
            safe.Hide();
            ClearSafe();
            AddHandlesToSafe();
            safe.Show();
            solveButton.Enabled = true;
        }


        private bool isSolutionShown = false;

        private bool IsSolutionShown
        {
            get => isSolutionShown;

            set
            {
                if (isSolutionShown == value)
                    return;

                isSolutionShown = value;

                ResumeOrPauseSolutionShowing(isSolutionShown);
            }
        }

        private void ResumeOrPauseSolutionShowing(bool resumeOrPause)
        {
            ThrowIfGameNull();

            foreach (int handleIndex in game.Solution)
                safe[handleIndex].State ^= HandleSates.PaintedOver;
        }


        private int NumberOfMoves => game?.NumberOfMoves ?? 0;

        private string OnGoingGameMessage =>
            "Number of moves: " + NumberOfMoves;

        private string VictoryMessage =>
            $"Congratulations! You won in {NumberOfMoves} moves";

        private void ClearSafe()
        {
            safe.ClearHandles();
            safe.ClearDimensions();
        }

        public void AddHandlesToSafe()
        {
            safe.SetDimensions(N, N); 

            if (preparedHandles.Count < N * N)
                AddNewHandles();
            else
                AddPreparedHandles();
        }

        private void AddNewHandles()
        {
            ThrowIfGameNull();

            int index = 0;
            foreach (bool position in game.configuration)
                safe[index++] = CreateHandleWithEvents(position);
        }

        private void AddPreparedHandles()
        {
            ThrowIfGameNull();

            int index = 0;
            foreach (bool position in game.configuration)
            {
                preparedHandles[index].IsVertical = position;
                safe[index] = preparedHandles[index];
                index++;
            }
        }

        public void PrepareHandles(int numberOfHandles)
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

            foreach (int handleIndex in game.moves[row, column])
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

        private void MakeMoveActions(int handleRow, int handleColumn)
        {
            ThrowIfGameNull();

            if (IsSolutionShown) ResumeOrPauseSolutionShowing(false);
            game.Move(handleRow, handleColumn);
            if (IsSolutionShown) ResumeOrPauseSolutionShowing(true);

            numberOfMovesLabel.Text = OnGoingGameMessage;

            if (game.Victory) ShowVictory();
        }

        private void ShowVictory()
        {
            safe.Enabled = false;
            victoryTimer.Start();
        }


        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            string toMenuMessage = "Are you sure you want to go to the menu?";
                
            if (State == BoardState.GameStarted)
                toMenuMessage += "\r\nAll game progress will be lost.";

            DialogResult toMenuBoxResult = MessageBox.Show(
                toMenuMessage, 
                "Back to menu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (toMenuBoxResult == DialogResult.No) return;

            State = BoardState.Default;
            Hide();
            Parent.Controls["mainMenu"].Show();
        }

        private void solveButton_Click(object sender, EventArgs e) =>
           IsSolutionShown ^= true;

        private void restartButton_Click(object sender, EventArgs e)
        {
            if (State != BoardState.GameStarted)
            {
                State = BoardState.GameStarted;
                return;
            }

            string restartMessage =
                "Are you sure you want to restart?" +
                "\r\nAll game progress will be lost.";

            DialogResult restartBoxResult = MessageBox.Show(
                restartMessage,
                "Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (restartBoxResult == DialogResult.Yes)
                State = BoardState.GameStarted;
        }


        private void ThrowIfGameNull() 
        {
            if (game == null)
                throw new NullReferenceException("Объект game равен null");
        }
    }
}
