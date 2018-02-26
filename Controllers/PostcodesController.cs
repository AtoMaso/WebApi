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
    [Authorize]
    [RoutePrefix("api/postcodes")]
    public class PostcodesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Postcodes
        public IQueryable<Postcode> GetPostcodes()
        {
            return db.Postcodes;
        }



        // GET: api/postcodes?placeId=xx
        [AllowAnonymous]
        [Route("GetPostcodesByPlaceId")]
        public IHttpActionResult GetPostcodesByPlaceId(int placeId)
        {
            try
            {
                List<Postcode> list = new List<Postcode>();
                foreach (Postcode pc in db.Postcodes.Where(pc => pc.placeId == placeId))
                {
                    Postcode pcdto = new Postcode();
                    pcdto.id = pc.id;
                    pcdto.number = pc.number;
                    pcdto.placeId = pc.placeId;                   
                    list.Add(pcdto);
                }
                return Ok<List<Postcode>>(list);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all postcodes!");
                return BadRequest(ModelState);
            }
        }



        // GET: api/postcodes/5
        [ResponseType(typeof(Postcode))]
        public async Task<IHttpActionResult> GetPostcode(int id)
        {
            Postcode postcode = await db.Postcodes.FindAsync(id);
            if (postcode == null)
            {
                return NotFound();
            }

            return Ok(postcode);
        }



        // PUT: api/postcodes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPostcode(int id, Postcode postcode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != postcode.id)
            {
                return BadRequest();
            }

            db.Entry(postcode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostcodeExists(id))
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

        // POST: api/Postcodes
        [ResponseType(typeof(Postcode))]
        public async Task<IHttpActionResult> PostPostcode(Postcode postcode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Postcodes.Add(postcode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = postcode.id }, postcode);
        }

        // DELETE: api/Postcodes/5
        [ResponseType(typeof(Postcode))]
        public async Task<IHttpActionResult> DeletePostcode(int id)
        {
            Postcode postcode = await db.Postcodes.FindAsync(id);
            if (postcode == null)
            {
                return NotFound();
            }

            db.Postcodes.Remove(postcode);
            await db.SaveChangesAsync();

            return Ok(postcode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostcodeExists(int id)
        {
            return db.Postcodes.Count(e => e.id == id) > 0;
        }
    }
}