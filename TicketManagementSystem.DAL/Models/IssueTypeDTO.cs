namespace TicketManagementSystem.DAL.Models
{
    public class IssueTypeDTO
    {
        public int IssueTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
} 