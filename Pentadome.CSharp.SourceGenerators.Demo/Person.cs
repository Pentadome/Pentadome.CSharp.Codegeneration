using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pentadome.CSharp.SourceGenerators.ApplicationCode;

namespace Pentadome.CSharp.SourceGenerators.Demo
{
    [ObservableObject]
    public partial class Person
    {
        [PropertyAttribute(typeof(KeyAttribute))]
        private Guid _id;

        private string _firstName;

        [PropertyAttribute(typeof(DemoAttribute), "test", 23, Properties = new )]
        private string _lastName;

        [PropertyAttribute(typeof(JsonPropertyNameAttribute), "dayOfBirth")]
        private DateTime _birthday;

        public bool OnLastNameChangedHasRun { get; private set; }

        partial void OnLastNameChanged() => OnLastNameChangedHasRun = true;


        [ObservableObject]
        // Cant be part of another class diagnostic test.
        public partial class Head
        {
#pragma warning disable
            private int _headSize;
#pragma warning restore
        }
    }
}
