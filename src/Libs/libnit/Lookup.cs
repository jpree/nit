namespace Libnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Lookup
    {
        public static IEnumerable<string> GetTaggedContentByFrequency(string[] tags, uint sampleSize)
        {
            // matches, sorted by frequency
            var matches = GetKeywordDictionary(tags).OrderByDescending(m => m.Value);

            foreach (var match in matches)
            {
                yield return Blob.Read(match.Key, sampleSize);
            }
        }

        public static Dictionary<byte[], uint> GetKeywordDictionary(string[] tags)
        {
            Guard.ThrowIfEmpty(tags, nameof(tags));

            var result = new Dictionary<byte[], uint>(new ByteArrayCompare());
            var buffer = new byte[Hash.Length];

            foreach (var tag in tags)
            {
                var tagHash = Hash.HashString(tag);
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
