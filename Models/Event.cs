using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    [Table("Event")]
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        public string? EventName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

    }
}
