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
    public class ProcessMessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProcessMessages
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

        // PUT: api/ProcessMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProcessMessage(int id, ProcessMessage processMessage)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != processMessage.messageId)
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
                if (!ProcessMessageExists(id))
                {
                    ModelState.AddModelError("Message", "Process message not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProcessMessages
        [ResponseType(typeof(ProcessMessage))]
        public async Task<IHttpActionResult> PostProcessMessage(ProcessMessage processMessage)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The process message details are not valid!");
                return BadRequest(ModelState);
            }

            db.ProcessMessages.Add(processMessage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = processMessage.messageId }, processMessage);
        }

        // DELETE: api/ProcessMessages/5
        [ResponseType(typeof(ProcessMessage))]
        public async Task<IHttpActionResult> DeleteProcessMessage(int id)
        {
            ProcessMessage processMessage = await db.ProcessMessages.FindAsync(id);
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