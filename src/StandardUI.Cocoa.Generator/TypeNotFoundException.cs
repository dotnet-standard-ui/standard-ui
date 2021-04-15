using System;
namespace StandardUI.Cocoa.Generator
{
    class TypeNotFoundException : Exception
    {
        public TypeNotFoundException(string qualifiedTypeName) : base($"Could not find {qualifiedTypeName}") =>
            QualifiedTypeName = qualifiedTypeName;

        public string QualifiedTypeName { get; }
    }
}
