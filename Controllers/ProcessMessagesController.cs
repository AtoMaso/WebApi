using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{

    [Authorize]
    [RoutePrefix("api/processmessages")]
    public class ProcessMessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProcessMessages
        [AllowAnonymous]
        public IHttpActionResult GetProcessMessages()
        {
           
            try
            {
                List<ProcessMessageDTO> dtoList = new List<ProcessMessageDTO>();
                foreach (ProcessMessage message in db.ProcessMessages)
                {
                    ProcessMessageDTO medto = new ProcessMessageDTO();

                    medto.messageId = message.messageId;
                    medto.messageCode = message.messageCode;
                    medto.messageText = message.messageText;
                    medto.messageTypeId = message.messageTypeId;
                    medto.messageTypeDescription = db.ProcessMessageTypes.First(pmt => pmt.messageTypeId == message.messageTypeId).messageTypeDescription;

                    dtoList.Add(medto);
                }
                return Ok<List<ProcessMessageDTO>>(dtoList);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/ProcessMessages?messgeCode=""
        [AllowAnonymous]
        [ResponseType(typeof(ProcessMessage))]
        public IHttpActionResult GetProcessMessageByMessageCode(string messageCode)
        {

            try
            { 
                           
                foreach (ProcessMessage message in db.ProcessMessages)
                {
                    if(message.messageCode == messageCode)
                    {
                        ProcessMessageDTO medto = new ProcessMessageDTO();

                        medto.messageId = message.messageId;
                        medto.messageCode = message.messageCode;
                        medto.messageText = message.messageText;
                        medto.messageTypeId = message.messageTypeId;
                        medto.messageTypeDescription = db.ProcessMessageTypes.First(pmt => pmt.messageTypeId == message.messageTypeId).messageTypeDescription;

                        return Ok<ProcessMessageDTO>(medto);
                    }                                    
                }
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
                return BadRequest(ModelState);
            }          
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/ProcessMessages/5
        [ResponseType(typeof(ProcessMessage))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetProcessMessage(int id)
        {
            ProcessMessage processMessage = await db.ProcessMessages.FindAsync(id);
            if (processMessage == null)
            {
                ModelState.AddModelError("Message", "Process message not found!");
                return BadRequest(ModelState);
            }

            return Ok(processMessage);
        }


        // PUT: api/ProcessMessages/PutProcessMessage?messageId=1
        [ResponseType(typeof(void))]
        [Route("PutProcessMessage")]
        public async Task<IHttpActionResult> PutProcessMessage(int messageId, ProcessMessage processMessage)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message details are not valid!");
                return BadRequest(ModelState);
            }

            if (messageId != processMessage.messageId)
            {
                ModelState.AddModelError("Message", "The process message id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(processMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessMessageExists(messageId))
                {
                    ModelState.AddModelError("Message", "Process message not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            ProcessMessage pm = await db.ProcessMessages.Where(pmt => pmt.messageId == messageId).FirstAsync();
            return Ok<ProcessMessage>(pm);
        }


        // POST: api/ProcessMessages
        [ResponseType(typeof(ProcessMessage))]
        [Route("PostProcessMessage")]
        public async Task<IHttpActionResult> PostProcessMessage([FromBody] ProcessMessage processMessage)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                db.ProcessMessages.Add(processMessage);
                await db.SaveChangesAsync();

                ProcessMessage lastpc = await db.ProcessMessages.OrderByDescending(pc => pc.messageId).FirstAsync();
                return Ok<ProcessMessage>(lastpc); ;
            }
            catch (Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your process message!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/ProcessMessages/DeleteProcessMessage?messageId =1
        [ResponseType(typeof(ProcessMessage))]
        [Route("DeleteProcessMessage")]
        public async Task<IHttpActionResult> DeleteProcessMessage(int messageId)
        {
            ProcessMessage processMessage = await db.ProcessMessages.FindAsync(messageId);
            if (processMessage == null)
            {
                ModelState.AddModelError("Message", "Process message not found!");
                return BadRequest(ModelState);
            }

            db.ProcessMessages.Remove(processMessage);
            await db.SaveChangesAsync();

            return Ok(processMessage);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool ProcessMessageExists(int id)
        {
            return db.ProcessMessages.Count(e => e.messageId == id) > 0;
        }
    }
}