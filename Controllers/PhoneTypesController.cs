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
    [RoutePrefix("api/phonetypes")]
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


        // PUT: api/PhoneTypes/PutPhoneType?id=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [Route("PutPhoneType")]
        public async Task<IHttpActionResult> PutPhoneType(int phoneTypeId, PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone type details are not valid!");
                return BadRequest(ModelState);
            }

            if (phoneTypeId != phoneTypes.phoneTypeId)
            {
                ModelState.AddModelError("Message", "The phone type id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(phoneTypes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneTypesExists(phoneTypeId))
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


        // POST: api/PhoneTypes/PostPhoneType
        [ResponseType(typeof(PhoneType))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostPhoneType")]
        public async Task<IHttpActionResult> PostPhoneType([FromBody] PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone type details are not valid!");
                return BadRequest(ModelState); ;
            }

            db.PhoneTypes.Add(phoneTypes);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phoneTypes.phoneTypeId }, phoneTypes);
        }


        // DELETE: api/PhoneTypes/DeletePhone?id=5
        [ResponseType(typeof(PhoneType))]
        [Route("DeletePhoneType")]
        public async Task<IHttpActionResult> DeletePhoneType(int id)
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
            return db.PhoneTypes.Count(e => e.phoneTypeId == id) > 0;
        }
    }
}