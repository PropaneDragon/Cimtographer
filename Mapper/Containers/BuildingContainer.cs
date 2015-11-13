using Mapper.OSM;
using System.Collections.Generic;

namespace Mapper.Containers
{
    class BuildingContainer
    {
        /// <summary>
        /// Main building service search. Service.None skips.
        /// </summary>
        public List<ItemInversionContainer<ItemClass.Service>> buildingServices = new List<ItemInversionContainer<ItemClass.Service>>();

        /// <summary>
        /// Main building sub service search. SubService.None skips.
        /// </summary>
        public List<ItemInversionContainer<ItemClass.SubService>> buildingSubServices = new List<ItemInversionContainer<ItemClass.SubService>>();

        /// <summary>
        /// Class name search. Empty string skips.
        /// </summary>
        public List<ItemInversionContainer<string>> buildingClassNames = new List<ItemInversionContainer<string>>();

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
