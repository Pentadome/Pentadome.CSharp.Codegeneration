using Pentadome.CSharp.SourceGenerators.ApplicationCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentadome.CSharp.SourceGenerators.Demo
{
    [ObservableObject]
    // Must be declared partial
    public class Car
    {
        private int _horsePower;
    }
}
