using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMapsApi.Directions.Request
{
    [Flags]
    public enum AvoidWay
    {
        Nothing = 0x0,
        Tolls = 0x1,
        Highways = 0x2
    }
}
