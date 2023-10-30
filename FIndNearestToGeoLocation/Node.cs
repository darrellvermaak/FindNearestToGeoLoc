using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNearestToGeoLocation
{
    public class Node
    {
        public int Axis { get; set; }
        public float Split { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public VehicleDataPoint? DataPoint { get; set; }
        public float DistanceFromSearchCoordinates { get; set; } // used during Lookup / Search when node is added to the shortlist

        public Node(int axis, float split, Node? left, Node? right, VehicleDataPoint? datapoint)
        {
            Axis = axis;
            Split = split;
            Left = left;
            Right = right;
            DataPoint = datapoint;
        }
    }
}
