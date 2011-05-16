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
using System.Xml;

namespace transcripa
{
    public partial class Transcriber
    {
        public partial class Language
        {
            private string iso;
            private string name;
            private List<Transcription> transcriptions = new List<Transcription>();
            private bool loaded = false;
            private const string xpath = "/Languages/Language[@Name=\"{0}\"]/Transcription";

            /// <summary>
            /// Constructs a language of the specified ISO code and full name.
            /// </summary>
            /// <param name="iso">The two-letter ISO code for the language.</param>
            /// <param name="name">The full English name of the language.</param>
            public Language(string iso, string name)
            {
                this.iso = iso;
                this.name = name;
            }

            /// <summary>
            /// Loads a language from the provided XML document reference.
            /// </summary>
            /// <param name="xml">A reference to the transliterations XML document.</param>
            public void Load(ref XmlDocument xml)
            {
                if (!loaded)
                {
                    XmlNodeList matches = xml.SelectNodes(string.Format(xpath, name));

                    foreach (XmlNode match in matches)
                    {
                        // A little lenient, in that it allows for all or none of these attributes to exist
                        string original = match.GetAttribute("Original");
                        string replacement = match.GetAttribute("Replacement");
                        string prefix = match.GetAttribute("Prefix");
                        string suffix = match.GetAttribute("Suffix");

                        transcriptions.Add(new Transcription(original, replacement, prefix, suffix));
                    }
                }
            }

            /// <summary>
            /// Transcribes a string according to the rules for the language.
            /// </summary>
            /// <param name="input">The string to transcribe.</param>
            /// <returns>The IPA transcription.</returns>
            public string Transcribe(string input)
            {
                StringBuilder builder = new StringBuilder();
                input = " " + input + " ";
                int inputLength = input.Length;

                for (int i = 1; i < inputLength - 1; i++)
                {
                    bool hasMatch = false;
                    foreach (Transcription transcription in transcriptions)
                    {
                        int matchLength = transcription.IsMatch(input, i);
                        if (matchLength != 0)
                        {
                            hasMatch = true;
                            builder.Append(transcription.Replacement);
                            i += matchLength - 1;
                            break;
                        }
                    }

                    if (!hasMatch) builder.Append(input[i]);
                }

                return builder.ToString().Trim();
            }

            #region Properties
            /// <summary>
            /// The two-letter ISO code for the language.
            /// </summary>
            public string IsoCode
            {
                get { return iso; }
                set { iso = value; }
            }

            /// <summary>
            /// The full English name of the language.
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// Whether the transliterations for the language have been loaded or not.
            /// </summary>
            public bool Loaded
            {
                get { return loaded; }
            }

            /// <summary>
            /// The XPath used to query for matching segments.
            /// </summary>
            public string XPath
            {
                get { return string.Format(xpath, name); }
            }
            #endregion
        }
    }
}
