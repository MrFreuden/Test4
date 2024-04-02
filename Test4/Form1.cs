using Velopack.Sources;
using Velopack;

namespace Test4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task Form1_Load(object sender, EventArgs e)
        {
            await UpdateMyApp();
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
