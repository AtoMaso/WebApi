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
    [RoutePrefix("api/securityanswers")]
    public class SecurityAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/securityanswers
        public IHttpActionResult GetSecurityAnswers()
        {
            try
            {
                List<SecurityAnswerDTO> dtoList = new List<SecurityAnswerDTO>();
                foreach (SecurityAnswer securityanswer in db.SecurityAnswers)
                {
                    SecurityAnswerDTO sadto = new SecurityAnswerDTO();

                    sadto.answerId = securityanswer.answerId;
                    sadto.questionId = securityanswer.questionId;
                    sadto.questionText = db.SecurityQuestions.FirstOrDefault(sq => sq.questionId == securityanswer.questionId).questionText;
                    sadto.questionAnswer = securityanswer.questionAnswer;
                    sadto.traderId = securityanswer.traderId;

                    dtoList.Add(sadto);
                }
                return Ok<List<SecurityAnswerDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting security answers!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/securityanswers?GetSecurityAnswersByTraderId="ss" -- used to get the list for security details id
        [Route("GetSecurityAnswersByTraderId")]
        public IHttpActionResult GetSecurityAnswersByTraderId(string traderId)
        {
            try
            {
                List<SecurityAnswerDTO> dtoList = new List<SecurityAnswerDTO>();
                foreach (SecurityAnswer securityanswer in db.SecurityAnswers)
                {
                    if(securityanswer.traderId == traderId)
                    {
                        SecurityAnswerDTO sadto = new SecurityAnswerDTO();

                        sadto.answerId = securityanswer.answerId;
                        sadto.questionId = securityanswer.questionId;
                        sadto.questionText = db.SecurityQuestions.FirstOrDefault(sq => sq.questionId == securityanswer.questionId).questionText;
                        sadto.questionAnswer = securityanswer.questionAnswer;
                        sadto.traderId = securityanswer.traderId;
                        dtoList.Add(sadto);
                    }                                      
                }
                return Ok<List<SecurityAnswerDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting image details!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/securityanswers/5
        [ResponseType(typeof(SecurityAnswer))]
        public async Task<IHttpActionResult> GetSecurityAnswer(int id)
        {
            SecurityAnswer securityanswer = await db.SecurityAnswers.FindAsync(id);
            if (securityanswer == null)
            {
                ModelState.AddModelError("Message", "Security answer not found!");
                return BadRequest(ModelState);
            }

            try
            {
                SecurityAnswerDTO sadto = new SecurityAnswerDTO();

                sadto.answerId = securityanswer.answerId;
                sadto.questionId = securityanswer.questionId;
                sadto.questionText = db.SecurityQuestions.FirstOrDefault(sq => sq.questionId == securityanswer.questionId).questionText;
                sadto.questionAnswer = securityanswer.questionAnswer;
                sadto.traderId = securityanswer.traderId;

                return Ok(sadto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone details!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/securityanswers/PutSecurityAnswer?id=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("PutSecurityAnswer")]
        public async Task<IHttpActionResult> PutSecurityAnswer(int id, SecurityAnswer securityAnswer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The security answer details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != securityAnswer.answerId)
            {
                ModelState.AddModelError("Message", "The security answer id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(securityAnswer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityAnswerExists(id))
                {
                    ModelState.AddModelError("Message", "Security answer not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/securityanswers/PostSecurityAnswer
        [ResponseType(typeof(SecurityAnswer))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("PostSecurityAnswer")]
        public async Task<IHttpActionResult> PostSecurityAnswer(SecurityAnswer securityAnswer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The security answer details are not valid!");
                return BadRequest(ModelState);
            }

            db.SecurityAnswers.Add(securityAnswer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = securityAnswer.answerId }, securityAnswer);
        }


        // DELETE: api/securityanswers/DeleteSecurityAnswer?id=5
        [ResponseType(typeof(SecurityAnswer))]
        [Route("DeleteSecurityAnswer")]
        public async Task<IHttpActionResult> DeleteSecurityAnswer(int id)
        {
            SecurityAnswer securityAnswer = await db.SecurityAnswers.FindAsync(id);
            if (securityAnswer == null)
            {
                ModelState.AddModelError("Message", "Security answer not found!");
                return BadRequest(ModelState);
            }

            db.SecurityAnswers.Remove(securityAnswer);
            await db.SaveChangesAsync();

            return Ok(securityAnswer);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SecurityAnswerExists(int id)
        {
            return db.SecurityAnswers.Count(e => e.answerId == id) > 0;
        }
    }
}