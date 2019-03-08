namespace Libnit
{
    using System;
    using System.Collections.Generic;

    public class ByteArrayCompare : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return left == right;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // it's already a sha256 hash, so just return the first 4 bytes
            return BitConverter.ToInt32(key);
        }
    }
}
