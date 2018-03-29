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
    [RoutePrefix("api/postcodes")]
    public class PostcodesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SuburbsController subctr = new SuburbsController();

        // GET: api/Postcodes
        [AllowAnonymous]
        public IQueryable<Postcode> GetPostcodes()
        {
            return db.Postcodes;
        }


        // GET: api/postcodes/GetPostcodesByPlaceId?placeId=xx   
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
                    pcdto.suburbs = ((OkNegotiatedContentResult<List<Suburb>>)subctr.GetSuburbsByPostcodeId(pc.id)).Content;
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
        [AllowAnonymous]
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


        // PUT: api/postcodes/PutPostcode?id=1  
        [ResponseType(typeof(void))] 
        [Route("PutPostcode")]
        public async Task<IHttpActionResult> PutPostcode(int id, Postcode postcode)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The postcode details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != postcode.id)
            {
                ModelState.AddModelError("Message", "The postcode id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Postcode not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            Postcode cat = await db.Postcodes.Where(cor => cor.id == id).FirstAsync();
            return Ok<Postcode>(cat);
        }


        // POST: api/Postcodes/PostPostcode       
        [ResponseType(typeof(Postcode))]      
        [Route("PostPostcode")]
        public async Task<IHttpActionResult> PostPostcode([FromBody] Postcode postcode)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The postcode details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                db.Postcodes.Add(postcode);
                await db.SaveChangesAsync();

                Postcode lastpc = await db.Postcodes.OrderByDescending(pc => pc.id).FirstAsync();

                // add the dummy suburb so the app does not fail
                Suburb sub = new Suburb();
                sub.postcodeId = lastpc.id;
                sub.name = "Miscellaneous";                         
                await subctr.PostSuburb(sub);

                return Ok<Postcode>(lastpc); ;
            }
            catch (Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your postcode!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/Postcodes/DeletePostcode?id=1
        [ResponseType(typeof(Postcode))]
        [Route("DeletePostcode")]
        public async Task<IHttpActionResult> DeletePostcode(int id)
        {
            Postcode postcode = await db.Postcodes.FindAsync(id);
            if (postcode == null)
            {
                ModelState.AddModelError("Message", "Postcode could not be found!");
                return BadRequest(ModelState);
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