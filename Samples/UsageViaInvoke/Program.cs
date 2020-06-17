using System;
using System.CommandLine.GeneralAppModel;

namespace NormalUsageViaInvoke
{
    class Program
    {
        static void Main(string[] args)
        {
            return Strategy.Standard.InvokeMethod<ManageGlobalJson>(args);
        }
    }
}
