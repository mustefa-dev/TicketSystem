namespace TicketSystem.DATA.DTOs;

public class TicketStatisticsDto
{
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public int PendingTickets { get; set; }
    public int SolvedTickets { get; set; }
    public int ClosedTickets { get; set; }
   
}