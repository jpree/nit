namespace Nit
{
    using System;
    using System.Globalization;
    using Libnit;
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Main extension class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Extension arguments.</param>
        /// <returns>Exit code.</returns>
        private static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "nit-node";
            app.Description = "Get objects that match tags.";
            app.HelpOption("-?|-h|--help");

            var tagsOpt = app.Option("-t", "Tags to match on", CommandOptionType.MultipleValue);
            var contentOpt = app.Option("-p", "Show content", CommandOptionType.NoValue);
            var sampleOpt = app.Option("-a", "Sample size", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                try
                {
                    if (contentOpt.HasValue())
                    {
                        var sampleSize = uint.TryParse(sampleOpt.Value(), out var i) ? i : 100;
                        var concat = string.Join(' ', tagsOpt.Values);
                        var tags = concat.ToUpper(CultureInfo.InvariantCulture).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        var result = Lookup.GetTaggedContentByFrequency(tags, sampleSize);
                        foreach (var match in result)
                        {
                            Console.WriteLine(match);
                        }
                    }
                    else
                    {
                        var concat = string.Join(' ', tagsOpt.Values);
                        var tags = concat.ToUpper(CultureInfo.InvariantCulture).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        var result = Lookup.GetKeywordDictionary(tags);
                        foreach (var match in result)
                        {
                            var value = ((Span<byte>)match.Key).GetHexString();
                            Console.WriteLine($"{value}\t {match.Value}");
                        }
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
