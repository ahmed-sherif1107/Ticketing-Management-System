using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.BLL.Models;
using TicketManagementSystem.DAL.Models;
using TicketManagementSystem.DAL.Repositories;

namespace TicketManagementSystem.BLL.Services
{
    public class IssueTypeService : IIssueTypeService
    {
        private readonly IIssueTypeRepository _issueTypeRepository;

        public IssueTypeService(IIssueTypeRepository issueTypeRepository)
        {
            _issueTypeRepository = issueTypeRepository ?? throw new ArgumentNullException(nameof(issueTypeRepository));
        }

        public async Task<IEnumerable<IssueTypeViewModel>> GetAllIssueTypesAsync()
        {
            try
            {
                var issueTypeDtos = await _issueTypeRepository.GetAllIssueTypesAsync();
                
                // Map DTOs to ViewModels
                return issueTypeDtos.Select(dto => new IssueTypeViewModel
                {
                    IssueTypeId = dto.IssueTypeId,
                    TypeName = dto.TypeName,
                    Description = dto.Description
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in IssueTypeService.GetAllIssueTypesAsync: {ex.Message}");
                throw;
            }
        }
    }
} 