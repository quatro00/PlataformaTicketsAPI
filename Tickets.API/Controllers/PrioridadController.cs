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
    }
}
