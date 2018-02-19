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
    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
     
        // GET: api/Places
        public IQueryable<Place> GetPlaces()
        {
            return db.Places;
        }



        // GET: api/Places?stateId=xx
        [AllowAnonymous]
        [Route("GetPlacesByStateId")]
        public IQueryable<Place> GetPlacesByStateId(int stateId)
        {
            return db.Places.Where(pl => pl.stateId == stateId);
        }


        // GET: api/Places/5      
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> GetPlace(int id)
        {
            Place place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        // PUT: api/Places/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.id)
            {
                return BadRequest();
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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

        // POST: api/Places
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Places.Add(place);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = place.id }, place);
        }

        // DELETE: api/Places/5
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> DeletePlace(int id)
        {
            Place place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            db.Places.Remove(place);
            await db.SaveChangesAsync();

            return Ok(place);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaceExists(int id)
        {
            return db.Places.Count(e => e.id == id) > 0;
        }
    }
}