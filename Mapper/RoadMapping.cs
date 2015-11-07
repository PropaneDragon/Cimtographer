using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Mapper.OSM;
using System.Text.RegularExpressions;

namespace Mapper
{
    public enum RoadTypes
    {
        None,
        ZonablePedestrianGravel,
        ZonablePedestrianPavement,
        ZonablePedestrianElevated,
        PedestrianConnection,
        PedestrianConnectionInside,
        PedestrianConnectionSurface,
        PedestrianGravel,
        PedestrianPavement,
        PedestrianPavementBicycle,
        PedestrianPavementBicycleElevated,
        PedestrianSlope,
        PedestrianSlopeBicycle,
        PedestrianTunnel,
        PedestrianTunnelBicycle,
        PedestrianElevatedBicycle,
        PedestrianElevated,
        
        BusLine,
        MetroLine,
        TrainLine,
        TrainCargoTrack,
        TrainCargoTrackTunnel,
        TrainCargoTrackElevated,

        BasicRoad,
        BasicRoadDecorationTrees,
        BasicRoadDecorationGrass,
        BasicRoadBridge,
        BasicRoadElevated,
        BasicRoadElevatedBike,
        BasicRoadTunnel,
        BasicRoadSlope,
        HarborRoad,
        SmallBusway,
        SmallBuswayDecorationGrass,
        SmallBuswayDecorationTrees,
        SmallBuswayElevated,
        SmallBuswayOneWay,
        SmallBuswayOneWayDecorationGrass,
        SmallBuswayOneWayDecorationTrees,
        SmallBuswayOneWayElevated,
        SmallBuswayOneWaySlope,
        SmallBuswayOneWayTunnel,
        SmallBuswayOneWayBridge,
        SmallBuswaySlope,
        SmallBuswayTunnel,
        SmallBuswayBridge,
        SmallAvenue,
        SmallRuralHighway,
        OnewayRoad,
        OnewayRoadDecorationTrees,
        OnewayRoadDecorationGrass,
        OnewayRoadElevated,
        OnewayRoadBridge,
        OnewayRoadSlope,
        OnewayRoadTunnel,
        Oneway3L,
        Oneway4L,
        LargeOneway,
        LargeOnewayDecorationGrass,
        LargeOnewayDecorationTrees,
        LargeOnewayBridge,
        LargeOnewayElevated,
        LargeOnewayRoadSlope,
        MediumRoad,
        MediumRoadBicycle,
        MediumRoadBus,
        MediumRoadElevated,
        MediumRoadElevatedBike,
        MediumRoadElevatedBus,
        MediumRoadSlope,
        MediumRoadSlopeBike,
        MediumRoadSlopeBus,
        MediumRoadDecorationGrass,
        MediumRoadDecorationTrees,
        MediumRoadBridge,
        MediumRoadTunnel,
        MediumRoadTunnelBike,
        MediumRoadTunnelBus,
        MediumAvenue,
        MediumAvenueTL,
        LargeRoad,
        LargeRoadBicycle,
        LargeRoadBridge,
        LargeRoadBus,
        LargeRoadDecorationGrass,
        LargeRoadDecorationGrassWithBusLanes,
        LargeRoadDecorationTrees,
        LargeRoadDecorationTreesWithBusLanes,
        LargeRoadElevated,
        LargeRoadElevatedBike,
        LargeRoadElevatedBus,
        LargeRoadElevatedWithBusLanes,
        LargeRoadSlope,
        LargeRoadSlopeBus,
        LargeRoadSlopeWithBusLanes,
        LargeRoadTunnel,
        LargeRoadTunnelBus,
        LargeRoadTunnelWithBusLanes,
        LargeRoadWithBusLanes,
        LargeHighway,
        LargeHighwayElevated,
        LargeHighwaySlope,
        LargeHighwayTunnel,
        GravelRoad,

        TrainTrack,
        MetroTrack,
        MetroStationTrack,
        TrainStationTrack,
        TrainTrackBridge,
        TrainTrackElevated,
        TrainTrackSlope,
        TrainTrackTunnel,
        OnewayTrainTrack,
        OnewayTrainTrackElevated,
        OnewayTrainTrackSlope,
        OnewayTrainTrackTunnel,
        OnewayTrainTrackBridge,
        StationTrackEleva,
        StationTrackSunken,
        TrainConnectionTrack,
        RuralHighway,
        RuralHighwayElevated,
        RuralHighwaySlope,
        RuralHighwayTunnel,
        Highway,
        HighwayBridge,
        HighwayElevated,
        HighwayRamp,
        HighwayRampSlope,
        HighwayRampElevated,
        HighwayRampTunnel,
        HighwaySlope,
        HighwayTunnel,
        HighwayBarrier,

        AirplaneTaxiway,
        AirplaneRunway,
        Dam,
    }


    public class RoadMapping
    {
        public const int GameSizeMetres = 18000;
        public const int GameSizeGameCoordinates = 1920 * 9;
        double maxBounds;
        private Dictionary<KeyValuePair<string, string>, RoadTypes> roadTypeMapping = new Dictionary<KeyValuePair<string, string>, RoadTypes>();
        private Dictionary<RoadTypes, KeyValuePair<string, string>> reverseMapping = new Dictionary<RoadTypes,KeyValuePair<string, string>>();
        private Dictionary<string, bool> pavedMapping = new Dictionary<string, bool>();
        private Dictionary<string, int> nameList = new Dictionary<string, int>(), elevationList = new Dictionary<string, int>(), undergroundList = new Dictionary<string, int>();
        private Dictionary<string, int> servicesList = new Dictionary<string, int>(), classNameList = new Dictionary<string, int>(), subServicesList = new Dictionary<string, int>();

        //private Vector2 startLatLon = new Vector2(float.MaxValue, float.MaxValue);
        private Vector2 middleLatLon = new Vector2(float.MinValue, float.MinValue);
        //private Vector2 endLatLon;
        double scaleX;
        double scaleY;

        public RoadMapping(double tiles)
        {
            maxBounds = tiles * 1920;
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "motorway"), RoadTypes.Highway);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "trunk"), RoadTypes.LargeRoadDecorationGrass);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "primary"), RoadTypes.MediumRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "secondary"), RoadTypes.MediumRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "tertiary"), RoadTypes.MediumRoadDecorationGrass);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "unclassified"), RoadTypes.BasicRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "bus_guideway"), RoadTypes.SmallBusway);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "road"), RoadTypes.BasicRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "residential"), RoadTypes.BasicRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "service"), RoadTypes.GravelRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "living_street"), RoadTypes.GravelRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "track"), RoadTypes.GravelRoad);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "motorway_link"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "trunk_link"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "primary_link"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "secondary_link"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "tertiary_link"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "raceway"), RoadTypes.HighwayRamp);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "pedestrian"), RoadTypes.PedestrianPavement);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "footway"), RoadTypes.PedestrianPavement);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "steps"), RoadTypes.PedestrianPavement);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "bridleway"), RoadTypes.PedestrianPavement);
            roadTypeMapping.Add(new KeyValuePair<string, string>("highway", "cycleway"), RoadTypes.PedestrianPavement);

            roadTypeMapping.Add(new KeyValuePair<string, string>("railway", "miniature"), RoadTypes.TrainTrack);
            roadTypeMapping.Add(new KeyValuePair<string, string>("railway", "monorail"), RoadTypes.TrainTrack);
            roadTypeMapping.Add(new KeyValuePair<string, string>("railway", "narrow_gauge"), RoadTypes.TrainTrack);
            roadTypeMapping.Add(new KeyValuePair<string, string>("railway", "preserved"), RoadTypes.TrainTrack);
            roadTypeMapping.Add(new KeyValuePair<string, string>("railway", "rail"), RoadTypes.TrainTrack);

            //reverseMapping.Add(RoadTypes.TrainLine, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainTrack, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainTrackElevated, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainTrackSlope, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainTrackTunnel, new KeyValuePair<string, string>("railway", "rail"));
            //reverseMapping.Add(RoadTypes.TrainConnectionTrack, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainTrackBridge, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainCargoTrack, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainCargoTrackElevated, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.TrainStationTrack, new KeyValuePair<string, string>("railway", "platform"));
            reverseMapping.Add(RoadTypes.StationTrackEleva, new KeyValuePair<string, string>("railway", "platform"));
            reverseMapping.Add(RoadTypes.StationTrackSunken, new KeyValuePair<string, string>("railway", "platform"));

            reverseMapping.Add(RoadTypes.MetroTrack, new KeyValuePair<string, string>("railway", "subway"));
            reverseMapping.Add(RoadTypes.MetroStationTrack, new KeyValuePair<string, string>("railway", "subway"));

            reverseMapping.Add(RoadTypes.OnewayTrainTrack, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.OnewayTrainTrackElevated, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.OnewayTrainTrackSlope, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.OnewayTrainTrackTunnel, new KeyValuePair<string, string>("railway", "rail"));
            reverseMapping.Add(RoadTypes.OnewayTrainTrackBridge, new KeyValuePair<string, string>("railway", "rail"));

            reverseMapping.Add(RoadTypes.PedestrianElevated, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.PedestrianElevatedBicycle, new KeyValuePair<string, string>("highway", "cycleway"));
            reverseMapping.Add(RoadTypes.PedestrianGravel, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.PedestrianPavement, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.PedestrianPavementBicycle, new KeyValuePair<string, string>("highway", "cycleway"));
            reverseMapping.Add(RoadTypes.PedestrianSlope, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.PedestrianSlopeBicycle, new KeyValuePair<string, string>("highway", "cycleway"));
            reverseMapping.Add(RoadTypes.PedestrianTunnel, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.PedestrianTunnelBicycle, new KeyValuePair<string, string>("highway", "cycleway"));
            reverseMapping.Add(RoadTypes.ZonablePedestrianPavement, new KeyValuePair<string, string>("highway", "pedestrian"));
            reverseMapping.Add(RoadTypes.ZonablePedestrianElevated, new KeyValuePair<string, string>("highway", "pedestrian"));

            reverseMapping.Add(RoadTypes.HighwayRamp, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.HighwayRampSlope, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.HighwayRampTunnel, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.HighwayRampElevated, new KeyValuePair<string, string>("highway", "secondary_link"));

            reverseMapping.Add(RoadTypes.RuralHighway, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.RuralHighwayElevated, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.RuralHighwaySlope, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.RuralHighwayTunnel, new KeyValuePair<string, string>("highway", "secondary_link"));
            reverseMapping.Add(RoadTypes.SmallRuralHighway, new KeyValuePair<string, string>("highway", "secondary_link"));

            reverseMapping.Add(RoadTypes.Highway, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.HighwayElevated, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.HighwaySlope, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.HighwayTunnel, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.HighwayBarrier, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.HighwayBridge, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.LargeHighway, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.LargeHighwayElevated, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.LargeHighwaySlope, new KeyValuePair<string, string>("highway", "motorway"));
            reverseMapping.Add(RoadTypes.LargeHighwayTunnel, new KeyValuePair<string, string>("highway", "motorway"));

            reverseMapping.Add(RoadTypes.GravelRoad, new KeyValuePair<string, string>("highway", "track"));
            reverseMapping.Add(RoadTypes.ZonablePedestrianGravel, new KeyValuePair<string, string>("highway", "track"));

            reverseMapping.Add(RoadTypes.BasicRoad, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadDecorationTrees, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadDecorationGrass, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadElevated, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadElevatedBike, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadSlope, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadTunnel, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.BasicRoadBridge, new KeyValuePair<string, string>("highway", "residential"));
            reverseMapping.Add(RoadTypes.HarborRoad, new KeyValuePair<string, string>("highway", "residential"));

            reverseMapping.Add(RoadTypes.OnewayRoad, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadElevated, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadSlope, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadTunnel, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadBridge, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadDecorationGrass, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.OnewayRoadDecorationTrees, new KeyValuePair<string, string>("highway", "road"));

            reverseMapping.Add(RoadTypes.Oneway3L, new KeyValuePair<string, string>("highway", "road"));
            reverseMapping.Add(RoadTypes.Oneway4L, new KeyValuePair<string, string>("highway", "road"));

            reverseMapping.Add(RoadTypes.SmallBusway, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayDecorationGrass, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayDecorationTrees, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayElevated, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWay, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWayDecorationGrass, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWayDecorationTrees, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWayElevated, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWaySlope, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWayTunnel, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayOneWayBridge, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswaySlope, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayTunnel, new KeyValuePair<string, string>("highway", "bus_guideway"));
            reverseMapping.Add(RoadTypes.SmallBuswayBridge, new KeyValuePair<string, string>("highway", "bus_guideway"));

            reverseMapping.Add(RoadTypes.MediumRoad, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadBicycle, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadBus, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadElevated, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadElevatedBike, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadElevatedBus, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadSlope, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadSlopeBike, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadSlopeBus, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadTunnel, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadTunnelBike, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadTunnelBus, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadBridge, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadDecorationGrass, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumRoadDecorationTrees, new KeyValuePair<string, string>("highway", "tertiary"));

            reverseMapping.Add(RoadTypes.SmallAvenue, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumAvenue, new KeyValuePair<string, string>("highway", "tertiary"));
            reverseMapping.Add(RoadTypes.MediumAvenueTL, new KeyValuePair<string, string>("highway", "tertiary"));

            reverseMapping.Add(RoadTypes.LargeRoad, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadBicycle, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadBus, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadDecorationGrass, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadDecorationGrassWithBusLanes, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadDecorationTrees, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadDecorationTreesWithBusLanes, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadElevated, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadElevatedBike, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadElevatedBus, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadElevatedWithBusLanes, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadSlope, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadSlopeBus, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadSlopeWithBusLanes, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadTunnel, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadTunnelBus, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadTunnelWithBusLanes, new KeyValuePair<string, string>("highway", "secondary"));
            reverseMapping.Add(RoadTypes.LargeRoadBridge, new KeyValuePair<string, string>("highway", "secondary"));

            reverseMapping.Add(RoadTypes.LargeOneway, new KeyValuePair<string, string>("highway", "trunk"));
            reverseMapping.Add(RoadTypes.LargeOnewayElevated, new KeyValuePair<string, string>("highway", "trunk"));
            reverseMapping.Add(RoadTypes.LargeOnewayRoadSlope, new KeyValuePair<string, string>("highway", "trunk"));
            reverseMapping.Add(RoadTypes.LargeOnewayBridge, new KeyValuePair<string, string>("highway", "trunk"));
            reverseMapping.Add(RoadTypes.LargeOnewayDecorationGrass, new KeyValuePair<string, string>("highway", "trunk"));
            reverseMapping.Add(RoadTypes.LargeOnewayDecorationTrees, new KeyValuePair<string, string>("highway", "trunk"));

            reverseMapping.Add(RoadTypes.AirplaneTaxiway, new KeyValuePair<string, string>("aeroway", "taxiway"));
            reverseMapping.Add(RoadTypes.AirplaneRunway, new KeyValuePair<string, string>("aeroway", "runway"));
            reverseMapping.Add(RoadTypes.Dam, new KeyValuePair<string, string>("waterway", "dam"));

            pavedMapping.Add("paved",true);
            pavedMapping.Add("asphalt", true);
            pavedMapping.Add("cobblestone", true);
            pavedMapping.Add("cobblestone:flattened", true);
            pavedMapping.Add("concrete", true);
            pavedMapping.Add("concrete:lanes", true);
            pavedMapping.Add("concrete:plates", true);
            pavedMapping.Add("paving_stones", true);
            pavedMapping.Add("metal", true);

            pavedMapping.Add("wood", false);
            pavedMapping.Add("unpaved", false);
            pavedMapping.Add("compacted", false);
            pavedMapping.Add("dirt", false);
            pavedMapping.Add("earth", false);
            pavedMapping.Add("fine_gravel", false);
            pavedMapping.Add("grass", false);
            pavedMapping.Add("gravel", false);
            pavedMapping.Add("ground", false);
            pavedMapping.Add("pebblestone", false);
            pavedMapping.Add("salt", false);
            pavedMapping.Add("sand", false);
        }


        public bool Mapped(osmWay way, ref List<uint> points, ref RoadTypes rt, ref int layer)
        {
            if (way.tag == null || way.nd == null || way.nd.Count() < 2)
            {
                return false;
            }
            rt = RoadTypes.None;
            bool oneWay = false;
            bool invert = false;
            var surface = "";

            foreach (var tag in way.tag)
            {
                if (tag.k.Trim().ToLower() == "oneway")
                {
                    oneWay = true;
                    if (tag.v.Trim() == "-1")
                    {
                        invert = true;
                    }
                }
                else if(tag.k.Trim().ToLower() =="bridge"){
                    layer = Math.Max(layer, 1);
                }
                else if (tag.k.Trim().ToLower() == "layer")
                {
                    int.TryParse(tag.v.Trim(), out layer);
                }
                else if (tag.k.Trim().ToLower() == "surface")
                {
                    surface = tag.v.Trim().ToLower();                    
                }
                else
                {
                    var kvp = new KeyValuePair<string, string>(tag.k.Trim(), tag.v.Trim());
                    if (roadTypeMapping.ContainsKey(kvp))
                    {
                        rt = roadTypeMapping[kvp];
                    }
                }
            }
            if (oneWay)
            {
                rt = GetOneway(rt);
            }

            if (rt != RoadTypes.None)
            {
                if (surface != "")
                {
                    rt = CheckSurface(rt, surface);
                }

                points = new List<uint>();
                if (invert)
                {
                    for (var i = way.nd.Count() - 1; i >= 0; i -=1 )
                    {
                        points.Add(way.nd[i].@ref);
                    }
                }
                else
                {
                    foreach (var nd in way.nd)
                    {
                        points.Add(nd.@ref);
                    }
                }
                return true;
            }
            return false;
        }


        internal bool GetTags(byte elevation, NetSegment netSegment, List<osmWayTag> tags, bool underground)
        {
            if (nameList.ContainsKey(netSegment.Info.name))
                nameList[netSegment.Info.name]++;
            else
                nameList[netSegment.Info.name] = 1;

            if (elevationList.ContainsKey(elevation.ToString()))
                elevationList[elevation.ToString()]++;
            else
                elevationList[elevation.ToString()] = 1;

            if (elevation > 0)
            {
                tags.Add(new osmWayTag { k = "bridge", v = "yes" });
                tags.Add(new osmWayTag { k = "level", v = Mathf.FloorToInt(elevation / 12).ToString() });
                tags.Add(new osmWayTag { k = "layer", v = Mathf.FloorToInt(elevation / 12).ToString() });
            }

            if(underground)
            {
                tags.Add(new osmWayTag { k = "tunnel", v = "yes" });
                tags.Add(new osmWayTag { k = "level", v = (-Mathf.FloorToInt(elevation / 12)).ToString() });
                tags.Add(new osmWayTag { k = "layer", v = (-Mathf.FloorToInt(elevation / 12)).ToString() });
            }

            var name = netSegment.Info.name.Replace(" ","");
            if (name.ToLower().Contains("oneway") || name.ToLower().Contains("highway"))
            {
                tags.Add(new osmWayTag { k = "oneway", v = "yes" });
            }

            if(name.ToLower().Contains("metro"))
            {
                tags.Add(new osmWayTag { k = "tunnel", v = "yes" });
                tags.Add(new osmWayTag { k = "level", v = "-1" });
                tags.Add(new osmWayTag { k = "layer", v = "-1" });
            }

            if(name.ToLower().Contains("trackeleva"))
            {
                tags.Add(new osmWayTag { k = "bridge", v = "yes" });
                tags.Add(new osmWayTag { k = "level", v = "1" });
                tags.Add(new osmWayTag { k = "layer", v = "1" });
            }

            var roadType = RoadTypes.None;

            try
            {                
                roadType = (RoadTypes)Enum.Parse(typeof(RoadTypes), name);
            }
            catch 
            {
                return false;
            }
            if (reverseMapping.ContainsKey(roadType))
            {
                tags.Add(new osmWayTag { k = reverseMapping[roadType].Key, v = reverseMapping[roadType].Value });
                return true;
            }
            return false;
        }

        private RoadTypes CheckSurface(RoadTypes rt, string surface)
        {
            if (pavedMapping.ContainsKey(surface))
            {
                if (pavedMapping[surface]){
                    if (rt == RoadTypes.GravelRoad){                        
                        return RoadTypes.BasicRoad;
                    }
                    else if (rt == RoadTypes.PedestrianGravel)
                    {
                        
                        return RoadTypes.PedestrianPavement;
                    }                    
                }
                else
                {
                    if (rt == RoadTypes.PedestrianPavement || rt == RoadTypes.PedestrianGravel)
                    {
                        return RoadTypes.PedestrianGravel;
                    }
                    else
                    {
                        return RoadTypes.GravelRoad;
                    }
                }
            }
            return rt;
        }

        private RoadTypes GetOneway(RoadTypes rt)
        {
            switch (rt)
            {
                case RoadTypes.BasicRoad:
                case RoadTypes.MediumRoad:
                    return RoadTypes.OnewayRoad;
                case RoadTypes.BasicRoadDecorationTrees:
                case RoadTypes.MediumRoadDecorationTrees:
                    return RoadTypes.OnewayRoadDecorationTrees;
                case RoadTypes.MediumRoadDecorationGrass:
                case RoadTypes.BasicRoadDecorationGrass:
                    return RoadTypes.OnewayRoadDecorationGrass;
                case RoadTypes.LargeRoad:                    
                case RoadTypes.LargeRoadDecorationGrass:
                case RoadTypes.LargeRoadDecorationTrees:
                case RoadTypes.Highway:
                    return RoadTypes.Highway;
                case RoadTypes.GravelRoad:
                    return RoadTypes.OnewayRoad;
                case RoadTypes.HighwayRamp:
                    return RoadTypes.HighwayRamp;
                case RoadTypes.LargeOneway:
                    return RoadTypes.LargeOneway;
            }
            return RoadTypes.None;
        }

        //public void InitBoundingBox(osmNode node)
        //{
        //    startLatLon = new Vector2(Math.Min(startLatLon.x, (float)node.lon), Math.Min(startLatLon.y, (float)node.lat));
        //    endLatLon = new Vector2(Math.Max(endLatLon.x, (float)node.lon), Math.Max(endLatLon.y, (float)node.lat));
        //}

        public void InitBoundingBox(osmBounds bounds,double scale)
        {

            this.middleLatLon = new Vector2((float)(bounds.minlon + bounds.maxlon) / 2f, (float)(bounds.minlat + bounds.maxlat) / 2f);
            var lat = Deg2rad(this.middleLatLon.y);
            var radius = WGS84EarthRadius(lat);
            var pradius = radius * Math.Cos(lat);
            scaleX = scale * GameSizeGameCoordinates / Rad2deg(GameSizeMetres / pradius);
            scaleY = scale * GameSizeGameCoordinates / Rad2deg(GameSizeMetres / radius);

        }

        public void MapCoordinates(osmNode node)
        {
            if (node != null)
            {
                Vector2 pos = Vector2.zero;
                GetPos(node.lon, node.lat, ref pos);
            }
        }

        public bool GetPos(decimal lon, decimal lat, ref Vector2 pos)
        {            
            pos = new Vector2((float)(((float)lon - middleLatLon.x) * scaleX), (float)(((float)lat - middleLatLon.y) * scaleY));

            if (Math.Abs(pos.x) > maxBounds || Math.Abs(pos.y) > maxBounds)
            {
                return false;
            }

            //pos += new Vector2(1920f * 0.5f, 1920f * -0.5f);
            return true;
        }

        internal void GetPos(Vector3 vector3, out decimal lon, out decimal lat)
        {
            lon = (decimal)((vector3.x / scaleX) + middleLatLon.x);
            lat = (decimal)((vector3.z / scaleY) + middleLatLon.y);
        }

        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]

        private static double Deg2rad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }


        private static double Rad2deg(double radians)
        {
            return 180.0 * radians / Math.PI;
        }

        private static double WGS84EarthRadius(double lat)
        {
            var An = WGS84_a * WGS84_a * Math.Cos(lat);
            var Bn = WGS84_b * WGS84_b * Math.Sin(lat);
            var Ad = WGS84_a * Math.Cos(lat);
            var Bd = WGS84_b * Math.Sin(lat);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }

        internal void GetTags(ushort buildingId, Building data, List<osmWayTag> tags, ref string amenity)
        {
            var service = data.Info.m_class.m_service;
            var ss = data.Info.m_class.m_subService;
            var landuse = "";
            var building = "";
            var name = "";
            var className = data.Info.m_class.name.ToLower();

            if (servicesList.ContainsKey(service.ToString()))
                servicesList[service.ToString()]++;
            else
                servicesList[service.ToString()] = 1;

            if (subServicesList.ContainsKey(ss.ToString()))
                subServicesList[ss.ToString()]++;
            else
                subServicesList[ss.ToString()] = 1;

            if (classNameList.ContainsKey(className))
                classNameList[className]++;
            else
                classNameList[className] = 1;

            if ((data.m_flags & Building.Flags.CustomName) != Building.Flags.None)
            {
                var id = new InstanceID();
                id.Building = buildingId;
                name = Singleton<InstanceManager>.instance.GetName(id);
            }

            switch (service)
            {
                case ItemClass.Service.Beautification:
                    if (className.Contains("marker")){
                        return;
                    }
                    landuse = "recreation_ground";
                    break;
                case ItemClass.Service.Commercial:
                    building = "retail";
                    break;
                case ItemClass.Service.Residential:
                    building = "residential";
                    break;
                case ItemClass.Service.Office:
                    building = "commercial";
                    break;
                case ItemClass.Service.Industrial:
                    building = "industrial";               
                    break;
                case ItemClass.Service.Garbage:
                    landuse = "landfill";               
                    break;
                case ItemClass.Service.Education:
                    amenity = "school";
                    building = "school";               
                    break;
                case ItemClass.Service.Electricity:
                    tags.Add(new osmWayTag { k = "power", v = "plant" });
                    break;
                case ItemClass.Service.FireDepartment:
                    amenity = "fire_station";
                    building = "yes";
                    break;
                case ItemClass.Service.HealthCare:
                    amenity = "hospital";
                    building = "yes";
                    break;
                case ItemClass.Service.Monument:
                    name = data.Info.name;
                    building = "yes";
                    break;
                case ItemClass.Service.PoliceDepartment:
                    amenity = "police";
                    building = "yes";
                    break;
                case ItemClass.Service.PublicTransport:
                    if (!className.Contains("facility"))
                    {
                        return;
                    }

                    if (ss == ItemClass.SubService.PublicTransportBus)
                    {
                        tags.Add(new osmWayTag { k = "amenity", v = "bus_station" });
                        tags.Add(new osmWayTag { k = "public_transport", v = "station" });
                    }
                    else if(ss == ItemClass.SubService.PublicTransportTrain)
                    {
                        tags.Add(new osmWayTag { k = "public_transport", v = "station" });
                        tags.Add(new osmWayTag { k = "railway", v = "station" });
                    }
                    else if(ss == ItemClass.SubService.PublicTransportTaxi)
                    {
                        tags.Add(new osmWayTag { k = "amenity", v = "taxi" });
                        tags.Add(new osmWayTag { k = "capacity", v = "8" });
                    }
                    else if(ss == ItemClass.SubService.PublicTransportPlane)
                    {
                        tags.Add(new osmWayTag { k = "aeroway", v = "terminal" });
                    }
                    else if(ss == ItemClass.SubService.PublicTransportMetro)
                    {
                        tags.Add(new osmWayTag { k = "railway", v = "subway_entrance" });
                    }
                    else if(ss == ItemClass.SubService.PublicTransportShip)
                    {
                        tags.Add(new osmWayTag { k = "amenity", v = "ferry_terminal" });
                    }
                    break;
                case ItemClass.Service.Tourism:                    
                    building = "hotel";
                    break;
                default:
                    return;
            }

            if(className.Contains("beautificationitem"))
            {
                tags.Add(new osmWayTag { k = "tourism", v = "attraction" });
                tags.Add(new osmWayTag { k = "building", v = "yes" });
            }

            if (landuse != ""){
                tags.Add(new osmWayTag { k = "landuse", v = landuse });
            }
            if (building != "")
            {
                tags.Add(new osmWayTag { k = "building", v = building });
            }
            if (name != "")
            {
                name = cleanUpName(name);
                tags.Add(new osmWayTag { k = "name", v = name });
            }
        }

        private string cleanUpName(string name)
        {
            Regex removeSteamworksData = new Regex("(?:[0-9]*\\.)(.*)(?:_Data.*)");
            Regex addSpacingOnUppercase = new Regex("(.+?)([A-Z])");

            name = removeSteamworksData.Replace(name, "$1");
            name = addSpacingOnUppercase.Replace(name, "$1 $2");

            return name;
        }

        public void printDebug()
        {
            var nameListKeys = nameList.Keys.ToList();
            var elevationListKeys = elevationList.Keys.ToList();
            var servicesListKeys = servicesList.Keys.ToList();
            var subServicesListKeys = subServicesList.Keys.ToList();
            var classNameListKeys = classNameList.Keys.ToList();

            nameListKeys.Sort();
            elevationListKeys.Sort();
            servicesListKeys.Sort();
            subServicesListKeys.Sort();
            classNameListKeys.Sort();

            string nameLog = "Names: ";
            foreach(string name in nameListKeys)
            {
                nameLog += "\n" + name + "(" + nameList[name].ToString() + ")";
            }
            Debug.Log(nameLog);

            string elevationLog = "Elevations: ";
            foreach (string elevation in elevationListKeys)
            {
                elevationLog += "\n" + elevation + "(" + elevationList[elevation].ToString() + ")";
            }
            Debug.Log(elevationLog);

            string servicesLog = "Services: ";
            foreach (string services in servicesListKeys)
            {
                servicesLog += "\n" + services + "(" + servicesList[services].ToString() + ")";
            }
            Debug.Log(servicesLog);

            string subServicesLog = "Sub Services: ";
            foreach (string subServices in subServicesListKeys)
            {
                subServicesLog += "\n" + subServices + "(" + subServicesList[subServices].ToString() + ")";
            }
            Debug.Log(subServicesLog);

            string classNamesLog = "Class names: ";
            foreach (string classNames in classNameListKeys)
            {
                classNamesLog += "\n" + classNames + "(" + classNameList[classNames].ToString() + ")";
            }
            Debug.Log(classNamesLog);
        }
    }

}
