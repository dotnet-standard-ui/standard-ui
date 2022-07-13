using System;

namespace Microsoft.StandardUI
{
    public class CreatorNotInitializedException : Exception
    {
        public CreatorNotInitializedException(string factoryName) : base($"Standard UI factory {factoryName} hasn't been initialized yet")
        {
        }
    }
}
