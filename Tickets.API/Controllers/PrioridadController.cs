using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Departamento;
using Tickets.API.Models.DTO.Prioridad;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;
using Tickets.API.Helpers;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioridadController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly IPrioridadRepository prioridadRepository;

        public PrioridadController(IPrioridadRepository prioridadRepository)
        {
            this.prioridadRepository = prioridadRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
            //User.FindFirst("Id").Value
            var response = await prioridadRepository.GetAllAsync();

            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(PrioridadDto request)
        {

            var response = await prioridadRepository.CreateAsync(request);
           
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PrioridadDto request)
        {
            var updatedItem = await prioridadRepository.UpdateAsync(request, id);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
        [HttpGet("GetPrioridades")]
        [Authorize(Roles = "Agente,Administrador,Supervisor,Cliente")]
        public async Task<IActionResult> GetPrioridades()
        {

            var response = await prioridadRepository.GetPrioridades(Guid.Parse(User.GetSucursalId()));
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }
    }
}
