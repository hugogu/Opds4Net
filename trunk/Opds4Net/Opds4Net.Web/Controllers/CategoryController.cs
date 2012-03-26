using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Opds4Net.Web.Models;
using PagedList;

namespace Opds4Net.Web.Controllers
{ 
    public class CategoryController : Controller
    {
        private BookDBContext db = new BookDBContext();

        //
        // GET: /Category/

        public ViewResult Index(int? page, int? pageSize)
        {
            page = page ?? 1;
            pageSize = pageSize ?? 15;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            var orderedCategores = db.Categories.OrderByDescending(c => c.Id);
            var categories = orderedCategores.ToPagedList(page.Value, pageSize.Value);

            return View(categories);
        }

        //
        // GET: /Category/Details/5

        public ViewResult Details(Guid id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create(bool? leaf)
        {
            InitializeCategoryViewBag(leaf, null);

            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category, bool? leaf)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                db.Categories.Add(category);
                // Otherwise EF will try to insert the parent and failed.
                db.Entry(category.Parent).State = EntityState.Unchanged;
                db.SaveChanges();

                return RedirectToAction("Index");  
            }
            InitializeCategoryViewBag(leaf, null);

            return View(category);
        }
        
        //
        // GET: /Category/Edit/5
 
        public ActionResult Edit(Guid id, bool? leaf)
        {
            Category category = db.Categories.Include(c => c.Parent).Single(c => c.Id == id);
            InitializeCategoryViewBag(leaf, category.Parent);

            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category, bool? leaf)
        {
            var existing = db.Categories.Include(c => c.Parent).Single(c => c.Id == category.Id);
            if (ModelState.IsValid)
            { 
                existing.Name = category.Name;
                category.Parent = db.Categories.Find(category.Parent.Id);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            InitializeCategoryViewBag(leaf, existing.Parent);

            return View(category);
        }

        //
        // GET: /Category/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeCategoryViewBag(bool? leaf, Category parent)
        {
            ViewBag.LeafNodeOnly = leaf ?? false;
            ViewBag.Categories = new SelectList(db.PickCategoriesWithEmpty.OrderBy(c => c.FullName), "Id", "FullName", parent == null ? db.NoCategory : db.PickCategories.Find(parent.Id));
        }
    }
}