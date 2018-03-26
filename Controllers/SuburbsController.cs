using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.Web.Http.Results;

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


        // PUT: api/Suburbs/PutSuburb/?id=1
       
        [ResponseType(typeof(void))]
        [Route("PutSuburb")]
        public async Task<IHttpActionResult> PutSuburb(int id, Suburb suburb)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The suburb details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != suburb.id)
            {
                ModelState.AddModelError("Message", "The suburb id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Suburb not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            Suburb cat = await db.Suburbs.Where(cor => cor.id == id).FirstAsync();
            return Ok<Suburb>(cat);
        }


        // POST: api/Suburbs/PostSuburb?id=1
       
        [ResponseType(typeof(Suburb))]         
        [Route("PostSuburb")]   
        public async Task<IHttpActionResult> PostSuburb([FromBody] Suburb suburb)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The sububb details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                db.Suburbs.Add(suburb);
                await db.SaveChangesAsync();

                Suburb lastsub = await db.Suburbs.OrderByDescending(sub => sub.id).FirstAsync();          
                return Ok<Suburb>(lastsub); 
            }
            catch (Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your suburb!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/Suburbs/DeleteSuburb?id=1
        [ResponseType(typeof(Suburb))]
        [Route("DeleteSuburb")]
        public async Task<IHttpActionResult> DeleteSuburb(int id)
        {
            Suburb suburb = await db.Suburbs.FindAsync(id);
            if (suburb == null)
            {
                ModelState.AddModelError("Message", "Suburb could not be found!");
                return BadRequest(ModelState);
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