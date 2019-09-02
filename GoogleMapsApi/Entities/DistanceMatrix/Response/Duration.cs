namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System;
    using System.Runtime.Serialization;

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
			get => (int)Math.Round(this.Value.TotalSeconds);
            set => this.Value = TimeSpan.FromSeconds(value);
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
	}
}
