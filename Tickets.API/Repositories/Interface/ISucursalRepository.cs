using Tickets.API.Models.Domain;

namespace Tickets.API.Repositories.Interface
{
    public interface ISucursalRepository
    {
        Task<IEnumerable<Sucursal>>GetAllAsync();
    }
}
