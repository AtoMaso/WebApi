using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{

    [Authorize]
    [RoutePrefix("api/processmessagetypes")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class ProcessMessageTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProcessMessageTypes
        [AllowAnonymous]
        public IQueryable<ProcessMessageType> GetProcessMessageTypes()
        {
            return db.ProcessMessageTypes;
        }


        // GET: api/ProcessMessageTypes/5
        [AllowAnonymous]
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


        // PUT: api/ProcessMessageTypes/PurProcessMessageType?messageTypeId=1
        [ResponseType(typeof(void))]
        [Route("PutProcessMessageType")]
        public async Task<IHttpActionResult> PutProcessMessageType(int messageTypeId, ProcessMessageType processMessageType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message type details are not valid!");
                return BadRequest(ModelState);
            }

            if (messageTypeId != processMessageType.messageTypeId)
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
                if (!ProcessMessageTypeExists(messageTypeId))
                {
                    ModelState.AddModelError("Message", "Process message type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            ProcessMessageType pmt = await db.ProcessMessageTypes.Where(pmty => pmty.messageTypeId == messageTypeId).FirstAsync();
            return Ok<ProcessMessageType>(pmt);
        }


        // POST: api/ProcessMessageTypes
        [ResponseType(typeof(ProcessMessageType))]
        [Route("PostProcessMessageType")]
        public async Task<IHttpActionResult> PostProcessMessageType([FromBody] ProcessMessageType processMessageType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message type details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                db.ProcessMessageTypes.Add(processMessageType);
                await db.SaveChangesAsync();

                ProcessMessageType lastpc = await db.ProcessMessageTypes.OrderByDescending(pc => pc.messageTypeId).FirstAsync();             
                return Ok<ProcessMessageType>(lastpc); ;
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your process message type!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/ProcessMessageTypes/DeleteProcessMessageType?messagetTypeId=1
        [ResponseType(typeof(ProcessMessageType))]
        [Route("DeleteProcessMessageType")]
        public async Task<IHttpActionResult> DeleteProcessMessageType(int messageTypeId)
        {
            ProcessMessageType processMessageType = await db.ProcessMessageTypes.FindAsync(messageTypeId);
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