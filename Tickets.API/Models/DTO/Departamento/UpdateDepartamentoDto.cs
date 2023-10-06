namespace Tickets.API.Models.DTO.Departamento
{
    public class UpdateDepartamentoDto
    {
        public Guid SucursalId { get; set; }

        public string Clave { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public bool Activo { get; set; }
    }
}
