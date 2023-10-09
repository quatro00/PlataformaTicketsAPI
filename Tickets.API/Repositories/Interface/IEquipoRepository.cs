using Tickets.API.Models;
using Tickets.API.Models.DTO.Equipo;

namespace Tickets.API.Repositories.Interface
{
    public interface IEquipoRepository
    {
        Task<ResponseModel> GetAll();
        Task<ResponseModel> CreateAsync(EquipoDto request, string user);
        Task<ResponseModel> UpdateAsync(EquipoDto request, Guid usuarioId);
        Task<ResponseModel> AsignarUsuario(AsignarUsuarioDto request);
        Task<ResponseModel> DesasignarUsuario(AsignarUsuarioDto request);
    }
}
