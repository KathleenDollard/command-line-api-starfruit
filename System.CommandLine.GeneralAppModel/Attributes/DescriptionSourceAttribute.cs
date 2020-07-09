using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
   public  class DescriptionSourceAttribute : Attribute 
    {
        public DescriptionSourceAttribute(Type descriptionSource)
        {
            if (!typeof(IDescriptionSource ).IsAssignableFrom (descriptionSource ))
            {
                throw new InvalidOperationException("Description source must implement IDescriptionSource interface");
            }
            DescriptionSource = descriptionSource;
        }

        public Type DescriptionSource { get; }
    }
}
