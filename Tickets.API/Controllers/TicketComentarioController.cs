using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Helpers;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Models.DTO.TicketComentario;
using Tickets.API.Repositories.Implementation;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketComentarioController : ControllerBase
    {
        private readonly ITicketComentarioRepository ticketComentarioRepository;

        public TicketComentarioController(ITicketComentarioRepository ticketComentarioRepository)
        {
            this.ticketComentarioRepository = ticketComentarioRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CrearComentario(TicketComentarioRequestDto request)
        {
            var response = await ticketComentarioRepository.CreateAsync(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
