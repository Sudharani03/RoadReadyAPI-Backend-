using Microsoft.Extensions.Logging;
using Moq;
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
    internal class RentalStoreTest
    {
        private Mock<IRepository<int, RentalStore>> _mockRentalStoreRepository;
        private Mock<ILogger<RentalStoreService>> _mockLogger;
        private RentalStoreService _rentalStoreService;

        [SetUp]
        public void Setup()
        {
            _mockRentalStoreRepository = new Mock<IRepository<int, RentalStore>>();
            _mockLogger = new Mock<ILogger<RentalStoreService>>();
            _rentalStoreService = new RentalStoreService(_mockRentalStoreRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddRentalStoreTest_ValidRentalStore_ReturnsAddedRentalStore()
        {
            // Arrange
            var validRentalStore = new RentalStore { StoreId = 1, PickUpLocation = "Location1", DropOffLocation = "Location2" };
            _mockRentalStoreRepository.Setup(repo => repo.Add(validRentalStore)).ReturnsAsync(validRentalStore);

            // Act
            var addedRentalStore = await _rentalStoreService.AddRentalStore(validRentalStore);

            // Assert
            Assert.IsNotNull(addedRentalStore);
            Assert.AreEqual(validRentalStore, addedRentalStore);
        }

        [Test]
        public void AddRentalStore_RentalStoreAlreadyExistsTest_ThrowsException()
        {
            // Arrange
            var existingRentalStore = new RentalStore { StoreId = 1, PickUpLocation = "Location1", DropOffLocation = "Location2" };
            _mockRentalStoreRepository.Setup(repo => repo.Add(existingRentalStore)).ThrowsAsync(new RentalStoreAlreadyExistsException());

            // Act & Assert
            Assert.ThrowsAsync<RentalStoreAlreadyExistsException>(async () => await _rentalStoreService.AddRentalStore(existingRentalStore));
        }

        [Test]
        public async Task RemoveRentalStoreTest_ExistingStoreId_ReturnsRemovedRentalStore()
        {
            // Arrange
            var existingStoreId = 1;
            var rentalStoreToRemove = new RentalStore { StoreId = existingStoreId, PickUpLocation = "Location1", DropOffLocation = "Location2" };
            _mockRentalStoreRepository.Setup(repo => repo.Delete(existingStoreId)).ReturnsAsync(rentalStoreToRemove);

            // Act
            var removedRentalStore = await _rentalStoreService.RemoveRentalStore(existingStoreId);

            // Assert
            Assert.IsNotNull(removedRentalStore);
            Assert.AreEqual(rentalStoreToRemove, removedRentalStore);
        }

        [Test]
        public void RemoveRentalStore_NonExistingStoreIdTest_ThrowsException()
        {
            // Arrange
            var nonExistingStoreId = 2;
            _mockRentalStoreRepository.Setup(repo => repo.Delete(nonExistingStoreId)).ThrowsAsync(new NoSuchRentalStoreException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRentalStoreException>(async () => await _rentalStoreService.RemoveRentalStore(nonExistingStoreId));
        }

        [Test]
        public async Task GetAllRentalStoresTest_ReturnsListOfRentalStores()
        {
            // Arrange
            var rentalStores = new List<RentalStore>
        {
            new RentalStore { StoreId = 1, PickUpLocation = "Location1", DropOffLocation = "Location2" },
            new RentalStore { StoreId = 2, PickUpLocation = "Location3", DropOffLocation = "Location4" }
        };

            _mockRentalStoreRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(rentalStores);

            // Act
            var allRentalStores = await _rentalStoreService.GetAllRentalStores();

            // Assert
            Assert.IsNotNull(allRentalStores);
            Assert.AreEqual(rentalStores.Count, allRentalStores.Count);
        }

        [Test]
        public void GetAllRentalStores_EmptyListTest_ThrowsException()
        {
            // Arrange
            _mockRentalStoreRepository.Setup(repo => repo.GetAsync()).ThrowsAsync(new RentalStoreListEmptyException());

            // Act & Assert
            Assert.ThrowsAsync<RentalStoreListEmptyException>(async () => await _rentalStoreService.GetAllRentalStores());
        }

        [Test]
        public async Task GetRentalStoreByIdTest_ExistingStoreId_ReturnsRentalStore()
        {
            // Arrange
            var existingStoreId = 1;
            var existingRentalStore = new RentalStore { StoreId = existingStoreId, PickUpLocation = "Location1", DropOffLocation = "Location2" };
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(existingStoreId)).ReturnsAsync(existingRentalStore);

            // Act
            var retrievedRentalStore = await _rentalStoreService.GetRentalStoreById(existingStoreId);

            // Assert
            Assert.IsNotNull(retrievedRentalStore);
            Assert.AreEqual(existingRentalStore, retrievedRentalStore);
        }

        [Test]
        public void GetRentalStoreById_NonExistingStoreIdTest_ThrowsException()
        {
            // Arrange
            var nonExistingStoreId = 2;
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(nonExistingStoreId)).ThrowsAsync(new NoSuchRentalStoreException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRentalStoreException>(async () => await _rentalStoreService.GetRentalStoreById(nonExistingStoreId));
        }


        [Test]
        public void GetCarsInStore_NonExistingStoreId_ThrowsException()
        {
            // Arrange
            var nonExistingStoreId = 2;
            _mockRentalStoreRepository.Setup(repo => repo.GetAsyncById(nonExistingStoreId)).ThrowsAsync(new NoSuchRentalStoreException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRentalStoreException>(async () => await _rentalStoreService.GetCarsInStore(nonExistingStoreId));
        }


    }
}
