using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.BLL.Models;

namespace TicketManagementSystem.BLL.Services
{
    public interface IIssueTypeService
    {
        Task<IEnumerable<IssueTypeViewModel>> GetAllIssueTypesAsync();
    }
} 