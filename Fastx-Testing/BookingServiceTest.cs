using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace FastxWebApi.Tests.Services
{
    public class BookingServiceTests
    {

        private IBookingService _bookingService;
        private Mock<IBookingRepository> _bookingRepoMock;
        private Mock<IRepository<int, BookingSeat>> _bookingSeatRepoMock;
        private Mock<IRefundRepository> _refundRepoMock;
        private Mock<IRouteRepository> _routeRepoMock;
        private Mock<ISeatRepository> _seatRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<BookingService>> _loggerMock;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _bookingRepoMock = new Mock<IBookingRepository>();
            _bookingSeatRepoMock = new Mock<IRepository<int, BookingSeat>>();

            _refundRepoMock = new Mock<IRefundRepository>();

            _routeRepoMock = new Mock<IRouteRepository>();
            _seatRepoMock = new Mock<ISeatRepository>();

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BookingService>>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _context = new ApplicationDbContext(options);

            _bookingService = new BookingService(
                _bookingRepoMock.Object,_bookingSeatRepoMock.Object,_refundRepoMock.Object, _routeRepoMock.Object,_seatRepoMock.Object,
                _mapperMock.Object,_context,_loggerMock.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task BookTicket_SuccessfulBooking_Returns_SuccessMessage()
        {
            
            var request = new BookingRequestDTO
            {
                UserId = 1, RouteId = 100,SeatNumbers = new List<string> { "A1", "A2" }
            };
            var seats = new List<Seat>
            {
                new Seat { SeatId = 1, SeatNumber = "A1", RouteId = 100, Status = "Available" },

                new Seat { SeatId = 2, SeatNumber = "A2", RouteId = 100, Status = "Available" }
            };
            var route = new Route { RouteId = 100, Fare = 200m };
            var booking = new Booking { BookingId = 1 };

            _routeRepoMock.Setup(r => r.GetById(100)).ReturnsAsync(route);

           
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
            _bookingRepoMock.Setup(b => b.Add(It.IsAny<Booking>())).ReturnsAsync(booking);
            _bookingSeatRepoMock.Setup(bs => bs.Add(It.IsAny<BookingSeat>())).ReturnsAsync(new BookingSeat());

            _mapperMock.Setup(m => m.Map<Booking>(request)).Returns(booking);

            
            var result = await _bookingService.BookTicket(request);

            
            Assert.That(result, Does.Contain("Ticket booked successfully"));
            _bookingRepoMock.Verify(b => b.Add(It.IsAny<Booking>()), Times.Once);
            _bookingSeatRepoMock.Verify(b => b.Add(It.IsAny<BookingSeat>()), Times.Exactly(2));
        }

        [Test]
        public void BookTicket_InvalidRoute_Throws_NoSuchEntityException()
        {
          
            var request = new BookingRequestDTO { RouteId = 999, SeatNumbers = new List<string> { "A1" } };
            _routeRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Route)null);

            
            Assert.ThrowsAsync<NoSuchEntityException>(() => _bookingService.BookTicket(request));
        }

        [Test]
        public async Task CancelBooking_BookingFound_ReturnsSuccess()
        {
            
            var booking = new Booking { BookingId = 1, Status = "Booked", TotalFare = 500m, IsDeleted = false };
            _bookingRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(booking);

            _bookingRepoMock.Setup(r => r.SoftDelete(1)).ReturnsAsync(true);
            _refundRepoMock.Setup(r => r.Add(It.IsAny<Refund>())).ReturnsAsync(new Refund { RefundId = 100 });

         
            var result = await _bookingService.CancelBooking(1);

            
            Assert.That(result, Does.Contain("successfully canceled"));
            _bookingRepoMock.Verify(r => r.SoftDelete(1), Times.Once);
        }

        [Test]
        public void CancelBooking_BookingNotFound_Throws_No_SuchEntityException()
        {
            
            _bookingRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Booking)null);

            
            Assert.ThrowsAsync<NoSuchEntityException>(() => _bookingService.CancelBooking(1));
        }

        [Test]
        public async Task GetAllBookings_ReturnsMapped_Results()
        {
            
            var bookings = new List<Booking> { new Booking { BookingId = 1 } };

            var pagedResponse = new PagenationResponseDTO<Booking> { Items = bookings, TotalCount = 1 };
            var bookingDTOs = new List<BookingResponseDTO> { new BookingResponseDTO { BookingId = 1 } };

            var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

            _bookingRepoMock.Setup(r => r.GetAll(pagination)).ReturnsAsync(pagedResponse);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookingResponseDTO>>(bookings)).Returns(bookingDTOs);

            
            var result = await _bookingService.GetAllBookings(pagination);

            
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());

            Assert.AreEqual(1, result.TotalCount);
        }


    }
}
