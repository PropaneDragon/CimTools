using CimTools.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CimToolsTests
{
    [TestClass]
    public class SavedOptionTests
    {
        [TestMethod]
        public void TestSaveLoadOptions()
        {
            SavedOptions.Instance().SavedCheckboxes.Add(new SavedCheckbox() { name = "test1", value = true });
            SavedOptions.Instance().SavedCheckboxes.Add(new SavedCheckbox() { name = "test2", value = false });

            SavedOptions.Instance().SavedDropdowns.Add(new SavedDropdown() { name = "test3", value = "testDropdown1" });
            SavedOptions.Instance().SavedDropdowns.Add(new SavedDropdown() { name = "test4", value = "testDropdown2" });

            SavedOptions.Instance().SavedSliders.Add(new SavedSlider() { name = "test5", value = 1.2f });
            SavedOptions.Instance().SavedSliders.Add(new SavedSlider() { name = "test6", value = 2.3f });

            SavedOptions.Save("TestSaveLoadOptions");
            SavedOptions.SetInstance(new SavedOptions());

            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes.Count, 0);
            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns.Count, 0);
            Assert.AreEqual(SavedOptions.Instance().SavedSliders.Count, 0);

            SavedOptions.Load("TestSaveLoadOptions");

            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes.Count, 2);
            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns.Count, 2);
            Assert.AreEqual(SavedOptions.Instance().SavedSliders.Count, 2);

            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes[0].name, "test1");
            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes[0].value, true);

            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes[1].name, "test2");
            Assert.AreEqual(SavedOptions.Instance().SavedCheckboxes[1].value, false);

            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns[0].name, "test3");
            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns[0].value, "testDropdown1");

            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns[1].name, "test4");
            Assert.AreEqual(SavedOptions.Instance().SavedDropdowns[1].value, "testDropdown2");

            Assert.AreEqual(SavedOptions.Instance().SavedSliders[0].name, "test5");
            Assert.AreEqual(SavedOptions.Instance().SavedSliders[0].value, 1.2f);

            Assert.AreEqual(SavedOptions.Instance().SavedSliders[1].name, "test6");
            Assert.AreEqual(SavedOptions.Instance().SavedSliders[1].value, 2.3f);
        }
    }
}
