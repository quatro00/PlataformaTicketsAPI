using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Sucursal;

namespace Tickets.API.Repositories.Interface
{
    public interface ISucursalRepository
    {
        Task<IEnumerable<Sucursal>>GetAllAsync();
        Task<Sucursal> CreateAsync(CreateSucursalRequestDto request);
        Task<Sucursal> UpdateAsync(UpdateSucursalRequestDto request, Guid id);
    }
}
