using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.Web.Http.Results;
using WebApi.Providers;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/geodatas")]

    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    [CacheFilter(TimeDuration = 100)]
    public class GeoDatasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/geodatas/GetStates
        [AllowAnonymous]
        [Route("GetStates")]
        [CacheFilter(TimeDuration = 100)]
        [CompressFilter]
        public IHttpActionResult GetStates()
        {
            Boolean exist = false;
            List<GeoData> list = new List<GeoData>();
            foreach (GeoData sb in db.GeoDatas.Distinct())
            {
                if (list.Count != 0)
                {
                    foreach (GeoData spps in list)
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



        // GET: api/geodatas/GetPlacesByStateCode?statecode = xxx
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]
        [Route("GetPlacesByStateCode")]
        [CacheFilter(TimeDuration = 100)]
        [CompressFilter]
        public IHttpActionResult GetPlacesByStateCode(string statecode)
        {
            Boolean exist = false;
            List<GeoData> list = new List<GeoData>();         
            foreach(GeoData sb in db.GeoDatas.Where(stpcsub => stpcsub.state == statecode).Distinct())
           {
                if(list.Count != 0)
                {
                    foreach (GeoData sbps in list)
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


        // GET: api/geodatas/GetPostcodesByPlaceNameAndStateCode?placename=""&statecode=""
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]
        [Route("GetPostcodesByPlaceNameAndStateCode")]
        [CacheFilter(TimeDuration =100)]
        [CompressFilter]
        public IHttpActionResult GetPostcodesByPlaceNameAndStateCode(string placename, string statecode)
        {
            Boolean exist = false;
            List<GeoData> list = new List<GeoData>();
            foreach (GeoData sb in db.GeoDatas.Where(stpcsub => stpcsub.place == placename && stpcsub.state == statecode))
           {
                GeoData sps = new GeoData();
                if (list.Count != 0)
                {
                    foreach (GeoData sbps in list)
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

     

        // GET: api/geodatas/GetSuburbsByPostcodeNumberAndPlaceName?postcodenumber=11&placename=""
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]       
        [Route("GetSuburbsByPostcodeNumberAndPlaceName")]
        [CacheFilter(TimeDuration = 100)]
        [CompressFilter]
        public IHttpActionResult GetSuburbsByPostcodeNumberAndPlaceName(string postcodenumber, string placename)
        {
            Boolean exist = false;
            List<GeoData> list = new List<GeoData>();
            foreach (GeoData sb in db.GeoDatas.Where(stpcsub => (stpcsub.postcode == postcodenumber && stpcsub.place == placename)))
            {
                GeoData sps = new GeoData();
                if (list.Count != 0)
                {
                    foreach (GeoData sbps in list)
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



        // PUT: api/geodatas/PutStatePlacePostcodeSuburb?geoid=1
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]
        [Route("PutGeoData")]     
        public async Task<IHttpActionResult> PutGeoData(int geoid, GeoData newspps)
        {        
            if (geoid != newspps.id)
            {
                return BadRequest();
            }

            try
            {
                var oldspps = db.GeoDatas.Where(x => x.id == geoid).First();
                if (oldspps.state != newspps.state && newspps.state != "")
                {
                    string oldstate = oldspps.state;
                    foreach (GeoData rec in db.GeoDatas)
                    {
                        if (rec.state == oldstate)
                        {
                            rec.state = newspps.state;                         
                        }
                    }
                    db.SaveChanges();
                }
                else if (oldspps.place != newspps.place && newspps.place != "")
                {
                    string oldplace = oldspps.place;
                    string oldstate = oldspps.state;
                    foreach (GeoData rec in db.GeoDatas)
                    {
                        if (rec.place == oldplace && rec.state == oldstate)
                        {
                            rec.place = newspps.place;
                        }
                    }
                    db.SaveChanges();
                }
                else if (oldspps.postcode != newspps.postcode && newspps.postcode != "")
                {
                    string oldplace = oldspps.place;
                    string oldstate = oldspps.state;
                    string oldpostcode = oldspps.postcode;
                    foreach (GeoData rec in db.GeoDatas)
                    {
                        if (rec.postcode == oldpostcode && rec.place == oldplace && rec.state == oldstate)
                        {
                            rec.postcode = newspps.postcode;
                        }
                    }
                    db.SaveChanges();
                }
                else if (oldspps.suburb != newspps.suburb && newspps.suburb != "")
                {
                    string oldplace = oldspps.place;
                    string oldstate = oldspps.state;
                    string oldpostcode = oldspps.postcode;
                    string oldsuburb = oldspps.suburb;
                    foreach (GeoData rec in db.GeoDatas)
                    {
                        if (rec.suburb == oldspps.suburb && rec.postcode == oldpostcode && rec.place == oldplace && rec.state == oldstate)
                        {
                            rec.suburb = newspps.suburb;
                        }
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception exc)
            {
                string str = exc.InnerException.Message;
                throw;
            }


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeoDatasExists(geoid))
                {
                    ModelState.AddModelError("Message", "Geo Record not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            GeoData recspps = await db.GeoDatas.Where(cor => cor.id == geoid).FirstAsync();
            return Ok<GeoData>(recspps);
        }



        // POST: api/geodatas
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]
        [Route("PostGeoData")]  
        public async Task<IHttpActionResult> PostGeoData([FromBody] GeoData geoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GeoDatas.Add(geoData);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = geoData.id }, geoData);
        }



        // DELETE: api/geodatas/DeleteStatePlacePostcodeSuburb?geoid=1
        [AllowAnonymous]
        [ResponseType(typeof(GeoData))]
        [Route("DeleteGeoData")]
        public async Task<IHttpActionResult> DeleteGeoData(int geoid)
        {
            GeoData geoData = await db.GeoDatas.FindAsync(geoid);
            if (geoData == null)
            {
                return NotFound();
            }

            db.GeoDatas.Remove(geoData);
            await db.SaveChangesAsync();

            return Ok(geoData);
        }





     



        [AllowAnonymous]
        [Route("GetStatesWithData")]
        [CacheFilter(TimeDuration =100)]
        [CompressFilter]
        public IHttpActionResult GetStatesWithData()
        {
            List<string> listOfStates = new List<string>();
            listOfStates = ((OkNegotiatedContentResult<List<string>>)GetStatesNames()).Content;
            List<State> listStatesWithData = new List<State>();
            foreach (string st in listOfStates)
            {
                State stdto = new State();
                stdto.state = st;
                stdto.places = GetPlacesNamesByStateCode(st);
                //stdto.places = ((OkNegotiatedContentResult<List<Place>>)GetPlacesNamesByStateCode(st)).Content;
                listStatesWithData.Add(stdto);
            }

            return Ok(listStatesWithData.OrderBy(x=> x.state));
        }


        public IHttpActionResult GetStatesNames()
        {
            Boolean exist = false;
            List<string> list = new List<string>();
            foreach (GeoData sb in db.GeoDatas.Distinct())
            {
                if (list.Count != 0)
                {
                    foreach (string spps in list)
                    {
                        exist = false;
                        if (sb.state == spps)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) { list.Add(sb.state); }
                }
                else
                {
                    list.Add(sb.state);
                }
            }

            return Ok(list);
        }

   
        public List<Place> GetPlacesNamesByStateCode(string statecode)
        {
            Boolean exist = false;
            List<Place> list = new List<Place>();
            foreach (GeoData sb in db.GeoDatas.Where(stpcsub => stpcsub.state == statecode).Distinct())
            {
                if (list.Count != 0)
                {
                    foreach (Place sbps in list)
                    {
                        exist = false;
                        if (sb.place == sbps.place)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) {
                        Place placeDto = new Place();
                        placeDto.place = sb.place;
                        placeDto.parentstate = sb.state;
                        placeDto.postcodes = GetPostcodesNumbersByPlaceNameAndStatecode(placeDto.place, placeDto.parentstate);
                        //placeDto.postcodes = ((OkNegotiatedContentResult<List<Postcode>>)GetPostcodesNumbersByPlaceNameAndStatecode(placeDto.place, placeDto.parentstate)).Content;
                        list.Add(placeDto);
                    }
                }
                else
                {
                    Place placeDto = new Place();
                    placeDto.place = sb.place;
                    placeDto.parentstate = sb.state;
                    placeDto.postcodes = GetPostcodesNumbersByPlaceNameAndStatecode(placeDto.place, placeDto.parentstate);
                    //placeDto.postcodes = ((OkNegotiatedContentResult<List<Postcode>>)GetPostcodesNumbersByPlaceNameAndStatecode(placeDto.place, placeDto.parentstate)).Content;
                    list.Add(placeDto);
                }
            }

            return list;
        }


        public List<Postcode> GetPostcodesNumbersByPlaceNameAndStatecode(string placename, string statecode)
        {
            Boolean exist = false;
            List<Postcode> list = new List<Postcode>();
            foreach (GeoData sb in db.GeoDatas.Where(stpcsub => stpcsub.place == placename && stpcsub.state == statecode))
            {              
                if (list.Count != 0)
                {
                    foreach (Postcode sbps in list)
                    {
                        exist = false;
                        if (sb.postcode == sbps.postcode)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) {
                        Postcode postcodeDto = new Postcode();
                        postcodeDto.postcode = sb.postcode;
                        postcodeDto.parentplace = sb.place;
                        postcodeDto.suburbs = GetSuburbNamesByPostcodeAndPlace(postcodeDto.postcode, postcodeDto.parentplace);
                        //postcodeDto.suburbs = ((OkNegotiatedContentResult<List<Suburb>>)GetSuburbNamesByPostcodeAndPlace(postcodeDto.postcode, postcodeDto.parentplace)).Content;
                        list.Add(postcodeDto);                     
                    }
                }
                else
                {
                    Postcode postcodeDto = new Postcode();
                    postcodeDto.postcode = sb.postcode;
                    postcodeDto.parentplace = sb.place;
                    postcodeDto.suburbs = GetSuburbNamesByPostcodeAndPlace(postcodeDto.postcode, postcodeDto.parentplace);
                    //postcodeDto.suburbs = ((OkNegotiatedContentResult<List<Suburb>>)GetSuburbNamesByPostcodeAndPlace(postcodeDto.postcode, postcodeDto.parentplace)).Content;
                    list.Add(postcodeDto);
                }
            }

            return list;
        }


        public List<Suburb> GetSuburbNamesByPostcodeAndPlace(string postcodenumber, string placename)
        {
            Boolean exist = false;
            List<Suburb> list = new List<Suburb>();
            foreach (GeoData sb in db.GeoDatas.Where(stpcsub => (stpcsub.postcode == postcodenumber && stpcsub.place == placename)))
            {
                if (list.Count != 0)
                {
                    foreach (Suburb sbps in list)
                    {
                        exist = false;
                        if (sb.suburb == sbps.suburb)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        Suburb suburbDto = new Suburb();
                        suburbDto.suburb = sb.suburb;
                        suburbDto.parentpostcode = sb.postcode;
                        list.Add(suburbDto);
                    }

                }
                else
                {
                    Suburb suburbDto = new Suburb();
                    suburbDto.suburb = sb.suburb;
                    suburbDto.parentpostcode = sb.postcode;
                    list.Add(suburbDto);              
                }
            }
            return list;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool GeoDatasExists(int id)
        {
            return db.GeoDatas.Count(e => e.id == id) > 0;
        }
    }
}