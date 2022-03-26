using DB = Booking.Models.Database;
using Booking.Models.Database;
using Booking.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Booking.Models.Helpers;
using Booking.Interface.DAL;

namespace Booking.DAL
{
    public class BookingDAL : IBookingDAL
    {
        private readonly HotelContext appDbContext;

        public BookingDAL(HotelContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Return a list of bookings specific to the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DB.Booking>> GetBookingsByUserId(string userId)
        {
            try
            {
                return await appDbContext.Bookings.Where(x=>x.UserId == userId)
                                    .Include(x => x.Room).ThenInclude(x => x.RoomType).ToListAsync();                
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve list of available rooms based on input criteria. Return one Room details for booking
        /// </summary>
        /// <param name="bookingModel"></param>
        /// <returns></returns>
        public Room GetAvailableRoomForBooking(AddBookingModel bookingModel)
        {
            try
            {
                var fromDateSqlParam = new SqlParameter("FromDate", bookingModel.FromDate);
                var toDateSqlParam = new SqlParameter("ToDate", bookingModel.ToDate);
                var roomTypeIdSqlParam = new SqlParameter("RoomTypeId", bookingModel.RoomTypeId);

                var availableRooms = appDbContext.Rooms
                                        .FromSqlRaw("EXECUTE dbo.GetAvailableRooms @FromDate, @ToDate, @RoomTypeId",
                                        fromDateSqlParam, toDateSqlParam, roomTypeIdSqlParam).ToList();

                if (availableRooms != null && availableRooms.Count > 0)
                {
                    return availableRooms.First();
                }
                return new Room();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DAL method to add a new booking for the user
        /// </summary>
        /// <param name="bookingModel"></param>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DB.Booking> AddBooking(AddBookingModel bookingModel, int roomId, string userId)
        {
            try
            {
                if (bookingModel == null) return null;
                                
                var booking = Mapper.MapAddBookingModelToDB(bookingModel);
                booking.UserId = userId;
                
                var room = await appDbContext.Rooms
                    .FirstOrDefaultAsync(e => e.RoomId == roomId);
                booking.Room = room ?? new Room();

                var result = await appDbContext.AddAsync(booking);
                appDbContext.Entry(booking.Room).State = EntityState.Detached;
                await appDbContext.SaveChangesAsync();

                return result.Entity != null && result.Entity.BookingId > 0 
                        ? result.Entity : new DB.Booking();
                
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypes()
        {
            return await appDbContext.RoomTypes.ToListAsync();
        }
    }
}
