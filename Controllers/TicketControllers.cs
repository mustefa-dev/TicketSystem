using TicketSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TicketSystem.Controllers;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;
using TicketSystem.Services;
using TicketSystem.Utils;

namespace TicketSystem.Controllers
{

    public class TicketsController : BaseController
    {
        private readonly ITicketServices _ticketServices;

        public TicketsController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }

        [Authorize(Roles = "SolvingCenter,issuer,Admin")]
        [HttpGet]
        public async Task<ActionResult<Respons<TicketDto>>> GetAll([FromQuery] TicketFilter filter) =>
            Ok(await _ticketServices.GetAllTickets(Id,filter), filter.PageNumber, filter.PageSize);
        [Authorize(Roles = "issuer")]

        [HttpPost]
        public async Task<ActionResult<TicketForm>> Create([FromForm] TicketForm ticketForm) =>
            Ok(await _ticketServices.CreateTicket(Id, ticketForm));

        // [HttpPut("{id}")]
        // public async Task<ActionResult<TicketUpdate>> Update([FromBody] TicketUpdate ticketUpdate, Guid id) =>
        //     Ok(await _ticketServices.UpdateTicket(id,Id ,ticketUpdate));
        [Authorize(Roles = "SolvingCenter")]

        [HttpDelete("{id}")]
        public async Task<ActionResult<TicketDto>> Delete(Guid id) => Ok(await _ticketServices.DeleteTicket(id));
        [Authorize (Roles = "issuer")]
        [HttpPut("{id}/assign/{assigneeId}")]
        public async Task<ActionResult<TicketDto>> Assign(Guid id, Guid assigneeId) => Ok(await _ticketServices.AssignTicket(id, assigneeId));

        // [HttpPut("{id}/escalate")]
        // public async Task<ActionResult<TicketDto>> Escalate(Guid id) => Ok(await _ticketServices.EscalatePriority(id));
        [Authorize(Roles = "SolvingCenter")]

        [HttpPut("{id}/status")]
        public async Task<ActionResult<TicketDto>> UpdateStatus(Guid id, TicketStatus newStatus) =>
            Ok(await _ticketServices.UpdateTicketStatus(Id,id, newStatus));

        [Authorize(Roles = "SolvingCenter,issuer,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetById(Guid id) => Ok(await _ticketServices.GetTicketById(id,Id));
        
        [HttpGet("statistics")]
        public async Task<ActionResult<TicketStatisticsDto>> GetStatistics() => Ok(await _ticketServices.GetTicketStatistics());
        [Authorize(Roles = "issuer")]
        [HttpPut("{id}/priority")]
        public async Task<ActionResult<TicketDto>> UpdatePriority(Guid id, TicketPriority newPriority) =>
            Ok(await _ticketServices.UpdateTicketPriority(Id, id, newPriority));
        [Authorize (Roles = "issuer")]
        
        [HttpPut("{id}/reopen")]
        public async Task<ActionResult<TicketDto>> ReopenTicket(Guid id,[FromBody] string description) => Ok(await _ticketServices.ReopenTicket(id, Id, description));
    }
}