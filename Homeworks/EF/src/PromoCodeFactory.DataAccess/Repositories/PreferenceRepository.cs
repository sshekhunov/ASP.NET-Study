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
    public class PreferenceRepository : EFRepository<Preference>, IPreferenceRepository
    {
        private readonly PromoDbContext _context;

        public PreferenceRepository(PromoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Preference>> GetByIdsAsync(IList<Guid> ids)
        {
            return await _context.Preferences
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
    }
}
