namespace Tickets.API.Models.DTO.Departamento
{
    public class GetDepartamentoResponseDto
    {
        public Guid id { get; set; }

        public Guid sucursalId { get; set; }
        public string sucursal { get; set; }
        public string sucursalClave { get; set; }

        public string clave { get; set; } = null!;

        public string descripcion { get; set; } = null!;

        public string telefono { get; set; } = null!;

        public bool activo { get; set; }
    }
}
