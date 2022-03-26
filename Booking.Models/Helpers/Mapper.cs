
using Booking.Models.Models;
using DB = Booking.Models.Database;

namespace Booking.Models.Helpers
{
    public static class Constants
    {
        public const string BOOKING_SUCCESS = "Thanks for making the booking. Room number {0} has been successfully booked for the selected dates.";
        public const string NO_ROOMS_AVAILABLE = "No rooms available for the selected criteria. Please try a different date or a room type.";
    }
    public static class Mapper
    {
        public static DB.Booking  MapAddBookingModelToDB(AddBookingModel bookingModel)
        {
            var booking = new DB.Booking
            {
                BookingDate = DateTime.Now,
                FromDate = bookingModel.FromDate,
                ToDate = bookingModel.ToDate               

            };
            return booking;
        }

        public static IEnumerable<BookingModel> MapBookingDBToModel(List<DB.Booking> bookings)
        {
            if (bookings == null || bookings.Count > 0) return new List<BookingModel>();
            var bookingsModel = new List<BookingModel>();
            foreach (var booking in bookings)
            {
                BookingModel bookingModel = new BookingModel
                {
                    BookingId = booking.BookingId,
                    BookingDate = booking.BookingDate,
                    FromDate = booking.FromDate,
                    ToDate = booking.ToDate,
                    RoomNumber = booking.Room != null ? booking.Room.RoomNumber : String.Empty,
                    RoomType = booking.Room != null && booking.Room.RoomType != null
                                ? booking.Room.RoomType.RoomTypeName : String.Empty
                };
                bookingsModel.Add(bookingModel);
            }
            return bookingsModel;
        }

        public static IEnumerable<RoomTypeModel> MapRoomTypeDBToModel(List<DB.RoomType> roomTypes)
        {
            if (roomTypes == null || roomTypes.Count == 0) return new List<RoomTypeModel>();
            var roomTypesModel = new List<RoomTypeModel>();
            foreach(var roomType in roomTypes)
            {
                var roomTypeModel = new RoomTypeModel
                {
                    RoomTypeId = roomType.RoomTypeId,
                    RoomTypeName = roomType.RoomTypeName
                };
                roomTypesModel.Add(roomTypeModel);
            }
            return roomTypesModel;
        }
    }
}
