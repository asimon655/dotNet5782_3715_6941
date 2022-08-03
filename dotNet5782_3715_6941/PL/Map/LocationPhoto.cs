using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PL.Map
{
    public class LocationPhoto
    {
        BO.Location photoLocation;
        public BO.Location location { get => photoLocation; }

        string mapLink;

        public string map { get => mapLink; }

         Dictionary<string, string> tokens;
        public int zoomLevel { get; set;  }

        public string mapParsed
        {

            get
            {
                TileLoct tileLoct = location.ToTileLoct(zoomLevel);
                tokens = new Dictionary<string, string> {

                { "{x}", tileLoct.x.ToString() },
                { "{y}", tileLoct.y.ToString()},
                { "{z}",tileLoct.z.ToString() } ,
                { "{s}" , "a" }
                 
            };
                var r = new Regex(string.Join("|", tokens.Keys.Select(Regex.Escape)));
                var me = new MatchEvaluator(m => tokens[m.Value]);

                return r.Replace(mapLink, me); ;
            }

        }
        public LocationPhoto(BO.Location loc, String MapLink)
        {
            this.photoLocation = loc;
            mapLink = MapLink; 
   

        }
                       
        private byte[] downloadPhotosync()
        {

            WebClient client = new WebClient();
            byte[] data = client.DownloadData(mapParsed);
            return data ;
        }

        public  BitmapImage LoadImageSync()
        {
            byte [] imageData = downloadPhotosync();
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }



        public  class TileLoct { //PDS - PASSIVE DATA STRUCTRE 

            public int x;
            public int y;
            public int z; 
        
        }


    }

  
}
