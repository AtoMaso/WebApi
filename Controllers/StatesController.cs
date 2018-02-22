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
    public class StatesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PlacesController plctr = new PlacesController();

        // GET: api/States
        public IHttpActionResult GetStates()
        {
            try
            {
                List<StateDTO> dtoList = new List<StateDTO>();
                foreach (State sta in db.States.OrderBy(x => x.name))
                {
                    StateDTO stadto = new StateDTO();

                    stadto.id = sta.id;
                    stadto.name = sta.name;
                    stadto.places = ((OkNegotiatedContentResult<List<Place>>)plctr.GetPlacesByStateId(sta.id)).Content;

                    dtoList.Add(stadto);
                }
                return Ok<List<StateDTO>>(dtoList);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting object categories!");
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


        // PUT: api/States/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutState(int id, State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != state.id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/States
        [ResponseType(typeof(State))]
        public async Task<IHttpActionResult> PostState(State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.States.Add(state);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = state.id }, state);
        }

        // DELETE: api/States/5
        [ResponseType(typeof(State))]
        public async Task<IHttpActionResult> DeleteState(int id)
        {
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
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