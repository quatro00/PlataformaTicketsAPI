namespace Tickets.API.Models.DTO.Sucursal
{
    public class UpdateSucursalRequestDto
    {
        public string clave { get; set; } = null!;

        public string nombre { get; set; } = null!;

        public string direccion { get; set; } = null!;

        public string telefono { get; set; } = null!;

        public string? telefono2 { get; set; }

        public bool activo { get; set; }
    }
}
