using Velopack;
using Velopack.Sources;

namespace Test4
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async void Main()
        {
            await UpdateMyApp();
            VelopackApp.Build().Run();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        private static async Task UpdateMyApp()
        {
            var token = Environment.GetEnvironmentVariable("TOKEN_TEST4");
            var mgr = new UpdateManager(new GithubSource("https://github.com/MrFreuden/Test4", token, false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}