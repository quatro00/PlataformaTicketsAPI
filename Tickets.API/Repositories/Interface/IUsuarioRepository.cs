using Tickets.API.Models;
using Tickets.API.Models.DTO.Usuario;

namespace Tickets.API.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        Task<ResponseModel> GetAll();
        Task<ResponseModel> CreateAsync(RequestUsuarioDto request, string user);
        Task<ResponseModel> UpdateAsync(RequestUsuarioDto request, Guid usuarioId);
        Task<ResponseModel> GetAgentes(Guid sucursalId);
        Task<ResponseModel> GetSupervisores(Guid sucursalId);
    }
}
