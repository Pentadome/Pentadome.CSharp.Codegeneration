using System;
using System.Collections.Generic;
using System.Text;

namespace Pentadome.CSharp.SourceGenerators.ObservableObjects.UserAttributes
{
    internal static partial class Attributes
    {
        internal const string _observablePropertyAttributeName = "ObservablePropertyAttribute";

        internal const string _observablePropertyAttributeString = @"
using System;
namespace " + _userAttributesNameSpace + @"
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    internal sealed class " + _observablePropertyAttributeName + @" : Attribute
    {
        public " + _observablePropertyAttributeName + @"(string propertyName, Type propertyType)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($""'{nameof(propertyName)}' cannot be null or whitespace."", nameof(propertyName));
            }

            PropertyName = propertyName;
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

        public " + _observablePropertyAttributeName + @"(string backingFieldName, string propertyName, Type propertyType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($""'{nameof(propertyName)}' cannot be null or empty."", nameof(propertyName));
            }
            ExplicitBackingFieldName = backingFieldName;
            PropertyName = propertyName;
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

    public string? ExplicitBackingFieldName { get; }
    public string PropertyName { get; }
    public Type PropertyType { get; }
    }
}";
    }
}
