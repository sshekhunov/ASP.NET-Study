using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPreferenceRepository _preferenceRepository;

        public CustomersController(ICustomerRepository customerRepository, IPreferenceRepository preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получает список всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var response = customers.Select(c => new CustomerShortResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            });

            return Ok(response);
        }

        /// <summary>
        /// Получает клиента по идентификатору с промокодами и предпочтениями
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Данные клиента с промокодами и предпочтениями</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdWithPromoCodesAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerPreferences = await _customerRepository.GetCustomerPreferencesAsync(id);

            var response = new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = customerPreferences.Select(cp => cp.Preference).ToList(),
                PromoCodes = customer.PromoCodes.Select(pc => new PromoCodeShortResponse
                {
                    Id = pc.Id,
                    Code = pc.Code,
                    ServiceInfo = pc.ServiceInfo,
                    BeginDate = pc.BeginDate.ToString("dd.MM.yyyy"),
                    EndDate = pc.EndDate.ToString("dd.MM.yyyy"),
                    PartnerName = pc.PartnerName
                }).ToList()
            };

            return Ok(response);
        }

        /// <summary>
        /// Создает нового клиента с предпочтениями
        /// </summary>
        /// <param name="request">Данные для создания клиента</param>
        /// <returns>Результат создания клиента</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            await _customerRepository.AddAsync(customer);

            if (request.PreferenceIds != null && request.PreferenceIds.Any())
            {
                var preferences = await _preferenceRepository.GetByIdsAsync(request.PreferenceIds);

                var customerPreferences = preferences.Select(p => new CustomerPreference
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    PreferenceId = p.Id,
                    Customer = customer,
                    Preference = p
                }).ToList();

                await _customerRepository.AddCustomerPreferencesAsync(customerPreferences);
            }

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Обновляет данные клиента и его предпочтения
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns>Результат обновления клиента</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            await _customerRepository.UpdateAsync(customer);

            await _customerRepository.RemoveCustomerPreferencesAsync(id);

            if (request.PreferenceIds != null && request.PreferenceIds.Any())
            {
                var preferences = await _preferenceRepository.GetByIdsAsync(request.PreferenceIds);

                var customerPreferences = preferences.Select(p => new CustomerPreference
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    PreferenceId = p.Id,
                    Customer = customer,
                    Preference = p
                }).ToList();

                await _customerRepository.AddCustomerPreferencesAsync(customerPreferences);
            }

            return NoContent();
        }

        /// <summary>
        /// Удаляет клиента вместе с его предпочтениями и промокодами
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Результат удаления клиента</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.RemoveCustomerPreferencesAsync(id);
            await _customerRepository.RemoveCustomerPromoCodesAsync(id);
            await _customerRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}