namespace EventManagementAPI.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required ICollection<Event> Events { get; set; }
    }


}
