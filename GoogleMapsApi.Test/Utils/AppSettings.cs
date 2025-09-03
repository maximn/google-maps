using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.Utils
{
    internal class AppSettings
    {
        [JsonPropertyName("GOOGLE_API_KEY")]
        public string? GoogleApiKey { get; set; }

        public static AppSettings? Load()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return null;
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path));
        }
    }
}
