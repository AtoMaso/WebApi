using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
  public class LocalitiesController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: api/Localities
    public IQueryable<Locality> GetLocalities()
    {
      return db.Localities;
    }

    // GET: api/Localities/5
    [ResponseType(typeof(Locality))]
    public async Task<IHttpActionResult> GetLocality(int id)
    {
      Locality locality = await db.Localities.FindAsync(id);
      if (locality == null)
      {
        return NotFound();
      }

      return Ok(locality);
    }

    // PUT: api/Localities/5
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutLocality(int id, Locality locality)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != locality.LocalityId)
      {
        return BadRequest();
      }

      db.Entry(locality).State = EntityState.Modified;

      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!LocalityExists(id))
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

    // POST: api/Localities
    [ResponseType(typeof(Locality))]
    public async Task<IHttpActionResult> PostLocality(Locality locality)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.Localities.Add(locality);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = locality.LocalityId }, locality);
    }

    // DELETE: api/Localities/5
    [ResponseType(typeof(Locality))]
    public async Task<IHttpActionResult> DeleteLocality(int id)
    {
      Locality locality = await db.Localities.FindAsync(id);
      if (locality == null)
      {
        return NotFound();
      }

      db.Localities.Remove(locality);
      await db.SaveChangesAsync();

      return Ok(locality);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool LocalityExists(int id)
    {
      return db.Localities.Count(e => e.LocalityId == id) > 0;
    }
  }

}