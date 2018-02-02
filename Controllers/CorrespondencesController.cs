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
    public class CorrespondencesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Correspondences
        public IHttpActionResult GetCorrespondences()
        {   
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences)
                {
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = corres.subject;
                    mesdto.message = corres.message;             
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;                  
                    mesdto.traderId = corres.traderId;

                    dtoList.Add(mesdto);
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all correspondence!");
                return BadRequest(ModelState);
            }
        }



        // GET: api/Correspondences?subject = ""
        public IHttpActionResult GetCorrespondencesByTradeId(string subject)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences)
                {
                    if(corres.subject == subject)
                    {
                        CorrespondenceDTO mesdto = new CorrespondenceDTO();

                        mesdto.id = corres.id;
                        mesdto.subject = corres.subject;
                        mesdto.message = corres.message;
                        mesdto.status = corres.status;
                        mesdto.dateSent = corres.dateSent;
                        mesdto.traderId = corres.traderId;

                        dtoList.Add(mesdto);
                    }                   
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by tradeId!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences?traderId = 5
        public IHttpActionResult GetCorrespondencesByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences)
                {
                    if (corres.traderId == traderId)
                    {
                        CorrespondenceDTO mesdto = new CorrespondenceDTO();

                        mesdto.id = corres.id;
                        mesdto.subject = corres.subject;
                        mesdto.message = corres.message;
                        mesdto.status = corres.status;
                        mesdto.dateSent = corres.dateSent;
                        mesdto.traderId = corres.traderId;

                        dtoList.Add(mesdto);
                    }
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by traderId!");
                return BadRequest(ModelState);
            }
        }



        // GET: api/Correspondences/5
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> GetCorrespondence(int id)
        {
            Correspondence corres = await db.Correspondences.FindAsync(id);
            if (corres == null)
            {
                return NotFound();
            }
            CorrespondenceDTO mesdto = new CorrespondenceDTO();

            mesdto.id = corres.id;
            mesdto.subject = corres.subject;
            mesdto.message = corres.message;
            mesdto.status = corres.status;
            mesdto.dateSent = corres.dateSent;
            mesdto.traderId = corres.traderId;

            return Ok(corres);
        }



        // PUT: api/Correspondences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCorrespondence(int id, Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != correspondence.id)
            {
                return BadRequest();
            }

            db.Entry(correspondence).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorrespondenceExists(id))
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

        // POST: api/Correspondences
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> PostCorrespondence(Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Correspondences.Add(correspondence);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = correspondence.id }, correspondence);
        }

        // DELETE: api/Correspondences/5
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> DeleteCorrespondence(int id)
        {
            Correspondence correspondence = await db.Correspondences.FindAsync(id);
            if (correspondence == null)
            {
                return NotFound();
            }

            db.Correspondences.Remove(correspondence);
            await db.SaveChangesAsync();

            return Ok(correspondence);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CorrespondenceExists(int id)
        {
            return db.Correspondences.Count(e => e.id == id) > 0;
        }
    }
}