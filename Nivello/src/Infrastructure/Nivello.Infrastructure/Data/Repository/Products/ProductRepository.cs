﻿using Microsoft.EntityFrameworkCore;
using Nivello.Infrastructure.Data.Context;
using Nivello.Infrastructure.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nivello.Infrastructure.Data.Repository.Products
{
    [ExcludeFromCodeCoverage]
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Product> _dataset;
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dataset = dataContext.Set<Product>();
        }
        public async Task CreatedAsync(Product product)
        {
            await _dataset.AddAsync(product);
            _dataContext.SaveChangesAsync().Wait();
        }

        public async Task UpdateAsync(Product product)
        {
            _dataset.Update(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Product> FirstOrDefaultAsync(Expression<Func<Product, bool>> filter)
        {
            return await _dataset.Include(fk => fk.SystemAdmin)
                .Include(fk2 => fk2.Bids).SingleOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<Product>> WhereAsync(Expression<Func<Product, bool>> filter)
        {
            return await _dataset.AsQueryable().Where(filter).ToListAsync();
        }
    }
}
