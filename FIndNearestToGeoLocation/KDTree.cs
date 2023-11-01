using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    class GEOSortLat : IComparer<VehicleDataPoint>
    {
        public int Compare(VehicleDataPoint x, VehicleDataPoint y)
        {
            return x.Latitude.CompareTo(y.Latitude);
        }
    }

    class GEOSortLon : IComparer<VehicleDataPoint>
    {
        public int Compare(VehicleDataPoint x, VehicleDataPoint y)
        {
            return x.Longitude.CompareTo(y.Longitude);
        }
    }

    public class KDTree
    {
        public KDTree() { }

        public Node? Buildrec(List<VehicleDataPoint> vdpList, int depth)
        {
            if (vdpList.Count == 0)
                return null;

            if (vdpList.Count == 1)
                return new Node(0, 0F, null, null, vdpList[0]);

            int axis = depth % vdpList[0].Position.Length;

            if (axis == 0)
            {
                // GEOSortLat geoSortLat = new GEOSortLat();
                if(vdpList.Count > 37500)
                {
                    vdpList = vdpList.AsParallel().OrderBy(x => x.Latitude).ToList();
                } else
                {
                    vdpList.Sort((x,y) => x.Latitude.CompareTo(y.Latitude));
                }
            }
            else
            {
                // GEOSortLon geoSortLon = new GEOSortLon();
                if(vdpList.Count > 37500)
                {
                    vdpList = vdpList.AsParallel().OrderBy(x => x.Longitude).ToList();
                } else
                {
                    vdpList.Sort((x, y) => x.Longitude.CompareTo(y.Longitude));
                }
            }

            int i = (int) Math.Floor(vdpList.Count * 0.5);

            ++depth;

            return new Node(
                axis,
                vdpList[i].Position[axis],
                Buildrec(vdpList.GetRange(0, i), depth),
                Buildrec(vdpList.GetRange(i, vdpList.Count - i - 1 ), depth),
                vdpList[i]
            );
        }

        /* returns a list of results (Nodes) - number of results specified by n, max = max distance from searched for position (in metres?) if 0 specified defaults to 100km */
        public List<Node> Lookup(float[] searchedCoords, Node node, int numberNearestNeighbors, float max)
        {
            if (!(max > 0))
                max = 100000;

            List<Node> resultsList = new List<Node> ();

            if (node == null || numberNearestNeighbors <= 0)
                return resultsList;

            List<float> stackDistance = new List<float> ();
            List<Node> stackNode = new List<Node>();
            stackNode.Add(node);
            stackDistance.Add(0);

            float dist;
            int i;

            bool validNode = true;
            while(stackNode.Count > 0)
            {
                validNode = true;
                dist = stackDistance[stackDistance.Count - 1];
                stackDistance.RemoveAt(stackDistance.Count - 1);
                node = stackNode[stackNode.Count - 1];
                stackNode.RemoveAt(stackNode.Count - 1);

                if (dist > max)
                    continue;

                if (resultsList.Count == numberNearestNeighbors && resultsList[resultsList.Count - 1].DistanceFromSearchCoordinates < dist * dist)
                    continue;

                while (validNode && node is Node)
                {
                    if ((node.Left == null) && (node.Right == null))
                    {
                        validNode = false;
                        continue;
                    }
                    if (searchedCoords[node.Axis] < node.Split)
                    {
                        if(node.Right != null)
                        {
                            stackNode.Add(node.Right);
                            stackDistance.Add(node.Split - searchedCoords[node.Axis]);
                            if (node.Left != null)
                                node = node.Left;
                            else
                                validNode = false;
                        } else { validNode = false; }
                    } else
                    {
                        if (node.Left != null)
                        {
                            stackNode.Add(node.Left);
                            stackDistance.Add(searchedCoords[node.Axis] - node.Split);
                            if(node.Right != null)
                                node = node.Right;
                            else
                                validNode = false;
                        } else { validNode = false; }
                    }
                       
                }
                dist = distance(searchedCoords, node.DataPoint.Position);

                if(dist <= max * max)
                {
                    node.DistanceFromSearchCoordinates = dist;
                    resultsList = ShortListManager.Insert(node, resultsList);
                }

                if (resultsList.Count > numberNearestNeighbors)
                    resultsList.RemoveRange(resultsList.Count - 1, 1);
            }
            return resultsList;
        }

        private float distance(float[] a, float[] b)
        {
            int i = Math.Min(a.Length, b.Length);
            float k;
            float d = 0;

            while(i-- > 0)
            {
                k = b[i] - a[i];
                d += k * k;
            }

            return d;
        }
    }
}
