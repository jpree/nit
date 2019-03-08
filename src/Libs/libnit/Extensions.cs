namespace Libnit
{
    using System;

    public static class Extensions
    {
        public static string GetHexString(this Span<byte> bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[(i * 2) + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }

        public static Span<byte> GetBinary(this string hex)
        {
            if (hex.Length % 2 == 1)
            {
                throw new Exception("The binary key cannot have an odd number of digits");
            }

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetUppercaseHexVal(hex[i << 1]) << 4) + GetUppercaseHexVal(hex[(i << 1) + 1]));
            }

            return arr;
        }

        private static int GetUppercaseHexVal(char hex)
        {
            int val = (int)hex;

            // For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);

            // For lowercase a-f letters:
            // return val - (val < 58 ? 48 : 87);

            // Or the two combined, but a bit slower:
            // return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
