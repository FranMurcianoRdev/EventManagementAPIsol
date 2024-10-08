namespace EventManagementAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string Description { get; set; }
        public required ICollection<Participant> Participants { get; set; }
    }
}
