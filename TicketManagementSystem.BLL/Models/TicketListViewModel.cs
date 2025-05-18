using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.BLL.Models
{
    public class TicketListViewModel
    {
        public List<TicketViewModel> Tickets { get; set; } = new List<TicketViewModel>();
        public List<IssueTypeViewModel> IssueTypes { get; set; } = new List<IssueTypeViewModel>();
        
        public int? FilterIssueTypeId { get; set; }
        public string FilterPriority { get; set; }

        [Display(Name = "Issue Type")]
        public string SelectedIssueTypeName { get; set; }
    }
} 