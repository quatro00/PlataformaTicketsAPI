using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Prioridad;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class PrioridadRepository : IPrioridadRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public PrioridadRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }
        public async Task<PrioridadDto> CreateAsync(PrioridadDto request)
        {
            request.Id = Guid.NewGuid();
            Prioridad model = new Prioridad()
            {
                Id = request.Id,
                SucursalId = request.SucursalId,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                TiempoDeAtencion = request.TiempoDeAtencion,
                NivelDePrioridad = request.NivelDePrioridad,
                Color = request.Color,
                Activo = request.Activo
            };

            await this.ticketsDbContext.Prioridads.AddAsync(model);
            await this.ticketsDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<PrioridadListDto>> GetAllAsync()
        {
            List<Prioridad> prioridades = await ticketsDbContext.Prioridads.Include(x => x.Sucursal).ToListAsync();
            List<PrioridadListDto> prioridadListDtos = new List<PrioridadListDto>();
            foreach(var item in prioridades)
            {
                prioridadListDtos.Add(new PrioridadListDto() { 
                    Id =  item.Id,
                    SucursalId = item.SucursalId,
                    SucursalNombre = item.Sucursal.Nombre,
                    SucursalClave = item.Sucursal.Clave,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    TiempoDeAtencion = item.TiempoDeAtencion,
                    NivelDePrioridad = item.NivelDePrioridad,
                    Color = item.Color,
                    Activo = item.Activo
                });
            }

            return prioridadListDtos;
        }

        public async Task<PrioridadDto> UpdateAsync(PrioridadDto request, Guid id)
        {
            var existingItem = await ticketsDbContext.Prioridads.FindAsync(id);


            if (existingItem == null)
            {
                return null;
            }
            existingItem.SucursalId = request.SucursalId;
            existingItem.Nombre = request.Nombre;
            existingItem.Descripcion = request.Descripcion;
            existingItem.TiempoDeAtencion = request.TiempoDeAtencion;
            existingItem.NivelDePrioridad = request.NivelDePrioridad;
            existingItem.Color = request.Color;

            //ticketsDbContext.Entry(existingItem).CurrentValues.SetValues(sucursal);

            await ticketsDbContext.SaveChangesAsync();

            return request;
        }
    }
}
