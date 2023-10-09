using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Data;
using Tickets.API.Helpers;
using Tickets.API.Models.DTO.Equipo;
using Tickets.API.Models.DTO.Prioridad;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly IEquipoRepository equipoRepository;

        public EquipoController(IEquipoRepository equipoRepository)
        {
            this.equipoRepository = equipoRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
            //User.FindFirst("Id").Value
            var response = await equipoRepository.GetAll();
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(EquipoDto request)
        {

            var response = await equipoRepository.CreateAsync(request, User.GetId());
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] EquipoDto request)
        {
            var updatedItem = await equipoRepository.UpdateAsync(request, id);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
        [HttpPost("AgregarAgente")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarAgente(AsignarUsuarioDto request)
        {
            request.EsSupervisor = false;
            var response = await equipoRepository.AsignarUsuario(request);
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }
        [HttpPost("AgregarSupervisor")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarSupervisor(AsignarUsuarioDto request)
        {
            request.EsSupervisor = true;
            var response = await equipoRepository.AsignarUsuario(request);
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }
        [HttpPost("DesasignarUsuario")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesasignarUsuario(AsignarUsuarioDto request)
        {
            request.EsSupervisor = true;
            var response = await equipoRepository.DesasignarUsuario(request);
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }
    }
}
