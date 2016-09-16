using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Taxjar.Tests
{
	public class TaxjarFixture
	{
		public static string GetJSON(string fixturePath)
		{
			using (StreamReader file = File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../", "Fixtures", fixturePath)))
			using (JsonTextReader reader = new JsonTextReader(file))
			{
				JObject response = (JObject)JToken.ReadFrom(reader);
				return response.ToString();
			}
		}
	}
}

