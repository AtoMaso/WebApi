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
    public class StatesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/States
        public IQueryable<State> GetStates()
        {
            return db.States;
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