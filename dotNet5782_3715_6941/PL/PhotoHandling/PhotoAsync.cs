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
        static internal string TMP = System.IO.Path.GetTempPath();
        internal const  string fileEnd = ".png";
        static internal readonly ImageFormat fileEndEnum =  ImageFormat.Png;
        const string RecognationName = @"image";
        static internal readonly string fullPathHeadder = TMP + RecognationName;
        static internal readonly string fullPathTail = fileEnd;
        internal static string makePath<T>(T obj) => fullPathHeadder + obj.ToString() + fullPathTail; 

        #endregion
        static internal async Task<bool> SaveImageAsync(string imageUrl, string filename, ImageFormat format, String? FileOther = null)
        {

            WebClient client = new WebClient();
            try
            {
                
                Stream stream = await client.OpenReadTaskAsync(imageUrl);

                    Bitmap bitmap; bitmap = new Bitmap(stream);

                    if (bitmap != null)
                    {
                    await Task.Run(() => bitmap.Save(filename, format) ) ;
                    }

                stream.Flush();
                stream.Close();
                client.Dispose();
                if (!(FileOther is null))
                {
                    if (AreEqule(filename, FileOther))
                    {
                      await  Task.Run(() => File.Delete(filename) );
                      await SaveImageAsync(imageUrl, filename, format, FileOther);
               
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
        

        #endregion
    }
}
