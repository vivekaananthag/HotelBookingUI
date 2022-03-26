using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Models.Models
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
