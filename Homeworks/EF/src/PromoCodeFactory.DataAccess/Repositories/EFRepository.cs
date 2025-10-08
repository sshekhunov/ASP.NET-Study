using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EFRepository<T>: IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entitySet;

        public EFRepository(DbContext context)
        {
            _context = context;
            _entitySet = _context.Set<T>();
        }

        public Task<IList<T>> GetAllAsync()
        {
            return null;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entitySet.FirstAsync(o => o.Id == id);
        }

        public async Task AddAsync(T entity)
        {
            await _entitySet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _entitySet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _entitySet.FirstOrDefaultAsync(o => o.Id == id);
            if (entity != null)
            {
                _entitySet.Remove(entity);                
            }
            await _context.SaveChangesAsync();
        }
    }
}