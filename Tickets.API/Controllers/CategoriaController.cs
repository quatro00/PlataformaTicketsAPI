using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Models.DTO.Departamento;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;
using Tickets.API.Helpers;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly ICategoriaRepository categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            this.categoriaRepository = categoriaRepository;
        }

        [HttpGet("GetCategorias")]
        [Authorize(Roles = "Agente,Administrador,Supervisor,Cliente")]
        public async Task<IActionResult> GetCategorias()
        {

            var response = await categoriaRepository.GetCategorias(Guid.Parse(User.GetSucursalId()));
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
            var response = await categoriaRepository.GetAllAsync();

            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(CategoriaDto request)
        {
            var response = await categoriaRepository.CreateAsync(request);

            return Ok(response);
        }
        [HttpPost("AsignarEquipo")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarEquipo(AsignarEquipoDto request)
        {
            var response = await categoriaRepository.AsignarEquipo(request);
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response);
        }

        [HttpPost("DesasignarEquipo")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesasignarEquipo(AsignarEquipoDto request)
        {
            var response = await categoriaRepository.DesasignarEquipo(request);
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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoriaDto request)
        {

            var updatedItem = await categoriaRepository.UpdateAsync(request, id);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
    }
}
