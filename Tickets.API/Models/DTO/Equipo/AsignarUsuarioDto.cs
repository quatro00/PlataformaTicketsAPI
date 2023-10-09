namespace Tickets.API.Models.DTO.Equipo
{
    public class AsignarUsuarioDto
    {
        public Guid UsuarioId { get; set; }
        public Guid EquipoId { get; set; }
        public bool? EsSupervisor { get; set; }
    }
}
