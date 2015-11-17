using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapper.Managers;
using Mapper.Containers;
using System.Collections.Generic;

namespace MapperTests.Tests
{
    [TestClass()]
    public class RoadTests
    {
        [TestMethod()]
        public void FlatRoadTest()
        {
            var testRoads = new RoadContainer()
            {
                inGameNamePrefix = "Test Road",
                roadType = RoadContainer.Type.Road,
                searchLimit = RoadContainer.Limit.Ground,
                tags =
                {
                    new Mapper.OSM.OSMWayTag() { k = "test", v = "value" }
                }
            };

            var testRoadExtensions = new List<KeyValuePair<string, RoadContainer.Limit>>()
            {
                new KeyValuePair<string, RoadContainer.Limit>("Test Bus", RoadContainer.Limit.Bus),
                new KeyValuePair<string, RoadContainer.Limit>("Test Bicycle", RoadContainer.Limit.Bicycle),
                new KeyValuePair<string, RoadContainer.Limit>("Test All Elevations", RoadContainer.Limit.ElevationsOnly),
                new KeyValuePair<string, RoadContainer.Limit>("Test Flat Only", RoadContainer.Limit.Ground)
            };

            var finalList = RoadManager.AddRoadExtensions(testRoads, testRoadExtensions);

            Assert.IsTrue(finalList.Count == 2);
            Assert.IsTrue(finalList[0].inGameNamePrefix == "Test Road Test All Elevations");
            Assert.IsTrue(finalList[1].inGameNamePrefix == "Test Road Test Flat Only");
        }

        [TestMethod()]
        public void ElevatedRoadTest()
        {
            var testRoads = new RoadContainer()
            {
                inGameNamePrefix = "Test Road",
                roadType = RoadContainer.Type.Road,
                searchLimit = RoadContainer.Limit.Elevated,
                tags =
                {
                    new Mapper.OSM.OSMWayTag() { k = "test", v = "value" }
                }
            };

            var testRoadExtensions = new List<KeyValuePair<string, RoadContainer.Limit>>()
            {
                new KeyValuePair<string, RoadContainer.Limit>("Test Bus", RoadContainer.Limit.Bus),
                new KeyValuePair<string, RoadContainer.Limit>("Test Bicycle", RoadContainer.Limit.Bicycle),
                new KeyValuePair<string, RoadContainer.Limit>("Test All Elevations", RoadContainer.Limit.ElevationsOnly),
                new KeyValuePair<string, RoadContainer.Limit>("Test Elevated Only", RoadContainer.Limit.Elevated),
                new KeyValuePair<string, RoadContainer.Limit>("Test Flat Only", RoadContainer.Limit.Ground)
            };

            var finalList = RoadManager.AddRoadExtensions(testRoads, testRoadExtensions);

            Assert.IsTrue(finalList.Count == 2);
            Assert.IsTrue(finalList[0].inGameNamePrefix == "Test Road Test All Elevations");
            Assert.IsTrue(finalList[1].inGameNamePrefix == "Test Road Test Elevated Only");
        }

        [TestMethod()]
        public void BicycleRoadTest()
        {
            var testRoads = new RoadContainer()
            {
                inGameNamePrefix = "Test Bicycle Road",
                roadType = RoadContainer.Type.Road,
                searchLimit = RoadContainer.Limit.Bicycle,
                tags =
                {
                    new Mapper.OSM.OSMWayTag() { k = "test", v = "value" }
                }
            };

            var testRoadExtensions = new List<KeyValuePair<string, RoadContainer.Limit>>()
            {
                new KeyValuePair<string, RoadContainer.Limit>("Test Bus", RoadContainer.Limit.Bus),
                new KeyValuePair<string, RoadContainer.Limit>("Test Bicycle", RoadContainer.Limit.Bicycle),
                new KeyValuePair<string, RoadContainer.Limit>("Test All Elevations", RoadContainer.Limit.ElevationsOnly),
                new KeyValuePair<string, RoadContainer.Limit>("Test Elevated Only", RoadContainer.Limit.Elevated),
                new KeyValuePair<string, RoadContainer.Limit>("Test Flat Only", RoadContainer.Limit.Ground)
            };

            var finalList = RoadManager.AddRoadExtensions(testRoads, testRoadExtensions);

            Assert.IsTrue(finalList.Count == 1);
            Assert.IsTrue(finalList[0].inGameNamePrefix == "Test Bicycle Road Test Bicycle");
        }

        [TestMethod()]
        public void MultipleRoadTest()
        {
            var testRoads = new RoadContainer()
            {
                inGameNamePrefix = "Test Road",
                roadType = RoadContainer.Type.Road,
                searchLimit = RoadContainer.Limit.Ground,
                tags =
                {
                    new Mapper.OSM.OSMWayTag() { k = "test", v = "value" }
                }
            };

            var testRoadExtensions = new List<KeyValuePair<string, RoadContainer.Limit>>()
            {
                new KeyValuePair<string, RoadContainer.Limit>("Test Bus", RoadContainer.Limit.Bus),
                new KeyValuePair<string, RoadContainer.Limit>("Test Bicycle", RoadContainer.Limit.Bicycle),
                new KeyValuePair<string, RoadContainer.Limit>("Test Elevated Only", RoadContainer.Limit.Elevated),
                new KeyValuePair<string, RoadContainer.Limit>("Test Flat Only", RoadContainer.Limit.Ground),
                new KeyValuePair<string, RoadContainer.Limit>("Test Flat Only 2", RoadContainer.Limit.Ground)
            };

            var finalList = RoadManager.AddRoadExtensions(testRoads, testRoadExtensions);

            Assert.IsTrue(finalList.Count == 2);
            Assert.IsTrue(finalList[0].inGameNamePrefix == "Test Road Test Flat Only");
            Assert.IsTrue(finalList[1].inGameNamePrefix == "Test Road Test Flat Only 2");
        }
    }
}