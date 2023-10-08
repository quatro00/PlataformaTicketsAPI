﻿using Tickets.API.Models.DTO.Categoria;

namespace Tickets.API.Repositories.Interface
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<CategoriaListDto>> GetAllAsync();
        Task<CategoriaDto> CreateAsync(CategoriaDto request);
        Task<CategoriaDto> UpdateAsync(CategoriaDto request, Guid id);
    }
}