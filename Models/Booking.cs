using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        public Event? Event { get; set; }

        public Venue? Venue { get; set; }
    }
}
