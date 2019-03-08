namespace Libnit
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    // nit <verb> <noun> <-switch options>
    public static class Hash
    {
        public const int Length = 64;

        public static int LineLength { get => Length + Environment.NewLine.Length; }

        /// <summary>
        /// Hashes a byte array.
        /// </summary>
        /// <param name="content">Content to hash.</param>
        /// <returns>Hash of the content.</returns>
        public static Span<byte> HashObject(Span<byte> content)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(content.ToArray());
            }
        }

        /// <summary>
        /// Hashes a byte array.
        /// </summary>
        /// <param name="content">Content to hash.</param>
        /// <returns>Hash of the content.</returns>
        public static Span<byte> HashString(string content)
        {
            var inputBytes = Encoding.UTF8.GetBytes(content);
            return HashObject(inputBytes);
        }

        /// <summary>
        /// Hashes a file, using a file stream.
        /// </summary>
        /// <param name="path">Absolute or relative file path.</param>
        /// <returns>Hashed byte array</returns>
        public static Span<byte> HashFile(string path)
        {
            using (var sha256 = SHA256.Create())
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                file.Position = 0;
                return sha256.ComputeHash(file);
            }
        }
    }
}
