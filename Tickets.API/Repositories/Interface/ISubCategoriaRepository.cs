using Tickets.API.Models.DTO.SubCategoria;

namespace Tickets.API.Repositories.Interface
{
    public interface ISubCategoriaRepository
    {
        Task<IEnumerable<SubCategoriaListDto>> GetAllAsync();
        Task<SubCategoriaDto> CreateAsync(SubCategoriaDto request);
        Task<SubCategoriaDto> UpdateAsync(SubCategoriaDto request, Guid id);
    }
}
