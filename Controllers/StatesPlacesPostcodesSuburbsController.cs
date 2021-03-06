﻿using System;
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
    [RoutePrefix("api/statesplacespostcodessuburbs")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class StatesPlacesPostcodesSuburbsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: api/StatePostcodeSuburbs
        [AllowAnonymous]
        [Route("GetStates")]
        public IHttpActionResult GetStates()
        {
            Boolean exist = false;
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach (StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Distinct())
            {
                if (list.Count != 0)
                {
                    foreach (StatePlacePostcodeSuburb spps in list)
                    {
                        exist = false;
                        if (sb.state == spps.state)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) { list.Add(sb); }
                }
                else
                {
                    list.Add(sb);
                }
            }

            return Ok(list.OrderBy(x => x.state));          
        }



        // GET: api/statesplacespostcodessuburbs/GetPlacesByStateCode?statecode = xxx
        [AllowAnonymous]
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetPlacesByStateCode")]
        public IHttpActionResult GetPlacesByStateCode(string statecode)
        {
            Boolean exist = false;
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();         
            foreach(StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => stpcsub.state == statecode).Distinct())
           {
                if(list.Count != 0)
                {
                    foreach (StatePlacePostcodeSuburb sbps in list)
                    {
                        exist = false;
                        if (sb.place == sbps.place)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist) {   list.Add(sb); }                                 
                }
                else
                {
                    list.Add(sb);
                }                             
            }
           
            return Ok(list.OrderBy(x=> x.place));
        }



        // GET: api/statesplacespostcodessuburbs/GetPostcodesByPlaceNameAndStateCode?placename=""&statecode=""
        [AllowAnonymous]
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetPostcodesByPlaceNameAndStateCode")]
        public IHttpActionResult GetPostcodesByPlaceNameAndStateCode(string placename, string statecode)
        {
            Boolean exist = false;
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach (StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => stpcsub.place == placename && stpcsub.state == statecode))
           {
                StatePlacePostcodeSuburb sps = new StatePlacePostcodeSuburb();
                if (list.Count != 0)
                {
                    foreach (StatePlacePostcodeSuburb sbps in list)
                    {
                        exist = false;
                        if (sb.postcode == sbps.postcode)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) { list.Add(sb); }
                }
                else
                {
                    list.Add(sb);
                }
              
            }

            return Ok(list.OrderBy(x=> x.postcode));
        }

     

        // GET: api/statesplacespostcodessuburbs/GetSuburbsByPostcodeNumberAndPlaceName?postcodenumber=11&placename=""
        [AllowAnonymous]
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("GetSuburbsByPostcodeNumberAndPlaceName")]
        public IHttpActionResult GetSuburbsByPostcodeNumberAndPlaceName(string postcodenumber, string placename)
        {
            Boolean exist = false;
            List<StatePlacePostcodeSuburb> list = new List<StatePlacePostcodeSuburb>();
            foreach (StatePlacePostcodeSuburb sb in db.StatesPlacesPostcodesSuburbs.Where(stpcsub => (stpcsub.postcode == postcodenumber && stpcsub.place == placename)))
            {
                StatePlacePostcodeSuburb sps = new StatePlacePostcodeSuburb();
                if (list.Count != 0)
                {
                    foreach (StatePlacePostcodeSuburb sbps in list)
                    {
                        exist = false;
                        if (sb.suburb == sbps.suburb)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) { list.Add(sb); }
                }
                else
                {
                    list.Add(sb);
                }
            }

            return Ok(list.OrderBy(x=> x.suburb));
        }



        // PUT: api/statesplacespostcodessuburbs/PutStatePlacePostcodeSuburb?geoid=1
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("PutStatePlacePostcodeSuburb")]
        [HttpPut]
        public async Task<IHttpActionResult> PutStatePlacePostcodeSuburb(int geoid, StatePlacePostcodeSuburb newspps)
        {        
            if (geoid != newspps.id)
            {
                return BadRequest();
            }           

            var oldspps = db.StatesPlacesPostcodesSuburbs.Where(x => x.id == geoid).First();           
            if (oldspps.state != newspps.state && newspps.state !="") {
                string oldstate = oldspps.state;             
                foreach (StatePlacePostcodeSuburb rec in db.StatesPlacesPostcodesSuburbs)
                {
                    if (rec.state == oldstate)
                    {
                        rec.state = newspps.state;
                    }
                }
                db.SaveChanges();               
            }
            else if (oldspps.place != newspps.place && newspps.place != "") {
                string oldplace = oldspps.place;
                string oldstate = oldspps.state;
                foreach (StatePlacePostcodeSuburb rec in db.StatesPlacesPostcodesSuburbs)
                {
                    if (rec.place == oldplace && rec.state == oldstate)
                    {
                        rec.place = newspps.place;                    
                    }
                }             
                db.SaveChanges();
            }
            else if  (oldspps.postcode != newspps.postcode && newspps.postcode != "") {
                string oldplace = oldspps.place;
                string oldstate = oldspps.state;
                string oldpostcode = oldspps.postcode;                
                foreach (StatePlacePostcodeSuburb rec in db.StatesPlacesPostcodesSuburbs)
                {
                    if (rec.postcode == oldpostcode && rec.place == oldplace && rec.state == oldstate)
                    {
                        rec.postcode = newspps.postcode;
                    }
                }
                db.SaveChanges();
            }
            else if (oldspps.suburb != newspps.suburb && newspps.suburb !="") {
                string oldplace = oldspps.place;
                string oldstate = oldspps.state;
                string oldpostcode = oldspps.postcode;
                string oldsuburb = oldspps.suburb;
                foreach (StatePlacePostcodeSuburb rec in db.StatesPlacesPostcodesSuburbs)
                {
                    if (rec.suburb == oldspps.suburb &&   rec.postcode == oldpostcode && rec.place == oldplace && rec.state == oldstate)
                    {
                        rec.suburb = newspps.suburb;
                    }
                }
                db.SaveChanges();
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatePostcodeSuburbExists(geoid))
                {
                    ModelState.AddModelError("Message", "Geo Record not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            StatePlacePostcodeSuburb recspps = await db.StatesPlacesPostcodesSuburbs.Where(cor => cor.id == geoid).FirstAsync();
            return Ok<StatePlacePostcodeSuburb>(recspps);
        }





        // POST: api/statesplacespostcodessuburbs
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("PostStatePlacePostcodeSuburb")]
        [HttpPost]
        public async Task<IHttpActionResult> PostStatePlacePostcodeSuburb([FromBody] StatePlacePostcodeSuburb statePlacePostcodeSuburb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StatesPlacesPostcodesSuburbs.Add(statePlacePostcodeSuburb);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = statePlacePostcodeSuburb.id }, statePlacePostcodeSuburb);
        }



        // DELETE: api/statesplacespostcodessuburbs/DeleteStatePlacePostcodeSuburb?geoid=1
        [ResponseType(typeof(StatePlacePostcodeSuburb))]
        [Route("DeleteStatePlacePostcodeSuburb")]
        public async Task<IHttpActionResult> DeleteStatePlacePostcodeSuburb(int geoid)
        {
            StatePlacePostcodeSuburb statePostcodeSuburb = await db.StatesPlacesPostcodesSuburbs.FindAsync(geoid);
            if (statePostcodeSuburb == null)
            {
                return NotFound();
            }

            db.StatesPlacesPostcodesSuburbs.Remove(statePostcodeSuburb);
            await db.SaveChangesAsync();

            return Ok(statePostcodeSuburb);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        private bool StatePostcodeSuburbExists(int id)
        {
            return db.StatesPlacesPostcodesSuburbs.Count(e => e.id == id) > 0;
        }
    }
}