namespace Nit.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Libnit;
    using System.Linq;

    /// <summary>
    /// Allows for importing markdown as nodes
    /// </summary>
    internal static class Markdown
    {
        /// <summary>
        /// Imports a markdown file into the nit repo. Headers content exists under
        /// are used as tags in the nit repo.
        /// </summary>
        /// <param name="filePath">Makrdown file to import.</param>
        public static void Import(string filePath)
        {
            Guard.ThrowIfEmpty(filePath, nameof(filePath));
            var ext = Path.GetExtension(filePath);

            if (!ext.Equals(".md", StringComparison.OrdinalIgnoreCase) &&
                !ext.Equals(".markdown", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("File must be markdown with either .md or .markdown extension", nameof(filePath));
            }

            var builder = new StringBuilder();
            using (var file = new StreamReader(filePath))
            {
                var outline = new Outline();

                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (outline.IsHeader(line))
                    {
                        // write content if we have it
                        if (builder.Length > 0)
                        {
                            // switching heading, so write blob
                            WriteBlob(outline);

                            // reset for next round.
                            builder.Clear();
                        }

                        // now set the new heading
                        outline.SetHeader(line);
                    }
                    else
                    {
                        builder.AppendLine(line);
                    }
                }

                if (builder.Length > 0)
                {
                    WriteBlob(outline);
                }
            }

            return;

            void WriteBlob(Outline outline)
            {
                // we read content on a previous loop, so write a blob
                var blob = Encoding.UTF8.GetBytes(builder.ToString());
                var hash = Blob.Write(blob);

                // create the tags from the markdown headers
                var tags = outline.GetTags();
                Tag.CreateTags(hash, tags.ToArray());
            }
        }
    }
}
