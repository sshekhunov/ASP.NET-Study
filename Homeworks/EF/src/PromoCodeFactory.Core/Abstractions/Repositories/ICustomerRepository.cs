using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByIdWithPromoCodesAsync(Guid id);
        Task<IList<CustomerPreference>> GetCustomerPreferencesAsync(Guid customerId);
        Task AddCustomerPreferencesAsync(IList<CustomerPreference> preferences);
        Task RemoveCustomerPreferencesAsync(Guid customerId);
        Task RemoveCustomerPromoCodesAsync(Guid customerId);
    }
}
