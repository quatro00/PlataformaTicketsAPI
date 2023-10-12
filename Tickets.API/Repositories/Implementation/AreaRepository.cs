using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class AreaRepository : IAreaRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public AreaRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }
        public async Task<ResponseModel> CreateAreaBaseAsync(CreateAreaBaseRequestDto request)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                Area area = new Area()
                {
                    Id = Guid.NewGuid(),
                    AreaPadreId = request.AreaPadreId,
                    DepartamentoId = request.DepartamentoId,
                    Clave = request.Clave,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Activo = request.Activo
                };

                await this.ticketsDbContext.Areas.AddAsync(area);
                await this.ticketsDbContext.SaveChangesAsync();

                rm.result = request;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            
            return rm;
        }

        public async Task<ResponseModel> GetAreas(Guid departamentoId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<Area> areaList = await this.ticketsDbContext.Areas.Include(x => x.InverseAreaPadre).Where(x => x.DepartamentoId == departamentoId && x.AreaPadreId == null).ToListAsync();
                List<AreaTreeDto> areaDtos = new List<AreaTreeDto>();
                foreach (var item in areaList)
                {
                    AreaTreeDto area = new AreaTreeDto()
                    {
                        value = item.Id.ToString(),
                        label = item.Clave + "-" + item.Nombre,
                        isLeaf = true,
                    };
                    area.children = await GetAreasAnidadasTree(area);
                    if(area.children.Count > 0) { area.isLeaf = false; }
                    else { area.isLeaf = true; }
                    areaDtos.Add(area);
                }
                areaDtos.Add(new AreaTreeDto() { children = new List<AreaTreeDto>(), isLeaf = true, label = "Ninguno", value = departamentoId.ToString() });
                rm.result = areaDtos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        private async Task<List<AreaTreeDto>> GetAreasAnidadasTree(AreaTreeDto requestArea)
        {
            List<Area> areaList = await this.ticketsDbContext.Areas.Where(x => x.AreaPadreId == Guid.Parse(requestArea.value)).ToListAsync();
            List<AreaTreeDto> areaDtos = new List<AreaTreeDto>();

            foreach (var item in areaList)
            {
                AreaTreeDto areaAux = new AreaTreeDto()
                {
                    value = item.Id.ToString(),
                    label = item.Clave + "-" + item.Nombre,
                    isLeaf = true,
                };
                areaAux.children = await GetAreasAnidadasTree(areaAux);
                
                if (areaAux.children.Count > 0) { areaAux.isLeaf = false; }
                else { areaAux.isLeaf = true; }
                areaDtos.Add(areaAux);

            }

            areaDtos.Add(new AreaTreeDto() { children = new List<AreaTreeDto>(), isLeaf = true, label = "Ninguno", value = requestArea.value });
            return areaDtos;
        }



        public async Task<ResponseModel> GetAreasByDepartamento(GetAreasByDepartamentoRequestDto request)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<Area> areaList = await this.ticketsDbContext.Areas.Include(x=>x.InverseAreaPadre).Where(x => x.DepartamentoId == request.DepartamentoId && x.AreaPadreId == null).ToListAsync();
                List<AreaDto> areaDtos = new List<AreaDto>();
                foreach(var item in areaList)
                {
                    AreaDto area = new AreaDto()
                    {
                        Id = item.Id,
                        AreaPadreId = item.AreaPadreId,
                        DepartamentoId = item.DepartamentoId,
                        Clave = item.Clave,
                        Nombre = item.Nombre,
                        Descripcion = item.Descripcion,
                        Activo = item.Activo
                    };
                    area.AreasHijas = await GetAreasAnidadas(area);
                    areaDtos.Add(area);
                }
                
                rm.result = areaDtos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        private async Task<List<AreaDto>> GetAreasAnidadas(AreaDto area)
        {
            List<Area> areaList = await this.ticketsDbContext.Areas.Where(x => x.AreaPadreId == area.Id).ToListAsync();
            List<AreaDto> areaDtos = new List<AreaDto>();

            foreach (var item in areaList)
            {
                AreaDto areaAux = new AreaDto()
                {
                    Id = item.Id,
                    AreaPadreId = item.AreaPadreId,
                    DepartamentoId = item.DepartamentoId,
                    Clave = item.Clave,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Activo = item.Activo
                };
                areaAux.AreasHijas = await GetAreasAnidadas(areaAux);
                areaDtos.Add(areaAux);
            }

            return areaDtos;
        }

    }
}
