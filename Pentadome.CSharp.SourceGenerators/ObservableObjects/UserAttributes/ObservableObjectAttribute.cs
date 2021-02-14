using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects.UserAttributes
{
    internal static partial class Attributes
    {
        internal const string _observableObjectAttributeTypeName = "ObservableObjectAttribute";

        internal const string _fullObservableObjectAttributeName = _userAttributesNameSpace + "." + _observableObjectAttributeTypeName;

        internal const string _observableObjectAttributeString = @"
using System;
namespace " + _userAttributesNameSpace + @"
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class " + _observableObjectAttributeTypeName + @" : Attribute
    {
    }
}
";
    }
}
