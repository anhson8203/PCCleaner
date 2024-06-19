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
            var userTempDirectory = new DirectoryInfo(Path.GetTempPath());
            var windowsTempDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"));

            _totalFiles = userTempDirectory.GetFiles().Length + userTempDirectory.GetDirectories().Length + windowsTempDirectory.GetFiles().Length + windowsTempDirectory.GetDirectories().Length;

            foreach (var file in userTempDirectory.GetFiles())
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
            
            foreach (var file in windowsTempDirectory.GetFiles())
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

            foreach (var folder in userTempDirectory.GetDirectories())
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
            
            foreach (var folder in windowsTempDirectory.GetDirectories())
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
            var nvidiaCachePathNew = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow", "NVIDIA", "PerDriverVersion", "DXCache");
            var nvidiaCachePathOld = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "NVIDIA", "DXCache");
            
            if (!Directory.Exists(nvidiaCachePathNew))
            {
                var nvidiaCacheDirectory = new DirectoryInfo(nvidiaCachePathOld);

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
            else if (Directory.Exists(nvidiaCachePathNew))
            {
                var nvidiaCacheDirectory = new DirectoryInfo(nvidiaCachePathNew);

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
            else if (!Directory.Exists(nvidiaCachePathNew) && !Directory.Exists(nvidiaCachePathOld))
            {
                MessageBox.Show("NVIDIA DXCache directory not found on this machine!");
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