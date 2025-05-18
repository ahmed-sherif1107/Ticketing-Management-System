using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.BLL.Models;
using TicketManagementSystem.BLL.Services;

namespace TicketManagementSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IIssueTypeService _issueTypeService;

        public TicketsController(
            ITicketService ticketService,
            IIssueTypeService issueTypeService)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _issueTypeService = issueTypeService ?? throw new ArgumentNullException(nameof(issueTypeService));
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? issueTypeId = null, string priority = null)
        {
            try
            {
                var model = new TicketListViewModel
                {
                    Tickets = (await _ticketService.GetAllTicketsAsync(issueTypeId, priority)).ToList(),
                    IssueTypes = (await _issueTypeService.GetAllIssueTypesAsync()).ToList(),
                    FilterIssueTypeId = issueTypeId,
                    FilterPriority = priority
                };

                // Set the selected issue type name for display purposes
                if (issueTypeId.HasValue)
                {
                    var issueType = model.IssueTypes.FirstOrDefault(i => i.IssueTypeId == issueTypeId.Value);
                    if (issueType != null)
                    {
                        model.SelectedIssueTypeName = issueType.TypeName;
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", ex);
            }
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", ex);
            }
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", ex);
            }
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _ticketService.CreateTicketAsync(ticket);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View(ticket);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", "An error occurred while creating the ticket.");
                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View(ticket);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", ex);
            }
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketViewModel ticket)
        {
            try
            {
                if (id != ticket.TicketId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _ticketService.UpdateTicketAsync(ticket);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View(ticket);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", "An error occurred while updating the ticket.");
                ViewBag.IssueTypes = await _issueTypeService.GetAllIssueTypesAsync();
                return View(ticket);
            }
        }
    }
} 