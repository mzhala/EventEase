using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    [Table("Event")]
    public class Event
    {
        public int EventId { get; set; }

        public string? EventName { get; set; }

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public int VenueId { get; set; }

        public Venue? Venue { get; set; }
    }
}
