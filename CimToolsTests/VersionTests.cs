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
            CimTools.v2.Utilities.Version versionTest = new CimTools.v2.Utilities.Version(Assembly.GetExecutingAssembly());

            string version = versionTest.Delimited();
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 3);

            version = versionTest.Delimited(CimTools.v2.Utilities.Version.Limit.Revision);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 4);

            version = versionTest.Delimited(CimTools.v2.Utilities.Version.Limit.Major);
            Assert.AreEqual(version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = versionTest.Delimited(CimTools.v2.Utilities.Version.Limit.Major, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 1);

            version = versionTest.Delimited(CimTools.v2.Utilities.Version.Limit.Revision, ",");
            Assert.AreEqual(version.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length, 4);
        }
    }
}
