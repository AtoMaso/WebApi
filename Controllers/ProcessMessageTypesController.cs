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
    public class ProcessMessageTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProcessMessageTypes
        public IQueryable<ProcessMessageType> GetProcessMessageTypes()
        {
            return db.ProcessMessageTypes;
        }

        // GET: api/ProcessMessageTypes/5
        [ResponseType(typeof(ProcessMessageType))]
        public async Task<IHttpActionResult> GetProcessMessageType(int id)
        {
            ProcessMessageType processMessageType = await db.ProcessMessageTypes.FindAsync(id);
            if (processMessageType == null)
            {
                ModelState.AddModelError("Message", "Process message type not found!");
                return BadRequest(ModelState);
            }

            return Ok(processMessageType);
        }

        // PUT: api/ProcessMessageTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProcessMessageType(int id, ProcessMessageType processMessageType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message type details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != processMessageType.messageTypeId)
            {
                ModelState.AddModelError("Message", "The process message type id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(processMessageType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessMessageTypeExists(id))
                {
                    ModelState.AddModelError("Message", "Process message type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProcessMessageTypes
        [ResponseType(typeof(ProcessMessageType))]
        public async Task<IHttpActionResult> PostProcessMessageType(ProcessMessageType processMessageType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message type details are not valid!");
                return BadRequest(ModelState);
            }

            db.ProcessMessageTypes.Add(processMessageType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = processMessageType.messageTypeId }, processMessageType);
        }

        // DELETE: api/ProcessMessageTypes/5
        [ResponseType(typeof(ProcessMessageType))]
        public async Task<IHttpActionResult> DeleteProcessMessageType(int id)
        {
            ProcessMessageType processMessageType = await db.ProcessMessageTypes.FindAsync(id);
            if (processMessageType == null)
            {
                ModelState.AddModelError("Message", "Process message type not found!");
                return BadRequest(ModelState);
            }

            db.ProcessMessageTypes.Remove(processMessageType);
            await db.SaveChangesAsync();

            return Ok(processMessageType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProcessMessageTypeExists(int id)
        {
            return db.ProcessMessageTypes.Count(e => e.messageTypeId == id) > 0;
        }
    }
}