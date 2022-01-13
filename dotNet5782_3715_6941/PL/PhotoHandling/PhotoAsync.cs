using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PL
{
    internal static  class PhotoAsync
    {
        #region PhotoAsunc Functions 
        #region Constans 
        internal const string FaceAIURL = "https://thispersondoesnotexist.com/image";
        internal const string GooglePhotosHeader = @"https://www.google.com/search?q=";
        internal const string GoooglePhotosTail = @"&tbm=isch&hl=iw&tbs=ic:trans&sa=X&ved=0CAMQpwVqFwoTCKjYjPC9qvUCFQAAAAAdAAAAABAI&biw=1522&bih=769";
        static internal string TMP = System.IO.Path.GetTempPath();
        internal const  string fileEnd = ".png";
        static internal readonly ImageFormat fileEndEnum =  ImageFormat.Png;
        const string RecognationName = @"image";
        static private string Plus = "%20";
        static internal readonly string fullPathHeadder = TMP + RecognationName;
        static internal readonly string fullPathTail = fileEnd;
        static internal readonly string SafeWord = "Drone";
        internal static string makePath<T>(T obj) => fullPathHeadder + obj.ToString().Replace(" ", "_") + fullPathTail;
        internal static string makePathDrnSearch<T>(T obj) => GooglePhotosHeader + obj.ToString().Replace(" ",Plus)+Plus + SafeWord + GoooglePhotosTail;

        #endregion
        #region Costumers 
        static internal async Task<bool> SaveImageAsync(string imageUrl, string filename, ImageFormat format, String? FileOther = null)
        {

            WebClient client = new WebClient();
            try
            {


                await client.DownloadFileTaskAsync(imageUrl, filename);
                client.Dispose();

                client.Dispose();
                if (!(FileOther is null))
                {
                    if (AreEqule(filename, FileOther))
                    {
                        await Task.Run(() => File.Delete(filename));
                        
                        return await SaveImageAsync(imageUrl, filename, format, FileOther);
                    }

                }
            }
            catch
            {


                return false;
            }
            return true;
        }
        static private bool AreEqule(string filepath1, string filepath2)
        {
            FileInfo first = new FileInfo(filepath1);
            FileInfo second = new FileInfo(filepath2);
            if (second.Length == 0 || first.Length == 0)
                return true;
            if ((first.CreationTime-second.CreationTime   ).Milliseconds < 400)
                return true;
            if (first.Length != second.Length)
                return false;
            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                HMACSHA384 h1 = new HMACSHA384();

                byte[] hash1 = h1.ComputeHash(fs1);
                byte[] hash2 = h1.ComputeHash(fs2);
                for (int i = 0; i < hash2.Length; i++)
                    if (hash1[i] != hash2[i])
                        return false;

            }

            return true;

        }

        #endregion#region Drones
        #region Drones
        static internal async Task<bool> SaveFirstImageAsync(string Model)
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();
            x.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
            try
            {
                // Setting the URL, then downloading the data from the URL.
                string source = await x.DownloadStringTaskAsync(new Uri(makePathDrnSearch(Model)));
                //HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                //// Loading document's source via HtmlAgilityPack
                //await Task.Run(() => document.LoadHtml(source));
                //var nodes = document.DocumentNode.SelectNodes("//img[@src]");
                //string link = await Task.Run(() => nodes == null ? "None" : nodes.SelectMany(j => j.Attributes).First(x => x.Value.Contains("http")).Value);
                int hash_place = source.IndexOf("data-tbnid=\"") + "data-tbnid=\"".Length;

                string hash = source.Substring(hash_place, 14);

                int link_place = 0;

                link_place = source.IndexOf(hash, link_place) + hash.Length;
                link_place = source.IndexOf(hash, link_place) + hash.Length;
                link_place = source.IndexOf(hash, link_place) + hash.Length;
                link_place = source.IndexOf(hash, link_place) + hash.Length;

                link_place = source.IndexOf('[', link_place) + 1;
                link_place = source.IndexOf('[', link_place) + 2;

                int link_end = source.IndexOf('"', link_place);

                string link = source.Substring(link_place, link_end - link_place);

                return await SaveImageAsync(link, makePath(Model), fileEndEnum);
            }
            catch
            {
            
                return false;
            }
            // Declaring 'document' as new HtmlAgilityPack() method

           

        }
        #endregion
        #endregion



    }
}
