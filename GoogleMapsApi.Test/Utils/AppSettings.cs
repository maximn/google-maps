using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.Utils
{
    internal class AppSettings
    {
        [JsonProperty(PropertyName ="GOOGLE_API_KEY")]
        public string? GoogleApiKey { get; set; }

        public static AppSettings? Load()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return null;
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
        }
    }
}
