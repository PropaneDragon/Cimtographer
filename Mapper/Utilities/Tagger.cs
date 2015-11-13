using ColossalFramework;
using Mapper.Containers;
using Mapper.Managers;
using Mapper.OSM;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mapper.Utilities
{
    static class Tagger
    {
        public static bool CreateWayTags(NetSegment segment, out List<OSMWayTag> wayTags)
        {
            wayTags = new List<OSMWayTag>();

            bool success = false;
            string segmentName = segment.Info.name;

            foreach(RoadContainer road in RoadManager.GetAllRoadTypes())
            {
                if(road.RoadNameMatches(segmentName))
                {
                    wayTags.AddRange(road.tags);
                    success = true;
                }
            }

            if(success)
            {
                wayTags.AddRange(AddExtendedWayTags(segment));
            }

            return success;
        }

        public static List<OSMWayTag> AddExtendedWayTags(NetSegment segment)
        {
            List<OSMWayTag> returnList = new List<OSMWayTag>();

            ushort startNodeId = segment.m_endNode;
            ushort endNodeId = segment.m_startNode;

            NetManager netManager = Singleton<NetManager>.instance;
            NetNode startNode = netManager.m_nodes.m_buffer[startNodeId];
            NetNode endNode = netManager.m_nodes.m_buffer[endNodeId];

            byte wayElevation = (byte)(Mathf.Clamp((startNode.m_elevation + endNode.m_elevation) / 2, 0, 255));
            bool wayUnderground = startNode.m_flags.IsFlagSet(NetNode.Flags.Underground) || endNode.m_flags.IsFlagSet(NetNode.Flags.Underground);

            if (wayUnderground)
            {
                returnList.Add(new OSMWayTag { k = "tunnel", v = "yes" });
                returnList.Add(new OSMWayTag { k = "level", v = (-Mathf.FloorToInt(wayElevation / 12)).ToString() });
                returnList.Add(new OSMWayTag { k = "layer", v = (-Mathf.FloorToInt(wayElevation / 12)).ToString() });
            }
            else if (wayElevation != 0)
            {
                returnList.Add(new OSMWayTag { k = "bridge", v = "yes" });
                returnList.Add(new OSMWayTag { k = "level", v = Mathf.FloorToInt(wayElevation / 12).ToString() });
                returnList.Add(new OSMWayTag { k = "layer", v = Mathf.FloorToInt(wayElevation / 12).ToString() });
            }

            return returnList;
        }

        public static bool CreateBuildingTags(Building building, out List<OSMWayTag> buildingTags)
        {
            buildingTags = new List<OSMWayTag>();

            ItemClass.Service buildingService = building.Info.m_class.m_service;
            ItemClass.SubService buildingSubService = building.Info.m_class.m_subService;

            string buildingClassName = building.Info.m_class.name.ToLower().Replace(" ", "");
            string buildingIngameName = cleanUpName(building.Info.name);

            foreach (BuildingContainer buildingContainer in Managers.BuildingManager.buildings)
            {
                List<OSMWayTag> containerTags = new List<OSMWayTag>();
                bool validComparisons = true;

                foreach (ItemInversionContainer<string> buildingContainerClassName in buildingContainer.buildingClassNames)
                {
                    string lowerClassName = buildingContainerClassName.storedItem_.ToLower().Replace(" ", "");

                    if ((buildingClassName.Contains(lowerClassName) && buildingContainerClassName.validation_ == ItemInversionContainer<string>.ValidationType.None) ||
                        (!buildingClassName.Contains(lowerClassName) && buildingContainerClassName.validation_ == ItemInversionContainer<string>.ValidationType.Inverted))
                    {
                        containerTags.AddRange(buildingContainer.tags);
                    }
                    else
                    {
                        validComparisons = false;
                    }
                }

                foreach (ItemInversionContainer<ItemClass.Service> buildingContainerService in buildingContainer.buildingServices)
                {
                    if (buildingContainerService.storedItem_ == buildingService && buildingContainerService.validation_ == ItemInversionContainer<ItemClass.Service>.ValidationType.None)
                    {
                        containerTags.Clear();
                        containerTags.AddRange(buildingContainer.tags);
                    }
                    else
                    {
                        validComparisons = false;
                    }
                }

                foreach(ItemInversionContainer<ItemClass.SubService> buildingContainerSubService in buildingContainer.buildingSubServices)
                {
                    if (buildingContainerSubService.storedItem_ == buildingSubService && buildingContainerSubService.validation_ == ItemInversionContainer<ItemClass.SubService>.ValidationType.None)
                    {
                        containerTags.Clear();
                        containerTags.AddRange(buildingContainer.tags);
                    }
                    else
                    {
                        validComparisons = false;
                    }
                }

                if(buildingContainer.useName)
                {
                    containerTags.Add(new OSMWayTag() { k = "name", v = buildingIngameName });
                }

                if(validComparisons)
                {
                    if (containerTags.Count > 0)
                    {
                        buildingTags = containerTags;
                    }
                }
            }

            return buildingTags.Count > 0;
        }

        private static string cleanUpName(string name)
        {
            Regex removeSteamworksData = new Regex("(?:[0-9]*\\.)(.*)(?:_Data.*)");
            Regex addSpacingOnUppercase = new Regex("(.+?)([A-Z])");

            name = removeSteamworksData.Replace(name, "$1");
            name = addSpacingOnUppercase.Replace(name, "$1 $2");

            return name;
        }
    }
}
