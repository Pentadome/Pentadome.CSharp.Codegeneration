using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentadome.CSharp.SourceGenerators.Demo
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DemoAttribute : Attribute
    {
        public DemoAttribute(string constructorString, int constructorInt)
        {
            ConstructorString = constructorString;
            ConstructorInt = constructorInt;
        }

        public string ConstructorString { get; }
        public int ConstructorInt { get; }

        public bool NamedBool { get; set; }

        public char[]? NamedCharArray { get; set; }
    }
}
