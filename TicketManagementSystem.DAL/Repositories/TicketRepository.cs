using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TicketManagementSystem.DAL.Database;
using TicketManagementSystem.DAL.Models;

namespace TicketManagementSystem.DAL.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DBConnectionFactory _connectionFactory;

        public TicketRepository(DBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> CreateTicketAsync(TicketDTO ticket)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spCreateTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FullName", ticket.FullName);
                        command.Parameters.AddWithValue("@MobileNumber", ticket.MobileNumber);
                        command.Parameters.AddWithValue("@Email", ticket.Email);
                        command.Parameters.AddWithValue("@IssueTypeId", ticket.IssueTypeId);
                        command.Parameters.AddWithValue("@Description", ticket.Description);
                        command.Parameters.AddWithValue("@Priority", ticket.Priority);

                        var result = await command.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int ticketId))
                        {
                            return ticketId;
                        }
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in CreateTicketAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<TicketDTO> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spGetTicketById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TicketId", ticketId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapToTicketDTO(reader);
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetTicketByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int? issueTypeId = null, string priority = null)
        {
            try
            {
                var tickets = new List<TicketDTO>();

                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spGetAllTickets", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (issueTypeId.HasValue)
                        {
                            command.Parameters.AddWithValue("@IssueTypeId", issueTypeId.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IssueTypeId", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(priority))
                        {
                            command.Parameters.AddWithValue("@Priority", priority);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Priority", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                tickets.Add(MapToTicketDTO(reader));
                            }
                        }
                    }
                }

                return tickets;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllTicketsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateTicketAsync(TicketDTO ticket)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spUpdateTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                        command.Parameters.AddWithValue("@FullName", ticket.FullName);
                        command.Parameters.AddWithValue("@MobileNumber", ticket.MobileNumber);
                        command.Parameters.AddWithValue("@Email", ticket.Email);
                        command.Parameters.AddWithValue("@IssueTypeId", ticket.IssueTypeId);
                        command.Parameters.AddWithValue("@Description", ticket.Description);
                        command.Parameters.AddWithValue("@Priority", ticket.Priority);
                        command.Parameters.AddWithValue("@Status", ticket.Status);

                        var result = await command.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int ticketId))
                        {
                            return ticketId;
                        }
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in UpdateTicketAsync: {ex.Message}");
                throw;
            }
        }

        private TicketDTO MapToTicketDTO(SqlDataReader reader)
        {
            return new TicketDTO
            {
                TicketId = reader.GetInt32(reader.GetOrdinal("TicketId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                IssueTypeId = reader.GetInt32(reader.GetOrdinal("IssueTypeId")),
                IssueTypeName = reader.GetString(reader.GetOrdinal("IssueTypeName")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Priority = reader.GetString(reader.GetOrdinal("Priority")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) 
                    ? (DateTime?)null 
                    : reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
            };
        }
    }
} 