using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.Core.Abstractions.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly IPreferenceRepository _preferenceRepository;

        public PreferencesController(IPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получает список всех предпочтений
        /// </summary>
        /// <returns>Список предпочтений</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();
            var response = preferences.Select(p => new PreferenceResponse
            {
                Id = p.Id,
                Name = p.Name
            });

            return Ok(response);
        }
    }
}
