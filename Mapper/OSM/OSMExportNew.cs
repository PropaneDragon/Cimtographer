using ColossalFramework;
using ColossalFramework.Math;
using Mapper.Contours;
using Mapper.Managers;
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
        private enum ContourType { Water, Ground };

        private OSM osm = new OSM();
        private List<OSMNode> osmNodes = new List<OSMNode>();
        private List<OSMWay> osmWays = new List<OSMWay>();
        private Vector3 cityCentre = Vector3.zero;
        private int unindexedNodeOffset = 2048000, unindexedWayOffset = 1; //Unindexed, as I don't want to interfere with Cities indexing
        private bool haveRoadNamerMod = false;

        public OSMExportNew()
        {
            osm.version = 0.6M;
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

            haveRoadNamerMod = RoadNamerManager.Instance().HaveMod();

            //Debug.Log("Exporting nodes");
            ExportNodes();

            //Debug.Log("Exporting ways");
            ExportWays();

            //Debug.Log("Exporting buildings");
            ExportBuildings();

            //Debug.Log("Exporting ground and water");
            ExportGroundAndWater();

            //Debug.Log("Exporting routes");
            ExportRoutes();

            //Debug.Log("Exporting districts");
            ExportDistricts();

            //UniqueLogger.PrintLog("Road name matches");
            UniqueLogger.PrintLog("Road names missing from search");
            UniqueLogger.PrintLog("Building names missing from search");

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
            for(int segmentId = 0; segmentId<netSegments.Length; segmentId++)
            {
                NetSegment netSegment = netSegments[segmentId];
                NetSegment.Flags segmentFlags = netSegment.m_flags;

                if (segmentFlags.IsFlagSet(NetSegment.Flags.Created))
                {
                    OSMWay generatedWay = CreateWay(unindexedWayOffset++, netSegment, (ushort)segmentId);

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

        private void ExportGroundAndWater()
        {
            /* I'll be honest, I tried to make sense of this bit but it went right
            ** over my head, so I just re-implemented it as it was and fixed it up
            ** so it took water levels into account, so it's a bit of a mess still.
            ** Sorry about that. If someone wants to help make it readable then be
            ** my guest!*/
            TerrainManager terrainManager = Singleton<TerrainManager>.instance;
            int gridSize = 16;
            int steps = (1920 * 9) / gridSize; //??????????

            double[,] waterPoints = new double[steps + 2, steps + 2]; //Why does it add 2? I have no idea
            double[,] groundPoints = new double[steps + 2, steps + 2];
            double[] contourX = new double[steps + 2], contourY = new double[steps + 2], contourZ = new double[] { 1.6 };

            for(int y = 0; y < steps + 2; ++y)
            {
                for(int x = 0; x < steps + 2; ++x)
                {
                    if(y == 0 || y == steps + 1 || x == 0 || x == steps + 1) //*shrug*
                    {
                        waterPoints[y, x] = 0.0F; //*more shrug*
                        groundPoints[y, x] = 0.0F;
                    }
                    else
                    {
                        Vector3 position = new Vector3(((y - 1) - (steps / 2)) * gridSize, 0, ((x - 1) - (steps / 2)) * gridSize); //What?

                        float waterLevel = terrainManager.SampleRawHeightSmoothWithWater(position, false, 0f);
                        float groundLevel = terrainManager.SampleRawHeightSmooth(position);
                        float difference = waterLevel - groundLevel;

                        UniqueLogger.AddLog("Levels", Math.Round(waterLevel).ToString() + " - " + Math.Round(groundLevel).ToString(), "");

                        if (difference < 5.0F)
                        {
                            waterPoints[y, x] = difference;
                            groundPoints[y, x] = groundLevel / 12; //Divided it, as it makes the points more rounded
                        }
                        else
                        {
                            waterPoints[y, x] = (double)difference;
                            groundPoints[y, x] = 0.0F;
                        }
                    }
                }

                contourX[y] = y * gridSize;
                contourY[y] = y * gridSize; //Still no idea...
            }

            //UniqueLogger.PrintLog("Levels");

            CreateContours(waterPoints, contourX, contourY, contourZ, gridSize, steps, ContourType.Water);
            CreateContours(groundPoints, contourX, contourY, contourZ, gridSize, steps, ContourType.Ground);
        }

        private void ExportRoutes()
        {
            TransportManager transportManager = Singleton<TransportManager>.instance;
            TransportLine[] transportLines = transportManager.m_lines.m_buffer;

            foreach (TransportLine line in transportLines)
            {
                TransportLine.Flags lineFlags = line.m_flags;

                if (lineFlags.IsFlagSet(TransportLine.Flags.Created))
                {
                    List<OSMNode> generatedLines = CreateTransportLine(unindexedNodeOffset++, line);

                    if (generatedLines != null)
                    {
                        osmNodes.AddRange(generatedLines);
                    }
                }
            }
        }

        private void ExportDistricts()
        {
            if (MapperOptionsManager.OptionChecked("districts", MapperOptionsManager.exportOptions))
            {
                DistrictManager districtManager = Singleton<DistrictManager>.instance;
                District[] districts = districtManager.m_districts.m_buffer;
                int districtId = 0;

                foreach (District district in districts)
                {
                    District.Flags districtFlags = district.m_flags;

                    if (districtFlags.IsFlagSet(District.Flags.Created))
                    {
                        OSMNode generatedNode = CreateDistrict(districtId, district);

                        if (generatedNode != null)
                        {
                            osmNodes.Add(generatedNode);
                        }
                    }

                    ++districtId;
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

        private OSMWay CreateWay(int index, NetSegment segment, ushort segmentId)
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
                    if (haveRoadNamerMod)
                    {
                        // If road namer mod is available, then attempt to get the name asscociated with the segment, if any
                        string roadName = RoadNamerManager.Instance().getSegmentName((ushort)(segmentId));
                        if (roadName != null)
                        {
                            //If a name is available, add a name tag
                            wayTags.Add(new OSMWayTag { k = "name", v = StringUtilities.RemoveTags(roadName) });
                        }
                    }
                    returnWay = new OSMWay { changeset = 50000000, id = (uint)index, timestamp = DateTime.Now, user = "Cimtographer", nd = wayPaths.ToArray(), tag = wayTags.ToArray(), version = 1 };
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
                buildingPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 4 }); //Need to connect back to the first point

                returnWay = new OSMWay { changeset = 50000000, id = (uint)index, timestamp = DateTime.Now, user = "Cimtographer", nd = buildingPaths.ToArray(), tag = buildingTags.ToArray(), version = 1 };
            }

            return returnWay;
        }

        private void CreateContours(double[,] points, double[] contourX, double[] contourY, double[] contourZ, int gridSize, int steps, ContourType contourType)
        {
            Dictionary<Vector2, List<Vector2>>[] contours = new Dictionary<Vector2, List<Vector2>>[contourZ.Length];

            for (int heightCount = 0; heightCount < contourZ.Length; ++heightCount) //Loop through the different Z height segments
            {
                contours[heightCount] = new Dictionary<Vector2, List<Vector2>>();
            }

            Conrec.Contour(points, contourX, contourY, contourZ, contours); //Get contours for the points. Returns an array of contours for the different heights.

            foreach (Dictionary<Vector2, List<Vector2>> contourList in contours)
            {
                List<List<Vector2>> contourChains = Chains.Process(contourList);
                contourChains = Chains.Simplify(contourChains);

                foreach (List<Vector2> chains in contourChains)
                {
                    List<OSMWayND> wayPaths = new List<OSMWayND>();
                    List<OSMWayTag> wayTags = new List<OSMWayTag>();

                    foreach (Vector2 node in chains)
                    {
                        osmNodes.Add(CreateNode(unindexedNodeOffset++, new Vector3((node.x - gridSize) - ((steps * gridSize) / 2), 0, (node.y - gridSize) - ((steps * gridSize) / 2))));
                        wayPaths.Add(new OSMWayND { @ref = (uint)unindexedNodeOffset - 1 });
                    }

                    wayPaths.Add(new OSMWayND { @ref = (uint)(unindexedNodeOffset - chains.Count) }); //Back to the first chain

                    switch(contourType)
                    {
                        case ContourType.Ground:
                            wayTags.Add(new OSMWayTag { k = "natural", v = "coastline" });
                            break;
                        case ContourType.Water:
                            wayTags.Add(new OSMWayTag { k = "natural", v = "water" });
                        break;
                    }

                    osmWays.Add(new OSMWay { changeset = 50000000, id = (uint)unindexedWayOffset++, timestamp = DateTime.Now, user = "Cimtographer", nd = wayPaths.ToArray(), tag = wayTags.ToArray(), version = 1 });
                }
            }
        }

        private List<OSMNode> CreateTransportLine(int index, TransportLine line)
        {
            List<OSMNode> returnNodes = new List<OSMNode>();
            TransportManager transportManager = Singleton<TransportManager>.instance;

            int numberOfStops = line.CountStops(line.m_stops);
            var transportType = line.Info.m_transportType;

            if (line.m_stops != 0 && numberOfStops > 0)
            {
                for (int stopIndex = 0; stopIndex < numberOfStops; ++stopIndex)
                {
                    ushort stopId = line.GetStop(stopIndex);
                    NetNode stop = Singleton<NetManager>.instance.m_nodes.m_buffer[(int)stopId];
                    Vector3 position = stop.m_position;
                    ushort transportLine = stop.m_transportLine;
                    string transportLineName = transportLineName = transportManager.GetLineName(transportLine); ;
                    decimal lon, lat;

                    Translations.VectorToLonLat(position, out lon, out lat);

                    List<OSMNodeTag> tags = new List<OSMNodeTag>();

                    if (transportType == TransportInfo.TransportType.Bus)
                    {
                        tags.Add(new OSMNodeTag { k = "highway", v = "bus_stop" });
                    }
                    else if (transportType == TransportInfo.TransportType.Train)
                    {
                        bool tramLine = transportLineName != null && (transportLineName.Contains("[t]") || transportLineName.ToLower().Contains("tram"));
                        tags.Add(new OSMNodeTag { k = "public_transport", v = "platform" });
                        tags.Add(new OSMNodeTag { k = "railway", v = tramLine ? "tram_stop" : "station" });
                    }

                    returnNodes.Add(new OSMNode { changeset = 50000000, id = (uint)unindexedNodeOffset++, version = 1, timestamp = DateTime.Now, user = "CS", lon = lon, lat = lat, tag = tags.ToArray() });
                }
            }

            return returnNodes;
        }

        private OSMNode CreateDistrict(int index, District district)
        {
            OSMNode returnNode = null;
            DistrictManager districtManager = Singleton<DistrictManager>.instance;
            string districtName = districtManager.GetDistrictName(index);

            if (districtName != "")
            {
                Vector3 position = district.m_nameLocation;
                List<OSMNodeTag> tags = new List<OSMNodeTag>();

                returnNode = CreateNode(unindexedNodeOffset++, position);

                tags.Add(new OSMNodeTag() { k = "name", v = districtName });
                tags.Add(new OSMNodeTag() { k = "place", v = "suburb" });

                returnNode.tag = tags.ToArray();
            }

            return returnNode;
        }
    }
}
