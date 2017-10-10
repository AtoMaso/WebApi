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
using System.IO;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/articles")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class ArticlesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: api/Articles
        //public IQueryable<Article> GetArticles()
        //{
        //    return db.Articles
        //        .Include(art => art.Author)
        //        .Include(art => art.Category)
        //        .Include(art => art.Content);
        //}

        // DONE - WORKS
        //GET: api/Articles
        [AllowAnonymous]
        public IHttpActionResult GetArticles()
        {
            try
            {
                var articlesdto = from a in db.Articles
                                  select new ArticleDTO()
                                  {
                                      /*Id = a.Id,*/
                                      ArticleId = a.ArticleId,
                                      Title = a.Title,
                                      Flash = a.Flash,
                                      DatePublished = a.DatePublished,
                                      CategoryName = a.Category.CategoryName,
                                      AuthorId = a.Author.Id,
                                      AuthorName = a.Author.Name
                                  };
                return Ok(articlesdto);
            }
            catch (Exception exc)
            {
                // log the exc
                ModelState.AddModelError("Article", "An unexpected error occured during getting all article!");
                return BadRequest(ModelState);
            }
        }


        // DONE - WORKS
        //GET: api/Articles?authorid=2      
        [AllowAnonymous]
        public IHttpActionResult GetArticles(string authorid)
        {
            try
            {
                var articles = from a in db.Articles
                               where (a.AuthorId.Equals(authorid))
                               select new ArticleDTO()
                               {
                                   //Id = a.Id,
                                   ArticleId = a.ArticleId,
                                   Title = a.Title,
                                   Flash = a.Flash,
                                   DatePublished = a.DatePublished,
                                   CategoryName = a.Category.CategoryName,
                                   AuthorId = a.Author.Id,
                                   AuthorName = a.Author.Name
                               };
                return Ok(articles);
            }
            catch (Exception exc)
            {
                // log the exc
                ModelState.AddModelError("Article", "An unexpected error occured during getting all article!");
                return BadRequest(ModelState);
            }
        }


        ////  GET: api/Articles/5
        //    [ResponseType(typeof(Article))]
        //    public async Task<IHttpActionResult> GetArticle(int id)
        //    {
        //        Article article = await db.Articles.FindAsync(id);
        //        if (article == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(article);
        //    }

        // DONE - WORKS
        //GET api/Articles/5
        [ResponseType(typeof(ArticleDetailDTO))]
        [AllowAnonymous]
        public IHttpActionResult GetArticle(string id)
        {
            var article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }
            try
            {
                ArticleDetailDTO articledto = new ArticleDetailDTO()
                {
                    //Id = article.Id,
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Flash = article.Flash,
                    DatePublished = article.DatePublished,
                    Content = article.Content,

                    CategoryName = db.Categories.First(cat => cat.CategoryId == article.CategoryId).CategoryName,

                    AuthorId = db.Users.First(user => user.Id == article.AuthorId).Id,
                    AuthorName = db.Users.First(user => user.Id == article.AuthorId).Name
                };
                var attachments = from att in db.Attachements
                                  where att.ArticleId == article.ArticleId
                                  select att;

                articledto.Attachements = attachments.ToList();

                return Ok(articledto);
            }
            catch (Exception exc)
            {
                // log the exc
                ModelState.AddModelError("Article", "An error occured in getting the article!");
                return BadRequest(ModelState);
            }
        }


        // TO DO UPDATE
        //PUT: api/Articles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArticle(string id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != article.Id)
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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


        //    POST: api/Articles
        //    [ResponseType(typeof(Article))]
        //    public async Task<IHttpActionResult> PostArticle(Article article)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.Articles.Add(article);
        //        await db.SaveChangesAsync();

        //        return CreatedAtRoute("DefaultApi", new { id = article.Id }, article);
        //    }


        //
        // POST: api/articles/PostArticle       
        [ResponseType(typeof(ArticleDetailDTO))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostArticle")]
        public async Task<IHttpActionResult> PostArticle([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // add the articles' attachements first
                foreach (Attachement attach in article.Attachements) { db.Attachements.Add(attach); }
                // add the article now
                db.Articles.Add(article);
                await db.SaveChangesAsync();

                // Load author and category virtual properties
                db.Entry(article).Reference(x => x.Author).Load();
                db.Entry(article).Reference(x => x.Category).Load();

                var articledto = new ArticleDetailDTO()
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Flash = article.Flash,
                    DatePublished = article.DatePublished,
                    AuthorName = article.Author.Name,
                    CategoryName = article.Category.CategoryName,
                    Attachements = article.Attachements
                };

                return Ok(articledto);
            }
            catch (Exception exc)
            {
                // TODO come up with logging solution here                
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during storing the article!");
                return BadRequest(ModelState);
            }
        }



        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        [Route("DeleteArticle")]
        public async Task<IHttpActionResult> DeleteArticle(string id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                // we are deleting the physical uploaded file (the attachements)
                DeletePhysicalArticle(id);

                // remove the record from the attachements table
                db.Attachements.RemoveRange(article.Attachements);

                // removing of the article
                db.Articles.Remove(article);
                await db.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception exc)
            {
                // try to roll back the changes
                DbEntityEntry entry = db.Entry(article);
                if (entry != null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        default: break;
                    }
                }

                string mess = exc.Message;
                // TODO come up with logging solution here                
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during removing the article!");
                return BadRequest(ModelState);
            }
        }


        public void DeletePhysicalArticle(string id) {

            Article article = db.Articles.Find(id);
            if (article != null)
            {
                // remove the uploaded files on the server side.
                foreach (Attachement attach in article.Attachements)
                {
                    string uploadDirectory = System.Web.HttpContext.Current.Server.MapPath("~");
                    uploadDirectory = Path.Combine(uploadDirectory + "Uploads");

                    DirectoryInfo dirInfo = new DirectoryInfo(uploadDirectory);
                    FileInfo[] Files = dirInfo.GetFiles("*.*");

                    foreach (FileInfo file in Files)
                    {
                        if (file.Name == attach.Name)
                        {
                            file.Delete();
                        }
                    }
                }
            }          
        }
        


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(string id)
        {
            return db.Articles.Count(e => e.ArticleId == id) > 0;
        }
    }
}