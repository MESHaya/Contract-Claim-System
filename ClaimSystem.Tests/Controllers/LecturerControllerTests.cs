using ClaimSystem.Controllers;
using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class LecturerControllerTests
{
    private LecturerController _controller;
    private ClaimDbContext _context;
    private ILogger<LecturerController> _mockLogger;

    public LecturerControllerTests()
    {
        // Setup the in-memory database
        var options = new DbContextOptionsBuilder<ClaimDbContext>()
            .UseInMemoryDatabase(databaseName: "TestClaimsDb")
            .Options;
        _context = new ClaimDbContext(options);

        // Setup the mock logger
        var mockLogger = new Mock<ILogger<LecturerController>>();
        _mockLogger = mockLogger.Object;

        // Initialize the LecturerController with in-memory context and mock logger
        _controller = new LecturerController(_mockLogger, _context);

        // Seed some data if needed
        SeedData();
    }

    private void SeedData()
    {
        // Add any necessary data for the tests
        _context.Claim.Add(new Claims
        {
            Id = 1,
            ClaimName = "Food",
            HourlyRate = 25,
            HoursWorked = 200,
            Status = "Pending",
            Notes = "Test claim"
        });
        _context.SaveChanges();
    }

    [Fact]
    public void CreateClaimForm_Returns_RedirectToAction()
    {
        // Arrange
        var newClaim = new Claims
        {
            Id = 2,
            ClaimName = "Transport",
            HourlyRate = 30,
            HoursWorked = 250,
            Status = "Pending",
            Notes = "New test claim"
        };

        // Act
        var result = _controller.CreateClaimForm(newClaim);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    
}
