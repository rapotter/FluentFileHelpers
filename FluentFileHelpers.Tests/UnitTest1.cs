using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentFileHelpers;

namespace FluentFileHelpers.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Configure_Test()
        {
            FluentFileHelpers Config = FluentFileHelpers.Configure();
            Config.AddFromAssemblyOf<TestClass1>();

        }
    }
}
