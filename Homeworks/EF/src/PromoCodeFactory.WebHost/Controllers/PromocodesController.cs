using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPreferenceRepository _preferenceRepository;

        public PromocodesController(
            IPromoCodeRepository promoCodeRepository,
            ICustomerRepository customerRepository,
            IPreferenceRepository preferenceRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получает список всех промокодов
        /// </summary>
        /// <returns>Список промокодов</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodeRepository.GetAllWithDetailsAsync();
            var response = promoCodes.Select(pc => new PromoCodeShortResponse
            {
                Id = pc.Id,
                Code = pc.Code,
                ServiceInfo = pc.ServiceInfo,
                BeginDate = pc.BeginDate.ToString("dd.MM.yyyy"),
                EndDate = pc.EndDate.ToString("dd.MM.yyyy"),
                PartnerName = pc.PartnerName
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Создает промокод и выдает его клиентам с указанным предпочтением
        /// </summary>
        /// <param name="request">Данные для создания промокода</param>
        /// <returns>Результат создания промокодов</returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = await _customerRepository.GetCustomersByPreferenceNameAsync(request.Preference);
            
            if (!customers.Any())
            {
                return NotFound($"Клиенты с предпочтением '{request.Preference}' не найдены");
            }

            var preference = await _preferenceRepository.GetAllAsync();
            var targetPreference = preference.FirstOrDefault(p => p.Name == request.Preference);
            
            if (targetPreference == null)
            {
                return NotFound($"Предпочтение '{request.Preference}' не найдено");
            }

            var currentDate = DateTime.Now;

            foreach (var customer in customers)
            {
                var promoCode = new PromoCode
                {
                    Id = Guid.NewGuid(),
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    BeginDate = currentDate,
                    EndDate = currentDate.AddDays(30),
                    PartnerName = request.PartnerName,
                    CustomerId = customer.Id,
                    Customer = customer,
                    Preference = targetPreference
                };

                await _promoCodeRepository.AddAsync(promoCode);
            }

            return Ok();
        }
    }
}