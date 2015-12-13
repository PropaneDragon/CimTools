using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CimTools.Workshop
{
    /// <summary>
    /// Used for grabbing change logs from the Steam Workshop. This will automatically
    /// get the most recent change log on the page and return it in either a list or a
    /// string.
    /// <para>
    /// The workshop changelogs must be formatted in a specific way in order for them to
    /// be correctly formatted when getting the output from the class. Every change must be
    /// within [list][/list] tags with [*] before each item when entering them into the workshop.
    /// </para>
    /// </summary>
    public class Changelog
    {
        /// <summary>
        /// The class instance
        /// </summary>
        protected static Changelog m_instance = null;

        /// <summary>
        /// The web client that accesses the community and downloads the changes
        /// </summary>
        protected WebClient m_webClient = new WebClient();

        /// <summary>
        /// A list of changes which is populated upon download.
        /// </summary>
        protected List<string> m_changeList = new List<string>()
        {
            "<color#f58282>You have not set this up for your mod!</color>",
            "You need to call <color#c8f582>DownloadChangelog</color> or <color#c8f582>DownloadChangelogAsync</color> to get changes!",
            "<color#f58282>If you're not the creator of this mod, let them know as soon as possible!</color>",
            "You might also want to read up on the documentation some more to make the most of this panel!"
        };

        /// <summary>
        /// A string which contains all changes. Populated upon download.
        /// </summary>
        protected string m_rawChanges =
            "<color#f58282>You have not set this up for your mod!</color>" +
            "\n\nYou need to call <color#c8f582>DownloadChangelog</color> or <color#c8f582>DownloadChangelogAsync</color> to get changes!" +
            "\n\n<color#f58282>If you're not the creator of this mod, let them know as soon as possible!</color>" +
            "\n\nYou might also want to read up on the documentation some more to make the most of this panel!";

        /// <summary>
        /// Whether the download is complete or not.
        /// </summary>
        protected bool m_downloadComplete = false;

        /// <summary>
        /// Whether a download is in progress or not.
        /// </summary>
        protected bool m_downloadInProgress = false;

        /// <summary>
        /// Whether the download has encountered an error.
        /// </summary>
        protected bool m_downloadError = false;

        /// <summary>
        /// If downloading Async, this determines whether or not there is an ongoing
        /// download.
        /// </summary>
        public bool DownloadInProgress
        {
            get
            {
                return m_downloadInProgress;
            }
        }

        /// <summary>
        /// If downloading Async, this will return whether the download has completed from
        /// that request. If the download fails it will still be false.
        /// </summary>
        public bool DownloadComplete
        {
            get
            {
                return m_downloadComplete;
            }
        }

        /// <summary>
        /// Whether an error has occured while downloading the most recent changes.
        /// </summary>
        public bool DownloadError
        {
            get
            {
                return m_downloadError;
            }
        }

        /// <summary>
        /// A formatted string of change logs. Each change log is separated by
        /// \n\n.
        /// </summary>
        public string ChangesString
        {
            get
            {
                return m_rawChanges;
            }
        }

        /// <summary>
        /// A list of change logs.
        /// </summary>
        public List<string> ChangesList
        {
            get
            {
                return m_changeList;
            }
        }

        /// <summary>
        /// Whether or not to replace HTML tags with colours.
        /// </summary>
        public bool m_colouriseTags = true;

        /// <summary>
        /// A list of tags to replace with colours.
        /// </summary>
        public List<KeyValuePair<string, Color>> m_tagsToColourise = new List<KeyValuePair<string, Color>>()
        {
            new KeyValuePair<string, Color>("b", new Color(.78f, .96f, .5f)),
            new KeyValuePair<string, Color>("u", new Color(.96f, .5f, .5f))
        };

        /// <summary>
        /// Gets and creates an instance of the Changelog class, if required.
        /// </summary>
        /// <returns>A Changelog class</returns>
        public static Changelog Instance()
        {
            if(m_instance == null)
            {
                m_instance = new Changelog();
            }

            return m_instance;
        }

        /// <summary>
        /// Downloads the latest changelog from a workshop item using the ID provided. Please note
        /// that this will stop the calling thread while it downloads, but you can guarantee the
        /// download will be complete when the thread resumes.
        /// </summary>
        public void DownloadChangelog()
        {
            if (Settings.WorkshopID != null)
            {
                m_changeList.Clear();
                m_rawChanges = "";

                m_downloadError = true;

                ExtractData(m_webClient.DownloadString(new Uri("http://steamcommunity.com/sharedfiles/filedetails/changelog/" + Settings.WorkshopID.ToString())));

                m_downloadComplete = true;
            }
        }

        /// <summary>
        /// Downloads the latest changelog from a workshop item using the ID provided. Please note
        /// that while this works on a separate thread, you can't guarantee the download will be
        /// complete immediately. You should check for a complete download using m_downloadCompleted
        /// before trying to get data from it.
        /// </summary>
        /// <seealso cref="m_downloadComplete"/>
        public void DownloadChangelogAsync()
        {
            if (Settings.WorkshopID != null)
            {
                m_changeList.Clear();
                m_rawChanges = "";

                m_downloadInProgress = true;
                m_downloadComplete = false;
                m_downloadError = true;

                try
                {
                    m_webClient.DownloadStringCompleted += M_webClient_DownloadStringCompleted;
                    m_webClient.DownloadStringAsync(new Uri("http://steamcommunity.com/sharedfiles/filedetails/changelog/" + Settings.WorkshopID.ToString()));
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        private void M_webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            m_downloadComplete = true;
            m_downloadInProgress = false;

            if (!e.Cancelled && e.Result != null && e.Result != "")
            {
                ExtractData(e.Result);
            }
        }

        /// <summary>
        /// Extracts data from a HTML page. This will only work on the Steam Workshop
        /// due to certain div IDs. You shouldn't need to call this yourself really, and
        /// should only be used if you really really need to use it.
        /// </summary>
        /// <param name="rawData"></param>
        protected void ExtractData(string rawData)
        {
            if (rawData != null && rawData != "")
            {
                int firstIndex = rawData.IndexOf("<div class=\"headline\"");
                int lastIndex = rawData.IndexOf("<div class=\"commentsLink changeLog\"");

                if (firstIndex > 0 && lastIndex > 0 && lastIndex > firstIndex)
                {
                    string splitData = rawData.Substring(firstIndex, lastIndex - firstIndex);
                    Regex findList = new Regex("(?:<ul.*?bb_ul.*?>)(.*)(?:<\\/ul>)");
                    Match matchedList = findList.Match(splitData);

                    if (matchedList.Success && matchedList.Groups.Count > 1)
                    {
                        string listData = matchedList.Groups[1].Value;
                        listData = listData.Replace("<li>", "\n\n");

                        if (m_colouriseTags)
                        {
                            foreach (KeyValuePair<string, Color> tag in m_tagsToColourise)
                            {
                                string hexValue = UIMarkupStyle.ColorToHex(tag.Value);
                                listData = listData.Replace("<" + tag.Key + ">", "|~|color" + hexValue + "!~|");
                                listData = listData.Replace("</" + tag.Key + ">", "|~|/color!~|");
                            }
                        }

                        Regex stripTags = new Regex("<.*?>");
                        listData = stripTags.Replace(listData, "");

                        listData = listData.Replace("|~|", "<");
                        listData = listData.Replace("!~|", ">");
                        listData = listData.Trim('\n');

                        m_rawChanges = listData;
                        m_changeList = listData.Split(new string[] { "\n\n" }, StringSplitOptions.None).ToList();
                        m_downloadError = false;
                    }
                }
            }
        }
    }
}
