using Mapper.Containers;
using System.Collections.Generic;

namespace Mapper.Managers
{
    public static class RoadManager
    {
        private static List<RoadContainer> allRoadTypes = new List<RoadContainer>();

        /// <summary>
        /// A list containing all base roads. Add extensions as required.
        /// </summary>
        public static readonly List<RoadContainer> roads = new List<RoadContainer>()
        {
            //Rail
            new RoadContainer() { inGameNamePrefix = "Train Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" }
            } },
            new RoadContainer() { inGameNamePrefix = "Train Cargo Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" }
            } },
            new RoadContainer() { inGameNamePrefix = "Train Station Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "platform" }
            } },
            new RoadContainer() { inGameNamePrefix = "Station Track Eleva", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "platform" },
                new OSM.OSMWayTag() { k = "bridge", v = "yes" },
                new OSM.OSMWayTag() { k = "level", v = "1" },
                new OSM.OSMWayTag() { k = "layer", v = "1" }
            } },
            new RoadContainer() { inGameNamePrefix = "Station Track Sunken", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "platform" },
                new OSM.OSMWayTag() { k = "tunnel", v = "yes" },
                new OSM.OSMWayTag() { k = "level", v = "-1" },
                new OSM.OSMWayTag() { k = "layer", v = "-1" }
            } },
            new RoadContainer() { inGameNamePrefix = "Metro Track", roadType = RoadContainer.Type.Subway, linkedOption = "subwayTrack", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "subway" },
                new OSM.OSMWayTag() { k = "tunnel", v = "yes" },
                new OSM.OSMWayTag() { k = "level", v = "-1" },
                new OSM.OSMWayTag() { k = "layer", v = "-1" }
            } },
            new RoadContainer() { inGameNamePrefix = "Metro Station Track", roadType = RoadContainer.Type.Subway, linkedOption = "subwayTrack", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "subway" },
                new OSM.OSMWayTag() { k = "tunnel", v = "yes" },
                new OSM.OSMWayTag() { k = "level", v = "-1" },
                new OSM.OSMWayTag() { k = "layer", v = "-1" }
            } },
            new RoadContainer() { inGameNamePrefix = "Oneway Train Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Concrete Train Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "Illuminated Tracks", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "Illumin No Cable", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "No Cable Train Track", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "No Cable Train Elevat", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "No Cable Concrete Trac", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "Tram Tracks", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },
            new RoadContainer() { inGameNamePrefix = "Tram Tracks No Cable", roadType = RoadContainer.Type.Train, linkedOption = "rail", tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "rail" },
            } },

            //Paved paths
            new RoadContainer() { inGameNamePrefix = "Pedestrian", roadType = RoadContainer.Type.Path, linkedOption = "paths", searchLimit = RoadContainer.Limit.ElevationsOnly, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "pedestrian" }
            } },
            new RoadContainer() { inGameNamePrefix = "Pedestrian", roadType = RoadContainer.Type.Cycleway, linkedOption = "cycleways", searchLimit = RoadContainer.Limit.Bicycle, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "cycleway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Pedestrian Pavement", roadType = RoadContainer.Type.Path, linkedOption = "paths", searchLimit = RoadContainer.Limit.ElevationsOnly, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "pedestrian" }
            } },
            new RoadContainer() { inGameNamePrefix = "Pedestrian Pavement", roadType = RoadContainer.Type.Cycleway, linkedOption = "cycleways", searchLimit = RoadContainer.Limit.Bicycle, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "cycleway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Pedestrian Pavement", inGameNamePostfix = "Bicycle", roadType = RoadContainer.Type.Cycleway, linkedOption = "cycleways", searchLimit = RoadContainer.Limit.ElevationsOnly, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "cycleway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Pedestrian", inGameNamePostfix = "Bicycle", roadType = RoadContainer.Type.Cycleway, linkedOption = "cycleways", searchLimit = RoadContainer.Limit.ElevationsOnly, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "cycleway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Zonable Pedestrian Pavement", roadType = RoadContainer.Type.Path, linkedOption = "paths", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "pedestrian" }
            } },
            new RoadContainer() { inGameNamePrefix = "Zonable Pedestrian", roadType = RoadContainer.Type.Path, linkedOption = "paths", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "pedestrian" }
            } },

            //Gravel paths
            new RoadContainer() { inGameNamePrefix = "Pedestrian Gravel", roadType = RoadContainer.Type.Path, linkedOption = "paths", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "pedestrian" }
            } },
            new RoadContainer() { inGameNamePrefix = "Gravel Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "track" }
            } },
            new RoadContainer() { inGameNamePrefix = "Zonable Pedestrian Gravel", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "track" }
            } },

            //Highway
            new RoadContainer() { inGameNamePrefix = "Highway Ramp", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "secondary_link" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Rural Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Small Rural Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Highway Barrier", roadType = RoadContainer.Type.Highway, linkedOption = "highways", searchLimit = RoadContainer.Limit.Ground, tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Large Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Five-Lane Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Four-Lane Highway", roadType = RoadContainer.Type.Highway, linkedOption = "highways", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "motorway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },

            //Small roads
            new RoadContainer() { inGameNamePrefix = "Basic Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "residential" }
            } },
            new RoadContainer() { inGameNamePrefix = "Harbor Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "residential" }
            } },
            new RoadContainer() { inGameNamePrefix = "Oneway Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "road" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Oneway 3L", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "road" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },

            //Medium roads
            new RoadContainer() { inGameNamePrefix = "Oneway 4L", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "tertiary" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Medium Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "tertiary" }
            } },
            new RoadContainer() { inGameNamePrefix = "Small Avenue", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "tertiary" }
            } },
            new RoadContainer() { inGameNamePrefix = "Medium Avenue", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "tertiary" }
            } },
            new RoadContainer() { inGameNamePrefix = "Medium Avenue TL", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "tertiary" }
            } },

            //Large roads
            new RoadContainer() { inGameNamePrefix = "Large Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "secondary" }
            } },
            new RoadContainer() { inGameNamePrefix = "Large Road", inGameNamePostfix = "With Bus Lanes", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "secondary" }
            } },
            new RoadContainer() { inGameNamePrefix = "Large Oneway", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "trunk" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },
            new RoadContainer() { inGameNamePrefix = "Large Oneway Road", roadType = RoadContainer.Type.Road, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "trunk" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },

            //Busways
            new RoadContainer() { inGameNamePrefix = "Small Busway", roadType = RoadContainer.Type.Busway, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "bus_guideway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Small Busway One Way", roadType = RoadContainer.Type.Busway, linkedOption = "roads", tags =
            {
                new OSM.OSMWayTag() { k = "highway", v = "bus_guideway" },
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } },

            //Air
            new RoadContainer() { inGameNamePrefix = "Airplane Taxiway", roadType = RoadContainer.Type.Runway, linkedOption = "airports", tags =
            {
                new OSM.OSMWayTag() { k = "aeroway", v = "taxiway" }
            } },
            new RoadContainer() { inGameNamePrefix = "Airplane Runway", roadType = RoadContainer.Type.Runway, linkedOption = "airports", tags =
            {
                new OSM.OSMWayTag() { k = "aeroway", v = "runway" }
            } },

            //Other
            new RoadContainer() { inGameNamePrefix = "Dam", roadType = RoadContainer.Type.Unknown, linkedOption = "water", tags =
            {
                new OSM.OSMWayTag() { k = "waterway", v = "dam" }
            } },
            new RoadContainer() { inGameNamePrefix = "Water Pipe", roadType = RoadContainer.Type.Water, linkedOption = "waterPipes", tags =
            {
                new OSM.OSMWayTag() { k = "man_made", v = "pipeline" },
                new OSM.OSMWayTag() { k = "location", v = "underground" }
            } },
            new RoadContainer() { inGameNamePrefix = "Power Line", roadType = RoadContainer.Type.Electricity, linkedOption = "powerCables", tags =
            {
                new OSM.OSMWayTag() { k = "power", v = "line" }
            } }
        };

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
            if(allRoadTypes.Count == 0)
            {
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
            }

            return allRoadTypes;
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
