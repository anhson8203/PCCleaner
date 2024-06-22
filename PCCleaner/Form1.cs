﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
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

        private static void Form1_Load(object sender, EventArgs e)
        {
            if (IsAdmin()) return;
            MessageBox.Show(Resources.administrative_privileges_warning, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ResetCounter()
        {
            _filesDeleted = 0;
            _foldersDeleted = 0;
            _totalFiles = 0;
        }

        private static bool IsFolderEmpty(DirectoryInfo folder) => folder.GetFiles().Length == 0 && folder.GetDirectories().Length == 0;

        private static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private void CleanGeneralFolder(object sender, EventArgs e)
        {
            ResetCounter();
            var directories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.GetTempPath()),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Microsoft", "Windows", "Explorer")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "CrashDumps")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "D3DSCache"))
            };

            foreach (var directory in directories)
            {
                _totalFiles += directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count();

                Parallel.ForEach(directory.EnumerateFiles(), file =>
                {
                    try
                    {
                        file.Delete();
                        Interlocked.Increment(ref _filesDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });

                Parallel.ForEach(directory.EnumerateDirectories(), folder =>
                {
                    try
                    {
                        folder.Delete(true);
                        Interlocked.Increment(ref _foldersDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanUpSteam(object sender, EventArgs e)
        {
            ResetCounter();
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Steam", "htmlcache");
            
            if (!Directory.Exists(basePath))
            {
                MessageBox.Show(Resources.steam_not_installed, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var directories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "appcache", "httpcache")),
                new DirectoryInfo(Path.Combine(basePath, "blob_storage")),
                new DirectoryInfo(Path.Combine(basePath, "Cache")),
                new DirectoryInfo(Path.Combine(basePath, "Code Cache")),
                new DirectoryInfo(Path.Combine(basePath, "DawnCache")),
                new DirectoryInfo(Path.Combine(basePath, "GPUCache"))
            };

            foreach (var directory in directories)
            {
                _totalFiles += directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count();

                Parallel.ForEach(directory.EnumerateFiles(), file =>
                {
                    try
                    {
                        file.Delete();
                        Interlocked.Increment(ref _filesDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });

                Parallel.ForEach(directory.EnumerateDirectories(), folder =>
                {
                    try
                    {
                        folder.Delete(true);
                        Interlocked.Increment(ref _foldersDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }

            if (_totalFiles == 0)
            {
                MessageBox.Show(Resources.empty_steam_cache, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanUpDiscord(object sender, EventArgs e)
        {
            ResetCounter();
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Roaming", "discord");
            var directories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.Combine(basePath, "blob_storage")),
                new DirectoryInfo(Path.Combine(basePath, "Cache")),
                new DirectoryInfo(Path.Combine(basePath, "Code Cache")),
                new DirectoryInfo(Path.Combine(basePath, "DawnCache")),
                new DirectoryInfo(Path.Combine(basePath, "GPUCache"))
            };
            
            if (!Directory.Exists(basePath))
            {
                MessageBox.Show(Resources.discord_not_installed, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            foreach (var directory in directories)
            {
                _totalFiles += directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count();

                Parallel.ForEach(directory.EnumerateFiles(), file =>
                {
                    try
                    {
                        file.Delete();
                        Interlocked.Increment(ref _filesDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });

                Parallel.ForEach(directory.EnumerateDirectories(), folder =>
                {
                    try
                    {
                        folder.Delete(true);
                        Interlocked.Increment(ref _foldersDeleted);
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }

            if (_totalFiles == 0)
            {
                MessageBox.Show(Resources.empty_discord, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanNvidiaCache(object sender, EventArgs e)
        {
            ResetCounter();
            var nvidiaCacheDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow", "NVIDIA", "PerDriverVersion", "DXCache"));
            
            if (!nvidiaCacheDirectory.Exists)
            {
                nvidiaCacheDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "NVIDIA", "DXCache"));
                if (!nvidiaCacheDirectory.Exists)
                {
                    MessageBox.Show(Resources.nvidia_cache_not_exist, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            
            if (IsFolderEmpty(nvidiaCacheDirectory))
            {
                MessageBox.Show(Resources.empty_nvidia_cache, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _totalFiles += nvidiaCacheDirectory.EnumerateFiles().Count();

            Parallel.ForEach(nvidiaCacheDirectory.EnumerateFiles(), file =>
            {
                try
                {
                    file.Delete();
                    Interlocked.Increment(ref _filesDeleted);
                }
                catch
                {
                    // ignored
                }
            });

            MessageBox.Show(_filesDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanWindowsUpdatePackages(object sender, EventArgs e)
        {
            ResetCounter();
            var windowsUpdatePackagesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));

            if (IsFolderEmpty(windowsUpdatePackagesDirectory))
            {
                MessageBox.Show(Resources.empty_windows_update_directory, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _totalFiles = windowsUpdatePackagesDirectory.EnumerateFiles().Count() + windowsUpdatePackagesDirectory.EnumerateDirectories().Count();

            Parallel.ForEach(windowsUpdatePackagesDirectory.EnumerateFiles(), file =>
            {
                try
                {
                    file.Delete();
                    Interlocked.Increment(ref _filesDeleted);
                }
                catch
                {
                    // ignored
                }
            });

            Parallel.ForEach(windowsUpdatePackagesDirectory.EnumerateDirectories(), folder =>
            {
                try
                {
                    folder.Delete(true);
                    Interlocked.Increment(ref _foldersDeleted);
                }
                catch
                {
                    // ignored
                }
            });

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}