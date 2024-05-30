using AutoMapper;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;
using TicketSystem.Helpers.OneSignal;
using TicketSystem.Repository;
using TicketSystem.Utils;

namespace TicketSystem.Services;

public interface ITicketServices
{
    Task<(TicketDto? ticket, string? error)> CreateTicket(Guid userId, TicketForm ticketForm);
    Task<(List<TicketDto> tickets, int? totalCount, string? error)> GetAllTickets(Guid userId, TicketFilter filter);
    Task<(TicketDto? ticket, string? error)> GetTicketById(Guid id, Guid userId);
    Task<(TicketDto? ticket, string? error)> UpdateTicket(Guid userId, Guid id, TicketUpdate ticketUpdate);
    Task<(TicketDto? ticket, string? error)> DeleteTicket(Guid id);
    Task<(TicketDto? ticket, string? error)> AssignTicket(Guid ticketId, Guid assigneeId);
    Task<(TicketDto? ticket, string? error)> EscalatePriority(Guid ticketId);
    Task<(TicketDto? ticket, string? error)> UpdateTicketStatus(Guid userId, Guid ticketId, TicketStatus newStatus);
    Task<TicketStatisticsDto> GetTicketStatistics();

    Task<(TicketDto? ticket, string? error)> UpdateTicketPriority(Guid userId, Guid ticketId,
        TicketPriority newPriority);

    Task<(TicketDto? ticket, string? error)> ReopenTicket(Guid ticketId, Guid userId, string description);
}

public class TicketServices : ITicketServices
{
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;


    public TicketServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper, IWebHostEnvironment env)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _env = env;
    }

    public async Task<(TicketDto? ticket, string? error)> CreateTicket(Guid userId, TicketForm ticketForm)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");

        var ticket = _mapper.Map<Ticket>(ticketForm);
        ticket.UserId = userId;
        ticket.TicketNumber = RandomNumberGeneratorNumber.GenerateUnique6DigitNumber(_repositoryWrapper);
        ticket.Status = TicketStatus.Open;

        var solverData = await _repositoryWrapper.SolverData.GetAll<SolverData>();
        if (solverData.data == null || !solverData.data.Any()) return (null, "No solver found");

        var solver = solverData.data
            .OrderBy(s => s.TicketCount)
            .FirstOrDefault();

        ticket.AssigneeId = solver.UserId;

        var imagePaths = new List<string>();
        if (ticketForm.Images != null)
            foreach (var image in ticketForm.Images)
            {
                var path = await SaveImage(image);
                imagePaths.Add(path);
            }

        solver.TicketCount++;
        await _repositoryWrapper.SolverData.Update(solver);
        ticket.Images = imagePaths.ToArray();

        var response = await _repositoryWrapper.Ticket.Add(ticket, userId);
        var responseDto = _mapper.Map<TicketDto>(response);
        if (response == null) return (null, "Ticket couldn't be created");
        
        var notification = new Notifications
        {
            NotifyId = Guid.NewGuid(),
            Title = "تم تعيين شكوى جديدة",
            Description = $"تم تعيين شكوى جديدة  {response.Assignee!.FullName} لك.",      UserId = solver.UserId,
            Date = DateTime.Now,
            TicketId = response.Id

        };
        await _repositoryWrapper.Notification.Add(notification);
        OneSignal.SendNoitications(notification, solver.User.ToString());

        return (responseDto, null);
    }
    public async Task<(List<TicketDto> tickets, int? totalCount, string? error)> GetAllTickets(Guid userId,
        TicketFilter filter)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, null, "User not found");

        var tickets = await _repositoryWrapper.Ticket.GetAll<TicketDto>(
            x =>
                (filter.Title == null || x.Title.Contains(filter.Title)) &&
                (filter.Status == null || x.Status == filter.Status) &&
                (filter.Priority == null || x.Priority == filter.Priority) &&
                (filter.TicketNumber == null || x.TicketNumber == filter.TicketNumber) &&
                (filter.TicketId == null || x.Id == filter.TicketId) &&
                !x.Deleted &&
                (user.Role != UserRole.issuer ||user.Role != UserRole.Admin || x.UserId == userId),
            filter.PageNumber, filter.PageSize);

        var responseDto = _mapper.Map<List<TicketDto>>(tickets.data);

        var baseUrl = "https://ticketsystem-api.digital-logic.tech/";

        foreach (var ticketDto in responseDto)
            ticketDto.Images = ticketDto.Images.Select(image => baseUrl + image).ToList();

        return (responseDto, tickets.totalCount, null);
    }

    public async Task<(TicketDto? ticket, string? error)> GetTicketById(Guid id, Guid userId)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");

        var ticket = await _repositoryWrapper.Ticket.GetAll<TicketDto>(x => x.Id == id && !x.Deleted && (user.Role != UserRole.issuer || user.Role != UserRole.Admin || x.UserId == userId));
        var responseDto = _mapper.Map<TicketDto>(ticket.data.FirstOrDefault());
        return ticket.data.Count == 0 ? (null, "Ticket not found") : (responseDto, null);
    }

    public async Task<(TicketDto? ticket, string? error)> UpdateTicket(Guid userId, Guid id, TicketUpdate ticketUpdate)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == id && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        if (ticketUpdate.EscalatePriority) ticket.Priority = (TicketPriority)((int)ticket.Priority + 1);

        ticket.UpdatedAt = ticketUpdate.UpdatedAt;


        ticket = _mapper.Map(ticketUpdate, ticket);
        var response = await _repositoryWrapper.Ticket.Update(ticket, userId);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket couldn't be updated") : (responseDto, null);
    }

    public async Task<(TicketDto? ticket, string? error)> DeleteTicket(Guid id)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == id && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        ticket.Deleted = true;
        var response = await _repositoryWrapper.Ticket.Update(ticket);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket couldn't be deleted") : (responseDto, null);
    }


    public async Task<(TicketDto? ticket, string? error)> AssignTicket(Guid ticketId, Guid assigneeId)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == ticketId && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        ticket.AssigneeId = assigneeId;
        ticket.UpdatedAt = DateTime.Now;

        var response = await _repositoryWrapper.Ticket.Update(ticket);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket couldn't be assigned") : (responseDto, null);
    }


    public async Task<(TicketDto? ticket, string? error)> EscalatePriority(Guid ticketId)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == ticketId && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        ticket.Priority = (TicketPriority)((int)ticket.Priority + 1);

        if (ticket.Priority > TicketPriority.High) ticket.Priority = TicketPriority.High;
        var response = await _repositoryWrapper.Ticket.Update(ticket);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket priority couldn't be escalated") : (responseDto, null);
    }

    public async Task<(TicketDto? ticket, string? error)> UpdateTicketStatus(Guid userId, Guid ticketId,
        TicketStatus newStatus)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == ticketId && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        ticket.Status = newStatus;

        if (newStatus == TicketStatus.Closed) ticket.ClosedAt = DateTime.UtcNow;

        var response = await _repositoryWrapper.Ticket.Update(ticket, userId);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket status couldn't be updated") : (responseDto, null);
    }

    public async Task<TicketStatisticsDto> GetTicketStatistics()
    {
        var statistics = new TicketStatisticsDto();
        var tickets = await _repositoryWrapper.Ticket.GetAll(x => !x.Deleted && x.Status != null);

        var groupedTickets = tickets.data.GroupBy(x => x.Status).ToDictionary(g => g.Key, g => g.Count());

        statistics.OpenTickets = groupedTickets.ContainsKey(TicketStatus.Open) ? groupedTickets[TicketStatus.Open] : 0;
        statistics.PendingTickets =
            groupedTickets.ContainsKey(TicketStatus.Pending) ? groupedTickets[TicketStatus.Pending] : 0;
        statistics.SolvedTickets =
            groupedTickets.ContainsKey(TicketStatus.Solved) ? groupedTickets[TicketStatus.Solved] : 0;
        statistics.ClosedTickets =
            groupedTickets.ContainsKey(TicketStatus.Closed) ? groupedTickets[TicketStatus.Closed] : 0;
        statistics.TotalTickets = statistics.OpenTickets + statistics.PendingTickets + statistics.SolvedTickets +
                                  statistics.ClosedTickets;
        return statistics;
    }

    public async Task<(TicketDto? ticket, string? error)> UpdateTicketPriority(Guid userId, Guid ticketId,
        TicketPriority newPriority)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == ticketId && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        ticket.Priority = newPriority;
        var response = await _repositoryWrapper.Ticket.Update(ticket, userId);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket priority couldn't be updated") : (responseDto, null);
    }


    public async Task<(TicketDto? ticket, string? error)> ReopenTicket(Guid ticketId, Guid userId, string description)
    {
        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == ticketId && !x.Deleted);
        if (ticket == null) return (null, "Ticket not found");

        if (ticket.Status != TicketStatus.Closed && ticket.Status != TicketStatus.Solved) 
            return (null, "Ticket is not in a state that can be reopened");

        ticket.Status = TicketStatus.Open;
        ticket.Ddescription = description;
        ticket.UpdatedAt = DateTime.Now;

        var response = await _repositoryWrapper.Ticket.Update(ticket);
        var responseDto = _mapper.Map<TicketDto>(response);
        return response == null ? (null, "Ticket couldn't be reopened") : (responseDto, null);
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }


        return "images/" + fileName;
    }
}