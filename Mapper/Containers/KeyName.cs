using Mapper.OSM;
using System;
using System.Collections.Generic;

namespace Mapper.Containers
{
    class KeyName
    {
        /// <summary>
        /// The key associated with the tags
        /// </summary>
        public string key = "";

        /// <summary>
        /// All OSM tags this key is associated with.
        /// </summary>
        public List<OSMWayTag> tags = new List<OSMWayTag>();

        /// <summary>
        /// Checks whether the name matches the input name.
        /// </summary>
        /// <param name="comparisonString">The name to compare against</param>
        /// <returns>Whether the key matches the comparison name.</returns>
        public bool RoadNameMatches(string comparisonString)
        {
            string nameCompare = key.ToLower().Replace(" ", "");
            comparisonString = comparisonString.ToLower().Replace(" ", "");

            return comparisonString.Contains(nameCompare);
        }
    }
}
