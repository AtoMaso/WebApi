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
    [RoutePrefix("api/suburbs")]
    public class SuburbsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Suburbs
        public IQueryable<Suburb> GetSuburbs()
        {
            return db.Suburbs;
        }

        // GET: api/postcodes?placeId=xx    
        [Route("GetSuburbsByPostcodeId")]
        public IHttpActionResult GetSuburbsByPostcodeId(int postcodeId)
        {
            try
            {
                List<Suburb> list = new List<Suburb>();              
                foreach (Suburb sub in db.Suburbs.Where(subs => subs.postcodeId == postcodeId))
                {
                    Suburb subdto = new Suburb();
                    subdto.id = sub.id;
                    subdto.name = sub.name;
                    subdto.postcodeId = sub.postcodeId;                
                    list.Add(subdto);
                }
                return Ok<List<Suburb>>(list);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all suburbs!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Suburbs/5
        [ResponseType(typeof(Suburb))]
        public async Task<IHttpActionResult> GetSuburb(int id)
        {
            Suburb suburb = await db.Suburbs.FindAsync(id);
            if (suburb == null)
            {
                return NotFound();
            }

            return Ok(suburb);
        }


        // PUT: api/Suburbs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSuburb(int id, Suburb suburb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != suburb.id)
            {
                return BadRequest();
            }

            db.Entry(suburb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuburbExists(id))
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


        // POST: api/Suburbs
        [ResponseType(typeof(Suburb))]        
        public async Task<IHttpActionResult> PostSuburb(Suburb suburb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suburbs.Add(suburb);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = suburb.id }, suburb);
        }


        // DELETE: api/Suburbs/5
        [ResponseType(typeof(Suburb))]      
        public async Task<IHttpActionResult> DeleteSuburb(int id)
        {
            Suburb suburb = await db.Suburbs.FindAsync(id);
            if (suburb == null)
            {
                return NotFound();
            }

            db.Suburbs.Remove(suburb);
            await db.SaveChangesAsync();

            return Ok(suburb);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool SuburbExists(int id)
        {
            return db.Suburbs.Count(e => e.id == id) > 0;
        }
    }
}