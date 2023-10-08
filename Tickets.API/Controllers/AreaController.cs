using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            this.areaRepository = areaRepository;
        }

        [HttpPost("CreateAreaBase")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateAreaBase(CreateAreaBaseRequestDto request)
        {
            var response = await areaRepository.CreateAreaBaseAsync(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
        /*
        [HttpPost("CreateAreaBase")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateAreaAnidada(CreateAreaBaseRequestDto request)
        {
            var response = await areaRepository.CreateAreaBaseAsync(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
        */
        [HttpGet("GetAreasBaseByDepartamento")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAreasBaseByDepartamento([FromQuery] GetAreasByDepartamentoRequestDto request)
        {
            var response = await areaRepository.GetAreasByDepartamento(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
