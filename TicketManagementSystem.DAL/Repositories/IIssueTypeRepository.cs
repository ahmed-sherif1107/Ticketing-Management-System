using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.DAL.Models;

namespace TicketManagementSystem.DAL.Repositories
{
    public interface IIssueTypeRepository
    {
        Task<IEnumerable<IssueTypeDTO>> GetAllIssueTypesAsync();
    }
} 