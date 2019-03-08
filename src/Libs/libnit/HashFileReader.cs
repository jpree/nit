namespace Libnit
{
    using System;
    using System.IO;
    using System.Text;

    public static class HashFileReader
    {
        public static void Read(string filePath, Func<byte[], bool> callback)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            byte[] buffer = new byte[Hash.Length];

            using (var file = File.OpenRead(filePath))
            {
                if (file.Length % Hash.LineLength != 0)
                {
                    throw new Exception("Tag file size mismatch");
                }

                // number of lines
                var hashes = file.Length / Hash.LineLength;

                for (int i = 0; i < hashes; i++)
                {
                    file.Read(buffer, 0, Hash.Length);

                    var value = Encoding.UTF8.GetString(buffer);
                    var lineHash = Extensions.GetBinary(value);
                    if (!callback(lineHash.ToArray()))
                    {
                        break;
                    }

                    // read newline and ignore
                    file.Read(buffer, 0, Environment.NewLine.Length);
                }
            }
        }
    }
}
