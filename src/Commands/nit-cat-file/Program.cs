namespace Nit.CatFile
{
    using System;
    using System.Text;
    using Libnit;
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Main extension class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Extension arguments.</param>
        /// <returns>Exit code.</returns>
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "nit-cat-file";
            app.Description = "Read back an object.";
            app.HelpOption("-?|-h|--help");

            var hashArg = app.Argument("hash", "An object hash");
            var prettyOpt = app.Option("-p", "Pretty print.", CommandOptionType.NoValue);
            var existOpt = app.Option("-e", "Exit zero status if exists, and non-zero if invalid or error.", CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                try
                {
                    if (prettyOpt.HasValue())
                    {
                        var hash = hashArg.Value.GetBinary();
                        var result = Blob.Read(hash);
                        var content = Encoding.UTF8.GetString(result);
                        Console.WriteLine(content);
                    }
                    else if (existOpt.HasValue())
                    {
                        var hash = hashArg.Value.GetBinary();
                        var exit = Blob.IsValid(hash) ? 0 : -1;
                        return exit;
                    }

                    return 0;
                }
                catch (Exception e)
                {
                    var backup = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = backup;
                    return -1;
                }
            });
            var ex = app.Execute(args);
            return ex;
        }
    }
}
