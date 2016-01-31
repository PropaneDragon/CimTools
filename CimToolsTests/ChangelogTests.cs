﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CimTools.V1.Workshop
{
    class ChangelogTest : Changelog
    {
        public ChangelogTest(CimToolSettings settings) : base(settings)
        {            
        }

        public void ForceExtractData(string data)
        {
            ExtractData(data);
        }
    }

    [TestClass]
    public class ChangelogTests
    {
        [TestMethod]
        public void ReturnList()
        {
            ChangelogTest tester = new ChangelogTest(new CimToolSettings(""));
            string testData = "<div class=\"test\"><u><b><i><div class=\"headline\"></div><ul class=\"bb_ul\"><li><b>TEST</b><u> test2</u></li><li>Another <invalid tag with stuff in><b>test</b> list</ul><div class=\"commentsLink changeLog\"></div></div>";

            tester.ForceExtractData(testData);

            List<string> changelogList = tester.ChangesList;

            Assert.AreEqual(changelogList.Count, 2);

            if(changelogList.Count >= 2)
            {
                Assert.AreEqual(changelogList[0], "<color#C6F47F>TEST</color><color#F47F7F> test2</color>");
                Assert.AreEqual(changelogList[1], "Another <color#C6F47F>test</color> list");
            }
        }

        [TestMethod]
        public void ReturnString()
        {
            ChangelogTest tester = new ChangelogTest(new CimToolSettings(""));
            string testData = "<div class=\"test\"><u><b><i><div class=\"headline\"></div><ul class=\"bb_ul\"><li><b>TEST</b><u> test2</u></li><li>Another <invalid tag with stuff in><b>test</b> list</ul><div class=\"commentsLink changeLog\"></div></div>";

            tester.ForceExtractData(testData);

            string changelogList = tester.ChangesString;

            Assert.AreEqual(changelogList, "<color#C6F47F>TEST</color><color#F47F7F> test2</color>\n\nAnother <color#C6F47F>test</color> list");
        }
    }
}
