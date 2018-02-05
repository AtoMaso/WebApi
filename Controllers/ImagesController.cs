using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ImagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/images
        public IHttpActionResult GetImages()
        {
            try
            {
                List<ImageDTO> dtoList = new List<ImageDTO>();
                foreach (Image image in db.Images)
                {
                    ImageDTO imgdto = new ImageDTO();

                    imgdto.imageId = image.imageId;
                    imgdto.imageTitle = image.imageTitle;
                    imgdto.imageUrl = image.imageUrl;
                    imgdto.tradeId = image.tradeId;

                    dtoList.Add(imgdto);
                }
                return Ok<List<ImageDTO>>(dtoList);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting image details!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/images?tradeId = 5  -- by tradeId to be used when is called from the trade controller
        public IHttpActionResult GetImagesByTradeId(int tradeId)
        {
            try
            {
                List<ImageDTO> dtoList = new List<ImageDTO>();
                foreach (Image image in db.Images)
                {
                    if(image.tradeId == tradeId)
                    {
                        ImageDTO imgdto = new ImageDTO();

                        imgdto.imageId = image.imageId;
                        imgdto.imageTitle = image.imageTitle;
                        imgdto.imageUrl = image.imageUrl;
                        imgdto.tradeId = image.tradeId;

                        dtoList.Add(imgdto);
                    }                  
                }
                return Ok<List<ImageDTO>>(dtoList);
            }
            catch (Exception exc)
            {            
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting image details!");
                return BadRequest(ModelState);
            }
        }

        // GET: api/images/5
        [ResponseType(typeof(ImageDTO))]
        public async Task<IHttpActionResult> GetImage(int id)
        {
            Image image = await db.Images.FindAsync(id);          
            if (image == null)
            {
                return NotFound();
            }

            try
            {
                ImageDTO imgdto = new ImageDTO();

                imgdto.imageId = image.imageId;
                imgdto.imageTitle = image.imageTitle;
                imgdto.imageUrl = image.imageUrl;
                imgdto.tradeId = image.tradeId;              

                return Ok(imgdto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone details!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/images/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutImage(int id, Image Image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Image.imageId)
            {
                return BadRequest();
            }

            db.Entry(Image).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/images
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> PostImage(Image Image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(Image);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Image.imageId }, Image);
        }

        // DELETE: api/imagees/5
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> DeleteImage(int id)
        {
            Image Image = await db.Images.FindAsync(id);
            if (Image == null)
            {
                return NotFound();
            }

            db.Images.Remove(Image);
            await db.SaveChangesAsync();

            return Ok(Image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.imageId == id) > 0;
        }
    }
}
