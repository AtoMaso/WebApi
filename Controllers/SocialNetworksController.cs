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
    public class SocialNetworksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/socialnetworks   - this is going to be used for list  of all social networks list 
        public IHttpActionResult GetSocialNetworks()
        {         

            try
            {
                List<SocialNetworkDTO> dtoList = new List<SocialNetworkDTO>();
                foreach (SocialNetwork socialnetwork in db.SocialNetworks)
                {
                    SocialNetworkDTO sndto = new SocialNetworkDTO();
                   
                    sndto.id = socialnetwork.id;
                    sndto.account = socialnetwork.account;
                    sndto.preferred = socialnetwork.preferred;
                    sndto.typeId = socialnetwork.typeId;
                    sndto.typeDescription = db.SocialNetworkTypes.FirstOrDefault(ty => ty.typeId == socialnetwork.typeId).typeDescription;                  
                    sndto.contactDetailsId = socialnetwork.contactDetailsId;

                    dtoList.Add(sndto);
                }
                return Ok<List<SocialNetworkDTO>>(dtoList);
            }
            catch (Exception exc)
            {             
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting social network details!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/socialnetworks?contactDetailsId = xx - by contactDetailsId - this is goin to be used for socila nwtwork list of the trader
        public IHttpActionResult GetSocialNetworksByContactId(int contactDetailsId)
        {

            try
            {
                List<SocialNetworkDTO> dtoList = new List<SocialNetworkDTO>();
                foreach (SocialNetwork socialnetwork in db.SocialNetworks)
                {
                    if(socialnetwork.contactDetailsId == contactDetailsId)
                    {
                        SocialNetworkDTO sndto = new SocialNetworkDTO();

                        sndto.id = socialnetwork.id;
                        sndto.account = socialnetwork.account;
                        sndto.preferred = socialnetwork.preferred;
                        sndto.typeId = socialnetwork.typeId;
                        sndto.typeDescription = db.SocialNetworkTypes.FirstOrDefault(ty => ty.typeId == socialnetwork.typeId).typeDescription;
                        sndto.contactDetailsId = socialnetwork.contactDetailsId;

                        dtoList.Add(sndto);
                    }                   
                }
                return Ok<List<SocialNetworkDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting social network details by contact details id!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/socialnetworks/id --    this by social network id and is going to be used only for a single social network
        [ResponseType(typeof(SocialNetworkDTO))]
        public async Task<IHttpActionResult> GetSocialNetwork(int id)
        {          
            SocialNetwork socialnetwork = await db.SocialNetworks.FindAsync(id);
            if (socialnetwork == null)
            {
                return NotFound();
            }

            try
            {
                SocialNetworkDTO sndto = new SocialNetworkDTO();

                sndto.id = socialnetwork.id;
                sndto.account = socialnetwork.account;
                sndto.preferred = socialnetwork.preferred;
                sndto.typeId = socialnetwork.typeId;
                sndto.typeDescription = db.SocialNetworkTypes.FirstOrDefault(ty => ty.typeId == socialnetwork.typeId).typeDescription;
                sndto.contactDetailsId = socialnetwork.contactDetailsId;

                sndto.contactDetailsId = socialnetwork.contactDetailsId;
              
                return Ok(sndto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the social network details by social network id!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/SocialNetworks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSocialNetwork(int id, SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != socialNetwork.id)
            {
                return BadRequest();
            }

            db.Entry(socialNetwork).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialNetworkExists(id))
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

        // POST: api/SocialNetworks
        [ResponseType(typeof(SocialNetwork))]
        public async Task<IHttpActionResult> PostSocialNetwork(SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SocialNetworks.Add(socialNetwork);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialNetwork.id }, socialNetwork);
        }

        // DELETE: api/SocialNetworks/5
        [ResponseType(typeof(SocialNetwork))]
        public async Task<IHttpActionResult> DeleteSocialNetwork(int id)
        {
            SocialNetwork socialNetwork = await db.SocialNetworks.FindAsync(id);
            if (socialNetwork == null)
            {
                return NotFound();
            }

            db.SocialNetworks.Remove(socialNetwork);
            await db.SaveChangesAsync();

            return Ok(socialNetwork);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SocialNetworkExists(int id)
        {
            return db.SocialNetworks.Count(e => e.id == id) > 0;
        }
    }
}