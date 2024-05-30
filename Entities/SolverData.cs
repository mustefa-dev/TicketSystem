namespace TicketSystem.Entities
{
    public class SolverData : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public int TicketCount { get; set; }
        public AppUser? User { get; set; }
    }
}
