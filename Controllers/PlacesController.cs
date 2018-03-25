using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.IO;
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PostcodesController pcctr = new PostcodesController();

        // GET: api/Places
        public IQueryable<Place> GetPlaces()
        {
            return db.Places;
        }



        // GET: api/Places?stateId=xx     
        [Route("GetPlacesByStateId")]
        public IHttpActionResult GetPlacesByStateId(int stateId)
        {
            try
            {
                List<Place> list = new List<Place>();
                foreach (Place pl in db.Places.Where(pl => pl.stateId == stateId).OrderBy(pl => pl.name))
                {
                    Place pldto = new Place();
                    pldto.id = pl.id;
                    pldto.name = pl.name;
                    pldto.stateId = pl.stateId;             
                    pldto.postcodes = ((OkNegotiatedContentResult<List<Postcode>>)pcctr.GetPostcodesByPlaceId(pl.id)).Content;
                    list.Add(pldto);
                }
                return Ok<List<Place>>(list);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the places!");
                return BadRequest(ModelState);
            }       
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