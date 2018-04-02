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
    [RoutePrefix("api/statesplacespostcodessuburbs")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class StatesPlacesPostcodesSuburbsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StatePostcodeSuburbs
        public IQueryable<StatePlacePostcodeSuburb> GetStatePostcodeSuburbs()
        {
            return db.StatesPlacesPostcodesSuburbs;
        }



        // GET: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetPostcodesByStateCode")]
        public IHttpActionResult GetPlacesByState(string state)
        {
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach(StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => stpcsub.state == state))
           {
                StatePlacePostcodeSuburb sps = new StatePlacePostcodeSuburb();
                sps.id = sb.id;
                sps.state = sb.state;
                sps.place = sb.place;         
                sps.postcode = sb.postcode;
                sps.suburb = sb.suburb;
                list.Add(sps);
            }
           
            return Ok(list);
        }

        // GET: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetSuburbsByState")]
        public IHttpActionResult GetPostcodesByPlace(string place)
        {
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach (StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => stpcsub.place == place))
           {
                StatePlacePostcodeSuburb sps = new StatePlacePostcodeSuburb();
                sps.id = sb.id;
                sps.state = sb.state;
                sps.place = sb.place;
                sps.postcode = sb.postcode;
                sps.suburb = sb.suburb;
                list.Add(sps);
            }

            return Ok(list);
        }

        // GET: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetSuburbsByPostcode")]
        public IHttpActionResult GetSuburbsByPostcode(string postcode)
        {
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach (StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => stpcsub.postcode == postcode))
           {
                StatePlacePostcodeSuburb sps = new StatePlacePostcodeSuburb();
                sps.id = sb.id;
                sps.state = sb.state;
                sps.place = sb.place;
                sps.postcode = sb.postcode;
                sps.suburb = sb.suburb;
                list.Add(sps);
            }

            return Ok(list);
        }



        // GET: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        public async Task<IHttpActionResult> GetStatePostcodeSuburb(int id)
        {
            StatePlacePostcodeSuburb statePostcodeSuburb = await db.StatesPlacesPostcodesSuburbs.FindAsync(id);
            if (statePostcodeSuburb == null)
            {
                return NotFound();
            }

            return Ok(statePostcodeSuburb);
        }


        // PUT: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(void))]
        [Route("PutStatePostcodeSuburb")]
        public async Task<IHttpActionResult> PutStatePostcodeSuburb(int id, StatePlacePostcodeSuburb statePostcodeSuburb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != statePostcodeSuburb.id)
            {
                return BadRequest();
            }

            db.Entry(statePostcodeSuburb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatePostcodeSuburbExists(id))
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

        // POST: api/statesplacespostcodessuburbs
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("PostStatePostcodeSuburb")]
        public async Task<IHttpActionResult> PostStatePostcodeSuburb(StatePlacePostcodeSuburb statePostcodeSuburb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StatesPlacesPostcodesSuburbs.Add(statePostcodeSuburb);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = statePostcodeSuburb.id }, statePostcodeSuburb);
        }

        // DELETE: api/statesplacespostcodessuburbs/5
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("PostStatePostcodeSuburb")]
        public async Task<IHttpActionResult> DeleteStatePostcodeSuburb(int id)
        {
            StatePlacePostcodeSuburb statePostcodeSuburb = await db.StatesPlacesPostcodesSuburbs.FindAsync(id);
            if (statePostcodeSuburb == null)
            {
                return NotFound();
            }

            db.StatesPlacesPostcodesSuburbs.Remove(statePostcodeSuburb);
            await db.SaveChangesAsync();

            return Ok(statePostcodeSuburb);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatePostcodeSuburbExists(int id)
        {
            return db.StatesPlacesPostcodesSuburbs.Count(e => e.id == id) > 0;
        }
    }
}