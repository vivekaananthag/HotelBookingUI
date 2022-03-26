using DB = Booking.Models.Database;
using Booking.Models.Database;
using Booking.Models.Models;

namespace Booking.Interface.DAL
{
    public interface IBookingDAL
    {
        Task<IEnumerable<DB.Booking>> GetBookingsByUserId(string userId);
        Room GetAvailableRoomForBooking(AddBookingModel bookingModel);
        Task<DB.Booking> AddBooking(AddBookingModel bookingModel, int roomId, string userId);
        Task<IEnumerable<RoomType>> GetRoomTypes();
    }
}
