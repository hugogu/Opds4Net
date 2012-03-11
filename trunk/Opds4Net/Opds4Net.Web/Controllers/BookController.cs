using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opds4Net.Web.Models;

namespace Opds4Net.Web.Controllers
{ 
    public class BookController : Controller
    {
        private BookDBContext db = new BookDBContext();

        //
        // GET: /Book/

        public ViewResult Index()
        {
            return View(db.Books.ToList());
        }

        //
        // GET: /Book/Details/5

        public ViewResult Details(Guid id)
        {
            Book book = db.Books.Find(id);
            return View(book);
        }

        //
        // GET: /Book/Create

        public ActionResult Create()
        {
            ViewBag.Categories = db.PickCategories;

            return View();
        } 

        //
        // POST: /Book/Create

        [HttpPost]
        public ActionResult Create(Book book, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                db.Books.Add(book);
                var guids = selectedCategories.Select(s => new Guid(s));
                book.Categories = db.Categories.Where(c => guids.Contains(c.Id)).ToList();
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(book);
        }
        
        //
        // GET: /Book/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            ViewBag.Categories = db.PickCategories;
            Book book = db.Books.Include(b => b.Categories).Single(b => b.Id == id);
            return View(book);
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        public ActionResult Edit(Book book, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                var guids = selectedCategories.Select(s => new Guid(s));
                book.Categories = db.Categories.Where(c => guids.Contains(c.Id)).ToList();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
    }
}