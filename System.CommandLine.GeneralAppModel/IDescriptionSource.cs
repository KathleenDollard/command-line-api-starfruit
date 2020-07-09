using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
 public    interface IDescriptionSource
    {
        string GetDescription(string route);
    }
}
