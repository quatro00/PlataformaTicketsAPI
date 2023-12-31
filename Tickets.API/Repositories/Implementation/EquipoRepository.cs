﻿using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Equipo;
using Tickets.API.Models.DTO.Ticket;
using Tickets.API.Models.DTO.Usuario;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class EquipoRepository : IEquipoRepository
    {
        private readonly TicketsDbContext ticketsDbContext;

        public EquipoRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<ResponseModel> AsignarUsuario(AsignarUsuarioDto request)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //buscamos si el usuario ya esta asignado al mismo equipo
                if(await ticketsDbContext.RelUsuarioEquipos.Where(x=>x.UsuarioId == request.UsuarioId && x.EquipoId == request.EquipoId).CountAsync() > 0)
                {
                    rm.SetResponse(false, "El usuario ya pertenece al equipo seleccionado.");
                    return rm;
                }

                var usuario = await ticketsDbContext.Usuarios.FindAsync(request.UsuarioId);
                var equipo = await ticketsDbContext.Equipos.FindAsync(request.EquipoId);

                if(usuario.SucursalId != equipo.SucursalId)
                {
                    rm.SetResponse(false, "El usuario y el equipo no corresponden a la misma sucursal.");
                    return rm;
                }

                RelUsuarioEquipo relUsuarioEquipo = new RelUsuarioEquipo()
                {
                    UsuarioId = request.UsuarioId,
                    EquipoId = request.EquipoId,
                    EsSupervisor = request.EsSupervisor ?? false,
                    Activo = true,
                };

                await ticketsDbContext.RelUsuarioEquipos.AddAsync(relUsuarioEquipo);
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

        public async Task<ResponseModel> CreateAsync(EquipoDto request, string user)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                Equipo equipo = new Equipo()
                {
                    Id= Guid.NewGuid(),
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    SucursalId = request.SucursalId,
                    Activo= true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = Guid.Parse(user)
                };

                await ticketsDbContext.Equipos.AddAsync(equipo);
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

        public async Task<ResponseModel> DesasignarUsuario(AsignarUsuarioDto request)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                await ticketsDbContext.RelUsuarioEquipos.Where(x => x.UsuarioId == request.UsuarioId && x.EquipoId == request.EquipoId).ExecuteDeleteAsync();
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

        public async Task<ResponseModel> GetAgentesBySupervisor(Guid supervisorId, Guid ticketId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<AgenteDto> agentes = new List<AgenteDto>();

                //obtenemos los equipos que es responsable el supervisor
                var equiposAsignados = await this.ticketsDbContext.RelUsuarioEquipos.Where(x => x.UsuarioId == supervisorId && x.EsSupervisor && x.Activo).ToListAsync();
                
                
                //buscamos la categoria del ticket
                var categorias = await this.ticketsDbContext.Tickets
                    .Include(x => x.SubCategoria)
                    .ThenInclude(x => x.Categoria)
                    .ThenInclude(x => x.RelCategoriaEquipos)
                    .Where(x => x.Id == ticketId)
                    .FirstOrDefaultAsync();

                var equipos = categorias.SubCategoria.Categoria.RelCategoriaEquipos.ToList();

                foreach(var item in equipos)
                {
                    //si el equipo es uno asignado agregamos todos los usuarios al final
                    if(equiposAsignados.Where(x=>x.EquipoId == item.EquipoId).Count() > 0)
                    {
                        var usuarios = await this.ticketsDbContext.Usuarios.Include(x=>x.RelUsuarioEquipos).Where(x=>x.RelUsuarioEquipos.Any(x=>x.EquipoId == item.EquipoId)).ToListAsync();
                        agentes.AddRange(usuarios.Select(x => new AgenteDto()
                        {
                            Id = x.Id,
                            Nombre = x.Apellidos.Trim() + " " + x.Nombre
                        }));
                    }
                }
                

                rm.result = agentes;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }

        public async Task<ResponseModel> GetAll()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<EquipoListDto> result = new List<EquipoListDto>();
                List<Equipo> equipos = await ticketsDbContext.Equipos
                    .Include(x=>x.Sucursal)
                    .Include(x=>x.RelCategoriaEquipos)
                        .ThenInclude(x=>x.Categoria)
                    .Include(x => x.RelUsuarioEquipos)
                        .ThenInclude(x=>x.Usuario)
                    .ToListAsync();

               

                foreach (var item in equipos)
                {
                    List<EquipoListDetDto> usuarios = new List<EquipoListDetDto>();
                    List<EquipoListDetDto> supervisores = new List<EquipoListDetDto>();
                    foreach (var usuario in item.RelUsuarioEquipos)
                    {
                        if (usuario.EsSupervisor)
                        {
                            supervisores.Add(new
                            EquipoListDetDto()
                            {
                                Nombre = $"{usuario.Usuario.Nombre} {usuario.Usuario.Apellidos}",
                                Id = usuario.UsuarioId
                            });
                        }
                        else
                        {
                            usuarios.Add(new
                            EquipoListDetDto()
                            {
                                Nombre = $"{usuario.Usuario.Nombre} {usuario.Usuario.Apellidos}",
                                Id = usuario.UsuarioId
                            });
                        }
                    }
                    result.Add(new EquipoListDto()
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Descripcion = item.Descripcion ?? "",
                        SucursalId = item.Sucursal.Id,
                        SucursalNombre = item.Sucursal.Nombre,
                        Categorias = item.RelCategoriaEquipos.Select(x => x.Categoria.Nombre).ToList(),
                        Usuarios = usuarios,
                        Supervisores = supervisores,
                        Activo = item.Activo
                    });
                }

                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }

        public async Task<ResponseModel> GetEquiposSucursal(GetEquipoSucursalDto model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<EquipoListDto> result = new List<EquipoListDto>();
                List<Equipo> equipos = await ticketsDbContext.Equipos
                    .Where(x=>x.SucursalId == model.sucursalId)
                    .ToListAsync();
               
                foreach(var item in equipos)
                rm.result = equipos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }

        public Task<ResponseModel> UpdateAsync(EquipoDto request, Guid usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
