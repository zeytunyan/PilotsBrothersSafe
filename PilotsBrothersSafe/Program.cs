namespace PilotsBrothersSafe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new GameForm());
        }

        // ѕроверка на то, адекватны ли переданные размеры пол€, используема€ повсюду в программе.
        static internal void CheckMNArguments(int m, int n = 2)
        {
            if (m < 2 || n < 2)
            {
                string errorMessage = "ѕереданные аргументы недопустимо малы";
                throw new ArgumentOutOfRangeException(errorMessage);
            }
        }
    }
}
