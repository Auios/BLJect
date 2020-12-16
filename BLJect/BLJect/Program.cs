using System;
using System.Diagnostics;
using System.IO;

namespace BLJect
{
    class Program
    {
        // Source: https://stackoverflow.com/a/5519517/5761668
        static void ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }

        static void Main()
        {
            Process[] processes = Process.GetProcessesByName("blockland");

            if(processes.Length > 0)
            {
                foreach(Process process in processes)
                {
                    Console.WriteLine($"Injecting into '{process.Id}' ...");

                    // RemoteDLLInjector32.exe must be in Path or in same directory as this executable.
                    //ExecuteCommand($"RemoteDLLInjector32.exe {process.Id} Chams.dll");
                    string cmd = $"RemoteDLLInjector32.exe {process.Id} {Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/blocksense.dll";
                    Console.WriteLine(cmd);
                    ExecuteCommand(cmd);
                }
            }
            else
            {
                Console.WriteLine("Failed to find any instances of Blockland!");
            }
        }
    }
}
