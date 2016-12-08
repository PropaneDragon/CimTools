using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TranslationHelper
{
    public partial class TranslationHelperWindow : Form
    {
        public TranslationHelperWindow()
        {
            InitializeComponent();
        }

        private void neatenButton_Click(object sender, EventArgs e)
        {
            Regex findTranslation = new Regex("(<Translation.*?)(ID=\"(.*?)\")");
            Regex findString = new Regex("(.*<Translation.*?String=\")(.*)(\".*)");
            Regex requiresTranslationFinder = new Regex("(<Translation.*?)(<!--.*TRANSLATE.*ME)");

            string[] referenceText = referenceTranslation.Text.Split('\n');
            string comparisonText = convertedTranslation.Text;
            string finalText = "";

            convertedTranslation.Clear();

            foreach (string line in referenceText)
            {
                if(!findTranslation.IsMatch(line))
                {
                    finalText += line + "\n";
                }
                else
                {
                    string id = findTranslation.Match(line).Groups[3].Value;
                    Regex findID = new Regex("(<Translation.*?)(ID=\"" + id + "\").*(String=\"(.*)\")");

                    if (findID.IsMatch(comparisonText))
                    {
                        string matchedLine = findID.Match(comparisonText).Value;

                        if(requiresTranslationFinder.IsMatch(line))
                        {
                            finalText += line + "\n";
                        }
                        else
                        {
                            string replacedText = findString.Replace(line, "${1}" + findString.Match(matchedLine).Groups[2] + "${3}") + "\n";
                            finalText += replacedText;
                        }
                    }
                    else
                    {
                        finalText += line + "\n";
                    }
                }
            }

            convertedTranslation.Text = finalText;
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            neatenButton_Click(sender, e);

            Clipboard.SetText(convertedTranslation.Text);
            convertedTranslation.Clear();
        }
    }
}
