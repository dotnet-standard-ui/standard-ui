using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StandardUI.Cocoa.Generator
{
    [Generator]
    public class CocoaToStandardUI : ISourceGenerator
    {
        const string PlatformNamespace = "AppKit";
        const string ProjectedPlatformNamespace = "Microsoft.StandardUI.Cocoa.Native";
        const string OutputDirectory = "StandardUI.Cocoa.Generator/StandardUI.Cocoa.Generator.CocoaToStandardUI";

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

        public void Initialize(GeneratorInitializationContext context)
        {
            // TODO Write the output of the source generator because VSMac doesn't have source generator
            // support yet. Once VSMac has source generator support we can stop writing the files and rely
            // on the IDE.
            if (Directory.Exists(OutputDirectory))
                Directory.Delete(OutputDirectory, recursive: true);
            Directory.CreateDirectory(OutputDirectory);
            var pwd = Directory.GetCurrentDirectory();
            context.RegisterForSyntaxNotifications(() => new ClassSearcher(pwd));
        }

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

        record ClassSearcher(string Pwd) : ISyntaxReceiver
        {
            Dictionary<string, List<ClassDeclarationSyntax>> classes = new();

            public ClassDeclarationSyntax? GetClassDeclaration(string ns, string name)
            {
                if (classes.TryGetValue(name, out var opts))
                {
                    foreach (var cls in opts)
                    {
                        if (cls.Parent is NamespaceDeclarationSyntax nsd && nsd.Name.ToString() == ns)
                            return cls;
                    }
                }

                return null;
            }

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax cds)
                {
                    var name = cds.Identifier.ValueText;
                    if (!classes.TryGetValue(name, out var lst))
                        classes[name] = lst = new();
                    lst.Add(cds);
                }
            }
        }

        record ConstructorInfo(string Name, int BaseParameterCount, string[] ParameterTypes, string[] Parameters);

        class TranslateTypes : SymbolVisitor
        {
#pragma warning disable RS1024 // Compare symbols correctly
            Dictionary<INamedTypeSymbol, ConstructorInfo> translated = new(SymbolEqualityComparer.Default);
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

            ConstructorInfo Translate(INamedTypeSymbol cocoaType)
            {
                if (translated.TryGetValue(cocoaType, out var v))
                    return v;

                var qualifiedType = cocoaType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!generatePlatformTypes && qualifiedType.StartsWith("global::AppKit.") &&
                    cocoaType.ContainingAssembly.Equals(types.NSView.ContainingAssembly, SymbolEqualityComparer.Default))
                {
                    // TODO
                    //var projected = $"global::{ProjectedPlatformNamespace}.{cocoaType.Name}Base";
                    //translated[cocoaType] = projected;
                    //return projected;
                }

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
                    projNamespace = DestNamespacePrefix;
                    projType = cocoaType.Name;
                }

                var projTypeBase = $"{projType}Base";
                var qualifiedProjTypeBase = $"global::{projNamespace}.{projTypeBase}";
                bool isNSView = types.NSView.Equals(cocoaType, SymbolEqualityComparer.Default);
                ConstructorInfo baseClassInfo = !isNSView ? Translate(cocoaType.BaseType!) : new("", 0, Array.Empty<string>(), Array.Empty<string>());
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
                        basedOn = $" : {baseClassInfo.Name}<TSubclass, TView> where TView : {qualifiedType}, new()";
                    }

                    source.AppendLine($@"
    {access} abstract partial class {projTypeBase}<TSubclass, TView> {basedOn}
    {{");
                    baseClassInfo = ComputeParameters(baseClassInfo, projNamespace, projTypeBase);
                    WriteConstructors(source, projTypeBase, baseClassInfo);
                    WriteBase(source, cocoaType, qualifiedType, "TSubclass");
                    WriteAbstractWith(source, baseClassInfo);
                    source.AppendLine("    }");
                }

                if (IsDefaultConstructable(cocoaType))
                {
                    string parent;
                    if (cocoaType.IsSealed)
                        parent = Translate(cocoaType.BaseType!).Name;
                    else
                        parent = $"global::{projNamespace}.{projTypeBase}";
                    source.AppendLine($@"
    {access} sealed partial class {name} : {parent}<{name}, {qualifiedType}>
    {{");
                    var info = ComputeParameters(baseClassInfo, projNamespace, name);
                    WriteConstructors(source, name, info);
                    if (cocoaType.IsSealed)
                        WriteBase(source, cocoaType, qualifiedType, name);
                    WriteReal(source, name, info);
                    source.AppendLine("    }");
                }
                source.AppendLine("}");


                var cwd = Directory.GetCurrentDirectory();
                source.AppendLine($"// {cwd}");

                var sourceFile = projNamespace.Replace('.', '_');
                sourceFile += $"_{name}.cs";
                var sourceStr = source.ToString();
                context.AddSource(sourceFile, sourceStr);

                // For debugging sometimes it's easiest to write the files to disk
                var pwd = ((ClassSearcher)context.SyntaxReceiver!).Pwd;
                File.WriteAllText($"{pwd}/{OutputDirectory}/{sourceFile}", sourceStr);

                translated[cocoaType] = baseClassInfo;
                return baseClassInfo;
            }

            ConstructorInfo ComputeParameters(ConstructorInfo baseClass, string ns, string name)
            {
                var searcher = (ClassSearcher)context.SyntaxReceiver!;
                var qualifiedName = $"global::{ns}.{name}";
                var baseParameterCount = baseClass.Parameters.Length;
                if (!(searcher.GetClassDeclaration(ns, name) is ClassDeclarationSyntax cds))
                    return baseClass with { Name = qualifiedName, BaseParameterCount = baseParameterCount };

                List<string> extraTypes = new();
                List<string> extraNames = new();
                var model = context.Compilation.GetSemanticModel(cds.SyntaxTree);
                foreach (var field in cds.Members.OfType<FieldDeclarationSyntax>())
                {
                    foreach (var variable in field.Declaration.Variables)
                    {
                        if (model.GetDeclaredSymbol(variable) is IFieldSymbol sym && !sym.IsStatic)
                        {
                            extraNames.Add(sym.Name);
                            extraTypes.Add(sym.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                        }
                    }
                }

                extraNames.InsertRange(0, baseClass.Parameters);
                extraTypes.InsertRange(0, baseClass.ParameterTypes);
                return new(qualifiedName, baseParameterCount, extraTypes.ToArray(), extraNames.ToArray());
            }

            void WriteConstructors(StringBuilder source, string name, ConstructorInfo info)
            {
                source.AppendLine($@"
        public {name}() {{}}
        public {name}(");
                for (int i = 0; i < info.Parameters.Length; ++i)
                {
                    var type = info.ParameterTypes[i];
                    var pname = info.Parameters[i];
                    source.Append(' ', 12);
                    source.Append(type);
                    source.Append(' ');
                    source.Append(pname);
                    if (i + 1 < info.Parameters.Length)
                        source.Append(',');
                    else
                        source.Append(')');
                    source.AppendLine();
                }

                if (info.BaseParameterCount > 0)
                {
                    source.Append(' ', 12);
                    source.Append(": base(");
                    var toBase = info.Parameters.Take(info.BaseParameterCount);
                    source.Append(string.Join(", ", toBase));
                    source.AppendLine(")");
                }

                source.Append(' ', 8);
                source.AppendLine("{");
                foreach (var pname in info.Parameters.Skip(info.BaseParameterCount))
                {
                    source.Append(' ', 12);
                    source.AppendLine($"this.{pname} = {pname};");
                }

                source.Append(' ', 8);
                source.AppendLine("}");
                source.AppendLine();
            }

            void WriteAbstractWith(StringBuilder source, ConstructorInfo info)
            {
                for (int i = info.BaseParameterCount; i < info.Parameters.Length; ++i)
                {
                    var type = info.ParameterTypes[i];
                    var pname = info.Parameters[i];
                    source.Append(' ', 8);
                    source.AppendLine($"protected abstract TSubclass With_{pname}({type} {pname});");
                }
            }

            void WriteBase(StringBuilder source, INamedTypeSymbol cocoaType, string qualifedCocoaType, string subclassName)
            {
                foreach (var member in cocoaType.GetMembers())
                {
                    if (member.IsStatic || !IsAccessible(member.DeclaredAccessibility))
                        continue;

                    string flags = "global::System.Reflection.BindingFlags.DeclaredOnly | global::System.Reflection.BindingFlags.Instance";
                    if (member.DeclaredAccessibility == Accessibility.Public)
                        flags += " | global::System.Reflection.BindingFlags.Public";
                    else
                        flags += " | global::System.Reflection.BindingFlags.NonPublic";
                    string access = member.DeclaredAccessibility.Format();

                    if (member is IPropertySymbol property && !property.IsReadOnly && !property.IsWriteOnly && !property.IsWithEvents)
                    {
                        source.AppendLine($@"
        {access} static readonly global::Microsoft.StandardUI.Cocoa.Native.Internal.UIProperty {property.Name}Property = new(typeof({qualifedCocoaType}).GetProperty(""{property.Name}"", {flags}));
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
                        var argsTypeDisp = argsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        source.AppendLine($@"
        {access} static readonly global::Microsoft.StandardUI.Cocoa.Native.Internal.ListenerFactory<
            {evt.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)},
            {argsTypeDisp}> {evt.Name}Event = new(typeof({qualifedCocoaType}).GetEvent(""{evt.Name}"", {flags}));
        {access} {subclassName} {evt.Name}(global::System.Action callback) =>
            On({evt.Name}Event, (_, _) => callback());
        {access} {subclassName} {evt.Name}(global::System.Action<object, {argsType}> callback) =>
            On({evt.Name}Event, callback);
");
                    }
                }
            }

            void WriteReal(StringBuilder source, string name, ConstructorInfo constructorInfo)
            {
                var args = string.Join(", ", constructorInfo.Parameters);
                for (int i = 0; i < constructorInfo.Parameters.Length; ++i)
                {
                    var pname = constructorInfo.Parameters[i];
                    var ptype = constructorInfo.ParameterTypes[i];
                    source.Append(' ', 8);
                    if (i < constructorInfo.BaseParameterCount)
                        source.Append("protected override ");
                    source.AppendLine($"{name} With_{pname}({ptype} {pname}) =>");

                    // Abusing the fact that our local variable, pname, will resolve before the field
                    source.Append(' ', 12);
                    source.AppendLine($"new({args});");
                }
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
