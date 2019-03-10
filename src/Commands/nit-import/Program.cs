namespace Nit.Import
{
    using System;
    using System.Globalization;
    using System.Text;
    using Libnit;
    using Microsoft.Extensions.CommandLineUtils;

    /// <summary>
    /// Main extension class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Extension arguments.</param>
        /// <returns>Exit code.</returns>
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "nit-import";
            app.Description = "Import content.";
            app.HelpOption("-?|-h|--help");

            var fileOpt = app.Option("-f|--file <path>", "Path to file.", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                Span<byte> hash = null;
                if (fileOpt.HasValue())
                {
                    Markdown.Import(fileOpt.Value());
                }
                return 1;
            });
            return app.Execute(args);
        }
    }
}
