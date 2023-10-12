using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tickets.API.Models.DTO.Categoria;
using Tickets.API.Repositories.Implementation;

namespace Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            //var response = await categoriaRepository.CreateAsync(request);
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            return Ok(Guid.NewGuid());
        }
    }
}
