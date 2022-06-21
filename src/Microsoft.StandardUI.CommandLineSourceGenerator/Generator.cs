using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.StandardUI.SourceGenerator;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace Microsoft.StandardUI.CommandLineSourceGenerator
{
    public static class Generator
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: StandardUI.CodelGenerator.exe <path-to-repo-root>");
                Environment.Exit(1);
            }

            string rootDirectory = NormalizePath(args[0]);

            MSBuildLocator.RegisterDefaults();

            using MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

            string standardUIProjectPath = Path.Combine(rootDirectory, "src", "Microsoft.StandardUI", "Microsoft.StandardUI.csproj");
            Console.WriteLine($"Loading project '{standardUIProjectPath}'");

            // Attach progress reporter so we print projects as they are loaded.
            Project project = await workspace.OpenProjectAsync(standardUIProjectPath, new ConsoleProgressReporter());
            Console.WriteLine($"Finished loading project '{standardUIProjectPath}'");

            try
            {
                GenerateClasses(rootDirectory, project);
            }
            catch (UserViewableException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.ToString()}");
                Environment.Exit(2);
            }
        }

        public class GatherInterfacesToGenerateFrom : SymbolVisitor
        {
            private List<INamedTypeSymbol> _interfaces = new List<INamedTypeSymbol>();

            public IEnumerable<INamedTypeSymbol> Interfaces => _interfaces;

            public override void VisitNamespace(INamespaceSymbol symbol)
            {
                foreach (var childSymbol in symbol.GetMembers())
                {
                    //We must implement the visitor pattern ourselves and 
                    //accept the child symbols in order to visit their children
                    childSymbol.Accept(this);
                }
            }

            public override void VisitNamedType(INamedTypeSymbol symbol)
            {
                InterfacePurpose? interfacePuprpose = Interface.IdentifyPurpose(symbol);
                if (interfacePuprpose.HasValue)
                {
                    _interfaces.Add(symbol);
                }
            }
        }

        private static void GenerateClasses(string rootDirectory, Project project)
        {
			Compilation? compilation = project.GetCompilationAsync().Result;
            if (compilation == null)
                return;

            var gatherInterfacesToGenerateFrom = new GatherInterfacesToGenerateFrom();
            gatherInterfacesToGenerateFrom.Visit(compilation.GlobalNamespace);

            Context context = new Context(compilation, new DirectoryOutput(rootDirectory));

            var wpfUIFramework = new WpfUIFramework(context);
            var winUIUIFramework = new WinUIUIFramework(context);
            var winFormsUIFramework = new WinFormsUIFramework(context);
            var macUIFramework = new MacUIFramework(context);
            var mauiUIFramework = new MauiUIFramework(context);

            foreach (INamedTypeSymbol interfaceType in gatherInterfacesToGenerateFrom.Interfaces)
            {
                Console.WriteLine($"Processing {interfaceType.Name}");

                var intface = new Interface(context, interfaceType);
                intface.Generate(wpfUIFramework);
                intface.Generate(winUIUIFramework);
                intface.Generate(winFormsUIFramework);
                intface.Generate(macUIFramework);
                intface.Generate(mauiUIFramework);
                intface.GenerateExtensionsClass();
            }
        }

        private static string NormalizePath(string path)
        {
            path = path.Trim();
            try
            {
                return Path.GetFullPath(path).TrimEnd('\\').ToLowerInvariant();
            }
            catch (ArgumentException)
            {
                // If invalid path, leave unmodified
                return path;
            }
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
