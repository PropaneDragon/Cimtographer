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
            new BuildingContainer() { buildingClassName = "Beautification Item", tags =
            {
                new OSM.OSMWayTag() { k = "tourism", v = "attraction" },
                new OSM.OSMWayTag() { k = "building", v = "yes" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Beautification, tags =
            {
                new OSM.OSMWayTag() { k = "landuse", v = "recreation_ground" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Commercial, tags =
            {
                new OSM.OSMWayTag() { k = "building", v = "retail" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Residential, tags =
            {
                new OSM.OSMWayTag() { k = "building", v = "residential" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Office, tags =
            {
                new OSM.OSMWayTag() { k = "building", v = "commercial" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Industrial, tags =
            {
                new OSM.OSMWayTag() { k = "building", v = "industrial" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Garbage, tags =
            {
                new OSM.OSMWayTag() { k = "landuse", v = "landfill" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Education, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "school" },
                new OSM.OSMWayTag() { k = "building", v = "school" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Electricity, tags =
            {
                new OSM.OSMWayTag() { k = "power", v = "plant" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.FireDepartment, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "fire_station" },
                new OSM.OSMWayTag() { k = "building", v = "yes" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.HealthCare, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "hospital" },
                new OSM.OSMWayTag() { k = "building", v = "yes" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Monument, useName = true, tags =
            {
                new OSM.OSMWayTag() { k = "building", v = "yes" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PoliceDepartment, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "police" },
                new OSM.OSMWayTag() { k = "building", v = "yes" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportBus, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "bus_station" },
                new OSM.OSMWayTag() { k = "public_transport", v = "station" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportTrain, tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "station" },
                new OSM.OSMWayTag() { k = "public_transport", v = "station" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportTaxi, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "taxi" },
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportPlane, tags =
            {
                new OSM.OSMWayTag() { k = "aeroway", v = "terminal" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportMetro, tags =
            {
                new OSM.OSMWayTag() { k = "railway", v = "subway_entrance" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.PublicTransport, buildingSubService = ItemClass.SubService.PublicTransportShip, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "ferry_terminal" }
            } },
            new BuildingContainer() { buildingService = ItemClass.Service.Tourism, tags =
            {
                new OSM.OSMWayTag() { k = "amenity", v = "ferry_terminal" }
            } }
        };
    }
}
