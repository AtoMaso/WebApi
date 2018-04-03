using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.IO;
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();
        private SubcategoriesController subctr = new SubcategoriesController();

    // GET: api/categories
    [AllowAnonymous]
    public IHttpActionResult GetCategories()
    {
            try
            {
                List<CategoryDTO> dtoList = new List<CategoryDTO>();
                foreach (Category objectCat in db.Categories.OrderBy(cat => cat.category))
                {
                    CategoryDTO objcatdto = new CategoryDTO();

                    objcatdto.categoryId = objectCat.categoryId;
                    objcatdto.category = objectCat.category;
                    objcatdto.subcategories = ((OkNegotiatedContentResult<List<Subcategory>>) subctr.GetSubcategoriesByCategoryId(objectCat.categoryId)).Content;

                    dtoList.Add(objcatdto);
                }
                return Ok<List<CategoryDTO>>(dtoList);
            }
            catch (Exception)
            {              
                ModelState.AddModelError("Message", "An unexpected error has occured during getting object categories!" );
                return BadRequest(ModelState);
            }
        }


    // GET: api/objectcategories/5
    [ResponseType(typeof(Category))]
    [AllowAnonymous]
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
    [Route("PutCategory")]
    public async Task<IHttpActionResult> PutCategory(int categoryId, Category category)
    {
      if (!ModelState.IsValid)
      {
            ModelState.AddModelError("Message", "The category details are not valid!");
            return BadRequest(ModelState);
       }

      if (categoryId != category.categoryId)
      {
            ModelState.AddModelError("Message", "The category id is not valid!");
            return BadRequest(ModelState);
        }

      db.Entry(category).State = EntityState.Modified;
      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CategoryExists(categoryId))
        {
            ModelState.AddModelError("Message", "Category not found!");
            return BadRequest(ModelState);
        }
        else
        {
          throw;
        }
      }

        Category cat = await db.Categories.Where(cor => cor.categoryId == categoryId).FirstAsync();
        return Ok<Category>(cat);
   }


    // POST: api/objectcategories
    [ResponseType(typeof(Category))]    
    [Route("PostCategory")]
    public async Task<IHttpActionResult> PostCategory([FromBody] Category category)
    {
            if (!ModelState.IsValid)
            {
                   ModelState.AddModelError("Message", "The category details are not valid!");
                   return BadRequest(ModelState);
             }

            try
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();

                Category lastcat = await db.Categories.OrderByDescending(catins => catins.categoryId).FirstAsync();

                // add the dummy subcategory so the app does not fail
                Subcategory subcat = new Subcategory();
                subcat.categoryId= lastcat.categoryId;
                subcat.subcategory = "Miscellaneous";
                SubcategoriesController subctr = new SubcategoriesController();
                await subctr.PostSubcategory(subcat);

                return Ok<Category>(lastcat); ;
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "Error during saving your category!");
                return BadRequest(ModelState);
            }
    }


    // DELETE: api/categories/DeleteCategory?categoryId =5
    [ResponseType(typeof(Category))]   
    [Route("DeleteCategory")]
    public async Task<IHttpActionResult> DeleteCategory(int categoryId)
    {
      Category category = await db.Categories.FindAsync(categoryId);
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