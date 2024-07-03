﻿using BookWeb.DataAccess.Data;
using BookWeb.DataAccess.Repository.IRepository;

namespace BookWeb.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _dbContext;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository{ get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        CategoryRepository = new CategoryRepository(_dbContext);
        ProductRepository = new ProductRepository(_dbContext);
    }
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}