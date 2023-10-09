using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Helpers;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Models.DTO.Usuario;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(RequestUsuarioDto request)
        {
            var response = await usuarioRepository.CreateAsync(request, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            
            return Ok(response.result);
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUsuarioDto request)
        {
            var response = await usuarioRepository.UpdateAsync(request, id);

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
            var response = await usuarioRepository.GetAll();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet("GetAgentes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAgentes([FromQuery] RequestGetAgenteDto request)
        {
            var response = await usuarioRepository.GetAgentes(request.SucursalId);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
            }
        [HttpGet("GetSupervisores")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetSupervisores([FromQuery] RequestGetAgenteDto request)
        {
            var response = await usuarioRepository.GetSupervisores(request.SucursalId);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
