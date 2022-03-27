
using System.ComponentModel.DataAnnotations;

namespace Booking.Models.Models
{
    public class AddBookingModel
    {
        [Display(Name = "Room type")]
        [Required(ErrorMessage = "Room type is required")]
        public int RoomTypeId { get; set; }

        [Display(Name = "From date")]
        [Required(ErrorMessage = "From date is required")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To date")]
        [Required(ErrorMessage = "To date is required")]
        public DateTime ToDate { get; set; }       

        public bool IsSuccess { get; set; }
                
        public string? Message { get; set; }

    }

    public class RoomTypeModel
    {
        public int RoomTypeId { get; set; }

        public string? RoomTypeName { get; set; }
    }
}
