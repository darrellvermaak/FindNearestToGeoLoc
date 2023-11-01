using NUnit.Framework;
using System;

namespace FindNearestToGeoLocation
{
    public class FindNearestToGeoLocation
    {
        List<VehicleDataPoint> lVDPList = new();

        [SetUp]
        public void Setup()
        {
            VehicleDataPoint vdp;
            int vehicleID;
            string vehicleReg;
            float lat;
            float lon;
            ulong recordedTimeUTC;

            vehicleID = 864907;
            vehicleReg = "3I - 388 XE";
            lat = 32.344444F;
            lon = -99.12403F;
            recordedTimeUTC = 1668691785;
            vdp = new(vehicleID, vehicleReg, lat, lon, recordedTimeUTC);
            lVDPList.Add(vdp);

            vehicleID = 835420;
            vehicleReg = "77 - 0126 BV";
            lat = 33.23224F;
            lon = -100.21374F;
            recordedTimeUTC = 1668691815;
            vdp = new(vehicleID, vehicleReg, lat, lon, recordedTimeUTC);
            lVDPList.Add(vdp);

            vehicleID = 239701;
            vehicleReg = "K2-080 CT";
            lat = 34.54235F;
            lon = -102.10086F;
            recordedTimeUTC = 1668691784;
            vdp = new(vehicleID, vehicleReg, lat, lon, recordedTimeUTC);
            lVDPList.Add(vdp);
            
            vehicleID = 1;
            vehicleReg = "25 - 750 QD";
            lat = 35.01914F;
            lon = -99.56651F;
            recordedTimeUTC = 1668691766;
            vdp = new(vehicleID, vehicleReg, lat, lon, recordedTimeUTC);
            lVDPList.Add(vdp);
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
        public void Search_For_Nearest_Lat_Test()
        {
            int index;
            // lat equals the last data point in the list
            index = SearchForNearestLocation.SearchLat(35.01914F, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            // lat sllightly greater than the last data point in the list
            index = SearchForNearestLocation.SearchLat(35.01915F, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            // lat slightly less than the last data point in the list
            index = SearchForNearestLocation.SearchLat(35.01913F, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            // lat equals the third data point in the list
            index = SearchForNearestLocation.SearchLat(34.54235F, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            // lat sllightly greater than the third data point in the list
            index = SearchForNearestLocation.SearchLat(34.54236F, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            // lat slightly less than the third data point in the list
            index = SearchForNearestLocation.SearchLat(34.54234F, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            // lat equals the second data point in the list
            index = SearchForNearestLocation.SearchLat(33.23224F, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            // lat sllightly greater than the second data point in the list
            index = SearchForNearestLocation.SearchLat(33.23225F, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            // lat slightly less than the second data point in the list
            index = SearchForNearestLocation.SearchLat(33.23223F, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            // lat equals the first data point in the list
            index = SearchForNearestLocation.SearchLat(32.344444F, lVDPList);
            Assert.That(index, Is.EqualTo(0));
            // lat sllightly greater than the first data point in the list
            index = SearchForNearestLocation.SearchLat(32.344445F, lVDPList);
            Assert.That(index, Is.EqualTo(0));
            // lat slightly less than the first data point in the list
            index = SearchForNearestLocation.SearchLat(32.344443F, lVDPList);
            Assert.That(index, Is.EqualTo(0));
        }
        [Test]
        public void Search_For_Nearest_Lon_Test()
        {
            int index;
            index = SearchForNearestLocation.SearchLon(35.01914F, -99.56651F, 3, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            index = SearchForNearestLocation.SearchLon(35.01915F, -99.56652F, 3, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            index = SearchForNearestLocation.SearchLon(35.01913F, -99.56650F, 3, lVDPList);
            Assert.That(index, Is.EqualTo(3));
            index = SearchForNearestLocation.SearchLon(34.54235F, -102.10086F, 2, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            index = SearchForNearestLocation.SearchLon(34.54236F, -102.10087F, 2, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            index = SearchForNearestLocation.SearchLon(34.54234F, -102.10085F, 2, lVDPList);
            Assert.That(index, Is.EqualTo(2));
            index = SearchForNearestLocation.SearchLon(33.23224F, -100.21374F, 1, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            index = SearchForNearestLocation.SearchLon(33.23225F, -100.21375F, 1, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            index = SearchForNearestLocation.SearchLon(33.23223F, -100.21373F, 1, lVDPList);
            Assert.That(index, Is.EqualTo(1));
            index = SearchForNearestLocation.SearchLon(32.344444F, -99.12403F, 0, lVDPList);
            Assert.That(index, Is.EqualTo(0));
            index = SearchForNearestLocation.SearchLon(32.344445F, -99.12404F, 0, lVDPList);
            Assert.That(index, Is.EqualTo(0));
            index = SearchForNearestLocation.SearchLon(32.344443F, -99.12402F, 0, lVDPList);
            Assert.That(index, Is.EqualTo(0));
        }
    }
}
