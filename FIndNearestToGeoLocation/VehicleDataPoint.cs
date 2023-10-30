using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    public class VehicleDataPoint
    {
        /*
        * VehicleId int32
        * VehicleRegistration Null terminated ASCII string
        * Latitude float4
        * Longitude float4
        * RecordedTimeUTC UInt64
        */
        public int VehicleID { get; set; }
        public string VehicleReg { get; set; }
        public float[] Position { get; set; } = { 0F, 0F };
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public UInt64 RecordedTimeUTC { get; set; }
        public VehicleDataPoint(int vehicleid, string vehiclereg, float latitude, float longitude, UInt64 recordedtimeutc)
        {
            VehicleID = vehicleid;
            VehicleReg = vehiclereg;
            Latitude = latitude;
            Longitude = longitude;
            Position[0] = latitude;
            Position[1] = longitude;
            RecordedTimeUTC = recordedtimeutc;
        }
        public VehicleDataPoint()
        {
            VehicleReg = "";
        }
    }
}
