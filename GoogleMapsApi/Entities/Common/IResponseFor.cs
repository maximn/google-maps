namespace GoogleMapsApi.Entities.Common
{
	/// <summary>
	/// Marker interface that pairs a response type with its corresponding request type,
	/// enabling compile-time matching in the engine facade.
	/// </summary>
	/// <typeparam name="T">The request type this response is produced for.</typeparam>
	public interface IResponseFor<T> where T : MapsBaseRequest { }
}