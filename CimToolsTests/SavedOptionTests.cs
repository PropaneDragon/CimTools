using CimTools.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CimToolsTests
{
    [TestClass]
    public class SavedOptionTests
    {
        [TestMethod]
        public void SetGetLooseValues()
        {
            PersistentOptions.SetInstance(new PersistentOptions()); //Clean slate
            PersistentOptions.Instance().SetValue("testFloat", 10.4f);
            PersistentOptions.Instance().SetValue("testString", "hello");
            PersistentOptions.Instance().SetValue("testDouble", 19.2);
            PersistentOptions.Instance().SetValue("testInt", 19);

            float floatValue;
            string stringValue;
            double doubleValue;
            int intValue;

            bool valid = PersistentOptions.Instance().GetValue("testFloat", out floatValue) == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(valid, "Retrieving \"testFloat\" option");
            if (valid)
            {
                Assert.AreEqual(10.4f, floatValue);
            }

            valid = PersistentOptions.Instance().GetValue("testString", out stringValue) == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(valid, "Retrieving \"testString\" option");
            if (valid)
            {
                Assert.AreEqual("hello", stringValue);
            }

            valid = PersistentOptions.Instance().GetValue("testDouble", out doubleValue) == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(valid, "Retrieving \"testDouble\" option");
            if (valid)
            {
                Assert.AreEqual(19.2, doubleValue);
            }

            valid = PersistentOptions.Instance().GetValue("testInt", out intValue) == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(valid, "Retrieving \"testInt\" option");
            if (valid)
            {
                Assert.AreEqual(19, intValue);
            }

            valid = PersistentOptions.Instance().GetValue("testNotInList", out floatValue) == PersistentOptions.OptionError.NoError;
            Assert.IsFalse(valid, "Retrieving \"testNotInList\" option");

            valid = PersistentOptions.Instance().GetValue("testInt", out intValue) == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(valid, "Retrieving \"testInt\" option");
            if (valid)
            {
                Assert.AreNotEqual("string", intValue);
                Assert.AreNotEqual("19", intValue);
            }
        }

        [TestMethod]
        public void SetGetGroupedValues()
        {
            PersistentOptions.SetInstance(new PersistentOptions()); //Clean slate
            PersistentOptions.Instance().SetValue("float", 12.3f, "group 1");
            PersistentOptions.Instance().SetValue("double", 34.5, "group 1");
            PersistentOptions.Instance().SetValue("double 2", 67.8, "group 1");
            PersistentOptions.Instance().SetValue("string", "a string", "group 1");
            PersistentOptions.Instance().SetValue("string 2", "56.7", "group 1");
            PersistentOptions.Instance().SetValue("int", 1, "group 1");

            PersistentOptions.Instance().SetValue("float", 98.7f, "group 2");
            PersistentOptions.Instance().SetValue("double", 65.4, "group 2");

            var foundFloatValues = PersistentOptions.Instance().GetValues<float>("group 1");
            Assert.AreEqual(1, foundFloatValues.Count);
            Assert.IsTrue(foundFloatValues.ContainsKey("float"));
            Assert.AreEqual(12.3f, foundFloatValues["float"]);

            var foundDoubleValues = PersistentOptions.Instance().GetValues<double>("group 1");
            Assert.AreEqual(2, foundDoubleValues.Count);
            Assert.IsTrue(foundDoubleValues.ContainsKey("double"));
            Assert.IsTrue(foundDoubleValues.ContainsKey("double 2"));
            Assert.AreEqual(34.5, foundDoubleValues["double"]);
            Assert.AreEqual(67.8, foundDoubleValues["double 2"]);

            var foundStringValues = PersistentOptions.Instance().GetValues<string>("group 1");
            Assert.AreEqual(2, foundStringValues.Count);
            Assert.IsTrue(foundStringValues.ContainsKey("string"));
            Assert.IsTrue(foundStringValues.ContainsKey("string 2"));
            Assert.AreEqual("a string", foundStringValues["string"]);
            Assert.AreEqual("56.7", foundStringValues["string 2"]);

            var foundIntValues = PersistentOptions.Instance().GetValues<int>("group 1");
            Assert.AreEqual(1, foundIntValues.Count);
            Assert.IsTrue(foundIntValues.ContainsKey("int"));
            Assert.AreEqual(1, foundIntValues["int"]);

            foundFloatValues = PersistentOptions.Instance().GetValues<float>("group 2");
            Assert.AreEqual(1, foundFloatValues.Count);
            Assert.IsTrue(foundFloatValues.ContainsKey("float"));
            Assert.AreEqual(98.7f, foundFloatValues["float"]);

            foundDoubleValues = PersistentOptions.Instance().GetValues<double>("group 2");
            Assert.AreEqual(1, foundDoubleValues.Count);
            Assert.IsTrue(foundDoubleValues.ContainsKey("double"));
            Assert.AreEqual(65.4, foundDoubleValues["double"]);
        }

        [TestMethod]
        public void SetExistingValues()
        {
            PersistentOptions.SetInstance(new PersistentOptions()); //Clean slate
            PersistentOptions.Instance().SetValue("int a", 1, "group 1");

            var foundIntValues = PersistentOptions.Instance().GetValues<int>("group 1");
            Assert.AreEqual(1, foundIntValues.Count);
            Assert.AreEqual(1, foundIntValues["int a"]);

            PersistentOptions.Instance().SetValue("int a", 2, "group 1");

            foundIntValues = PersistentOptions.Instance().GetValues<int>("group 1");
            Assert.AreEqual(1, foundIntValues.Count);
            Assert.AreEqual(2, foundIntValues["int a"]);

            PersistentOptions.Instance().SetValue("int b", 3, "group 1");

            foundIntValues = PersistentOptions.Instance().GetValues<int>("group 1");
            Assert.AreEqual(2, foundIntValues.Count);
            Assert.AreEqual(2, foundIntValues["int a"]);
            Assert.AreEqual(3, foundIntValues["int b"]);

            PersistentOptions.Instance().SetValue("int b", 4, "group 1");

            foundIntValues = PersistentOptions.Instance().GetValues<int>("group 1");
            Assert.AreEqual(2, foundIntValues.Count);
            Assert.AreEqual(2, foundIntValues["int a"]);
            Assert.AreEqual(4, foundIntValues["int b"]);
        }

        [TestMethod]
        public void SaveLoadValues()
        {
            CimTools.Settings.ModName = "TestSaveLoad";

            PersistentOptions.SetInstance(new PersistentOptions()); //Clean slate
            PersistentOptions.Instance().SetValue("testFloat", 10.4f);
            PersistentOptions.Instance().SetValue("testString", "hello");
            PersistentOptions.Instance().SetValue("testDouble", 19.2);
            PersistentOptions.Instance().SetValue("testInt", 19);
            PersistentOptions.Instance().SetValue("testInt", 2082, "awesome group");
            PersistentOptions.Instance().SetValue("testDouble", 106.8, "awesome group");
            PersistentOptions.Instance().SetValue("testString", "hello again", "awesome group");
            PersistentOptions.Instance().SetValue("testDifferentString", "hello again again", "awesome group");
            PersistentOptions.Instance().SetValue("testDifferentGroup", "hi", "not as awesome group");

            bool succeeded = PersistentOptions.Save() == PersistentOptions.OptionError.NoError;
            Assert.IsTrue(succeeded, "Options failed to save to disk");

            if (succeeded)
            {
                float floatValue;
                int intValue;
                string stringValue;

                PersistentOptions.SetInstance(new PersistentOptions());

                bool valid = PersistentOptions.Instance().GetValue("testFloat", out floatValue) == PersistentOptions.OptionError.NoError;
                Assert.IsFalse(valid);

                PersistentOptions.Load();

                valid = PersistentOptions.Instance().GetValue("testFloat", out floatValue) == PersistentOptions.OptionError.NoError;
                Assert.IsTrue(valid, "Obtaining \"testFloat\" from the options");
                if (valid)
                {
                    Assert.AreEqual(10.4f, floatValue, "Checking whether the value is a float");
                }

                valid = PersistentOptions.Instance().GetValue("testInt", out intValue, "awesome group") == PersistentOptions.OptionError.NoError;
                Assert.IsTrue(valid, "Obtaining \"testInt\" from the options under \"awesome group\"");
                if (valid)
                {
                    Assert.AreEqual(2082, intValue, "Checking whether the value is an int");
                }

                valid = PersistentOptions.Instance().GetValue("testString", out stringValue, "awesome group") == PersistentOptions.OptionError.NoError;
                Assert.IsTrue(valid, "Obtaining \"testString\" from the options under \"awesome group\"");
                if (valid)
                {
                    Assert.AreEqual("hello again", stringValue, "Checking whether the value is a string");
                }
            }
        }

        [TestMethod]
        public void ErrorHandling()
        {
            CimTools.Settings.ModName = "DoesntExist";

            Assert.AreEqual(PersistentOptions.Load(), PersistentOptions.OptionError.FileNotFound);

            int tempIntStorage;

            Assert.AreEqual(PersistentOptions.Instance().GetValue<int>("NotAValidName", out tempIntStorage, "NotAValidGroup"), PersistentOptions.OptionError.GroupNotFound);
            Assert.AreEqual(PersistentOptions.Instance().GetValue<int>("NotAValidName", out tempIntStorage), PersistentOptions.OptionError.OptionNotFound);
        }
    }
}
