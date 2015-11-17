using Mapper.CustomUI;
using System.Collections.Generic;
using System.Reflection;

namespace Mapper.Managers
{
    static class MapperOptionsManager
    {
        public static readonly int major = Assembly.GetExecutingAssembly().GetName().Version.Major;
        public static readonly int minor = Assembly.GetExecutingAssembly().GetName().Version.Minor;
        public static readonly int build = Assembly.GetExecutingAssembly().GetName().Version.Build;
        public static readonly int revision = Assembly.GetExecutingAssembly().GetName().Version.Revision;

        /// <summary>
        /// Options to set what to be exported
        /// </summary>
        public static Dictionary<string, OptionItem> exportOptions = new Dictionary<string, OptionItem>()
        {
            {"districts", new OptionItem() { readableLabel = "Districts" }},
            {"paths", new OptionItem() { readableLabel = "Paths" }},
            {"cycleways", new OptionItem() { readableLabel = "Cycleways" }},
            {"roads", new OptionItem() { readableLabel = "Roads" }},
            {"highways", new OptionItem() { readableLabel = "Highways" }},
            {"rail", new OptionItem() { readableLabel = "Rail" }},
            {"subwayTrack", new OptionItem() { readableLabel = "Subway track", value = false, enabled = false }},
            {"buildingNames", new OptionItem() { readableLabel = "Building names" }},
            {"serviceBuildings", new OptionItem() { readableLabel = "Service buildings" }},
            {"buildings", new OptionItem() { readableLabel = "Regular buildings" }},
            {"parks", new OptionItem() { readableLabel = "Parks" }},
            {"busStops", new OptionItem() { readableLabel = "Bus stops" }},
            {"busStations", new OptionItem() { readableLabel = "Bus stations" }},
            {"trainStops", new OptionItem() { readableLabel = "Train stations" }},
            {"tramStops", new OptionItem() { readableLabel = "Tram stations" }},
            {"subwayStations", new OptionItem() { readableLabel = "Subway stations" }},
            {"ferry", new OptionItem() { readableLabel = "Ferry terminals" }},
            {"airports", new OptionItem() { readableLabel = "Airports" }},
            {"taxis", new OptionItem() { readableLabel = "Taxi ranks" }},
            {"power", new OptionItem() { readableLabel = "Power" }},
            {"powerCables", new OptionItem() { readableLabel = "Power Cables", value = false }},
            {"tunnels", new OptionItem() { readableLabel = "Tunnels" }},
            {"bridges", new OptionItem() { readableLabel = "Bridges" }},
            {"water", new OptionItem() { readableLabel = "Water", enabled = false }},
            {"waterPipes", new OptionItem() { readableLabel = "Water Pipes", value = false }}
        };

        public static Dictionary<string, OptionItem> additionalOptions = new Dictionary<string, OptionItem>()
        {
            {"findTramStations", new OptionItem() { readableLabel = "Automatically find tram lines", enabled = false }}
        };

        /// <summary>
        /// Gets the value of the option
        /// </summary>
        /// <param name="optionName">The option key</param>
        /// <param name="optionList">The list to search for the option in</param>
        /// <returns></returns>
        public static bool OptionChecked(string optionName, Dictionary<string, OptionItem> optionList)
        {
            bool returnValue = false;

            if(optionList.ContainsKey(optionName))
            {
                returnValue = optionList[optionName].value;
            }

            return returnValue;
        }
    }
}
