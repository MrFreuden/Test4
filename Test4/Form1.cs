using Velopack.Sources;
using Velopack;
using NuGet.Versioning;
using System.Threading.Channels;
using Velopack.NuGet;
using Microsoft.Extensions.Logging;
using Velopack.Locators;

namespace Test4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await UpdateMyApp();
        }

        private static async Task UpdateMyApp()
        {
            ILogger logger = new FileLogger("log.txt");
            VelopackLocator loc = new WindowsVelopackLocator(logger);
            var token = Environment.GetEnvironmentVariable("TOKEN_TEST4");
            var mgr = new UpdateManager(new GithubSource("https://github.com/MrFreuden/Test4", token, false), null, logger);
            if (!mgr.IsInstalled)
            {
                logger.LogInformation("test1");
                IVelopackLocator locator = VelopackLocator.GetDefault(logger);
                mgr = new UpdateManager(new GithubSource("https://github.com/MrFreuden/Test4", token, false), null, logger, locator);
            }
            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);

            var ourExePath = VelopackRuntimeInfo.EntryExePath;
        }
    }

    class FileLogger : ILogger
    {
        private string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            System.IO.File.AppendAllText(filePath, message + Environment.NewLine);
        }
    }
}
