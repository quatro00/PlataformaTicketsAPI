using Tickets.API.Models.DTO.Area;
using Tickets.API.Models;
using Tickets.API.Models.DTO.Ticket;

namespace Tickets.API.Repositories.Interface
{
    public interface ITicketRepository
    {
        Task<ResponseModel> CrearTicket(CrearTicketRequestDto request, Guid userId);
        Task<ResponseModel> GetUsuarioTickets(int? estatusId, Guid usuarioId);
        Task<ResponseModel> GetSupervisorTickets(int? estatusId, Guid usuarioId);
        Task<ResponseModel> GetSupervisorTicketDetalle(Guid ticketId, Guid usuarioId);
    }
}
