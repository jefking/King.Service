namespace King.Azure.BackgroundWorker.Integration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Configuration;
    using System.Diagnostics;

    [TestClass]
    public class Initialization
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            var emulator = ConfigurationManager.AppSettings["AzureEmulator"];

            using (var process = new Process())
            {
                process.StartInfo = CreateProcessStartInfo(emulator, "/devstore:start");
                process.Start();

                process.WaitForExit();
            }
        }

        private static ProcessStartInfo CreateProcessStartInfo(string fileName, string arguments)
        {
            return new ProcessStartInfo(fileName, arguments)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };
        }
    }
}