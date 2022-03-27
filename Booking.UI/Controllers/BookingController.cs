using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Booking.Interface.Service;
using Booking.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Booking.Models.Helpers;

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
            var roomTypes = _bookingService.GetRoomTypes().Result.ToList();
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now.AddDays(1);
            
            ViewBag.RoomTypes = (from rt in roomTypes
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
            var result = new AddBookingModel();
            try
            {
                var userIdentity = _signInManager.Context.User.Identities.First();

                var userId = userIdentity.Claims.First(x =>
                                x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
                
                //Date Validation
                if (model.FromDate == DateTime.MinValue || model.FromDate < DateTime.Today)
                    ModelState.AddModelError("FromDate", "Please select a valid From date");
                if (model.ToDate == DateTime.MinValue || model.ToDate < model.FromDate)
                    ModelState.AddModelError("ToDate", "Please select a valid To date");

                //If no validation errors call the service to add booking
                if (model != null && ModelState.IsValid && userIdentity.IsAuthenticated)
                {
                    result = await _bookingService.AddBooking(model, userId);
                    if (result != null && !result.IsSuccess && !string.IsNullOrEmpty(result.Message))
                        ModelState.AddModelError("Model", result.Message);
                }                               
            }
            catch
            {
                result = model;
                ModelState.AddModelError("Model", Constants.UNEXPECTED_ERROR);
            }
            //Set the model with static values before sending to the view
            var roomTypes = _bookingService.GetRoomTypes().Result.ToList();
            ViewBag.RoomTypes = (from rt in roomTypes
                                 select new SelectListItem
                                 {
                                     Text = rt.RoomTypeName,
                                     Value = rt.RoomTypeId.ToString(),
                                     Selected = false
                                 }).ToList();            
            return View(result);
        }

        public async Task<IActionResult> MyBookings()
        {
            var bookings = new List<BookingModel>();
            var userIdentity = _signInManager.Context.User.Identities.First();

            var userId = userIdentity.Claims.First(x =>
                            x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;

            if (userIdentity.IsAuthenticated)
            {
                var result = await _bookingService.GetBookingsByUserId(userId);
                return View(result);
            }

            return View(bookings);
        }
    }
}
