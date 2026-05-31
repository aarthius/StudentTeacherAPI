using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using StudentTeacherAPI.Controllers;
using StudentTeacherAPI.DAL;
using StudentTeacherAPI.Models;
using Xunit;

namespace StudentTeacherAPI.Tests
{
    public class RecordsControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;

        public RecordsControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"])
                .Returns("Server=PSILENL124;Database=StudentTeacherDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        // Test 1: GetAll returns OkResult
        [Fact]
        public void GetAll_ReturnsOkResult()
        {
            var controller = new RecordsController(_mockConfig.Object);
            var result = controller.GetAll();
            Assert.IsType<OkObjectResult>(result);
        }

        // Test 2: GetById with invalid ID returns NotFound
        [Fact]
        public void GetById_InvalidId_ReturnsNotFound()
        {
            var controller = new RecordsController(_mockConfig.Object);
            var result = controller.GetById(-1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test 3: GetById with valid ID returns OkResult or NotFound
        [Fact]
        public void GetById_ValidId_ReturnsResult()
        {
            var controller = new RecordsController(_mockConfig.Object);
            var result = controller.GetById(1);
            Assert.True(result is OkObjectResult || result is NotFoundObjectResult);
        }

        // Test 4: Create with valid record returns OkResult
        [Fact]
        public void Create_ValidRecord_ReturnsOkResult()
        {
            var controller = new RecordsController(_mockConfig.Object);
            var record = new StudentTeacherAPI.Models.Record
            {
                Title = "Test Record",
                Description = "Test Description",
                CreatedBy = 1
            };
            var result = controller.Create(record);
            Assert.IsType<OkObjectResult>(result);
        }

        // Test 5: Delete with invalid ID returns NotFound
        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var controller = new RecordsController(_mockConfig.Object);
            var result = controller.Delete(-1);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}