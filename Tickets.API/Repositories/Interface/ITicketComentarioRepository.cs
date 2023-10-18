using Tickets.API.Models.DTO.Area;
using Tickets.API.Models;
using Tickets.API.Models.DTO.TicketComentario;

namespace Tickets.API.Repositories.Interface
{
    public interface ITicketComentarioRepository
    {
        Task<ResponseModel> CreateAsync(TicketComentarioRequestDto request, Guid usuarioId);
    }
}
