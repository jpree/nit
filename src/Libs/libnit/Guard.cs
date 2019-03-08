namespace Libnit
{
    using System;

    public static class Guard
    {
        public static void ThrowIfNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ThrowIfEmpty(object[] argumentValue, string argumentName)
        {
            ThrowIfNull(argumentValue, argumentName);
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException(argumentName);
            }
        }

        public static void ThrowIfEmpty(string argumentValue, string argumentName)
        {
            ThrowIfNull(argumentValue, argumentName);
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException(argumentName);
            }
        }
    }
}
