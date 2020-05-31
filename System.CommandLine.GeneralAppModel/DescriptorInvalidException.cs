using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    // Jon: What's the current thinking on derived exceptions. I think they should be rare, but here they allow better to be returned. I'm using an inner exception.

    public class DescriptorInvalidException : Exception
    {
        public DescriptorInvalidException(IEnumerable<ValidationFailureInfo> validationMessages)
        {
            ValidationFailures = validationMessages;
        }

        public IEnumerable<ValidationFailureInfo> ValidationFailures { get; }
    }
}
