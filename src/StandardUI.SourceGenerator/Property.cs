﻿using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Property : PropertyBase
    {
        public IPropertySymbol SourceProperty { get; }

        public Property(Context context, Interface intface, IPropertySymbol propertySymbol) :
            base(context, intface, propertySymbol.Name, propertySymbol.Type, propertySymbol.IsReadOnly, intface.Name)
        {
            SourceProperty = propertySymbol;
            SpecifiedDefaultValue = GetSpecifiedDefaultValue(propertySymbol.GetAttributes());
        }

        public void GenerateExtensionClassMethods(Source source)
        {
            if (IsReadOnly)
                return;

            source.Usings.AddTypeNamespace(Type);

            string interfaceVariableName = Interface.VariableName;

            source.AddBlankLineIfNonempty();
            source.AddLines(
                $"public static T {Name}<T>(this T {interfaceVariableName}, {TypeName} value) where T : {Interface.Name}",
                "{");
            using (source.Indent())
            {
                source.AddLines(
                    $"{interfaceVariableName}.{Name} = value;",
                    $"return {interfaceVariableName};");
            }
            source.AddLine(
                "}");
        }
    }
}
