using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace CimToolsTests
{
    [TestClass]
    public class VersionTests
    {
        [TestMethod]
        public void Delimited()
        {
            CimTools.V1.File.Version versionTest = new CimTools.V1.File.Version(Assembly.GetExecutingAssembly());

            string version = versionTest.Delimited();
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 3);

            version = versionTest.Delimited(CimTools.V1.File.Version.Limit.Revision);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 4);

            version = versionTest.Delimited(CimTools.V1.File.Version.Limit.Major);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = versionTest.Delimited(CimTools.V1.File.Version.Limit.Major, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = versionTest.Delimited(CimTools.V1.File.Version.Limit.Revision, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 4);
        }
    }
}
