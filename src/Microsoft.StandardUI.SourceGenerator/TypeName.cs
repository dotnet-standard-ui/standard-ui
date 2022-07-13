namespace Microsoft.StandardUI.SourceGenerator
{
    public class TypeName
    {
        public string Namespace { get; }

        public string Name { get; }

        public string FullName => $"{Namespace}.{Name}";

        public TypeName(string @namespace, string type)
        {
            Namespace = @namespace;
            Name = type;
        }
    }
}
