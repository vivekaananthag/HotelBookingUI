using System;
using System.Collections.Generic;

namespace Booking.Models.Database
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime BookingDate { get; set; }

        public virtual Room Room { get; set; } = null!;
    }
}
