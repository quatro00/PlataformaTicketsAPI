using Microsoft.EntityFrameworkCore;
using Tickets.API.Data;
using Tickets.API.Models.Domain;
using Tickets.API.Repositories.Interface;

namespace Tickets.API.Repositories.Implementation
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly TicketsDbContext ticketsDbContext;
        public SucursalRepository(TicketsDbContext ticketsDbContext)
        {
            this.ticketsDbContext = ticketsDbContext;
        }

        public async Task<IEnumerable<Sucursal>> GetAllAsync()
        {
            return await ticketsDbContext.Sucursals.ToListAsync();
        }
    }
}
