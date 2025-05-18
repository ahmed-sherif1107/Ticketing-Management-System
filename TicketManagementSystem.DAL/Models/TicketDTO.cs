using System;

namespace TicketManagementSystem.DAL.Models
{
    public class TicketDTO
    {
        public int TicketId { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int IssueTypeId { get; set; }
        public string IssueTypeName { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
} 