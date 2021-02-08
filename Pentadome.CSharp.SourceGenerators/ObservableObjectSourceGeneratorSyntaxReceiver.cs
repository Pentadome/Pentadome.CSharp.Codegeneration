using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators
{
    internal class ObservableObjectSourceGeneratorSyntaxReceiver : ISyntaxReceiver
    {
        public IReadOnlyList<FieldDeclarationSyntax> CandidateFields => _candidateFields ??= _candidateFieldsList.AsReadOnly();

        private IReadOnlyList<FieldDeclarationSyntax>? _candidateFields;

        private readonly List<FieldDeclarationSyntax> _candidateFieldsList = new List<FieldDeclarationSyntax>();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // any field where parent is a class and that class has at least one attribute is a candidate for property generation
            if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax
                && fieldDeclarationSyntax.Parent is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                _candidateFieldsList.Add(fieldDeclarationSyntax);
            }
        }
    }
}
