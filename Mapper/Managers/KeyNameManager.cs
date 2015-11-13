using Mapper.Containers;
using System.Collections.Generic;

namespace Mapper.Managers
{
    class KeyNameManager
    {
        public static readonly List<KeyName> names = new List<KeyName>()
        {
            new KeyName() { key = "Oneway", tags =
            {
                new OSM.OSMWayTag() { k = "oneway", v = "yes" }
            } }
        };
    }
}
