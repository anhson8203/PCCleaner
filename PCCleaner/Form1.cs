using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using PCCleaner.Properties;

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

        private static bool IsFolderEmpty(DirectoryInfo folder)
        {
            return folder.GetFiles().Length == 0 && folder.GetDirectories().Length == 0;
        }

        private static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void Form1_Load(object sender, EventArgs e)
        {
            if (IsAdmin()) return;
            MessageBox.Show(Resources.administrative_privileges_warning, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CleanGeneralFolder(object sender, EventArgs e)
        {
            ResetCounter();
            var userTempDirectory = new DirectoryInfo(Path.GetTempPath());
            var windowsTempDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"));
            var windowsPrefetchDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"));
            var windowsCrashDumpDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "CrashDumps"));

            _totalFiles = userTempDirectory.GetFiles().Length + userTempDirectory.GetDirectories().Length +
                          windowsTempDirectory.GetFiles().Length + windowsTempDirectory.GetDirectories().Length +
                          windowsPrefetchDirectory.GetFiles().Length +
                          windowsPrefetchDirectory.GetDirectories().Length + windowsCrashDumpDirectory.GetFiles().Length +
                          windowsCrashDumpDirectory.GetDirectories().Length;

            foreach (var file in windowsPrefetchDirectory.GetFiles())
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

            foreach (var file in windowsCrashDumpDirectory.GetFiles())
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
            
            foreach (var folder in windowsPrefetchDirectory.GetDirectories())
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
            
            foreach (var folder in windowsCrashDumpDirectory.GetDirectories())
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

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles);
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
                    MessageBox.Show(Resources.empty_nvidia_cache);
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

                MessageBox.Show(_filesDeleted + Resources.folders_deleted + _totalFiles);
            }
            else if (Directory.Exists(nvidiaCachePathNew))
            {
                var nvidiaCacheDirectory = new DirectoryInfo(nvidiaCachePathNew);

                if (IsFolderEmpty(nvidiaCacheDirectory))
                {
                    MessageBox.Show(Resources.empty_nvidia_cache);
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

                MessageBox.Show(_filesDeleted + Resources.folders_deleted + _totalFiles);
            }
            else if (!Directory.Exists(nvidiaCachePathNew) && !Directory.Exists(nvidiaCachePathOld))
            {
                MessageBox.Show(Resources.nvidia_cache_not_exist);
            }
        }

        private void CleanWindowsUpdatePackages(object sender, EventArgs e)
        {
            ResetCounter();
            var windowsUpdatePackagesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));

            if (IsFolderEmpty(windowsUpdatePackagesDirectory))
            {
                MessageBox.Show(Resources.empty_windows_update_directory);
                return;
            }

            _totalFiles = windowsUpdatePackagesDirectory.GetFiles().Length +
                          windowsUpdatePackagesDirectory.GetDirectories().Length;

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

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles);
        }
    }
}