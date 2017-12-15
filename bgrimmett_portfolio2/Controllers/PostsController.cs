using bgrimmett_portfolio2.Helpers;
using bgrimmett_portfolio2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace bgrimmett_portfolio2.Controllers
{
    [RequireHttps]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? page, int? Id)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            Post post = db.Posts.Find(Id);

            return View(db.Posts.OrderByDescending(p => p.Id).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SearchResults(string searchitem)
        {

            if (searchitem == null)
            {
                return HttpNotFound();
            }

            return View(db.Posts.Where(i => i.Title.Contains(searchitem) || i.Slug.Contains(searchitem) || i.Body.Contains(searchitem)).ToList());


        }
        // GET: Posts/Details/5
        public ActionResult Details(string slug)
        {

            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p => p.Slug == slug);


            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,CreationDate,UpdatedDate,Title,Body,MediaURL, Published")] Post post, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)   //Validating the image
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format.");
            }
            if (ModelState.IsValid)
            {
                //Create Slug Here
                var Slug = StringUtilities.UrlFriendly(post.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(post);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(post);
                }


                if (image != null)
                {
                    var filePath = "/template/uploads/";  //adding the file to the database
                    var absPath = Server.MapPath("~" + filePath);
                    post.MediaURL = filePath + image.FileName;   //specifies the path of the file
                    image.SaveAs(Path.Combine(absPath, image.FileName)); //saves the file. necessary for the specified path to have something to point at
                }

                post.Slug = Slug;
                post.CreationDate = System.DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreationDate,UpdatedDate,Title,Body,MediaURL")] Post post, string slug)
        {
            if (ModelState.IsValid)
            {
                post.Slug = slug;
                db.Entry(post).State = EntityState.Modified;
                post.UpdatedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string slug)
        {
            Post post = db.Posts.FirstOrDefault(p => p.Slug == slug);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        //[RequireHttps]
        [ValidateAntiForgeryToken]
        public ActionResult CommentCreate([Bind(Include = "Id,PostId,Body,Created,AuthorId")] Comment comment)
        { // only pass in the bind the attributes that have forms
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(comment.Body))
                {
                    var post = db.Posts.Find(comment.PostId);
                    comment.CreationDate = System.DateTime.Now;
                    comment.AuthorId = User.Identity.GetUserId();
                    db.Comments.Add(comment);
                    db.SaveChanges();


                    return RedirectToAction("Details", new { Slug = post.Slug });
                }
            }

            return RedirectToAction("Index");
        }




    }
}
