using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Sucursal;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        private readonly ISucursalRepository sucursalRepository;
        public SucursalController(ISucursalRepository sucursalRepository)
        {
            this.sucursalRepository = sucursalRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSucursales()
        {
            var sucursales = await sucursalRepository.GetAllAsync();
            List<SucursalDto> response = new List<SucursalDto>();
            foreach (var item in sucursales) {
                response.Add(new SucursalDto() { 
                Id = item.Id,
                Clave = item.Clave,
                Nombre = item.Nombre,
                Direccion = item.Direccion,
                Telefono = item.Telefono,
                Telefono2 = item.Telefono2,
                Activo = item.Activo,
                });
            }
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateSucursal(CreateSucursalRequestDto request) 
        {
            Sucursal sucursal = await sucursalRepository.CreateAsync(request);
            SucursalDto sucursalDto = new SucursalDto()
            {
                Clave = sucursal.Clave,
                Nombre= sucursal.Nombre,
                Direccion= sucursal.Direccion,
                Telefono= sucursal.Telefono,
                Telefono2= sucursal.Telefono2,
                Activo= sucursal.Activo,
                Id = sucursal.Id
            };
            return Ok(sucursal);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateSucursal([FromRoute] Guid id,[FromBody] UpdateSucursalRequestDto request)
        {
            var updatedItem = await sucursalRepository.UpdateAsync(request, id);

            if(updatedItem == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
    }
}
