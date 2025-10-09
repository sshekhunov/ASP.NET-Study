using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPromoCodeRepository : IRepository<PromoCode>
    {
        Task<IList<PromoCode>> GetAllWithDetailsAsync();
    }
}
