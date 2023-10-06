using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Departamento;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public DepartamentoRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<Departamento> CreateAsync(CreateDepartamentoDto request)
        {
            Departamento model = new Departamento()
            {
                Id = Guid.NewGuid(),
                Clave = request.Clave,
                Descripcion = request.Descripcion,
                Telefono = request.Telefono,
                SucursalId = request.SucursalId,
                Activo = request.Activo,
                FechaCreacion = DateTime.Now,
                UsuarioCreacionId = Guid.NewGuid(),
            };

            await this.ticketsDbContext.Departamentos.AddAsync(model);
            await this.ticketsDbContext.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await ticketsDbContext.Departamentos.Include(x=>x.Sucursal).ToListAsync();
        }

        public async Task<Departamento> UpdateAsync(UpdateDepartamentoDto request, Guid id)
        {
            var existingItem = await ticketsDbContext.Departamentos.FindAsync(id);


            if (existingItem == null)
            {
                return null;
            }
            existingItem.Clave = request.Clave;
            existingItem.Descripcion = request.Descripcion;
            existingItem.Telefono = request.Telefono;
            existingItem.SucursalId = request.SucursalId;
            existingItem.FechaModificacion = DateTime.Now;

            //ticketsDbContext.Entry(existingItem).CurrentValues.SetValues(sucursal);

            await ticketsDbContext.SaveChangesAsync();

            return existingItem;
        }
    }
}
