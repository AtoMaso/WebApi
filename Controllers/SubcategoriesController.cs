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

    [Authorize]
    [RoutePrefix("api/subcategories")]
    public class SubcategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Subcategories      
        [AllowAnonymous]
        public IQueryable<Subcategory> GetSubcategories()
        {
            return db.Subcategories.OrderBy(subcat => subcat.subcategory);
        }


        // GET: api/subcategories/GetSubcategoriesByCategoryId?categoryId = xx     
        [Route("GetSubcategoriesByCategoryId")]       
        public IHttpActionResult GetSubcategoriesByCategoryId(int categoryId)
        {
            try
            {
                List<Subcategory> list = new List<Subcategory>();
                foreach (Subcategory sub in db.Subcategories.Where(sub => sub.categoryId == categoryId).OrderBy(subcat => subcat.subcategory))
                {
                    Subcategory subdto = new Subcategory();
                    subdto.subcategoryId = sub.subcategoryId;
                    subdto.subcategory = sub.subcategory;
                    subdto.categoryId = sub.categoryId;
                    list.Add(subdto);
                }
                return Ok<List<Subcategory>>(list);
            }
            catch(Exception) {
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all subcategories!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/subcategories/5
        [ResponseType(typeof(Subcategory))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSubcategory(int id)
        {
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }


        // PUT: api/subcategories?subcategoryId = 5
        [ResponseType(typeof(Subcategory))]     
        [Route("PutSubcategory")]
        public async Task<IHttpActionResult> PutSubcategory(int subcategoryId, Subcategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (subcategoryId != subcategory.subcategoryId)
            {
                return BadRequest();
            }

            db.Entry(subcategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubcategoryExists(subcategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Subcategory subcat = await db.Subcategories.Where(cat => cat.subcategoryId == subcategoryId).FirstAsync();
            return Ok<Subcategory>(subcat);
        }


        // POST: api/subcategories
        [ResponseType(typeof(Subcategory))]   
        [Route("PostSubcategory")]
        public async Task<IHttpActionResult> PostSubcategory([FromBody] Subcategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }        
            try
            {
                db.Subcategories.Add(subcategory);
                await db.SaveChangesAsync();

                Subcategory subcat = await db.Subcategories.OrderByDescending(catins => catins.subcategoryId).FirstAsync();             
                return Ok<Subcategory>(subcat); ;
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "Error during saving your subcategory!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/subcategories/5
        [ResponseType(typeof(Subcategory))]
        [Route("DeleteSubcategory")]
        public async Task<IHttpActionResult> DeleteSubcategory(int subcategoryId)
        {
            Subcategory subcategory = await db.Subcategories.FindAsync(subcategoryId);
            if (subcategory == null)
            {
                return NotFound();
            }

            db.Subcategories.Remove(subcategory);
            await db.SaveChangesAsync();

            return Ok(subcategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubcategoryExists(int id)
        {
            return db.Subcategories.Count(e => e.subcategoryId == id) > 0;
        }
    }
}