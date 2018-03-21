using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;


namespace WebApi.Controllers
{
    [RoutePrefix("api/uploads")]
    public class UploadsController : ApiController
    {

        /// <summary>
        /// This is a mehotd to upload a file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadFile()
        {
            HttpResponseMessage result = null;

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string root = HttpContext.Current.Server.MapPath("~/Uploads");
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    // to be stored in database
                    Char[] separators = { '_' };
                    // get the posted file as an array of bytes
                    var postedFile = httpRequest.Files[file];
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }
                    // Resize the file to a smaller size, pass the array of bytes
                    var image = resizeImage(300, 300, fileData);

                    // prepare the file path
                    string[] foldername = postedFile.FileName.Split(separators);
                    string filePathImages = Path.GetFullPath(Path.Combine(root + "/images/"));
                    System.IO.DirectoryInfo dir = new DirectoryInfo(filePathImages);
                    dir.CreateSubdirectory(foldername[0]);
                    string filePath = Path.GetFullPath(Path.Combine(root + "/images/" + foldername[0] + "/", postedFile.FileName));

                    // write the new resized image to the folder
                    System.IO.File.WriteAllBytes(filePath, image);
                    //postedFile.SaveAs(filePath);             

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }


        public byte[] resizeImage(int newWidth, int newHeight, byte[] imagefile)
        {
            Image imgPhoto = Image.FromStream(new MemoryStream(imagefile));

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            Bitmap bmPhoto = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;// HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            MemoryStream mm = new MemoryStream();
            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);

            grPhoto.Dispose();
            imgPhoto.Dispose();

            return mm.GetBuffer();
            //return bmPhoto;
        }


        /// <summary>
        /// This is method to download file 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DownLoadFile(string FileName, string fileType)
        {
            Byte[] bytes = null;
            if (FileName != null)
            {
                string filePath = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), FileName));
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            System.IO.MemoryStream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(fileType);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = FileName };
            return (result);
        }


    }

}

