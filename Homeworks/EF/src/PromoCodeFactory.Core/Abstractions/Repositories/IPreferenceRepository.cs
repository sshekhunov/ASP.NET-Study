using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPreferenceRepository : IRepository<Preference>
    {
        Task<IList<Preference>> GetByIdsAsync(IList<Guid> ids);
    }
}
