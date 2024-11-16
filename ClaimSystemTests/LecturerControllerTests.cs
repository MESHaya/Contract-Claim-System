/*-using ClaimSystem.Controllers;
using ClaimSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ClaimSystem.Tests
{
    public class LecturerControllerTests
    {
        private readonly LecturerController _lecturerController;
        private readonly ClaimDbContext _context;

        public LecturerControllerTests()
        {
            var options = new DbContextOptionsBuilder<ClaimDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ClaimDbContext(options);
            _lecturerController = new LecturerController(null, _context); // Adjust parameters as needed
        }
        [Fact]
        public async Task CreateClaimForm_ShouldAddClaimToDatabase()
        {
            // Arrange
            var claim = new Claims
            {
                LecturerName = "Mark",
                HoursWorked = 10,
                HourlyRate = 50,
                Notes = "Worked extra hours",
                Status = "Pending"
            };

            // Mock the uploaded file
            var mockFile = new Mock<IFormFile>();
            var content = "This is a test file";
            var fileName = "testfile.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(f => f.OpenReadStream()).Returns(ms);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(ms.Length);

            var files = new List<IFormFile> { mockFile.Object };

            // Act
            var result = await _lecturerController.CreateClaimForm(claim, files);

            // Assert
            var addedClaim = _context.Claim.FirstOrDefault(c => c.LecturerName == "Mark");
            Assert.NotNull(addedClaim); // Verify claim was added
            Assert.Equal("Mark", addedClaim.LecturerName);
            Assert.Equal("Pending", addedClaim.Status);
            Assert.Equal(fileName, addedClaim.FileName); // Check if the file name is saved correctly
            Assert.Equal(1, _context.Claim.Count()); // Ensure only one claim is added

            // Assert redirect action
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("LecturerDash", redirectResult.ActionName); // Ensure redirect happens to LecturerDash
        }



        [Fact]
        public void Claim_ReturnsAllClaims()
        {
            // Arrange
            var claims = new List<Claims>
            {
                new Claims { Id = 1, LecturerName= "Amy", Status = "Pending" },
                new Claims { Id = 2, LecturerName = "John", Status = "Approved" }
            };

            _context.Claim.AddRange(claims);
            _context.SaveChanges();

            // Act
            var result = _lecturerController.Claim();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Claims>>(viewResult.Model);
            Assert.Equal(2, model.Count); // Verify we have two claims
        }



    }
}

*/