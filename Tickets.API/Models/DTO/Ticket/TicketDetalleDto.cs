﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tickets.API.Models.DTO.Ticket
{
    public class TicketDetalleDto
    {
        public Guid Id { get; set; }
        public int Folio { get; set; }
        public string Solicitante { get; set; }
        public string Subcategoria { get; set; }
        public string Categoria { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string Area { get; set; }
        public string Titulo { get; set; }
        public string Prioridad { get; set; }
        public decimal NivelDePrioridad { get; set; }
        public string Color { get; set; }
        public string Estatus { get; set; }
        public string EstatusColor { get; set; }
        public string FechaCreacion { get; set; }
        public string Descripcion { get; set; }
        public List<TicketDetalleArchivoDto> Archivos { get; set; }
        public List<TicketDetalleComentarioDto> Comentarios { get; set; }

    }

    public class TicketDetalleArchivoDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string NombreFisico { get; set; }
        public string Tipo { get; set; }
        public string Tamano { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class TicketDetalleComentarioDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
    }
}
