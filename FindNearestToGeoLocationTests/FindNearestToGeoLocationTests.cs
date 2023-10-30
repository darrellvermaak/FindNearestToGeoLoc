using NUnit.Framework;
using static FindNearestToGeoLocation.VehicleDataPoint;

namespace FindNearestToGeoLocation
{
    public class FindNearestToGeoLocation
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void VehicleDataPoint_Create_Instance_Without_Data_Test()
        {
            VehicleDataPoint vdp = new();
            Assert.IsNotNull(vdp);
            Assert.IsInstanceOf<VehicleDataPoint>(vdp);
            Assert.That(vdp.VehicleID, Is.EqualTo(0));
            Assert.That(vdp.VehicleReg, Is.EqualTo(""));
            Assert.That(vdp.Latitude, Is.EqualTo(0.0));
            Assert.That(vdp.Longitude, Is.EqualTo(0.0));
            Assert.That(vdp.RecordedTimeUTC, Is.EqualTo(0));
        }
        [Test]
        public void VehicleDataPoint_Create_Instance_With_Data_Test()
        {
            int vehicleID = 1;
            string vehicleReg = "25 - 750 QD";
            float lat = 35.01914F;
            float lon = -99.56651F;
            ulong recordedTimeUTC = 1668691766;
            VehicleDataPoint vdp = new(vehicleID, vehicleReg, lat, lon, recordedTimeUTC);
            Assert.IsNotNull(vdp);
            Assert.IsInstanceOf<VehicleDataPoint>(vdp);
            Assert.That(vdp.VehicleID, Is.EqualTo(vehicleID));
            Assert.That(vdp.VehicleReg, Is.EqualTo(vehicleReg));
            Assert.That(vdp.Latitude, Is.EqualTo(lat));
            Assert.That(vdp.Longitude, Is.EqualTo(lon));
            Assert.That(vdp.RecordedTimeUTC, Is.EqualTo(recordedTimeUTC));
        }
        [Test]
        public void ShortListManager_Search_Tests()
        {
            float[] testArray = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61 };
            VehicleDataPoint vdpTest = new(1, "25-750 QD",0,0,0);
            Node testNode = new(0,0F,null, null, vdpTest);
            List<Node> testList = new List<Node>();

            for(int i = 0; i < testArray.Length; i++)
            {
                testNode = new(0, 0F, null, null, vdpTest);
                testNode.DistanceFromSearchCoordinates = testArray[i];
                testList.Add(testNode);
            }
            Node newTestNode = new(0, 0F, null, null, vdpTest);
            newTestNode.DistanceFromSearchCoordinates = 2;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(0));
            newTestNode.DistanceFromSearchCoordinates = 61;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(17));
            newTestNode.DistanceFromSearchCoordinates = 23;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(8));
            newTestNode.DistanceFromSearchCoordinates = 1;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(-1));
            newTestNode.DistanceFromSearchCoordinates = 62;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(-19));
            newTestNode.DistanceFromSearchCoordinates = 10;
            Assert.That(ShortListManager.Search(newTestNode, testList), Is.EqualTo(-5));
        }
        [Test]
        public void ShortListManager_Insert_Tests()
        {
            VehicleDataPoint vdpTest = new(1, "25-750 QD", 35.01914F, -99.56651F, 0);
            Node testNode = new(0, 0F, null, null, vdpTest);
            List<Node> testList = new List<Node>();

            testNode.DistanceFromSearchCoordinates = 50;
            List<Node> returnedList_A = ShortListManager.Insert(testNode, testList);
            Assert.That(returnedList_A.Count, Is.EqualTo(1));
            Assert.That(returnedList_A[0].DistanceFromSearchCoordinates, Is.EqualTo(50));

            vdpTest = new (2, "7A-0157 DJ", 33.623158F, -102.35253F, 0);
            testNode = new(0, 0F, null, null, vdpTest);
            testNode.DistanceFromSearchCoordinates = 25;
            List<Node>returnedList_B = ShortListManager.Insert(testNode, returnedList_A);
            Assert.That(returnedList_B.Count, Is.EqualTo(2));
            Assert.That(returnedList_B[0].DistanceFromSearchCoordinates, Is.EqualTo(25));
            Assert.That(returnedList_B[0].DataPoint.VehicleID, Is.EqualTo(2));
            Assert.That(returnedList_B[1].DistanceFromSearchCoordinates, Is.EqualTo(50));
            Assert.That(returnedList_B[1].DataPoint.VehicleID, Is.EqualTo(1));
        }
        [Test]
        public void Node_Create_Instance_Tests()
        {
            Node node = new(1, 1, null, null, null);
            Assert.IsNotNull(node);
            Assert.IsInstanceOf<Node>(node);
        }
        [Test]
        public void KDTree_Tests()
        {
            KDTree kDTree = new KDTree();
            List<VehicleDataPoint> vdpList = new List<VehicleDataPoint>();
            Node? tree = kDTree.Buildrec(vdpList, 0);
            Assert.IsNull(tree);

            vdpList.Add(new VehicleDataPoint(1, "25-750 QD", 35.01914F, -99.56651F, 0));
            tree = kDTree.Buildrec(vdpList, 0);
            Assert.IsNotNull(tree);
            Assert.IsInstanceOf<Node>(tree);
            Assert.That(tree.DataPoint.VehicleID, Is.EqualTo(1));
            Assert.That(tree.DataPoint.VehicleReg, Is.EqualTo("25-750 QD"));
            Assert.That(tree.DataPoint.Latitude, Is.EqualTo(35.01914F));
            Assert.That(tree.DataPoint.Longitude, Is.EqualTo(-99.56651F));
            Assert.That(tree.DataPoint.RecordedTimeUTC, Is.EqualTo(0));
            Console.WriteLine(tree.Split);

            vdpList.Add(new VehicleDataPoint(2, "7A-0157 DJ", 33.623158F, -102.35253F, 0));
            tree = kDTree.Buildrec(vdpList, 0);
            Assert.IsNotNull(tree);
            Assert.IsInstanceOf<Node>(tree);
            Assert.That(tree.DataPoint.VehicleID, Is.EqualTo(1));
            Assert.That(tree.DataPoint.VehicleReg, Is.EqualTo("25-750 QD"));
            Assert.That(tree.DataPoint.Latitude, Is.EqualTo(35.01914F));
            Assert.That(tree.DataPoint.Longitude, Is.EqualTo(-99.56651F));
            Assert.That(tree.DataPoint.RecordedTimeUTC, Is.EqualTo(0));
            Console.WriteLine(tree.Split);

            Assert.IsNotNull(tree);
            Assert.IsInstanceOf<Node>(tree.Left);
            Assert.That(tree.Left.DataPoint.VehicleID, Is.EqualTo(2));
            Assert.That(tree.Left.DataPoint.VehicleReg, Is.EqualTo("7A-0157 DJ"));
            Assert.That(tree.Left.DataPoint.Latitude, Is.EqualTo(33.623158F));
            Assert.That(tree.Left.DataPoint.Longitude, Is.EqualTo(-102.35253F));
            Assert.That(tree.Left.DataPoint.RecordedTimeUTC, Is.EqualTo(0));
            Console.WriteLine(tree.Split);

            vdpList.Add(new VehicleDataPoint(3, "E1-50445 VI", 32.624306F, -99.52194F, 0));
            tree = kDTree.Buildrec(vdpList, 0);
            Assert.IsNotNull(tree);
            Assert.IsInstanceOf<Node>(tree.Left);
            Assert.That(tree.Right.DataPoint.VehicleID, Is.EqualTo(2));
            Assert.That(tree.Right.DataPoint.VehicleReg, Is.EqualTo("7A-0157 DJ"));
            Assert.That(tree.Right.DataPoint.Latitude, Is.EqualTo(33.623158F));
            Assert.That(tree.Right.DataPoint.Longitude, Is.EqualTo(-102.35253F));
            Assert.That(tree.Right.DataPoint.RecordedTimeUTC, Is.EqualTo(0));
            Console.WriteLine(tree.Split);
        }
    }
}
