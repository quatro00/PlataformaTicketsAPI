using Tickets.API.Models.Domain;
using Tickets.API.Models.DTO.Departamento;
using Tickets.API.Models.DTO.Sucursal;

namespace Tickets.API.Repositories.Interface
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetAllAsync();
        Task<Departamento> CreateAsync(CreateDepartamentoDto request);
        Task<Departamento> UpdateAsync(UpdateDepartamentoDto request, Guid id);
    }
}
