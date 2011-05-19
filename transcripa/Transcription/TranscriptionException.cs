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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace transcripa
{
    public partial class Transcription
    {
        /// <summary>
        /// An exception to the default transcription or transliteration.
        /// </summary>
        private class TranscriptionException
        {
            private Regex prefixRegex;
            private Regex suffixRegex;

            /// <summary>
            /// An exception to the default transcription or transliteration.
            /// </summary>
            /// <param name="replacement">A string representing what the combination will be replaced with.</param>
            /// <param name="prefix">The regular expression pattern for checking the prefix. Defaults to blank.</param>
            /// <param name="suffix">The regular expression pattern for checking the prefix. Defaults to blank.</param>
            public TranscriptionException(string replacement, string prefix, string suffix)
            {
                Replacement=replacement;
                if (prefix != "")
                {
                    this.prefixRegex = new Regex("(?:" + prefix + ")$", Transcription.regexOptions);
                }
                if (suffix != "")
                {
                    this.suffixRegex = new Regex("^(?:" + suffix + ")", Transcription.regexOptions);
                }
            }

            /// <summary>
            /// Determines whether the input string is a match for the exception.
            /// </summary>
            /// <param name="prefix">The string prefix to search for an exception.</param>
            /// <param name="suffix">The string suffix to search for an exception.</param>
            /// <returns>True or false, depending on whether the string qualifies as an exception.</returns>
            public bool IsMatch(string prefix, string suffix)
            {
                return ((prefixRegex == null || prefixRegex.IsMatch(prefix)) &&
                        (suffixRegex == null || suffixRegex.IsMatch(suffix)));
            }

            #region Properties
            /// <summary>
            /// A string representing what the combination will be replaced with.
            /// </summary>
            public string Replacement { get; set; }
            #endregion
        }
    }
}
