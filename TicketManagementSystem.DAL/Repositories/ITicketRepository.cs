using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.DAL.Models;

namespace TicketManagementSystem.DAL.Repositories
{
    public interface ITicketRepository
    {
        Task<int> CreateTicketAsync(TicketDTO ticket);
        Task<TicketDTO> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int? issueTypeId = null, string priority = null);
        Task<int> UpdateTicketAsync(TicketDTO ticket);
    }
} 