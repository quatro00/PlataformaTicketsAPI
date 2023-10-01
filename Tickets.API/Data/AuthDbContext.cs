using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tickets.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "43868f73-fc0a-4fb6-9ded-5646ae843552";
            var clienteRoleId = "caf48194-53ee-4903-b9b9-8642a8505a71";
            var agenteRoleId = "228388ef-e006-4ae8-8705-95f6d598c26f";
            var supervisorRoleId = "61ca7686-7128-4bfe-82a3-a27f204f91d9";

            var adminUserId = "c6e69bd3-a1a9-46d7-b00f-05900d59a585";
            var clienteUserId = "20eeea3c-4b7c-419c-a457-569492ab7677";
            var agenteUserId = "2f45b379-9a6e-4291-a07f-81afeb02a05c";
            var supervisorUserId = "3eb1755e-5e01-4480-a20e-621b689fa1bc";

            //Crear roles de admin, cliente, agente
            var roles = new List<IdentityRole> { 
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name ="Administrador",
                    NormalizedName = "Administrador".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole()
                {
                    Id = clienteRoleId,
                    Name ="Cliente",
                    NormalizedName = "Cliente".ToUpper(),
                    ConcurrencyStamp = clienteRoleId
                },
                new IdentityRole()
                {
                    Id = agenteRoleId,
                    Name ="Agente",
                    NormalizedName = "Agente".ToUpper(),
                    ConcurrencyStamp = agenteRoleId
                },
                new IdentityRole()
                {
                    Id = supervisorRoleId,
                    Name ="Supervisor",
                    NormalizedName = "Supervisor".ToUpper(),
                    ConcurrencyStamp = supervisorRoleId
                },
            };

            //Crear las semillas de los roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Creamos el usuario Adinistrador
            var admin = new IdentityUser() { 
                Id = adminUserId,
                UserName = "admin@imss.gob.mx",
                Email = "admin@imss.gob.mx",
                NormalizedEmail = "admin@imss.gob.mx".ToUpper(),
                NormalizedUserName = "admin@imss.gob.mx".ToUpper(),
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "password");

            //Creamos el usuario Cliente
            var cliente = new IdentityUser()
            {
                Id = clienteUserId,
                UserName = "cliente@imss.gob.mx",
                Email = "cliente@imss.gob.mx",
                NormalizedEmail = "cliente@imss.gob.mx".ToUpper(),
                NormalizedUserName = "cliente@imss.gob.mx".ToUpper()
            };
            cliente.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(cliente, "password");

            //Creamos el usuario Agente
            var agente = new IdentityUser()
            {
                Id = agenteUserId,
                UserName = "agente@imss.gob.mx",
                Email = "agente@imss.gob.mx",
                NormalizedEmail = "agente@imss.gob.mx".ToUpper(),
                NormalizedUserName = "agente@imss.gob.mx".ToUpper()
            };
            agente.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(agente, "password");

            //Creamos el usuario Supervisor
            var supervisor = new IdentityUser()
            {
                Id = supervisorUserId,
                UserName = "supervisor@imss.gob.mx",
                Email = "supervisor@imss.gob.mx",
                NormalizedEmail = "supervisor@imss.gob.mx".ToUpper(),
                NormalizedUserName = "supervisor@imss.gob.mx".ToUpper()
            };
            supervisor.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(supervisor, "password");

            List<IdentityUser> users = new List<IdentityUser>() {admin, cliente, agente,supervisor};
           

            var Roles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                },
                new()
                {
                    RoleId = clienteRoleId,
                    UserId = clienteUserId
                },
                new()
                {
                    RoleId = agenteRoleId,
                    UserId = agenteUserId
                },
                new()
                {
                    RoleId = supervisorRoleId,
                    UserId = supervisorUserId
                }
            };

            builder.Entity<IdentityUser>().HasData(users);
            builder.Entity<IdentityUserRole<string>>().HasData(Roles);
        }
    }
}
