﻿using System;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Directions.Response
{
	/// <summary>
	/// duration indicates the total duration of this leg
	/// These fields may be absent if the duration is unknown.
	/// </summary>
	[DataContract(Name = "duration")]
	public class Duration
	{
		[DataMember(Name = "value")]
		internal int ValueInSec
		{
			get
			{
				return (int)Math.Round(Value.TotalSeconds);
			}
			set
			{
				Value = TimeSpan.FromSeconds(value);
			}
		}

		/// <summary>
		/// value indicates the duration in seconds.
		/// </summary>
		public TimeSpan Value { get; set; }

		/// <summary>
		/// text contains a human-readable representation of the duration.
		/// </summary>
		[DataMember(Name = "text")]
		public string Text { get; set; }

		[DataMember(Name = "time_zone")]
		public string TimeZone { get; set; }
	}
}
