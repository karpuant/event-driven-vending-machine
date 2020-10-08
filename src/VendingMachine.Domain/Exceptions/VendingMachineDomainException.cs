using System;

namespace VendingMachine.Domain.Exceptions
{
    public class VendingMachineDomainException : Exception
    {
        public VendingMachineDomainException()
        { }

        public VendingMachineDomainException(string message)
            : base(message)
        { }

        public VendingMachineDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
