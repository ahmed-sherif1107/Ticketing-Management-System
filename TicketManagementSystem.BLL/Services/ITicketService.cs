using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.BLL.Models;

namespace TicketManagementSystem.BLL.Services
{
    public interface ITicketService
    {
        Task<int> CreateTicketAsync(TicketViewModel ticket);
        Task<TicketViewModel> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketViewModel>> GetAllTicketsAsync(int? issueTypeId = null, string priority = null);
        Task<int> UpdateTicketAsync(TicketViewModel ticket);
    }
} 