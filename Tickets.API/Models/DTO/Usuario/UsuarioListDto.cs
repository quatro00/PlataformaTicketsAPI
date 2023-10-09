namespace Tickets.API.Models.DTO.Usuario
{
    public class UsuarioListDto
    {
        public Guid Id { get; set; }
        public string LoginId { get; set; }
        public int Matricula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public Guid SucursalId { get; set; }
        public string RolId { get; set; }
        public string SucursalName { get; set; }
        public string CorreoElectronico { get; set; }
        public string roles { get; set; }
        public bool Activo { get;set; }
        
    }
}
