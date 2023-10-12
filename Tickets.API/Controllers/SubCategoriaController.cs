using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Tickets.API.Data;
using Tickets.API.Helpers;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Models.DTO.SubCategoria;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriaController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly ISubCategoriaRepository subCategoriaRepository;

        public SubCategoriaController(ISubCategoriaRepository subCategoriaRepository)
        {
            this.subCategoriaRepository = subCategoriaRepository;
        }
        [HttpGet("GetSubCategorias/{id:Guid}")]
        [Authorize(Roles = "Agente,Administrador,Supervisor,Cliente")]
        public async Task<IActionResult> GetSubCategorias([FromRoute] Guid id)
        {

            var response = await subCategoriaRepository.GetSubCategorias(id);
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
            var response = await subCategoriaRepository.GetAllAsync();

            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(SubCategoriaDto request)
        {
            var response = await subCategoriaRepository.CreateAsync(request);
            //Claims
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SubCategoriaDto request)
        {

            var updatedItem = await subCategoriaRepository.UpdateAsync(request, id);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
    }
}
