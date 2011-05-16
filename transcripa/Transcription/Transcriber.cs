/* 
 * transcripa
 * http://code.google.com/p/transcripa/
 * 
 * Copyright 2011, Bryan McKelvey
 * Licensed under the MIT license
 * http://www.opensource.org/licenses/mit-license.php
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace transcripa
{
    /// <summary>
    /// A class containing information on each language and the available transliterations.
    /// </summary>
    public partial class Transcriber
    {
        private const string xmlPath = "Data/Transcriptions.xml";
        private XmlDocument xml = new XmlDocument();
        private Dictionary<string, Language> languages = new Dictionary<string, Language>();
        private string currentLanguage = "";

        /// <summary>
        /// A class containing information on each language and the available transliterations.
        /// </summary>
        public Transcriber()
        {
            XmlNodeList matches;
            xml.Load(xmlPath);
            matches = xml.SelectNodes("/Languages/Language");
            foreach (XmlNode match in matches)
            {
                string iso = match.Attributes["ISO"].Value;
                string name = match.Attributes["Name"].Value;

                languages.Add(name, new Language(iso, name));
            }
        }

        /// <summary>
        /// Loads the language from the XML.
        /// </summary>
        /// <param name="name">The name of the language to load.</param>
        /// <returns>The current object.</returns>
        public void Load(string name)
        {
            currentLanguage = name;
            Current().Load(ref xml);
        }

        /// <summary>
        /// The language in current use for transliteration.
        /// </summary>
        /// <returns></returns>
        private Language Current()
        {
            return languages[currentLanguage];
        }

        /// <summary>
        /// Transcribes the provided input string.
        /// </summary>
        /// <param name="input">The string to transcribe.</param>
        /// <returns>The IPA transcription.</returns>
        public string Transcribe(string input)
        {
            return Current().Transcribe(input);
        }

        #region Properties
        /// <summary>
        /// The path to the transcriptions XML file.
        /// </summary>
        public string XmlPath
        {
            get { return xmlPath; }
        }

        /// <summary>
        /// An string array consisting of the names of languages available for transliteration.
        /// </summary>
        public string[] Languages
        {
            get {
                string[] keys = new string[languages.Keys.Count];
                languages.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        /// <summary>
        /// The number of languages available for transliteration.
        /// </summary>
        public int Length
        {
            get { return languages.Count; }
        }

        /// <summary>
        /// The name of the current language being transliterated.
        /// </summary>
        public string CurrentLanguage
        {
            get { return currentLanguage; }
            set { currentLanguage = value; }
        }
        #endregion
    }
}
