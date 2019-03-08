namespace Libnit
{
    using System;
    using System.IO;

    /// <summary>
    /// Managing objects.
    /// </summary>
    public static class Blob
    {
        /// <summary>
        /// Write a blob to the object index.
        /// </summary>
        /// <param name="blob">Blob to write.</param>
        /// <returns>Hashed blob.</returns>
        public static Span<byte> Write(Span<byte> blob)
        {
            var hash = Hash.HashObject(blob);
            var blobPath = NitPath.GetFullObjectPath(hash);

            // ensure folders exist
            var blobFolder = NitPath.GetObjectDirectoryPath(hash);
            Directory.CreateDirectory(blobFolder);

            File.WriteAllBytes(blobPath, blob.ToArray());
            return hash;
        }

        /// <summary>
        /// Copy source file to object index.
        /// </summary>
        /// <param name="targetPath">Path to write blob.</param>
        /// <returns>Hash of blob written.</returns>
        public static Span<byte> Write(string targetPath)
        {
            if (!File.Exists(targetPath))
            {
                throw new ArgumentException("Target does not exist.");
            }

            var hash = Hash.HashFile(targetPath);
            var fullPath = NitPath.GetFullObjectPath(hash);
            var dirPath = NitPath.GetObjectDirectoryPath(hash);

            if (File.Exists(fullPath))
            {
                // already exists, don't bother
                return hash;
            }

            // ensure the folders exist
            Directory.CreateDirectory(dirPath);

            // copy the file, using the hex hash as the filename
            File.Copy(targetPath, fullPath);
            return hash;
        }

        /// <summary>
        /// Read the entry for a given hash.
        /// </summary>
        /// <param name="hash">Hash to look up.</param>
        /// <returns>Byte array representing the object.</returns>
        public static Span<byte> Read(Span<byte> hash)
        {
            if (!IsValid(hash))
            {
                throw new SignatureException("Hash mismatch");
            }

            var fullPath = NitPath.GetFullObjectPath(hash);

            if (!File.Exists(fullPath))
            {
                // already exists, don't bother
                return null;
            }

            return File.ReadAllBytes(fullPath);
        }

        /// <summary>
        /// Read some of a file and return as a string.
        /// </summary>
        /// <param name="hash">Hash to look up.</param>
        /// <param name="sampleSize">Amount of content to return.</param>
        /// <returns>Byte array representing the object.</returns>
        public static string Read(Span<byte> hash, uint sampleSize)
        {
            if (!IsValid(hash))
            {
                throw new SignatureException("Hash mismatch");
            }

            var fullPath = NitPath.GetFullObjectPath(hash);

            if (!File.Exists(fullPath))
            {
                // already exists, don't bother
                return null;
            }

            var buffer = new char[sampleSize];
            var bufferSpan = (Span<char>)buffer;
            using (var sr = new StreamReader(fullPath))
            {
                sr.ReadBlock(bufferSpan);
            }

            return bufferSpan.ToString();
        }

        /// <summary>
        /// Check if file is valid.
        /// </summary>
        /// <param name="hash">The file's hash.</param>
        /// <returns>If the hash still matches.</returns>
        public static bool IsValid(Span<byte> hash)
        {
            var fullPath = NitPath.GetFullObjectPath(hash);
            var hashCheck = Hash.HashFile(fullPath);
            return hash.SequenceEqual(hashCheck);
        }
    }
}
