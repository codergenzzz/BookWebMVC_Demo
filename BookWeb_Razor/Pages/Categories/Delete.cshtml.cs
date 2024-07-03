using BookWeb_Razor.Data;
using BookWeb_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWeb_Razor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category? Category { get; set; }
        public DeleteModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public void OnGet(int? id)
        {
            if (id != null || id != 0)
            {
                Category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (Category != null)
            {
                Category? obj = _db.Categories.Find(Category.Id);
                if (obj == null)
                {
                    return NotFound();
                }

                _db.Categories.Remove(obj);
            }

            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";


            return RedirectToPage("Index");
        }
    }
}
