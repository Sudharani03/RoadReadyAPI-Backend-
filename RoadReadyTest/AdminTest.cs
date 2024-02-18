using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Repositories;
using RoadReady.Services;
using System.Text;

namespace RoadReadyTest
{

    public class AdminTest
    {
        CarRentalDbContext context;
        private AdminService _adminService;
        private Mock<IRepository<int, Admin>> _mockRepo;
        private Mock<ILogger<AdminService>> _mockLogger;

        /// <summary>
        /// Setup Method in which InMemory dummy database is created
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new CarRentalDbContext(options);

            _mockRepo = new Mock<IRepository<int, Admin>>();
            _mockLogger = new Mock<ILogger<AdminService>>();
            _adminService = new AdminService(_mockRepo.Object, _mockLogger.Object);
        }



        //public async Task AddAdminTest_ReturnsAdmin()
        //{
        //    //Arrange
        //    var validAdmin = new Admin
        //    {
        //        // Initialize with valid admin data here for testing
        //        AdminId = 1,
        //        FirstName = "Sudharani",
        //        LastName = "Suvvari",
        //        Email = "sudharani@gmail.com",
        //        Username = "sudha_rani_suvvari",
        //        Password = "Paswordsudha", // Sample password bytes
        //        PhoneNumber = "1234567890",
        //        Role = "Admin"
        //    };
        //    _mockRepo.Setup(repo => repo.Add(validAdmin)).ReturnsAsync(validAdmin);

        //    // Act
        //    var result = await _adminService.AddAdmin(validAdmin);

        //    // Assert
        //    Assert.That(result, Is.TypeOf<Admin>());
        //    Assert.AreEqual(validAdmin, result);
        //    Assert.Pass();
        //}

        [Test]
        public async Task DeleteAdminTest()
        {
            // Arrange
            const int adminIdToDelete = 1;
            var adminToDelete = new Admin
            {
                AdminId = 1,
                FirstName = "Sudharani",
                LastName = "Suvvari",
                Email = "sudharani@gmail.com",
                Username = "sudha_rani_suvvari",
                Password = new byte[] { 0x01, 0x02, 0x03 }, // Sample password bytes
                PhoneNumber = "1234567890"
               
            };

            _mockRepo.Setup(repo => repo.Delete(adminIdToDelete)).ReturnsAsync(adminToDelete);

            // Act
            var result = await _adminService.DeleteAdmin(adminIdToDelete);

            // Assert
            Assert.That(result, Is.TypeOf<Admin>());
            Assert.That(result, Is.EqualTo(adminToDelete));
        }

        [Test]
        public async Task GetAllAdminsTest()
        {
            // Arrange
            var adminList = new List<Admin>
            {
                new Admin { AdminId = 1, FirstName = "Sudha", LastName = "Rani" },
                new Admin { AdminId = 2, FirstName = "Swathi", LastName = "R" },

            };

            _mockRepo.Setup(repo => repo.GetAsync()).ReturnsAsync(adminList);

            // Act
            var result = await _adminService.GetAllAdmins();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Admin>>(result);
            Assert.AreEqual(adminList, result);
        }


        [Test]
        public async Task GetAdminByIdTest()
        {
            // Arrange
            const int adminIdToRetrieve = 1;
            var adminToRetrieve = new Admin { AdminId = adminIdToRetrieve, FirstName = "Sudha", LastName = "Rani" };

            _mockRepo.Setup(repo => repo.GetAsyncById(adminIdToRetrieve)).ReturnsAsync(adminToRetrieve);

            // Act
            var result = await _adminService.GetAdminById(adminIdToRetrieve);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Admin>(result);
            Assert.AreEqual(adminToRetrieve, result);
        }

        [Test]
        public async Task UpdateAdminEmailTest_ValidInput_ReturnsUpdatedAdmin()
        {
            // Arrange
            int adminId = 1;
            string newEmail = "sudharani@gmail.com";
            var existingAdmin = new Admin { AdminId = adminId, FirstName = "kishu", LastName = "R", Email = "kishu@gmail.com" };

            // Setup the mock repository
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync(existingAdmin);

            // Act
            var result = await _adminService.UpdateAdminEmail(adminId, newEmail);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AdminId, Is.EqualTo(adminId));
            Assert.That(result.Email, Is.EqualTo(newEmail));
        }

        [Test]
        public async Task UpdateAdminEmail_AdminNotFoundTest_ThrowsNoSuchAdminException()
        {
            // Arrange
            int adminId = 1;
            string newEmail = "sudharanisuvvari@gmail.com";

            // Setup the mock repository to return null, simulating admin not found
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync((Admin)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchAdminException>(() => _adminService.UpdateAdminEmail(adminId, newEmail));
        }

        [Test]
        public async Task UpdateAdminEmail_NullOrEmptyEmailTest_ThrowsArgumentException()
        {
            // Arrange
            int adminId = 1;
            string invalidEmail = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _adminService.UpdateAdminEmail(adminId, invalidEmail));
        }

        [Test]
        public async Task UpdateAdminPassword_ValidInput_Success()
        {
            //Arrange
            int adminId = 1;
            byte[] newPassword = Encoding.UTF8.GetBytes("Sudha1234"); // Example new password
            Admin existingAdmin = new Admin { AdminId = adminId, Password = Encoding.UTF8.GetBytes("oldPassword") }; // Existing admin with an old password
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync(existingAdmin); // Setup mock repository to return the existing admin

            // Act
            var result = await _adminService.UpdateAdminPassword(adminId, newPassword);

            // Assert
            Assert.IsNotNull(result); // Ensure result is not null
            Assert.AreEqual(adminId, result.AdminId); // Ensure AdminId is unchanged
            Assert.AreEqual(newPassword, result.Password); // Ensure password is updated correctly
        }

        [Test]
        public async Task UpdateAdminPhoneNumberTest_Valid()
        {
            // Arrange
            int adminId = 1;
            string newPhoneNumber = "1234567890"; // Example new phone number
            var adminToUpdate = new Admin { AdminId = adminId, PhoneNumber = "98765432345" }; // Create a sample admin entity
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync(adminToUpdate); // Mock repository method to return the sample admin entity

            // Act
            var updatedAdmin = await _adminService.UpdateAdminPhoneNumber(adminId, newPhoneNumber);

            // Assert
            Assert.IsNotNull(updatedAdmin);
            Assert.AreEqual(adminId, updatedAdmin.AdminId);
            Assert.AreEqual(newPhoneNumber, updatedAdmin.PhoneNumber);
        }

        [Test]
        public async Task UpdateAdminPhoneNumber_NullPhoneNumberTest_ThrowsArgumentException()
        {
            // Arrange
            int adminId = 1;
            string nullPhoneNumber = null; // Null phone number
            var adminToUpdate = new Admin { AdminId = adminId, PhoneNumber = "9876543455" }; // Sample admin entity
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync(adminToUpdate); // Mock repository method

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _adminService.UpdateAdminPhoneNumber(adminId, nullPhoneNumber);
            });
        }

        [Test]
        public async Task UpdateAdminPhoneNumber_EmptyPhoneNumberTest_ThrowsArgumentException()
        {
            // Arrange
            int adminId = 1;
            string emptyPhoneNumber = ""; // Empty phone number
            var adminToUpdate = new Admin { AdminId = adminId, PhoneNumber = "987654567" }; // Sample admin entity
            _mockRepo.Setup(repo => repo.GetAsyncById(adminId)).ReturnsAsync(adminToUpdate); // Mock repository method

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _adminService.UpdateAdminPhoneNumber(adminId, emptyPhoneNumber);
            });
        }

        [Test]
        public async Task UpdateAdminUserName_InvalidUserNameTest_ThrowsArgumentException()
        {
            // Arrange
            int adminId = 1;
            string invalidUserName = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _adminService.UpdateAdminUserName(adminId, invalidUserName);
            });
        }

        [Test]
        public async Task UpdateAdminUserName_AdminNotFoundTest_ThrowsNoSuchAdminException()
        {
            // Arrange
            int nonExistingAdminId = 999;
            string validUserName = "swat2723";

            // Act & Assert
            Assert.ThrowsAsync<NoSuchAdminException>(async () =>
            {
                await _adminService.UpdateAdminUserName(nonExistingAdminId, validUserName);
            });
        }

    }
}