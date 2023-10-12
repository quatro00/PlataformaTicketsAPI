using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Data;
using Tickets.API.Helpers;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Departamento;
using Tickets.API.Models.DTO.Sucursal;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly IDepartamentoRepository departamentoRepository;

        public DepartamentoController(IDepartamentoRepository departamentoRepository)
        {
            this.departamentoRepository = departamentoRepository;
        }
        [HttpGet("GetDepartamentos")]
        [Authorize(Roles = "Agente,Administrador,Supervisor,Cliente")]
        public async Task<IActionResult> GetDepartamentos()
        {

            var departamentos = await departamentoRepository.GetDepartamentos(Guid.Parse(User.GetSucursalId()));

            if (!departamentos.response)
            {
                ModelState.AddModelError("error", departamentos.message);
                return ValidationProblem(ModelState);
            }

            List<GetDepartamentoResponseDto> response = new List<GetDepartamentoResponseDto>();
            foreach (var item in departamentos.result)
            {
                response.Add(new GetDepartamentoResponseDto()
                {
                    id = item.Id,
                    sucursalId = item.Sucursal.Id,
                    sucursal = item.Sucursal.Nombre,
                    sucursalClave = item.Sucursal.Clave,
                    clave = item.Clave,
                    telefono = item.Telefono,
                    descripcion = item.Descripcion,
                    activo = item.Activo,
                });
            }


            return Ok(response);
        }




        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
           
            var departamentos = await departamentoRepository.GetAllAsync();
             List<GetDepartamentoResponseDto> response = new List<GetDepartamentoResponseDto>();
            foreach (var item in departamentos)
            {
                response.Add(new GetDepartamentoResponseDto()
                {
                    id = item.Id,
                    sucursalId = item.Sucursal.Id,
                    sucursal = item.Sucursal.Nombre,
                    sucursalClave = item.Sucursal.Clave,
                    clave = item.Clave,
                    telefono = item.Telefono,
                    descripcion = item.Descripcion,
                    activo = item.Activo,
                });
            }
            

            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(CreateDepartamentoDto request)
        {

            Departamento departamento = await departamentoRepository.CreateAsync(request);
            DepartamentoDto departamentoDto = new DepartamentoDto()
            {
                Clave = departamento.Clave,
                Descripcion = departamento.Descripcion,
                Telefono = departamento.Telefono,
                Activo = departamento.Activo,
                SucursalId = departamento.SucursalId
            };
            return Ok(departamentoDto);
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDepartamentoDto request)
        {
            var updatedItem = await departamentoRepository.UpdateAsync(request, id);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
    }
}
