using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentadome.CSharp.SourceGenerators
{
    public static class CompilationExtensions
    {
        /// <summary>
        /// Gets the type within the compilation's assembly and all referenced assemblies
        ///  (other than those that can only be referenced via an extern alias) using its
        ///  canonical CLR metadata name.
        /// </summary>
        public static INamedTypeSymbol GetTypeByMetadataNameOrThrow(this Compilation @this, string fullyQualifiedMetadataName)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (string.IsNullOrWhiteSpace(fullyQualifiedMetadataName))
            {
                throw new ArgumentException($"'{nameof(fullyQualifiedMetadataName)}' cannot be null or whitespace", nameof(fullyQualifiedMetadataName));
            }

            return @this.GetTypeByMetadataName(fullyQualifiedMetadataName) ?? throw new TypeLoadException("Can not load: " + fullyQualifiedMetadataName);
        }
    }
}
