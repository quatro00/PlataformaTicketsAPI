using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
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

        public async Task<ResponseModel> DesasignarEquipo(AsignarEquipoDto request)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                //buscamos si el usuario ya esta asignado al mismo equipo
                await ticketsDbContext.RelCategoriaEquipos.Where(x=>x.EquipoId == request.EquipoId && x.CategoriaId == request.CategoriaId).ExecuteDeleteAsync();
                await ticketsDbContext.SaveChangesAsync();
                rm.result = request;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }

        public async Task<ResponseModel> AsignarEquipo(AsignarEquipoDto request)
        {
            ResponseModel rm = new ResponseModel();
            
            try
            {
                //buscamos si el usuario ya esta asignado al mismo equipo
                if (await ticketsDbContext.RelCategoriaEquipos.Where(x => x.CategoriaId == request.CategoriaId && x.EquipoId == request.EquipoId).CountAsync() > 0)
                {
                    rm.SetResponse(false, "El equipo ya pertenece a la categoria seleccionada.");
                    return rm;
                }

                var categoria = await ticketsDbContext.Categoria.FindAsync(request.CategoriaId);
                var equipo = await ticketsDbContext.Equipos.FindAsync(request.EquipoId);

                if (categoria.SucursalId != equipo.SucursalId)
                {
                    rm.SetResponse(false, "El categoria y el equipo no corresponden a la misma sucursal.");
                    return rm;
                }

                RelCategoriaEquipo relCategoriaEquipo = new RelCategoriaEquipo()
                {
                    CategoriaId = request.CategoriaId,
                    EquipoId = request.EquipoId,
                    Activo = true,
                };

                await ticketsDbContext.RelCategoriaEquipos.AddAsync(relCategoriaEquipo);
                await ticketsDbContext.SaveChangesAsync();
                rm.result = request;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            
            return rm;
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
            List<Categorium> categorias = await ticketsDbContext.Categoria
                .Include(x => x.Sucursal)
                .Include(x=>x.RelCategoriaEquipos).ThenInclude(x=>x.Equipo)
                .ToListAsync();
            List<CategoriaListDto> result = new List<CategoriaListDto>();
            foreach (var item in categorias)
            {
                List<CategoriaEqupoListDto> equipos = new List<CategoriaEqupoListDto>();
                foreach (var relEquipo in item.RelCategoriaEquipos)
                {
                    equipos.Add(new CategoriaEqupoListDto() { 
                    Id = relEquipo.Equipo.Id,
                    Nombre = relEquipo.Equipo.Nombre
                    });
                }
                result.Add(new CategoriaListDto()
                {
                    Id = item.Id,
                    SucursalId = item.SucursalId,
                    SucursalClave = item.Sucursal.Clave,
                    SucursalNombre = item.Sucursal.Nombre,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Equipos = equipos,
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
