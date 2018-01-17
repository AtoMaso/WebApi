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

        // GET: api/Imagees
        public IQueryable<Image> GetImages()
        {
            return db.Images;
        }

        // GET: api/Imagees/5
        [ResponseType(typeof(Image))]
        public async Task<IHttpActionResult> GetImage(int id)
        {
            Image Image = await db.Images.FindAsync(id);
            if (Image == null)
            {
                return NotFound();
            }

            return Ok(Image);
        }

        // PUT: api/Images/5
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

        // POST: api/Images
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

        // DELETE: api/Imagees/5
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
