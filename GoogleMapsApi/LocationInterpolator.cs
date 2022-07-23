using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
	public class LocationInterpolator
	{
		public static IList<ILocationString> GetList(Location a, Location b, int num)
		{
            

            var diff_X = b.Latitude - a.Latitude;
            var diff_Y = b.Longitude - a.Longitude;
            int pointNum = num;

            var interval_X = diff_X / (pointNum + 1);
            var interval_Y = diff_Y / (pointNum + 1);

            IList<ILocationString> pointList = new List<ILocationString>();
            pointList.Add(a);
            for (int i = 1; i <= pointNum; i++)
            {
                pointList.Add(new Location(a.Latitude + interval_X * i, a.Longitude + interval_Y * i));
            }
            pointList.Add(b);
            return pointList;


        }
	}
}
