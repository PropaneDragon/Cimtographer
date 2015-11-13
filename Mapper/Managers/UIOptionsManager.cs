using Mapper.CustomUI;
using System.Collections.Generic;

namespace Mapper.Managers
{
    class MapperOptionsManager
    {
        private static MapperOptionsManager instance_ = null;

        public static MapperOptionsManager Instance()
        {
            if(instance_ == null)
            {
                instance_ = new MapperOptionsManager();
            }

            return instance_;
        }

        public Dictionary<string, OptionItem> exportOptions = new Dictionary<string, OptionItem>()
        {
            {"paths", new OptionItem() { readableLabel = "Paths", enabled = true }},
            {"roads", new OptionItem() { readableLabel = "Roads", enabled = true }},
            {"highways", new OptionItem() { readableLabel = "Highways/Motorways", enabled = true }},
            {"buildingNames", new OptionItem() { readableLabel = "Building names", enabled = true }},
            {"buildingTypes", new OptionItem() { readableLabel = "Service buildings", enabled = true }},
            {"water", new OptionItem() { readableLabel = "Water", enabled = true }},
            {"subwayTrack", new OptionItem() { readableLabel = "Subway track", enabled = false }},
            {"busStops", new OptionItem() { readableLabel = "Bus stops", enabled = true }},
            {"busStations", new OptionItem() { readableLabel = "Bus stations", enabled = true }},
            {"trainStops", new OptionItem() { readableLabel = "Train stations", enabled = true }},
            {"tramStops", new OptionItem() { readableLabel = "Tram stations", enabled = true }},
            {"subwayStations", new OptionItem() { readableLabel = "Subway stations", enabled = true }},
            {"ferry", new OptionItem() { readableLabel = "Ferry terminals", enabled = true }},
            {"airports", new OptionItem() { readableLabel = "Airports", enabled = true }},
            {"taxis", new OptionItem() { readableLabel = "Taxi ranks", enabled = true }},
            {"power", new OptionItem() { readableLabel = "Power", enabled = true }},
            {"tunnels", new OptionItem() { readableLabel = "Tunnels", enabled = true }},
            {"bridges", new OptionItem() { readableLabel = "Bridges", enabled = true }}
        };

        public Dictionary<string, OptionItem> additionalOptions = new Dictionary<string, OptionItem>()
        {
            {"findTramStations", new OptionItem() { readableLabel = "Automatically find tram lines", enabled = true }}
        };
    }
}
