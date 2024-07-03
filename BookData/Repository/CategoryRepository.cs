using BookWeb.DataAccess.Repository.IRepository;
using BookWeb.DataAccess.Data;
using BookWeb.Models;

namespace BookWeb.DataAccess.Repository
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        //private ICategoryRepository _categoryRepositoryImplementation;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Category cte)
        {
            _dbContext.Update(cte);
        }

    }
}
