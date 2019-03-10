namespace Nit.Import
{
    using System;
    using System.Collections.Generic;

    internal class Outline
    {
        private List<string> headings = new List<string>();

        public void SetHeader(string headerLine)
        {
            var depth = this.Depth(headerLine);
            var count = this.headings.Count - depth;

            if (count > 0)
            {
                // shrink to the depth this header line says we're at
                this.headings.RemoveRange(depth, count);
            }
            else if (count < 0)
            {
                for (int i = count; i < 0; i++)
                {
                    this.headings.Add(string.Empty);
                }
            }

            this.headings.Add(this.Normalize(headerLine));
        }

        /// <summary>
        /// Get the tags as currently configured based on headings.
        /// </summary>
        /// <returns>The tag array of appended and split headings.</returns>
        public Span<string> GetTags() => string.Join(" ", this.headings).Split(' ', StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Determines if the line is a header line.
        /// </summary>
        /// <param name="line">The line to examine.</param>
        /// <returns>True if it's a header line.</returns>
        internal bool IsHeader(string line) => line.TrimStart().StartsWith('#');

        /// <summary>
        /// Gets the depth of a header line.
        /// </summary>
        /// <param name="line">The header line.</param>
        /// <returns>The header line's depth.</returns>
        internal int Depth(string line)
        {
            var level = 0;
            var trimmedLine = line.TrimStart();
            foreach (var c in trimmedLine)
            {
                if (c == '#')
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level - 1;
        }

        /// <summary>
        /// Normalize a markdown header for use as a tag set.
        /// </summary>
        /// <param name="line">The header row,</param>
        /// <returns>Potential tags.</returns>
        internal string Normalize(string line) => line.TrimStart('#', ' ');
    }
}
