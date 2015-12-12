using CimTools.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace CimToolsTests
{
    [TestClass]
    public class VersionTests
    {
        [TestMethod]
        public void TestVersionDelimiter()
        {
            string version = CimTools.File.Version.Delimited(Assembly.GetExecutingAssembly());
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 3);

            version = CimTools.File.Version.Delimited(Assembly.GetExecutingAssembly(), CimTools.File.Version.Limit.Revision);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 4);

            version = CimTools.File.Version.Delimited(Assembly.GetExecutingAssembly(), CimTools.File.Version.Limit.Major);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = CimTools.File.Version.Delimited(Assembly.GetExecutingAssembly(), CimTools.File.Version.Limit.Major, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = CimTools.File.Version.Delimited(Assembly.GetExecutingAssembly(), CimTools.File.Version.Limit.Revision, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 4);
        }
    }
}
