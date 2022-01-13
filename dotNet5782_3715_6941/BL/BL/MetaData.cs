using BO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        internal static string TMP = Path.GetTempPath();
        internal static async Task<DO.DronePic> SaveDroneFirstImage(string Model)
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();

            try
            {
                // Setting the URL, then downloading the data from the URL.
                string source = await x.DownloadStringTaskAsync(@"https://www.google.com/search?q=" + Model.Replace(" ", "+") + @"+drone&tbm=isch&ved=2ahUKEwj2__aHx8P0AhVNwoUKHWOxAyMQ2-cCegQIABAA&oq=mavic+3+cine&gs_lcp=CgNpbWcQAzIHCCMQ7wMQJzIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGFCPDliPDmDuD2gAcAB4AIABhQGIAfoBkgEDMC4ymAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=cOqnYfaHCc2ElwTj4o6YAg&bih=596&biw=1229");
                // Declaring 'document' as new HtmlAgilityPack() method
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

                // Loading document's source via HtmlAgilityPack
                document.LoadHtml(source);
                var nodes = document.DocumentNode.SelectNodes("//img[@src]");
                string link = nodes == null ? "None" : nodes.SelectMany(j => j.Attributes).First(x => x.Value.Contains("http")).Value;

                string filename = TMP + @"image" + Model.Replace(" ", "_") + ".png";
                await SaveImage(link, filename, ImageFormat.Png);

                return new DO.DronePic { Model = Model, Path = filename };
            }
            catch
            {
                throw new NoOrBadInternet("please connect to unfiltered intenet");
            }
        }
        internal static async Task SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            try
            {
                Stream stream = await client.OpenReadTaskAsync(new Uri(imageUrl));

                Bitmap bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(filename, format);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            catch
            {
                throw new NoOrBadInternet("please connect to unfiltered intenet ");
            }
        }

        public async Task<string> GetRandomPersonPic()
        {
            string filepath = TMP + Guid.NewGuid() + ".png";
            await SaveImage("https://thispersondoesnotexist.com/image", filepath, ImageFormat.Png);
            return filepath;
        }
        public async Task<string> GetDronePic(string Model)
        {
            DO.DronePic pic;
            try
            {
                pic = data.GetDronePic(Model);
            }
            catch (DO.IdDosntExists)
            {
                pic = await SaveDroneFirstImage(Model);

                data.AddDronePic(pic);
            }
            return pic.Path;
        }
        public string GetCustomerPic(int customerId)
        {
            DO.CustomerPic pic;
            try
            {
                pic = data.GetCustomerPic(customerId);
                return pic.Path;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomerPic(int customerId, string filepath)
        {
            try
            {
                data.AddCustomerPic(new DO.CustomerPic { Id = customerId, Path = filepath });
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            }
        }
        public async Task<List<string>> GetCapchaQuestion()
        {
            WebClient client = new WebClient();
            try
            {
                Stream stream = await client.OpenReadTaskAsync("http://api.textcaptcha.com/idangolo123@gmail.com.xml");
                StreamReader reader = await Task.Run(() => new StreamReader(stream));
                string plaintext = reader.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                List<string> QuestionAndAnswers = new List<string>();
                await Task.Run(() => doc.LoadXml(plaintext));
                foreach (XmlNode Element in doc.DocumentElement.ChildNodes)
                {
                    QuestionAndAnswers.Add(Element.InnerText);
                }

                return QuestionAndAnswers;
            }
            catch
            {
                throw new NoOrBadInternet("Ethenet connection is filterd (by rimon or net spark) or weak ");
            }
        }
    }
}