namespace Libnit
{
    using System;

    public class SignatureException : ApplicationException
    {
        public SignatureException(string message)
            : base(message)
        {
        }
    }
}
