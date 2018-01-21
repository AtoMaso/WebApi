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
  public class CategoriesController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: api/Categories
    public IQueryable<Category> GetCategories()
    {
      return db.Categories;
    }


    // GET: api/Categories
    public Category GetCategoriesByCategoryId(int categoryId)
    {
            try
            {
              
                foreach (Category category in db.Categories)
                {
                    if(category.categoryId == categoryId)  return category;
                    
                }
                return null;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all address!");
                return null; //BadRequest(ModelState);
            }
    }


    // GET: api/Categories/5
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> GetCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(category);
    }

    // PUT: api/Categories/5
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutCategory(int id, Category category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != category.categoryId)
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

    // POST: api/Categories
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> PostCategory(Category category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.Categories.Add(category);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = category.categoryId }, category);
    }

    // DELETE: api/Categories/5
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> DeleteCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      db.Categories.Remove(category);
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
      return db.Categories.Count(e => e.categoryId == id) > 0;
    }
  }

}