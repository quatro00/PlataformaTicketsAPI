using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Models.DTO.SubCategoria;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class SubCategoriaRepository : ISubCategoriaRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public SubCategoriaRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }
        public async Task<SubCategoriaDto> CreateAsync(SubCategoriaDto request)
        {
            SubCategorium model = new SubCategorium()
            {
                Id = Guid.NewGuid(),
                CategoriaId = request.CategoriaId,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Activo = request.Activo
            };

            await this.ticketsDbContext.SubCategoria.AddAsync(model);
            await this.ticketsDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<SubCategoriaListDto>> GetAllAsync()
        {
            List<SubCategorium> categorias = await ticketsDbContext.SubCategoria.Include(x => x.Categoria.Sucursal).ToListAsync();
            List<SubCategoriaListDto> result = new List<SubCategoriaListDto>();
            foreach (var item in categorias)
            {
                result.Add(new SubCategoriaListDto()
                {
                    Id = item.Id,
                    SucursalId = item.Categoria.SucursalId,
                    SucursalNombre = item.Categoria.Sucursal.Nombre,
                    SucursalClave = item.Categoria.Sucursal.Clave,
                    CategoriaId = item.Categoria.Id,
                    CategoriaName = item.Categoria.Nombre,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Activo = item.Activo
                });
            }

            return result;
        }

        public async Task<SubCategoriaDto> UpdateAsync(SubCategoriaDto request, Guid id)
        {
            var existingItem = await ticketsDbContext.SubCategoria.FindAsync(id);

            if (existingItem == null)
            {
                return null;
            }
            existingItem.CategoriaId = request.CategoriaId;
            existingItem.Nombre = request.Nombre;
            existingItem.Descripcion = request.Descripcion;

            //ticketsDbContext.Entry(existingItem).CurrentValues.SetValues(sucursal);

            await ticketsDbContext.SaveChangesAsync();

            return request;
        }
    }
}
