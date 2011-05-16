/* 
 * transcripa
 * http://code.google.com/p/transcripa/
 * 
 * Copyright 2011, Bryan McKelvey
 * Licensed under the MIT license
 * http://www.opensource.org/licenses/mit-license.php
 */

using System;
using System.Xml;

namespace transcripa
{
    public static partial class Extensions
    {
        /// <summary>
        /// Determines whether the current node has an attribute with the specified name.
        /// </summary>
        /// <param name="node">The node in which to search for an attribute.</param>
        /// <param name="name">The name of the attribute to find.</param>
        /// <returns>Whether the current node has an attribute with the specified name.</returns>
        public static bool HasAttribute(this XmlNode node, string name)
        {
            XmlNode attributeNode = node.Attributes[name];
            return (attributeNode != null);
        }

        /// <summary>
        /// Gets the value of an attribute of the specified name from the current node,
        /// returning a blank string if no such attribute exists.
        /// </summary>
        /// <param name="node">The node in which to search for an attribute.</param>
        /// <param name="name">The name of the attribute to find.</param>
        /// <returns>The attribute value.</returns>
        public static string GetAttribute(this XmlNode node, string name)
        {
            return (node.HasAttribute(name) ? node.Attributes[name].Value : "");
        }

    }
}
