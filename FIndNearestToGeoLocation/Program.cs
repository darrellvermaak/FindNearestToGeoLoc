using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace FindNearestToGeoLocation;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-ZA");
            List<VehicleDataPoint> lGeoLocs = new();
            List<float[]> coordsList = new();
            Stopwatch totalTimer = new();
            Stopwatch stopwatch = new();

            totalTimer.Start();
            Console.WriteLine("Initialised - now reading data file into list...");
            readDataFileIntoList(lGeoLocs);
            lGeoLocs = sortGeoLocs(lGeoLocs);
            addGeoCoOrdinatesToFind(coordsList);
            printLine();
            searchForCoOrdinates(coordsList, lGeoLocs);
            totalTimer.Stop();
            Console.WriteLine($"Done! - toal time elapsed {totalTimer.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)}ms");
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
        Console.WriteLine("VehicleLat " + tvdp.Latitude.ToString(CultureInfo.InvariantCulture));
        Console.WriteLine("VehicleLon " + tvdp.Longitude.ToString(CultureInfo.InvariantCulture));
        Console.WriteLine("VehicleRec " + tvdp.RecordedTimeUTC.ToString());
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

    private static void readDataFileIntoList(List<VehicleDataPoint> lGeoLocs)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        ReadVehiclePositionsFromDataFile rvpfbdf = new();
        for (int i = 0; i <= 2000002; i++)
        {
            rvpfbdf.ReadNext();
            VehicleDataPoint lvdp = new VehicleDataPoint(rvpfbdf.vdp.VehicleID, rvpfbdf.vdp.VehicleReg, rvpfbdf.vdp.Latitude, rvpfbdf.vdp.Longitude, rvpfbdf.vdp.RecordedTimeUTC);
            lGeoLocs.Add(lvdp);
        }
        stopwatch.Stop();
        Console.WriteLine($"(Done reading data file in {stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)}ms)");
    }

    private static void addGeoCoOrdinatesToFind(List<float[]> coordsList)
    {
        // add the 10 coordinates
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
    }

    private static void searchForCoOrdinates(List<float[]> coordsList, List<VehicleDataPoint> lGeoLocs)
    {
        Stopwatch stopwatch = new();
        int index;
        for (int i = 0; i < coordsList.Count; i++)
        {
            stopwatch.Restart();
            Console.WriteLine("Now searching for nearest neighbors to coordinates  " + coordsList[i][0].ToString() + ", " + coordsList[i][1].ToString());
            index = SearchForNearestLocation.SearchLat(coordsList[i][0], lGeoLocs);
            index = SearchForNearestLocation.SearchLon(coordsList[i][0], coordsList[i][1], index, lGeoLocs);
            stopwatch.Stop();
            printVehicle(lGeoLocs[index]);
            Console.WriteLine($"(Found in {stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} ms)");
            printLine();
        }
    }

    private static List<VehicleDataPoint> sortGeoLocs(List<VehicleDataPoint> lGeoLocs)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine($"Sorting list...");
        lGeoLocs = lGeoLocs.AsParallel().OrderBy(x => x.Latitude).ToList();
        stopwatch.Stop();
        Console.WriteLine($"(Done sorting list in {stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)}ms)");
        return lGeoLocs;
    }
}
