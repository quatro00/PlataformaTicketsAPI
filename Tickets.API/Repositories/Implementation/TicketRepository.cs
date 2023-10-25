using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Models.DTO.Ticket;
using Tickets.API.Repositories.Interface;
using Tickets.API.Helpers;
using static Azure.Core.HttpHeader;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using Azure.Core;

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
                            .Where(x=> (x.EstatusId != 4 || estatusId == 4) && (x.EstatusId == estatusId || estatusId == 0) && x.SubCategoria.Categoria.RelCategoriaEquipos.Any(x=>x.Equipo.RelUsuarioEquipos.Any(y=>y.UsuarioId == usuarioId && y.EsSupervisor == true && y.Activo == true)))
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
        public async Task<ResponseModel> GetSupervisorTicketDetalle(Guid ticketId, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

                //this.ticketsDbContext.RelCategoriaEquipos.IntersectBy(x=>x.Equipo.RelUsuarioEquipos.Where(y=>y.UsuarioId == usuarioId), u=>u.EquipoId)
                // this.ticketsDbContext.SubCategoria.Where(x=>x.Categoria.RelCategoriaEquipos.IntersectBy())
                var ticket = await this.ticketsDbContext.Tickets
                            .Include(x => x.Estatus)
                            .Include(x => x.Departamento)
                            .Include(x => x.Prioridad)
                            .Include(x => x.UsuarioCreacion)
                            .Include(x => x.SubCategoria)
                            .Include(x => x.TicketMaterials)
                            .Include(x => x.SubCategoria.Categoria.Sucursal)
                            .ThenInclude(x => x.Categoria)
                            .ThenInclude(x => x.RelCategoriaEquipos)
                            .ThenInclude(x => x.Equipo)
                            .Include(x => x.TicketComentarios)
                            .Include(x=>x.TicketArchivos)
                            .Include(x=>x.TicketUsuariosAsignados).ThenInclude(x=>x.Usuario)
                            .Where(x => (x.Id == ticketId) && x.SubCategoria.Categoria.RelCategoriaEquipos.Any(x => x.Equipo.RelUsuarioEquipos.Any(y => y.UsuarioId == usuarioId && y.EsSupervisor == true && y.Activo == true)))
                            .FirstOrDefaultAsync();

                if(ticket == null)
                {
                    return rm;
                }
                TicketDetalleDto ticketsDto = new TicketDetalleDto()
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
                    Descripcion = ticket.Descripcion,
                    FechaCreacion = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                    Archivos = ticket.TicketArchivos.Select(x => new TicketDetalleArchivoDto() {
                        Id = x.Id,
                        Tipo = x.Tipo.Replace(".", "").ToLower(),
                        Fecha = x.Fecha,
                        Nombre = x.Nombre,
                        NombreFisico = x.NombreFisico,
                        Tamano = x.Tamaño,
                        UsuarioId = x.UsuarioId
                    }).ToList(),
                    Comentarios = ticket.TicketComentarios.Select(x => new TicketDetalleComentarioDto() {
                        Id = x.Id,
                        Fecha = x.Fecha,
                        Texto = x.Texto,
                        UsuarioId = x.UsuarioId,
                        Nombre = ""
                    }).ToList(),
                    Materiales = ticket.TicketMaterials.Select(x=> new TicketMaterialDto()
                    {
                        Id = x.Id,
                        Concepto = x.Concepto,  
                        Tipo=x.Tipo,
                        Unidad=x.Unidad,
                        Cantidad = x.Cantidad ?? 0,
                        Precio = x.Precio ?? 0
                    }).ToList(),
                    Asignados = ticket.TicketUsuariosAsignados.Select(x=> new TicketUsuarioAsignadoDto() { Id = x.UsuarioId, Nombre = x.Usuario.Apellidos.Trim() + " " + x.Usuario.Nombre}).ToList()
                };
               
                foreach(var item in ticketsDto.Comentarios)
                {
                    Usuario usuario = await this.ticketsDbContext.Usuarios.Where(x=>x.Id == item.UsuarioId).FirstOrDefaultAsync();
                    item.Nombre = usuario.Apellidos.Trim() + " " + usuario.Nombre.Trim();
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
        public async Task<ResponseModel> GetTicketDetalle(Guid ticketId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

                //this.ticketsDbContext.RelCategoriaEquipos.IntersectBy(x=>x.Equipo.RelUsuarioEquipos.Where(y=>y.UsuarioId == usuarioId), u=>u.EquipoId)
                // this.ticketsDbContext.SubCategoria.Where(x=>x.Categoria.RelCategoriaEquipos.IntersectBy())
                var ticket = await this.ticketsDbContext.Tickets
                            .Include(x => x.Estatus)
                            .Include(x => x.Departamento)
                            .Include(x => x.Prioridad)
                            .Include(x => x.UsuarioCreacion)
                            .Include(x => x.SubCategoria)
                            .Include(x => x.TicketMaterials)
                            .Include(x => x.SubCategoria.Categoria.Sucursal)
                            .ThenInclude(x => x.Categoria)
                            .ThenInclude(x => x.RelCategoriaEquipos)
                            .ThenInclude(x => x.Equipo)
                            .Include(x => x.TicketComentarios)
                            .Include(x => x.TicketArchivos)
                            .Include(x => x.TicketUsuariosAsignados).ThenInclude(x => x.Usuario)
                            .Where(x => (x.Id == ticketId))
                            .FirstOrDefaultAsync();

                if (ticket == null)
                {
                    return rm;
                }
                TicketDetalleDto ticketsDto = new TicketDetalleDto()
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
                    Descripcion = ticket.Descripcion,
                    FechaCreacion = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                    Archivos = ticket.TicketArchivos.Select(x => new TicketDetalleArchivoDto()
                    {
                        Id = x.Id,
                        Tipo = x.Tipo.Replace(".", "").ToLower(),
                        Fecha = x.Fecha,
                        Nombre = x.Nombre,
                        NombreFisico = x.NombreFisico,
                        Tamano = x.Tamaño,
                        UsuarioId = x.UsuarioId
                    }).ToList(),
                    Comentarios = ticket.TicketComentarios.Select(x => new TicketDetalleComentarioDto()
                    {
                        Id = x.Id,
                        Fecha = x.Fecha,
                        Texto = x.Texto,
                        UsuarioId = x.UsuarioId,
                        Nombre = ""
                    }).ToList(),
                    Materiales = ticket.TicketMaterials.Select(x => new TicketMaterialDto()
                    {
                        Id = x.Id,
                        Concepto = x.Concepto,
                        Tipo = x.Tipo,
                        Unidad = x.Unidad,
                        Cantidad = x.Cantidad ?? 0,
                        Precio = x.Precio ?? 0
                    }).ToList(),
                    Asignados = ticket.TicketUsuariosAsignados.Select(x => new TicketUsuarioAsignadoDto() { Id = x.UsuarioId, Nombre = x.Usuario.Apellidos.Trim() + " " + x.Usuario.Nombre }).ToList()
                };

                foreach (var item in ticketsDto.Comentarios)
                {
                    Usuario usuario = await this.ticketsDbContext.Usuarios.Where(x => x.Id == item.UsuarioId).FirstOrDefaultAsync();
                    item.Nombre = usuario.Apellidos.Trim() + " " + usuario.Nombre.Trim();
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
        public async Task<ResponseModel> AsignarTicketAgente(Guid ticketId, List<Guid> agentes, Guid usuarioId, string observaciones)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
               await this.ticketsDbContext.TicketUsuariosAsignados.Where(x => x.TicketId == ticketId).ExecuteDeleteAsync();
                await this.ticketsDbContext.SaveChangesAsync();
                //colocamos los asignados
                foreach (var item in agentes)
                {
                    await this.ticketsDbContext.TicketUsuariosAsignados.AddAsync(new TicketUsuariosAsignado()
                    {
                        TicketId = ticketId,
                        UsuarioId = item,
                        FechaCreacion =DateTime.Now,
                        UsuarioCreacion = usuarioId,
                        Activo = true
                    });
                }

                //generamos el comentario
                TicketComentario ticketComentario = new TicketComentario()
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticketId,
                    UsuarioId = usuarioId,
                    Texto = "[Estatus: En atención]:"+observaciones,
                    Fecha = DateTime.Now
                };

                await this.ticketsDbContext.Tickets.Where(x=>x.Id == ticketId).ExecuteUpdateAsync(x => x.SetProperty(y => y.EstatusId, 2));
                await this.ticketsDbContext.TicketComentarios.AddAsync(ticketComentario);
                await this.ticketsDbContext.SaveChangesAsync();
                
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        public async Task<ResponseModel> CrearTicketMaterial(CapturaMaterialesRequestDto request, Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //Generamos el id del ticket
                var id = Guid.NewGuid();
                TicketMaterial ticketMaterial = new TicketMaterial()
                {
                    Id = id,
                    TicketId = request.TicketId,
                    Concepto = request.Concepto,
                    Tipo = request.Tipo,
                    Unidad = request.Unidad,
                    Cantidad = request.Cantidad,
                    Precio = request.Precio,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = userId
                };

                await ticketsDbContext.TicketMaterials.AddAsync(ticketMaterial);
                await ticketsDbContext.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> BorrarTicketMaterial(BorrarTicketMaterialDto request, Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que el ticket este en atencion
                var ticket = await this.ticketsDbContext.Tickets.FindAsync(request.TicketId);
                if (!(ticket.EstatusId == 2 || ticket.EstatusId == 3))
                {
                    rm.SetResponse(false, "el ticket debe estar en estatus: En atención o Pendiente");
                    return rm;
                }
                this.ticketsDbContext.TicketMaterials.Where(x => x.Id == request.MaterialId && x.TicketId == request.TicketId).ExecuteDeleteAsync();
                await ticketsDbContext.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> Cerrar(Guid ticketId, string observaciones, Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que el ticket este en atencion
                var ticket = await this.ticketsDbContext.Tickets.FindAsync(ticketId);
                if (!(ticket.EstatusId == 2))
                {
                    rm.SetResponse(false, "el ticket debe estar en estatus: En atención");
                    return rm;
                }

                //colocamos estatus de cerrado
                TicketComentario ticketComentario = new TicketComentario()
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticketId,
                    UsuarioId = userId,
                    Texto = "[Estatus: Cerrado]:" + observaciones,
                    Fecha = DateTime.Now
                };

                await this.ticketsDbContext.Tickets.Where(x => x.Id == ticketId).ExecuteUpdateAsync(x => x.SetProperty(y => y.EstatusId, 4));
                await this.ticketsDbContext.TicketComentarios.AddAsync(ticketComentario);
                await this.ticketsDbContext.SaveChangesAsync();

                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> EnEspera(Guid ticketId, string observaciones, Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que el ticket este en atencion
                var ticket = await this.ticketsDbContext.Tickets.FindAsync(ticketId);
                if (!(ticket.EstatusId == 2))
                {
                    rm.SetResponse(false, "el ticket debe estar en estatus: En atención");
                    return rm;
                }

                //colocamos estatus de cerrado
                TicketComentario ticketComentario = new TicketComentario()
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticketId,
                    UsuarioId = userId,
                    Texto = "[Estatus: Pendiente]:" + observaciones,
                    Fecha = DateTime.Now
                };

                await this.ticketsDbContext.Tickets.Where(x => x.Id == ticketId).ExecuteUpdateAsync(x => x.SetProperty(y => y.EstatusId, 3));
                await this.ticketsDbContext.TicketComentarios.AddAsync(ticketComentario);
                await this.ticketsDbContext.SaveChangesAsync();

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
