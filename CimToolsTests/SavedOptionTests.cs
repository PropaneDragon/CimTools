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
            ExternalOptions.Instance().SavedCheckboxes.Add(new SavedCheckbox() { name = "test1", value = true });
            ExternalOptions.Instance().SavedCheckboxes.Add(new SavedCheckbox() { name = "test2", value = false });

            ExternalOptions.Instance().SavedDropdowns.Add(new SavedDropdown() { name = "test3", value = "testDropdown1" });
            ExternalOptions.Instance().SavedDropdowns.Add(new SavedDropdown() { name = "test4", value = "testDropdown2" });

            ExternalOptions.Instance().SavedSliders.Add(new SavedSlider() { name = "test5", value = 1.2f });
            ExternalOptions.Instance().SavedSliders.Add(new SavedSlider() { name = "test6", value = 2.3f });

            ExternalOptions.Save("TestSaveLoadOptions");
            ExternalOptions.SetInstance(new ExternalOptions());

            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes.Count, 0);
            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns.Count, 0);
            Assert.AreEqual(ExternalOptions.Instance().SavedSliders.Count, 0);

            ExternalOptions.Load("TestSaveLoadOptions");

            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes.Count, 2);
            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns.Count, 2);
            Assert.AreEqual(ExternalOptions.Instance().SavedSliders.Count, 2);

            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes[0].name, "test1");
            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes[0].value, true);

            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes[1].name, "test2");
            Assert.AreEqual(ExternalOptions.Instance().SavedCheckboxes[1].value, false);

            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns[0].name, "test3");
            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns[0].value, "testDropdown1");

            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns[1].name, "test4");
            Assert.AreEqual(ExternalOptions.Instance().SavedDropdowns[1].value, "testDropdown2");

            Assert.AreEqual(ExternalOptions.Instance().SavedSliders[0].name, "test5");
            Assert.AreEqual(ExternalOptions.Instance().SavedSliders[0].value, 1.2f);

            Assert.AreEqual(ExternalOptions.Instance().SavedSliders[1].name, "test6");
            Assert.AreEqual(ExternalOptions.Instance().SavedSliders[1].value, 2.3f);
        }
    }
}
