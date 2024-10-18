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
    public class ManagerControllerTests
    {
        private readonly ManagerController _managerController;
        private readonly ClaimDbContext _context;

        public ManagerControllerTests()
        {
            var options = new DbContextOptionsBuilder<ClaimDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ClaimDbContext(options);
            _managerController = new ManagerController(null, _context); // Adjust parameters as needed
        }

        [Fact]
        public void Academic_Manager_Dash_ReturnsAllPendingClaims()
        {
            // Arrange
            var claims = new List<Claims>
            {
                new Claims { Id = 1, ClaimName = "John Doe", Status = "Pending" },
                new Claims { Id = 2, ClaimName = "Jane Smith", Status = "Approved" },
                new Claims { Id = 3, ClaimName = "Alice Johnson", Status = "Pending" }
            };

            _context.Claim.AddRange(claims);
            _context.SaveChanges();

            // Act
            var result = _managerController.Academic_Manager_Dash();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Claims>>(viewResult.Model);
            Assert.Equal(2, model.Count); // Verify we have two pending claims
        }

        [Fact]
        public async Task ApproveClaim_ValidId_UpdatesClaimStatus()
        {
            // Arrange
            var claim = new Claims { Id = 1, ClaimName = "John Doe", Status = "Pending" };
            _context.Claim.Add(claim);
            _context.SaveChanges();

            // Act
            var result = await Task.FromResult(_managerController.ApproveClaim(1)); // Adjust method name if necessary

            // Assert
            var updatedClaim = _context.Claim.Find(1);
            Assert.NotNull(updatedClaim);
            Assert.Equal("Approved", updatedClaim.Status);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task RejectClaim_ValidId_UpdatesClaimStatus()
        {
            // Arrange
            var claim = new Claims { Id = 2, ClaimName = "Jane Smith", Status = "Pending" };
            _context.Claim.Add(claim);
            _context.SaveChanges();

            // Act
            var result = await Task.FromResult(_managerController.RejectClaim(2)); // Adjust method name if necessary

            // Assert
            var updatedClaim = _context.Claim.Find(2);
            Assert.NotNull(updatedClaim);
            Assert.Equal("Rejected", updatedClaim.Status);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task ApproveClaim_NonExistentId_ReturnsRedirectToAcademicManagerDash()
        {
            // Act
            var result = await Task.FromResult(_managerController.ApproveClaim(99)); // Using a non-existent ID

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task RejectClaim_NonExistentId_ReturnsRedirectToAcademicManagerDash()
        {
            // Act
            var result = await Task.FromResult(_managerController.RejectClaim(99)); // Using a non-existent ID

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        // Add more tests for other actions as needed
    }
}
