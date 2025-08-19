using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class SeatServiceTests
{
    private Mock<ISeatRepository> _mockSeatRepository;
    private Mock<IMapper> _mockMapper;

    private Mock<ILogger<SeatService>> _mockLogger;
    private ISeatService _seatService;

    [SetUp]
    public void Setup()
    {
        _mockSeatRepository = new Mock<ISeatRepository>();
        _mockMapper = new Mock<IMapper>();

        _mockLogger = new Mock<ILogger<SeatService>>();

        _seatService = new SeatService(
            _mockSeatRepository.Object,_mockMapper.Object,_mockLogger.Object
        );
    }

    private IEnumerable<Seat> CreateSeats(int routeId, int count)
    {
        return Enumerable.Range(1, count).Select(i => new Seat
        {
            SeatId = i,SeatNumber = $"S{i}",IsBooked = false,RouteId = routeId,IsDeleted = false
        }).ToList();
    }

    private IEnumerable<SeatDTO> CreateSeatDTOs(int count)
    {
        return Enumerable.Range(1, count).Select(i => new SeatDTO
        {
            SeatId = i,

            SeatNumber = $"S{i}",
            
            Status = "Available" 
        }).ToList();
    }

    [Test]
    public async Task GetSeatsByRoute_WithExistingSeats_ReturnsPaginatedSeatDTOs()
    {
       
        int routeId = 1;
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

        var seats = CreateSeats(routeId, 5);

        var pagedResponse = new PagenationResponseDTO<Seat> { Items = seats, TotalCount = 5 };
        var seatDTOs = CreateSeatDTOs(5);

        _mockSeatRepository.Setup(r => r.GetSeatsByRoute(routeId, pagination)).ReturnsAsync(pagedResponse);
        _mockMapper.Setup(m => m.Map<IEnumerable<SeatDTO>>(seats)).Returns(seatDTOs);

        
        var result = await _seatService.GetSeatsByRoute(routeId, pagination);

       
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.Count(), Is.EqualTo(5));
        Assert.That(result.TotalCount, Is.EqualTo(5));
    }

    [Test]
    public void GetSeatsByRoute_WithNoSeats_ThrowsNoEntriessInCollectionException()
    {
       
        int routeId = 1;
        var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

        var emptyPagedResponse = new PagenationResponseDTO<Seat> { Items = new List<Seat>(), TotalCount = 0 };
        _mockSeatRepository.Setup(r => r.GetSeatsByRoute(routeId, pagination)).ReturnsAsync(emptyPagedResponse);

        
        Assert.ThrowsAsync<NoEntriessInCollectionException>(() => _seatService.GetSeatsByRoute(routeId, pagination));
    }

    [Test]
    public async Task SoftDeleteSeat_WithValidId_ReturnsTrue()
    {
       
        int seatId = 1;
        _mockSeatRepository.Setup(r => r.SoftDelete(seatId)).ReturnsAsync(true);

        
        var result = await _seatService.SoftDeleteSeat(seatId);

        
        Assert.IsTrue(result);
        _mockSeatRepository.Verify(r => r.SoftDelete(seatId), Times.Once);
    }

    [Test]
    public void SoftDeleteSeat_WithInvalidId_ThrowsNoSuchEntityException()
    {
       
        int seatId = 999;
        _mockSeatRepository.Setup(r => r.SoftDelete(seatId)).ReturnsAsync(false);

        
        Assert.ThrowsAsync<NoSuchEntityException>(() => _seatService.SoftDeleteSeat(seatId));
        _mockSeatRepository.Verify(r => r.SoftDelete(seatId), Times.Once);
    }

}