using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EventEase.Models
{
    [Table("Venue")]
    public class Venue
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image upload is required")]
        public IFormFile? ImageFile { get; set; }
    }
}
