using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace StandardUI.Cocoa.Generator
{
    [Generator]
    public class CocoaToStandardUI : ISourceGenerator
    {
        const string PlatformNamespace = "AppKit";
        const string ProjectedPlatformNamespace = "Microsoft.StandardUI.Cocoa.Native";

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                DoExecute(context);
            }
            catch (TypeNotFoundException e)
            {
                var diagnostic = Diagnostic.Create(
                    "DINO404", "StandardUI", $"Failed to find type {e.QualifiedTypeName}",
                    DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0);
                context.ReportDiagnostic(diagnostic);
            }
        }

        public void Initialize(GeneratorInitializationContext context) { }

        void DoExecute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            bool generatePlatformTypes = compilation.AssemblyName == "StandardUI.Cocoa";
            Types types = new(compilation);

            TranslateTypes translate = new(context, types, generatePlatformTypes);
            if (generatePlatformTypes)
            {
                translate.SourceNamespacePrefix = PlatformNamespace;
                translate.DestNamespacePrefix = ProjectedPlatformNamespace;
                types.NSView.ContainingNamespace.Accept(translate);
            }
        }

        class TranslateTypes : SymbolVisitor
        {
#pragma warning disable RS1024 // Compare symbols correctly
            Dictionary<INamedTypeSymbol, string> translated = new(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly
            GeneratorExecutionContext context;
            Types types;
            bool generatePlatformTypes;
            bool allowInternal;

            public TranslateTypes(GeneratorExecutionContext context, Types types, bool generatePlatformTypes)
            {
                this.context = context;
                this.types = types;
                this.generatePlatformTypes = generatePlatformTypes;
            }

            public string SourceNamespacePrefix { get; set; } = "";
            public string DestNamespacePrefix { get; set; } = "";

            public override void VisitAssembly(IAssemblySymbol symbol)
            {
                allowInternal = symbol.Equals(context.Compilation.Assembly, SymbolEqualityComparer.Default);
                foreach (var member in symbol.GlobalNamespace.GetMembers())
                    member.Accept(this);
                allowInternal = false;
            }

            public override void VisitNamespace(INamespaceSymbol symbol)
            {
                foreach (var type in symbol.GetTypeMembers())
                    type.Accept(this);

                foreach (var ns in symbol.GetNamespaceMembers())
                    ns.Accept(this);
            }

            public override void VisitNamedType(INamedTypeSymbol symbol)
            {
                if ((symbol.IsSubclassOf(types.NSView) || symbol.Equals(types.NSView, SymbolEqualityComparer.Default)) &&
                    IsDefaultConstructable(symbol) &&
                    (generatePlatformTypes || symbol.GetAttributes().Any(types.IsCocoaToStandardUIAttribute)))
                    Translate(symbol);
            }

            string Translate(INamedTypeSymbol cocoaType)
            {
                if (translated.TryGetValue(cocoaType, out var v))
                    return v;

                var qualifiedType = cocoaType.ToDisplayString();
                if (!generatePlatformTypes && qualifiedType.StartsWith("AppKit.") &&
                    cocoaType.ContainingAssembly.Equals(types.NSView.ContainingAssembly, SymbolEqualityComparer.Default))
                {
                    var projected = $"global::{ProjectedPlatformNamespace}.{cocoaType.Name}Base";
                    translated[cocoaType] = projected;
                    return projected;
                }

                qualifiedType = $"global::{qualifiedType}";

                var name = cocoaType.Name;
                string projNamespace;
                string projType;
                var projAttribute = cocoaType.GetAttributes().FirstOrDefault(types.IsCocoaToStandardUIAttribute);
                if (projAttribute != null)
                {
                    // TODO
                    projNamespace = "";
                    projType = "";
                }
                else
                {
                    projNamespace = DestNamespacePrefix + ".";
                    projType = cocoaType.Name;
                }

                var projTypeBase = $"{projType}Base";
                var qualifiedProjTypeBase = $"global::{projTypeBase}";
                translated[cocoaType] = qualifiedProjTypeBase;

                bool isNSView = types.NSView.Equals(cocoaType, SymbolEqualityComparer.Default);
                var access = cocoaType.DeclaredAccessibility.Format();
                StringBuilder source = new();

                source.AppendLine($@"#pragma warning disable CS0108
#pragma warning disable CS8604
namespace {projNamespace}
{{");
                if (!cocoaType.IsSealed)
                {
                    string basedOn = "";
                    if (!isNSView)
                    {
                        var baseType = Translate(cocoaType.BaseType!);
                        basedOn = $" : {baseType}<TSubclass, TView> where TView : {qualifiedType}, new()";
                    }
                    source.AppendLine($@"
    {access} abstract partial class {projTypeBase}<TSubclass, TView> {basedOn}
    {{");
                    WriteConstructors(source, projTypeBase);
                    WriteBase(source, cocoaType, qualifiedProjTypeBase, "TSubclass");
                    source.AppendLine("    }");
                }

                if (IsDefaultConstructable(cocoaType))
                {
                    string parent;
                    if (cocoaType.IsSealed)
                        parent = Translate(cocoaType.BaseType!);
                    else
                        parent = $"global::{projNamespace}.{projTypeBase}";
                    source.AppendLine($@"
    {access} sealed partial class {name} : {parent}<{name}, {qualifiedType}>
    {{");
                    WriteConstructors(source, name);
                    if (cocoaType.IsSealed)
                        WriteBase(source, cocoaType, qualifiedProjTypeBase, name);
                    WriteReal(source, name);
                    source.AppendLine("    }");
                }
                source.AppendLine("}");

            }

            void WriteConstructors(StringBuilder source, string name)
            {
                source.AppendLine($@"
        public {name}() {{}}
        public {name}(
            global::System.Collections.Immutable.ImmutableDictionary<global::System.Reflection.PropertyInfo, global::Microsoft.StandardUI.Native.Internal.ValueInfo> propertyValues,
            global::System.Collections.Immutable.ImmutableDictionary<global::System.Reflection.EventInfo, (global::Microsoft.StandardUI.Native.Internal.IListenerFactory, object)> events,
            global::System.Collections.Immutable.ImmutableList<(global::AppKit.NSGestureRecognizer, object)> gestures) :
            base(propertyValues, events, gestures)
        {{}}
");
            }

            void WriteBase(StringBuilder source, INamedTypeSymbol cocoaType, string qualifedCocoaType, string subclassName)
            {
                string nonPublicFlags = ", global::System.Reflection.BindingFlags.NonPublic";
                foreach (var member in cocoaType.GetMembers())
                {
                    if (member.IsStatic || !IsAccessible(member.DeclaredAccessibility))
                        continue;

                    string flags = "";
                    if (member.DeclaredAccessibility != Accessibility.Public)
                        flags = nonPublicFlags;
                    string access = member.DeclaredAccessibility.Format();

                    if (member is IPropertySymbol property && !property.IsReadOnly && !property.IsWriteOnly && !property.IsWithEvents)
                    {
                        source.AppendLine($@"
            {access} static readonly {property.Name}Property = typeof({qualifedCocoaType}).GetProperty(""{property.Name}{flags}"");
            {access} {subclassName} {property.Name}({property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} value) =>
                Set({property.Name}Property, value);
");
                    }
                    else if (member is IEventSymbol evt)
                    {
                        var invoke = evt.Type.GetMembers("Invoke")
                            .OfType<IMethodSymbol>()
                            .Where(m => m.Parameters.Length == 2)
                            .FirstOrDefault();

                        if (invoke == null)
                            continue;

                        var argsType = invoke.Parameters[1].Type;
                        var argsTypeDisp = argsType.ToDisplayParts(SymbolDisplayFormat.FullyQualifiedFormat);
                        source.AppendLine($@"
            {access} static readonly global::Microsoft.StandardUI.Cocoa.Native.Internal.ListenerFactory<
                {evt.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)},
                {argsTypeDisp}> {evt.Name}Event = new(typeof({qualifedCocoaType}).GetEvent(""{evt.Name}""{flags}));
            {access} {subclassName} {evt.Name}(Action action) =>
                On({evt.Name}Event, (_, _) => event());
            {access} {subclassName} {evt.Name}(Action<object, {argsType}> action) =>
                On({evt.Name}Event, action);
");
                    }
                }
            }

            void WriteReal(StringBuilder source, string name)
            {
                source.AppendLine($@"
            protected override {name} WithEvents(System.Collections.Immutable.ImmutableDictionary<EventInfo, (IListenerFactory, object)> events) =>
                new(PropertyValues, events, Gestures);
            protected override {name} WithGestures(System.Collections.Immutable.ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures) =>
                new(PropertyValues, Events, gestures);
            protected override {name} WithProperties(System.Collections.Immutable.ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues) =>
                new(propertyValues, Events, Gestures);");
            }

            bool IsDefaultConstructable(INamedTypeSymbol symbol) =>
                IsAccessible(symbol.DeclaredAccessibility) &&
                !symbol.IsGenericType &&
                !symbol.IsAbstract &&
                symbol.InstanceConstructors.Any(c =>
                    c.Parameters.IsEmpty &&
                    IsAccessible(c.DeclaredAccessibility) &&
                    !c.IsStatic);

            bool IsAccessible(Accessibility access) =>
                access == Accessibility.Public || (allowInternal && access == Accessibility.Internal);
        }

        class Types
        {
            Compilation compilation;

            public Types(Compilation compilation)
            {
                this.compilation = compilation;
                NSView = Load("AppKit.NSView");
                CocoaToStandardUIAttribute = Load("Microsoft.StandardUI.Cocoa.CocoaToStandardUIAttribute");
            }

            public INamedTypeSymbol NSView { get; }
            public INamedTypeSymbol CocoaToStandardUIAttribute { get; }

            public bool IsCocoaToStandardUIAttribute(AttributeData attribute) =>
                CocoaToStandardUIAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.IncludeNullability);

            INamedTypeSymbol Load(string fullyQualifiedMetadataName) =>
                compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) ?? throw new TypeNotFoundException(fullyQualifiedMetadataName);
        }
    }
}
