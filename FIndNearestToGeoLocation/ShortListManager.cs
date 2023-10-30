using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    /*
     * This class manages the short list of potential node results
     */
    public static class ShortListManager
    {
        /* this inserts the new candidate into the shortlist */
        public static List<Node> Insert(Node item, List<Node> shortList)
        {
            int i = Search(item, shortList);
            if (i < 0)
            {
                i = -(i + 1);
            }

            shortList.Insert(i, item);
            return shortList;
        }

        /* this returns the index in the array / list where the new candidate should be inserted */
        public static int Search(Node item, List<Node> shortList)
        {
            int low = 0;
            int high = shortList.Count - 1;
            int mid;
            float comp;

            while(low <= high)
            {
                mid = (low + high) >> 1;
                comp = shortList[mid].DistanceFromSearchCoordinates - item.DistanceFromSearchCoordinates;
                if (comp < 0)
                {
                    low = mid + 1;
                }
                else
                {
                    if (comp > 0)
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        return mid;
                    }
                }
            }
            return -(low + 1);
        }
    }
}
