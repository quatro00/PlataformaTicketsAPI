using Tickets.API.Data;
using Tickets.API.Models;
using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.TicketComentario;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class TicketComentarioRepository : ITicketComentarioRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public TicketComentarioRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }
        public async Task<ResponseModel> CreateAsync(TicketComentarioRequestDto request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                TicketComentario ticketComentario = new TicketComentario()
                {
                    Id = Guid.NewGuid(),
                    TicketId = request.TicketId,
                    UsuarioId = usuarioId,
                    Texto = request.Mensaje,
                    Fecha = DateTime.Now
                };

                await this.ticketsDbContext.TicketComentarios.AddAsync(ticketComentario);
                await this.ticketsDbContext.SaveChangesAsync();

                rm.result = request;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

            return rm;
        }
    }
}
