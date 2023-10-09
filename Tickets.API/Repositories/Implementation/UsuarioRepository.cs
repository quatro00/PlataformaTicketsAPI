using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Usuario;
using Tickets.API.Repositories.Interface;
using Tickets.API.Helpers;

namespace Tickets.API.Repositories.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly TicketsDbContext ticketsDbContext;

        public UsuarioRepository(UserManager<IdentityUser> userManager, TicketsDbContext ticketsDbContext)
        {
            this.userManager = userManager;
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<ResponseModel> CreateAsync(RequestUsuarioDto request, string user)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que no exista la matricula ni el correo
                if(await ticketsDbContext.Usuarios.Where(x=>
                x.Matricula == request.Matricula || 
                x.CorreoElectronico == request.CorreoElectronico).CountAsync() > 0)
                {
                    //si existe la matricula o el correo electronico regresamos falso
                    rm.SetResponse(false, "La matricula o el correo ya estan siendo usados por otro usuario.");
                    return rm;
                }
                //generamos el usuario
                IdentityUser applicationUser = new IdentityUser();
                Guid guid = Guid.NewGuid();
                applicationUser.Id = guid.ToString();
                applicationUser.UserName = request.Matricula.ToString();
                applicationUser.Email = request.CorreoElectronico;
                applicationUser.NormalizedEmail = request.CorreoElectronico.ToUpper();
                applicationUser.NormalizedUserName = request.Matricula.ToString();
                applicationUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(applicationUser, request.Matricula.ToString());

                Usuario usuario = new Usuario()
                {
                    Id = guid,
                    LoginId = guid.ToString(),
                    Matricula = request.Matricula,
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    SucursalId = request.SucursalId,
                    CorreoElectronico = request.CorreoElectronico,
                    Activo = true,
                    UsuarioCreacionId = Guid.Parse(user),
                    FechaCreacion = DateTime.Now
                };
                
                await userManager.CreateAsync(applicationUser);
                await userManager.AddToRoleAsync(applicationUser, request.RolId);
                await ticketsDbContext.Usuarios.AddAsync(usuario);
                await ticketsDbContext.SaveChangesAsync();
                
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
                List<Usuario> usuarios = await ticketsDbContext.Usuarios
                    .Include(x => x.Sucursal)
                    .Include(x=>x.RelUsuarioEquipos)
                        .ThenInclude(x=>x.Equipo)
                    .ToListAsync();

                List<UsuarioListDto> result = new List<UsuarioListDto>();

                foreach(var item in usuarios)
                {
                    AspNetUser user = await ticketsDbContext.AspNetUsers.Include(x=>x.Roles).Where(x=>x.Id == item.LoginId).FirstOrDefaultAsync();

                    string roles = "";
                    string rol = "";

                    foreach(var itemAux in user.Roles)
                    {
                        roles = roles + itemAux.Name.ToString().Trim() + " ,";
                        rol = itemAux.NormalizedName;
                    }
                    roles = roles.Substring(0,roles.Trim().Length - 1).Trim();
                    result.Add(new UsuarioListDto()
                    {
                        Id = item.Id,
                        LoginId = item.LoginId,
                        Matricula = item.Matricula,
                        Nombre = item.Nombre,
                        Apellidos = item.Apellidos,
                        SucursalId = item.SucursalId,
                        SucursalName = item.Sucursal.Nombre,
                        CorreoElectronico = item.CorreoElectronico,
                        Activo = item.Activo,
                        roles = roles,
                        RolId = rol
                    }); ;
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
        public async Task<ResponseModel> GetAgentes(Guid sucursalId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var usuariosAgente = await userManager.GetUsersInRoleAsync("Agente");
                
                var usuarios = await ticketsDbContext.Usuarios.Where(x=>x.SucursalId == sucursalId).ToListAsync();

                var result = usuarios.Join(usuariosAgente,
                               c => c.LoginId,
                                m => m.Id,
                               (c, m) => new
                               {
                                   id = c.Id,
                                   matricula = c.Matricula,
                                   nombre = c.Nombre + " " + c.Apellidos,
                               }).ToList();
                    
               
                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }
        public async Task<ResponseModel> GetSupervisores(Guid sucursalId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var usuariosAgente = await userManager.GetUsersInRoleAsync("Supervisor");

                var usuarios = await ticketsDbContext.Usuarios.Where(x => x.SucursalId == sucursalId).ToListAsync();

                var result = usuarios.Join(usuariosAgente,
                               c => c.LoginId,
                                m => m.Id,
                               (c, m) => new
                               {
                                   id = c.Id,
                                   matricula = c.Matricula,
                                   nombre = c.Nombre + " " + c.Apellidos,
                               }).ToList();


                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }
        public async Task<ResponseModel> UpdateAsync(RequestUsuarioDto request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                if (await ticketsDbContext.Usuarios.Where(x =>
                (x.Matricula == request.Matricula || x.CorreoElectronico == request.CorreoElectronico) && x.Id != usuarioId).CountAsync() > 0)
                {
                    //si existe la matricula o el correo electronico regresamos falso
                    rm.SetResponse(false, "La matricula o el correo ya estan siendo usados por otro usuario.");
                    return rm;
                }

                //buscamos los datos del usuario
                Usuario usuario = await ticketsDbContext.Usuarios.FindAsync(usuarioId);
                var aspnetuser = await userManager.FindByIdAsync(usuario.LoginId);
                aspnetuser.UserName = request.Matricula.ToString();
                aspnetuser.NormalizedUserName = request.Matricula.ToString().ToUpper();
                aspnetuser.Email = request.CorreoElectronico.ToString();
                aspnetuser.NormalizedEmail = request.CorreoElectronico.ToString().ToUpper();

                //quitamos todos los roles
                await userManager.RemoveFromRoleAsync(aspnetuser, "ADMINISTRADOR");
                await userManager.RemoveFromRoleAsync(aspnetuser, "SUPERVISOR");
                await userManager.RemoveFromRoleAsync(aspnetuser, "AGENTE");
                await userManager.RemoveFromRoleAsync(aspnetuser, "CLIENTE");

                await userManager.AddToRoleAsync(aspnetuser, request.RolId);

                usuario.Matricula = request.Matricula;
                usuario.Nombre = request.Nombre;
                usuario.Apellidos = request.Apellidos;
                usuario.SucursalId = request.SucursalId;
                usuario.CorreoElectronico = request.CorreoElectronico;
                await ticketsDbContext.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }

        public async Task<ResponseModel> GetPerfil(Guid userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                PerfilDto result = new PerfilDto();
                var usuario = await ticketsDbContext.Usuarios.Include(x=>x.Sucursal).Where(x=>x.Id == userId).FirstOrDefaultAsync();
                var identityUser = await userManager.FindByIdAsync(usuario.LoginId);

                result.Matricula = usuario.Matricula;
                result.SucursalNombre = usuario.Sucursal.Clave + "-" + usuario.Sucursal.Nombre;
                result.Nombre = usuario.Nombre;
                result.Apellidos = usuario.Apellidos;
                result.CorreoElectronico = usuario.CorreoElectronico;
                result.Telefono = identityUser.PhoneNumber;

                rm.result = result;
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
