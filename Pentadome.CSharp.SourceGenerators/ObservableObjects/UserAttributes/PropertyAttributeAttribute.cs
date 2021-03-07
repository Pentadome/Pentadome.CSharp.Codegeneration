using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects.UserAttributes
{
    internal static partial class Attributes
    {
        internal const string _fullPropertyAttributeAttributeName = _userAttributesNameSpace + "." + _propertyAttributeAttributeTypeName;

        internal const string _propertyAttributeAttributeTypeName = "PropertyAttributeAttribute";

        internal const string _attributeNamedPropertyStartIdentifier = "AttributeProperty";

        internal const string _attributeNamedPropertyNameIdentifier = "Name";

        internal const string _attributeNamedPropertyValueIdentifier = "Value";

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

        public object?[] ConstuctorArguments { get; }
        
        public string " + _attributeNamedPropertyStartIdentifier + "1" + _attributeNamedPropertyNameIdentifier + @" { get; set; }
        
        public object? " + _attributeNamedPropertyStartIdentifier + "1" + _attributeNamedPropertyValueIdentifier + @" { get; set; }

        public string " + _attributeNamedPropertyStartIdentifier + "2" + _attributeNamedPropertyNameIdentifier + @" { get; set; }
        
        public object? " + _attributeNamedPropertyStartIdentifier + "2" + _attributeNamedPropertyValueIdentifier + @" { get; set; }

        public string " + _attributeNamedPropertyStartIdentifier + "3" + _attributeNamedPropertyNameIdentifier + @" { get; set; }
        
        public object? " + _attributeNamedPropertyStartIdentifier + "3" + _attributeNamedPropertyValueIdentifier + @" { get; set; }

        public string " + _attributeNamedPropertyStartIdentifier + "4" + _attributeNamedPropertyNameIdentifier + @" { get; set; }
        
        public object? " + _attributeNamedPropertyStartIdentifier + "4" + _attributeNamedPropertyValueIdentifier + @" { get; set; }

        public string " + _attributeNamedPropertyStartIdentifier + "5" + _attributeNamedPropertyNameIdentifier + @" { get; set; }
        
        public object? " + _attributeNamedPropertyStartIdentifier + "5" + _attributeNamedPropertyValueIdentifier + @" { get; set; }
    }
}";
    }
}
