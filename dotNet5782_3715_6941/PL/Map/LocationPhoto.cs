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
        public string map { get; set;  }
        public int zoomLevel { get; set;  }
        public LocationPhoto(BO.Location loc, String MapLink)
        {
            this.photoLocation = loc;
            map= MapLink; 
        }                   
        public  async Task<BitmapImage> loadImageAsync()
        {
            byte [] imageData =  await photoLocation.getLoactionPhoto(map,zoomLevel);
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
    }

  
}
