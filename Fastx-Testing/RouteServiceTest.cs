using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FastxTest
{
    internal class RouteServiceTest
    {
        private Mock<IRouteRepository> _mockRouteRepository;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IBusRepository> _mockBusRepository;

        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<RouteService>> _mockLogger;

        private ApplicationDbContext _context;
        private RouteService _routeService;

        [SetUp]
        public void Setup()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockBusRepository = new Mock<IBusRepository>();

            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RouteService>>();

            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _routeService = new RouteService(
                _mockRouteRepository.Object,_mockUserRepository.Object,_mockBusRepository.Object,_mockMapper.Object,_mockLogger.Object,_context
            );
        }

        [Test]
        public async Task AddRoute_ShouldAdd_WhenUserIsBusOperator()
        {
           
            var routeDto = new RouteDTO { BusId = 1 };

            var bus = new Bus { BusId = 1, BusOperator = new User { UserId = 2, RoleId = 2 } };
            _context.Buses.Add(bus);
            _context.SaveChanges();

            var user = new User { UserId = 2, RoleId = 2 };

            var route = new Route { RouteId = 99 };

            _mockUserRepository.Setup(r => r.GetById(2)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<Route>(routeDto)).Returns(route);

            _mockRouteRepository.Setup(r => r.Add(route)).ReturnsAsync(route);

            _mockMapper.Setup(m => m.Map<RouteDTO>(route)).Returns(new RouteDTO { RouteId = 99 });

            
            var result = await _routeService.AddRoute(routeDto, 2);

           
            Assert.NotNull(result);

            Assert.AreEqual(99, result.RouteId);
        }

        [Test]
        public void AddRoute_ShouldThrow_WhenUserNotAuthorized()
        {
            var routeDto = new RouteDTO { BusId = 1 };

            _context.Buses.Add(new Bus { BusId = 1 });
            _context.SaveChanges();

            _mockUserRepository.Setup(r => r.GetById(3)).ReturnsAsync(new User { UserId = 3, RoleId = 1 });

            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>_routeService.AddRoute(routeDto, 3));
        }

        [Test]
        public void AddRoute_ShouldThrow_WhenBusNotFound()
        {
            var routeDto = new RouteDTO { BusId = 99 };

            _mockUserRepository.Setup(r => r.GetById(1)).ReturnsAsync(new User { UserId = 1, RoleId = 2 });

            Assert.ThrowsAsync<NoSuchEntityException>(() =>_routeService.AddRoute(routeDto, 1));
        }

        [Test]
        public async Task UpdateRoute_ShouldUpdate_WhenAuthorized()
        {
            var user = new User { UserId = 1, Role = new Role { RoleName = "Admin" } };
            var route = new Route
            {
                RouteId = 1,

                Bus = new Bus { BusOperator = new User { UserId = 1 } }
            };

            _mockUserRepository.Setup(r => r.GetById(1)).ReturnsAsync(user);
            _mockRouteRepository.Setup(r => r.GetById(1)).ReturnsAsync(route);

            _mockMapper.Setup(m => m.Map(It.IsAny<RouteDTO>(), route));

            var result = await _routeService.UpdateRoute(1, new RouteDTO(), 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateRoute_ShouldThrow_WhenUnauthorized()
        {
            var user = new User { UserId = 2, Role = new Role { RoleName = "User" } };
            var route = new Route
            {
                RouteId = 1,

                Bus = new Bus { BusOperator = new User { UserId = 1 } }
            };

            _mockUserRepository.Setup(r => r.GetById(2)).ReturnsAsync(user);

            _mockRouteRepository.Setup(r => r.GetById(1)).ReturnsAsync(route);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>_routeService.UpdateRoute(1, new RouteDTO(), 2));
        }

        [Test]
        public async Task SoftDeleteRoute_ShouldDelete_WhenAuthorized()
        {
            var user = new User { UserId = 1, Role = new Role { RoleName = "Admin" } };
            var route = new Route
            {
                RouteId = 1,

                Bus = new Bus { BusOperator = new User { UserId = 1 } }
            };

            _mockUserRepository.Setup(r => r.GetById(1)).ReturnsAsync(user);

            _mockRouteRepository.Setup(r => r.GetById(1)).ReturnsAsync(route);
            _mockRouteRepository.Setup(r => r.SoftDelete(1)).ReturnsAsync(true);

            var result = await _routeService.SoftDeleteRoute(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void GetAllRoutes_ShouldThrow_WhenNoRoutes()
        {
            _mockRouteRepository.Setup(r => r.GetAll(It.IsAny<PagenationRequestDTO>()))
                .ReturnsAsync(new PagenationResponseDTO<Route>
                {
                    Items = new List<Route>(),

                    TotalCount = 0
                });

            Assert.ThrowsAsync<NoEntriessInCollectionException>(() =>_routeService.GetAllRoutes(new PagenationRequestDTO()));
        }

        [Test]
        public async Task GetAllRoutes_ShouldReturnData_WhenRoutesExist()
        {
            _mockRouteRepository.Setup(r => r.GetAll(It.IsAny<PagenationRequestDTO>()))
                .ReturnsAsync(new PagenationResponseDTO<Route>
                {
                    Items = new List<Route> { new Route { RouteId = 1 } },
                    TotalCount = 1
                });
            _mockMapper.Setup(m => m.Map<IEnumerable<RouteDTO>>(It.IsAny<IEnumerable<Route>>())).Returns(new List<RouteDTO> { new RouteDTO { RouteId = 1 } });

            var result = await _routeService.GetAllRoutes(new PagenationRequestDTO());

            Assert.AreEqual(1, result.TotalCount);

            Assert.AreEqual(1, result.Items.First().RouteId);
        }

        [Test]
        public async Task SearchRoutes_ShouldReturnResults_WhenFound()
        {
            _mockRouteRepository.Setup(r => r.SearchRoutes(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<PagenationRequestDTO>()))
                .ReturnsAsync(new PagenationResponseDTO<Route>
                {
                    Items = new List<Route> { new Route { RouteId = 1 } },
                    TotalCount = 1
                });

            _mockMapper.Setup(m => m.Map<IEnumerable<RouteDTO>>(It.IsAny<IEnumerable<Route>>())).Returns(new List<RouteDTO> { new RouteDTO { RouteId = 1 } });

            var result = await _routeService.SearchRoutes("A", "B", DateTime.UtcNow, new PagenationRequestDTO());

            Assert.AreEqual(1, result.TotalCount);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }






    }
}
