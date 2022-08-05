using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Map
{
    [Serializable]
    internal  class MapsDBObject // PDS
    {
        [JsonProperty("maps")]
        public IEnumerable<MapObject> maps { set; get;  }
        [JsonProperty("mainLink")]
        public string mainLink { set; get; }
        [JsonProperty("defaultMap")]
        public string defaultMap { set; get; }

        [Serializable]
        public class MapObject {
            [JsonProperty("name")]
            public string name { set; get; }
            [JsonProperty("link")]
            public string link { set; get; }
            [JsonProperty("description")]
            public string description { set; get; }
            [JsonProperty("bgColor")]
            public string bgColor { set; get; }
            [JsonProperty("heavy")]
            public bool heavy { set; get; }

            public override bool Equals(object obj)
            {
                return obj is MapObject @object &&
                       name == @object.name &&
                       link == @object.link &&
                       description == @object.description &&
                       bgColor == @object.bgColor;
            }
        }

    }
}
