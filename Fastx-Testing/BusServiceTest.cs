using System;
using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Repository;
using FastxWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FastxTest
{
    public class BusServiceTest
    {
        private Mock<IBusRepository> _busRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<BusService>> _loggerMock;
        private BusService _busService;

        [SetUp]
        public void Setup()
        {
            _busRepositoryMock = new Mock<IBusRepository>();

            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BusService>>();

            _busService = new BusService(
                _busRepositoryMock.Object,_mapperMock.Object,_loggerMock.Object,_userRepositoryMock.Object
            );
        }

        [Test]
        public async Task AddBus_ValidBusOperator_AddsBusSuccessfully()
        {
            var busDTO = new BusDTO { BusName = "Test Bus" };
            var busOperatorId = 3;
            var user = new User { UserId = busOperatorId, RoleId = 3 };

            var busEntity = new Bus();
            var addedBusEntity = new Bus { BusId = 10, BusOperatorId = busOperatorId };

            _userRepositoryMock.Setup(r => r.GetById(busOperatorId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Bus>(busDTO)).Returns(busEntity);
            _busRepositoryMock.Setup(r => r.Add(busEntity)).ReturnsAsync(addedBusEntity);
            _mapperMock.Setup(m => m.Map<BusDTO>(addedBusEntity)).Returns(new BusDTO { BusId = 10, BusName = "Test Bus" });

            var result = await _busService.AddBus(busDTO, busOperatorId);

            Assert.IsNotNull(result);

            Assert.AreEqual(10, result.BusId);
            _userRepositoryMock.Verify(r => r.GetById(busOperatorId), Times.Once);

            _busRepositoryMock.Verify(r => r.Add(busEntity), Times.Once);
        }

        [Test]
        public void AddBus_InvalidBusOperator_ThrowsNoSuchEntityException()
        {
            var busDTO = new BusDTO { BusName = "Test Bus" };
            var busOperatorId = 5;
            User nullUser = null;

            _userRepositoryMock.Setup(r => r.GetById(busOperatorId)).ReturnsAsync(nullUser);

            var ex = Assert.ThrowsAsync<Exception>(() => _busService.AddBus(busDTO, busOperatorId));

            Assert.That(ex.InnerException, Is.TypeOf<NoSuchEntityException>());
            Assert.That(ex.InnerException.Message, Is.EqualTo("Bus operator not found or is not authorized."));
        }



        [Test]
        public async Task UpdateBus_ExistingBus_ReturnsTrue()
        {
            int busId = 1;
            var busDTO = new BusDTO { BusName = "Updated Bus" };

            var existingBus = new Bus { BusId = busId };

            _busRepositoryMock.Setup(r => r.GetById(busId)).ReturnsAsync(existingBus);
            _mapperMock.Setup(m => m.Map(busDTO, existingBus));

            _busRepositoryMock.Setup(r => r.Update(existingBus)).ReturnsAsync(existingBus);

            var result = await _busService.UpdateBus(busId, busDTO);

            Assert.IsTrue(result);
            _busRepositoryMock.Verify(r => r.GetById(busId), Times.Once);
            _busRepositoryMock.Verify(r => r.Update(existingBus), Times.Once);
        }

        [Test]
        public async Task UpdateBus_NonExistingBus_ReturnsFalse()
        {
            int busId = 1;
            var busDTO = new BusDTO();
            Bus nullBus = null;

            _busRepositoryMock.Setup(r => r.GetById(busId)).ReturnsAsync(nullBus);

            var result = await _busService.UpdateBus(busId, busDTO);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteBus_ExistingBus_ReturnsTrue()
        {
            int busId = 1;

            _busRepositoryMock.Setup(r => r.SoftDelete(busId)).ReturnsAsync(true);

            var result = await _busService.SoftDeleteBus(busId);

            Assert.IsTrue(result);
            _busRepositoryMock.Verify(r => r.SoftDelete(busId), Times.Once);
        }

        [Test]
        public async Task SoftDeleteBus_NonExistingBus_ReturnsFalse()
        {
            int busId = 1;

            _busRepositoryMock.Setup(r => r.SoftDelete(busId)).ReturnsAsync(false);

            var result = await _busService.SoftDeleteBus(busId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllBuses_WithBuses_ReturnsPagedResult()
        {
            var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };
            var buses = new PagenationResponseDTO<Bus>
            {
                TotalCount = 2,

                PageSize = 10,
                CurrentPage = 1,
                Items = new List<Bus>
                {
                    new Bus { BusId = 1,   BusName = "Bus1" },


                    new Bus { BusId = 2, BusName = "Bus2" }
                }
            };
            var busDTOs = new List<BusDTO>
            {
                new BusDTO { BusId = 1, BusName = "Bus1" },

                new BusDTO { BusId = 2, BusName = "Bus2" }
            };

            _busRepositoryMock.Setup(r => r.GetAll(pagination)).ReturnsAsync(buses);
            _mapperMock.Setup(m => m.Map<IEnumerable<BusDTO>>(buses.Items)).Returns(busDTOs);

            var result = await _busService.GetAllBuses(pagination);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount);

            Assert.AreEqual(2, result.Items.Count());
            _busRepositoryMock.Verify(r => r.GetAll(pagination), Times.Once);
        }

        [Test]
        public void GetAllBuses_NoBuses_ThrowsNoEntriessInCollectionException()
        {
            var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };
            var emptyBuses = new PagenationResponseDTO<Bus>
            {
                TotalCount = 0,
                Items = new List<Bus>()
            };

            _busRepositoryMock.Setup(r => r.GetAll(pagination)).ReturnsAsync(emptyBuses);

            Assert.ThrowsAsync<NoEntriessInCollectionException>(() => _busService.GetAllBuses(pagination));
        }

        [Test]
        public async Task GetBusById_ExistingBus_ReturnsBusDTO()
        {
            int busId = 1;
            var bus = new Bus { BusId = busId, BusName = "Bus1" };

            var busDTO = new BusDTO { BusId = busId, BusName = "Bus1" };

            _busRepositoryMock.Setup(r => r.GetById(busId)).ReturnsAsync(bus);
            _mapperMock.Setup(m => m.Map<BusDTO>(bus)).Returns(busDTO);

            var result = await _busService.GetBusById(busId);

            Assert.IsNotNull(result);
            Assert.AreEqual(busId, result.BusId);
            _busRepositoryMock.Verify(r => r.GetById(busId), Times.Once);
        }

        [Test]
        public void GetBusById_NonExistingBus_ThrowsNoSuchEntityException()
        {
            int busId = 1;
            Bus nullBus = null;

            _busRepositoryMock.Setup(r => r.GetById(busId)).ReturnsAsync(nullBus);

            Assert.ThrowsAsync<NoSuchEntityException>(() => _busService.GetBusById(busId));
        }

    }
}
