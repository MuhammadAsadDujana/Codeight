using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BOL.Helper
{
    public static class Images
    {
        public static string byteArrayToImage(byte[] bytesArr, HttpServerUtilityBase Server)
        {
            var ticks = DateTime.Now.Ticks;
            var image_location = Server.MapPath("~/Content/profile_pictures/") + ticks;

            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                using (Image image = Image.FromStream(memstr))
                {
                    string ext = new ImageFormatConverter().ConvertToString(image.RawFormat);

                    var thumbImg = new Bitmap(image.Width, image.Height);
                    var thumbGraph = Graphics.FromImage(thumbImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var ImageRectangle = new Rectangle(0, 0, image.Width, image.Height);
                    thumbGraph.DrawImage(image, ImageRectangle);

                    image_location = image_location + ext;
                    thumbImg.Save(image_location, image.RawFormat);
                }
            }
            return image_location;
        }
        public static byte[] ImageToByteArray(string file_location)
        {
            using (System.Drawing.Image _image = System.Drawing.Image.FromFile(file_location))
            {
                using (MemoryStream _mStream = new MemoryStream())
                {
                    _image.Save(_mStream, _image.RawFormat);
                    byte[] _imageBytes = _mStream.ToArray();

                    return _imageBytes;
                }
            }
        }


        public static string Base64ToImage(string base64String, HttpServerUtilityBase Server)
        {
            if (string.IsNullOrEmpty(base64String))
                return "";
            try
            {
                var ticks = DateTime.Now.Ticks;
                var location = "content/profile_pictures/" + ticks + ".jpg";
                var filepath = Server.MapPath("~/" + location);

                base64String = base64String.Replace("data:image/jpeg;base64,", string.Empty).Replace("data:image/jpg;base64,", string.Empty).Replace("data:image/png;base64,", string.Empty);

                //Image image;
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64String)))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        //string ext = new ImageFormatConverter().ConvertToString(image.RawFormat);

                        var thumbImg = new Bitmap(image.Width, image.Height);
                        var thumbGraph = Graphics.FromImage(thumbImg);
                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var ImageRectangle = new Rectangle(0, 0, image.Width, image.Height);
                        thumbGraph.DrawImage(image, ImageRectangle);

                        thumbImg.Save(filepath, image.RawFormat);
                    }
                }

                var image_url = System.Configuration.ConfigurationManager.AppSettings["liveUrl"] + location;
           //   image_url = TripleDESCryptography.Encrypt(image_url);  // code for encryption
                return image_url;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string ImageToBase64(string file_location)
        {
            string _base64String = null;

            using (System.Drawing.Image _image = System.Drawing.Image.FromFile(file_location))
            {
                using (MemoryStream _mStream = new MemoryStream())
                {
                    _image.Save(_mStream, _image.RawFormat);
                    byte[] _imageBytes = _mStream.ToArray();
                    _base64String = Convert.ToBase64String(_imageBytes);

                    return "data:image/jpeg;base64," + _base64String;
                }
            }
        }

        public static bool IsBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0 || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return false;
        }
    }

}
