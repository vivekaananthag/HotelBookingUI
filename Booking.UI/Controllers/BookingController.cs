using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Booking.Interface.Service;
using Booking.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingUI.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IBookingService _bookingService;
        public BookingController(SignInManager<IdentityUser> signInManager, IBookingService bookingService)
        {           
            _signInManager = signInManager;
            _bookingService = bookingService;
        }
        public IActionResult AddBooking()
        {
            var model = new AddBookingModel();
            model.RoomTypes = _bookingService.GetRoomTypes().Result.ToList();
            ViewBag.RoomTypes = (from rt in model.RoomTypes
                                 select new SelectListItem
                                 {
                                     Text = rt.RoomTypeName,
                                     Value = rt.RoomTypeId.ToString(),
                                     Selected = false
                                 }).ToList();             

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBooking(AddBookingModel model)
        {
            var userId = string.Empty;
            if(model != null && ModelState.IsValid)
            {
                var result = await _bookingService.AddBooking(model, userId);
            }
            return View(model);
        }

        public async Task<IActionResult> Bookings()
        {
            var userId = string.Empty;
            var bookings = await _bookingService.GetBookingsByUserId(userId);

            return View(bookings);
        }
    }
}
