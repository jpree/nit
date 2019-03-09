namespace Nit
{
    using System;
    using Libnit;

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
                return Command.TryRunInternal(args) ?? Command.RunExternalExtension(args);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"nit <command> -[options] [parameters]");
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
