using BruTile.Predefined;
using BruTile.Web;
using Mapsui.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Map
{
    internal static  class ExestionsionMap
    {
  
       

        static private MapsDBObject mapDBObject = null;

        static public async Task<MapsDBObject> getMapDBObject() { // singleton async 
            if (mapDBObject is null) {
                mapDBObject = await loadJsonAsyncFactory();
                if (mapDBObject.maps.Count() == 0) {
                    throw new Exception("cant load Map DB"); 
                }
            }
            return mapDBObject; 
        
        
        } 

        private  static  async Task<MapsDBObject> loadJsonAsyncFactory()  { // factory method 
            var baseAdress = AppDomain.CurrentDomain.BaseDirectory; 
            var mapDBLocation = @"\MapsDB\maps.json";
            var jsonString = await File.ReadAllTextAsync(baseAdress + mapDBLocation);
            var desriliaze =  JsonSerializer.Deserialize<MapsDBObject>(jsonString);
            return desriliaze; 
        }

        public static  async Task TaskLoadTheme(this Mapsui.IMap map  , Action<object> callBack , string ?  mapName = null )
        {
            MapsDBObject mapObj =await getMapDBObject();
            if(map.Layers.Any(x => x.Name == "BaseLayer"))
                map.Layers.Remove(map.Layers.First(layer => layer.Name == "BaseLayer"));
            MapsDBObject.MapObject mapObjtheme = null;
            if (mapName is null)
                mapObjtheme = mapObj.maps.First(); 
            else
                mapObjtheme = mapObj.maps.First(x => x.name == mapName);
            var tileSource = new HttpTileSource(new GlobalSphericalMercator(), mapObjtheme.link, new[] { "a", "b", "c" }, name: "OpenStreetMap", userAgent: "OpenStreetMap in Mapsui");
            var tileLayer = new TileLayer(tileSource)
            {
                Name = "BaseLayer",
            };
            map.Layers.Insert(0,tileLayer);
            map.BackColor = Mapsui.Styles.Color.FromString(mapObjtheme.bgColor);
            callBack(mapObjtheme);

        }






    }





}
