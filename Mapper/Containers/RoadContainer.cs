using Mapper.OSM;
using Mapper.Utilities;
using System;
using System.Collections.Generic;

namespace Mapper.Containers
{
    public class RoadContainer
    {
        /// <summary>
        /// Road types. Used for filtering.
        /// </summary>
        [Flags]
        public enum Type : byte
        {
            Unknown = 0,
            Path = 1 << 0,
            Cycleway = 1 << 1,
            Road = 1 << 2,
            Highway = 1 << 3,
            Train = 1 << 4,
            Subway = 1 << 5,
            Runway = 1 << 6,
            Busway = 1 << 7
        };

        /// <summary>
        /// Limits the search to only these types
        /// </summary>
        [Flags]
        public enum Limit : byte
        {
            None = 0,
            Tunnel = 1 << 0,
            Slope = 1 << 1,
            Ground = 1 << 2,
            Elevated = 1 << 3,
            Bridge = 1 << 4,
            Bicycle = 1 << 5,
            Bus = 1 << 6,
            Decoration = 1 << 7,
            ElevationsOnly = Tunnel | Slope | Ground | Elevated | Bridge
        };

        /// <summary>
        /// The human readable name of the road as it is
        /// in game before adding extensions.
        /// </summary>
        public string inGameNamePrefix = "";

        /// <summary>
        /// The human readable name of the road as it is
        /// in game after adding extensions.
        /// </summary>
        public string inGameNamePostfix = "";

        /// <summary>
        /// All OSM tags this road is associated with.
        /// </summary>
        public List<OSMWayTag> tags = new List<OSMWayTag>();

        /// <summary>
        /// The type of road.
        /// </summary>
        public Type roadType = Type.Unknown;

        /// <summary>
        /// Limit the search to only these types.
        /// </summary>
        public Limit searchLimit = Limit.None;

        /// <summary>
        /// Checks whether the name of the road matches
        /// the input name.
        /// </summary>
        /// <param name="comparisonName">The road name to compare against</param>
        /// <returns>Whether the name of the road matches the comparison name.</returns>
        public bool RoadNameMatches(string comparisonName)
        {
            string prefixCompare = inGameNamePrefix.ToLower().Replace(" ", "");
            string postfixCompare = inGameNamePostfix.ToLower().Replace(" ", "");
            string roadNameCompare = prefixCompare + postfixCompare;
            comparisonName = comparisonName.ToLower().Replace(" ", "");

            UniqueLogger.AddLog("Road name matches", inGameNamePrefix + " " + inGameNamePostfix, "");

            return roadNameCompare == comparisonName;
        }
    }
}
