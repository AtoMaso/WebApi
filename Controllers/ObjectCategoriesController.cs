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
  public class ObjectCategoriesController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: api/ObjectCategories
    public IQueryable<ObjectCategory> ObjectGetCategories()
    {
      return db.ObjectCategories;
    }


    // GET: api/ObjectCategories?categoryid=5
    public ObjectCategory GetObjectCategoriesByCategoryId(int categoryId)
    {
            try
            {
              
                foreach (ObjectCategory category in db.ObjectCategories)
                {
                    if(category.objectCategoryId == categoryId)  return category;
                    
                }
                return null;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return null; //BadRequest(ModelState);
            }
    }


    // GET: api/objectcategories/5
    [ResponseType(typeof(ObjectCategory))]
    public async Task<IHttpActionResult> GetObjectCategory(int id)
    {
      ObjectCategory category = await db.ObjectCategories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(category);
    }


    // PUT: api/objectcategories/5
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutObjectCategory(int id, ObjectCategory category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != category.objectCategoryId)
      {
        return BadRequest();
      }

      db.Entry(category).State = EntityState.Modified;

      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CategoryExists(id))
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


    // POST: api/objectcategories
    [ResponseType(typeof(ObjectCategory))]
    public async Task<IHttpActionResult> PostObjectCategory(ObjectCategory category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.ObjectCategories.Add(category);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = category.objectCategoryId }, category);
    }


    // DELETE: api/objecctcategories/5
    [ResponseType(typeof(ObjectCategory))]
    public async Task<IHttpActionResult> DeleteObjectCategory(int id)
    {
      ObjectCategory category = await db.ObjectCategories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      db.ObjectCategories.Remove(category);
      await db.SaveChangesAsync();

      return Ok(category);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool CategoryExists(int id)
    {
      return db.ObjectCategories.Count(e => e.objectCategoryId == id) > 0;
    }
  }

}