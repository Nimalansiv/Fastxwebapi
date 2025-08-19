using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
public class RefundServiceTests
{
    private Mock<IRefundRepository> _mockRefundRepository;
    private Mock<IBookingRepository> _mockBookingRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<ApplicationDbContext> _mockContext;
    private Mock<ILogger<RefundService>> _mockLogger;
    private RefundService _refundService;

    [SetUp]
    public void Setup()
    {
        _mockRefundRepository = new Mock<IRefundRepository>();
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<RefundService>>();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        _mockContext = new Mock<ApplicationDbContext>(options);

        
        var mockTransaction = new Mock<IDbContextTransaction>();
        var mockDatabaseFacade = new Mock<DatabaseFacade>(_mockContext.Object);
        mockDatabaseFacade.Setup(db => db.BeginTransactionAsync(default)).ReturnsAsync(mockTransaction.Object);

        _mockContext.Setup(c => c.Database).Returns(mockDatabaseFacade.Object);

        _refundService = new RefundService(
            _mockRefundRepository.Object,
            _mockBookingRepository.Object,
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mockContext.Object,
            _mockLogger.Object
        );
    }

    [Test]
    public void ApproveRefund_InvalidProcessedByUserId_ThrowsInvalidActionException()
    {
        Assert.ThrowsAsync<InvalidActionException>(() =>
            _refundService.ApproveRefund(1, 0));
    }

    [Test]
    public void ApproveRefund_RefundNotFound_ThrowsNoSuchEntityException()
    {
        _mockRefundRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Refund)null);

        Assert.ThrowsAsync<NoSuchEntityException>(() =>_refundService.ApproveRefund(1, 10));
    }

    [Test]
    public void ApproveRefund_StatusNotPending_ThrowsInvalidActionException()
    {
        _mockRefundRepository.Setup(r => r.GetById(It.IsAny<int>())) .ReturnsAsync(new Refund { Status = "Approved" });

        Assert.ThrowsAsync<InvalidActionException>(() =>_refundService.ApproveRefund(1, 10));
    }

    [Test]
    public void ApproveRefund_UserNotAdmin_ThrowsInvalidActionException()
    {
        _mockRefundRepository.Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync(new Refund { Status = "Pending" });

        _mockUserRepository.Setup(u => u.GetById(It.IsAny<int>()))
            .ReturnsAsync(new User { RoleId = 1 }); 

        Assert.ThrowsAsync<InvalidActionException>(() =>
            _refundService.ApproveRefund(1, 10));
    }

    [Test]
    public async Task ApproveRefund_Successful_ReturnsSuccessMessage()
    {
        int refundId = 1;
        int processedByUserId = 2;

        var refund = new Refund
        {
            RefundId = refundId,
            BookingId = 10,
            Status = "Pending"
        };

        var booking = new Booking
        {
            BookingId = 10,
            Status = "Booked"
        };

        var adminUser = new User
        {
            UserId = processedByUserId,
            RoleId = 2 
        };

        _mockRefundRepository.Setup(r => r.GetById(refundId)).ReturnsAsync(refund);
        _mockUserRepository.Setup(u => u.GetById(processedByUserId)).ReturnsAsync(adminUser);

        _mockBookingRepository.Setup(b => b.GetById(refund.BookingId)).ReturnsAsync(booking);



        _mockRefundRepository.Setup(r => r.Update(It.IsAny<Refund>())).ReturnsAsync((Refund r) => r);

        _mockBookingRepository.Setup(b => b.Update(It.IsAny<Booking>())).ReturnsAsync((Booking b) => b);

        var result = await _refundService.ApproveRefund(refundId, processedByUserId);

        Assert.AreEqual("Refund approved successfully.", result);
        _mockRefundRepository.Verify(r => r.Update(It.Is<Refund>(x => x.Status == "Approved")), Times.Once);
        _mockBookingRepository.Verify(b => b.Update(It.Is<Booking>(x => x.Status == "Refunded")), Times.Once);
    }

    [Test]
    public async Task GetAllRefunds_WithResults_ReturnsPaged()
    {
        var refunds = new List<Refund> { new Refund() };
        var paged = new PagenationResponseDTO<Refund>
        {
            Items = refunds,
            TotalCount = 1
        };

        _mockRefundRepository.Setup(r => r.GetAll(It.IsAny<PagenationRequestDTO>())).ReturnsAsync(paged);

        _mockMapper.Setup(m => m.Map<IEnumerable<RefundDTO>>(refunds)).Returns(new List<RefundDTO> { new RefundDTO() });

        var result = await _refundService.GetAllRefunds(new PagenationRequestDTO());

        Assert.AreEqual(1, result.TotalCount);
    }

    [Test]
    public void GetAllRefunds_NoResults_Throws()
    {
        _mockRefundRepository.Setup(r => r.GetAll(It.IsAny<PagenationRequestDTO>())).ReturnsAsync(new PagenationResponseDTO<Refund> { Items = new List<Refund>() });

        Assert.ThrowsAsync<NoEntriessInCollectionException>(() =>_refundService.GetAllRefunds(new PagenationRequestDTO()));
    }

    [Test]
    public async Task GetPendingRefunds_WithResults_ReturnsPaged()
    {
        var refunds = new List<Refund> { new Refund() };
        var paged = new PagenationResponseDTO<Refund>
        {
            Items = refunds,
            TotalCount = 1
        };

        _mockRefundRepository.Setup(r => r.GetPendingRefunds(It.IsAny<PagenationRequestDTO>())).ReturnsAsync(paged);

        _mockMapper.Setup(m => m.Map<IEnumerable<RefundDTO>>(refunds)).Returns(new List<RefundDTO> { new RefundDTO() });

        var result = await _refundService.GetPendingRefunds(new PagenationRequestDTO());

        Assert.AreEqual(1, result.TotalCount);
    }

    [Test]
    public void GetPendingRefunds_NoResults_Throws()
    {
        _mockRefundRepository.Setup(r => r.GetPendingRefunds(It.IsAny<PagenationRequestDTO>())).ReturnsAsync(new PagenationResponseDTO<Refund> { Items = new List<Refund>() });

        Assert.ThrowsAsync<NoEntriessInCollectionException>(() =>_refundService.GetPendingRefunds(new PagenationRequestDTO()));
    }

    [Test]
    public async Task GetRefundByBookingId_Found_ReturnsDto()
    {
        var refund = new Refund { RefundId = 1 };
        _mockRefundRepository.Setup(r => r.GetRefundByBookingId(100)).ReturnsAsync(refund);
        _mockMapper.Setup(m => m.Map<RefundDTO>(refund)).Returns(new RefundDTO { RefundId = 1 });

        var result = await _refundService.GetRefundByBookingId(100);

        Assert.AreEqual(1, result.RefundId);
    }

    [Test]
    public void GetRefundByBookingId_NotFound_Throws()
    {
        _mockRefundRepository.Setup(r => r.GetRefundByBookingId(100)).ReturnsAsync((Refund)null);

        Assert.ThrowsAsync<NoSuchEntityException>(() =>_refundService.GetRefundByBookingId(100));
    }

    [Test]
    public async Task GetRefundsByUserId_WithResults_ReturnsPaged()
    {
        var refunds = new List<Refund> { new Refund() };
        var paged = new PagenationResponseDTO<Refund>
        {
            Items = refunds,

            TotalCount = 1
        };

        _mockRefundRepository.Setup(r => r.GetRefundsByUserId(1, It.IsAny<PagenationRequestDTO>())).ReturnsAsync(paged);

        _mockMapper.Setup(m => m.Map<IEnumerable<RefundDTO>>(refunds)).Returns(new List<RefundDTO> { new RefundDTO() });

        var result = await _refundService.GetRefundsByUserId(1, new PagenationRequestDTO());

        Assert.AreEqual(1, result.TotalCount);
    }

    [Test]
    public void GetRefundsByUserId_NoResults_Throws()
    {
        _mockRefundRepository.Setup(r => r.GetRefundsByUserId(1, It.IsAny<PagenationRequestDTO>())).ReturnsAsync(new PagenationResponseDTO<Refund> { Items = new List<Refund>() });

        Assert.ThrowsAsync<NoEntriessInCollectionException>(() =>_refundService.GetRefundsByUserId(1, new PagenationRequestDTO()));
    }



}