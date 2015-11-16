using Mapper.CustomUI;
using System.Collections.Generic;
using System.Reflection;

namespace Mapper.Managers
{
    static class MapperOptionsManager
    {
        public static readonly int major = Assembly.GetExecutingAssembly().GetName().Version.Major;
        public static readonly int minor = Assembly.GetExecutingAssembly().GetName().Version.Minor;
        public static readonly int revision = Assembly.GetExecutingAssembly().GetName().Version.Revision;

        /// <summary>
        /// Options to set what to be exported
        /// </summary>
        public static Dictionary<string, OptionItem> exportOptions = new Dictionary<string, OptionItem>()
        {
            {"paths", new OptionItem() { readableLabel = "Paths", value = true }},
            {"cycleways", new OptionItem() { readableLabel = "Cycleways", value = true }},
            {"roads", new OptionItem() { readableLabel = "Roads", value = true }},
            {"highways", new OptionItem() { readableLabel = "Highways", value = true }},
            {"rail", new OptionItem() { readableLabel = "Rail", value = true }},
            {"subwayTrack", new OptionItem() { readableLabel = "Subway track", value = false, enabled = false }},
            {"buildingNames", new OptionItem() { readableLabel = "Building names", value = true }},
            {"serviceBuildings", new OptionItem() { readableLabel = "Service buildings", value = true }},
            {"buildings", new OptionItem() { readableLabel = "Regular buildings", value = true }},
            {"parks", new OptionItem() { readableLabel = "Parks", value = true }},
            {"busStops", new OptionItem() { readableLabel = "Bus stops", value = true }},
            {"busStations", new OptionItem() { readableLabel = "Bus stations", value = true }},
            {"trainStops", new OptionItem() { readableLabel = "Train stations", value = true }},
            {"tramStops", new OptionItem() { readableLabel = "Tram stations", value = true }},
            {"subwayStations", new OptionItem() { readableLabel = "Subway stations", value = true }},
            {"ferry", new OptionItem() { readableLabel = "Ferry terminals", value = true }},
            {"airports", new OptionItem() { readableLabel = "Airports", value = true }},
            {"taxis", new OptionItem() { readableLabel = "Taxi ranks", value = true }},
            {"power", new OptionItem() { readableLabel = "Power", value = true }},
            {"powerCables", new OptionItem() { readableLabel = "Power Cables", value = false }},
            {"tunnels", new OptionItem() { readableLabel = "Tunnels", value = true }},
            {"bridges", new OptionItem() { readableLabel = "Bridges", value = true }},
            {"water", new OptionItem() { readableLabel = "Water", value = true, enabled = false }},
            {"waterPipes", new OptionItem() { readableLabel = "Water Pipes", value = false }}
        };

        public static Dictionary<string, OptionItem> additionalOptions = new Dictionary<string, OptionItem>()
        {
            {"findTramStations", new OptionItem() { readableLabel = "Automatically find tram lines", value = true, enabled = false }}
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
