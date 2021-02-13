using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public sealed class " + _observableObjectAttributeTypeName + @" : Attribute
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

#if DEBUG
            if (!Debugger.IsAttached)
                Debugger.Launch();
#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource(_observableObjectAttributeTypeName, SourceText.From(_observableObjectAttributeString, Encoding.UTF8));


            if (context.SyntaxReceiver is not ObservableObjectSourceGeneratorSyntaxReceiver syntaxReceiver)
                return;

            var compilation = (CSharpCompilation)context.Compilation;
            EnsureSymbolsSet(compilation);

            foreach (var symbolsGroup in GetFieldSymbols(compilation, syntaxReceiver.CandidateFields).GroupBy(x => x.ContainingType))
            {
                var classSourceString = ProcessClass(symbolsGroup.Key, symbolsGroup.AsEnumerable(), _iNotifyChangedSymbol, _iNotifyChangingSymbol);
                context.AddSource($"{symbolsGroup.Key.Name}_observable.g.cs", SourceText.From(classSourceString, Encoding.UTF8));
            }
        }

        private IEnumerable<IFieldSymbol> GetFieldSymbols(CSharpCompilation compilation, IEnumerable<FieldDeclarationSyntax> fields)
        {
            foreach (var field in fields)
            {
                var model = compilation.GetSemanticModel(field.SyntaxTree);
                foreach (var variable in field.Declaration.Variables)
                {
                    // Get the symbol being decleared by the field, and keep it if the containing class is annotated.
                    var fieldSymbol = (model.GetDeclaredSymbol(variable) as IFieldSymbol)!;

                    if (fieldSymbol.ContainingType.TypeKind == TypeKind.Class &&
                        fieldSymbol.ContainingType.GetAttributes()
                            .Any(x => x.AttributeClass!.Equals(_attributeTypeSymbol, SymbolEqualityComparer.Default)))
                    {
                        yield return fieldSymbol;
                    }
                }
            }
        }

        //[MemberNotNull(nameof(_attributeTypeSymbol), nameof(_iNotifyChangedSymbol), nameof(_iNotifyChangingSymbol))]
        // Attribute not supported in netstandard 2.0
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

        private static string ProcessClass(INamedTypeSymbol classSymbol, IEnumerable<IFieldSymbol> fields, INamedTypeSymbol notifyChangedSymbol, INamedTypeSymbol notifyChangingSymbol)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return null; //TODO: issue a diagnostic that it must be top level
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // begin building the generated source
            var source = new StringBuilder($@"
// The following was generated by a Source Generator.
namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : {notifyChangedSymbol.ToDisplayString()}, {notifyChangingSymbol.ToDisplayString()}
    {{
");

            // if the class doesn't implement INotifyPropertyChanged already, add it
            if (!classSymbol.Interfaces.Contains(notifyChangedSymbol))
            {
                source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
            }
            // if the class doesn't implement INotifyPropertyChanging already, add it
            if (!classSymbol.Interfaces.Contains(notifyChangingSymbol))
            {
                source.Append("public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;");
            }

            // create properties for each field 
            foreach (var fieldSymbol in fields)
            {
                ProcessField(source, fieldSymbol);
            }

            source.Append("\n}\n}");
            return source.ToString();
        }

        private static void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol)
        {
            // get the name and type of the field
            string fieldName = fieldSymbol.Name;
            ITypeSymbol fieldType = fieldSymbol.Type;

            string propertyName = getPropertyName(fieldName);
            if (propertyName.Length == 0 || propertyName == fieldName)
            {
                //TODO: issue a diagnostic that we can't process this field
                return;
            }

            source.AppendLine().Append("public ").Append(fieldType).Append(' ').Append(propertyName).Append(@"
{
    get
    {
        return this.").Append(fieldName).Append(@";
    }
    set
    {
        this.PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nameof(").Append(propertyName).Append(@")));
        this.").Append(fieldName).Append(@" = value;
        this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(").Append(propertyName).Append(@")));
    }
}
");

            static string getPropertyName(string fieldName)
            {
                fieldName = fieldName.TrimStart('_');
                if (fieldName.Length == 0)
                    return string.Empty;

                if (fieldName.Length == 1)
                    return fieldName.ToUpper();

                return char.ToUpperInvariant(fieldName[0]) + fieldName.Substring(1);
            }
        }
    }
}
