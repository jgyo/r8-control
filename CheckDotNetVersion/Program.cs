using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CheckDotNetVersion
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Environment.Exit(-1);
                    return;
                }
                var desiredVersion = args[0];
                var output = "";

                using(Process dotnet = new Process())
                {
                    dotnet.StartInfo.FileName = "dotnet.exe";
                    dotnet.StartInfo.Arguments = "--version";
                    dotnet.StartInfo.UseShellExecute = false;
                    dotnet.StartInfo.RedirectStandardOutput = true;
                    dotnet.Start();

                    output = dotnet.StandardOutput.ReadToEnd();
                    dotnet.WaitForExit();
                }

                Environment.Exit(Comparer(desiredVersion, output) ? 0 : -1);
            }
            catch
            {
                Environment.Exit(-1);
            }
        }

        private static bool Comparer(string desiredVersion, string output)
        {
            var leftArray = desiredVersion.Split('.');
            var rightArray = output.Split('.');

            for (int i = 0; i < Math.Min(leftArray.Length, rightArray.Length); i++)
            {
                if(int.Parse(leftArray[i]) > int.Parse(rightArray[i]))
                    return false;
                if(int.Parse(leftArray[i]) < int.Parse(rightArray[i]))
                    return true;
            }

            return true;
        }
    }
}
