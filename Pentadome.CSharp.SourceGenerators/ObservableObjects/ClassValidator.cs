using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumsNET;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects
{
    public static class ClassValidator
    {
        public static bool ValidateAndReportDiagnostics(INamedTypeSymbol classSymbol, GeneratorExecutionContext context)
        {
            var isValid = true;
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "100",
                            "Incorrect attribute usage.",
                            "Targetted class {0} can not be a part of another class.",
                            "Attribute Usage",
                            DiagnosticSeverity.Warning,
                            true,
                            "Targetted class can not be a part of another class.")
                        , classSymbol.Locations[0],
                        classSymbol.ToDisplayString()));
                isValid = false;
            }

            if (!(classSymbol.DeclaringSyntaxReferences[0].GetSyntax() as ClassDeclarationSyntax)!.Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword)))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "101",
                            "Class must be partial",
                            "Targetted class {0} must be declared partial",
                            "Attribute Usage",
                            DiagnosticSeverity.Warning,
                            true,
                            "Targetted class {0} must be declared partial")
                        , classSymbol.Locations[0],
                        classSymbol.ToDisplayString()));
                isValid = false;
            }

            return isValid;
        }
    }
}
