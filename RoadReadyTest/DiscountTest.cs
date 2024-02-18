using Microsoft.Extensions.Logging;
using Moq;
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
    internal class DiscountTest
    {
        private Mock<IRepository<int, Discount>> _mockDiscountRepository;
        private Mock<IRepository<int, Reservation>> _mockReservationRepository;
        private Mock<IRepository<int, Car>> _mockCarRepository;
        private Mock<ILogger<DiscountService>> _mockLogger;

        private DiscountService _discountService;

        [SetUp]
        public void Setup()
        {
            // Initialize mock repositories and logger
            _mockDiscountRepository = new Mock<IRepository<int, Discount>>();
            _mockReservationRepository = new Mock<IRepository<int, Reservation>>();
            _mockCarRepository = new Mock<IRepository<int, Car>>();
            _mockLogger = new Mock<ILogger<DiscountService>>();

            // Create an instance of DiscountService with the mock repositories and logger
            _discountService = new DiscountService(
                _mockDiscountRepository.Object,
                _mockReservationRepository.Object,
                _mockCarRepository.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task AddNewDiscountTest_ValidDiscount_ReturnsAddedDiscount()
        {
            // Arrange
            var discountToAdd = new Discount { DiscountId = 1, DiscountName = "Test Discount" };
            _mockDiscountRepository.Setup(repo => repo.Add(discountToAdd)).ReturnsAsync(discountToAdd);

            // Act
            var addedDiscount = await _discountService.AddNewDiscount(discountToAdd);

            // Assert
            Assert.IsNotNull(addedDiscount);
            Assert.AreEqual(discountToAdd, addedDiscount);
        }

        

        [Test]
        public async Task AssignDiscountToCar_ValidInputs_ReturnsTrue()
        {
            // Arrange
            var discountId = 1;
            var carId = 2;
            var existingDiscount = new Discount { DiscountId = discountId };
            var car = new Car { CarId = carId };

            _mockDiscountRepository.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(existingDiscount);
            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);
            _mockCarRepository.Setup(repo => repo.Update(car)).ReturnsAsync(car);

            // Act
            var isAssigned = await _discountService.AssignDiscountToCar(discountId, carId);

            // Assert
            Assert.IsTrue(isAssigned);
        }

        [Test]
        public async Task DeactivateDiscount_ValidDiscountId_ReturnsTrue()
        {
            // Arrange
            var discountId = 1;
            var existingDiscount = new Discount { DiscountId = discountId };

            _mockDiscountRepository.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(existingDiscount);
            _mockDiscountRepository.Setup(repo => repo.Update(existingDiscount)).ReturnsAsync(existingDiscount);

            // Act
            var isDeactivated = await _discountService.DeactivateDiscount(discountId);

            // Assert
            Assert.IsTrue(isDeactivated);
        }

        [Test]
        public async Task RemoveDiscountFromCar_ValidInputs_ReturnsTrue()
        {
            // Arrange
            var discountId = 1;
            var carId = 2;
            var existingDiscount = new Discount { DiscountId = discountId };
            var car = new Car { CarId = carId, Discounts = new List<Discount> { existingDiscount } };

            _mockDiscountRepository.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(existingDiscount);
            _mockCarRepository.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);
            _mockCarRepository.Setup(repo => repo.Update(car)).ReturnsAsync(car);

            // Act
            var isRemoved = await _discountService.RemoveDiscountFromCar(discountId, carId);

            // Assert
            Assert.IsTrue(isRemoved);
        }

        [Test]
        public async Task ViewAllDiscounts_ReturnsListOfDiscounts()
        {
            // Arrange
            var discounts = new List<Discount> { new Discount { DiscountId = 1 }, new Discount { DiscountId = 2 } };
            _mockDiscountRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(discounts);

            // Act
            var allDiscounts = await _discountService.ViewAllDiscounts();

            // Assert
            Assert.IsNotNull(allDiscounts);
            Assert.AreEqual(discounts.Count, allDiscounts.Count);
        }

        
        

        [Test]
        public async Task ViewCarsWithDiscounts_ReturnsListOfCars()
        {
            // Arrange
            var discounts = new List<Discount> { new Discount { DiscountId = 1, Cars = new List<Car> { new Car { CarId = 1 } } } };
            _mockDiscountRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(discounts);

            // Act
            var carsWithDiscounts = await _discountService.ViewCarsWithDiscounts();

            // Assert
            Assert.IsNotNull(carsWithDiscounts);
            Assert.AreEqual(1, carsWithDiscounts.Count);
        }

        [Test]
        public async Task ViewDiscountDetails_ValidDiscountId_ReturnsDiscount()
        {
            // Arrange
            var discountId = 1;
            var discount = new Discount { DiscountId = discountId };

            _mockDiscountRepository.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(discount);

            // Act
            var discountDetails = await _discountService.ViewDiscountDetails(discountId);

            // Assert
            Assert.IsNotNull(discountDetails);
            Assert.AreEqual(discountId, discountDetails.DiscountId);
        }

    }
}
