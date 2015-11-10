using ColossalFramework;
using ColossalFramework.Math;
using Mapper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Mapper.OSM
{
    class OSMExportNew
    {
        private OSM osm = new OSM();
        private List<OSMNode> osmNodes = new List<OSMNode>();
        private List<OSMWay> osmWays = new List<OSMWay>();
        private Vector3 cityCentre = Vector3.zero;
        private int unindexedNodeOffset = 2048000, unindexedWayOffset = 0; //Unindexed, as I don't want to interfere with Cities indexing

        public OSMExportNew()
        {
            osm.version = 1M;
            osm.meta = new OSMMeta { osm_base = DateTime.Now };
            osm.generator = "Cities Skylines Cimtographer Mod";
            osm.note = Singleton<SimulationManager>.instance.m_metaData.m_CityName;
            osm.bounds = new OSMBounds { minlon = 35.753054M, minlat = 34.360353M, maxlon = 35.949310M, maxlat = 34.522050M };

            Translations.SetUpBounds(osm.bounds, 1);
        }

        public void Export()
        {
            osmNodes.Clear();
            osmWays.Clear();

            Debug.Log("Exporting nodes");
            ExportNodes();

            Debug.Log("Exporting ways");
            ExportWays();

            Debug.Log("Exporting buildings");
            ExportBuildings();

            UniqueLogger.PrintLog("Road name matches");
            UniqueLogger.PrintLog("Road names missing from search");

            osm.node = osmNodes.ToArray();
            osm.way = osmWays.ToArray();

            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(OSM));
            StreamWriter writer = new StreamWriter(Singleton<SimulationManager>.instance.m_metaData.m_CityName + ".osm");

            xmlSerialiser.Serialize(writer, osm);
            writer.Close();
        }

        private void ExportNodes()
        {
            NetManager netManager = Singleton<NetManager>.instance;
            NetNode[] netNodes = netManager.m_nodes.m_buffer;
            int nodeId = 0;

            foreach(NetNode netNode in netNodes)
            {
                NetNode.Flags nodeFlags = netNode.m_flags;

                if (nodeFlags.IsFlagSet(NetNode.Flags.Created))
                {
                    OSMNode generatedNode = CreateNode(nodeId, netNode.m_position);

                    if (generatedNode != null)
                    {
                        osmNodes.Add(generatedNode);
                    }
                }

                ++nodeId;
            }
        }

        private void ExportWays()
        {
            NetManager netManager = Singleton<NetManager>.instance;
            NetSegment[] netSegments = netManager.m_segments.m_buffer;

            foreach(NetSegment netSegment in netSegments)
            {
                NetSegment.Flags segmentFlags = netSegment.m_flags;

                if (segmentFlags.IsFlagSet(NetSegment.Flags.Created))
                {
                    OSMWay generatedWay = CreateWay(unindexedWayOffset++, netSegment);

                    if (generatedWay != null)
                    {
                        osmWays.Add(generatedWay);
                    }
                }
            }
        }

        private void ExportBuildings()
        {
            BuildingManager buildingManager = Singleton<BuildingManager>.instance;
            Building[] buildings = buildingManager.m_buildings.m_buffer;

            foreach(Building building in buildings)
            {
                Building.Flags buildingFlags = building.m_flags;

                if(buildingFlags.IsFlagSet(Building.Flags.Created))
                {
                    OSMWay generatedBuilding = CreateBuilding(unindexedWayOffset++, building);

                    if (generatedBuilding != null)
                    {
                        osmWays.Add(generatedBuilding);
                    }
                }
            }
        }

        private OSMNode CreateNode(int index, Vector3 position)
        {
            OSMNode returnNode = null;
            decimal lon, lat;

            Translations.VectorToLonLat(position, out lon, out lat);

            returnNode = new OSMNode { changeset = 50000000, id = (uint)index, version = 1, timestamp = DateTime.Now, user = "Cimtographer", lon = lon, lat = lat };

            return returnNode;
        }

        private OSMWay CreateWay(int index, NetSegment segment)
        {
            OSMWay returnWay = null;
            NetSegment.Flags segmentFlags = segment.m_flags;
            NetManager netManager = Singleton<NetManager>.instance;
            List<OSMWayND> wayPaths = new List<OSMWayND>();
            List<OSMWayTag> wayTags;
            ushort startNodeId = segment.m_startNode, endNodeId = segment.m_endNode;

            if(startNodeId != 0 && endNodeId != 0)
            {
                Vector3 startNodeDirection = segment.m_startDirection;
                Vector3 endNodeDirection = segment.m_endDirection;

                if(segmentFlags.IsFlagSet(NetSegment.Flags.Invert))
                {
                    startNodeId = segment.m_endNode;
                    endNodeId = segment.m_startNode;
                    startNodeDirection = segment.m_endDirection;
                    endNodeDirection = segment.m_startDirection;
                }

                NetNode startNode = netManager.m_nodes.m_buffer[startNodeId];
                NetNode endNode = netManager.m_nodes.m_buffer[endNodeId];
                Vector3 startNodePosition = startNode.m_position;
                Vector3 endNodePosition = endNode.m_position;

                wayPaths.Add(new OSMWayND { @ref = startNodeId });

                if (Vector3.Angle(startNodeDirection, -endNodeDirection) > 3f)
                {
                    Vector3 midPointA = Vector3.zero, midPointB = Vector3.zero;
                    NetSegment.CalculateMiddlePoints(startNodePosition, startNodeDirection, endNodePosition, endNodeDirection, false, false, out midPointA, out midPointB);
                    Bezier3 bezier = new Bezier3(startNodePosition, midPointA, midPointB, endNodePosition);

                    osmNodes.Add(CreateNode(unindexedNodeOffset++, bezier.Position(0.25f)));
                    osmNodes.Add(CreateNode(unindexedNodeOffset++, bezier.Position(0.5f)));
                    osmNodes.Add(CreateNode(unindexedNodeOffset++, bezier.Position(0.75f)));

                    wayPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 3 });
                    wayPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 2 });
                    wayPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 1 });
                }

                wayPaths.Add(new OSMWayND { @ref = endNodeId });

                if(Tagger.CreateWayTags(segment, out wayTags))
                {
                    returnWay = new OSMWay { changeset = 50000000, id = (uint)index, timestamp = DateTime.Now, user = "Cimtographer", nd = wayPaths.ToArray(), tag = wayTags.ToArray() };
                }
                else
                {
                    UniqueLogger.AddLog("Road names missing from search", segment.Info.name, "");
                }
            }

            return returnWay;
        }

        private OSMWay CreateBuilding(int index, Building building)
        {
            OSMWay returnWay = null;
            List<OSMWayND> buildingPaths = new List<OSMWayND>();
            List<OSMWayTag> buildingTags = new List<OSMWayTag>();

            if (Tagger.CreateBuildingTags(building, out buildingTags))
            {
                int buildingWidth = building.Width;
                int BuildingLength = building.Length;
                Vector3 buildingCentre = building.m_position;
                Vector3 directionRight = new Vector3(Mathf.Cos(building.m_angle), 0f, Mathf.Sin(building.m_angle)) * 8f;
                Vector3 directionUp = new Vector3(directionRight.z, 0f, -directionRight.x);

                cityCentre += buildingCentre; //Gets averaged out later, for finding centre of city based on buildings

                osmNodes.Add(CreateNode(unindexedNodeOffset++, buildingCentre - (((float)buildingWidth / 2f) * directionRight) - (((float)BuildingLength / 2f) * directionUp)));
                osmNodes.Add(CreateNode(unindexedNodeOffset++, buildingCentre + (((float)buildingWidth / 2f) * directionRight) - (((float)BuildingLength / 2f) * directionUp)));
                osmNodes.Add(CreateNode(unindexedNodeOffset++, buildingCentre + (((float)buildingWidth / 2f) * directionRight) + (((float)BuildingLength / 2f) * directionUp)));
                osmNodes.Add(CreateNode(unindexedNodeOffset++, buildingCentre - (((float)buildingWidth / 2f) * directionRight) + (((float)BuildingLength / 2f) * directionUp)));

                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 4 });
                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 3 });
                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 2 });
                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 1 });
                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 4 });

                returnWay = new OSMWay { changeset = 50000000, id = (uint)index, timestamp = DateTime.Now, user = "Cimtographer", nd = buildingPaths.ToArray(), tag = buildingTags.ToArray() };
            }

            return returnWay;
        }
    }
}
