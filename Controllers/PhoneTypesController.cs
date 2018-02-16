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
    public class PhoneTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PhoneTypes
        public IQueryable<PhoneType> GetPhoneTypes()
        {
            return db.PhoneTypes;
        }

        // GET: api/PhoneTypes/5
        [ResponseType(typeof(PhoneType))]
        public async Task<IHttpActionResult> GetPhoneTypes(int id)
        {
            PhoneType phoneTypes = await db.PhoneTypes.FindAsync(id);
            if (phoneTypes == null)
            {
                ModelState.AddModelError("Message", "Phone type not found!");
                return BadRequest(ModelState);
            }

            return Ok(phoneTypes);
        }

        // PUT: api/PhoneTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoneTypes(int id, PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phoneTypes.typeId)
            {
                return BadRequest();
            }

            db.Entry(phoneTypes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneTypesExists(id))
                {
                    ModelState.AddModelError("Message", "Phone type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PhoneTypes
        [ResponseType(typeof(PhoneType))]
        public async Task<IHttpActionResult> PostPhoneTypes(PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PhoneTypes.Add(phoneTypes);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phoneTypes.typeId }, phoneTypes);
        }

        // DELETE: api/PhoneTypes/5
        [ResponseType(typeof(PhoneType))]
        public async Task<IHttpActionResult> DeletePhoneTypes(int id)
        {
            PhoneType phoneTypes = await db.PhoneTypes.FindAsync(id);
            if (phoneTypes == null)
            {
                ModelState.AddModelError("Message", "Phone type not found!");
                return BadRequest(ModelState);
            }

            db.PhoneTypes.Remove(phoneTypes);
            await db.SaveChangesAsync();

            return Ok(phoneTypes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneTypesExists(int id)
        {
            return db.PhoneTypes.Count(e => e.typeId == id) > 0;
        }
    }
}