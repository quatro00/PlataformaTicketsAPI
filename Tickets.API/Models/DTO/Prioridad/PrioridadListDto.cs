namespace Tickets.API.Models.DTO.Prioridad
{
    public class PrioridadListDto
    {
        public Guid Id { get; set; }

        public Guid SucursalId { get; set; }

        public string SucursalNombre { get; set; } = null!;
        public string SucursalClave { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public decimal TiempoDeAtencion { get; set; }

        public decimal NivelDePrioridad { get; set; }

        public string Color { get; set; } = null!;

        public bool Activo { get; set; }
    }
}
