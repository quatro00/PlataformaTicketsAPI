namespace Tickets.API.Models.DTO.Prioridad
{
    public class PrioridadDto
    {

        public Guid SucursalId { get; set; }//

        public string Nombre { get; set; }//

        public string Descripcion { get; set; }//

        public decimal TiempoDeAtencion { get; set; }//

        public decimal NivelDePrioridad { get; set; }//

        public string Color { get; set; }//

        public bool Activo { get; set; }//
    }
}
