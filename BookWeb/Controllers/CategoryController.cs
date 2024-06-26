﻿using BookWeb.Data;
using BookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        // Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";

                return RedirectToAction("Index");
            }
            return View();

        }

        // Update
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? cteCategory = _db.Categories.Find(id);
            //Category? cteCategory1 = _db.Categories.FirstOrDefault(cte => cte.Id == id);
            //Category? cteCategory2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (cteCategory == null)
            {
                return NotFound();
            }

            return View(cteCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";

                return RedirectToAction("Index");
            }
            return View();

        }

        // Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? cteCategory = _db.Categories.Find(id);

            if (cteCategory == null)
            {
                return NotFound();
            }

            return View(cteCategory);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index");

        }
    }
}
