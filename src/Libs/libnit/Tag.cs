namespace Libnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Tag
    {
        /// <summary>
        /// Create tags for a give hash code.
        /// </summary>
        /// <param name="hash">A hash to associate tags with.</param>
        /// <param name="tags">The tags to associate.</param>
        public static void CreateTags(Span<byte> hash, string[] tags)
        {
            foreach (var tag in tags)
            {
                var tagHash = Hash.HashString(tag);
                var fullPath = NitPath.GetFullTagPath(tagHash);
                var directoryPath = NitPath.GetTagDirectoryPath(tagHash);

                if (!File.Exists(fullPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    using (var s = File.CreateText(fullPath))
                    {
                        var l = hash.GetHexString();
                        s.WriteLine(l);
                    }

                    continue;
                }

                // need a value type for lambda
                byte[] temp = hash.ToArray();

                // scan hashes in the file and stop when a match is found
                HashFileReader.Read(fullPath, (input) => !((Span<byte>)temp).SequenceEqual((Span<byte>)input));

                // Add the entry to the end
                var line = hash.GetHexString();
                File.AppendAllLines(fullPath, new List<string>() { line });
            }
        }
    }
}
