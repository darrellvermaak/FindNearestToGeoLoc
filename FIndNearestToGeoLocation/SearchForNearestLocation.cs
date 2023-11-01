using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    public class SearchForNearestLocation
    {
        public SearchForNearestLocation() { }

        public static int SearchLat(float latitude, List<VehicleDataPoint> vdpList)
        {
            int nearestPos = -1;
            float nearestLat = 180.0F;
            int elementPos = -1;
            int maxPosition = vdpList.Count -1;
            int minPosition = 0;
            while ((elementPos > minPosition) || (elementPos < maxPosition))
            {
                elementPos = minPosition + (int) Math.Floor((maxPosition - minPosition + 1) * 0.5);
                if (vdpList[elementPos].Latitude > latitude)
                {
                    if((maxPosition == elementPos) && (maxPosition == vdpList.Count -1))
                    { 
                        minPosition++;
                    } else if(maxPosition == elementPos)
                    {
                        maxPosition--;
                    } else
                    {
                        maxPosition = elementPos;
                    }
                } else if(vdpList[elementPos].Latitude < latitude)
                {
                    if(minPosition == elementPos)
                    {
                        maxPosition--;
                    } else
                    {
                        minPosition = elementPos;
                    }
                } else
                {
                    return elementPos;
                }
                if (Math.Abs(nearestLat - latitude) > Math.Abs(vdpList[elementPos].Latitude - latitude))
                {
                    nearestLat = vdpList[elementPos].Latitude;
                    nearestPos = elementPos;
                }
            }
            return nearestPos;
        }

        public static int SearchLon(float latitude, float longitude, int elementPos, List<VehicleDataPoint> vdpList)
        {
            int lowerPos = elementPos;
            int upperPos = elementPos;
            int closestElement = elementPos;

            while (((Math.Pow(vdpList[lowerPos].Latitude - latitude, 2) <= (Math.Abs(vdpList[closestElement].Longitude - longitude) * Math.Abs(vdpList[closestElement].Longitude - longitude)))
                || (Math.Pow(vdpList[upperPos].Latitude - latitude, 2) <= (Math.Abs(vdpList[closestElement].Longitude - longitude) * Math.Abs(vdpList[closestElement].Longitude - longitude))))
                && ((lowerPos > 0)||(upperPos < vdpList.Count -1))
                )
            {
                if(lowerPos > 0)
                { lowerPos--; }
                if (Math.Pow((vdpList[lowerPos].Longitude - longitude),2) < (Math.Pow((vdpList[closestElement].Longitude - longitude),2)))
                {
                    closestElement = lowerPos;
                }

                if (upperPos < vdpList.Count -1)
                { upperPos++; }
                if (Math.Pow((vdpList[upperPos].Longitude - longitude), 2) < (Math.Pow((vdpList[closestElement].Longitude - longitude), 2)))
                {
                    closestElement = upperPos;
                }
            }
            return closestElement;
        }
    }
}
