using Tickets.API.Models;
using Tickets.API.Models.DTO.Area;
using Tickets.API.Models.DTO.Categoria;

namespace Tickets.API.Repositories.Interface
{
    public interface IAreaRepository
    {
        Task<ResponseModel> CreateAreaBaseAsync(CreateAreaBaseRequestDto request);
        Task<ResponseModel> GetAreasByDepartamento(GetAreasByDepartamentoRequestDto request);
        
    }
}
