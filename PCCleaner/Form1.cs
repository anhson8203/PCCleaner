using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace PCCleaner
{
    public partial class Form1 : Form
    {
        private int _filesDeleted;
        private int _foldersDeleted;
        private int _totalFiles;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void ResetCounter()
        {
            _filesDeleted = 0;
            _foldersDeleted = 0;
            _totalFiles = 0;
        }

        private static bool IsFolderEmpty(DirectoryInfo folder) => folder.GetFiles().Length == 0 && folder.GetDirectories().Length == 0;

        private static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static void Form1_Load(object sender, EventArgs e)
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
            var tempDirectory = new DirectoryInfo(Path.GetTempPath());

            _totalFiles = tempDirectory.GetFiles().Length + tempDirectory.GetDirectories().Length;

            foreach (var file in tempDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    _filesDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            foreach (var folder in tempDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    _foldersDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            MessageBox.Show(_filesDeleted + " files and " + _foldersDeleted + " folders deleted of total " + _totalFiles);
        }

        private void CleanNvidiaCache(object sender, EventArgs e)
        {
            ResetCounter();
            var nvidiaCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow", "NVIDIA", "PerDriverVersion", "DXCache");

            if (Directory.Exists(nvidiaCachePath))
            {
                var nvidiaCacheDirectory = new DirectoryInfo(nvidiaCachePath);

                if (IsFolderEmpty(nvidiaCacheDirectory))
                {
                    MessageBox.Show("NVIDIA DXCache directory is already empty!");
                    return;
                }

                _totalFiles = nvidiaCacheDirectory.GetFiles().Length;

                foreach (var file in nvidiaCacheDirectory.GetFiles())
                {
                    try
                    {
                        file.Delete();
                        _filesDeleted++;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                MessageBox.Show(_filesDeleted + " files deleted of total " + _totalFiles);
            }
            else
            {
                MessageBox.Show("NVIDIA DXCache directory does not exist on this machine.");
            }
        }

        private void CleanPrefetch(object sender, EventArgs e)
        {
            ResetCounter();
            var prefetchDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"));

            if (IsFolderEmpty(prefetchDirectory))
            {
                MessageBox.Show("Prefetch directory is already empty!");
                return;
            }

            _totalFiles = prefetchDirectory.GetFiles().Length;

            foreach (var file in prefetchDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    _filesDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            foreach (var folder in prefetchDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    _foldersDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            MessageBox.Show(_filesDeleted + " files and " + _foldersDeleted + " folders deleted of total " + _totalFiles);
        }

        private void CleanWindowsUpdatePackages(object sender, EventArgs e)
        {
            ResetCounter();
            var windowsUpdatePackagesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));

            if (IsFolderEmpty(windowsUpdatePackagesDirectory))
            {
                MessageBox.Show("Windows Update Packages directory is already empty!");
                return;
            }

            _totalFiles = windowsUpdatePackagesDirectory.GetFiles().Length + windowsUpdatePackagesDirectory.GetDirectories().Length;

            foreach (var file in windowsUpdatePackagesDirectory.GetFiles())
            {
                try
                {
                    file.Delete();
                    _filesDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            foreach (var folder in windowsUpdatePackagesDirectory.GetDirectories())
            {
                try
                {
                    folder.Delete(true);
                    _foldersDeleted++;
                }
                catch
                {
                    // ignored
                }
            }

            MessageBox.Show(_filesDeleted + " files and " + _foldersDeleted + " folders deleted of total " + _totalFiles);
        }
    }
}