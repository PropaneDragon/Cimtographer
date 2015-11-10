using Mapper.OSM;
using System.Collections.Generic;

namespace Mapper.Containers
{
    class BuildingContainer
    {
        /// <summary>
        /// Main building service search. Service.None skips.
        /// </summary>
        public ItemClass.Service buildingService = ItemClass.Service.None;

        /// <summary>
        /// Main building sub service search. SubService.None skips.
        /// </summary>
        public ItemClass.SubService buildingSubService = ItemClass.SubService.None;

        /// <summary>
        /// Class name search. Empty string skips.
        /// </summary>
        public string buildingClassName = "";

        /// <summary>
        /// All OSM tags this building is associated with.
        /// </summary>
        public List<OSMWayTag> tags = new List<OSMWayTag>();

        /// <summary>
        /// Use the name of the building when exporting.
        /// </summary>
        public bool useName = false;
    }
}
