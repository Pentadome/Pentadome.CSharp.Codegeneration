using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects
{
    internal class ObservableObjectSyntaxReceiver : ISyntaxReceiver
    {
        public IReadOnlyList<ClassDeclarationSyntax> CandidateClasses => _candidateClasses ??= _candidateClassesList.AsReadOnly();

        private IReadOnlyList<ClassDeclarationSyntax>? _candidateClasses;

        private readonly List<ClassDeclarationSyntax> _candidateClassesList = new();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // any class has at least one attribute is a candidate
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Any())
            {
                _candidateClassesList.Add(classDeclarationSyntax);
            }
        }
    }
}
