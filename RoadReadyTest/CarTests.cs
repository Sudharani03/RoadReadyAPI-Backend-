using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Repositories;
using RoadReady.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadReadyTest
{
    internal class CarTests
    {
        CarRentalDbContext context;
        private CarService _carService;
        private Mock<IRepository<int, Car>> _mockRepo;
        private Mock<IRepository<int, Discount>> _mockDiscountRepo;
        private Mock<ILogger<CarService>> _mockLogger;

        // <summary>
        /// Setup Method in which InMemory dummy database is created
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new CarRentalDbContext(options);

            _mockRepo = new Mock<IRepository<int, Car>>();
            _mockDiscountRepo = new Mock<IRepository<int, Discount>>();
            _mockLogger = new Mock<ILogger<CarService>>();
            _carService = new CarService(_mockRepo.Object, _mockDiscountRepo.Object, _mockLogger.Object);
            

        }
        

        /// <summary>
        /// Test mathod for GetCarsList
        /// </summary>
        /// <returns></returns>

        [Test]
        public async Task GetCarsListTest()
        {
            // Arrange
            var carList = new List<Car>
        {
            new Car
            {
                CarId = 1,
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Availability = true,
                DailyRate = 50.00,
                ImageURL = "example.com/toyota_camry.jpg",
                Specification = "Some specifications...",
                Discount = new Discount { DiscountId = 1, DiscountPercentage = 10.0 },
                CarStore = new List<CarStore>(),
                Reviews = new List<Review>(),
                Reservations = new List<Reservation>(),
                Payments = new List<Payment>(),
                Discounts = new List<Discount> { new Discount { DiscountId = 2, DiscountPercentage = 5.0 } }
            },
            // Add more sample cars as needed...
        };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(carList);

            // Act
            var result = await _carService.GetCarsList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Car>>(result);
            Assert.AreEqual(carList.Count, result.Count);

            // You might want to add more specific assertions based on your business logic.
            // For example, compare specific properties of the cars.
        }

     /// <summary>
     /// Test method for AddCars
     /// </summary>
     /// <returns></returns>

        [Test]
        
        public async Task AddCarTest()
        {
            // Arrange
            var validCar = new Car
            {
                CarId = 1,
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Availability = true,
                DailyRate = 50.00,
                ImageURL = "example.com/toyota_camry.jpg",
                Specification = "Some specifications...",
                Discount = new Discount { DiscountId = 1, DiscountPercentage = 10.0 },
                CarStore = new List<CarStore>(),
                Reviews = new List<Review>(),
                Reservations = new List<Reservation>(),
                Payments = new List<Payment>(),
                Discounts = new List<Discount> { new Discount { DiscountId = 2, DiscountPercentage = 5.0 } }
            };

            _mockRepo.Setup(repo => repo.Add(validCar)).ReturnsAsync(validCar);

            // Act
            var result = await _carService.AddCar(validCar);

            // Assert
            Assert.That(result, Is.TypeOf<Car>());
            Assert.AreEqual(validCar, result);

            // You might want to add more specific assertions based on your business logic.
            // For example, ensure that the car was added to the repository.
            _mockRepo.Verify(repo => repo.Add(It.IsAny<Car>()), Times.Once);
        }


        [Test]
        [Order(2)]
        public async Task AddCarWithExistingCarIdTest_ThrowsCarAlreadyExistsExceptionTest()
        {
            // Arrange
            var existingCarId = 1;
            var existingCar = new Car { CarId = existingCarId };
            _mockRepo.Setup(repo => repo.Add(existingCar)).ThrowsAsync(new CarAlreadyExistsException());

            // Act & Assert
            Assert.That(async () => await _carService.AddCar(existingCar), Throws.TypeOf<CarAlreadyExistsException>());
        }

        [Test]
        public async Task AddDiscountToCar_ValidIdsTest_ReturnsCarWithDiscount()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;
            var existingDiscount = new Discount { DiscountId = discountId };
            var existingCar = new Car { CarId = carId};

            _mockDiscountRepo.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(existingDiscount);
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(existingCar);
            _mockRepo.Setup(repo => repo.Update(It.IsAny<Car>())).ReturnsAsync(existingCar);

            // Act
            var result = await _carService.AddDiscountToCar(carId, discountId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Discounts, Contains.Item(existingDiscount));

        }

        [Test]
        public async Task AddDiscountToCar_DiscountNotFoundTest_ReturnsNull()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;

            _mockDiscountRepo.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync((Discount)null);
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(new Car());

            // Act
            var result = await _carService.AddDiscountToCar(carId, discountId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddDiscountToCar_CarNotFoundTest_ReturnsNull()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;

            _mockDiscountRepo.Setup(repo => repo.GetAsyncById(discountId)).ReturnsAsync(new Discount());
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync((Car)null);

            // Act
            var result = await _carService.AddDiscountToCar(carId, discountId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddDiscountToCar_ExceptionThrownTest_ReturnsNull()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;

            _mockDiscountRepo.Setup(repo => repo.GetAsyncById(discountId)).ThrowsAsync(new Exception());
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(new Car());

            // Act
            var result = await _carService.AddDiscountToCar(carId, discountId);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task DeleteCarTest_ExistingCarTest_DeletesCar()
        {
            // Arrange
            int carId = 1;

            _mockRepo.Setup(repo => repo.Delete(carId)).ReturnsAsync(new Car { CarId = carId });

            // Act
            var result = await _carService.DeleteCar(carId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(carId, result.CarId);
        }
        [Test]
        public async Task GetCarByIdTest_ExistingCar_ReturnsCar()
        {
            // Arrange
            int existingCarId = 1;
            Car existingCar = new Car { CarId = existingCarId, Make = "Toyota", Model = "Camry", Year = 2022 };

            _mockRepo.Setup(repo => repo.GetAsyncById(existingCarId)).ReturnsAsync(existingCar);

            // Act
            var result = await _carService.GetCarById(existingCarId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingCarId, result.CarId);
            Assert.AreEqual(existingCar.Make, result.Make);
            Assert.AreEqual(existingCar.Model, result.Model);
            Assert.AreEqual(existingCar.Year, result.Year);
        }

        [Test]
        public async Task GetCarsByAvailabilityStatusTest_ReturnsAvailableCars()
        {
            // Arrange
            var allCars = new List<Car>
            {
             new Car { CarId = 1, Make = "Toyota", Model = "Camry", Year = 2020, Availability = true },
             new Car { CarId = 2, Make = "Honda", Model = "Accord", Year = 2019, Availability = false },
             new Car { CarId = 3, Make = "Ford", Model = "Fusion", Year = 2018, Availability = true }

             };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(allCars);

            // Act
            var result = await _carService.GetCarsByAvailabilityStatus();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2)); // Assuming two cars are available
            Assert.That(result, Is.All.Matches<Car>(c => c.Availability == true));// All cars should be available
        }

        [Test]
        public async Task GetCarsListTest_ReturnsListOfCars()
        {
            // Arrange
            var cars = new List<Car>
        {
        // Populate with sample cars for testing
        new Car { CarId = 1, Make = "Toyota", Model = "Camry", Year = 2020, Availability = true },
        new Car { CarId = 2, Make = "Honda", Model = "Civic", Year = 2019, Availability = true },

        };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(cars);

            // Act
            var result = await _carService.GetCarsList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(cars.Count));
            Assert.That(result, Is.EqualTo(cars));
        }
        [Test]
        public async Task MakeReservationTest_ValidInputsTest_ReturnsReservation()
        {
            // Arrange
            int carId = 1;
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = DateTime.Now.AddDays(3);

            var car = new Car
            {
                CarId = carId,
                Availability = true,
                Reservations = new List<Reservation>()
            };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);

            // Act
            var result = await _carService.MakeReservation(carId, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(carId, result.CarId);
            Assert.AreEqual(startDate, result.PickUpDateTime);
            Assert.AreEqual(endDate, result.DropOffDateTime);
        }

        [Test]
        public async Task MakeReservation_CarNotAvailableTest_ThrowsNoSuchCarException()
        {
            // Arrange
            int carId = 1;
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = DateTime.Now.AddDays(3);

            var car = new Car
            {
                CarId = carId,
                Availability = false
            };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NoSuchCarException>(() => _carService.MakeReservation(carId, startDate, endDate));
            Assert.IsNotNull(exception);
        }

        [Test]
        public async Task RemoveDiscountFromCarTest_ValidInputsTest_ReturnsCar()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;

            var car = new Car
            {
                CarId = carId,
                Discounts = new List<Discount>
        {
            new Discount { DiscountId = discountId }
        }
            };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);

            // Act
            var result = await _carService.RemoveDiscountFromCar(carId, discountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(carId, result.CarId);
            Assert.IsFalse(result.Discounts.Any(d => d.DiscountId == discountId));
        }

        [Test]
        public async Task RemoveDiscountFromCar_DiscountNotAssignedTest_ThrowsDiscountNotAssignedToCarException()
        {
            // Arrange
            int carId = 1;
            int discountId = 1;

            var car = new Car
            {
                CarId = carId,
                Discounts = new List<Discount>()
            };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);

            // Act & Assert
            var exception = Assert.ThrowsAsync<DiscountNotAssignedToCarException>(() => _carService.RemoveDiscountFromCar(carId, discountId));
            Assert.IsNotNull(exception); ;
        }
        [Test]
        public async Task UpdateCarDailyRateTest_ExistingCar_ReturnsUpdatedCar()
        {
            // Arrange
            int carId = 1;
            double newDailyRate = 50.00;
            var existingCar = new Car { CarId = carId, DailyRate = 30.00 };
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(existingCar);
            _mockRepo.Setup(repo => repo.Update(existingCar)).ReturnsAsync(existingCar);

            // Act
            var result = await _carService.UpdateCarDailyRate(carId, newDailyRate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newDailyRate, result.DailyRate);
        }

        [Test]
        public async Task UpdateCarSpecificationTest_ExistingCar_ReturnsUpdatedCar()
        {
            // Arrange
            int carId = 1;
            string newSpecification = "Updated specification";
            var existingCar = new Car { CarId = carId, Specification = "Original specification" };
            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(existingCar);
            _mockRepo.Setup(repo => repo.Update(existingCar)).ReturnsAsync(existingCar);

            // Act
            var result = await _carService.UpdateCarSpecification(carId, newSpecification);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newSpecification, result.Specification);
        }
        [Test]
        public async Task ViewAllReservationsTest_ReturnsListOfReservations()
        {
            // Arrange
            var car1 = new Car { CarId = 1, Reservations = new List<Reservation> { new Reservation { ReservationId = 1 } } };
            var car2 = new Car { CarId = 2, Reservations = new List<Reservation> { new Reservation { ReservationId = 2 } } };
            var cars = new List<Car> { car1, car2 };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(cars);

            // Act
            var result = await _carService.ViewAllReservations();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Some.Property("ReservationId").EqualTo(1));
            Assert.That(result, Has.Some.Property("ReservationId").EqualTo(2));
        }

        [Test]
        public async Task ViewCarDetailsTest_ReturnsCarDetails()
        {
            // Arrange
            var carId = 1;
            var expectedCar = new Car { CarId = carId, Make = "Toyota", Model = "Camry", Year = 2022 };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(expectedCar);

            // Act
            var result = await _carService.ViewCarDetails(carId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCar));
        }
        [Test]
        public async Task ViewPastReservationsTest_ReturnsPastReservationsForUser()
        {
            // Arrange
            var userId = 1;
            var currentDateTime = DateTime.Now;
            var pastReservations = new List<Reservation>
         {
        new Reservation { UserId = userId, DropOffDateTime = currentDateTime.AddDays(-1) },
        new Reservation { UserId = userId, DropOffDateTime = currentDateTime.AddDays(-2) },
        // Add more past reservations as needed...
         };

            var allCars = new List<Car>
        {
        new Car { Reservations = pastReservations },
        // Add more cars with reservations as needed...
        };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(allCars);

            // Act
            var result = await _carService.ViewPastReservations(userId);

            // Assert
            Assert.That(result, Is.EqualTo(pastReservations));
        }

        [Test]
        public async Task ViewPaymentsTest_ReturnsPaymentsForUser()
        {
            // Arrange
            var userId = 1;
            var payments = new List<Payment>
         {
        new Payment { UserId = userId },

         };

            var allCars = new List<Car>
         {
        new Car { Payments = payments },

           };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(allCars);

            // Act
            var result = await _carService.ViewPayments(userId);

            // Assert
            Assert.That(result, Is.EqualTo(payments));
        }

        [Test]
        public async Task ViewReservationDetailsTest_ReturnsReservationDetails()
        {
            // Arrange
            var reservationId = 1;
            var reservation = new Reservation { ReservationId = reservationId };
            var allCars = new List<Car>
        {
        new Car { Reservations = new List<Reservation> { reservation } },

        };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(allCars);

            // Act
            var result = await _carService.ViewReservationDetails(reservationId);

            // Assert
            Assert.That(result, Is.EqualTo(reservation));
        }

        [Test]
        public async Task ViewReservationDetailsForAdminTest_ReturnsReservationDetails()
        {
            // Arrange
            var reservationId = 1;
            var reservation = new Reservation { ReservationId = reservationId };
            var allCars = new List<Car>
         {
        new Car { Reservations = new List<Reservation> { reservation } },

          };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(allCars);

            // Act
            var result = await _carService.ViewReservationDetailsForAdmin(reservationId);

            // Assert
            Assert.That(result, Is.EqualTo(reservation));
        }
        [Test]
        public async Task ViewReviewsTest_ReturnsReviewsForCar()
        {
            // Arrange
            var carId = 1;
            var reviews = new List<Review>
           {
        new Review { CarId = carId },

            };
            var car = new Car { CarId = carId, Reviews = reviews };

            _mockRepo.Setup(repo => repo.GetAsyncById(carId)).ReturnsAsync(car);

            // Act
            var result = await _carService.ViewReviews(carId);

            // Assert
            Assert.That(result, Is.EqualTo(reviews));
        }






    }

}
