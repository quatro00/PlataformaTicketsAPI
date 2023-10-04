using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Sucursal;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public SucursalRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<Sucursal> CreateAsync(CreateSucursalRequestDto request)
        {
            Sucursal sucursal = new Sucursal()
            {
                Id = Guid.NewGuid(),
                Clave = request.clave,
                Nombre =request.nombre,
                Direccion = request.direccion,
                Telefono = request.telefono,
                Telefono2 = request.telefono2,
                Activo=request.activo,
                FechaCreacion = DateTime.Now,
                UsuarioCreacionId = Guid.NewGuid(),
            };

            await this.ticketsDbContext.Sucursals.AddAsync(sucursal);
            await this.ticketsDbContext.SaveChangesAsync();
            return sucursal;

           // throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sucursal>> GetAllAsync()
        {
            return await ticketsDbContext.Sucursals.ToListAsync();
        }

        public async Task<Sucursal> UpdateAsync(UpdateSucursalRequestDto request, Guid id)
        {

            var existingItem = await ticketsDbContext.Sucursals.FindAsync(id);
            

            if(existingItem == null)
            {
                return null;
            }
            existingItem.Clave = request.clave;
            existingItem.Nombre = request.nombre;
            existingItem.Direccion = request.direccion;
            existingItem.Telefono = request.telefono;
            existingItem.Telefono2 = request.telefono2;
            existingItem.FechaModificacion = DateTime.Now;

            //ticketsDbContext.Entry(existingItem).CurrentValues.SetValues(sucursal);

            await ticketsDbContext.SaveChangesAsync();

            return existingItem;
        }
    }
}
