using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PromoCodeRepository : EFRepository<PromoCode>, IPromoCodeRepository
    {
        private readonly PromoDbContext _context;

        public PromoCodeRepository(PromoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<PromoCode>> GetAllWithDetailsAsync()
        {
            return await _context.PromoCodes
                .Include(pc => pc.Customer)
                .Include(pc => pc.Preference)
                .ToListAsync();
        }
    }
}
