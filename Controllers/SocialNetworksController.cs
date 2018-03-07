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
    [RoutePrefix("api/socialnetworks")]
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
                    sndto.preferredFlag = socialnetwork.preferredFlag;
                    sndto.socialTypeId = socialnetwork.socialTypeId;
                    sndto.socialType = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialTypeId == socialnetwork.socialTypeId).socialType;                  
                    sndto.traderId = socialnetwork.traderId;

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


        // GET: api/socialnetworks/GetSocialNetworksByTradertId?traderId = "xx" 
        [Route("GetSocialNetworksByTraderId")]
        public IHttpActionResult GetSocialNetworksByTraderId(string traderId)
        {

            try
            {
                List<SocialNetworkDTO> dtoList = new List<SocialNetworkDTO>();
                foreach (SocialNetwork socialnetwork in db.SocialNetworks)
                {
                    if(socialnetwork.traderId == traderId)
                    {
                        SocialNetworkDTO sndto = new SocialNetworkDTO();

                        sndto.id = socialnetwork.id;
                        sndto.account = socialnetwork.account;
                        sndto.preferredFlag = socialnetwork.preferredFlag;
                        sndto.socialTypeId = socialnetwork.socialTypeId;
                        sndto.socialType = db.SocialNetworkTypes.First(ty => ty.socialTypeId == socialnetwork.socialTypeId).socialType;
                        sndto.traderId = socialnetwork.traderId;

                        dtoList.Add(sndto);
                    }                   
                }
                return Ok<List<SocialNetworkDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting social network details by trader id!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/socialnetworks/GetPreferredSocialNetworks?traderId = "xx" &preferredFlag="Yes"
        [AllowAnonymous]
        [Route("GetPreferredSocialNetwork")]
        public IHttpActionResult GetPreferredSocialNetwork(string traderId, string preferredFlag)
        {

            try
            {
                var soc = db.SocialNetworks.FirstOrDefault(sn => sn.traderId == traderId && sn.preferredFlag == preferredFlag);
                if (soc != null)
                {
                    SocialNetworkDTO sndto = new SocialNetworkDTO();
                    sndto.id = soc.id;
                    sndto.account = soc.account;
                    sndto.preferredFlag = soc.preferredFlag;
                    sndto.socialTypeId = soc.socialTypeId;
                    sndto.socialType = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialTypeId == soc.socialTypeId).socialType;
                    sndto.traderId = soc.traderId;

                    return Ok<SocialNetworkDTO>(sndto);
                }
                return Ok<SocialNetwork>(new SocialNetwork());                                                         
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting preferred social network by trader id!");
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
                ModelState.AddModelError("Message", "Social Network not found!");
                return BadRequest(ModelState);
            }

            try
            {
                SocialNetworkDTO sndto = new SocialNetworkDTO();

                sndto.id = socialnetwork.id;
                sndto.account = socialnetwork.account;
                sndto.preferredFlag = socialnetwork.preferredFlag;
                sndto.socialTypeId = socialnetwork.socialTypeId;
                sndto.socialType = db.SocialNetworkTypes.FirstOrDefault(ty => ty.socialTypeId == socialnetwork.socialTypeId).socialType;
                sndto.traderId = socialnetwork.traderId;

                return Ok(sndto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the social network details by social network id!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/SocialNetworks/PutSocialNetwork?id=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("PutSocialNetwork")]
        public async Task<IHttpActionResult> PutSocialNetwork(int id, SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The social network details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != socialNetwork.id)
            {
                ModelState.AddModelError("Message", "The social network id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Social Network not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/SocialNetworks/PostSocialNetwork
        [ResponseType(typeof(SocialNetwork))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("PostSocialNetwork")]
        public async Task<IHttpActionResult> PostSocialNetwork(SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The social network details are not valid!");
                return BadRequest(ModelState);
            }

            db.SocialNetworks.Add(socialNetwork);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialNetwork.id }, socialNetwork);
        }


        // DELETE: api/SocialNetworks/DeleteSocialNetwork/5
        [ResponseType(typeof(SocialNetwork))]
        [Route("DeleteSocialNetwork")]
        public async Task<IHttpActionResult> DeleteSocialNetwork(int id)
        {
            SocialNetwork socialNetwork = await db.SocialNetworks.FindAsync(id);
            if (socialNetwork == null)
            {
                ModelState.AddModelError("Message", "Social Network not found!");
                return BadRequest(ModelState);
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