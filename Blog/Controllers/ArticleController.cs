using Blog.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.Models.Extensions;
using System.Data.Entity.Infrastructure;

namespace Blog.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        //
        // GET: Article
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: Article/List
        [AllowAnonymous]
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var articles = database.Articles
                    .Include(a => a.Author).OrderByDescending(b => b.Id)
                    .ToList();

                return View(articles);
            }
        }

        //
        // GET: Article/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }
        // GET: Article/Edit/1
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var article = database.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(article);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var courseToUpdate = database.Articles.Find(id);
                if (TryUpdateModel(courseToUpdate, "",
                   new string[] { "Title", "Content" }))
                {
                    try
                    {
                        database.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }

                return View(courseToUpdate);
            }
        }
        //
        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }


        //
        // POST: Article/Create
        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    article.AuthorId = authorId;
                    database.Articles.Add(article);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(article);
        }

        // GET: /Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new BlogDbContext())
            {
                var currArticle = db.Articles.Find(id);
                if (currArticle == null)
                {
                    return HttpNotFound();
                }
                return View(currArticle);
            }
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new BlogDbContext())
            {
                var currArticle = db.Articles.Find(id);
                db.Articles.Remove(currArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}