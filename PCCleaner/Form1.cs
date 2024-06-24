using System;
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
        
        private static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        
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
        
        private void ClearCache(List<DirectoryInfo> directories, int totalFiles)
        {
            foreach (var directory in directories)
            {
                var files = directory.EnumerateFiles().ToList();
                var folders = directory.EnumerateDirectories().ToList();
                
                Parallel.ForEach(files, file =>
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

                Parallel.ForEach(folders, folder =>
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
            
            if (totalFiles == 0)
            {
                MessageBox.Show(Resources.empty_folder, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(_filesDeleted + Resources.files_deleted + _foldersDeleted + Resources.folders_deleted + _totalFiles, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanGeneralFolder(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                MessageBox.Show(Resources.admin_required, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            ResetCounter();
            var generalDirectories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.GetTempPath()),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Microsoft", "Windows", "Explorer")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "CrashDumps")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "D3DSCache"))
            };
            
            _totalFiles = generalDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(generalDirectories, _totalFiles);
        }
        
        private void CleanUpSteam(object sender, EventArgs e)
        {
            var baseSteamPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Steam", "htmlcache");
            
            if (!Directory.Exists(baseSteamPath))
            {
                MessageBox.Show(Resources.steam_not_installed, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var steamDirectories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "appcache", "httpcache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "blob_storage")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "Cache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "Code Cache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "DawnCache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "GPUCache"))
            };
            
            ResetCounter();
            _totalFiles = steamDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(steamDirectories, _totalFiles);
        }
        
        private void CleanUpDiscord(object sender, EventArgs e)
        {
            var baseDiscordPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Roaming", "discord");
            
            if (!Directory.Exists(baseDiscordPath))
            {
                MessageBox.Show(Resources.discord_not_installed, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var discordDirectories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.Combine(baseDiscordPath, "blob_storage")),
                new DirectoryInfo(Path.Combine(baseDiscordPath, "Cache")),
                new DirectoryInfo(Path.Combine(baseDiscordPath, "Code Cache")),
                new DirectoryInfo(Path.Combine(baseDiscordPath, "DawnCache")),
                new DirectoryInfo(Path.Combine(baseDiscordPath, "GPUCache"))
            };
            
            ResetCounter();
            _totalFiles = discordDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(discordDirectories, _totalFiles);
        }
        
        private void CleanNvidiaCache(object sender, EventArgs e)
        {
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
            
            ResetCounter();
            _totalFiles = nvidiaCacheDirectory.EnumerateFiles().Count() + nvidiaCacheDirectory.EnumerateDirectories().Count();
            ClearCache(new List<DirectoryInfo> { nvidiaCacheDirectory }, _totalFiles);
        }
        
        private void CleanWindowsUpdatePackages(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                MessageBox.Show(Resources.admin_required, @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            ResetCounter();
            var windowsUpdatePackagesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));

            _totalFiles = windowsUpdatePackagesDirectory.EnumerateFiles().Count() + windowsUpdatePackagesDirectory.EnumerateDirectories().Count();
            ClearCache(new List<DirectoryInfo> { windowsUpdatePackagesDirectory }, _totalFiles);
        }
    }
}