using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects.UserAttributes
{
    internal static partial class Attributes
    {
        internal const string _userAttributesNameSpace = "Pentadome.CSharp.SourceGenerators.ApplicationCode";

        internal static void AddAttributesToSource(GeneratorExecutionContext executionContext)
        {
            executionContext.AddSource(_observableObjectAttributeTypeName, SourceText.From(_observableObjectAttributeString, Encoding.UTF8));
            executionContext.AddSource(_propertyAttributeAttributeTypeName, SourceText.From(_propertyAttributeAttributeString, Encoding.UTF8));
        }

        internal static CSharpCompilation AddAttributesToSyntax(CSharpCompilation compilation, CSharpParseOptions? options = null)
        {
            return compilation.AddSyntaxTrees(
                CSharpSyntaxTree.ParseText(_observableObjectAttributeString, options, encoding: Encoding.UTF8),
                CSharpSyntaxTree.ParseText(_propertyAttributeAttributeString, options, encoding: Encoding.UTF8));
        }
    }
}
