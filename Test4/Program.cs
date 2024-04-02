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
        static void Main()
        {
            VelopackApp.Build().Run();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        
    }
}