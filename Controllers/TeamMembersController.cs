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
    public class TeamMembersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TeamMembers
        public IQueryable<TeamMember> GetTeamMember()
        {
            return db.TeamMembers;
        }

        // GET: api/TeamMembers/5
        [ResponseType(typeof(TeamMember))]
        public async Task<IHttpActionResult> GetTeamMember(string id)
        {
            TeamMember teamMember = await db.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return Ok(teamMember);
        }

        // PUT: api/TeamMembers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeamMember(string id, TeamMember teamMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teamMember.Id)
            {
                return BadRequest();
            }

            db.Entry(teamMember).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamMemberExists(id))
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

        // POST: api/TeamMembers
        [ResponseType(typeof(TeamMember))]
        public async Task<IHttpActionResult> PostTeamMember(TeamMember teamMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamMembers.Add(teamMember);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamMemberExists(teamMember.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = teamMember.Id }, teamMember);
        }

        // DELETE: api/TeamMembers/5
        [ResponseType(typeof(TeamMember))]
        public async Task<IHttpActionResult> DeleteTeamMember(string id)
        {
            TeamMember teamMember = await db.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            db.TeamMembers.Remove(teamMember);
            await db.SaveChangesAsync();

            return Ok(teamMember);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamMemberExists(string id)
        {
            return db.TeamMembers.Count(e => e.Id == id) > 0;
        }
    }
}