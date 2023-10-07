using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Models.DTO.Prioridad;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public CategoriaRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }
        public async Task<CategoriaDto> CreateAsync(CategoriaDto request)
        {
            Categorium model = new Categorium()
            {
                Id = Guid.NewGuid(),
                SucursalId = request.SucursalId,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Activo = request.Activo
            };

            await this.ticketsDbContext.Categoria.AddAsync(model);
            await this.ticketsDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<CategoriaListDto>> GetAllAsync()
        {
            List<Categorium> categorias = await ticketsDbContext.Categoria.Include(x => x.Sucursal).ToListAsync();
            List<CategoriaListDto> result = new List<CategoriaListDto>();
            foreach (var item in categorias)
            {
                result.Add(new CategoriaListDto()
                {
                    Id = item.Id,
                    SucursalId = item.SucursalId,
                    SucursalClave = item.Sucursal.Clave,
                    SucursalNombre = item.Sucursal.Nombre,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Activo = item.Activo
                });
            }

            return result;
        }

        public async Task<CategoriaDto> UpdateAsync(CategoriaDto request, Guid id)
        {
            var existingItem = await ticketsDbContext.Categoria.FindAsync(id);

            if (existingItem == null)
            {
                return null;
            }
            existingItem.SucursalId = request.SucursalId;
            existingItem.Nombre = request.Nombre;
            existingItem.Descripcion = request.Descripcion;

            //ticketsDbContext.Entry(existingItem).CurrentValues.SetValues(sucursal);

            await ticketsDbContext.SaveChangesAsync();

            return request;
        }
    }
}
