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
        public IQueryable<Subcategory> GetSubcategories()
        {
            return db.Subcategories;
        }



        // GET: api/subcategories/Subcategories?categoryId = xx
        [AllowAnonymous]
        [Route("GetSubcategoriesByCategoryId")]
        public IQueryable<Subcategory> GetSubcategoriesByCategoryId(int categoryId)
        {
            return db.Subcategories.Where(sub => sub.categoryId == categoryId);
        }



        // GET: api/Subcategories/5
        [ResponseType(typeof(Subcategory))]
        public async Task<IHttpActionResult> GetSubcategory(int id)
        {
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }


        // PUT: api/Subcategories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubcategory(int id, Subcategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subcategory.subcategoryId)
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
                if (!SubcategoryExists(id))
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

        // POST: api/Subcategories
        [ResponseType(typeof(Subcategory))]
        public async Task<IHttpActionResult> PostSubcategory(Subcategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subcategories.Add(subcategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = subcategory.subcategoryId }, subcategory);
        }

        // DELETE: api/Subcategories/5
        [ResponseType(typeof(Subcategory))]
        public async Task<IHttpActionResult> DeleteSubcategory(int id)
        {
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
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