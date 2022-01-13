using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PL
{
    internal static class PhotoAsync
    {
        #region PhotoAsunc Functions 
        #region Constans 
        internal const string FaceAIURL = "https://thispersondoesnotexist.com/image";
        internal const string GooglePhotosHeader = @"https://www.google.com/search?q=";
        internal const string GoooglePhotosTail = @"&tbm=isch&hl=iw&tbs=ic:trans&sa=X&ved=0CAMQpwVqFwoTCKjYjPC9qvUCFQAAAAAdAAAAABAI&biw=1522&bih=769";
        internal static string TMP = System.IO.Path.GetTempPath();
        internal const string fileEnd = ".png";
        internal static readonly ImageFormat fileEndEnum = ImageFormat.Png;
        private const string RecognationName = @"image";
        private static readonly string Plus = "%20";
        internal static readonly string fullPathHeadder = TMP + RecognationName;
        internal static readonly string fullPathTail = fileEnd;
        internal static readonly string SafeWord = "Drone";
        internal static string makePath<T>(T obj)
        {
            return fullPathHeadder + obj.ToString().Replace(" ", "_") + fullPathTail;
        }

        internal static string makePathDrnSearch<T>(T obj)
        {
            return GooglePhotosHeader + obj.ToString().Replace(" ", Plus) + Plus + SafeWord + GoooglePhotosTail;
        }

        #endregion
        #region Costumers 
        internal static async Task<bool> SaveImageAsync(string imageUrl, string filename, ImageFormat format, string? FileOther = null , int hm =0 )
        {
            if (hm > 5)
                return false;
            if (CriticalSection.Count((x) => x == imageUrl) == 0)
            {
                CriticalSection.Add(imageUrl);
                WebClient client = new WebClient();
                try
                {


                    await client.DownloadFileTaskAsync(imageUrl, filename);
                    client.Dispose();

                    client.Dispose();
                    if (!(FileOther is null))
                    {
                        if (await AreEqule(filename, FileOther))
                        {
                            try
                            {
                                await Task.Delay(50);
                                 File.Delete(filename);
                            }
                            catch {
                                CriticalSection.Remove(imageUrl);
                                return false;
                            }
                            CriticalSection.Remove(imageUrl);
                            return await SaveImageAsync(imageUrl, filename, format, FileOther , ++ hm );
                        }

                    }
                    CriticalSection.Remove(imageUrl);
                }
                catch
                {
                    CriticalSection.Remove(imageUrl);
                    return false;
                }
                return true;
            }
            else
            {
                int hmi = 0; 
                while (await Task.Run(() => CriticalSection.Count((x) => x == imageUrl) != 0) && hmi++ < 5)
                {
                    await Task.Delay(10);


                }
                if (hmi == 6)
                    return false;
                return true;
            }
        }
        private static async Task<bool> AreEqule(string filepath1, string filepath2)
        {
            FileInfo first = new FileInfo(filepath1);
            FileInfo second = new FileInfo(filepath2);
            if (first.Length != second.Length)
            {
                return false;
            }

            if (second.Length == 0 || first.Length == 0)
            {
                return true;
            }

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                HMACSHA1 h1 = new HMACSHA1();

                byte[] hash1 = await h1.ComputeHashAsync(fs1);
                byte[] hash2 = await h1.ComputeHashAsync(fs2);
                for (int i = 0; i < hash2.Length; i++)
                {
                    if (hash1[i] != hash2[i])
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        #endregion#region Drones
        private static readonly List<string> CriticalSection = new List<string>();
        #region Drones
        internal static async Task<bool> SaveFirstImageAsync(string Model)
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();

            try
            {
                // Setting the URL, then downloading the data from the URL.
                string source = await x.DownloadStringTaskAsync(new Uri(makePathDrnSearch(Model)));
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                // Loading document's source via HtmlAgilityPack
                await Task.Run(() => document.LoadHtml(source));
                var nodes = document.DocumentNode.SelectNodes("//img[@src]");
                string link = await Task.Run(() => nodes == null ? "None" : nodes.SelectMany(j => j.Attributes).First(x => x.Value.Contains("http")).Value);
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
