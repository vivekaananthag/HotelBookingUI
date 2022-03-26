using Booking.Interface.DAL;
using Booking.Interface.Service;
using Booking.Models.Helpers;
using Booking.Models.Models;

namespace Booking.Service
{
    public class BookingService : IBookingService
    {
        private IBookingDAL _bookingDAL;

        public BookingService(IBookingDAL bookingDAL)
        {
            _bookingDAL = bookingDAL;
        }

        /// <summary>
        /// Service method to get a list of bookings made by the logged in user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingModel>> GetBookingsByUserId(string userId)
        {
            try
            {
                var bookings = await _bookingDAL.GetBookingsByUserId(userId);
                var bookingsModel = Mapper.MapBookingDBToModel(bookings.ToList());

                return bookingsModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add a booking for the user
        /// </summary>
        /// <param name="bookingModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> AddBooking(AddBookingModel bookingModel, string userId)
        {
            var result = string.Empty;
            var availableRoom = _bookingDAL.GetAvailableRoomForBooking(bookingModel);
            if(availableRoom != null && availableRoom.RoomId > 0)
            {
                var booking = await _bookingDAL.AddBooking(bookingModel, availableRoom.RoomId, userId);
                if (booking != null && booking.BookingId > 0)
                    result = string.Format(Constants.BOOKING_SUCCESS, availableRoom.RoomNumber);
                else
                    result = "ERROR";
            }
            else
            {
                result = Constants.NO_ROOMS_AVAILABLE;
            }
            return result;
        }

        public async Task<IEnumerable<RoomTypeModel>> GetRoomTypes()
        {
            var roomTypes = await _bookingDAL.GetRoomTypes();
            return Mapper.MapRoomTypeDBToModel(roomTypes.ToList());
        }
    }
}
