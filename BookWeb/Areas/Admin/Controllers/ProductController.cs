using BookWeb.DataAccess.Repository.IRepository;
using BookWeb.Models;
using BookWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.
                ProductRepository.GetAll(includeProperties:"Category").ToList();
           
            return View(objProductList);
        }

        // Create
        public IActionResult Upsert(int? id) // Update and Insert
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository
            //    .GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()

            //    });
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"]= CategoryList;

            ProductVM productVm = new()
            {
                CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()

                    }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // create
                return View(productVm);

            }
            else
            {
                // update
                productVm.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                return View(productVm);
            }

        }

        //[HttpPost]
        //public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath = Path.Combine(wwwRootPath, @"\images\product");

        //            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        //            {

        //                file.CopyTo(fileStream);
        //            }

        //            productVm.Product.ImageUrl = @"\image\product\" + fileName;
        //        }

        //        _unitOfWork.
        //            ProductRepository.Add(productVm.Product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully";

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {

        //        productVm.CategoryList = _unitOfWork.CategoryRepository
        //            .GetAll().Select(u => new SelectListItem
        //            {
        //                Text = u.Name,
        //                Value = u.Id.ToString()
        //            });
        //        return View(productVm);

        //    }
        //}

        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!Directory.Exists(productPath))
                {
                    Directory.CreateDirectory(productPath);
                }

                if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                {
                    // delete old img
                    var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\').TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVm.Product.ImageUrl = @"/images/product/" + fileName;
            }

            // Remove ImageUrl from ModelState as it has been set programmatically
            ModelState.Remove("Product.ImageUrl");

            if (ModelState.IsValid)
            {
                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVm.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVm.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product saved successfully";
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form with categories
            productVm.CategoryList = _unitOfWork.CategoryRepository
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(productVm);
        }



        //// Update
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
        //    //Product? product1 = _db.Categories.FirstOrDefault(cte => cte.Id == id);
        //    //Product? product2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.
        //            ProductRepository.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully";

        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        // Delete
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product? product = _unitOfWork.
        //        ProductRepository.Get(u => u.Id == id);

        //    return View(product);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.
        //        ProductRepository.Get(u => u.Id == id);

        //    _unitOfWork.
        //        ProductRepository.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted successfully";

        //    return RedirectToAction("Index");

        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    productToBeDeleted.ImageUrl.TrimStart('\\').TrimStart('/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
