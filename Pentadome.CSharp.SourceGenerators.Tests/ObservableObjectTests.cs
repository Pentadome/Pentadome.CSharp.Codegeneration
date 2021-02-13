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
        public void TestMethod1()
        {
            var person = new Person();
            var lastPropertyChanged = "";
            person.PropertyChanged += (sender, propertyChanged) => lastPropertyChanged = propertyChanged.PropertyName;
            person.FirstName = "Peter";
            Assert.IsTrue(lastPropertyChanged == "FirstName");
        }
    }
}
