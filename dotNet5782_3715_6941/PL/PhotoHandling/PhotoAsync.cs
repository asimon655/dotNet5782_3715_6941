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
        internal const string FaceAIURL = "https://thispersondoesnotexist.com/image"; //The Ai that creats the peoples photos 
        internal const string GooglePhotosHeader = "https://www.google.com/search?q=";
        internal const string GoooglePhotosTail = "&tbm=isch&hl=iw&tbs=ic:trans&sa=X&ved=0CAMQpwVqFwoTCKjYjPC9qvUCFQAAAAAdAAAAABAI&biw=1522&bih=769";
        internal static string TMP = System.IO.Path.GetTempPath();
        internal const string fileEnd = ".png";
        internal static readonly ImageFormat fileEndEnum = ImageFormat.Png;
        private const string RecognationName = @"image";
        private const string Plus = "%20";
        internal static readonly string fullPathHeadder = TMP + RecognationName;
        internal static readonly string fullPathTail = fileEnd;
        internal const string SafeWord = "Drone"; //Prevent you from getting wird photos if you put vird Models Name (like doge,lion,chair ... ) 
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
        internal static async Task<bool> tryDelte(string FilePath , int RecCount =0 )
        {
            try
            {
                await Task.Delay(200);
                File.Delete(FilePath); //Delte the file 
                return true;
            }
            catch  {
                if (RecCount >= 10)
                    return false;
                return await   tryDelte(FilePath, ++RecCount);


            }

        }
        internal static async Task<bool> SaveImageAsync(string imageUrl, string filename, ImageFormat format, string? FileOther = null , int hm =0 )
        {
            if (hm > 5) // limit recursion depth to 5 
                return false; 
            if (CriticalSection.Count((x) => x == imageUrl) == 0) //check if the Critical Section is free
            {
                CriticalSection.Add(imageUrl); //lock Upgraded Critical Section
                WebClient client = new WebClient();
                try
                {


                    await client.DownloadFileTaskAsync(imageUrl, filename); // Save The File Async 
                    client.Dispose(); //closes WebClient
                    if (!(FileOther is null)) //checks if the 2th cilet photo downlad to prevent from downloading twice 
                    {
                        if (await AreEqule(filename, FileOther)) // Compare between the files 
                        {
                            if (!await tryDelte(filename))
                            {
                                //cant Delte 
                                CriticalSection.Remove(imageUrl); // free the critical Section 
                                return false; // the program didnt success
                            }

                            CriticalSection.Remove(imageUrl);
                            await Task.Delay(300);
                            return await SaveImageAsync(imageUrl, filename, format, FileOther, ++hm);
                        }

                    }
                    CriticalSection.Remove(imageUrl);
                }
                catch
                {
                    CriticalSection.Remove(imageUrl);
                    return false; // the program didnt success
                }
                return true;
            }
            else
            {
                int hmi = 0;
                    try
                {
                    while ( CriticalSection.Count((x) => x == imageUrl) != 0 && hmi++ < 10)
                    {
                        await Task.Delay(500);


                    }
                    if (hmi >= 10)
                    {
                        CriticalSection.Remove(imageUrl);
                        return false;
                    }
                }
                catch
                {
                    CriticalSection.Remove(imageUrl);
                    return false;
                }
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
                HMACSHA1 h1 = new HMACSHA1();// uses Sha1 to compare between the files - Async 

                Task<byte[]> hash1 = h1.ComputeHashAsync(fs1);
                Task<byte[]> hash2 = h1.ComputeHashAsync(fs2);

                await Task.WhenAll(hash1, hash2);

                if (hash1.Result != hash2.Result)
                {
                    return false;
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
                var nodes = document.DocumentNode.SelectNodes("//img[@src]"); // search form image and inside her for src(HTML)
                string link = await Task.Run(() => nodes == null ? "None" : nodes.SelectMany(j => j.Attributes).First(x => x.Value.Contains("http")).Value);//gets the first valid link
                return await SaveImageAsync(link, makePath(Model), fileEndEnum); // uses SaveImageAsync To save him
            }
            catch
            {

                return false;
            }




        }
        #endregion
        #endregion



    }
}
