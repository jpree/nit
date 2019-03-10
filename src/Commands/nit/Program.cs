namespace Nit
{
    using System;
    using Libnit;
    using Nit.Properties;

    /// <summary>
    /// Main program for calling extensions.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main function to handle calling extensions.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>Internal or extension exit code.</returns>
        private static int Main(string[] args)
        {
            try
            {
                Guard.ThrowIfEmpty(args, nameof(args));
                if (args[0].Equals("-?", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("--help", StringComparison.OrdinalIgnoreCase))
                {
                    string helpText = Properties.Resources.HelpInfo;
                    Console.Write(helpText);
                }
                else
                {
                    return Command.TryRunInternal(args) ?? Command.RunExternalExtension(args);
                }
            }
            catch (Exception e)
            {
                var backup = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {e.Message}");
                Console.ForegroundColor = backup;
            }

            return 0;
        }
    }
}
