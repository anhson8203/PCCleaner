using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace PCCleaner
{
    public partial class Form1 : Form
    {
        int FilesDeleted = 0;
        int FoldersDeleted = 0;
        int TotalFiles = 0;

        public Form1()
        {
            InitializeComponent();
            Load += new EventHandler(Form1_Load);
        }

        private void ResetCounter()
        {
            FilesDeleted = 0;
            FoldersDeleted = 0;
            TotalFiles = 0;
        }

        private bool IsFolderEmpty(DirectoryInfo folder) => folder.GetFiles().Length == 0 && folder.GetDirectories().Length == 0;

        private bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsAdmin())
            {
                return;
            }

            MessageBox.Show("This program is not running with administrative privileges. Some functions may not work properly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CleanTempFolder(object sender, EventArgs e)
        {
            ResetCounter();
            DirectoryInfo tempDirectory = new DirectoryInfo(Path.GetTempPath());

            TotalFiles = tempDirectory.GetFiles().Length + tempDirectory.GetDirectories().Length;

            foreach (FileInfo file in tempDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    FilesDeleted++;
                }
                catch { }
            }

            foreach (DirectoryInfo folder in tempDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    FoldersDeleted++;
                }
                catch { }
            }

            MessageBox.Show(FilesDeleted + " files and " + FoldersDeleted + " folders deleted of total " + TotalFiles + " files!");
        }

        private void CleanNvidiaCache(object sender, EventArgs e)
        {
            ResetCounter();
            string nvidiaCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow", "NVIDIA", "PerDriverVersion", "DXCache");

            if (Directory.Exists(nvidiaCachePath))
            {
                DirectoryInfo nvidiaCacheDirectory = new DirectoryInfo(nvidiaCachePath);

                if (IsFolderEmpty(nvidiaCacheDirectory))
                {
                    MessageBox.Show("NVIDIA DXCache directory is already empty!");
                    return;
                }

                TotalFiles = nvidiaCacheDirectory.GetFiles().Length;

                foreach (FileInfo file in nvidiaCacheDirectory.GetFiles())
                {
                    try
                    {
                        file.Delete();
                        FilesDeleted++;
                    }
                    catch { }
                }

                MessageBox.Show(FilesDeleted + " files deleted of total " + TotalFiles + " files!");
            }
            else
            {
                MessageBox.Show("NVIDIA DXCache directory does not exist on this machine.");
            }
        }

        private void CleanPrefetch(object sender, EventArgs e)
        {
            ResetCounter();
            DirectoryInfo prefetchDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"));

            if (IsFolderEmpty(prefetchDirectory))
            {
                MessageBox.Show("Prefetch directory is already empty!");
                return;
            }

            TotalFiles = prefetchDirectory.GetFiles().Length;

            foreach (FileInfo file in prefetchDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    FilesDeleted++;
                }
                catch { }
            }

            foreach (DirectoryInfo folder in prefetchDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    FoldersDeleted++;
                }
                catch { }
            }

            MessageBox.Show("Deleted " + FilesDeleted + " files and " + FoldersDeleted + " folders of total " + TotalFiles + " files!");
        }

        private void CleanWindowsUpdatePackages(object sender, EventArgs e)
        {
            ResetCounter();
            DirectoryInfo windowsUpdatePackagesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));

            if (IsFolderEmpty(windowsUpdatePackagesDirectory))
            {
                MessageBox.Show("Windows Update Packages directory is already empty!");
                return;
            }

            TotalFiles = windowsUpdatePackagesDirectory.GetFiles().Length + windowsUpdatePackagesDirectory.GetDirectories().Length;

            foreach (FileInfo file in windowsUpdatePackagesDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    FilesDeleted++;
                }
                catch { }
            }

            foreach (DirectoryInfo folder in windowsUpdatePackagesDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    FoldersDeleted++;
                }
                catch { }
            }

            MessageBox.Show(FilesDeleted + " files and " + FoldersDeleted + " folders deleted of total " + TotalFiles + " files!");
        }
    }
}