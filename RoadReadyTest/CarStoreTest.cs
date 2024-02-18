using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadReadyTest
{
    internal class CarStoreTest
    {
        CarRentalDbContext _context;
        private CarStoreService _carStoreService;
        private Mock<IRepository<int, CarStore>> _mockRepo;
        private Mock<IRepository<int, Car>> _mockCarRepository;
        private Mock<IRepository<int, RentalStore>> _mockRentalStoreRepository;
        private Mock<ILogger<CarStoreService>> _mockLogger;

        // <summary>
        /// Setup Method in which InMemory dummy database is created
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            _context = new CarRentalDbContext(options);

            _mockRepo = new Mock<IRepository<int, CarStore>>();
            _mockCarRepository = new Mock<IRepository<int, Car>>();
            _mockRentalStoreRepository = new Mock<IRepository<int, RentalStore>>();
            _mockLogger = new Mock<ILogger<CarStoreService>>();
            _carStoreService = new CarStoreService(_mockRepo.Object, _mockCarRepository.Object, _mockRentalStoreRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddCarToStore_WithNoSuchCarException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ThrowsAsync(new NoSuchCarException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchCarException>(async () => await _carStoreService.AddCarToStore(storeId, carId));
        }

        [Test]
        public async Task AddCarToStore_WithNoSuchRentalStoreException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(new Car { CarId = carId });
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ThrowsAsync(new NoSuchRentalStoreException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRentalStoreException>(async () => await _carStoreService.AddCarToStore(storeId, carId));
        }

        [Test]
        public async Task AddCarToStore_WithCarAlreadyExistsException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(new Car { CarId = carId });
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(new RentalStore { StoreId = storeId });
            _mockRepo.Setup(repo => repo.Add(It.IsAny<CarStore>())).ThrowsAsync(new CarAlreadyExistsException());

            // Act & Assert
            Assert.ThrowsAsync<CarAlreadyExistsException>(async () => await _carStoreService.AddCarToStore(storeId, carId));
        }

        [Test]
        public async Task AddCarToStore_WithGeneralException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(new Car { CarId = carId });
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(new RentalStore { StoreId = storeId });
            _mockRepo.Setup(repo => repo.Add(It.IsAny<CarStore>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _carStoreService.AddCarToStore(storeId, carId));
        }

        //[Test]
        //public async Task RemoveCarFromStore_WithValidInput_ReturnsCarStore()
        //{
        //    // Arrange
        //    int storeId = 1;
        //    int carId = 1;

        //    var store = new RentalStore { StoreId = storeId };
        //    var carStore = new CarStore { CarId = carId, StoreId = storeId };

        //    _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(store);
        //    _mockRepo.Setup(repo => repo.Delete(storeId)).ReturnsAsync(carStore);

        //    // Act
        //    var result = await _carStoreService.RemoveCarFromStore(storeId, carId);

        //    // Assert
        //    Assert.That(result, Is.EqualTo(carStore));
        //}

        [Test]
        public async Task RemoveCarFromStore_WithNoSuchRentalStoreException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ThrowsAsync(new NoSuchRentalStoreException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRentalStoreException>(async () => await _carStoreService.RemoveCarFromStore(storeId, carId));
        }

        [Test]
        public async Task RemoveCarFromStore_WithNoSuchCarException_ThrowsException()
        {
            // Arrange
            int storeId = 1;
            int carId = 1;

            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(new RentalStore { StoreId = storeId });

            // Act & Assert
            Assert.ThrowsAsync<NoSuchCarException>(async () => await _carStoreService.RemoveCarFromStore(storeId, carId));
        }

        //[Test]
        //public async Task RemoveCarFromStore_WithValidInput_ReturnsCarStore()
        //{
        //    //Arrange
        //    int storeId = 1;
        //    int carId = 1;

        //    var store = new RentalStore { StoreId = storeId };
        //    var carStore = new CarStore { CarId = carId, StoreId = storeId };

        //    _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(store);
        //    _mockRepo.Setup(repo => repo.Delete(storeId)).ReturnsAsync(carStore);

        //    //Act
        //   var result = await _carStoreService.RemoveCarFromStore(storeId, carId);

        //    //Assert
        //    Assert.That(result, Is.EqualTo(carStore));
        //}

        //[Test]
        //public async Task RemoveCarFromStore_WithGeneralException_ThrowsException()
        //{
        //    // Arrange
        //    int storeId = 1;
        //    int carId = 1;

        //    _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(storeId)).ReturnsAsync(new RentalStore { StoreId = storeId });
        //    _mockRepo.Setup(repo => repo.Delete(storeId)).ThrowsAsync(new Exception());

        //    // Act & Assert
        //    Assert.ThrowsAsync<Exception>(async () => await _carStoreService.RemoveCarFromStore(storeId, carId));
        //}

        [Test]
        public async Task ViewAllCarsInAllStores_ReturnsAllCars()
        {
            // Arrange
            var stores = new List<RentalStore>
        {
            new RentalStore
            {
                StoreId = 1,
                CarStore = new List<CarStore>
                {
                    new CarStore { Car = new Car { CarId = 1 } },
                    new CarStore { Car = new Car { CarId = 2 } }
                }
            },
            new RentalStore
            {
                StoreId = 2,
                CarStore = new List<CarStore>
                {
                    new CarStore { Car = new Car { CarId = 3 } }
                }
            }
        };

            _mockRentalStoreRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(stores);

            // Act
            var result = await _carStoreService.ViewAllCarsInAllStores();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.Any(car => car.CarId == 1));
            Assert.That(result.Any(car => car.CarId == 2));
            Assert.That(result.Any(car => car.CarId == 3));
        }

        [Test]
        public async Task ViewAllCarsInAllStores_WithEmptyStores_ReturnsEmptyList()
        {
            // Arrange
            var stores = new List<RentalStore>();

            _mockRentalStoreRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(stores);

            // Act
            var result = await _carStoreService.ViewAllCarsInAllStores();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void ViewAllCarsInAllStores_WithException_ThrowsException()
        {
            // Arrange
            _mockRentalStoreRepository.Setup(repo => repo.GetAsync()).ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _carStoreService.ViewAllCarsInAllStores());
        }


    }
}
