using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class ClassSource
    {
        public Context Context { get; }

        public Usings Usings { get; }

        public string GeneratedFrom { get; }

        public Source Attributes { get; }
        public string NamespaceName { get; }
        public string ClassName { get; }
        public string? DerivedFrom { get; set; }

        public Source StaticFields { get; }
        public Source NonstaticFields { get; }

        public Source DefaultConstructorBody { get; }
        public Source StaticMethods { get; }
        public Source NonstaticMethods { get; }

        public ClassSource(Context context, string generatedFrom, string namespaceName, string className)
        {
            Context = context;
            Usings = new Usings(context, namespaceName);

            GeneratedFrom = generatedFrom;

            Attributes = new Source(context, Usings);
            NamespaceName = namespaceName;
            ClassName = className;

            StaticFields = new Source(context, Usings);
            NonstaticFields = new Source(context, Usings);

            DefaultConstructorBody = new Source(context, Usings);
            StaticMethods = new Source(context, Usings);
            NonstaticMethods = new Source(context, Usings);
        }

        public Source Generate()
        {
            Source fileSource = new Source(Context);

            fileSource.AddLine($"// This file is generated from {GeneratedFrom}. Update the source file to change its contents.");
            fileSource.AddBlankLine();

            Source usingsDeclarations = Usings.Generate();
            if (!usingsDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingsDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {NamespaceName}",
                "{");

            using (fileSource.Indent())
            {
                Source classBody = new Source(Context);
                if (!StaticFields.IsEmpty)
                    classBody.AddSource(StaticFields);
                if (!StaticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(StaticMethods);
                }
                if (!NonstaticFields.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(NonstaticFields);
                }
                if (!DefaultConstructorBody.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();

                    classBody.AddLines(
                        $"public {ClassName}()",
                        "{");
                    using (classBody.Indent())
                        classBody.AddSource(DefaultConstructorBody);
                    classBody.AddLine(
                        "}");
                }
                if (!NonstaticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(NonstaticMethods);
                }

                if (! Attributes.IsEmpty)
                    fileSource.AddSource(Attributes);
                if (DerivedFrom != null)
                    fileSource.AddLine(
                        $"public class {ClassName} : {DerivedFrom}");
                else
                    fileSource.AddLine(
                        $"public class {ClassName}");
                fileSource.AddLine(
                    "{");
                using (fileSource.Indent())
                    fileSource.AddSource(classBody);
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        public void AddToOutput(UIFramework uiFramework)
        {
            Source fileSource = Generate();
            Context.Output.AddSource(uiFramework, NamespaceName, ClassName, fileSource);
        }
    }
}
