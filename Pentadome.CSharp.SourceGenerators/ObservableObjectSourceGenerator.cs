using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators
{
    [Generator]
    public sealed class ObservableObjectSourceGenerator : ISourceGenerator
    {
        private const string _observableObjectAttributeTypeName = "ObservableObjectAttribute";

        private const string _observableObjectAttributeNameSpace = "Pentadome.CSharp.SourceGenerators.ApplicationCode";

        private const string _observableObjectAttributeString = @"
using System;
namespace " + _observableObjectAttributeNameSpace + @"
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class " + _observableObjectAttributeTypeName + @" : Attribute
    {
    }
}
";
        private INamedTypeSymbol? _attributeTypeSymbol;

        private INamedTypeSymbol? _iNotifyChangedSymbol;

        private INamedTypeSymbol? _iNotifyChangingSymbol;

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ObservableObjectSourceGeneratorSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource(_observableObjectAttributeTypeName, SourceText.From(_observableObjectAttributeString, Encoding.UTF8));

            if (context.SyntaxReceiver is not ObservableObjectSourceGeneratorSyntaxReceiver syntaxReceiver)
                return;

            var compilation = (CSharpCompilation)context.Compilation;

            EnsureSymbolsSet(compilation);

            foreach (var field in syntaxReceiver.CandidateFields)
            {
                var model = compilation.GetSemanticModel(field.SyntaxTree);

                foreach (var variable in field.Declaration.Variables)
                {
                    var fieldSymbol = (IFieldSymbol)model.GetDeclaredSymbol(variable)!;
                }
            }
        }

        [MemberNotNull(nameof(_attributeTypeSymbol), nameof(_iNotifyChangedSymbol), nameof(_iNotifyChangingSymbol))]
        private void EnsureSymbolsSet(CSharpCompilation cSharpCompilation)
        {
            if (_attributeTypeSymbol is not null && _iNotifyChangedSymbol is not null && _iNotifyChangingSymbol is not null)
                return;

            CSharpParseOptions options = (cSharpCompilation.SyntaxTrees[0].Options as CSharpParseOptions)!;
            Compilation compilation = cSharpCompilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(_observableObjectAttributeString, Encoding.UTF8), options));

            // get the newly bound attribute, INotifyPropertyChanging and INotifyPropertyChanged
            const string fullAttributeName = _observableObjectAttributeNameSpace + "." + _observableObjectAttributeTypeName;
            _attributeTypeSymbol = compilation.GetTypeByMetadataNameOrThrow(fullAttributeName);
            _iNotifyChangedSymbol = compilation.GetTypeByMetadataNameOrThrow("System.ComponentModel.INotifyPropertyChanged");
            _iNotifyChangingSymbol = compilation.GetTypeByMetadataNameOrThrow("System.ComponentModel.INotifyPropertyChanging");
        }
    }
}
