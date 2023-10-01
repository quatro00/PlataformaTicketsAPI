namespace Tickets.API.Models.DTO.Sucursal
{
    public class CreateSucursalRequestDto
    {
        public string Clave { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string? Telefono2 { get; set; }

        public bool Activo { get; set; }
    }
}
