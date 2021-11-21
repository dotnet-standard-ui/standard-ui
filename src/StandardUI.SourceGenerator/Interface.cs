﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Interface
    {
        public Context Context { get; }
        public INamedTypeSymbol Type { get; }
        public InterfacePurpose Purpose { get; }
        public INamespaceSymbol Namespace { get; }
        public string NamespaceName { get; }
        public string? ChildNamespaceName { get; }
        public string FrameworkClassName { get; }
        public INamedTypeSymbol? AttachedType { get; }
        public string Name { get; }
        public string VariableName { get; }
        public INamedTypeSymbol? LayoutManagerType { get; }

        public static InterfacePurpose? IdentifyPurpose(INamedTypeSymbol type)
        {
            // Skip ...Attached interfaces, processing them when their paired main interface is processed instead
            if (type.Name.EndsWith("Attached"))
                return null;

            foreach (AttributeData attribute in type.GetAttributes())
            {
                var attributeTypeFullName = Utils.GetTypeFullName(attribute.AttributeClass);

                if (attributeTypeFullName == "Microsoft.StandardUI.StandardPanelAttribute")
                    return InterfacePurpose.StandardPanel;
                else if (attributeTypeFullName == "Microsoft.StandardUI.UIModelObjectAttribute")
                    return InterfacePurpose.StandardUIObject;
                else if (attributeTypeFullName == "Microsoft.StandardUI.StandardControlAttribute")
                    return InterfacePurpose.StandardControl;
                else continue;
            }

            return null;
        }

        public Interface(Context context, INamedTypeSymbol type)
        {
            Context = context;

            Type = type;
            Name = type.Name;
            if (!Name.StartsWith("I"))
                throw new UserViewableException($"Data model interface {Name} must start with 'I'");

            InterfacePurpose? purpose = IdentifyPurpose(type);
            if (! purpose.HasValue)
                throw new UserViewableException($"Interface {type} doesn't have expected attributes indicating purpose");
            Purpose = purpose.Value;

            FrameworkClassName = Name.Substring(1);

            // Form the default variable name for the interface by dropping the "I" and lower casing the first letter(s) after (ICanvas => canvas)
            VariableName = Utils.TypeNameToVariableName(Name.Substring(1));

            Namespace = Type.ContainingNamespace;
            NamespaceName = Utils.GetNamespaceFullName(Namespace);
            ChildNamespaceName = Utils.GetChildNamespaceName(NamespaceName);

            // Get attached type, if it exists
            string fullNameAttached = Utils.GetTypeFullName(type) + "Attached";
            AttachedType = Context.Compilation.GetTypeByMetadataName(fullNameAttached);

            if (Purpose == InterfacePurpose.StandardPanel)
            {
                string layoutManagerFullName = $"{NamespaceName}.{Name.Substring(1)}LayoutManager";
                LayoutManagerType = Context.Compilation.GetTypeByMetadataName(layoutManagerFullName);

                if (LayoutManagerType == null)
                {
                    throw new UserViewableException($"No type {layoutManagerFullName} found for StandardPanel interface {Name}");
                }
            }
        }

        public void Generate(UIFramework uiFramework)
        {
            var frameworkNamespaceName = uiFramework.ToFrameworkNamespaceName(Namespace);

            var usings = new Usings(Context, frameworkNamespaceName);
            var extensionsClassUsings = new Usings(Context, NamespaceName);

            var properties = new List<Property>();

            var stubSourceForUsings = new Source(Context, usings);
            var mainClassStaticFields = new Source(Context, usings);
            var mainClassStaticMethods = new Source(Context, usings);
            var mainClassNonstaticFields = new Source(Context, usings);
            var mainClassNonstaticMethods = new Source(Context, usings);

            var extensionClassMethods = new Source(Context, extensionsClassUsings);
            var attachedClassStaticFields = new Source(Context, usings);
            var attachedClassMethods = new Source(Context, usings);

            // Add the property descriptors and accessors
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                var property = new Property(Context, this, propertySymbol);
                properties.Add(property);

                uiFramework.GeneratePropertyDescriptor(property, mainClassStaticFields);
                uiFramework.GeneratePropertyField(property, mainClassNonstaticFields);
                uiFramework.GeneratePropertyMethods(property, mainClassNonstaticMethods);
                property.GenerateExtensionClassMethods(extensionClassMethods);
            }

            if (Utils.IncludeDraw(Type))
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();
                mainClassNonstaticMethods.AddLine(
                    $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{FrameworkClassName}(this);");
            }

            // Add a special case for the WPF visual tree child methods for Panel; later we'll generalize this as needed
            if (Name == "IPanel" && uiFramework is WpfUIFramework)
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();

                mainClassNonstaticMethods.AddLine(
                    "protected override int VisualChildrenCount => _uiElementCollection.Count;");
                mainClassNonstaticMethods.AddBlankLine();
                mainClassNonstaticMethods.AddLine(
                    "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _uiElementCollection[index];");
            }

            if (Purpose == InterfacePurpose.StandardPanel)
            {
                uiFramework.GenerateStandardPanelLayoutMethods(mainClassNonstaticMethods, LayoutManagerType!.Name);
            }

            // If there are any attached properties, add the property descriptors and accessors for them
            if (AttachedType != null)
            {
                foreach (ISymbol member in AttachedType.GetMembers())
                {
                    if (!(member is IMethodSymbol getterMethod))
                        continue;

                    // We just process the Get 
                    string methodName = getterMethod.Name;
                    if (!methodName.StartsWith("Get"))
                    {
                        if (!methodName.StartsWith("Set"))
                            throw new UserViewableException(
                                $"Attached type method {AttachedType.Name}.{methodName} doesn't start with Get or Set");
                        else continue;
                    }

                    string propertyName = methodName.Substring("Get".Length);
                    string setterMethodName = "Set" + propertyName;
                    IMethodSymbol? setterMethod = (IMethodSymbol?) AttachedType.GetMembers(setterMethodName).FirstOrDefault();

                    var attachedProperty = new AttachedProperty(Context, this, AttachedType, getterMethod, setterMethod);

                    uiFramework.GenerateAttachedPropertyDescriptor(attachedProperty, mainClassStaticFields);
                    uiFramework.GenerateAttachedPropertyMethods(attachedProperty, mainClassStaticMethods);
                    uiFramework.GenerateAttachedPropertyAttachedClassMethods(attachedProperty, attachedClassMethods);
                }
            }

            usings.AddTypeNamespace(Type);
            usings.AddNamespace(frameworkNamespaceName);
            uiFramework.AddUsings(usings, !mainClassStaticFields.IsEmpty, OutpuHasTypeConverterAttribute(uiFramework));
            Source usingDeclarations = GenerateUsingDeclarations(uiFramework, usings);

            string? destinationBaseClass = GetOutputBaseClass(uiFramework);

            Source? constructor = GenerateConstructor(uiFramework, properties);

            string frameworkOutputDirectory = uiFramework.GetOutputDirectory(ChildNamespaceName);

            string mainClassDerviedFrom;
            if (destinationBaseClass == null)
                mainClassDerviedFrom = Name;
            else
                mainClassDerviedFrom = $"{destinationBaseClass}, {Name}";

            bool isPartial = Utils.IsPanelSubclass(Type);

            Source mainClassSource = GenerateClassFile(usings, frameworkNamespaceName, FrameworkClassName, mainClassDerviedFrom, isPartial: isPartial,
                constructor: constructor, staticFields: mainClassStaticFields, staticMethods: mainClassStaticMethods, nonstaticFields: mainClassNonstaticFields,
                nonstaticMethods: mainClassNonstaticMethods);
            mainClassSource.WriteToFile(frameworkOutputDirectory, FrameworkClassName + ".cs");

            if (AttachedType != null)
            {
                string attachedClassName = FrameworkClassName + "Attached";
                string attachedClassDerivedFrom = AttachedType.Name;

                attachedClassStaticFields.AddLine($"public static {attachedClassName} Instance = new {attachedClassName}();");

                Source attachedClassSource = GenerateClassFile(usings, frameworkNamespaceName, attachedClassName, attachedClassDerivedFrom,
                    staticFields: attachedClassStaticFields, nonstaticMethods: attachedClassMethods);
                attachedClassSource.WriteToFile(frameworkOutputDirectory, attachedClassName + ".cs");
            }

            if (!extensionClassMethods.IsEmpty)
            {
                string extensionsClassName = FrameworkClassName + "Extensions";
                Source extensionsClassSource = GenerateStaticClassFile(extensionsClassUsings, NamespaceName, extensionsClassName, extensionClassMethods);
                extensionsClassSource.WriteToFile(Context.GetSharedOutputDirectory(ChildNamespaceName), extensionsClassName + ".cs");
            }
        }

        public Source GenerateClassFile(Usings usings, string namespaceName, string className, string derivedFrom, bool isPartial = false,
            Source? constructor = null, Source? staticFields = null, Source? staticMethods = null, Source? nonstaticFields = null, Source? nonstaticMethods = null)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            Source usingsDeclarations = usings.Generate();
            if (!usingsDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingsDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                Source classBody = new Source(Context);
                if (staticFields != null && !staticFields.IsEmpty)
                    classBody.AddSource(staticFields);
                if (staticMethods != null && !staticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(staticMethods);
                }
                if (nonstaticFields != null && !nonstaticFields.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticFields);
                }
                if (constructor != null && !constructor.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(constructor);
                }
                if (nonstaticMethods != null && !nonstaticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticMethods);
                }

                string modifiers = isPartial ? "public partial" : "public";
                fileSource.AddLines(
                    $"{modifiers} class {className} : {derivedFrom}",
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

        public Source GenerateStaticClassFile(Usings usings, string namespaceName, string className, Source staticMethods)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            Source usingDeclarations = usings.Generate();
            if (!usingDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                fileSource.AddLines(
                    $"public static class {className}",
                    "{");
                using (fileSource.Indent())
                {
                    fileSource.AddSource(
                        staticMethods);
                }
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        private void GenerateFileHeader(Source fileSource)
        {
            fileSource.AddLine($"// This file is generated from {Name}.cs. Update the source file to change its contents.");
            fileSource.AddBlankLine();
        }

        private Source? GenerateConstructor(UIFramework uiFramework, List<Property> collectionProperties)
        {
            Source constructorBody = new Source(Context);
            foreach (Property property in collectionProperties)
                uiFramework.GeneratePropertyConstructorLines(property, constructorBody);

            if (constructorBody.IsEmpty)
                return null;

            Source constructor = new Source(Context);
            constructor.AddLines(
                $"public {FrameworkClassName}()",
                "{");
            using (constructor.Indent())
                constructor.AddSource(
                    constructorBody);
            constructor.AddLine(
                "}");

            return constructor;
        }

        private Source GenerateUsingDeclarations(UIFramework uiFramework, Usings usings)
        {
            if (OutpuHasTypeConverterAttribute(uiFramework))
                usings.AddNamespace(uiFramework.RootNamespace + ".Converters");

#if false
            foreach (var member in Declaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax modelProperty))
                    continue;

                // Array.Empty requires System
                if (modelProperty.Type is ArrayTypeSyntax)
                    AddUsing(usings, IdentifierName("System"));
            }
#endif

            /*
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                var property = new Property(Context, this, propertySymbol);
                OutputType.AddTypeAliasUsingIfNeeded(usings, property.FrameworkTypeName.ToString());
            }
            */

            return usings.Generate();
        }

        private bool OutpuHasTypeConverterAttribute(UIFramework uiFramework)
        {
            return uiFramework is XamlUIFramework &&
                   (FrameworkClassName == "Geometry" || FrameworkClassName == "Brush");
        }

        private string? GetOutputBaseClass(UIFramework uiFramework)
        {
            string? elementType = Utils.IsCollectionType(Name);
            if (elementType != null)
                return $"StandardUICollection<{elementType}>";

            INamedTypeSymbol? baseInterface = Type.Interfaces.FirstOrDefault();

            if (baseInterface == null)
                return uiFramework.DefaultBaseClassName;
            else
                return uiFramework.OutputTypeName(baseInterface);
        }
    }
}
