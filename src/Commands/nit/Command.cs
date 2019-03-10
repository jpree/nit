namespace Nit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Nit extension invocation.
    /// </summary>
    public static class Command
    {
        private static bool Processing { get; set; } = true;

        /// <summary>
        /// Runs internal commands.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Exit code.</returns>
        public static int? TryRunInternal(string[] args)
        {
            var command = args[0];
            var commandArgs = args.AsSpan(1, args.Length - 1).ToArray();
            switch (command)
            {
                case "add":
                    return Nit.Add.Program.Main(commandArgs);
                case "node":
                    return Nit.Node.Program.Main(commandArgs);
                case "cat-file":
                    return Nit.CatFile.Program.Main(commandArgs);
                case "import":
                    return Nit.Import.Program.Main(commandArgs);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Runs the nit extension.
        /// </summary>
        /// <param name="args">Arguments to pass along.</param>
        /// <returns>Exit code of extension.</returns>
        public static int RunExternalExtension(string[] args)
        {
            var command = args[0];
            var proc = new Process();
            proc.StartInfo.FileName = $"nit-{command}.exe";
            proc.StartInfo.Arguments = string.Join(' ', args, 1, args.Length - 1);
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.EnableRaisingEvents = true;
            proc.OutputDataReceived += new DataReceivedEventHandler(HandleOutputData);
            proc.ErrorDataReceived += new DataReceivedEventHandler(HandleOutputData);
            proc.Exited += new EventHandler(Proc_Exited);
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            while (Processing == true)
            {
                Thread.Sleep(0);
            }

            return proc.ExitCode;
        }

        /// <summary>
        /// Process exited.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">argument.</param>
        private static void Proc_Exited(object sender, EventArgs e)
        {
            ((Process)sender).WaitForExit();
            Processing = false;
        }

        private static void HandleOutputData(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}
