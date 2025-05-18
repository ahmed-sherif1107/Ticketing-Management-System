using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.BLL.Models;
using TicketManagementSystem.DAL.Models;
using TicketManagementSystem.DAL.Repositories;

namespace TicketManagementSystem.BLL.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        public async Task<int> CreateTicketAsync(TicketViewModel ticket)
        {
            try
            {
                // Input validation
                if (ticket == null)
                {
                    throw new ArgumentNullException(nameof(ticket));
                }

                // Map ViewModel to DTO
                var ticketDto = new TicketDTO
                {
                    FullName = ticket.FullName,
                    MobileNumber = ticket.MobileNumber,
                    Email = ticket.Email,
                    IssueTypeId = ticket.IssueTypeId,
                    Description = ticket.Description,
                    Priority = ticket.Priority,
                    Status = "Open"
                };

                // Call repository to create ticket
                return await _ticketRepository.CreateTicketAsync(ticketDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in TicketService.CreateTicketAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<TicketViewModel> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                var ticketDto = await _ticketRepository.GetTicketByIdAsync(ticketId);
                
                if (ticketDto == null)
                {
                    return null;
                }

                // Map DTO to ViewModel
                return MapToViewModel(ticketDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in TicketService.GetTicketByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TicketViewModel>> GetAllTicketsAsync(int? issueTypeId = null, string priority = null)
        {
            try
            {
                var ticketDtos = await _ticketRepository.GetAllTicketsAsync(issueTypeId, priority);
                
                // Map DTOs to ViewModels
                return ticketDtos.Select(MapToViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in TicketService.GetAllTicketsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateTicketAsync(TicketViewModel ticket)
        {
            try
            {
                // Input validation
                if (ticket == null)
                {
                    throw new ArgumentNullException(nameof(ticket));
                }

                // Map ViewModel to DTO
                var ticketDto = new TicketDTO
                {
                    TicketId = ticket.TicketId,
                    FullName = ticket.FullName,
                    MobileNumber = ticket.MobileNumber,
                    Email = ticket.Email,
                    IssueTypeId = ticket.IssueTypeId,
                    Description = ticket.Description,
                    Priority = ticket.Priority,
                    Status = ticket.Status ?? "Open" // Set default status if null
                };

                // Call repository to update ticket
                return await _ticketRepository.UpdateTicketAsync(ticketDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in TicketService.UpdateTicketAsync: {ex.Message}");
                throw;
            }
        }

        private TicketViewModel MapToViewModel(TicketDTO dto)
        {
            return new TicketViewModel
            {
                TicketId = dto.TicketId,
                FullName = dto.FullName,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                IssueTypeId = dto.IssueTypeId,
                IssueTypeName = dto.IssueTypeName,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = dto.Status,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }
    }
} 