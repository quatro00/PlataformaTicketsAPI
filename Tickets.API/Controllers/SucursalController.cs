using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Sucursal;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly TicketsDbContext dbContext;
        public SucursalController(TicketsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSucursal(CreateSucursalRequestDto request) 
        {
            var sucursal = new Sucursal() {
                Id = Guid.NewGuid(),
                Clave = request.Clave,
                Nombre = request.Nombre,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Telefono2 = request.Telefono2,
                Activo = request.Activo,
                FechaCreacion = DateTime.Now,
                UsuarioCreacionId = Guid.NewGuid(),
            };

            await dbContext.Sucursals.AddAsync(sucursal);
            await dbContext.SaveChangesAsync();

            var response = new SucursalDto()
            {
                Clave = sucursal.Clave,
                Nombre = sucursal.Nombre,
                Direccion = sucursal.Direccion,
                Telefono = sucursal.Telefono,
                Telefono2 = sucursal.Telefono2,
                Activo = sucursal.Activo
            };

            return Ok(response);
        }
    }
}
