using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects.UserAttributes
{
    internal static partial class Attributes
    {
        internal const string _fullPropertyAttributeAttributeName = _userAttributesNameSpace + "." + _propertyAttributeAttributeTypeName;

        internal const string _propertyAttributeAttributeTypeName = "PropertyAttributeAttribute";

        internal const string _propertyAttributeAttributeString = @"
using System;

namespace " + _userAttributesNameSpace + @"
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    internal sealed class " + _propertyAttributeAttributeTypeName + @" : Attribute
    {
        public " + _propertyAttributeAttributeTypeName + @"(Type attributeType, params object[] constuctorArguments)
        {
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException(nameof(attributeType) + ""must be a type representing an attribute"", nameof(attributeType));

            AttributeType = attributeType;
            ConstuctorArguments = constuctorArguments;
        }

        public Type AttributeType { get; }
        public object[] ConstuctorArguments { get; }
        public (string attributePropertyName, object value)[]? Properties { get; set; }
    }
}";
    }
}
