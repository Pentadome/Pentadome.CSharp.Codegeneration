using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pentadome.CSharp.SourceGenerators.Demo;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Pentadome.CSharp.SourceGenerators.Tests
{
    [TestClass]
    public class ObservableObjectTests
    {
        [TestMethod]
        public void PropertyChangedTest()
        {
            var person = new Person();
            var lastPropertyChanged = "";
            person.PropertyChanged += (sender, propertyChanged) => lastPropertyChanged = propertyChanged.PropertyName;
            person.FirstName = "Peter";
            Assert.IsTrue(lastPropertyChanged == nameof(Person.FirstName));
        }

        [TestMethod]
        public void OnPropertyChangedPartialMethodTest()
        {
            var person = new Person();
            Assert.IsFalse(person.OnLastNameChangedHasRun);
            person.LastName = "Jones";
            Assert.IsTrue(person.OnLastNameChangedHasRun);
        }
    }
}
