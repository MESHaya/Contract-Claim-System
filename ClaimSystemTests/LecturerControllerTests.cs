using ClaimSystem.Controllers;
using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public void Claim_ReturnsAllClaims()
        {
            // Arrange
            var claims = new List<Claims>
            {
                new Claims { Id = 1, ClaimName = "Transport", Status = "Pending" },
                new Claims { Id = 2, ClaimName = "Food", Status = "Approved" }
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

