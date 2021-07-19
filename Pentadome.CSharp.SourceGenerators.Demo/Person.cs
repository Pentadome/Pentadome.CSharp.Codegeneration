using System;
using Pentadome.CSharp.SourceGenerators.ApplicationCode;

namespace Pentadome.CSharp.SourceGenerators.Demo
{
    [ObservableObject]
    public partial class Person
    {
        private string _firstName;

        private string _lastName;

        private DateTime _birthday;

        public bool OnLastNameChangedHasRun { get; private set; }

        partial void OnLastNameChanged() => OnLastNameChangedHasRun = true;
    }
}
