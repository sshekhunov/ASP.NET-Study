using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : EFRepository<Customer>, ICustomerRepository
    {
        private readonly PromoDbContext _context;

        public CustomerRepository(PromoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdWithPromoCodesAsync(Guid id)
        {
            return await _context.Customers
                .Include(c => c.PromoCodes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IList<CustomerPreference>> GetCustomerPreferencesAsync(Guid customerId)
        {
            return await _context.CustomerPreferences
                .Include(cp => cp.Preference)
                .Where(cp => cp.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddCustomerPreferencesAsync(IList<CustomerPreference> preferences)
        {
            await _context.CustomerPreferences.AddRangeAsync(preferences);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCustomerPreferencesAsync(Guid customerId)
        {
            var preferences = await _context.CustomerPreferences
                .Where(cp => cp.CustomerId == customerId)
                .ToListAsync();
            
            _context.CustomerPreferences.RemoveRange(preferences);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCustomerPromoCodesAsync(Guid customerId)
        {
            var promoCodes = await _context.PromoCodes
                .Where(pc => pc.CustomerId == customerId)
                .ToListAsync();
            
            _context.PromoCodes.RemoveRange(promoCodes);
            await _context.SaveChangesAsync();
        }
    }
}
