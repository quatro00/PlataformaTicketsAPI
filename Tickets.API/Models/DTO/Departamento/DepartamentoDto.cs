namespace Tickets.API.Models.DTO.Departamento
{
    public class DepartamentoDto
    {
        public Guid Id { get; set; }

        public Guid SucursalId { get; set; }

        public string Clave { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public bool Activo { get; set; }

        //public virtual Sucursal Sucursal { get; set; } = null!;
    }
}
