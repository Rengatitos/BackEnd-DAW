using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using System.Threading.Tasks;

namespace Onboarding.Api.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public class CatalogoOnboardingController : ControllerBase
    {
        private readonly ICatalogoOnboardingService _service;

        public CatalogoOnboardingController(ICatalogoOnboardingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogo()
        {
            var c = await _service.GetCatalogoAsync();
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpGet("{etapa}")]
        public async Task<IActionResult> GetEtapa(string etapa)
        {
            var e = await _service.GetEtapaAsync(etapa);
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCatalogo([FromBody] CatalogoOnboardingCreateDTO dto)
        {
            try
            {
                await _service.CreateCatalogoAsync(dto);
                return CreatedAtAction(nameof(GetCatalogo), null);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{etapa}")]
        public async Task<IActionResult> UpdateEtapa(string etapa, [FromBody] CatalogoOnboardingUpdateEtapaDTO dto)
        {
            var updated = await _service.UpdateEtapaAsync(etapa, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{etapa}")]
        public async Task<IActionResult> DeleteEtapa(string etapa)
        {
            var deleted = await _service.DeleteEtapaAsync(etapa);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}