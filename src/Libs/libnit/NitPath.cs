namespace Libnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class NitPath
    {
        private const string TagFolder = "tag";
        private const string ObjectFolder = "obj";
        private const string NitRootFolder = ".nit";

        static NitPath()
        {
            RootFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), NitRootFolder);
        }

        public static string RootFolder { get; private set; }

        public static void OverrideRootFolder(string rootFolder)
        {
            RootFolder = rootFolder;
        }

        public static string GetDir(Span<byte> hash) => hash.Slice(0, 2).GetHexString();

        public static string GetFileName(Span<byte> hash) => hash.Slice(2, hash.Length - 2).GetHexString();

        public static string GetBlobRoot(string subfolder) => Path.Join(RootFolder, subfolder);

        public static string GetDirectoryPath(string subfolder, Span<byte> hash) => Path.Join(GetBlobRoot(subfolder), GetDir(hash));

        public static string GetObjectDirectoryPath(Span<byte> hash) => Path.Join(GetBlobRoot(ObjectFolder), GetDir(hash));

        public static string GetTagDirectoryPath(Span<byte> hash) => Path.Join(GetBlobRoot(TagFolder), GetDir(hash));

        public static string GetFullObjectPath(Span<byte> hash) => GetFullPath(ObjectFolder, hash);

        public static string GetFullTagPath(Span<byte> hash) => GetFullPath(TagFolder, hash);

        private static string GetFullPath(string subfolder, Span<byte> hash) => Path.Join(GetBlobRoot(subfolder), GetDir(hash), GetFileName(hash));
    }
}
