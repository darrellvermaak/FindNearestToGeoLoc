using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    public class ReadVehiclePositionsFromDataFile
    {
        private string dataFileName;
        static BinaryReader? binaryReader;
        private long dataFileLength;
        public VehicleDataPoint vdp = new VehicleDataPoint();
        private StringBuilder stringBuilder = new StringBuilder();

        public ReadVehiclePositionsFromDataFile()
        {
            dataFileName = AppDomain.CurrentDomain.BaseDirectory + "\\VehiclePositions.dat";
            openFile();
            dataFileLength = binaryReader.BaseStream.Length;
        }

        public void ReadNext()
        {
            if (binaryReader.BaseStream.Position < dataFileLength)
            {
                stringBuilder.Clear();
                vdp.VehicleReg = "";
                vdp.VehicleID = binaryReader.ReadInt32();
                char nextChar;
                while ((nextChar = binaryReader.ReadChar()) != '\0')
                {
                    stringBuilder.Append(nextChar);
                }
                vdp.VehicleReg = stringBuilder.ToString();
                vdp.Latitude = binaryReader.ReadSingle();
                vdp.Longitude = binaryReader.ReadSingle();
                vdp.RecordedTimeUTC = binaryReader.ReadUInt64();
            }
        }

        private void openFile() 
        {
            try
            {
                binaryReader = new BinaryReader(File.Open(dataFileName, FileMode.Open));
            } catch (Exception e)
            {
                Console.WriteLine("Unable to open data file at :");
                Console.WriteLine(dataFileName);
                Console.WriteLine("Please place the data file in the same directory/folder as this application - the required location is printed above");
                throw new Exception("Unable to open data file.");
            }
        }
    }
}
