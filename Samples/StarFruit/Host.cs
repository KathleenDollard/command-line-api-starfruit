using System;

namespace StarFruit
{
    /// <summary>
    /// Dummy class to show what a Main might look like
    /// </summary>
    internal class Host
    {
        private static ManageGlobalJson arg;

        internal static Builder Build(ManageGlobalJson arg)
        {
            return new Builder(arg); 
        }

        internal static Builder Build(string[] args)
        {
            return new Builder(arg);
        }
    }

    internal class Builder
    {
        private ManageGlobalJson arg;

        public Builder(ManageGlobalJson arg)
        {
            this.arg = arg;
        }

        internal void Run()
        {
            throw new NotImplementedException();
        }
    }
}