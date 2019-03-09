namespace Nit.Add
{
    using System;
    using System.Globalization;
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
            app.Name = "nit-add";
            app.Description = "Add notes and documents.";
            app.HelpOption("-?|-h|--help");

            var messageArg = app.Option("-m|--message <message>", "Content to hash.", CommandOptionType.SingleValue);
            var fileOpt = app.Option("-f|--file <path>", "Path to file.", CommandOptionType.SingleValue);
            var tagsOpt = app.Option("-t|--tag <tags>", "Tags for content.", CommandOptionType.MultipleValue);
            var verboseOpt = app.Option("-v", "Verbose", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                Span<byte> hash = null;
                if (fileOpt.HasValue())
                {
                    hash = Blob.Write(fileOpt.Value());
                }
                else if (messageArg.HasValue())
                {
                    var inputBytes = Encoding.UTF8.GetBytes(messageArg.Value());
                    hash = Blob.Write(inputBytes);
                }

                if (tagsOpt.HasValue())
                {
                    var concat = string.Join(' ', tagsOpt.Values);
                    var tags = concat.ToUpper(CultureInfo.InvariantCulture).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    Tag.CreateTags(hash, tags);
                }

                return 1;
            });
            return app.Execute(args);
        }
    }
}
