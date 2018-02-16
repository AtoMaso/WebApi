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

    // GET: api/ObjectCategories
    public IHttpActionResult GetObjectCategories()
    {
            try
            {
                List<Category> dtoList = new List<Category>();
                foreach (Category objectCat in db.Categories)
                {
                    Category objcatdto = new Category();

                    objcatdto.categoryId = objectCat.categoryId;
                    objcatdto.categoryDescription = objectCat.categoryDescription;

                    dtoList.Add(objcatdto);
                }
                return Ok<List<Category>>(dtoList);
            }
            catch (Exception)
            {              
                ModelState.AddModelError("Message", "An unexpected error has occured during getting object categories!" );
                return BadRequest(ModelState);
            }
        }


    // GET: api/objectcategories/5
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> GetCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
                ModelState.AddModelError("Message", "Category not found!");
                return BadRequest(ModelState);
    }

      return Ok(category);
    }


    // PUT: api/objectcategories/5
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
            ModelState.AddModelError("Message", "Category not found!");
            return BadRequest(ModelState);
        }
        else
        {
          throw;
        }
      }

      return StatusCode(HttpStatusCode.NoContent);
    }


    // POST: api/objectcategories
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


    // DELETE: api/objecctcategories/5
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> DeleteCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
                ModelState.AddModelError("Message", "Category not found!");
                return BadRequest(ModelState);
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