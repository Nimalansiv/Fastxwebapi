using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class PaymentServiceTests
{
    private Mock<IPaymentRepository> _mockPaymentRepository;
    private Mock<IBookingRepository> _mockBookingRepository;
    private Mock<IMapper> _mockMapper;

    private Mock<ApplicationDbContext> _mockContext;
    private Mock<ILogger<PaymentService>> _mockLogger;
    private IPaymentService _paymentService;

    [SetUp]
    public void Setup()
    {
        _mockPaymentRepository = new Mock<IPaymentRepository>();

        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PaymentService>>();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;
        _mockContext = new Mock<ApplicationDbContext>(options);

        _paymentService = new PaymentService(
            _mockPaymentRepository.Object,_mockBookingRepository.Object,_mockMapper.Object,_mockContext.Object,_mockLogger.Object
        );
    }

   
    private Booking CreateBooking(int bookingId, string status) => new Booking { BookingId = bookingId, Status = status };
    private Payment CreatePayment(int paymentId, int bookingId, decimal amount) => new Payment { PaymentId = paymentId, BookingId = bookingId, Amount = amount };
    private PaymentDTO CreatePaymentDTO(int bookingId, decimal amount) => new PaymentDTO { BookingId = bookingId, Amount = amount };
    private PaymentDisplayDTO CreatePaymentDisplayDTO(int paymentId, int bookingId, decimal amount) => new PaymentDisplayDTO { PaymentId = paymentId, BookingId = bookingId, Amount = amount };

    [Test]
    public async Task GetAllPayments_WhenPaymentsExist_ReturnsPaginatedResult()
    {
        
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

        var payments = new List<Payment> { CreatePayment(1, 101, 500), CreatePayment(2, 102, 600) };

        var pagedResponse = new PagenationResponseDTO<Payment> { Items = payments, TotalCount = 2 };
        var paymentDTOs = new List<PaymentDisplayDTO> { CreatePaymentDisplayDTO(1, 101, 500), CreatePaymentDisplayDTO(2, 102, 600) };

        _mockPaymentRepository.Setup(r => r.GetAll(pagination)).ReturnsAsync(pagedResponse);
        _mockMapper.Setup(m => m.Map<IEnumerable<PaymentDisplayDTO>>(pagedResponse.Items)).Returns(paymentDTOs);

      
        var result = await _paymentService.GetAllPayments(pagination);

        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.Count(), Is.EqualTo(2));
        Assert.That(result.TotalCount, Is.EqualTo(2));
        _mockPaymentRepository.Verify(r => r.GetAll(pagination), Times.Once);
    }

    [Test]
    public void GetAllPayments_WhenNoPaymentsExist_ThrowsNoEntriessInCollectionException()
    {
      
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };
        var emptyPagedResponse = new PagenationResponseDTO<Payment> { Items = new List<Payment>(), TotalCount = 0 };
        _mockPaymentRepository.Setup(r => r.GetAll(pagination)).ReturnsAsync(emptyPagedResponse);

        
        Assert.ThrowsAsync<NoEntriessInCollectionException>(() => _paymentService.GetAllPayments(pagination));
    }

    [Test]
    public async Task GetPaymentByBookingId_WithExistingId_ReturnsPaymentDisplayDTO()
    {
        
        int bookingId = 101;
        var payment = CreatePayment(1, bookingId, 500);
        var paymentDTO = CreatePaymentDisplayDTO(1, bookingId, 500);
        _mockPaymentRepository.Setup(r => r.GetPaymentByBookingId(bookingId)).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<PaymentDisplayDTO>(payment)).Returns(paymentDTO);

        
        var result = await _paymentService.GetPaymentByBookingId(bookingId);

        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.BookingId, Is.EqualTo(bookingId));
        _mockPaymentRepository.Verify(r => r.GetPaymentByBookingId(bookingId), Times.Once);
    }

    [Test]
    public void GetPaymentByBookingId_WhenPaymentDoesNotExist_ThrowsNoSuchEntityException()
    {
        
        int bookingId = 999;
        _mockPaymentRepository.Setup(r => r.GetPaymentByBookingId(bookingId)).ReturnsAsync((Payment)null);

        
        Assert.ThrowsAsync<NoSuchEntityException>(async () => await _paymentService.GetPaymentByBookingId(bookingId));
    }

    [Test]
    public async Task GetPaymentsByUserId_WhenPaymentsExist_ReturnsPaginatedResult()
    {
        
        int userId = 1;
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

        var payments = new List<Payment> { CreatePayment(1, 101, 500), CreatePayment(2, 102, 600) };
        var pagedResponse = new PagenationResponseDTO<Payment> { Items = payments, TotalCount = 2 };

        var paymentDTOs = new List<PaymentDisplayDTO> { CreatePaymentDisplayDTO(1, 101, 500), CreatePaymentDisplayDTO(2, 102, 600) };

        _mockPaymentRepository.Setup(r => r.GetPaymentsByUserId(userId, pagination)).ReturnsAsync(pagedResponse);
        _mockMapper.Setup(m => m.Map<IEnumerable<PaymentDisplayDTO>>(pagedResponse.Items)).Returns(paymentDTOs);

        
        var result = await _paymentService.GetPaymentsByUserId(userId, pagination);

        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.Count(), Is.EqualTo(2));
        Assert.That(result.TotalCount, Is.EqualTo(2));
        _mockPaymentRepository.Verify(r => r.GetPaymentsByUserId(userId, pagination), Times.Once);
    }

    [Test]
    public void GetPaymentsByUserId_WhenNoPaymentsExist_ThrowsNoEntriessInCollectionException()
    {
       
        int userId = 1;
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };
        var emptyPagedResponse = new PagenationResponseDTO<Payment> { Items = new List<Payment>(), TotalCount = 0 };
        _mockPaymentRepository.Setup(r => r.GetPaymentsByUserId(userId, pagination)).ReturnsAsync(emptyPagedResponse);

        
        Assert.ThrowsAsync<NoEntriessInCollectionException>(async () => await _paymentService.GetPaymentsByUserId(userId, pagination));
    }

    [Test]
    public async Task ProcessPayment_WithValidBooking_ReturnsSuccessMessageAndCommitsTransaction()
    {
       
        var paymentDTO = CreatePaymentDTO(101, 500);

        var booking = new Booking { BookingId = 101, Status = "Booked" };
        var payment = CreatePayment(1, 101, 500);

        var mockTransaction = new Mock<IDbContextTransaction>();
        var mockDatabase = new Mock<Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade>(_mockContext.Object);


        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockTransaction.Object);
        _mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        _mockBookingRepository.Setup(r => r.GetById(paymentDTO.BookingId)).ReturnsAsync(booking);

        _mockMapper.Setup(m => m.Map<Payment>(paymentDTO)).Returns(payment);
        _mockPaymentRepository.Setup(r => r.Add(payment)).ReturnsAsync(payment);
        _mockBookingRepository.Setup(r => r.Update(booking)).ReturnsAsync(booking);

        
        var result = await _paymentService.ProcessPayment(paymentDTO);

        
        Assert.That(result, Is.EqualTo($"Payment for BookingId {paymentDTO.BookingId} processed successfully."));
        _mockBookingRepository.Verify(r => r.Update(booking), Times.Once);

        _mockPaymentRepository.Verify(r => r.Add(payment), Times.Once);
        mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public void ProcessPayment_WhenBookingDoesNotExist_ThrowsNoSuchEntityExceptionAndRollsback()
    {
        
        var paymentDTO = CreatePaymentDTO(999, 500);

        var mockTransaction = new Mock<IDbContextTransaction>();
        var mockDatabase = new Mock<Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade>(_mockContext.Object);

        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockTransaction.Object);
        _mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        _mockBookingRepository.Setup(r => r.GetById(paymentDTO.BookingId)).ReturnsAsync((Booking)null);

        
        Assert.ThrowsAsync<NoSuchEntityException>(async () => await _paymentService.ProcessPayment(paymentDTO));
        mockTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);

        mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ProcessPayment_WhenPaymentAlreadyProcessed_ReturnsMessageWithoutProcessing()
    {
        
        var paymentDTO = CreatePaymentDTO(101, 500);
        var booking = CreateBooking(101, "Completed");

        
        var mockTransaction = new Mock<IDbContextTransaction>();

        
        var mockDatabase = new Mock<Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade>(_mockContext.Object);

        
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockTransaction.Object);

        
        _mockContext.Setup(c => c.Database)
                    .Returns(mockDatabase.Object);

        _mockBookingRepository.Setup(r => r.GetById(paymentDTO.BookingId)).ReturnsAsync(booking);

        
        var result = await _paymentService.ProcessPayment(paymentDTO);

        
        Assert.That(result, Is.EqualTo("Payment already processed for this booking."));
        _mockBookingRepository.Verify(r => r.Update(It.IsAny<Booking>()), Times.Never);

        _mockPaymentRepository.Verify(r => r.Add(It.IsAny<Payment>()), Times.Never);

        
        mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mockTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public void ProcessPayment_WhenPaymentAddFails_ThrowsExceptionAndRollsback()
    {
       
        var paymentDTO = CreatePaymentDTO(101, 500);

        var booking = CreateBooking(101, "Pending");

        var payment = CreatePayment(1, 101, 500);
        var mockTransaction = new Mock<IDbContextTransaction>();

        var mockDatabase = new Mock<Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade>(_mockContext.Object);

        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockTransaction.Object);
        _mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        _mockBookingRepository.Setup(r => r.GetById(paymentDTO.BookingId)).ReturnsAsync(booking);

        _mockMapper.Setup(m => m.Map<Payment>(paymentDTO)).Returns(payment);
        _mockPaymentRepository.Setup(r => r.Add(payment)).ReturnsAsync((Payment)null);
        _mockBookingRepository.Setup(r => r.Update(booking)).ReturnsAsync(booking);

        
        var ex = Assert.ThrowsAsync<Exception>(async () => await _paymentService.ProcessPayment(paymentDTO));
        Assert.That(ex.Message, Is.EqualTo("Error processing payments."));

        mockTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}