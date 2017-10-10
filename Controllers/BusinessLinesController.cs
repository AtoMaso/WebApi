using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

  public class BusinessLinesController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: api/BusinessLines
    public IQueryable<BusinessLine> GetBusinessLines()
    {
      return db.BusinessLines;
    }

    // GET: api/BusinessLines/5
    [ResponseType(typeof(BusinessLine))]
    public async Task<IHttpActionResult> GetBusinessLine(int id)
    {
      BusinessLine businessLine = await db.BusinessLines.FindAsync(id);
      if (businessLine == null)
      {
        return NotFound();
      }

      return Ok(businessLine);
    }

    // PUT: api/BusinessLines/5
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutBusinessLine(int id, BusinessLine businessLine)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != businessLine.BusinessLineId)
      {
        return BadRequest();
      }

      db.Entry(businessLine).State = EntityState.Modified;

      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BusinessLineExists(id))
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

    // POST: api/BusinessLines
    [ResponseType(typeof(BusinessLine))]
    public async Task<IHttpActionResult> PostBusinessLine(BusinessLine businessLine)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.BusinessLines.Add(businessLine);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = businessLine.BusinessLineId }, businessLine);
    }

    // DELETE: api/BusinessLines/5
    [ResponseType(typeof(BusinessLine))]
    public async Task<IHttpActionResult> DeleteBusinessLine(int id)
    {
      BusinessLine businessLine = await db.BusinessLines.FindAsync(id);
      if (businessLine == null)
      {
        return NotFound();
      }

      db.BusinessLines.Remove(businessLine);
      await db.SaveChangesAsync();

      return Ok(businessLine);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool BusinessLineExists(int id)
    {
      return db.BusinessLines.Count(e => e.BusinessLineId == id) > 0;
    }
  }

}