using Mapper.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapper.Managers
{
    static class BuildingManager
    {
        public static readonly List<BuildingContainer> buildings = new List<BuildingContainer>()
        {
            new BuildingContainer()
            {
                linkedOption = "parks",
                buildingClassNames =
                {
                    new ItemInversionContainer<string>("Beautification Item")
                },

                tags = 
                {
                    new OSM.OSMWayTag() { k = "tourism", v = "attraction" },
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "parks",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Beautification)
                },

                buildingClassNames =
                {
                    new ItemInversionContainer<string>("Marker", ItemInversionContainer<string>.ValidationType.Inverted)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "landuse", v = "recreation_ground" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "buildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Commercial)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "building", v = "retail" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "buildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Residential)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "building", v = "residential" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "buildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Office)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "building", v = "commercial" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "buildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Industrial)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "building", v = "industrial" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "serviceBuildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Garbage)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "landuse", v = "landfill" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "serviceBuildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Education)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "school" },
                    new OSM.OSMWayTag() { k = "building", v = "school" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "power",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Electricity)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "power", v = "plant" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "serviceBuildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.FireDepartment)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "fire_station" },
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "serviceBuildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.HealthCare)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "hospital" },
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "parks",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Monument)
                },

                useName = true,
                tags =
                {
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "serviceBuildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PoliceDepartment)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "police" },
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "busStations",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportBus)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "bus_station" },
                    new OSM.OSMWayTag() { k = "public_transport", v = "station" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "trainStops",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportTrain)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "railway", v = "station" },
                    new OSM.OSMWayTag() { k = "public_transport", v = "station" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "taxis",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportTaxi)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "taxi" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "airports",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportPlane)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "aeroway", v = "terminal" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "subwayStations",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportMetro)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "railway", v = "subway_entrance" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "ferry",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.PublicTransport)
                },

                buildingSubServices =
                {
                    new ItemInversionContainer<ItemClass.SubService>(ItemClass.SubService.PublicTransportShip)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "amenity", v = "ferry_terminal" }
                }
            },

            new BuildingContainer()
            {
                linkedOption = "buildings",
                buildingServices =
                {
                    new ItemInversionContainer<ItemClass.Service>(ItemClass.Service.Tourism)
                },

                tags =
                {
                    new OSM.OSMWayTag() { k = "tourism", v = "attraction" },
                    new OSM.OSMWayTag() { k = "building", v = "yes" }
                }
            }
        };
    }
}
