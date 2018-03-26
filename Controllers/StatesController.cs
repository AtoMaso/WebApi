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
    [RoutePrefix("api/states")]
    public class StatesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PlacesController plctr = new PlacesController();

        // GET: api/States
        public IHttpActionResult GetStates()
        {
            try
            {
                List<State> dtoList = new List<State>();
                foreach (State sta in db.States.OrderBy(x => x.name))
                {
                    State stadto = new State();

                    stadto.id = sta.id;
                    stadto.name = sta.name;
                    stadto.places = ((OkNegotiatedContentResult<List<Place>>)plctr.GetPlacesByStateId(sta.id)).Content;

                    dtoList.Add(stadto);
                }
                return Ok<List<State>>(dtoList);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the states!");
                return BadRequest(ModelState);
            }          
        }


        // GET: api/States/5
        [ResponseType(typeof(State))]
        public async Task<IHttpActionResult> GetState(int id)
        {
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }


        // PUT: api/States/PutState/1      
        [ResponseType(typeof(void))]    
        [Route("PutState")]
        public async Task<IHttpActionResult> PutState(int id, State state)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The state details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != state.id)
            {
                ModelState.AddModelError("Message", "The id provided is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(state).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    ModelState.AddModelError("Message", "Suburb not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            State cat = await db.States.Where(cor => cor.id == id).FirstAsync();
            return Ok<State>(cat);
        }



        // POST: api/States/PostState      
        [ResponseType(typeof(State))]      
        [Route("PostState")]
        public async Task<IHttpActionResult> PostState([FromBody] State state)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The state details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                db.States.Add(state);
                await db.SaveChangesAsync();

                State lastst = await db.States.OrderByDescending(st => st.id).FirstAsync();

                // add the dummy suburb so the app does not fail
                Place pl = new Place();
                pl.stateId = lastst.id;
                pl.name = "Miscellaneous";
                await plctr.PostPlace(pl);

                return Ok<State>(lastst);
            }
            catch (Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your state!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/States/DeleteState?id=5
        [ResponseType(typeof(State))]
        [Route("DeleteState")]
        public async Task<IHttpActionResult> DeleteState(int id)
        {
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                ModelState.AddModelError("Message", "State could not be found!");
                return BadRequest(ModelState);
            }

            db.States.Remove(state);
            await db.SaveChangesAsync();

            return Ok(state);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool StateExists(int id)
        {
            return db.States.Count(e => e.id == id) > 0;
        }
    }
}