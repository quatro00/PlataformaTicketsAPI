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
        Task<ResponseModel> GetEquiposSucursal(GetEquipoSucursalDto model);
        //Obtiene los agente asociados a un supervisor y que pueden atender el tickcet seleccionado
        Task<ResponseModel> GetAgentesBySupervisor(Guid supervisorId, Guid ticketId);
    }
}
