﻿namespace Tickets.API.Models.DTO.Usuario
{
    public class RequestUsuarioDto
    {
        public int Matricula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public Guid SucursalId { get; set; }
        public string CorreoElectronico { get; set; }
        public string RolId { get; set; }
    }
}
