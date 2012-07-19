using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opds4Net.Util;
using Opds4Net.Util.Extension;
using Opds4Net.Web.Models;
using PagedList;

namespace Opds4Net.Web.Controllers
{
    public class BookController : Controller
    {
        private BookDBContext db = new BookDBContext();

        //
        // GET: /Book/

        public ViewResult Index(string orderBy, bool? direction, int? page, int? pageSize)
        {
            pageSize = pageSize ?? 10;
            ViewBag.PageSize = pageSize;
            ViewBag.OrderBy = orderBy;
            ViewBag.Direction = direction ?? false;
            ViewBag.Page = page ?? 1;
            IEnumerable<Book> sortedBooks;
            if (direction.HasValue && direction.Value)
                sortedBooks = db.Books.OrderBy(orderBy);
            else
                sortedBooks = db.Books.OrderByDescending(orderBy);
            var pagedBooks = sortedBooks.ToPagedList(page ?? 1, pageSize.Value);

            return View(pagedBooks);
        }

        //
        // GET: /Book/Details/5

        public ActionResult Details(Guid id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
                return new HttpNotFoundResult();

            return View(book);
        }

        //
        // GET: /Book/Create

        [Authorize]
        public ActionResult Create(bool? leaf)
        {
            InitializeBookPageViewBag(leaf);

            return View();
        }

        //
        // POST: /Book/Create

        [HttpPost]
        public ActionResult Create(Book book, string[] selectedCategories, bool? leaf)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                if (UploadFile(Request, book))
                {

                    book.UpdateTime = DateTime.Now;
                    db.Books.Add(book);
                    if (selectedCategories != null && selectedCategories.Any())
                    {
                        var guids = selectedCategories.Select(s => new Guid(s));
                        book.Categories = db.Categories.Where(c => guids.Contains(c.Id)).ToList();
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            InitializeBookPageViewBag(leaf);

            return View(book);
        }

        //
        // GET: /Book/Edit/5

        public ActionResult Edit(Guid id, bool? leaf)
        {
            InitializeBookPageViewBag(leaf);
            Book book = db.Books.Include(b => b.Categories).Single(b => b.Id == id);

            return View(book);
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        public ActionResult Edit(Book book, string[] selectedCategories, bool? leaf)
        {
            if (ModelState.IsValid)
            {
                if (UploadFile(Request, book))
                {
                    book.UpdateTime = DateTime.Now;
                    db.Entry(book).State = EntityState.Modified;
                }
                else
                    ViewBag.ErrorInfo = "文件上传失败";

                var guids = selectedCategories.Select(s => new Guid(s));
                if (guids.Any())
                {
                    book.Categories = db.Categories.Where(c => guids.Contains(c.Id)).ToList();
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            InitializeBookPageViewBag(leaf);
            book = db.Books.Include(b => b.Categories).Single(b => b.Id == book.Id);

            return View(book);
        }

        //
        // GET: /Book/Delete/5

        public ActionResult Delete(Guid id)
        {
            Book book = db.Books.Find(id);
            return View(book);
        }

        //
        // POST: /Book/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeBookPageViewBag(bool? leaf)
        {
            ViewBag.LeafNodeOnly = leaf ?? false;
            ViewBag.Categories = db.PickCategories
                .Include(c => c.SubCategories)
                .OrderBy(c => c.FullName)
                .Where(c => !leaf.Value || c.SubCategories.Count == 0);
        }

        private bool UploadFile(HttpRequestBase request, Book book)
        {
            // Files count should be 1.
            foreach (string file in Request.Files)
            {
                if (Request.Files[file] == null || Request.Files[file].ContentLength == 0)
                {
                    continue;
                }

                book.DownloadAddress = MvcApplication.Current.ContentSaver
                    .Store(book.Id + Path.GetExtension(Request.Files[file].FileName), Request.Files[file].InputStream);
                book.MimeType = OpdsHelper.DetectFileMimeType(Request.Files[file].FileName);
                book.FileSize = Request.Files[file].ContentLength;

                return true;
            }

            return false;
        }
    }
}