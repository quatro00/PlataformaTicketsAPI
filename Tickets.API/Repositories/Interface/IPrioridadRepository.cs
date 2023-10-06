using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Prioridad;
using Tickets.API.Models.DTO.Sucursal;

namespace Tickets.API.Repositories.Interface
{
    public interface IPrioridadRepository
    {
        Task<IEnumerable<PrioridadListDto>> GetAllAsync();
        Task<PrioridadDto> CreateAsync(PrioridadDto request);
        Task<PrioridadDto> UpdateAsync(PrioridadDto request, Guid id);
    }
}
