using System.Collections.Generic;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
	/// <summary>
	/// Tests examples from - http://code.google.com/apis/maps/documentation/staticmaps/
	/// </summary>
	[TestFixture]
	public class StaticMaps
	{
		private StaticMapsEngine staticMapGenerator;

		[TestFixtureSetUp]
		public void Init()
		{
			staticMapGenerator = new StaticMapsEngine();
		}

		[Test(Description = "First basic example")]
		public void BasicTest()
		{
			// downtown New York City
			StaticMapRequest request = new StaticMapRequest(new AddressLocation("Brooklyn Bridge,New York,NY"), 14, new ImageSize(512, 512))
																						{
																							MapType = MapType.Roadmap,
																							Markers = new List<Marker>(){
			                                    		                            	new Marker(){
			                                    		                            	            	Style = new MarkerStyle()
			                                    		                            	            	        	{
			                                    		                            	            	        		Color = "blue",
			                                    		                            	            	        		Label = "S"

			                                    		                            	            	        	},
			                                    		                            	            	Locations = new List<ILocation>()
			                                    		                            	            	            	{
			                                    		                            	            	            		new Location(40.702147,-74.015794)
			                                    		                            	            	            	}
			                                    		                            	            },
																																												new Marker()
																																													{
																																														Style = new MarkerStyle()
																																														        	{
																																														        		Color = "green",
																																																				Label = "G"
																																														        	},
																																																			Locations = new List<ILocation>()
																																																			            	{
																																																			            		new Location(40.711614,-74.012318)
																																																			            	}
																																													},
			                                    		                            	new Marker()
			                                    		                            		{
			                                    		                            			Style = new MarkerStyle()
			                                    		                            			        	{
			                                    		                            			        		Color = "red",
			                                    		                            			        		Label = "C"
			                                    		                            			        	},
			                                    		                            			Locations = new List<ILocation>()
			                                    		                            			            	{
			                                    		                            			            		new Location(40.718217,-73.998284)
			                                    		                            			            	}
																		
			                                    		                            		}
			                                    		                            },
																							Sensor = false

																						};

			string generateStaticMapURL = staticMapGenerator.GenerateStaticMapURL(request);


			string expectedResult =
				@"http://maps.google.com/maps/api/staticmap?center=Brooklyn+Bridge,New+York,NY&zoom=14&size=512x512&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&sensor=false";

			Assert.AreEqual(expectedResult, generateStaticMapURL);
		}

		[Test]
		public void AddressTest()
		{
			StaticMapRequest request = new StaticMapRequest(new AddressLocation("Berkeley,CA"), 14, new ImageSize(400, 400));

			string generateStaticMapURL = staticMapGenerator.GenerateStaticMapURL(request);

			string expectedResult =
				@"http://maps.google.com/maps/api/staticmap?center=Berkeley,CA&zoom=14&size=400x400&sensor=false";

			Assert.AreEqual(expectedResult, generateStaticMapURL);
		}

		[Test]
		public void ZoomLevels()
		{
			StaticMapRequest request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(400, 400));

			string generateStaticMapURL = staticMapGenerator.GenerateStaticMapURL(request);

			string expectedResult =
				@"http://maps.google.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=12&size=400x400&sensor=false";

			Assert.AreEqual(expectedResult, generateStaticMapURL);
		}

		[Test]
		public void ImageSize()
		{
			StaticMapRequest request = new StaticMapRequest(new Location(0, 0), 1, new ImageSize(400, 50));

			string generateStaticMapURL = staticMapGenerator.GenerateStaticMapURL(request);

			string expectedResult =
				@"http://maps.google.com/maps/api/staticmap?center=0,0&zoom=1&size=400x50&sensor=false";

			Assert.AreEqual(expectedResult, generateStaticMapURL);
		}


		[Test]
		public void MapTypes()
		{
			StaticMapRequest request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(400, 400));

			request.MapType = MapType.Terrain;

			string generateStaticMapURL = staticMapGenerator.GenerateStaticMapURL(request);

			string expectedResult =
				@"http://maps.google.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=12&size=400x400&maptype=terrain&sensor=false";

			Assert.AreEqual(expectedResult, generateStaticMapURL);
		}


	}
}