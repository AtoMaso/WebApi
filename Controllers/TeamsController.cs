using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApi.Models;
using System.Web.Http.Description;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using Microsoft.AspNet.Identity;

namespace WebApi.Controllers
{

  [Authorize]
  [RoutePrefix("api/teams")]
  public class TeamsController : ApiController
    {       
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public TeamsController() { }

        public TeamsController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // DONE-WORKS
        [AllowAnonymous]
        [ResponseType(typeof(TeamDTO))]
        public IHttpActionResult GetTeams()
        {      
            IQueryable<ApplicationUser> users = db.Users;
            List <TeamDTO> teamsdto = new List<TeamDTO>();

            try
            {
                foreach (Team team in db.Teams)
                {
                    TeamDTO teamdto = new TeamDTO();

                    teamdto.TeamId = team.TeamId;
                    teamdto.TeamName = team.TeamName;
                    teamdto.TeamLeadId = team.TeamLeadId;
                    teamdto.ProjectManagerId = team.ProjectManagerId;
                    teamdto.ProjectDirectorId = team.ProjectDirectorId;
                    teamdto.BusinessLineName = team.BusinessLine.BusinessLineName;

                    teamdto.TeamLead = users.First(x => x.Id == team.TeamLeadId).Name;
                    teamdto.ProjectManager = users.First(x => x.Id == team.ProjectManagerId).Name;
                    teamdto.ProjectDirector = users.First(x => x.Id == team.ProjectDirectorId).Name;

                    teamsdto.Add(teamdto);
                }
                return Ok(teamsdto);
            }
            catch (Exception exc)
            {
                // TODO come up with logging solution
                // log the exc here
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all teams!");
                return BadRequest();
            }                                                                         
          
        }


        // DONE-WORKS
        // GET api/Articles/5 -
        [AllowAnonymous]  
        [ResponseType(typeof(TeamDetailDTO))]
        public IHttpActionResult GetTeam(int id)
        {
            IQueryable<Locality> locs = db.Localities;
            IQueryable<ApplicationUser> users = db.Users;
            Team team = db.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }
            try
            {
                var teamdto = new TeamDetailDTO()
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TeamDescription = team.TeamDescription,
                    ProjectDirectorId = team.ProjectDirectorId,
                    ProjectManagerId = team.ProjectManagerId,
                    TeamLeadId = team.TeamLeadId,

                    ProjectDirector = users.First(x => x.Id == team.ProjectDirectorId).Name.ToString(),
                    ProjectManager = users.First(x => x.Id == team.ProjectManagerId).Name.ToString(),
                    TeamLead = users.First(x => x.Id == team.TeamLeadId).Name.ToString(),

                    LocalityId = team.LocalityId,
                    LocalityNumber = locs.First(x => x.LocalityId == team.LocalityId).Number,
                    LocalityStreet = locs.First(x => x.LocalityId == team.LocalityId).Street,
                    LocalitySuburb = locs.First(x => x.LocalityId == team.LocalityId).Suburb,
                    LocalityCity = locs.First(x => x.LocalityId == team.LocalityId).City,
                    LocalityPostcode = locs.First(x => x.LocalityId == team.LocalityId).Postcode,
                    LocalityState = locs.First(x => x.LocalityId == team.LocalityId).State,

                    BusinessLineId = team.BusinessLine.BusinessLineId,
                    BusinessLineName = team.BusinessLine.BusinessLineName,
                };
                return Ok(teamdto);
            }
            catch (Exception exc)
            {
                // TODO come up with logging solution
                // log the exc here
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the team!");
                return BadRequest();
            }
        }

        // TODO UPDATE
        // PUT: api/Teams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeam(int id, Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.TeamId)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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


        // DONE-WORKS
        // POST: api/Teams/PostTeam
        [ResponseType(typeof(Team))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostTeam")]
        public async Task<IHttpActionResult> PostTeam([FromBody] Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                                           
            try
            {

                db.Teams.Add(team);
                int resultUse = await db.SaveChangesAsync();

                Locality lo = db.Localities.Find(team.LocalityId);
                BusinessLine bl = db.BusinessLines.Find(team.BusinessLineId);
                IQueryable<ApplicationUser> users = db.Users;
                TeamDetailDTO teamdto = new TeamDetailDTO();

                teamdto.TeamName = team.TeamName;
                teamdto.TeamDescription = team.TeamDescription;

                teamdto.BusinessLineName = bl.BusinessLineName;

                teamdto.TeamLead = users.First(x => x.Id == team.TeamLeadId).Name;
                teamdto.ProjectManager = users.First(x => x.Id == team.ProjectManagerId).Name;
                teamdto.ProjectDirector = users.First(x => x.Id == team.ProjectManagerId).Name;

                teamdto.LocalityNumber = lo.Number;
                teamdto.LocalityStreet = lo.Street;
                teamdto.LocalitySuburb = lo.Suburb;
                teamdto.LocalityCity = lo.City;
                teamdto.LocalityPostcode = lo.Postcode;
                teamdto.LocalityState = lo.State;
                    
                return Ok(teamdto);
            }
            catch (Exception)
            {
                // TODO come up with logging solution
                // log the exc here
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during adding the team!");
                return BadRequest();
            }
          
        }


        // DONE - WORKS
        // DELETE: api/Teams/5
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> DeleteTeam(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }                  
          
            // the deletion of the team should remove all the members of the team
            // in the TeamMember table as we have relationship between the Team
            // and the TeamMember table
            db.Teams.Remove(team);
            int result = await db.SaveChangesAsync();
            if (result > 0) {return Ok(team); }
            ModelState.AddModelError("Unexpected", "An unexpected error has occured during removing the team!");
            return BadRequest(ModelState);                
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamId == id) > 0;
        }
              
    }
}