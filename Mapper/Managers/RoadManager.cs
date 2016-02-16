using Mapper.Containers;
using Mapper.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mapper.Managers
{
    public static class RoadManager
    {
        private static List<RoadContainer> allRoadTypes = new List<RoadContainer>();

        /// <summary>
        /// A list containing all base roads. Add extensions as required.
        /// </summary>
        public static List<RoadContainer> roads = new List<RoadContainer>();

        /// <summary>
        /// All possible road elevations
        /// </summary>
        public static readonly List<KeyValuePair<string, RoadContainer.Limit>> roadElevations = new List<KeyValuePair<string, RoadContainer.Limit>>()
        {
            new KeyValuePair<string, RoadContainer.Limit>("Tunnel", RoadContainer.Limit.Tunnel),
            new KeyValuePair<string, RoadContainer.Limit>("Slope", RoadContainer.Limit.Slope),
            new KeyValuePair<string, RoadContainer.Limit>("", RoadContainer.Limit.Ground),
            new KeyValuePair<string, RoadContainer.Limit>("Elevated", RoadContainer.Limit.Elevated),
            new KeyValuePair<string, RoadContainer.Limit>("Bridge", RoadContainer.Limit.Bridge)
        };

        /// <summary>
        /// All possible road decorations
        /// </summary>
        public static readonly List<KeyValuePair<string, RoadContainer.Limit>> roadDecorations = new List<KeyValuePair<string, RoadContainer.Limit>>()
        {
            new KeyValuePair<string, RoadContainer.Limit>("Decoration Trees", RoadContainer.Limit.Decoration),
            new KeyValuePair<string, RoadContainer.Limit>("Decoration Grass", RoadContainer.Limit.Decoration)
        };

        /// <summary>
        /// All possible road lanes
        /// </summary>
        public static readonly List<KeyValuePair<string, RoadContainer.Limit>> roadLanes = new List<KeyValuePair<string, RoadContainer.Limit>>()
        {
            new KeyValuePair<string, RoadContainer.Limit>("Bike", RoadContainer.Limit.Bicycle),
            new KeyValuePair<string, RoadContainer.Limit>("Bicycle", RoadContainer.Limit.Bicycle),
            new KeyValuePair<string, RoadContainer.Limit>("Bus", RoadContainer.Limit.Bus)
        };

        /// <summary>
        /// Makes combinations of all possible road types.
        /// Some don't exist, but it catches everything that does
        /// (and maybe some future roads)
        /// </summary>
        /// <returns>A list of all possible road types in game</returns>
        public static List<RoadContainer> GetAllRoadTypes()
        {
            allRoadTypes.Clear();
            LoadRoadsFromFile();

            foreach(RoadContainer road in roads)
            {
                if (road.linkedOption == "" || MapperOptionsManager.OptionChecked(road.linkedOption, MapperOptionsManager.exportOptions))
                {
                    List<RoadContainer> roadPlusElevations = AddRoadExtensions(road, roadElevations);

                    //allRoadTypes.Add(road);
                    allRoadTypes.AddRange(roadPlusElevations);
                    allRoadTypes.AddRange(AddRoadExtensions(road, roadDecorations));
                    allRoadTypes.AddRange(AddRoadExtensions(road, roadLanes));

                    foreach (RoadContainer roadPlusElevation in roadPlusElevations)
                    {
                        allRoadTypes.AddRange(AddRoadExtensions(roadPlusElevation, roadLanes));
                    }
                }
            }

            UniqueLogger.PrintLog("Road name matches");
            UniqueLogger.PrintLog("Road names missing from search");
            UniqueLogger.PrintLog("Building names missing from search");

            return allRoadTypes;
        }

        internal static void LoadRoadsFromFile()
        {
            XmlRoads _xmlRoads = XmlRoadHandler.LoadRoads();

            Debug.Log("Loading roads from file...");

            foreach(XmlRoad road in _xmlRoads.roads)
            {
                RoadContainer _container = new RoadContainer();
                List<OSM.OSMWayTag> _tags = new List<OSM.OSMWayTag>();

                _container.inGameNamePostfix = road.postfix;
                _container.inGameNamePrefix = road.prefix;
                _container.linkedOption = road.linkedOption;
                _container.roadType = Enum.Parse(typeof(RoadContainer.Type), road.roadType) as RoadContainer.Type? ?? RoadContainer.Type.Unknown;
                _container.searchLimit = Enum.Parse(typeof(RoadContainer.Limit), road.searchLimit) as RoadContainer.Limit? ?? RoadContainer.Limit.None;
                
                foreach(XmlOSMTag tag in road.tags)
                {
                    _tags.Add(new OSM.OSMWayTag() { k = tag.key, v = tag.value });
                }

                _container.tags = _tags;

                Debug.Log("Added " + road.prefix + " " + road.postfix);

                roads.Add(_container);
            }
        }

        /// <summary>
        /// Adds an extension to the name of the base road
        /// based on the roadExtensions list
        /// </summary>
        /// <param name="baseRoad">The initial road to add extension names to</param>
        /// <param name="roadExtensions">A list of strings containing possible road name extensions</param>
        /// <returns>A list of new roads plus all extensions in the name</returns>
        public static List<RoadContainer> AddRoadExtensions(RoadContainer baseRoad, List<KeyValuePair<string, RoadContainer.Limit>> roadExtensions)
        {
            List<RoadContainer> returnRoads = new List<RoadContainer>();

            foreach (KeyValuePair<string, RoadContainer.Limit> roadExtension in roadExtensions)
            {
                if(baseRoad.searchLimit == RoadContainer.Limit.None || (baseRoad.searchLimit & roadExtension.Value) != RoadContainer.Limit.None)
                {
                    RoadContainer modifiedRoad = new RoadContainer { inGameNamePrefix = baseRoad.inGameNamePrefix + " " + roadExtension.Key, roadType = baseRoad.roadType, inGameNamePostfix = baseRoad.inGameNamePostfix, searchLimit = baseRoad.searchLimit, tags = baseRoad.tags };
                    returnRoads.Add(modifiedRoad);
                }
            }

            return returnRoads;
        }
    }
}
