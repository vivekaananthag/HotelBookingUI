using Booking.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Interface.Service
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingModel>> GetBookingsByUserId(string userId);
        Task<string> AddBooking(AddBookingModel bookingModel, string userId);
        Task<IEnumerable<RoomTypeModel>> GetRoomTypes();
    }
}
