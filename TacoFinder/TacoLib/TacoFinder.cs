using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BingMapsSDSToolkit;
using BingMapsSDSToolkit.QueryAPI;

namespace TacoLib
{
	public struct TacoLocation
	{
		public double Latitude { get; private set; }
		public double Longitude { get; private set; }
		public string Name { get; private set; }

		public TacoLocation (double latitude, double longitude, string name)
		{
			Latitude = latitude;
			Longitude = longitude;
			Name = name;
		}
	}

	public partial class TacoFinder
	{
		// Almost any >NET code can be shared between iOS and Mac
		// This example uses https://github.com/Microsoft/BingMapsSDSToolkit
		// to find locations that may serve taco's within 50 miles of your
		// current location, but this could be any business-specific logic

		BasicDataSourceInfo DataSource;
		public TacoFinder ()
		{
			DataSource = new BasicDataSourceInfo ()
			{
				QueryUrl = "http://spatial.virtualearth.net/REST/v1/data/f22876ec257b474b82fe2ffcb8393150/NavteqNA/NavteqPOIs",
				QueryKey = API_KEY
			};
		}

		static readonly string [] TacoWordList = { "taco", "mex", "cactus", "ranch", "spanish", "chili", "chile"};
		bool LikelyHashTaco (string name)
		{
			// This is obviously insufficient for more than a simple sample
			foreach (var word in TacoWordList)
				if (name.ToLower().Contains (word))
					return true;
			return false;
		}

		public async Task<List<TacoLocation>> Find (double latitude, double longitude)
		{
			var request = new FindNearByRequest (DataSource)
			{
				Distance = 5,
				DistanceUnits = DistanceUnitType.Miles,
				Center = new GeodataLocation (latitude, longitude),
				Top = 250
			};

			// Work around https://github.com/Microsoft/BingMapsSDSToolkit/pull/1
			var expression = new FilterExpression ("EntityTypeID", LogicalOperator.IsIn, null);
			expression.Value = new List<string> () { "5800" }; // 5800 = Restaurant
			request.Filter = expression;

			var response = await QueryManager.ProcessQuery (request);

			List<TacoLocation> locations = new List<TacoLocation> (response.Results.Count);
			foreach (var result in response.Results.Where (x => x.Properties.ContainsKey ("Name")))
			{
				string name = (string)result.Properties ["Name"];
				if (LikelyHashTaco (name))
					locations.Add (new TacoLocation (result.Location.Latitude, result.Location.Longitude, name));
			}
			return locations;
		}
	}
}
