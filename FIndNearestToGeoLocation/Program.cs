using System.Diagnostics;

namespace FindNearestToGeoLocation;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            ReadVehiclePositionsFromDataFile rvpfbdf = new();
            KDTree kDTree = new KDTree();
            StreamWriter strWriter = new("Locations.csv");
            List<VehicleDataPoint> lGeoLocs = new();
            Stopwatch totalTimer = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();

            totalTimer.Start();
            Console.WriteLine("Initialised - now reading data file into list...");
            stopwatch.Start();
            for (int i = 0; i <= 2000002; i++)
            {
                rvpfbdf.ReadNext();
                /* write to csv file*/
                // strWriter.WriteLine(rvpfbdf.vdp.VehicleID.ToString() + "," + rvpfbdf.vdp.VehicleReg + ",'" + rvpfbdf.vdp.Latitude.ToString() + "','" + rvpfbdf.vdp.Longitude.ToString() + "'");
                VehicleDataPoint lvdp = new VehicleDataPoint(rvpfbdf.vdp.VehicleID, rvpfbdf.vdp.VehicleReg, rvpfbdf.vdp.Latitude, rvpfbdf.vdp.Longitude, rvpfbdf.vdp.RecordedTimeUTC);
                lGeoLocs.Add(lvdp);
            }
            stopwatch.Stop();
            // load the geolocs into the kdtree
            Console.WriteLine($"Done ({stopwatch.ElapsedMilliseconds}ms) - now loading list into KD Tree...");
            stopwatch.Restart();
            Node tree = kDTree.Buildrec(lGeoLocs, 0);
            stopwatch.Stop();
            Console.WriteLine($"Done ({stopwatch.ElapsedMilliseconds}ms)");
            // search for nearest location to the 10 coordinates
            List<float[]> coordsList = new List<float[]>();
            coordsList.Add(new float[] { 34.544909F, -102.100843F });
            coordsList.Add(new float[] { 32.345544F, -99.123124F });
            coordsList.Add(new float[] { 33.234235F, -100.214124F });
            coordsList.Add(new float[] { 35.195739F, -95.348899F });
            coordsList.Add(new float[] { 31.895839F, -97.789573F });
            coordsList.Add(new float[] { 32.895839F, -101.789573F });
            coordsList.Add(new float[] { 34.115839F, -100.225732F });
            coordsList.Add(new float[] { 32.335839F, -99.992232F });
            coordsList.Add(new float[] { 33.535339F, -94.792232F });
            coordsList.Add(new float[] { 32.234235F, -100.222222F });
            printLine();
            List<Node> searchResults;
            for (int i = 0; i < coordsList.Count; i++)
            {
                stopwatch.Restart();
                Console.Write("Done - now searching for nearest neighbors to coordinates  " + coordsList[i][0].ToString() + ", " + coordsList[i][1].ToString());
                searchResults = kDTree.Lookup(coordsList[i], tree, 1, 1000F);
                stopwatch.Stop();
                Console.WriteLine($"  ({stopwatch.Elapsed.TotalMilliseconds} ms)");
                if(!(searchResults == null))
                {
                    printVehicle(searchResults[0].DataPoint);
                }
            }
            totalTimer.Stop();
            Console.WriteLine($"Done! - toal time elapsed {totalTimer.ElapsedMilliseconds}ms");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            /* Console.WriteLine(ex.ToString()); */
        }
    }

    private static void printVehicle(VehicleDataPoint tvdp)
    {
        Console.WriteLine("VehicleId  " + tvdp.VehicleID.ToString());
        Console.WriteLine("vehicleReg " + tvdp.VehicleReg);
        Console.WriteLine("VehicleLat " + tvdp.Latitude.ToString());
        Console.WriteLine("VehicleLon " + tvdp.Longitude.ToString());
        Console.WriteLine("VehicleRec " + tvdp.RecordedTimeUTC.ToString());
        printLine();
    }

    private static void printVehicleRegOnly(VehicleDataPoint tvdp)
    {
        Console.WriteLine("vehicleReg " + tvdp.VehicleReg);
        printLine();
    }

    private static void printLine()
    {
        Console.WriteLine("________________________________________________________________________________");
    }
}
