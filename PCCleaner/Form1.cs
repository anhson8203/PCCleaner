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
        private const string Caption = "PC Cleaner"; // MessageBox caption

        private int _totalDeleted;
        private int _totalFiles;

        private long _totalSize;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;

            var toolTip = new ToolTip();

            // Details for each button
            toolTip.SetToolTip(cleanTempBtn, "Clear Windows temporary and prefetch files and folders\n\nNote: This might slightly affect your machine performance!");
            toolTip.SetToolTip(cleanSteamBtn, "Clear Steam caches and temporary files");
            toolTip.SetToolTip(cleanDiscordBtn, "Clear Discord caches and temporary files");
            toolTip.SetToolTip(cleanShaderBtn, "Clear graphics driver shader caches");
        }
        
        private static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static void Form1_Load(object sender, EventArgs e)
        {
            if (IsAdmin()) return;
            MessageBox.Show(Resources.administrative_privileges_warning, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ResetCounter()
        {
            _totalDeleted = 0;
            _totalFiles = 0;
            _totalSize = 0L;
        }

        private static long GetDirectorySize(DirectoryInfo directory) => directory.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);

        private static string FormatSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private void ClearCache(List<DirectoryInfo> directories, int totalFiles)
        {
            if (totalFiles == 0)
            {
                MessageBox.Show(Resources.empty_folder, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            // Reset the all counters
            ResetCounter();

            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            foreach (var directory in directories)
            {
                var files = directory.EnumerateFiles().ToList();
                var folders = directory.EnumerateDirectories().ToList();

                Parallel.ForEach(files, options, file =>
                {
                    try
                    {
                        file.Delete();
                        
                        Interlocked.Add(ref _totalSize, file.Length);
                        Interlocked.Increment(ref _totalDeleted);
                    }
                    catch
                    {
                        // ignore
                        
                        // For debugging purposes
                        // Console.WriteLine(@"Error deleting file " + file.FullName + @" error: " + e.Message);
                    }
                });

                Parallel.ForEach(folders, options, folder =>
                {
                    try
                    {
                        folder.Delete(true);
                        
                        Interlocked.Add(ref _totalSize, GetDirectorySize(folder));
                        Interlocked.Increment(ref _totalDeleted);
                    }
                    catch
                    {
                        // ignore
                        
                        // For debugging purposes
                        // Console.WriteLine(@"Error deleting folder " + folder.FullName + @" error: " + e.Message);
                    }
                });
            }
            
            if (_totalDeleted == 0)
            {
                MessageBox.Show(Resources.no_item_removed, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            MessageBox.Show(_totalDeleted + Resources.items_removed + Resources.total_size + FormatSize(_totalSize), @"PC Cleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearWindows(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                MessageBox.Show(Resources.admin_required, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var generalDirectories = new List<DirectoryInfo>
            {
                new DirectoryInfo(Path.GetTempPath()),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Temp")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "Microsoft", "Windows", "Explorer")),
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "CrashDumps")),
                new DirectoryInfo(Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "D3DSCache"))
            };
            
            _totalFiles = generalDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(generalDirectories, _totalFiles);
        }

        private void ClearSteam(object sender, EventArgs e)
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
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "logs")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "blob_storage")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "Cache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "Code Cache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "DawnCache")),
                new DirectoryInfo(Path.Combine(baseSteamPath, "GPUCache"))
            };

            _totalFiles = steamDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(steamDirectories, _totalFiles);
        }

        private void ClearDiscord(object sender, EventArgs e)
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

            _totalFiles = discordDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(discordDirectories, _totalFiles);
        }

        private void ClearShader(object sender, EventArgs e)
        {
            var nvidiaCacheDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow", "NVIDIA", "PerDriverVersion", "DXCache"));

            if (!nvidiaCacheDirectory.Exists)
            {
                nvidiaCacheDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Local", "NVIDIA", "DXCache"));
                
                if (!nvidiaCacheDirectory.Exists)
                {
                    MessageBox.Show(Resources.nvidia_cache_not_exist, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            var shaderCacheDirectories = new List<DirectoryInfo>{ nvidiaCacheDirectory };
            
            _totalFiles = shaderCacheDirectories.Sum(directory => directory.EnumerateFiles().Count() + directory.EnumerateDirectories().Count());
            ClearCache(shaderCacheDirectories, _totalFiles);
        }
    }
}