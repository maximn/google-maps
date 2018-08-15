using System;

namespace GoogleMapsApi.Entities.Directions.Request
{
	[Flags]
	public enum AvoidWay
	{
		Nothing = 0x0,
		Tolls = 0x1,
		Highways = 0x2,
		Ferries = 0x3,
		Indoor = 0x4
	}
}
