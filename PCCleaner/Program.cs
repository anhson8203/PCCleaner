using System;
using System.Threading;
using System.Windows.Forms;

namespace PCCleaner
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (var mutex = new Mutex(false, "PCCleanerSingletonMutex", out var createdNew))
            {
                // Only the illiterate don't understand this mutex.
                if (!createdNew)
                {
                    MessageBox.Show("Another instance of PC Cleaner is already running.", "PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}