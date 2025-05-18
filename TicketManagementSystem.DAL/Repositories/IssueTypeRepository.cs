using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TicketManagementSystem.DAL.Database;
using TicketManagementSystem.DAL.Models;

namespace TicketManagementSystem.DAL.Repositories
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        private readonly DBConnectionFactory _connectionFactory;

        public IssueTypeRepository(DBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<IssueTypeDTO>> GetAllIssueTypesAsync()
        {
            try
            {
                var issueTypes = new List<IssueTypeDTO>();

                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spGetAllIssueTypes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                issueTypes.Add(new IssueTypeDTO
                                {
                                    IssueTypeId = reader.GetInt32(reader.GetOrdinal("IssueTypeId")),
                                    TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Description")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                });
                            }
                        }
                    }
                }

                return issueTypes;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllIssueTypesAsync: {ex.Message}");
                throw;
            }
        }
    }
} 