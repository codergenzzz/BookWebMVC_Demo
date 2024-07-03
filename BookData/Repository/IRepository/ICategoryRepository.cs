using BookWeb.Models;

namespace BookWeb.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        void Update(Category cte);
    }
}
