namespace Libnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Lookup
    {
        /// <summary>
        /// Tagged content sorted by hit count.
        /// </summary>
        /// <param name="tags">Tags to look up.</param>
        /// <param name="sampleSize">Max size of text to return.</param>
        /// <returns>Truncated content of blobs sorted by keyword hit count.</returns>
        public static IEnumerable<string> GetTaggedContentByFrequency(string[] tags, uint sampleSize)
        {
            // matches, sorted by frequency
            var matches = GetKeywordDictionary(tags).OrderByDescending(m => m.Value);

            foreach (var match in matches)
            {
                yield return Blob.Read(match.Key, sampleSize);
            }
        }

        /// <summary>
        /// Gets dictionary of all blob identities to number of keyword hits.
        /// </summary>
        /// <param name="tags">Tags to look for.</param>
        /// <returns>Dictionary of blob hash identities to kit count.</returns>
        public static Dictionary<byte[], uint> GetKeywordDictionary(string[] tags)
        {
            Guard.ThrowIfEmpty(tags, nameof(tags));

            var result = new Dictionary<byte[], uint>(new ByteArrayCompare());
            var buffer = new byte[Hash.Length];

            foreach (var tag in tags)
            {
                var tagHash = Hash.HashString(tag.ToUpper());
                var tagPath = NitPath.GetFullTagPath(tagHash);

                if (File.Exists(tagPath))
                {
                    HashFileReader.Read(tagPath, (input) =>
                    {
                        // if not there, add it with cnt 1
                        if (!result.TryAdd(input, 1))
                        {
                            // it already exists, so increment
                            result[input]++;
                        }

                        return true;
                    });
                }
            }

            return result;
        }
    }
}
