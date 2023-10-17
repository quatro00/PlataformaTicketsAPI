﻿using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Models.DTO.Ticket;
using Tickets.API.Repositories.Interface;
using Tickets.API.Helpers;
using static Azure.Core.HttpHeader;
using System.Runtime.CompilerServices;

namespace Tickets.API.Repositories.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        public DbSet<TicketDto> Customers { get; set; }
        private readonly TicketsDbContext ticketsDbContext;
        public TicketRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<ResponseModel> CrearTicket(CrearTicketRequestDto request, Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //Generamos el id del ticket
                var id = Guid.NewGuid();
                //copiamos los archivos a la ruta correcta
                var tempFolderName = Path.Combine("Temp");
                var folderName = Path.Combine("Resources", id.ToString());

                var tempPath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var folio = 1;
                try
                {
                    folio = await ticketsDbContext.Tickets.CountAsync() + 1;
                }
                catch(Exception ioe)
                {
                    folio = 1;
                }

                //buscamos las areas del ticket
                Guid departamentoId = request.AreaId.FirstOrDefault();
                Guid? areaId = null;

                string areaString = "";
                foreach(var item in request.AreaId)
                {
                    //buscamos el area
                    Area area = await ticketsDbContext.Areas.Where(x=>x.Id == item).FirstOrDefaultAsync();

                    if(area == null) { departamentoId = item; areaString = "Ninguno."; }
                    else { departamentoId =(Guid)area.DepartamentoId; areaString = areaString + "/" + area.Clave+"-"+ area.Nombre; }

                    
                }
                if(areaString.IndexOf("/") != -1)
                {
                    areaString = areaString.Substring(0, areaString.LastIndexOf("/"));
                }
                
                List<TicketArchivo> archivos = new List<TicketArchivo>();
                //validamos que tengamos algun arhivo
                if(request.Archivos.Count > 0)
                {
                    //creamos el folder del ticket
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    //copiamos cada uno de los archivos de la carpeta temporal a la nueva carpeta
                    foreach(var item in request.Archivos)
                    {
                        System.IO.File.Move(Path.Combine(tempFolderName, item.NombreFisico), Path.Combine(folderName, item.NombreFisico));

                        FileInfo fi = new FileInfo(Path.Combine(folderName, item.NombreFisico));
                        string extn = fi.Extension;
                        string fullFileName = item.Nombre;
                        long size = fi.Length;

                        archivos.Add(new TicketArchivo() { 
                            Id = Guid.NewGuid(),
                            TicketId = id,
                            UsuarioId = userId,
                            Nombre = item.Nombre,
                            NombreFisico = item.NombreFisico,
                            Tipo = fi.Extension,
                            Tamaño = fi.Length.ToString(),
                            Fecha = DateTime.Now,

                        });
                    }

                    
                }

                
                Ticket ticket = new Ticket()
                {
                 Id = id,
                 AreaId = areaId,
                 AreaString = areaString,
                 DepartamentoId = departamentoId,
                 Folio = folio,
                 EstatusId = 1,
                 PrioridadId = request.PrioridadId,
                 Titulo = request.Titulo,
                 SubCategoriaId = request.SubCategoriaId,
                 Descripcion = request.Descripcion,
                 FechaCreacion = DateTime.Now,
                 UsuarioCreacionId = userId,
                 TicketArchivos = archivos,
                };

                await ticketsDbContext.Tickets.AddAsync(ticket);
                await ticketsDbContext.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        public async Task<ResponseModel> GetUsuarioTickets(int? estatusId, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
            

        List<Ticket> tickets = await this.ticketsDbContext.Tickets
                    .Include(x=>x.Estatus)
                    .Include(x => x.Departamento)
                    .Include(x => x.Prioridad)
                    .Include(x=>x.UsuarioCreacion)
                    .Include(x=>x.SubCategoria)
                    .ThenInclude(x=>x.Categoria)
                    .ThenInclude(x=>x.Sucursal)
                    .Where(x=>(x.EstatusId == estatusId || estatusId == 0) && x.UsuarioCreacionId == usuarioId)
                    .ToListAsync();

                List<TicketDto> ticketsDto = new List<TicketDto>();
                foreach (Ticket ticket in tickets)
                {
                    ticketsDto.Add(new TicketDto()
                    {
                        Id = ticket.Id,
                        Folio = ticket.Folio,
                        Solicitante = ticket.UsuarioCreacion.Apellidos + " " + ticket.UsuarioCreacion.Nombre,
                        Subcategoria = ticket.SubCategoria.Nombre,
                        Categoria = ticket.SubCategoria.Categoria.Nombre,
                        Sucursal  = ticket.SubCategoria.Categoria.Sucursal.Nombre,
                        Departamento = ticket.Departamento.Clave + "-" + ticket.Departamento.Descripcion,
                        Area = ticket.AreaString,
                        Titulo = ticket.Titulo,
                        Prioridad = ticket.Prioridad.Nombre,
                        NivelDePrioridad = ticket.Prioridad.NivelDePrioridad,
                        Color = ticket.Prioridad.Color,
                        Estatus = ticket.Estatus.Descripcion,
                        EstatusColor = ticket.Estatus.Color,
                        FechaCreacion = ticket.FechaCreacion.ToString("dd/MM/yyyy")
                    });
                }
                rm.result = ticketsDto;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        public async Task<ResponseModel> GetSupervisorTickets(int? estatusId, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

               //this.ticketsDbContext.RelCategoriaEquipos.IntersectBy(x=>x.Equipo.RelUsuarioEquipos.Where(y=>y.UsuarioId == usuarioId), u=>u.EquipoId)
               // this.ticketsDbContext.SubCategoria.Where(x=>x.Categoria.RelCategoriaEquipos.IntersectBy())
                List<Ticket> tickets = await this.ticketsDbContext.Tickets
                            .Include(x => x.Estatus)
                            .Include(x => x.Departamento)
                            .Include(x => x.Prioridad)
                            .Include(x => x.UsuarioCreacion)
                            .Include(x => x.SubCategoria)
                            .Include(x => x.SubCategoria.Categoria.Sucursal)
                            .ThenInclude(x => x.Categoria)
                            .ThenInclude(x => x.RelCategoriaEquipos)
                            .ThenInclude(x=>x.Equipo)
                            
                            .Where(x=> (x.EstatusId == estatusId || estatusId == 0) && x.SubCategoria.Categoria.RelCategoriaEquipos.Any(x=>x.Equipo.RelUsuarioEquipos.Any(y=>y.UsuarioId == usuarioId && y.EsSupervisor == true && y.Activo == true))
                            
                            
                            
                            )
                            .ToListAsync();

                List<TicketDto> ticketsDto = new List<TicketDto>();
                foreach (Ticket ticket in tickets)
                {
                    ticketsDto.Add(new TicketDto()
                    {
                        Id = ticket.Id,
                        Folio = ticket.Folio,
                        Solicitante = ticket.UsuarioCreacion.Apellidos + " " + ticket.UsuarioCreacion.Nombre,
                        Subcategoria = ticket.SubCategoria.Nombre,
                        Categoria = ticket.SubCategoria.Categoria.Nombre,
                        Sucursal = ticket.SubCategoria.Categoria.Sucursal.Nombre,
                        Departamento = ticket.Departamento.Clave + "-" + ticket.Departamento.Descripcion,
                        Area = ticket.AreaString,
                        Titulo = ticket.Titulo,
                        Prioridad = ticket.Prioridad.Nombre,
                        NivelDePrioridad = ticket.Prioridad.NivelDePrioridad,
                        Color = ticket.Prioridad.Color,
                        Estatus = ticket.Estatus.Descripcion,
                        EstatusColor = ticket.Estatus.Color,
                        FechaCreacion = ticket.FechaCreacion.ToString("dd/MM/yyyy")
                    });
                }
                rm.result = ticketsDto;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
    }
}
