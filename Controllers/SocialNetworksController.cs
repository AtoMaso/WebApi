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
        public List<SocialNetworkDTO> GetSocialNetworks()
        {         

            try
            {
                List<SocialNetworkDTO> dtoList = new List<SocialNetworkDTO>();
                foreach (SocialNetwork socialnetwork in db.SocialNetworks)
                {
                    SocialNetworkDTO sndto = new SocialNetworkDTO();
                   
                    sndto.socialNetworkId = socialnetwork.socialNetworkId;
                    sndto.socialNetworkAccount = socialnetwork.socialNetworkAccount;
                    sndto.socialNetworkTypeId = socialnetwork.socialNetworkTypeId;
                    sndto.socialNetworkTypeText = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialNetworkTypeId == socialnetwork.socialNetworkTypeId).socialNetworkTypeText;                  
                    sndto.contactDetailsId = socialnetwork.contactDetailsId;

                    dtoList.Add(sndto);
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting social network details!");
                return null;// BadRequest(ModelState);
            }
        }


        // GET: api/socialnetworks?contactDetailsId = xx - by contactDetailsId - this is goin to be used for socila nwtwork list of the trader
        public List<SocialNetworkDTO> GetSocialNetworksByContactId(int contactDetailsId)
        {

            try
            {
                List<SocialNetworkDTO> dtoList = new List<SocialNetworkDTO>();
                foreach (SocialNetwork socialnetwork in db.SocialNetworks)
                {
                    if(socialnetwork.contactDetailsId == contactDetailsId)
                    {
                        SocialNetworkDTO sndto = new SocialNetworkDTO();

                        sndto.socialNetworkId = socialnetwork.socialNetworkId;
                        sndto.socialNetworkAccount = socialnetwork.socialNetworkAccount;
                        sndto.socialNetworkTypeId = socialnetwork.socialNetworkTypeId;
                        sndto.socialNetworkTypeText = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialNetworkTypeId == socialnetwork.socialNetworkTypeId).socialNetworkTypeText;
                        sndto.contactDetailsId = socialnetwork.contactDetailsId;

                        dtoList.Add(sndto);
                    }                   
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting social network details by contact details id!");
                return null;// BadRequest(ModelState);
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

                sndto.socialNetworkId = socialnetwork.socialNetworkId;
                sndto.socialNetworkTypeId = socialnetwork.socialNetworkTypeId;
                sndto.socialNetworkTypeText = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialNetworkTypeId == socialnetwork.socialNetworkTypeId).socialNetworkTypeText;
                sndto.socialNetworkAccount = socialnetwork.socialNetworkAccount;
             
                sndto.contactDetailsId = socialnetwork.contactDetailsId;
              
                return Ok(sndto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the social network details by social network id!");
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

            if (id != socialNetwork.socialNetworkId)
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

            return CreatedAtRoute("DefaultApi", new { id = socialNetwork.socialNetworkId }, socialNetwork);
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
            return db.SocialNetworks.Count(e => e.socialNetworkId == id) > 0;
        }
    }
}