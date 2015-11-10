using Mapper.OSM;
using Mapper.Utilities;
using System;
using System.Collections.Generic;

namespace Mapper.Containers
{
    class RoadContainer
    {
        /// <summary>
        /// Road types. Used for filtering.
        /// </summary>
        [Flags]
        public enum Type : byte
        {
            Unknown = 0,
            Path = 1,
            Cycleway = 2,
            Road = 4,
            Highway = 8,
            Train = 16,
            Subway = 32,
            Runway = 64,
            Busway = 128
        };

        /// <summary>
        /// Limits the search to only these types
        /// </summary>
        [Flags]
        public enum Limit : byte
        {
            None = 0,
            Tunnel = 1,
            Slope = 2,
            Ground = 4,
            Elevated = 8,
            Bridge = 16,
            Bicycle = 32,
            Bus = 64,
            Decoration = 128,
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

            UniqueLogger.AddLog("Road name matches", inGameNamePrefix, "");

            return roadNameCompare == comparisonName;
        }
    }
}
